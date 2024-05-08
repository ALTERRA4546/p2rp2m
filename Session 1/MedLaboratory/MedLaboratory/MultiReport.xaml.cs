using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static MedLaboratory.Autorisation;
using System.Xml.Linq;

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
                                     join
                                     pol in bd.Пользователи on z.Код_пациент equals pol.Код_пользователя
                                     join
                                     dop in bd.Другое on pol.Код_пользователя equals dop.Код_пользователя
                                     where (z.Код_заказа == userData.idOrder)
                                     select new
                                     {
                                         Дата_заказа = z.Время_заказа,
                                         z.Код_заказа,
                                         bio.Код_пробирки,
                                         dop.Номер_страхового_полиса,
                                         ФИО = pol.Фамилия + " " + pol.Имя + " " + pol.Отчество,
                                         dop.Дата_рождения,
                                         Услуга = uslug.Наименование,
                                         uslug.Стоимость
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
                                     join
                                     pol in bd.Пользователи on z.Код_пациент equals pol.Код_пользователя
                                     join
                                     dop in bd.Другое on pol.Код_пользователя equals dop.Код_пользователя
                                     where (z.Код_заказа == userData.idOrder)
                                     select new
                                     {
                                        Дата_заказа = z.Время_заказа,
                                        z.Код_заказа,
                                        bio.Код_пробирки,
                                        dop.Номер_страхового_полиса,
                                        ФИО = pol.Фамилия + " " + pol.Имя + " " + pol.Отчество,
                                        dop.Дата_рождения,
                                        Услуга = uslug.Наименование,
                                        uslug.Стоимость
                                     };

                        Document doc = new Document();
                        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream($@"{DateTime.Now}.pdf".Replace(':', '_'), FileMode.Create));
                        doc.SetPageSize(PageSize.A4.Rotate());

                        BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                        Font font = new Font(baseFont, 12);

                        doc.Open();
                        PdfPTable table = new PdfPTable(8);

                        table.AddCell(new Paragraph("Дата заказа", font));
                        table.AddCell(new Paragraph("Номер заказа", font));
                        table.AddCell(new Paragraph("Номер пробирки", font));
                        table.AddCell(new Paragraph("Номер страхового полиса", font));
                        table.AddCell(new Paragraph("ФИО", font));
                        table.AddCell(new Paragraph("Дата рождения", font));
                        table.AddCell(new Paragraph("Услуги", font));
                        table.AddCell(new Paragraph("Стоимость", font));

                        foreach (var item in report)
                        {
                            table.AddCell(new Paragraph(item.Дата_заказа.Value.Date.ToString(), font));
                            table.AddCell(new Paragraph(item.Код_заказа.ToString(), font));
                            table.AddCell(new Paragraph(item.Код_пробирки.ToString(), font));
                            table.AddCell(new Paragraph(item.Номер_страхового_полиса.ToString(), font));
                            table.AddCell(new Paragraph(item.ФИО.ToString(), font));
                            table.AddCell(new Paragraph(item.Дата_рождения.ToString(), font));
                            table.AddCell(new Paragraph(item.Услуга.ToString(), font));
                            table.AddCell(new Paragraph(item.Стоимость.ToString(), font));
                        }

                        doc.Add(table);
                        doc.Close();
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

                        break;
                }
                MessageBox.Show("Отчет сохранен в папке программы");

            }
        }
    }
}
