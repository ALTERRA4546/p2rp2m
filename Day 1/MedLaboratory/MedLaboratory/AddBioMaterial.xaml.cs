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
    /// Логика взаимодействия для AddBioMaterial.xaml
    /// </summary>
    public partial class AddBioMaterial : Window
    {
        public AddBioMaterial()
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
                var biomaterial = bd.Биоматериал.Select(b => b.Наименование).ToList();
                biomat.ItemsSource = biomaterial;

                var checkBio = bd.Сданный_биоматериал.Where(b => b.Код_услуги_заказа == userData.idOrder).FirstOrDefault();
                if (checkBio != null)
                {
                    var kodeBio = bd.Биоматериал.Where(b => b.Код_биоматериала == checkBio.Код_биоматериала).FirstOrDefault();
                    biomat.SelectedItem = kodeBio.Наименование;
                    kolvo.Text = checkBio.Количество.ToString();
                }
            }
        }

        private void enterBio_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(kolvo.Text, out _))
            {
                MessageBox.Show("Количество введенно неверно");
                return;
            }
            using (var bd = new MedLaboratoryEntities())
            {
                var checkBio = bd.Сданный_биоматериал.Where(b => b.Код_услуги_заказа == userData.idOrder).FirstOrDefault();
                if (checkBio != null)
                {
                    var kodeBio = bd.Биоматериал.Where(b => b.Наименование == biomat.SelectedItem.ToString()).FirstOrDefault();
                    checkBio.Код_биоматериала = kodeBio.Код_биоматериала;
                    checkBio.Количество = Convert.ToInt32(kolvo.Text);
                    checkBio.Время_сдачи = DateTime.Now;
                    bd.SaveChanges();
                }
                else
                {
                    var biomaterial = new Сданный_биоматериал();
                    biomaterial.Код_услуги_заказа = userData.idOrder;
                    var kodeBio = bd.Биоматериал.Where(b => b.Наименование == biomat.SelectedItem.ToString()).FirstOrDefault();
                    biomaterial.Код_биоматериала = kodeBio.Код_биоматериала;
                    biomaterial.Количество = Convert.ToInt32(kolvo.Text);
                    biomaterial.Время_сдачи = DateTime.Now;
                    bd.Сданный_биоматериал.Add(biomaterial);
                    bd.SaveChanges();
                }
                MessageBox.Show("Данные внесенны");
                this.Close();
            }
        }

    }
}
