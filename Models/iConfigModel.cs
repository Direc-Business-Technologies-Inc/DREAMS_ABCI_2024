namespace ABROWN_DREAMS.Models
{
    public interface iConfigModel
    {
        string SapServiceLayerServer { get; }
        string SapDatabaseServer { get; }
        string SapDatabasePassword { get; }
        string SapDatabaseUserID { get; }
        string SapDatabase { get; }
        string SapUserId { get; }
        string SapPassword { get; }
    }
}