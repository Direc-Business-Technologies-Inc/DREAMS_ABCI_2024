using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABROWN_DREAMS
{
    /// <summary>
    /// Summary description for BlockPreview
    /// </summary>
    public class BlockPreview : IHttpHandler
    {
        DirecWebService ws = new DirecWebService();
        SAPHanaAccess hana = new SAPHanaAccess();
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string id = context.Request.QueryString["Id"];
                string block = context.Request.QueryString["Block"];
                context.Response.ContentType = "image";
                if (block != "")
                { context.Response.BinaryWrite((Byte[])ws.BlockPreview(hana.GetConnection("SAOHana"), id, block)); }
            }
            catch
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