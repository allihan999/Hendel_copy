using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hendel.DAL_copy.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CatalogId { get; set; }

        //Информация о пользователе
        public string UserName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DoublePassword { get; set; }
        public string Role { get; set; }


        public string LiveCity { get; set; }
        public string NumbetPhone { get; set; }


        //Информация о товаре
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }

      
        //Соглосовать визит 
        public string Visit {  get; set; }
        public string Pochta { get; set; }

        
    }
}
