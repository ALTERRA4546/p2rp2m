using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Windows.Threading;
using static MedLaboratory.Autorisation;

namespace MedLaboratory
{
    /// <summary>
    /// Логика взаимодействия для LaboratoryAssistant.xaml
    /// </summary>
    public partial class LaboratoryAssistant : Window
    {
        private DispatcherTimer timer;
        public int minutes = 30;
        public int hour = 2;

        public LaboratoryAssistant()
        {
            InitializeComponent();
            Initial();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += OnTimedEvent;
            timer.Start();
        }

        private void Initial()
        {
            exitTime.Content = hour + ":" + minutes;
            using (var bd = new MedLaboratoryEntities())
            {
                var user = bd.Пользователи.FirstOrDefault(u=> u.Код_пользователя == userData.idUser);
                byte[] imageData = user.Фотография;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(imageData);
                bitmapImage.EndInit();
                photo.Source = bitmapImage;

                var role = bd.Должность.FirstOrDefault(r=>r.Код_должности == user.Код_должности);

                fio.Content = user.Фамилия + " " + user.Имя + " " + role.Название;
            }
        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            if (hour == 0 && minutes == 0)
            {
                MessageBox.Show("Время доступа закончилось");

                using (var bd = new MedLaboratoryEntities())
                {
                    var autor = bd.Авторизация.FirstOrDefault(au => au.Код_пользователя == userData.idUser);

                    autor.Время_блокировки = DateTime.Now;
                    bd.SaveChanges();
                }

                timer.Stop();
                Autorisation a = new Autorisation();
                a.Show();
                this.Close();
            }

            minutes--;
            if (minutes == 0 && hour != 0)
            { 
                hour--;
                minutes = 60;
            }    

            if(minutes > 10)
                exitTime.Content = hour + ":" + minutes;
            else
                exitTime.Content = hour + ":0" + minutes;


            if (hour == 0 && minutes == 15)
            {
                MessageBox.Show("До окончания сеанса осталось 15 минут");
            }
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Autorisation a = new Autorisation();
            a.Show();
            this.Close();
        }
    }
}
