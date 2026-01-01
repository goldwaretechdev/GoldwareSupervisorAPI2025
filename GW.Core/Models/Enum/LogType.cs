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
        Update_FOTA_File = 5,
        Programming=6 ,
        Stock = 7,
        Sale = 8,
        Login = 9,
        Insert_SoftwareVersion_File = 10,

    }
}
