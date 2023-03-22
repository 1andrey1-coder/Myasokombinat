using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Shapes;

namespace Myasokombinat
{
    /// <summary>
    /// Логика взаимодействия для RedacProd.xaml
    /// </summary>
    public partial class RedacProd : Window
    {
        public bool Editable { get; set; }
        public List<TblCategory> TblCategories { get; set; }
        public List<TblManufacturer> TblManufacturers { get; set; }
        public List<TblProvider> TblProviders { get; set; }

        public RedacProd(Product selectedProduct)
        {
            InitializeComponent();
            TblCategories = DB.GetInstance().TblCategories.ToList();
            TblManufacturers = DB.GetInstance().TblManufacturers.ToList();
            TblProviders = DB.GetInstance().TblProviders.ToList();
            if (string.IsNullOrEmpty(selectedProduct.ProductArticleNumber))
            {
                SelectedProduct = selectedProduct;
            }
            else
            {
                SelectedProduct = selectedProduct.Clone();
                Editable = true;
            }

            DataContext = this;
        }

        public Product SelectedProduct { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private void SaveClose(object sender, RoutedEventArgs e)
        {
            {
                // проверки
            }
            if (!Editable)
                DB.GetInstance().Products.Add(SelectedProduct);
            else
            {
                var original = DB.GetInstance().Products.
                    Find(SelectedProduct.ProductArticleNumber);
                DB.GetInstance().Products.Entry(original)
                    .CurrentValues.SetValues(SelectedProduct);
            }
            DB.GetInstance().SaveChanges();
            Close();
        }

        private void SelectPhoto(object sender, RoutedEventArgs e)
        {
            string dir = Environment.CurrentDirectory + @"\Images\";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Images|*.png;";
            if (dlg.ShowDialog() == true)
            {
                var test = new BitmapImage(new Uri(dlg.FileName));
                if (test.PixelWidth > 400 || test.PixelHeight > 300)
                {
                    MessageBox.Show("Картинка слишком большая");
                    return;
                }
                string newFile = dir + new FileInfo(dlg.FileName).Name;
                File.Copy(dlg.FileName, newFile, true);
                SelectedProduct.ProductPhoto = File.ReadAllBytes(newFile);
                Signal("SelectedProduct");
            }
        }
    }

    public static class ProductExtension
    {
        public static Product Clone(this Product product)
        {
            var values = DB.GetInstance().Products.Entry(product).CurrentValues.Clone();
            var clone = (Product)values.ToObject();
            clone.ProductCategoryNavigation = product.ProductCategoryNavigation;
            clone.ProductManufacturerNavigation = product.ProductManufacturerNavigation;
            clone.ProductProviderNavigation = product.ProductProviderNavigation;
            return clone;
        }
    }
}

