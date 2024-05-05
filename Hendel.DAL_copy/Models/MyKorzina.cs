using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hendel.DAL_copy.Models
{
    public class MyKorzina
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int WatchId { get; set; }

        public List<Watch> KorzinaWatches { get; set; } = new List<Watch>();

        public string UserName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DoublePassword { get; set; }
        public string Role { get; set; }

        public int AmountBuyUser { get; set; }
    }
}
