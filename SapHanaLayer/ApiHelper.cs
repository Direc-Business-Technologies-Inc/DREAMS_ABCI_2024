using MSXML2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SapHanaLayer
{
    public class ApiHelper
    {
        #region XMLHTTP
        public string ServiceURL(string url)
        {
            var output = string.Empty;
            try
            {
                const string httpStr = "http://";
                const string httpsStr = "https://";
                Random r = new Random();
                int rInt = r.Next(1, 4);

                if (!url.StartsWith(httpStr, true, null) &&
                        !url.StartsWith(httpsStr, true, null))
                {
                    output = $"{httpStr}{url}:5000{rInt}{(!url.EndsWith("/") ? "/" : "")}b1s/v1/";
                }
                else
                {
                    output = $"{url}:5000{rInt}{(!url.EndsWith("/") ? "/" : "")}b1s/v1/";
                }
            }
            catch (Exception ex)
            { throw new Exception($"Error : Direc Layer API returns - {ex.Message}"); }
            return output;
        }
        public string PatchService(string url, string json, XMLHTTP60 ServiceLayer)
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

        public string PutService(string url, string json, XMLHTTP60 ServiceLayer)
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

        public string GetService(string url, XMLHTTP60 ServiceLayer)
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

        public string PostService(string url, string json, XMLHTTP60 ServiceLayer)
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

        public string DeleteService(string url, XMLHTTP60 ServiceLayer)
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
    }
}
