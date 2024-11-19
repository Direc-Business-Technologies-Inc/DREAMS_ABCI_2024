using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirecLayer
{
    public class ValidationHelper
    {
        #region Validation
        public static bool isNull(params string[] values)
        {
            bool output = false;
            var value = values.Where(x => string.IsNullOrEmpty(x));
            output = value.Any();
            return output;
        }

        public static bool isNotNull(params string[] values)
        {
            bool output = false;
            var value = values.Where(x => !string.IsNullOrEmpty(x));
            output = value.Any();
            return output;
        }
        #endregion

        public static string UrlValid(string url)
        {
            string output = "";

            output = $"{url}";
            const string httpStr = "http://";
            const string httpsStr = "https://";
            if (!output.StartsWith(httpStr, true, null) &&
                !output.StartsWith(httpsStr, true, null))
            {
                output = $"{httpStr}{output}";
            }

            return output;
        }
    }
}
