using DirecLayer;

namespace ABROWN_DREAMS.Models
{
    public class ConfigModel : iConfigModel
    {
        public string SapServiceLayerServer { get; private set; }
        public string SapDatabaseServer { get; private set; }
        public string SapDatabaseUserID { get; private set; }
        public string SapDatabasePassword { get; private set; }
        public string SapDatabase { get; private set; }
        public string SapUserId { get; private set; }
        public string SapPassword { get; private set; }

        public ConfigModel()
        {
            SapServiceLayerServer = App.AppSettings("SapServiceLayerServer");
            SapDatabaseServer = App.GetConnectionDetails("SAPHana", "SERVERNODE="); 
            SapDatabaseUserID = App.GetConnectionDetails("SAPHana", "UID=");
            SapDatabasePassword = App.GetConnectionDetails("SAPHana", "PWD=");
            SapDatabase = App.GetConnectionDetails("SAPHana", "CS=");
            SapUserId = App.AppSettings("SapUserId");
            SapPassword = App.AppSettings("SapPassword");
        }
    }
}
