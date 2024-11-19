using Newtonsoft.Json.Linq;
using System.Text;

namespace MDC_REALITY
{
    public class JsonHelper
    {

        public static string GetJsonValue(string json, string value)
        {
            try
            {
                if (json != null)
                {
                    JObject err = JObject.Parse(json);
                    if (err.ToString().Contains("error"))
                    {
                        return $"error : {GetJsonError(err.ToString())}";
                    }
                    else
                    {
                        return (string)err[value];
                    }
                }
                else
                {
                    return "";
                }

            }
            catch
            {
                if (json.Contains("error"))
                {
                    string retJson = GetJsonString(json, "");
                    var sbJson = new StringBuilder();
                    sbJson.Append("{" + retJson + "}}}");
                    return GetJsonError(sbJson.ToString());
                }
                else { return "Operation completed successfully"; }
            }

        }

        static string GetJsonError(string json)
        {
            JObject err = JObject.Parse(json);
            return (string)err["error"]["message"]["value"];
        }

        static string GetJsonString(string ret, string tag)
        {
            var startTag = "{";
            int startIndex = ret.IndexOf(startTag) + startTag.Length;
            int endIndex = ret.IndexOf("}", startIndex);
            return ret.Substring(startIndex, endIndex - startIndex);
        }
    }
}
