using System.ComponentModel.DataAnnotations;

namespace Hendel_copy.Models
{
    public class WatchEditViewModels
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите название")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "минимум 1 до 100 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Выберите картинку")]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "Укажите описание")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "минимум 1 до 100 символов")]
        public string Description { get; set; }

        [Range(0, 100000, ErrorMessage = "Используйте цену до 100т.")]
        public int Price { get; set; }

        [Range(0, 10000, ErrorMessage = "Используйте количесвто до 10т.")]
        public int Amount { get; set; }
        public string WhichCatalog { get; set; }

    }
}
