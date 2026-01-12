using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Core.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string ShortName { get; set; }
        public string? Icon { get; set; }
        public int Charge { get; set; } = 3;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? UpdatedOn { get; set; }

        public ICollection<Device> OwnerDevices { get; set; }
        public ICollection<Device> MainOwnerDevices { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<FOTA> OwnerFOTAs { get; set; }
        public ICollection<FOTA> MainOwnerFOTAs { get; set; }

    }
}
