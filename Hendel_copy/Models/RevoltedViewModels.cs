using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Hendel_copy.Models
{
    public class RevoltedViewModels
    {
        [Required(ErrorMessage = "Не указан электронный адрес")]
        public string Email { get; set; }

        [Range(0, 100000, ErrorMessage = "Введите цифру от 0")]
        public int PSW { get; set; } //пароль для подтверждения 


        [Remote(action: "CheckPassword", controller: "Account", ErrorMessage = "Не првильный код подтверждение")]
        public int RevoltedPassword { get; set; }
        
        [Required(ErrorMessage = "Не указан пароль")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "минимум 3 до 100 символов")]
        public string NewPassword { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "минимум 3 до 100 символов")]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
