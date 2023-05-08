using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBDApp1.Items;

namespace WpfBDApp1.Metod
{
    public class Metod
    {
        public static string AddProduct(Guid Id, string name, double price, string Description)
        {

            string result = "Уже есть такой";
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.Migrate();
                bool check = db.Products.Any(el => el.Name == name && el.Price == price && el.Description == Description);
                if (!check)
                {
                    Product newproduct = new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = name,
                        Price = price,
                        Description = Description,
                    };
                    db.Products.Add(newproduct);
                    db.SaveChanges();
                    result = "Сделано";
                }
                return result;
            }
        }
    }
}
