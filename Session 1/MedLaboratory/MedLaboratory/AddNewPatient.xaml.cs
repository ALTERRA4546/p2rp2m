using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace MedLaboratory
{
    /// <summary>
    /// Логика взаимодействия для AddNewPatient.xaml
    /// </summary>
    public partial class AddNewPatient : Window
    {
        public AddNewPatient()
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
                var type = bd.Тип_страхового_полиса.Select(s=>s.Наименование);
                var company = bd.Страховая_компания.Select(s=>s.Название);

                typestrah.ItemsSource = type.ToList();
                strahcompany.ItemsSource = company.ToList();
            }
        }

        private void enterBio_Click(object sender, RoutedEventArgs e)
        {
            if (famaly.Text == "" || name.Text == "" || lastname.Text == "" || birthday.SelectedDate.Value == null || series.Text == "" || number.Text == "" || phone.Text == "" || email.Text == "" || nomstah.Text == "")
            {
                MessageBox.Show("Не все данные введенны");
                return;
            }
            if (!int.TryParse(series.Text, out _) || !int.TryParse(nomstah.Text, out _) || !int.TryParse(number.Text, out _))
            {
                MessageBox.Show("Данные введены в неверном формате");
                return;
            }

            using (var bd = new MedLaboratoryEntities())
            {
                var user = new Пользователи();
                user.Фамилия = famaly.Text;
                user.Имя = name.Text;
                user.Отчество = lastname.Text;
                bd.Пользователи.Add(user);
                bd.SaveChanges();
                var kodeuser = bd.Пользователи.OrderByDescending(d=>d.Код_пользователя).FirstOrDefault();

                var detals = new Другое();
                detals.Код_пользователя = kodeuser.Код_пользователя;
                detals.Серия_паспорта = Convert.ToInt32(series.Text);
                detals.Номер_паспорта = Convert.ToInt32(number.Text);
                detals.Телефон = phone.Text;
                detals.E_mail = email.Text;
                detals.Номер_страхового_полиса = Convert.ToInt32(nomstah.Text);
                var kodetype = bd.Тип_страхового_полиса.Where(d => d.Наименование == typestrah.SelectedItem.ToString()).FirstOrDefault();
                detals.Код_типа_страхового_полиса = kodetype.Код_типа_страхового_полиса;
                var kodestarh = bd.Страховая_компания.Where(d => d.Название == strahcompany.SelectedItem.ToString()).FirstOrDefault();
                detals.Код_страховой_компании = kodestarh.Код_страховой_компании;
                detals.Дата_рождения = birthday.SelectedDate.Value;
                bd.Другое.Add(detals);
                bd.SaveChanges();
            }
        }

    }
}
