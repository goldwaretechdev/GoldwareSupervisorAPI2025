using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Core.Models.Enum
{
    public enum LogType
    {
        SetSettings=1,
        GetSettings=2,
        FOTA_Update_Requested = 3,
        FOTA_Update_Done=4,
        Programming_ESP=5,
        Programming_STM=6,
        Programming_HT=7,
        Stock = 8,
        Sale = 9,
    }
}
