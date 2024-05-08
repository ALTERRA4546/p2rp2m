using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
using System.Drawing.Printing;

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
            GenKode();
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

        private void GenKode()
        {
            raphPane myPane = new GraphPane();

            // Добавляем пустой график
            myPane.CurveList.Add(new LineItem());

            // Создаем объект Barcode
            Barcode barcode = new Barcode(Barcode.TYPE.CODE128, "1234567890");

            // Устанавливаем параметры ограничителей
            barcode.Margins.Left = 10;
            barcode.Margins.Right = 10;
            barcode.Margins.Top = 10;
            barcode.Margins.Bottom = 10;

            // Устанавливаем параметры разделителя
            barcode.Label = "1234567890";
            barcode.LabelFont = new Font("Arial", 12);

            // Добавляем штрихкод к графику
            myPane.AddBarcodeItem(barcode, 0, 0);

            // Устанавливаем размер контрола ZedGraphControl
            barcode.Size = new System.Drawing.Size(400, 100);

            // Отображаем штрихкод в контроле ZedGraphControl
            barcode.GraphPane = myPane;
        }

    }
}
