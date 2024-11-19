using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace ABROWN_DREAMS
{
    /// <summary>
    /// Summary description for HouseModel
    /// </summary>
    public class HouseModel : IHttpHandler
    {
        SAPHanaAccess hana = new SAPHanaAccess();

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string id = context.Request.QueryString["Id"];
                context.Response.ContentType = "image/jpeg"; // for JPEG file
                DataTable dt = hana.GetData(@"SELECT ""BitmapPath"" FROM ""OADP""", hana.GetConnection("SAPHana"));
                string path = Path.Combine(dt.Rows[0][0].ToString(), id);
                context.Response.WriteFile(path);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "image/jpeg"; // for JPEG file
                context.Response.WriteFile("~/assets/img/no_image.png");
            }  
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}