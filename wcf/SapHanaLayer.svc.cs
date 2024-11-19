using ABROWN_DREAMS.Models;
using DirecLayer;
using MSXML2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Script.Serialization;

namespace ABROWN_DREAMS.wcf
{
	public class LoginModel
	{
		public string CompanyDB { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
	}

	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SapHanaLayer" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select SapHanaLayer.svc or SapHanaLayer.svc.cs at the Solution Explorer and start debugging.
	public class SapHanaLayer : ISapHanaLayer
	{
		public long ResultCode { get; set; }
		public string ResultDescription { get; set; }
		XMLHTTP60 ServiceLayer { get; set; }

		#region RestfulAPI
		public string GetService(string url)
		{
			string _output = "";
			try
			{
				ServiceLayer.open("GET", url);
				ServiceLayer.send();
				_output = ServiceLayer.responseText;
			}
			catch (Exception ex)
			{ _output = $"Error : (ServiceLayer) {ex.Message}"; }

			return _output;
		}


		public string PostService(string url, string json)
		{
			string _output = "";
			try
			{
				ServiceLayer.open("POST", url);
				ServiceLayer.send(json);
				_output = ServiceLayer.responseText;
			}
			catch (Exception ex)
			{ _output = $"Error : (ServiceLayer) {ex.Message}"; }
			return _output;
		}

		public string PatchService(string url, string json)
		{
			string _output = "";
			try
			{
				ServiceLayer.open("PATCH", url);
				ServiceLayer.send(json);
				_output = ServiceLayer.responseText;
			}
			catch (Exception ex)
			{ _output = $"Error : (ServiceLayer) {ex.Message}"; }

			return _output;
		}

		public string PutService(string url, string json)
		{
			string _output = "";
			try
			{
				ServiceLayer.open("PATCH", url);
				ServiceLayer.setRequestHeader("B1S-ReplaceCollectionsOnPatch", "true");
				ServiceLayer.send(json);
				_output = ServiceLayer.responseText;
			}
			catch (Exception ex)
			{ _output = $"Error : (ServiceLayer) {ex.Message}"; }

			return _output;
		}

		public string DeleteService(string url)
		{
			string _output = "";
			try
			{
				ServiceLayer.open("DELETE", url);
				ServiceLayer.send(string.Empty);
				_output = ServiceLayer.responseText;
			}
			catch (Exception ex)
			{ _output = $"Error : (ServiceLayer) {ex.Message}"; }

			return _output;
		}
		#endregion

