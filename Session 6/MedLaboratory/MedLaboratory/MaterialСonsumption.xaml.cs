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
    /// Логика взаимодействия для MaterialСonsumption.xaml
    /// </summary>
    public partial class MaterialСonsumption : Window
    {
        public MaterialСonsumption()
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
                    var mat = bd.Материал.Select(m => m.Наименование);
                    matrial.ItemsSource = mat.ToList();

                    var checkIspolMater = bd.Используемый_материал.Where(w => w.Код_используемого_материала == userData.idIspolMater).FirstOrDefault();
                    if (checkIspolMater != null)
                    {
                        var kodeMaterial = bd.Материал.Where(w => w.Код_материала == checkIspolMater.Код_материала).FirstOrDefault();
                        matrial.SelectedItem = kodeMaterial.Наименование;
                        kolvo.Text = checkIspolMater.Количество.ToString();
                    }
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
                    if (!int.TryParse(kolvo.Text, out _))
                    {
                        MessageBox.Show("Данные введенны неверно");
                        return;
                    }

                    var checkMater = bd.Используемый_материал.Where(w => w.Код_используемого_материала == userData.idIspolMater).FirstOrDefault();
                    if (checkMater != null)
                    {
                        var kodeMaterial = bd.Материал.Where(w => w.Наименование == matrial.SelectedItem.ToString()).FirstOrDefault();
                        checkMater.Код_материала = kodeMaterial.Код_материала;
                        checkMater.Количество = Convert.ToInt32(kolvo.Text);
                        bd.SaveChanges();
                    }
                    else
                    {
                        var newMaterial = new Используемый_материал();
                        newMaterial.Код_услуги_заказа = userData.idOrder;
                        var kodeMaterial = bd.Материал.Where(w => w.Наименование == matrial.SelectedItem.ToString()).FirstOrDefault();
                        newMaterial.Код_материала = kodeMaterial.Код_материала;
                        newMaterial.Количество = Convert.ToInt32(kolvo.Text);
                        bd.Используемый_материал.Add(newMaterial);
                        bd.SaveChanges();
                    }
                    MessageBox.Show("Данные сохранены");
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
