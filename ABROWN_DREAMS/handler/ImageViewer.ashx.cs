using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace ABROWN_DREAMS
{
    /// <summary>
    /// Summary description for ImageViewer
    /// </summary>
    public class ImageViewer : IHttpHandler
    {
        DirecWebService ws = new DirecWebService();
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string id = context.Request.QueryString["Id"];
                if ((Byte[])ws.GetProjectImage("Addon", id) != null)
                {
                    Byte[] inputBytes = (Byte[])ws.GetProjectImage("Addon", id);
                    //var jpegQuality = 50;
                    //Image image;
                    //Byte[] outputBytes;
                    //using (var inputStream = new MemoryStream(inputBytes))
                    //{
                    //    image = Image.FromStream(inputStream);
                    //    var jpegEncoder = ImageCodecInfo.GetImageDecoders()
                    //      .First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                    //    var encoderParameters = new EncoderParameters(1);
                    //    encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, jpegQuality);
                    //    using (var outputStream = new MemoryStream())
                    //    {
                    //        image.Save(outputStream, jpegEncoder, encoderParameters);
                    //        outputBytes = outputStream.ToArray();
                    //    }
                    //}
                    context.Response.ContentType = "image";
                    context.Response.BinaryWrite(inputBytes);
                }
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