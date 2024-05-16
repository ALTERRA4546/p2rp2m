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
    /// Логика взаимодействия для MaterialСonsumptionMain.xaml
    /// </summary>
    public partial class MaterialСonsumptionMain : Window
    {
        public MaterialСonsumptionMain()
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
                    var zakaz = from ismast in bd.Используемый_материал
                                join
                                mat in bd.Материал on ismast.Код_материала equals mat.Код_материала
                                where (ismast.Код_услуги_заказа == userData.idOrder)
                                select new
                                {
                                    ismast.Код_используемого_материала,
                                    ismast.Код_услуги_заказа,
                                    Материал = mat.Наименование,
                                    ismast.Количество
                                };

                    dgrid.ItemsSource = zakaz.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            userData.idIspolMater = Convert.ToInt32(((TextBlock)cell.Content).Text);
            MaterialСonsumption co = new MaterialСonsumption();
            co.ShowDialog();
            Initial();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            dgrid.SelectedItem = null;
            userData.idIspolMater = -1;
            MaterialСonsumption co = new MaterialСonsumption();
            co.ShowDialog();
            Initial();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgrid.SelectedIndex < 0)
                {
                    MessageBox.Show("Строка не была выбрана");
                    return;
                }
                DataGridRow row = (DataGridRow)dgrid.ItemContainerGenerator.ContainerFromIndex(dgrid.SelectedIndex);
                DataGridCell cell = dgrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                int kodedel = Convert.ToInt32(((TextBlock)cell.Content).Text);

                if (MessageBox.Show("Вы хотите удалить запись", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    using (var bd = new MedLaboratoryEntities())
                    {
                        var del = bd.Используемый_материал.Where(w => w.Код_используемого_материала == kodedel).FirstOrDefault();
                        bd.Используемый_материал.Remove(del);
                        bd.SaveChanges();
                    }
                    Initial();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
