using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Core.Models.Dto
{
    public class UserRoleDto
    {
        public int Id { get; set; }

        public Guid FkUserId { get; set; }

        public string Role { get; set; }
        public int FkRoleId { get; set; }
    }
}
