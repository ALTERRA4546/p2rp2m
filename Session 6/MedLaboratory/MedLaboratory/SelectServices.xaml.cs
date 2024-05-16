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
    /// Логика взаимодействия для SelectServices.xaml
    /// </summary>
    public partial class SelectServices : Window
    {
        public SelectServices()
        {
            InitializeComponent();
            Initial();
        }

        private void Initial()
        {
            try
            {
                using (var bd = new MedLaboratoryEntities())
                {
                    var uslugi = bd.Услуга.Select(w => w.Наименование);
                    selUsl.ItemsSource = uslugi.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void enterBio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var bd = new MedLaboratoryEntities())
                {
                    var usluga = new Услуги_заказа();
                    var kodeuslugi = bd.Услуга.Where(g => g.Наименование == selUsl.SelectedItem.ToString()).FirstOrDefault();
                    usluga.Код_заказа = userData.idOrder;
                    usluga.Код_услуг = kodeuslugi.Код_услуги;
                    usluga.Код_сотрудника = userData.idUser;
                    usluga.Код_статуса_услуги = 1;
                    usluga.Дата_и_время_выполнения = DateTime.Now;
                    bd.Услуги_заказа.Add(usluga);
                    bd.SaveChanges();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
