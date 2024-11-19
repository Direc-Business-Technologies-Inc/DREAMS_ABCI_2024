using Sap.Data.Hana;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace ABROWN_DREAMS
{
    public class SAPHanaAccess
    {
        public string GetConnection(string sAppName)
        {
            var output = ConfigurationManager.ConnectionStrings[sAppName] != null ? ConfigurationManager.ConnectionStrings[sAppName].ToString() : "";
            return output;
        }



        public DataTable GetData(string sQuery, string Connectionstring)
        {
            try
            {
                using (DataTable dt = new DataTable())
                {
                    {
                        using (var dataAdapter = new HanaDataAdapter(sQuery, Connectionstring))
                        {
                            using (var dataTable = new DataTable())
                            {
                                dataAdapter.Fill(dataTable);
                                return dataTable;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Log("0"
                    , "UserManagement"
                    , "Critical"
                    , ex.Message
                    , "SAPHanaAccess.GetData function error");
                return null;
            }

            //return output;
        }

        public DataSet GetDataDS(string query, string Connectionstring)
        {
            DataSet ret = null;
            try
            {
                using (HanaDataAdapter da = new HanaDataAdapter(query, Connectionstring))
                {
                    using (DataSet ds = new DataSet())
                    {
                        da.Fill(ds);
                        ret = ds;
                    }
                }

            }
            catch (Exception ex) { ret = null; }
            return ret;
        }



        public bool Execute(string sQuery, string Connectionstring)
        {
            Boolean _bool = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(sQuery))
                {
                    using (DataTable dt = new DataTable())
                    {
                        using (HanaConnection con = new HanaConnection(Connectionstring))
                        {
                            HanaCommand cmd = new HanaCommand();
                            cmd = con.CreateCommand();
                            con.Open();
                            cmd.CommandText = sQuery;
                            cmd.ExecuteNonQuery();
                            _bool = true;
                        }
                    }
                }
                else
                {
                    _bool = true;
                }
            }
            catch (Exception ex)
            {
                //frmMain.NotiMsg(ex.Message, Color.Red);
            }
            return _bool;
        }

        public string ExecuteStr(string sQuery, string Connectionstring)
        {
            string ret = "";
            try
            {
                using (DataTable dt = new DataTable())
                {
                    using (HanaConnection con = new HanaConnection(Connectionstring))
                    {
                        HanaCommand cmd = new HanaCommand();
                        cmd = con.CreateCommand();
                        con.Open();
                        cmd.CommandText = sQuery;
                        cmd.ExecuteNonQuery();
                        ret = "Operation completed successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                //frmMain.NotiMsg(ex.Message, Color.Red);
            }
            return ret;
        }

    }
}