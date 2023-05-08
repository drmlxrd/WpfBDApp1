﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.Drawing;
using WpfBDApp1.Items;
using System.IO;
using QRCoder;
using Microsoft.EntityFrameworkCore;

namespace WpfBDApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationContext db = new ApplicationContext();
        public ObservableCollection<Product> ItemProduct { get; set; }
        public Product Product { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            ItemProduct = new();
            DataContext = this;
            this.Loaded += Sqlite_Loaded;
            
        }

        private void Sqlite_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationContext db = new ApplicationContext();
            db.Database.Migrate();
            List<Product> products = db.Products.ToList();
            productList.ItemsSource = products;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            foreach (Product product in products)
            {
                string str = "Уникальный идентификатор: " + product.Id + "\n" + "Название товара: " + product.Name + "\n" + "Цена товара: " + product.Price + " рублей" + "\n" + "Описание товара: " + product.Description;
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(str, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                BitmapImage qrCodeImage = new BitmapImage();
                using (MemoryStream stream = new MemoryStream())
                {
                    qrCode.GetGraphic(20).Save(stream, ImageFormat.Png);
                    stream.Seek(0, SeekOrigin.Begin);
                    qrCodeImage.BeginInit();
                    qrCodeImage.CacheOption = BitmapCacheOption.OnLoad;
                    qrCodeImage.StreamSource = stream;
                    qrCodeImage.EndInit();
                }

                ItemProduct.Add(new Product { Name = product.Name, Price = product.Price, QrCode = qrCodeImage, Description = product.Description, Id = product.Id });
            }

            productList.ItemsSource = ItemProduct;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Product user = productList.SelectedItem as Product;
            if (user is null) return;
            UserWindow UserWindow = new UserWindow(new Product
            {
                Id = user.Id,
                Name = user.Name,
                Price = user.Price,
                Description = user.Description
            });

            if (UserWindow.ShowDialog() == true)
            {
                user = db.Products.Find(UserWindow.Product.Id);
                if (user != null)
                {
                    user.Name = UserWindow.Product.Name;
                    user.Price = UserWindow.Product.Price;
                    user.Description = UserWindow.Product.Description;
                    db.SaveChanges();
                    productList.Items.Refresh();
                }
            }


        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            UserWindow UserWindow = new UserWindow(new Product());
            if (UserWindow.ShowDialog() == true)
            {
                Product User = UserWindow.Product;
                db.Products.Add(User);
                db.SaveChanges();
                Close();

            }
            

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Product? user = productList.SelectedItem as Product;
            if (user is null) return;
            db.Products.Remove(user);
            db.SaveChanges();


        }
    }
}
