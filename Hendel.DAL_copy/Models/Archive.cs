using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hendel.DAL_copy.Models
{
    public class Archive
    {
        public int Id { get; set; }
        public int AdminInputClassId {  get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public string WatchCategory { get; set; }
    }
}
