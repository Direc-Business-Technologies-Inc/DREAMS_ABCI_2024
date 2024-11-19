using System.Text;

namespace SapHanaLayer
{
    public interface iDocuments
    {
        StringBuilder GET(iCompany iCompany, string module);
        bool PATCH(iCompany iCompany, string module, StringBuilder model);
        bool POST(iCompany iCompany, string module, StringBuilder model);
    }
}