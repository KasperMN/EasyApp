using EasyApp.DataAccess;
using EasyApp.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EasyApp
{
    class BootApp
    {
        static void Main(string[] args)
        {
            DBProducts dBProducts = new DBProducts();
            dBProducts.Delete(dBProducts.GetByTitle("Car"));

            dBProducts.Delete(dBProducts.GetByTitle("Bus"));

            bool insert = dBProducts.Insert(new Product()
            {   Title = "Bus", 
                Description = "A small bus", 
                Price = 79_900 
            });

            bool insert2 = dBProducts.Insert(new Product()
            {
                Title = "Car",
                Description = "A small car",
                Price = 29_000
            });
            Console.WriteLine(insert + " " + insert2);

            dBProducts.GetAll();
        }
    }
}
