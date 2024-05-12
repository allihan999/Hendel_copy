using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hendel_copy.Models
{
    public class RegistrModelsProject
    {

        [Required(ErrorMessage = "Не указано имя")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "минимум 3 до 100 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указана фамилия")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "минимум 3 до 100 символов")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Не указан электронный адрес")]
        //[Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "Email уже используется")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "минимум 3 до 100 символов")]
        public string Password { get; set; }


        [StringLength(100, MinimumLength = 3, ErrorMessage = "минимум 3 до 100 символов")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string DoublePassword { get; set; }

        [StringLength(100, MinimumLength = 0, ErrorMessage = "минимум 3 до 100 символов")]
        [Required(ErrorMessage = "Не указан пароль")]
        public string Role { get; set; }
    }
}

