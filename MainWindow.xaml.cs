﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
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


namespace Myasokombinat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string errorMessage;

        public event PropertyChangedEventHandler? PropertyChanged;
        void Signal([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public string Login { get; set; }
        public string CaptchaUserText { get; set; }
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                Signal();
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            

            DataContext = this;
        }

        private void CheckAuth(string login, string pass)
        {

            var user = DB.GetInstance().Users.Include(s => s.UserRole).FirstOrDefault(s => s.UserLogin == login && s.UserPassword == pass);

            if (user == null)
            {
                GenerateCaptcha();  
            }
            else
            {
                Zakaz zakazs = new Zakaz(user);
                zakazs.ShowDialog();
            }

        }


        private void buttonEnter(object sender, RoutedEventArgs e)
        {
            if (CaptchaValid())
                CheckAuth(Login, passwordBox.Password);
            else
            {
                ErrorMessage = "Неверный код с картинки";
                GenerateCaptcha();
            }
        }

        private void CheckAuth(object login, string pass)
        {
            ErrorMessage = null;
            var user = DB.GetInstance().Users.
                            Include(s => s.UserRole).
                            FirstOrDefault(s => s.UserLogin == login &&
                                s.UserPassword == pass);
            if (user == null)
            { // неудачная авторизация
                ErrorMessage = "Ошибка авторизации";
                GenerateCaptcha();
            }
            else
            {
            }
        }

        private bool CaptchaValid()
        {
            if (captchaText == null)
                return true;
            return captchaText == CaptchaUserText;
        }

        Random random = new Random();
        string captchaText = null;
        private void GenerateCaptcha()
        {
            captchaText = GenerateText();
            captchCanvas.Children.Clear();
            captchaPanel.Visibility = Visibility.Visible;
            int x, y;
            x = y = 10;
            foreach (var s in captchaText)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.FontSize = 25;
                textBlock.Text = s.ToString();
                captchCanvas.Children.Add(textBlock);
                Canvas.SetLeft(textBlock, x);
                Canvas.SetTop(textBlock, y);
                x += 8 + random.Next(-5, 10);
                y = 13 + random.Next(-5, 5);
            }
            var line = new Line();
            line.X1 = random.Next(5, 10);
            line.X2 = 50 + random.Next(5, 15);
            line.Y1 = random.Next(15, 20);
            line.Y2 = random.Next(10, 15);
            line.Stroke = Brushes.Black;
            line.StrokeThickness = 2;

            captchCanvas.Children.Add(line);
            Canvas.SetLeft(line, random.Next(5, 15));
            Canvas.SetTop(line, random.Next(5, 15));
        }

        (int, int) digits = (48, 58);
        (int, int) charsUpper = (65, 91);
        (int, int) charsLower = (97, 123);

        private string GenerateText()
        {
            string result = null;
            for (int i = 0; i < 4; i++)
            {
                char s;
                switch (random.Next(3))
                {
                    case 0: s = (char)random.Next(digits.Item1, digits.Item2); break;
                    case 1: s = (char)random.Next(charsUpper.Item1, charsUpper.Item2); break;
                    default: s = (char)random.Next(charsLower.Item1, charsLower.Item2); break;
                }
                result += s;
            }
            return result;
        }

        SoundPlayer player1 = new SoundPlayer(Environment.CurrentDirectory+ @"\music\playful-cat-sounds_z1wz-rnd.wav");

        private void meow(object sender, RoutedEventArgs e)
        {
            player1.Play();
        }

        SoundPlayer player2 = new SoundPlayer(Environment.CurrentDirectory + @"\music\укпук (audio-extractor.net).wav");
        private void ce(object sender, MouseButtonEventArgs e)
        {
            player2.Play();
        }

        SoundPlayer player3 = new SoundPlayer(Environment.CurrentDirectory + @"\music\антошка (audio-extractor.net).wav");
        private void antoshka(object sender, MouseButtonEventArgs e)
        {
            player3.Play();
        }

        SoundPlayer player5 = new SoundPlayer(Environment.CurrentDirectory + @"\music\u kill a ch (audio-extractor.net).wav");
        private void kaz(object sender, MouseButtonEventArgs e)
        {
            player5.Play();
        }
    }
}
