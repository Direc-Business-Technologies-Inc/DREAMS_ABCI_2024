using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace DirecLayer
{
    public class DataHelper
    {
        public static StringBuilder JsonBuilder(IDictionary<string, object> header, StringBuilder lines)
        {
            char q = '"';

            StringBuilder json = new StringBuilder();

            json.Append("{");

            foreach (var arr in header)
            {
                if (arr.Value != null || !string.IsNullOrEmpty(arr.Value.ToString()))
                {
                    json.Append($"{q}{arr.Key}{q}:{q}{arr.Value}{q},");
                }
            }
            if (lines.Length > 0)
            {
                json.Append(lines);
            }
            else
            {
                json.Length--;
            }


            json.Append("}");

            return json;
        }

        public static StringBuilder JsonLinesBuilder(string lineName, IList<IDictionary<string, object>> lines)
        {
            StringBuilder json = new StringBuilder();
            if (lines.Count > 0)
            {
                char q = '"';

                json.Append($"{q}{lineName}{q}: [");

                foreach (var line in lines)
                {
                    var jsn = new JavaScriptSerializer().Serialize(line).ToString();

                    json.Append(jsn);
                    json.Append(",");
                }
                json.Length--;
                json.Append("]");
            }
            return json;
        }

        public static StringBuilder JsonLinesBuilder(string lineName, StringBuilder lines)
        {
            StringBuilder json = new StringBuilder();
            if (lines.Length > 0)
            {
                char q = '"';

                json.Append($"{q}{lineName}{q}: [");
                json.Append(lines.ToString());
                json.Append("]}");
            }
            return json;
        }

        public static StringBuilder JsonLinesCombine(params StringBuilder[] lines)
        {
            char q = '"';

            StringBuilder json = new StringBuilder();

            foreach (var line in lines)
            {
                json.Append(line);
                if (line.Length > 0)
                {
                    json.Append(",");
                }
            }

            return json;
        }


        public static string GetJsonValue(string json, string value)
        {
            try
            {
                if (json != null)
                {
                    JObject err = JObject.Parse(json);
                    if (err.ToString().Contains("error"))
                    {
                        return $"error : {GetJsonErrorValue(err.ToString())}";
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
            catch (Exception ex)
            {
                if (json.Contains("error"))
                {
                    string retJson = GetJsonString(json, "");
                    var sbJson = new StringBuilder();
                    sbJson.Append("{" + retJson + "}}}");
                    return GetJsonErrorValue(sbJson.ToString());
                }
                else { return json; }
            }
        }

        public static JToken GetJsonTokenValue(string json, string token)
        {
            try
            {
                if (json != null)
                {
                    JToken jToken = JToken.Parse(json);
                    return jToken.Children<JProperty>().FirstOrDefault(x => x.Name == token).Value;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static string GetJsonErrorValue(string json)
        {
            JObject err = JObject.Parse(json);
            return (string)err["error"]["message"]["value"];
        }

        public static long GetJsonErrorCode(string json)
        {
            try
            {
                if (json != null)
                {
                    JObject err = JObject.Parse(json);
                    if (err.ToString().Contains("error"))
                    {
                        return (long)err["error"]["code"];
                    }
                    return 0;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                //if (json.Contains("error"))
                //{
                //    string retJson = GetJsonString(json, "");
                //    var sbJson = new StringBuilder();
                //    sbJson.Append("{" + retJson + "}}}");
                //    return GetJsonErrorValue(sbJson.ToString());
                //}
                //else { return json; }
                return 0;
            }
        }

        public static string GetJsonString(string ret, string tag)
        {
            var startTag = "{";
            int startIndex = ret.IndexOf(startTag) + startTag.Length;
            int endIndex = ret.IndexOf("}", startIndex);
            return ret.Substring(startIndex, endIndex - startIndex);
        }

        public static string ReadFile(string sFilePath, string sFileName)
        {
            string FilePath = $"{sFilePath}\\{sFileName}";
            if (File.Exists(FilePath) == true)
            {
                using (StreamReader objReader = new StreamReader(FilePath))
                { return objReader.ReadToEnd(); }
            }
            else { return $"No such file {sFileName}"; }
        }

        #region DataTable
        public static bool DataTableExist(DataTable dt)
        {
            var output = false;
            try
            {
                output = dt != null ? (dt.Rows.Count > 0 ? true : false) : false;
            }
            catch (Exception ex)
            { throw new Exception($"Error : {ex.Message}"); }

            return output;
        }
        #endregion

        #region DataRow
        public static string ReadDataRow(DataRow drDataRow,
                                        string sColumnName,
                                        string oReplace)
        {
            var output = "";

            try
            {
                output = drDataRow != null ? drDataRow[sColumnName].ToString() : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error : ReadDataRow {ex.Message}"); }

            return output;
        }

        public static string ReadDataRow(DataTable dtData,
                                string sColumnName,
                                string oReplace,
                                int iRowCount)
        {
            var output = "";

            try
            {
                output = dtData != null ? (DataTableExist(dtData) ? dtData.Rows[iRowCount][sColumnName].ToString() : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error : ReadDataRow {ex.Message}"); }

            return output;
        }

        public static string ReadDataRow(DataTable dtData,
                                int sColumnName,
                                string oReplace,
                                int iRowCount)
        {
            var output = "";

            try
            {
                output = dtData != null ? (dtData.Rows.Count > 0 ? dtData.Rows[iRowCount][sColumnName].ToString() : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error : ReadDataRow {ex.Message}"); }

            return output;
        }

        public static int ReadDataRow(DataRow drDataRow,
                                        string sColumnName,
                                        int oReplace)
        {
            var output = 0;

            try
            {
                output = drDataRow != null ? (int.TryParse(drDataRow[sColumnName].ToString(), out int ret) ? ret : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error : ReadDataRow {ex.Message}"); }

            return output;
        }
        public static int ReadDataRow(DataTable dtData,
                                string sColumnName,
                                int oReplace,
                                int iRowCount)
        {
            var output = 0;

            try
            {
                output = dtData != null ? (int.TryParse((dtData.Rows[iRowCount][sColumnName] != DBNull.Value ? dtData.Rows[iRowCount][sColumnName].ToString() : 0.ToString()), out int ret) ? ret : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error :  ReadDataRow{ex.Message}"); }

            return output;
        }

        public static int ReadDataRow(DataTable dtData,
                                int sColumnName,
                                int oReplace,
                                int iRowCount)
        {
            var output = 0;

            try
            {
                output = dtData != null ? (int.TryParse((dtData.Rows[iRowCount][sColumnName] != DBNull.Value ? dtData.Rows[iRowCount][sColumnName].ToString() : 0.ToString()), out int ret) ? ret : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error :  ReadDataRow{ex.Message}"); }

            return output;
        }
        public static double ReadDataRow(DataRow drDataRow,
                                        string sColumnName,
                                        double oReplace)
        {
            var output = 0.0;

            try
            {
                output = drDataRow != null ? (double.TryParse(drDataRow[sColumnName].ToString(), out double ret) ? ret : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error : ReadDataRow {ex.Message}"); }

            return output;
        }
        public static double ReadDataRow(DataTable dtData,
                                string sColumnName,
                                double oReplace,
                                int iRowCount)
        {
            var output = 0.0;

            try
            {
                output = dtData != null ? (double.TryParse((dtData.Rows[iRowCount][sColumnName] != DBNull.Value ? dtData.Rows[iRowCount][sColumnName].ToString() : 0.ToString()), out double ret) ? ret : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error : ReadDataRow {ex.Message}"); }

            return output;
        }

        public static double ReadDataRow(DataTable dtData,
                                int sColumnName,
                                double oReplace,
                                int iRowCount)
        {
            var output = 0.0;

            try
            {
                output = dtData != null ? (double.TryParse((dtData.Rows[iRowCount][sColumnName] != DBNull.Value ? dtData.Rows[iRowCount][sColumnName].ToString() : 0.ToString()), out double ret) ? ret : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error : ReadDataRow {ex.Message}"); }

            return output;
        }

        public static DateTime ReadDataRow(DataRow drDataRow,
                                        string sColumnName,
                                        DateTime oReplace)
        {
            var output = DateTime.Now;

            try
            {
                output = drDataRow != null ? (DateTime.TryParse(drDataRow[sColumnName].ToString(), out DateTime ret) ? ret : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error : ReadDataRow {ex.Message}"); }

            return output;
        }

        public static DateTime ReadDataRow(DataTable dtData,
                                string sColumnName,
                                DateTime oReplace,
                                int iRowCount)
        {
            var output = DateTime.Now;

            try
            {
                output = dtData != null && dtData.Rows.Count > 0 ?
                    (DateTime.TryParse((dtData.Rows[iRowCount][sColumnName] != DBNull.Value ? dtData.Rows[iRowCount][sColumnName].ToString() :
                    DateTime.Now.ToString()), out DateTime ret) ? ret : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error : ReadDataRow {ex.Message}"); }

            return output;
        }

        public static DateTime ReadDataRow(DataTable dtData,
                                int sColumnName,
                                DateTime oReplace,
                                int iRowCount)
        {
            var output = DateTime.Now;

            try
            {
                output = dtData != null ? (DateTime.TryParse((dtData.Rows[iRowCount][sColumnName] != DBNull.Value ? dtData.Rows[iRowCount][sColumnName].ToString() : DateTime.Now.ToString()), out DateTime ret) ? ret : oReplace) : oReplace;
            }
            catch (Exception ex)
            { throw new Exception($"Error : ReadDataRow {ex.Message}"); }

            return output;
        }

        public static string DataTableRet(DataTable dt, int row, string ColName, string newvalue)
        {
            string value;
            try
            {
                value = dt.Rows[row][ColName].ToString();
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    value = newvalue;
                }
            }
            catch
            {
                value = newvalue;
            }

            return value.ToString();
        }
        #endregion
    }
}
