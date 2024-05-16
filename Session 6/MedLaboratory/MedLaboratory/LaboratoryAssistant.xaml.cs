using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
        public bool closeApp = true;

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
            try
            {
                exitTime.Content = hour + ":" + minutes;
                using (var bd = new MedLaboratoryEntities())
                {
                    var user = bd.Пользователи.FirstOrDefault(u => u.Код_пользователя == userData.idUser);
                    byte[] imageData = user.Фотография;
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = new MemoryStream(imageData);
                    bitmapImage.EndInit();
                    photo.Source = bitmapImage;

                    var role = bd.Должность.FirstOrDefault(r => r.Код_должности == user.Код_должности);

                    fio.Content = user.Фамилия + " " + user.Имя + " " + role.Название;

                    var zakaz = from z in bd.Заказ
                                join
                                sotr in bd.Пользователи on z.Код_пациент equals sotr.Код_пользователя
                                join
                                stat in bd.Статус on z.Статус_заказа equals stat.Код_статуса
                                select new
                                {
                                    z.Код_заказа,
                                    z.Код_пациент,
                                    Статаус = stat.Наименование,
                                    sotr.Имя,
                                    sotr.Отчество,
                                };

                    dgrid.ItemsSource = zakaz.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            try
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

                if (minutes > 10)
                    exitTime.Content = hour + ":" + minutes;
                else
                    exitTime.Content = hour + ":0" + minutes;


                if (hour == 0 && minutes == 15)
                {
                    MessageBox.Show("До окончания сеанса осталось 15 минут");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Autorisation a = new Autorisation();
            a.Show();
            closeApp = false;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (closeApp)
            {
                Application.Current.Shutdown();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid.SelectedIndex < 0)
            {
                MessageBox.Show("Строка не была выбрана");
                return;
            }
            DataGridRow row = (DataGridRow)dgrid.ItemContainerGenerator.ContainerFromIndex(dgrid.SelectedIndex);
            DataGridCell cell = dgrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
            userData.idOrder = Convert.ToInt32(((TextBlock)cell.Content).Text);
            ReceptionBioMaterial co = new ReceptionBioMaterial();
            co.ShowDialog();
            Initial();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (dgrid.SelectedIndex < 0)
            {
                MessageBox.Show("Строка не была выбрана");
                return;
            }
            DataGridRow row = (DataGridRow)dgrid.ItemContainerGenerator.ContainerFromIndex(dgrid.SelectedIndex);
            DataGridCell cell = dgrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
            userData.idOrder = Convert.ToInt32(((TextBlock)cell.Content).Text);
            userData.idReport = 1;
            MultiReport co = new MultiReport();
            co.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            dgrid.UnselectAll();
            userData.idOrder = -1;
            ReceptionBioMaterial co = new ReceptionBioMaterial();
            co.ShowDialog();
            Initial();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            userData.idReport = 4;
            MultiReport mr = new MultiReport();
            mr.Show();
        }
    }
}
