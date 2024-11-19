using MSXML2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DirecLayer
{
    public class ApiHelper
    {
        #region WebRequest
        public static HttpWebRequest CreateWebRequest(string url, string sMethod)
        {
            HttpWebRequest webRequest = null;
            try
            {
                webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
                webRequest.ContentType = "application/json";
                webRequest.Accept = "application/json";
                webRequest.Method = sMethod;

                if (url.StartsWith("https://"))
                {
                    webRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                }
            }
            catch (Exception ex)
            { throw new Exception($"Error : Direc Layer API returns - {ex.Message}"); }

            return webRequest;
        }

        public static FtpWebRequest FTPWebRequest(string url, string method, string userId, string password)
        {
            FtpWebRequest webRequest = null;
            try
            {
                webRequest = (FtpWebRequest)WebRequest.Create(new Uri(url));
                webRequest.UseBinary = true;
                webRequest.KeepAlive = false;
                webRequest.Method = method;

                if (!ValidationHelper.isNull(userId, password))
                {
                    webRequest.Credentials = new NetworkCredential(userId, password);
                }
            }
            catch (Exception ex)
            { throw new Exception($"Error : Direc Layer API returns - {ex.Message}"); }

            return webRequest;
        }

        public static FtpWebRequest FTPNoProxyWebRequest(string url, string userId, string password)
        {
            FtpWebRequest webRequest = null;
            try
            {
                webRequest = (FtpWebRequest)WebRequest.Create(new Uri(url));
                webRequest.UseBinary = true;
                webRequest.Method = "NLST";
                webRequest.Proxy = null;
                webRequest.KeepAlive = false;
                webRequest.UsePassive = false;

                if (!ValidationHelper.isNull(userId, password))
                {
                    webRequest.Credentials = new NetworkCredential(userId, password);
                }
            }
            catch (Exception ex)
            { throw new Exception($"Error : Direc Layer API returns - {ex.Message}"); }

            return webRequest;
        }

        public static FtpWebRequest FTPDeleteWebRequest(string url, string userId, string password)
        {
            FtpWebRequest webRequest = null;
            try
            {
                webRequest = (FtpWebRequest)WebRequest.Create(new Uri(url));
                webRequest.UseBinary = true;
                webRequest.Method = "RETR";
                webRequest.KeepAlive = false;

                if (!ValidationHelper.isNull(userId, password))
                {
                    webRequest.Credentials = new NetworkCredential(userId, password);
                }
            }
            catch (Exception ex)
            { throw new Exception($"Error : Direc Layer API returns - {ex.Message}"); }

            return webRequest;
        }

        public static void SendWebRequest(HttpWebRequest request, StringBuilder json)
        {
            try
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            catch (Exception ex)
            { throw new Exception($"Error : Direc Layer API returns - {ex.Message}"); }
        }

        public static string ResponseWebRequest(HttpWebRequest request)
        {
            var output = "";
            try
            {
                var httpResponse = (HttpWebResponse)request.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    output = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            { throw new Exception($"Error : Direc Layer API returns - {ex.Message}"); }
            return output;
        }
        #endregion

        #region HttpClient
        public static async Task<object> GetHttpClient(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    if (response != null)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<object>(jsonString);
                    }
                }

                return null;
            }
            catch (HttpRequestException ex)
            {

                return ex;
            }

        }

        public static async Task<object> GetHttpClient(string url, string userName, string password)
        {
            try
            {
                using (var handler = new HttpClientHandler())
                {
                    using (var client = new HttpClient(handler))
                    {
                        client.BaseAddress = new Uri(url);
                        client.Timeout = new TimeSpan(0, 2, 0);


                        var plainTextBytes = Encoding.UTF8.GetBytes($"{userName}:{password}");
                        var val = Convert.ToBase64String(plainTextBytes);
                        client.DefaultRequestHeaders.Add("Authorization", "Basic " + val);
                        client.DefaultRequestHeaders.Add("ContentType", "application/json");

                        var response = await client.GetAsync(url);
                        var content = string.Empty;

                        if (response != null)
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();
                            return JsonConvert.DeserializeObject<object>(jsonString);
                        }
                    }
                }
                return null;
            }
            catch (HttpRequestException ex)
            {
                return ex;
            }

        }

        public static async Task<object> PostHttpClient(string url, string jsonObject)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);
                    if (response != null)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<object>(jsonString);
                    }
                }

                return null;
            }
            catch (HttpRequestException ex)
            {

                return ex;
            }

        }

        public static async Task<object> TestCall(string url, string userName, string password)
        {
            try
            {
                HttpMessageHandler handler = new HttpClientHandler()
                {
                };

                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(url),
                    Timeout = new TimeSpan(0, 2, 0)
                };

                httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");

                var plainTextBytes = Encoding.UTF8.GetBytes($"{userName}:{password}");
                string val = Convert.ToBase64String(plainTextBytes);
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + val);

                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                string content = string.Empty;

                using (StreamReader stream = new StreamReader(response.Content.ReadAsStreamAsync().Result))
                {
                    content = await stream.ReadToEndAsync();
                }

                return content;
            }
            catch (Exception ex)
            {

                return ex;
            }

        }
        #endregion

        #region XMLHTTP
        public static XMLHTTP60 ServiceLayer { get; set; }
        public static string ServiceURL(string url)
        {
            var output = string.Empty;
            try
            {
                const string httpStr = "http://";
                const string httpsStr = "https://";
                Random r = new Random();
                //int rInt = r.Next(1, 4);
                int rInt = 1;

                if (!url.StartsWith(httpStr, true, null) &&
                        !url.StartsWith(httpsStr, true, null))
                {
                    output = $"{httpStr}{url}:50001{(!url.EndsWith("/") ? "/" : "")}b1s/v1/";
                    //output = $"{httpStr}{url}:50000{(!url.EndsWith("/") ? "/" : "")}b1s/v1/";
                }
                else
                {
                    output = $"{url}:50001{(!url.EndsWith("/") ? "/" : "")}b1s/v1/";
                    //output = $"{url}:50000{(!url.EndsWith("/") ? "/" : "")}b1s/v1/";
                }
            }
            catch (Exception ex)
            { throw new Exception($"Error : Direc Layer API returns - {ex.Message}"); }
            return output;
        }

        public static void InitializeServiceInstance()
        {
            if (ServiceLayer == null)
            { ServiceLayer = new XMLHTTP60(); }
        }

        public static string PatchService(string url, string json)
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

        public static string PutService(string url, string json)
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

        public static string GetService(string url)
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

        public static string PostService(string url, string json)
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

        public static string DeleteService(string url)
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
