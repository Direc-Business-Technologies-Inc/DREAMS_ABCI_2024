using System;
using System.Configuration;
using System.Xml;

namespace DirecLayer
{
    public class AppConfig
    {
        public string AppSettings(string value)
        {
            var output = ConfigurationManager.AppSettings[value] != null ? ConfigurationManager.AppSettings[value].ToString() : "";
            return output;
        }

        public string[] AppList(string sAppName)
        {
            string[] output = ConfigurationManager.AppSettings[sAppName] != null ? ConfigurationManager.AppSettings[sAppName].Split(',') : null;
            return output;
        }

        public string GetConnection(string sAppName)
        {
            var output = ConfigurationManager.ConnectionStrings[sAppName] != null ? ConfigurationManager.ConnectionStrings[sAppName].ToString() : "";
            return output;
        }

        public void UpdateConnectionString(string sConectionName, string sConnectionString)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionStringsSection.ConnectionStrings[sConectionName].ConnectionString = sConnectionString;
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        public bool UpdateConfig(string sElemet, string sKey, string sValue)
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
    }
}
