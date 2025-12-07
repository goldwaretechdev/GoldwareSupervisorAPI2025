using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Core.Models.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Mobile { get; set; }
    }


    public class LoginInfo
    {
        [MaxLength(50)]
        public string UserName { get; set; }
        [MinLength(8)]
        public string Password { get; set; }
        public int Role {  get; set; }
    }
}
