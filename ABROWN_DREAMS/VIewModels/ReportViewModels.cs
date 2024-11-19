using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABROWN_DREAMS.VIewModels
{
    public class ReportViewModel
    {
        public ImageData imgData { get; set; }
        public List<ImageData> ImageDatas { get; set; }
        public ImageData1 imgData1 { get; set; }
        public List<ImageData1> ImageDatas1 { get; set; }
        public class ImageData 
        {
            public string projectCode { get; set; }
            public string projectName { get; set; }
            public int imgWidth { get; set; }
            public int imgHeight { get; set; }
            //public HttpPostedFileBase canvassImage { get; set; }

        }
        public class ImageData1
        {
            public dynamic canvassImage { get; set; }
            public string prjCode { get; set; }
            //public HttpPostedFileBase canvassImage { get; set; }

        }
    }
}