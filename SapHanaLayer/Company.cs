using DirecLayer;
using MSXML2;
using System;
using System.Collections.Generic;
using System.Data;

namespace SapHanaLayer
{
    public class CompanyModel : iCompanyModel
    {
        public string CompanyName { get; set; }
        public string Database { get; set; }
        public string Localization { get; set; }
        public string Version { get; set; }
    }

    public class Company : iCompany
    {
        public string CompanyDB { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public long ResultCode { get; set; }
        public string ResultDescription { get; set; }
        public XMLHTTP60 ServiceLayer { get; private set; }

        public IList<iCompanyModel> Lists()
        {
            IList<iCompanyModel> _output = new List<iCompanyModel>();
            try
            {
                var dt = new DataTable();

                dt = Hana.Get(App.GetConnection("SAPHana"),@"SELECT ""cmpName"",""dbName"",""LOC"",""versStr"" FROM SBOCOMMON.SRGC");

                foreach (DataRow dr in dt.Rows)
                {
                    string[] row = new string[] { dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString() };

                    _output.Add(new CompanyModel
                    {
                        CompanyName = dr[0].ToString(),
                        Database = dr[1].ToString(),
                        Localization = dr[2].ToString(),
                        Version = dr[3].ToString()
                    });
                }
            }
            catch { }

            return _output;
        }

        public bool Connect()
        {
            bool _output = false;
            try
            {
                iLogin login = new Login();
                login.CompanyDB = CompanyDB;
                login.UserName = UserName;
                login.Password = Password;

                _output = login.Connect();

                ServiceLayer = login.ServiceLayer;
                ResultCode = login.ResultCode;
                ResultDescription = login.ResultDescription;
            }
            catch (Exception ex)
            {
                ResultCode = 1;
                ResultDescription = ex.Message;
            }

            return _output;
        }
    }
}
