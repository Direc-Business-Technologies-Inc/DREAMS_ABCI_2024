using DirecLayer;
using MSXML2;
using SapHanaLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SapHanaLayer
{
    public class LoginModel
    {
        public string CompanyDB { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class Login : iLogin
    {

        public string CompanyDB { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public long ResultCode { get; set; }
        public string ResultDescription { get; set; }
        public XMLHTTP60 ServiceLayer { get; private set; }

        public bool Connect()
        {
            bool _output = false;

            try
            {
                IList<LoginModel> model = new List<LoginModel>();

                model.Add(new LoginModel
                {
                    CompanyDB = CompanyDB,
                    UserName = UserName,
                    Password = Password
                });

                ConfigModel configModel = new ConfigModel();
                ApiHelper apiHelper = new ApiHelper();
                string url = ValidationHelper.UrlValid($"{apiHelper.ServiceURL(configModel.SapServiceLayerServer)}Login");

                var json = new JavaScriptSerializer().Serialize(model).ToString();

                if (json.Length > 2)
                {
                    json = json.Substring(1, json.Length - 2);
                }

                ServiceLayer = new XMLHTTP60();

                string result = apiHelper.PostService(url, json, ServiceLayer);

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
                if (string.IsNullOrEmpty(sResponse))
                { output = false; }
                else
                { output = sResponse.Contains("-"); }
            }
            catch (Exception ex)
            { throw new Exception($"Error : (Login) IsLoginSuccess {ex.Message}"); }

            return output;
        }
    }
}
