namespace Hendel_copy.Models
{
    public class WatchEdit_Archive_View_Models
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int AdminInputClassId { get; set; }
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public string WatchCategory { get; set; }
    }
}
