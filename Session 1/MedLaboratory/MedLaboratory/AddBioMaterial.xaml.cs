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
using ZXing;
using static System.Net.Mime.MediaTypeNames;
using iTextSharp.text.pdf;
using iTextSharp.text;

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
                    checkBio.Код_пробирки = Convert.ToInt64(kodetest.Text);
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
                    biomaterial.Код_пробирки = Convert.ToUInt32(kodetest.Text);
                    biomaterial.Количество = Convert.ToInt32(kolvo.Text);
                    biomaterial.Время_сдачи = DateTime.Now;
                    bd.Сданный_биоматериал.Add(biomaterial);
                    bd.SaveChanges();
                }
                MessageBox.Show("Данные внесенны");
                this.Close();
            }
        }

        private void GenKode(string code)
        {
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.CODABAR;
            writer.Options.Height = 150;
            writer.Options.Width = 500;
            Bitmap barcodeImage = writer.Write(code);
            barcodeImage.Save("barcode.png", ImageFormat.Png);

            Document document = new Document();
            PdfWriter.GetInstance(document, new FileStream("barckode.pdf", FileMode.Create));
            document.Open();
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(barcodeImage, System.Drawing.Imaging.ImageFormat.Png);
            document.Add(image);
            document.Close();

            byte[] byteArray = File.ReadAllBytes("barcode.png");
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(byteArray);
            bitmapImage.EndInit();

            barcode.Source = bitmapImage;
            File.Delete("barcode.png");
        }

        private void kodetest_LostFocus(object sender, RoutedEventArgs e)
        {
            if (kodetest.Text.Length < 0)
            {
                MessageBox.Show("Баркод должен состоять из 13 символов");
                return;
            }
            GenKode(kodetest.Text);
        }
    }
}
