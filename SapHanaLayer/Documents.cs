using DirecLayer;
using Newtonsoft.Json.Linq;
using SapHanaLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SapHanaLayer
{
    public class Documents : iDocuments
    {
        public bool PATCH(iCompany iCompany, string module, StringBuilder model)
        {
            bool result = true;
            try
            {
                if (result = iCompany.Connect())
                {
                    ConfigModel configModel = new ConfigModel();
                    ApiHelper apiHelper = new ApiHelper();
                    string url = ValidationHelper.UrlValid($"{apiHelper.ServiceURL(configModel.SapServiceLayerServer)}{module}");

                    var json = model.ToString();
                    if (!string.IsNullOrEmpty(json))
                    {
                        var jObject = JObject.Parse(json);
                    }

                    var xml = apiHelper.PatchService(url, json,iCompany.ServiceLayer);

                    iCompany.ResultDescription = DataHelper.GetJsonValue(xml, "Code");

                    result = IsSuccessful(iCompany.ResultDescription, true);

                    if (!result)
                    {
                        iCompany.ResultCode = DataHelper.GetJsonErrorCode(xml);
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }

        public bool POST(iCompany iCompany, string module, StringBuilder model)
        {
            bool result = true;
            try
            {
                string sam = model.ToString();
                if (result = iCompany.Connect())
                {
                    ConfigModel configModel = new ConfigModel();
                    ApiHelper apiHelper = new ApiHelper();
                    string url = ValidationHelper.UrlValid($"{apiHelper.ServiceURL(configModel.SapServiceLayerServer)}{module}");

                    var json = model.ToString();
                    if (!string.IsNullOrEmpty(json))
                    {
                        var jObject = JObject.Parse(json);
                    }
                    var xml = apiHelper.PostService(url, json, iCompany.ServiceLayer);

                    iCompany.ResultDescription = DataHelper.GetJsonValue(xml, "DocEntry");

                    result = IsSuccessful(iCompany.ResultDescription);

                    if (!result)
                    {
                        iCompany.ResultCode = DataHelper.GetJsonErrorCode(xml);
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }

        public StringBuilder GET(iCompany iCompany, string module)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                if (iCompany.Connect())
                {
                    ConfigModel configModel = new ConfigModel();
                    ApiHelper apiHelper = new ApiHelper();
                    string url = ValidationHelper.UrlValid($"{apiHelper.ServiceURL(configModel.SapServiceLayerServer)}{module}");

                    return new StringBuilder(apiHelper.GetService(url, iCompany.ServiceLayer));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        bool IsSuccessful(string sResponse, bool isPatch = false)
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
            { throw new Exception($"Error : (UserObjects) IsSuccessful {ex.Message}"); }

            return output;
        }
    }
}
