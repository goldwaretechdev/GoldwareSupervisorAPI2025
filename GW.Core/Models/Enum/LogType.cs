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
        GetSettings =2,
        FOTA_Update=3,
        Programming_ESP=4,
        Programming_STM=5,
        Programming_HT=6,
        Stock = 7,
        Sale = 8,
    }
}
