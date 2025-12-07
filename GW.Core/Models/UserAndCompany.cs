using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Core.Models
{
    public class UserAndCompany
    {
        [Key]
        public int Id { get; set; }

        public User User { get; set; }
        public Guid FkUserId { get; set; }
        public Company Company { get; set; }
        public int FkCompanyId { get; set; }
    }
}
