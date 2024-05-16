using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace MedLaboratory
{
    /// <summary>
    /// Логика взаимодействия для Capcha.xaml
    /// </summary>
    public partial class Capcha : Window
    {
        public Capcha()
        {
            InitializeComponent();
            GenerateCaptcha();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (captchaText == cap.Text)
            {
                userData.checkCapcha = false;
                this.Close();
            }
            else
            {
                MessageBox.Show("Капча введена неврено");
                GenerateCaptcha();
            }
        }

        private readonly Random random = new Random();
        private string captchaText;

        private void GenerateCaptcha()
        {
            try
            {
                var captchaImage = new Bitmap(380, 100);
                var graphics = Graphics.FromImage(captchaImage);
                captchaText = GenerateRandomText();
                DrawCaptchaText(graphics, captchaImage);
                AddNoise(graphics, captchaImage);

                imageCap.Source = BitmapToImageSource(captchaImage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public string GenerateRandomText()
        {
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var text = "";
            for (var i = 0; i < 4; i++)
            {
                text += characters[random.Next(characters.Length)];
            }

            return text;
        }

        public void DrawCaptchaText(Graphics graphics, Bitmap captchaImage)
        {
            var font = new Font("Arial", 30);
            var brush = new SolidBrush(System.Drawing.Color.Red);

            for (var i = 0; i < captchaText.Length; i++)
            {
                var character = captchaText[i].ToString();
                var x = 40 + i * 80;
                var y = random.Next(captchaImage.Height-50);
                var angle = random.Next(-30, 30);

                if (random.Next(2) == 0)
                {
                    graphics.DrawString(character, font, brush, x, y);
                }
                else
                {
                    graphics.TranslateTransform(x, y);
                    graphics.RotateTransform(angle);
                    graphics.DrawString(character, font, brush, 0, 0);
                    graphics.ResetTransform();
                }
            }
        }

        public void AddNoise(Graphics graphics, Bitmap captchaImage)
        {
            var pen = new System.Drawing.Pen(System.Drawing.Color.Gray, 1);
            for (var i = 0; i < 100; i++)
            {
                var x1 = random.Next(captchaImage.Width);
                var y1 = random.Next(captchaImage.Height);
                var x2 = random.Next(captchaImage.Width);
                var y2 = random.Next(captchaImage.Height);
                graphics.DrawLine(pen, x1, y1, x2, y2);
            }

            for (var i = 0; i < 100; i++)
            {
                var x = random.Next(captchaImage.Width);
                var y = random.Next(captchaImage.Height);
                graphics.FillEllipse(System.Drawing.Brushes.Black, x, y, 2, 2);
            }
        }

        public BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (var memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GenerateCaptcha();
        }
    }
}
