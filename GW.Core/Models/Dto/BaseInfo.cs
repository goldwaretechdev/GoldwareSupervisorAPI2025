using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Core.Models.Dto
{
    public class BaseInfo
    {
        public List<SelectListItem> DeviceTypes { get; set; }
        public List<SelectListItem> ProductCategory { get; set; }
        public List<SelectListItem> ProductOwner{ get; set; }

    }

}
