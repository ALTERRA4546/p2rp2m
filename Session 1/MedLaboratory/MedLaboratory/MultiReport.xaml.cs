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
using ZXing.OneD;

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
                bDate.Visibility = Visibility.Collapsed;
                eDate.Visibility = Visibility.Collapsed;
                sum.Visibility = Visibility.Collapsed;
                this.Height = 520;
                rg.Height = 450;

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
                        var report1 = from st in bd.Счет_страховой
                                      join
                                      z in bd.Заказ on st.Код_заказа equals z.Код_заказа
                                      join
                                      usl in bd.Услуги_заказа on z.Код_заказа equals usl.Код_заказа
                                      join
                                      uslug in bd.Услуга on usl.Код_услуг equals uslug.Код_услуги
                                      join
                                      paset in bd.Пользователи on z.Код_пациент equals paset.Код_пользователя
                                      join
                                      more in bd.Другое on paset.Код_пользователя equals more.Код_пользователя
                                      join
                                      stcom in bd.Страховая_компания on more.Код_страховой_компании equals stcom.Код_страховой_компании
                                      where (bDate.SelectedDate.Value == null || eDate.SelectedDate.Value == null || st.Начало_периода_оплаты.Value >= bDate.SelectedDate.Value && st.Окночание_переода_оплаты <= eDate.SelectedDate.Value)
                                      select new
                                      { 
                                          Страховая_компания = stcom.Название,
                                          Период_оплаты = st.Начало_периода_оплаты + " - " + st.Окночание_переода_оплаты,
                                          ФИО = paset.Фамилия + " " + paset.Имя + " " + paset.Отчество,
                                          Услуга = uslug.Наименование,
                                          uslug.Стоимость
                                      };

                        bDate.Visibility = Visibility.Visible;
                        eDate.Visibility = Visibility.Visible;
                        sum.Visibility = Visibility.Visible;
                        this.Height = 580;
                        rg.Height = 520;
                        double summ = Convert.ToDouble(report1.Sum(s => s.Стоимость));
                        sum.Content = "Итоговая стоимость = " + summ;

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
                        var report1 = from st in bd.Счет_страховой
                                      join
                                      z in bd.Заказ on st.Код_заказа equals z.Код_заказа
                                      join
                                      usl in bd.Услуги_заказа on z.Код_заказа equals usl.Код_заказа
                                      join
                                      uslug in bd.Услуга on usl.Код_услуг equals uslug.Код_услуги
                                      join
                                      paset in bd.Пользователи on z.Код_пациент equals paset.Код_пользователя
                                      join
                                      more in bd.Другое on paset.Код_пользователя equals more.Код_пользователя
                                      join
                                      stcom in bd.Страховая_компания on more.Код_страховой_компании equals stcom.Код_страховой_компании
                                      where (bDate.SelectedDate.Value == null || eDate.SelectedDate.Value == null || st.Начало_периода_оплаты.Value >= bDate.SelectedDate.Value && st.Окночание_переода_оплаты <= eDate.SelectedDate.Value)
                                      select new
                                      {
                                          Страховая_компания = stcom.Название,
                                          Период_оплаты = st.Начало_периода_оплаты + " - " + st.Окночание_переода_оплаты,
                                          ФИО = paset.Фамилия + " " + paset.Имя + " " + paset.Отчество,
                                          Услуга = uslug.Наименование,
                                          uslug.Стоимость
                                      };

                        double summ = Convert.ToDouble(report1.Sum(s => s.Стоимость));

                        Document doc1 = new Document();
                        PdfWriter writer1 = PdfWriter.GetInstance(doc1, new FileStream($@"Страховая компания {DateTime.Now}.pdf".Replace(':', '_'), FileMode.Create));
                        doc1.SetPageSize(PageSize.A4.Rotate());

                        BaseFont baseFont1 = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                        Font font1 = new Font(baseFont1, 12);

                        doc1.Open();
                        PdfPTable table1 = new PdfPTable(5);

                        table1.AddCell(new Paragraph("Страховая компания", font1));
                        table1.AddCell(new Paragraph("Период оплаты", font1));
                        table1.AddCell(new Paragraph("ФИО", font1));
                        table1.AddCell(new Paragraph("Услуги", font1));
                        table1.AddCell(new Paragraph("Стоимость", font1));

                        foreach (var item in report1)
                        {
                            table1.AddCell(new Paragraph(item.Страховая_компания.ToString(), font1));
                            table1.AddCell(new Paragraph(item.Период_оплаты.ToString(), font1));
                            table1.AddCell(new Paragraph(item.ФИО.ToString(), font1));
                            table1.AddCell(new Paragraph(item.Услуга.ToString(), font1));
                            table1.AddCell(new Paragraph(item.Стоимость.ToString(), font1));
                        }

                        doc1.Add(table1);
                        Paragraph paragraph = new Paragraph($"Итоговая стоимость : {summ}", font1);
                        doc1.Add(paragraph);
                        doc1.Close();

                        var file = new StreamWriter($"Страховая компания {DateTime.Now}.csv".Replace(':', '_'), false, Encoding.UTF8);
                        foreach (var line in report1)
                        {
                            file.WriteLine(line);
                        }
                        file.Flush();
                
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

        private void bDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Initial();
        }
    }
}
