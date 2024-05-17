﻿using System;
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
using static MedLaboratory.Autorisation;

namespace MedLaboratory
{
    /// <summary>
    /// Логика взаимодействия для Administrator.xaml
    /// </summary>
    public partial class Administrator : Window
    {
        public Administrator()
        {
            InitializeComponent();
            Initial();
        }
        public bool closeApp = true;

        private void Initial()
        {
            try
            {
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
                                us in bd.Услуги_заказа on z.Код_заказа equals us.Код_заказа
                                join
                                usl in bd.Услуга on us.Код_услуг equals usl.Код_услуги
                                join
                                stat in bd.Статус on us.Код_статуса_услуги equals stat.Код_статуса
                                join
                                sotr in bd.Пользователи on us.Код_сотрудника equals sotr.Код_пользователя
                                select new
                                {
                                    z.Код_заказа,
                                    us.Код_услуги_заказа,
                                    z.Код_пациент,
                                    Услуга = usl.Наименование,
                                    Статаус = stat.Наименование,
                                    us.Дата_и_время_выполнения,
                                    sotr.Фамилия,
                                    sotr.Имя,
                                    sotr.Отчество,
                                    us.Результат,
                                    us.Среднее_отклонение,
                                };

                    dgrid.ItemsSource = zakaz.ToList();
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
            LoginHistory lh = new LoginHistory();
            lh.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (dgrid.SelectedIndex < 0)
            {
                MessageBox.Show("Строка не была выбрана");
                return;
            }
            DataGridRow row = (DataGridRow)dgrid.ItemContainerGenerator.ContainerFromIndex(dgrid.SelectedIndex);
            DataGridCell cell = dgrid.Columns[1].GetCellContent(row).Parent as DataGridCell;
            userData.idOrder = Convert.ToInt32(((TextBlock)cell.Content).Text);
            MaterialСonsumptionMain co = new MaterialСonsumptionMain();
            co.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (dgrid.SelectedIndex < 0)
            {
                MessageBox.Show("Строка не была выбрана");
                return;
            }
            DataGridRow row = (DataGridRow)dgrid.ItemContainerGenerator.ContainerFromIndex(dgrid.SelectedIndex);
            DataGridCell cell = dgrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
            userData.idOrder = Convert.ToInt32(((TextBlock)cell.Content).Text);
            userData.idReport = 3;
            MultiReport co = new MultiReport();
            co.ShowDialog();
        }
    }
}