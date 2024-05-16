using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
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

namespace MedLaboratory
{
    /// <summary>
    /// Логика взаимодействия для ReceptionBioMaterial.xaml
    /// </summary>
    public partial class ReceptionBioMaterial : Window
    {
        public ReceptionBioMaterial()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Initial();
        }

        private void Initial()
        {
            try
            {
                using (var bd = new MedLaboratoryEntities())
                {
                    var user = from p in bd.Пользователи
                               join
                               d in bd.Другое on p.Код_пользователя equals d.Код_пользователя
                               join
                               t in bd.Тип_страхового_полиса on d.Код_типа_страхового_полиса equals t.Код_типа_страхового_полиса
                               join
                               c in bd.Страховая_компания on d.Код_страховой_компании equals c.Код_страховой_компании
                               select new
                               {
                                   p.Код_пользователя,
                                   p.Фамилия,
                                   p.Имя,
                                   p.Отчество,
                                   d.Серия_паспорта,
                                   d.Номер_паспорта,
                                   d.Телефон,
                                   d.E_mail,
                                   d.Номер_страхового_полиса,
                                   Тип_страховой = t.Наименование,
                                   Страховая_компания = c.Название
                               };
                    var uslugi = from z in bd.Заказ
                                 join
                                 us in bd.Услуги_заказа on z.Код_заказа equals us.Код_заказа
                                 join
                                 usl in bd.Услуга on us.Код_услуг equals usl.Код_услуги
                                 where (z.Код_заказа == userData.idOrder)
                                 select new
                                 {
                                     z.Код_заказа,
                                     us.Код_услуг,
                                     Услуга = usl.Наименование,
                                     us.Дата_и_время_выполнения
                                 };

                    var suser = from z in bd.Заказ
                                join
                                p in bd.Пользователи on z.Код_пациент equals p.Код_пользователя
                                where (z.Код_заказа == userData.idOrder)
                                select new
                                {
                                    p.Код_пользователя,
                                    p.Фамилия,
                                    p.Имя,
                                    p.Отчество
                                };

                    dgridUser.ItemsSource = user.ToList();
                    dgridUslug.ItemsSource = uslugi.ToList();
                    selectUser.IsReadOnly = true;
                    if (suser.FirstOrDefault() != null)
                    {
                        selectUser.Text = suser.FirstOrDefault().Код_пользователя + " " + suser.FirstOrDefault().Фамилия + " " + suser.FirstOrDefault().Имя + " " + suser.FirstOrDefault().Отчество;
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
            userData.idPosetitel = -1;
            AddNewPatient ap = new AddNewPatient();
            ap.ShowDialog();
            Initial();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var bd = new MedLaboratoryEntities())
                {
                    if (selectUser.Text == "")
                    {
                        MessageBox.Show("Выбирите пользователя");
                        return;
                    }
                    if (userData.idOrder == -1)
                    {
                        string[] temp = selectUser.Text.Split(' ');
                        var zakaz = new Заказ();
                        zakaz.Код_пациент = Convert.ToInt32(temp[0]);
                        zakaz.Статус_заказа = 1;
                        zakaz.Время_заказа = DateTime.Now;
                        bd.Заказ.Add(zakaz);
                        bd.SaveChanges();
                        var kode = bd.Заказ.OrderByDescending(d => d.Код_заказа).FirstOrDefault();
                        userData.idOrder = kode.Код_заказа;
                        SelectServices ss = new SelectServices();
                        ss.ShowDialog();
                    }
                    else
                    {
                        SelectServices ss = new SelectServices();
                        ss.ShowDialog();
                    }
                    Initial();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (dgridUslug.SelectedIndex < 0)
            {
                MessageBox.Show("Строка не была выбрана");
                return;
            }
            DataGridRow row = (DataGridRow)dgridUslug.ItemContainerGenerator.ContainerFromIndex(dgridUslug.SelectedIndex);
            DataGridCell cell = dgridUslug.Columns[1].GetCellContent(row).Parent as DataGridCell;
            userData.idOrder = Convert.ToInt32(((TextBlock)cell.Content).Text);
            AddBioMaterial ab = new AddBioMaterial();
            ab.ShowDialog();
        }

        private void findUser_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    using (var bd = new MedLaboratoryEntities())
                    {
                        var user = from p in bd.Пользователи
                                   join
                                   d in bd.Другое on p.Код_пользователя equals d.Код_пользователя
                                   join
                                   t in bd.Тип_страхового_полиса on d.Код_типа_страхового_полиса equals t.Код_типа_страхового_полиса
                                   join
                                   c in bd.Страховая_компания on d.Код_страховой_компании equals c.Код_страховой_компании
                                   select new
                                   {
                                       p.Код_пользователя,
                                       p.Фамилия,
                                       p.Имя,
                                       p.Отчество,
                                       d.Серия_паспорта,
                                       d.Номер_паспорта,
                                       d.Телефон,
                                       d.E_mail,
                                       d.Номер_страхового_полиса,
                                       Тип_страховой = t.Наименование,
                                       Страховая_компания = c.Название
                                   };
                        dgridUser.ItemsSource = user.ToList().Where(p => findUser.Text == null || LevenshteinDistance(p.Фамилия, findUser.Text) <= 3 || LevenshteinDistance(p.Имя, findUser.Text) <= 3 || LevenshteinDistance(p.Отчество, findUser.Text) <= 3);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        int LevenshteinDistance(string str1, string str2)
        {
            var matrix = new int[str1.Length + 1, str2.Length + 1];

            for (var i = 0; i <= str1.Length; i++)
            {
                matrix[i, 0] = i;
            }

            for (var j = 0; j <= str2.Length; j++)
            {
                matrix[0, j] = j;
            }

            for (var i = 1; i <= str1.Length; i++)
            {
                for (var j = 1; j <= str2.Length; j++)
                {
                    var cost = str1[i - 1] == str2[j - 1] ? 0 : 1;
                    matrix[i, j] = Math.Min(
                        matrix[i - 1, j] + 1,
                        Math.Min(
                            matrix[i, j - 1] + 1,
                            matrix[i - 1, j - 1] + cost
                        )
                    );
                }
            }

            return matrix[str1.Length, str2.Length];
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                using (var bd = new MedLaboratoryEntities())
                {
                    if (selectUser.Text != "")
                    {
                        var zakaz = bd.Заказ.Where(w => w.Код_заказа == userData.idOrder).FirstOrDefault();
                        string[] temp = selectUser.Text.Split(' ');
                        zakaz.Код_пациент = Convert.ToInt32(temp[0]);
                        bd.SaveChanges();

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

                        MessageBox.Show("Данные сохранены");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (dgridUser.SelectedIndex < 0)
            {
                MessageBox.Show("Строка не была выбрана");
                return;
            }
            DataGridRow row = (DataGridRow)dgridUser.ItemContainerGenerator.ContainerFromIndex(dgridUser.SelectedIndex);
            DataGridCell cell = dgridUser.Columns[0].GetCellContent(row).Parent as DataGridCell;
            userData.idPosetitel = Convert.ToInt32(((TextBlock)cell.Content).Text);
            AddNewPatient ap = new AddNewPatient();
            ap.ShowDialog();
            Initial();
        }

        private void dgridUser_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgridUser.SelectedItem != null)
            {
                DataGridRow row = (DataGridRow)dgridUser.ItemContainerGenerator.ContainerFromIndex(dgridUser.SelectedIndex);
                DataGridCell cell = dgridUser.Columns[0].GetCellContent(row).Parent as DataGridCell;
                DataGridCell cell1 = dgridUser.Columns[1].GetCellContent(row).Parent as DataGridCell;
                DataGridCell cell2 = dgridUser.Columns[2].GetCellContent(row).Parent as DataGridCell;
                DataGridCell cell3 = dgridUser.Columns[3].GetCellContent(row).Parent as DataGridCell;
                string id = ((TextBlock)cell.Content).Text;
                string f = ((TextBlock)cell1.Content).Text;
                string n = ((TextBlock)cell2.Content).Text;
                string o = ((TextBlock)cell3.Content).Text;
                selectUser.Text = id + " " + f + " " + n + " " + o;
            }
        }
    }
}
