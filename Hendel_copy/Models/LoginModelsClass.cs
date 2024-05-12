using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hendel_copy.Models
{
    public class LoginModelsClass
    {
        [Required(ErrorMessage = "Не указано имя")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "минимум 3 до 100 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "минимум 3 до 100 символов")]
        public string Password { get; set; }
    }
}
