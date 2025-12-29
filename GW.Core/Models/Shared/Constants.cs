using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Core.Models.Shared
{
    public static class Constants
    {
        public static readonly string GW_FOTA_DIRECTORY = "GW_Files\\GW_FOTA_DIRECTORY";
        public static readonly string GW_SOFTWARE_DIRECTORY = "GW_Files\\GW_SOFTWARE_DIRECTORY";
        public static string NO_DATA = "NO_DATA";
        public static Guid FOTA_URL_IDENTITY = Guid.NewGuid();
        public static int FOTA_TIMER = 10;
        public static string FOTA_FILE = "fota";
        public static string SOFT_FILE = "soft";
        public static string FOTA_ESP_FILE_CONTENT = "FOTA_ESP";
        public static string FOTA_STM_FILE_CONTENT = "FOTA_STM";
        public static string FOTA_HOLTECK_FILE_CONTENT = "FOTA_HOLTECK";
        public static readonly string NULL_SETTING_VALUE = "-1";
    }

    public static class ErrorCode
    {
        public static readonly string INTERNAL_ERROR = "INTERNAL_ERROR";
        public static readonly string NOT_FOUND = "NOT_FOUND";
        public static readonly string INVALID_ID = "INVALID_ID";
        public static readonly string DUPLICATE_DATA = "DUPLICATE_DATA";
        public static readonly string NO_FILE_UPLOADED = "NO_FILE_UPLOADED";
        public static readonly string NO_CONTENT = "NO_CONTENT";
        public static readonly string ACTION_LOCKED = "ACTION_LOCKED";
    }
}
