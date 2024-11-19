using MSXML2;
using System.Threading.Tasks;

namespace SapHanaLayer
{
    public interface iLogin
    {
        string CompanyDB { get; set; }
        string Password { get; set; }
        long ResultCode { get; set; }
        string ResultDescription { get; set; }
        string UserName { get; set; }
        bool Connect();
        XMLHTTP60 ServiceLayer { get; }
    }
}