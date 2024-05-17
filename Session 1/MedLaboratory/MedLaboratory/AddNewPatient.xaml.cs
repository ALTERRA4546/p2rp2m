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
using static MedLaboratory.Autorisation;

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

                var potient = from p in bd.Пользователи
                              join
                              det in bd.Другое on p.Код_пользователя equals det.Код_пользователя
                              where (p.Код_пользователя == userData.idPosetitel)
                              select new
                              {
                                  p.Фамилия,
                                  p.Имя,
                                  p.Отчество,
                                  det.Дата_рождения,
                                  det.Серия_паспорта,
                                  det.Номер_паспорта,
                                  det.Телефон,
                                  det.E_mail,
                                  det.Номер_страхового_полиса,
                                  det.Код_типа_страхового_полиса,
                                  det.Код_страховой_компании
                              };
                if (potient.FirstOrDefault() != null)
                {
                    var selecttype = bd.Тип_страхового_полиса.Where(w => w.Код_типа_страхового_полиса == potient.FirstOrDefault().Код_типа_страхового_полиса).Select(s => s.Наименование).FirstOrDefault();
                    var selectcompany = bd.Страховая_компания.Where(w => w.Код_страховой_компании == potient.FirstOrDefault().Код_страховой_компании).Select(s => s.Название).FirstOrDefault();

                    famaly.Text = potient.FirstOrDefault().Фамилия;
                    name.Text = potient.FirstOrDefault().Имя;
                    lastname.Text = potient.FirstOrDefault().Отчество;
                    birthday.SelectedDate = potient.FirstOrDefault().Дата_рождения;
                    series.Text = potient.FirstOrDefault().Серия_паспорта.ToString();
                    number.Text = potient.FirstOrDefault().Номер_паспорта.ToString();
                    phone.Text = potient.FirstOrDefault().Телефон;
                    email.Text = potient.FirstOrDefault().E_mail;
                    nomstah.Text = potient.FirstOrDefault().Номер_страхового_полиса.ToString();
                    typestrah.SelectedItem = selecttype;
                    strahcompany.SelectedItem = selectcompany;
                }
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
                if (userData.idPosetitel <= 0)
                {
                    var user = new Пользователи();
                    user.Фамилия = famaly.Text;
                    user.Имя = name.Text;
                    user.Отчество = lastname.Text;
                    bd.Пользователи.Add(user);
                    bd.SaveChanges();
                    var kodeuser = bd.Пользователи.OrderByDescending(d => d.Код_пользователя).FirstOrDefault();

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
                else
                { 
                    var user = bd.Пользователи.Where(w=>w.Код_пользователя == userData.idPosetitel).FirstOrDefault();
                    user.Фамилия = famaly.Text;
                    user.Имя = name.Text;
                    user.Отчество = lastname.Text;
                    var detals = bd.Другое.Where(w => w.Код_пользователя == userData.idPosetitel).FirstOrDefault();
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
                    bd.SaveChanges();
                }
                MessageBox.Show("Данные сохранены");
                this.Close();
            }
        }

    }
}
