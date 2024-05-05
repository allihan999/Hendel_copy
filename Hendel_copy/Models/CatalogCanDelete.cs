namespace Hendel_copy.Models
{
    public class CatalogCanDelete
    {
        public IQueryable<Catalog> Catalog { get; set; }
        public string Text { get; set; }    
    }
}
