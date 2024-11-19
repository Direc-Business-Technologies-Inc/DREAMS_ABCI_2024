using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirecLayer
{
    public class Hana
    {
        #region ConnectionString
        public static string ConnectionString(string sServer,
                                            string sDbUserId,
                                            string sDbPassword,
                                            string sDatabase,
                                            bool bRefresh = false)
        {
            var output = new StringBuilder();

            try
            {
                output.Append("DRIVER={HDBODBC32};");
                output.Append($"SERVERNODE={sServer};");
                output.Append($"UID={sDbUserId};");
                output.Append($"PWD={sDbPassword};");
                output.Append($"CS={sDatabase};");

                if (bRefresh)
                {
                    App.UpdateConnectionString("SapHana", output.ToString());
                }
            }
            catch (Exception ex)
            { throw new Exception($"Error : {ex.Message}"); }

            return output.ToString();
        }

        public static string ConnectionString(string sServer,
                                        string sDbUserId,
                                        string sDbPassword,
                                        bool bRefresh = false)
        {
            var output = new StringBuilder();

            try
            {
                output.Append("DRIVER={HDBODBC32};");
                output.Append($"SERVERNODE={sServer};");
                output.Append($"UID={sDbUserId};");
                output.Append($"PWD={sDbPassword};");
                output.Append($"CS={App.AppSettings("SapDatabaseName")};");

                if (bRefresh)
                { App.UpdateConnectionString("SapHana", output.ToString()); }

            }
            catch (Exception ex)
            { throw new Exception($"Error : {ex.Message}"); }

            return output.ToString();
        }

        public static string ConnectionString(string sDatabase, bool bRefresh = false)
        {
            var output = new StringBuilder();

            try
            {
                output.Append("DRIVER={HDBODBC32};");
                output.Append($"SERVERNODE={App.AppSettings("SapDatabaseServer")};");
                output.Append($"UID={App.AppSettings("SapDatabaseUserID")};");
                output.Append($"PWD={App.AppSettings("SapDatabasePassword")};");
                output.Append($"CS={sDatabase};");

                if (bRefresh)
                { App.UpdateConnectionString("SapHana", output.ToString()); }
            }
            catch (Exception ex)
            { throw new Exception($"Error : {ex.Message}"); }

            return output.ToString();
        }

        public static string ConnectionString(bool bRefresh = false)
        {
            var output = new StringBuilder();

            try
            {
                output.Append("DRIVER={HDBODBC32};");
                output.Append($"SERVERNODE={App.AppSettings("SapDatabaseServer")};");
                output.Append($"UID={App.AppSettings("SapDatabaseUserID")};");
                output.Append($"PWD={App.AppSettings("SapDatabasePassword")};");
                output.Append($"CS={App.AppSettings("SapDatabaseName")};");

                if (bRefresh)
                { App.UpdateConnectionString("SapHana", output.ToString()); }
            }
            catch (Exception ex)
            { throw new Exception($"Error : {ex.Message}"); }

            return output.ToString();
        }
        #endregion

        #region RESTful Return
        public static DataTable Get(string sConnectionString,
                              string sQuery)
        {
            var output = new DataTable();
            try
            {
                using (var dataAdapter = new HanaDataAdapter(sQuery, sConnectionString))
                {
                    using (var dataTable = new DataTable())
                    {
                        dataAdapter.Fill(dataTable);
                        output = dataTable;
                    }
                }

            }
            catch (Exception ex)
            { throw new Exception($"Error : RESTful Return Create {ex.Message}"); }

            return output;
        }

        public static DataTable Get(string sQuery)
        {
            var output = new DataTable();

            try
            {
                if (string.IsNullOrEmpty(sQuery) == false)
                {
                    using (var dataAdapter = new HanaDataAdapter(sQuery, ConnectionString()))
                    {
                        using (var dataTable = new DataTable())
                        {
                            dataAdapter.Fill(dataTable);
                            output = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error : RESTful Return Create {ex.Message}");
            }
            return output;
        }


        public static int Execute(string sConnectionString,
                                string sQuery)
        {
            var output = -999;
            try
            {
                using (var connection = new HanaConnection(sConnectionString))
                {
                    using (var command = new HanaCommand(sQuery, connection))
                    {
                        connection.Open();
                        output = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            { throw new Exception($"Error : Execute {ex.Message}"); }

            return output;
        }

        public static int Execute(string sQuery)
        {
            var output = -999;
            try
            {
                using (var connection = new HanaConnection(ConnectionString()))
                {
                    using (var command = new HanaCommand(sQuery, connection))
                    {
                        connection.Open();
                        output = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            { throw new Exception($"Error : Execute {ex.Message}"); }

            return output;
        }
        #endregion
    }
}
