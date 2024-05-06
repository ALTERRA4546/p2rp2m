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
    /// Логика взаимодействия для Analizator.xaml
    /// </summary>
    public partial class Analizator : Window
    {
        public Analizator()
        {
            InitializeComponent();
            Initial();
        }

        private void Initial()
        {
            using (var bd = new MedLaboratoryEntities())
            {
                var analizators = bd.Анализатор.Select(b => b.Наименование).ToList();
                analiz.ItemsSource = analizators;

                var checkAnaliz = bd.Работа_анализатора.Where(b => b.Код_услуги_заказа == userData.idOrder).FirstOrDefault();
                if (checkAnaliz != null)
                {
                    var kodeBio = bd.Анализатор.Where(b => b.Код_анализатора == checkAnaliz.Код_анализатора).FirstOrDefault();
                    analiz.SelectedItem = kodeBio.Наименование;
                }
                var checkUslugi = bd.Услуги_заказа.Where(u => u.Код_услуги_заказа == userData.idOrder).FirstOrDefault();
                result.Text = checkUslugi.Результат.ToString();
                otklon.Text = checkUslugi.Среднее_отклонение.ToString();
            }
        }

        private void enterBio_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(result.Text, out _) || !double.TryParse(otklon.Text, out _))
            {
                MessageBox.Show("Данные внесенны неверно");
                return;
            }

            using (var bd = new MedLaboratoryEntities())
            {
                var checkAnaliz = bd.Работа_анализатора.Where(b => b.Код_услуги_заказа == userData.idOrder).FirstOrDefault();
                if (checkAnaliz != null)
                {
                    var kodeAnaliz = bd.Анализатор.Where(b => b.Наименование == analiz.SelectedItem.ToString()).FirstOrDefault();
                    var checkUslugi = bd.Услуги_заказа.Where(u => u.Код_услуги_заказа == userData.idOrder).FirstOrDefault();
                    checkAnaliz.Код_анализатора = kodeAnaliz.Код_анализатора;
                    checkAnaliz.Дата_и_время_выполнения_услуги = DateTime.Now;

                    checkUslugi.Результат = Convert.ToDouble(result.Text);
                    checkUslugi.Среднее_отклонение = Convert.ToDouble(otklon.Text);

                    bd.SaveChanges();
                }
                else
                {
                    var newAnaliz = new Работа_анализатора();
                    newAnaliz.Код_услуги_заказа = userData.idOrder;
                    var kodeAnaliz = bd.Анализатор.Where(b => b.Наименование == analiz.SelectedItem.ToString()).FirstOrDefault();
                    newAnaliz.Код_анализатора = kodeAnaliz.Код_анализатора;
                    var checkZakaz = bd.Услуги_заказа.Where(b => b.Код_услуги_заказа == userData.idOrder).FirstOrDefault();
                    newAnaliz.Дата_и_время_поступления_заказа = checkZakaz.Дата_и_время_выполнения;
                    newAnaliz.Дата_и_время_выполнения_услуги = DateTime.Now;
                    bd.Работа_анализатора.Add(newAnaliz);
                    
                    checkZakaz.Результат = Convert.ToDouble(result.Text);
                    checkZakaz.Среднее_отклонение = Convert.ToDouble(otklon.Text);
                    bd.SaveChanges();
                }
                MessageBox.Show("Данные внесенны");
                this.Close();
            }
        }
    }
}