		public bool Connect()
		{
			bool _output = false;

			try
			{
				iConfigModel config = new ConfigModel();
				IList<LoginModel> model = new List<LoginModel>
				{
					new LoginModel
					{
						CompanyDB = (config.SapDatabase).Trim(),
						UserName = config.SapUserId.Trim(),
						Password = config.SapPassword.Trim()
					}
				};

				ConfigModel configModel = new ConfigModel();
				string url = ValidationHelper.UrlValid($"{ApiHelper.ServiceURL(configModel.SapServiceLayerServer)}Login");

				var json = new JavaScriptSerializer().Serialize(model).ToString();

				if (json.Length > 2)
				{
					json = json.Substring(1, json.Length - 2);
				}

				ServiceLayer = new XMLHTTP60();
				string result = PostService(url, json);

				ResultDescription = DataHelper.GetJsonValue(result, "SessionId");
				_output = IsLoginSuccess(ResultDescription);

				if (_output)
				{
					ResultCode = DataHelper.GetJsonErrorCode(result);
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Error : (Login) {ex.Message}");
			}

			return _output;
		}

		bool IsLoginSuccess(string sResponse)
		{
			var output = false;
			try
			{
				if (string.IsNullOrEmpty(sResponse) || sResponse.Contains("error"))
				{ output = false; }
				else
				{ output = sResponse.Contains("-"); }
			}
			catch (Exception ex)
			{ throw new Exception($"Error : (Login) IsLoginSuccess {ex.Message}"); }

			return output;
		}

		bool IsSuccessful(string sResponse, string module, bool isPatch = false)
		{
			var output = false;
			try
			{
				if (string.IsNullOrEmpty(sResponse) && isPatch)
				{ output = true; }
				else
				{ output = !sResponse.Contains("error"); }
			}
			catch (Exception ex)
			{ throw new Exception($"Error : ({module}) IsSuccessful {ex.Message}"); }

			return output;
		}
		public StringBuilder GET(string module)
		{
			StringBuilder result = new StringBuilder();
			try
			{
				if (Connect())
				{
					ConfigModel configModel = new ConfigModel();
					string url = ValidationHelper.UrlValid($"{ApiHelper.ServiceURL(configModel.SapServiceLayerServer)}{module}");

					return new StringBuilder(GetService(url));
				}
			}
			catch (Exception ex)
			{
				throw;
			}
			return result;
		}

		public bool POST(string module, StringBuilder model)
		{
			bool result = true;
			try
			{
				if (Connect())
				{
					ConfigModel configModel = new ConfigModel();
					string url = ValidationHelper.UrlValid($"{ApiHelper.ServiceURL(configModel.SapServiceLayerServer)}{module}");

					var xml = PostService(url, model.ToString());
					if (module == "BusinessPartners")
					{
						ResultDescription = DataHelper.GetJsonValue(xml, "CardCode");
					}
					else if (module == "JournalEntries")
					{
						ResultDescription = DataHelper.GetJsonValue(xml, "JdtNum");
					}
					else
					{
						ResultDescription = DataHelper.GetJsonValue(xml, "DocEntry");
					}

					result = IsSuccessful(ResultDescription, module);

					if (!result)
					{
						ResultCode = DataHelper.GetJsonErrorCode(xml);
					}
				}
			}
			catch (Exception ex)
			{
				result = false;
				throw;
			}
			return result;
		}


		public bool PATCH(string module, StringBuilder model)
		{
			bool result = true;
			try
			{
				if (Connect())
				{
					ConfigModel configModel = new ConfigModel();
					string url = ValidationHelper.UrlValid($"{ApiHelper.ServiceURL(configModel.SapServiceLayerServer)}{module}");

					var xml = PatchService(url, model.ToString());

					ResultDescription = DataHelper.GetJsonValue(xml, "Code");

					result = IsSuccessful(ResultDescription, module, true);

					if (!result)
					{
						ResultCode = DataHelper.GetJsonErrorCode(xml);
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
			return result;
		}

		public bool PATCH(string module, string model, out string errorDesc)
		{
			bool result = true;
			errorDesc = "";

			try
			{
				if (Connect())
				{
					ConfigModel configModel = new ConfigModel();
					string url = ValidationHelper.UrlValid($"{ApiHelper.ServiceURL(configModel.SapServiceLayerServer)}{module}");

					var xml = PatchService(url, model);

					ResultDescription = DataHelper.GetJsonValue(xml, "Code");
					errorDesc = ResultDescription;

					result = IsSuccessful(ResultDescription, module, true);

					if (!result)
					{
						ResultCode = DataHelper.GetJsonErrorCode(xml);
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}

			return result;
		}

		public bool PUT(string module, StringBuilder model)
		{
			bool result = true;
			try
			{
				if (Connect())
				{
					ConfigModel configModel = new ConfigModel();
					string url = ValidationHelper.UrlValid($"{ApiHelper.ServiceURL(configModel.SapServiceLayerServer)}{module}");

					var xml = PutService(url, model.ToString());

					ResultDescription = DataHelper.GetJsonValue(xml, "Code");

					result = IsSuccessful(ResultDescription, module, true);

					if (!result)
					{
						ResultCode = DataHelper.GetJsonErrorCode(xml);
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
			return result;
		}

		public bool DELETE(string module)
		{
			bool result = true;
			try
			{
				if (Connect())
				{
					ConfigModel configModel = new ConfigModel();
					string url = ValidationHelper.UrlValid($"{ApiHelper.ServiceURL(configModel.SapServiceLayerServer)}{module}");

					var xml = DeleteService(url);

					ResultDescription = DataHelper.GetJsonValue(xml, "Code");

					result = IsSuccessful(ResultDescription, module); ;

					if (!result)
					{
						ResultCode = DataHelper.GetJsonErrorCode(xml);
					}
				}

			}
			catch (Exception ex)
			{

				throw;
			}
			return result;
		}
	}
}
