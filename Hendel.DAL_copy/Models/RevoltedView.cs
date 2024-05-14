using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hendel.DAL_copy.Models
{
    public class RevoltedView
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int PSW { get; set; } //пароль для подтверждения 
        public int RevoltedPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
