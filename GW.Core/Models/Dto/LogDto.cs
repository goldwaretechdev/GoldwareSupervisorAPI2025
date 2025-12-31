using GW.Core.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Core.Models.Dto
{
    public class LogDto
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        [MaxLength(150)]
        public string? Desc { get; set; }
        public LogType Type { get; set; }
        public int? FkDeviceId { get; set; }
        public int? FkUserRoleId { get; set; }
    }
}
