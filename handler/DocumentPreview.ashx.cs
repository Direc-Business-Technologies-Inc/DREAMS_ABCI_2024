using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABROWN_DREAMS
{
    public class DocumentPreview : IHttpHandler
    {
        DirecWebService ws = new DirecWebService();
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string id = context.Request.QueryString["Id"];
                string doc = context.Request.QueryString["Doc"];
                context.Response.ContentType = "image";
                context.Response.BinaryWrite((Byte[])ws.DocumentPreview(id, doc));
            }
            catch(Exception ex)
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