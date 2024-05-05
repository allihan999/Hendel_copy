namespace Hendel_copy.Models
{
    public class WatchEditViewModels
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public string WhichCatalog { get; set; }

    }
}
