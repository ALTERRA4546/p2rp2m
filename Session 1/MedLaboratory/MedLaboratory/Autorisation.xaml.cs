using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.Xml;
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
using System.Windows.Threading;

namespace MedLaboratory
{
    /// <summary>
    /// Логика взаимодействия для Autorisation.xaml
    /// </summary>
    public partial class Autorisation : Window
    {
        private DispatcherTimer timer;
        public int second = 10;
        public bool closeApp = true;

        public Autorisation()
        {
            InitializeComponent();

            passwordV.Visibility = Visibility.Collapsed;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += OnTimedEvent;
        }

        public bool passwordVisability = false;
        public int tryer = 0;

        public static class userData
        { 
            public static int idUser {  get; set; }
            public static int idOrder { get; set; }
            public static int idIspolMater { get; set; }
            public static int idReport { get; set; }
            public static int idService { get; set; }
            public static bool checkCapcha { get;set; }
        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            if (second <= 0)
            {
                userData.checkCapcha = false;
                enter.IsEnabled = true;
                enter.Content = "Авторизация";
                tryer = 1;
                timer.Stop();
                return;
            }
            enter.Content = second;
            second--;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tryer >= 1)
            {
                Capcha cp = new Capcha();
                userData.checkCapcha = true;
                cp.ShowDialog();
            }

            if (login.Text == "" || passwordH.Password == "")
            {
                MessageBox.Show("Заполните поля");
                return;
            }

            using (var bd = new MedLaboratoryEntities())
            {
                string log = login.Text;
                string pas = passwordH.Password;
                var user = bd.Авторизация.FirstOrDefault(p=>p.Логин == log && p.Пароль == pas);
                if (user != null)
                {
                    if ((user.Время_блокировки != null) && (Convert.ToInt32((DateTime.Now - user.Время_блокировки).Value.TotalMinutes) <= 30))
                    {
                        MessageBox.Show($"Учетная запись временно заблокированна. До конца блокировки {30 - ((DateTime.Now - user.Время_блокировки).Value.Minutes)} минут.");
                        return;
                    }

                    var role = from pol in bd.Пользователи
                               join
                               rol in bd.Должность on pol.Код_должности equals rol.Код_должности
                               where (pol.Код_пользователя == user.Код_пользователя)
                               select rol.Название;
                    switch (role.FirstOrDefault())
                    {
                        case "лаборант":
                            UpdateTime(Convert.ToInt32(user.Код_пользователя));
                            LaboratoryAssistant la = new LaboratoryAssistant();
                            la.Show();
                            this.Hide();
                            break;
                        
                        case "лаборант-исследователь":
                            UpdateTime(Convert.ToInt32(user.Код_пользователя));
                            LaboratoryResearcher lr = new LaboratoryResearcher();
                            lr.Show();
                            this.Hide();
                            break;

                        case "бухгалтер":
                            UpdateTime(Convert.ToInt32(user.Код_пользователя));
                            Accountant ac = new Accountant();
                            ac.Show();
                            this.Hide();
                            break;
                        
                        case "администратор":
                            UpdateTime(Convert.ToInt32(user.Код_пользователя));
                            Administrator ad = new Administrator();
                            ad.Show();
                            this.Hide();
                            break;
                    }
                }
                else
                {
                    if (!userData.checkCapcha && tryer >= 1)
                    {
                        MessageBox.Show("Система заблокированна на 10 секунд");
                        second = 10;
                        timer.Start();
                        enter.IsEnabled = false;
                        return;
                    }
                    MessageBox.Show("Логин или пароль указаны неверно");
                    tryer++;
                    return;
                }
            }
        }

        private void UpdateTime(int userKode)
        {
            IPAddress localIPv4 = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            using (var bd = new MedLaboratoryEntities())
            {
                var user = bd.Авторизация.FirstOrDefault(a=>a.Код_авторизации == userKode);
                var history = new История();
                userData.idUser = userKode;

                user.Дата_и_время_последнего_входа = DateTime.Now;
                user.IP_Адрес = localIPv4.ToString();

                history.Код_пользователя = user.Код_пользователя;
                history.Дата_и_время_входа = DateTime.Now;
                history.IP_адрес = localIPv4.ToString();
                bd.История.Add(history);
                bd.SaveChanges();
            }
        }

        private void passwordV_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (passwordH.Password.Length != passwordV.Text.Length)
            {
                passwordH.Password = passwordV.Text;
                passwordV.Focus();
            }
        }

        private void passwordH_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (passwordH.Password.Length != passwordV.Text.Length)
            {
                passwordV.Text = passwordH.Password;
                passwordH.Focus();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!passwordVisability)
            {
                passwordV.Visibility = Visibility.Visible;
                passwordH.Visibility = Visibility.Collapsed;
                passwordVisability = true;
                
            }
            else
            {
                passwordV.Visibility = Visibility.Collapsed;
                passwordH.Visibility = Visibility.Visible;
                passwordVisability = false;
            }

        }
    }
}
