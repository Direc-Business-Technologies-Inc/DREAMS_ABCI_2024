namespace SapHanaLayer
{
    public interface iCompanyModel
    {
        string CompanyName { get; set; }
        string Database { get; set; }
        string Localization { get; set; }
        string Version { get; set; }
    }
}