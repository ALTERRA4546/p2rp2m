using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для MultiReport.xaml
    /// </summary>
    public partial class MultiReport : Window
    {
        public MultiReport()
        {
            InitializeComponent();
            Initial();
        }

        private void Initial()
        {
            using (var bd = new MedLaboratoryEntities())
            {
                switch (userData.idReport)
                { 
                    case 1:
                        var report = from z in bd.Заказ
                                     join
                                     us in bd.Услуги_заказа on z.Код_заказа equals us.Код_заказа
                                     join
                                     uslug in bd.Услуга on us.Код_услуг equals uslug.Код_услуги
                                     join
                                     bio in bd.Сданный_биоматериал on us.Код_услуги_заказа equals bio.Код_услуги_заказа
                                     join
                                     biomat in bd.Биоматериал on bio.Код_биоматериала equals biomat.Код_биоматериала
                                     where (z.Код_заказа == userData.idOrder)
                                     select new
                                     {
                                         us.Код_услуги_заказа,
                                         Услуга = uslug.Наименование,
                                         Биоматериал = biomat.Наименование,
                                         bio.Количество
                                     };
                        dgrid.ItemsSource = report.ToList();
                        break;
                    case 2:
                        var report1 = from z in bd.Заказ
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

                        dgrid.ItemsSource = report1.ToList();

                        break;
                    case 3:
                        var report2 = from z in bd.Заказ
                                      join
                                      us in bd.Услуги_заказа on z.Код_заказа equals us.Код_заказа
                                      join
                                      uslug in bd.Услуга on us.Код_услуг equals uslug.Код_услуги
                                      join
                                      ispmat in bd.Используемый_материал on us.Код_услуги_заказа equals ispmat.Код_услуги_заказа
                                      join
                                      mat in bd.Материал on ispmat.Код_материала equals mat.Код_материала
                                      where (z.Код_заказа == userData.idOrder)
                                      select new
                                      {
                                          us.Код_услуг,
                                          Услуга = uslug.Наименование,
                                          Материал = mat.Наименование,
                                          ispmat.Количество
                                      };
                        dgrid.ItemsSource = report2.ToList();

                        break;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("В разработке");
        }
    }
}
