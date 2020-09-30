using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyApp.Library.Model
{
    public class Product
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public Product()
        {
        }

        public override string ToString()
        {
            return "Title: " + Title + "\n" + "Description: " + Description + "\n" + "Price: " + Price;
        }
    }
}
