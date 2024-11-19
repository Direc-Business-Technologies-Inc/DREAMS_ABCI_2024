using MSXML2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ABROWN_DREAMS.wcf
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISapHanaLayer" in both code and config file together.
    [ServiceContract]
    public interface ISapHanaLayer
    {
        long ResultCode { get; set; }
        string ResultDescription { get; set; }
    }
}
