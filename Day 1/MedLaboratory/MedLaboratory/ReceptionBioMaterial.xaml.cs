using System;
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
using System.Windows.Shapes;
using static MedLaboratory.Autorisation;

namespace MedLaboratory
{
    /// <summary>
    /// Логика взаимодействия для ReceptionBioMaterial.xaml
    /// </summary>
    public partial class ReceptionBioMaterial : Window
    {
        public ReceptionBioMaterial()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Initial();
        }

        private void Initial()
        {
            using (var bd = new MedLaboratoryEntities())
            {
                var user = from p in bd.Пользователи
                           join
                           d in bd.Другое on p.Код_пользователя equals d.Код_пользователя
                           join
                           t in bd.Тип_страхового_полиса on d.Код_типа_страхового_полиса equals t.Код_типа_страхового_полиса
                           join
                           c in bd.Страховая_компания on d.Код_страховой_компании equals c.Код_страховой_компании
                           select new
                           {
                               p.Код_пользователя,
                               p.Фамилия,
                               p.Имя,
                               p.Отчество,
                               d.Серия_паспорта,
                               d.Номер_паспорта,
                               d.Телефон,
                               d.E_mail,
                               d.Номер_страхового_полиса,
                               Тип_страховой = t.Наименование,
                               Страховая_компания = c.Название
                           };
                var uslugi = from z in bd.Заказ
                             join
                             us in bd.Услуги_заказа on z.Код_заказа equals us.Код_заказа
                             join
                             usl in bd.Услуга on us.Код_услуг equals usl.Код_услуги
                             where (z.Код_заказа == userData.idOrder)
                             select new
                             { 
                                z.Код_заказа,
                                us.Код_услуг,
                                Услуга = usl.Наименование,
                                us.Дата_и_время_выполнения
                             };

                var suser = from z in bd.Заказ
                            join
                            p in bd.Пользователи on z.Код_пациент equals p.Код_пользователя
                            where (z.Код_заказа == userData.idOrder)
                            select new
                            {
                                p.Код_пользователя,
                                p.Фамилия,
                                p.Имя,
                                p.Отчество
                            };

                dgridUser.ItemsSource = user.ToList();
                dgridUslug.ItemsSource = uslugi.ToList();
                selectUser.IsReadOnly = true;
                if (suser.FirstOrDefault() != null)
                {
                    selectUser.Text = suser.FirstOrDefault().Код_пользователя + " " + suser.FirstOrDefault().Фамилия + " " + suser.FirstOrDefault().Имя + " " + suser.FirstOrDefault().Отчество;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddNewPatient ap = new AddNewPatient();
            ap.ShowDialog();
            Initial();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var bd = new MedLaboratoryEntities())
            {
                if (selectUser.Text == "")
                {
                    MessageBox.Show("Выбирите пользователя");
                    return;
                }
                if (userData.idOrder == -1)
                {
                    string[] temp = selectUser.Text.Split(' ');
                    var zakaz = new Заказ();
                    zakaz.Код_пациент = Convert.ToInt32(temp[0]);
                    zakaz.Статус_заказа = 1;
                    bd.Заказ.Add(zakaz);
                    bd.SaveChanges();
                    var kode = bd.Заказ.OrderByDescending(d => d.Код_заказа).FirstOrDefault();
                    userData.idOrder = kode.Код_заказа;
                    SelectServices ss = new SelectServices();
                    ss.ShowDialog();
                }
                else
                {
                    SelectServices ss = new SelectServices();
                    ss.ShowDialog();
                }
                Initial();
            }    
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (dgridUslug.SelectedIndex < 0)
            {
                MessageBox.Show("Строка не была выбрана");
                return;
            }
            DataGridRow row = (DataGridRow)dgridUslug.ItemContainerGenerator.ContainerFromIndex(dgridUslug.SelectedIndex);
            DataGridCell cell = dgridUslug.Columns[1].GetCellContent(row).Parent as DataGridCell;
            userData.idOrder = Convert.ToInt32(((TextBlock)cell.Content).Text);
            AddBioMaterial ab = new AddBioMaterial();
            ab.ShowDialog();
        }

        private void dgridUslug_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGridRow row = (DataGridRow)dgridUser.ItemContainerGenerator.ContainerFromIndex(dgridUser.SelectedIndex);
            DataGridCell cell = dgridUser.Columns[0].GetCellContent(row).Parent as DataGridCell;
            DataGridCell cell1 = dgridUser.Columns[1].GetCellContent(row).Parent as DataGridCell;
            DataGridCell cell2 = dgridUser.Columns[2].GetCellContent(row).Parent as DataGridCell;
            DataGridCell cell3 = dgridUser.Columns[3].GetCellContent(row).Parent as DataGridCell;
            string id = ((TextBlock)cell.Content).Text;
            string f = ((TextBlock)cell1.Content).Text;
            string n = ((TextBlock)cell2.Content).Text;
            string o = ((TextBlock)cell3.Content).Text;
            selectUser.Text = id + " " + f + " " + n + " " + o;
        }

        private void findUser_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter) 
            {
                using (var bd = new MedLaboratoryEntities())
                {
                    var user = from p in bd.Пользователи
                               join
                               d in bd.Другое on p.Код_пользователя equals d.Код_пользователя
                               join
                               t in bd.Тип_страхового_полиса on d.Код_типа_страхового_полиса equals t.Код_типа_страхового_полиса
                               join
                               c in bd.Страховая_компания on d.Код_страховой_компании equals c.Код_страховой_компании
                               where (findUser.Text == null || p.Фамилия.Contains(findUser.Text) || p.Имя.Contains(findUser.Text) || p.Отчество.Contains(findUser.Text))
                               select new
                               {
                                   p.Код_пользователя,
                                   p.Фамилия,
                                   p.Имя,
                                   p.Отчество,
                                   d.Серия_паспорта,
                                   d.Номер_паспорта,
                                   d.Телефон,
                                   d.E_mail,
                                   d.Номер_страхового_полиса,
                                   Тип_страховой = t.Наименование,
                                   Страховая_компания = c.Название
                               };
                    dgridUser.ItemsSource = user.ToList();
                }
            }
        }
    }
}
