using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
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
    /// Логика взаимодействия для Order.xaml
    /// </summary>
    public partial class Zakaz : Window, INotifyPropertyChanged
    {

        public string CountView { get; set; }

        public string FIO { get; }
        private readonly User user;
        private string searchText = "";
        private int selectedSorting;
        private TblManufacturer selectedManufactorer;

        public event PropertyChangedEventHandler? PropertyChanged;
        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public Product SelectedProduct { get; set; }

        public List<TblManufacturer> Manufactorers { get; set; }
        public List<Product> Products { get; set; }
        public List<string> Sorting { get; set; } = new List<string>() { "Без сортировки", "Стоимость по убыванию", "Стоимость по возрастанию" };
        public Visibility IsAdminVisibility { get; set; }

        public int SelectedSorting
        {
            get => selectedSorting;
            set
            {
                selectedSorting = value;
                Search();
            }
        }
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                Search();
            }
        }
        public TblManufacturer SelectedManufactorer
        {
            get => selectedManufactorer;
            set
            {
                selectedManufactorer = value;
                Search();
            }
        }

        private void Search()
        {
            var result = DB.GetInstance().Products.
                Include(s => s.ProductProviderNavigation).
                Include(s => s.ProductManufacturerNavigation).
                Include(s => s.ProductCategoryNavigation).Where(s =>
                    s.ProductProviderNavigation.ProviderTitle.Contains(searchText) ||
                    s.ProductManufacturerNavigation.ManufacturerTitle.Contains(searchText) ||
                    s.ProductArticleNumber.Contains(searchText) ||
                    s.ProductCategoryNavigation.CategoryTitle.Contains(searchText) ||
                    s.ProductDescription.Contains(searchText) ||
                    s.ProductName.Contains(searchText)
                );
            if (SelectedManufactorer.ManufacturerId != 0)
                result = result.Where(s => s.ProductManufacturer == SelectedManufactorer.ManufacturerId);
            if (selectedSorting == 1)
                result = result.OrderByDescending(s => s.ProductCost);
            else if (selectedSorting == 2)
                result = result.OrderBy(s => s.ProductCost);
            Products = result.ToList();
            Signal(nameof(Products));

            CountView = $"Записей: {Products.Count} из {DB.GetInstance().Products.Count()}";
            Signal(nameof(CountView));
        }

        public Zakaz(User user)
        {
            InitializeComponent();

            this.user = user;
            FIO = $"{user.UserSurname} {user.UserName} {user.UserPatronymic}";
            IsAdminVisibility =
                user.UserRoleId == 1 ? // Администратор
                Visibility.Visible :
                Visibility.Collapsed;
            FillManufactorers();
            Search();
            DataContext = this;
        }

        private void FillManufactorers()
        {
            Manufactorers = new List<TblManufacturer>();
            Manufactorers.Add(new TblManufacturer { ManufacturerTitle = "Все производители" });
            Manufactorers.AddRange(DB.GetInstance().TblManufacturers.ToList());

            selectedManufactorer = Manufactorers.FirstOrDefault();
        }


        private void buttonExitToLogin(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void AddProduct(object sender, RoutedEventArgs e)
        {
            new RedacProd(new Product()).ShowDialog();
            Search();
        }

        private void EditProduct(object sender, RoutedEventArgs e)
        {
            if (SelectedProduct != null)
            {
                new RedacProd(SelectedProduct).ShowDialog();
                Search();
            }
        }

        private void RemoveProduct(object sender, RoutedEventArgs e)
        {
            if (SelectedProduct != null)
            {
                if (SelectedProduct.OrderProducts.Count == 0)
                {
                    if (MessageBox.Show("Удалить выбранный товар?", "Внимание!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        DB.GetInstance().Products.Remove(SelectedProduct);
                        DB.GetInstance().SaveChanges();
                        Search();
                    }
                }
                else
                    MessageBox.Show("Выбранный товар нельзя удалить, поскольку он фигурирует в заказах");
            }
        }

        private void EditProduct(object sender, MouseButtonEventArgs e)
        {
            if (user.UserRoleId != 1)
                return;
            if (SelectedProduct != null)
            {
                new RedacProd(SelectedProduct).ShowDialog();
                Search();
            }
        }

        SoundPlayer player4 = new SoundPlayer(Environment.CurrentDirectory + @"\music\bonk_BEtiM8g.wav");
        private void bonk(object sender, MouseButtonEventArgs e)
        {
            player4.Play();
        }
    }
}

