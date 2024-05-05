using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hendel.DAL_copy.Models
{
    public class Watch
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public int MyKorzinaId { get; set; }

        public string Name { get; set; }
        public byte[] Image { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
    }
}
