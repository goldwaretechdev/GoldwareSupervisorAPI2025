using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Core.Models.Dto
{
    public class DeviceDto
    {
    }

    public class DeviceCheckDto
    {
        public string AccessCode { get; set; }
        public string Type { get; set; }
    }

    public class FOTAVerify
    {
        public bool Success { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
    }
}
