using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfBDApp1.Items
{
    public class Product
    {
        [Key]
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public Guid Id { get; set; }

        [NotMapped]
        public BitmapImage QrCode { get; set; }
    }
}
