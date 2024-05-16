using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using MedLaboratory;
using System;
using System.Data.SqlClient;


namespace UnitTest
{
    [TestClass]
    public class GenerationCapcha
    {
        [TestMethod]
        public void GenerateTextCapcha()
        {
            Capcha capcha = new Capcha();
            string checkKode = capcha.GenerateRandomText();
            Assert.IsTrue(checkKode != null);
        }

        [TestMethod]
        public void DrawCaptchaText()
        {
            Capcha capcha = new Capcha();
            var captchaImage = new Bitmap(380, 100);
            var graphics = Graphics.FromImage(captchaImage);
            capcha.DrawCaptchaText(graphics, captchaImage);
            Assert.IsTrue(graphics.TextContrast > 0);
        }

        [TestMethod]
        public void AddCaptchaNoise()
        {
            Capcha capcha = new Capcha();
            var captchaImage = new Bitmap(380, 100);
            var graphics = Graphics.FromImage(captchaImage);
            capcha.AddNoise(graphics, captchaImage);
            Assert.IsTrue(graphics.TextContrast > 0);
        }

        [TestMethod]
        public void BitmapToImageSource()
        {
            Capcha capcha = new Capcha();
            var captchaImage = new Bitmap(380, 100);
            var graphics = Graphics.FromImage(captchaImage);
            string captchaText = capcha.GenerateRandomText();
            capcha.DrawCaptchaText(graphics, captchaImage);
            var checkData = capcha.BitmapToImageSource(captchaImage);
            Assert.IsTrue(checkData.StreamSource != null);
        }
    }

    [TestClass]
    public class TestDataBase
    {
        [TestMethod]
        public void CheckData()
        {
            string connectionString = "Data Source=DESKTOP-09DGVTM\\SQLEXPRESS;Initial Catalog=MedLaboratory;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Пользователи", connection);
                int rowCount = (int)command.ExecuteScalar();

                Assert.AreEqual(rowCount, 8);
            }
        }
    }
}
