using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DirecLayer
{
    public class ConfigList
    {
        public string Code { get; set; }
        public string Value { get; set; }
    }

    public class App
    {
        public static string AppSettings(string value)
        {
            var output = ConfigurationManager.AppSettings[value] != null ? ConfigurationManager.AppSettings[value].ToString() : "";
            return output;
        }

        public static string[] AppList(string sAppName)
        {
            string[] output = ConfigurationManager.AppSettings[sAppName] != null ? ConfigurationManager.AppSettings[sAppName].Split(',') : null;
            return output;
        }

        public static string GetConnection(string sAppName)
        {
            var output = ConfigurationManager.ConnectionStrings[sAppName] != null ? ConfigurationManager.ConnectionStrings[sAppName].ToString() : "";
            return output;
        }

        public static string GetConnectionDetails(string sAppName,string sAppDetails)
        {
            string output = "";
            if (ConfigurationManager.ConnectionStrings[sAppName] != null)
            {
                foreach (var item in ConfigurationManager.ConnectionStrings[sAppName].ToString().Split(';'))
                {
                    if (item.Contains(sAppDetails))
                    {
                        output = item.Replace(sAppDetails,"");
                        break;
                    }
                }
            }
            return output;
        }

        public static void UpdateConnectionString(string sConectionName, string sConnectionString)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionStringsSection.ConnectionStrings[sConectionName].ConnectionString = sConnectionString;
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        public static bool UpdateConfig(string sElemet, string sKey, string sValue)
        {
            bool result = true;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            foreach (XmlElement element in xmlDoc.DocumentElement)
            {
                if (element.Name.Equals(sElemet))
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {
                        if (node.Attributes[0].Value.Equals(sKey))
                        {
                            node.Attributes[1].Value = sValue;
                            break;
                        }
                    }
                    break;
                }
            }

            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            ConfigurationManager.RefreshSection(sElemet);

            return result;
        }

        public static bool UpdateSettings(IList<ConfigList> lAppSettings)
        {
            var output = false;
            try
            {
                foreach (var appSetting in lAppSettings)
                {
                    UpdateConfig("appSettings", appSetting.Code, appSetting.Value);
                }
                output = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return output;
        }
    }
}
