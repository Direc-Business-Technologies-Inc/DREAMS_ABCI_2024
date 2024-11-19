using MSXML2;
using System.Collections.Generic;

namespace SapHanaLayer
{
    public interface iCompany
    {
        string CompanyDB { get; set; }
        string Password { get; set; }
        long ResultCode { get; set; }
        string ResultDescription { get; set; }
        string UserName { get; set; }
        XMLHTTP60 ServiceLayer { get; }
        bool Connect();
        IList<iCompanyModel> Lists();
    }
}