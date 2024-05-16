using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using static MedLaboratory.Autorisation;

namespace MedLaboratory
{
    /// <summary>
    /// Логика взаимодействия для InsuranceАccount.xaml
    /// </summary>
    public partial class InsuranceАccount : Window
    {
        public InsuranceАccount()
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
                    var zakaz = from z in bd.Заказ
                                join
                                us in bd.Услуги_заказа on z.Код_заказа equals us.Код_заказа
                                join
                                uslg in bd.Услуга on us.Код_услуг equals uslg.Код_услуги
                                where (z.Код_заказа == userData.idOrder)
                                select new
                                {
                                    us.Код_услуг,
                                    Услуга = uslg.Наименование,
                                    uslg.Стоимость
                                };

                    dgrid.ItemsSource = zakaz.ToList();

                    summa.Text = zakaz.ToList().Sum(s => s.Стоимость).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var bd = new MedLaboratoryEntities())
                {
                    if (!decimal.TryParse(summa.Text, out _))
                    {
                        MessageBox.Show("Данные введенны неверно");
                        return;
                    }
                    var accoun = bd.Счет_страховой.Where(a => a.Код_заказа == userData.idOrder).FirstOrDefault();
                    if (accoun != null)
                    {
                        accoun.Сумма = Convert.ToDecimal(summa.Text);
                        accoun.Начало_периода_оплаты = DateTime.Now.Date;
                        accoun.Окночание_переода_оплаты = DateTime.Now.AddDays(15).Date;
                        bd.SaveChanges();
                    }
                    else
                    {
                        var newAccoun = new Счет_страховой();
                        newAccoun.Код_бухгалтера = userData.idUser;
                        newAccoun.Код_заказа = userData.idOrder;
                        newAccoun.Сумма = Convert.ToDecimal(summa.Text);
                        newAccoun.Начало_периода_оплаты = DateTime.Now.Date;
                        newAccoun.Окночание_переода_оплаты = DateTime.Now.AddDays(15).Date;
                        bd.Счет_страховой.Add(newAccoun);
                        bd.SaveChanges();
                    }
                    MessageBox.Show("Данные внесены");
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
