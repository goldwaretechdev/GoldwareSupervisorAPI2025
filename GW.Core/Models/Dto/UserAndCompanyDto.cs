using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Core.Models.Dto
{
    public class UserAndCompanyDto
    {
        public int Id { get; set; }
        public Guid FkUserId { get; set; }
        public int FkCompanyId { get; set; }
    }
}
