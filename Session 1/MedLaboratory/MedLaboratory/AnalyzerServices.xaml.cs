using Newtonsoft.Json;
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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static MedLaboratory.Autorisation;

namespace MedLaboratory
{
    /// <summary>
    /// Логика взаимодействия для AnalyzerServices.xaml
    /// </summary>
    public partial class AnalyzerServices : Window
    {
        private DispatcherTimer timer;
        public int second = 0;
        public int inter = 0;

        public AnalyzerServices()
        {
            InitializeComponent();
            Initial();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += OnTimedEvent;
        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            if (pb.Value == 100)
            { 
                send.IsEnabled = true;
                timer.Stop();
                MessageBox.Show("Услуга выполнена");
                Initial();
            }
            pb.Value += inter;
            procent.Content = pb.Value + "%";
        }

        private void Initial()
        {
            using (var bd = new MedLaboratoryEntities())
            {
                var uslug = from a in bd.Анализатор
                            join
                            uslg in bd.Услуга on a.Код_услуги equals uslg.Код_услуги
                            join
                            us in bd.Услуги_заказа on uslg.Код_услуги equals us.Код_услуг
                            join
                            bio in bd.Сданный_биоматериал on us.Код_услуги_заказа equals bio.Код_услуги_заказа
                            join
                            biomat in bd.Биоматериал on bio.Код_биоматериала equals biomat.Код_биоматериала
                            join
                            st in bd.Статус on us.Код_статуса_услуги equals st.Код_статуса
                            where (a.Код_анализатора == userData.idAnalizator && us.Код_статуса_услуги == 1)
                            select new
                            { 
                                us.Код_услуги_заказа,
                                uslg.Код_услуги,
                                Услуга = uslg.Наименование,
                                Биоматериал = biomat.Наименование,
                                bio.Количество,
                                Статус = st.Наименование
                            };
                dgrid.ItemsSource = uslug.ToList();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var bd = new MedLaboratoryEntities())
            {
                Random rnd = new Random();
                DataGridRow row = (DataGridRow)dgrid.ItemContainerGenerator.ContainerFromIndex(dgrid.SelectedIndex);
                DataGridCell cell = dgrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                DataGridCell cell1 = dgrid.Columns[1].GetCellContent(row).Parent as DataGridCell;
                userData.idService = Convert.ToInt32(((TextBlock)cell.Content).Text);
                int kodeuslug = Convert.ToInt32(((TextBlock)cell1.Content).Text);

                var analiz = new Работа_анализатора();
                analiz.Код_анализатора = userData.idAnalizator;
                analiz.Код_услуги_заказа = userData.idService;
                analiz.Дата_и_время_выполнения_услуги = DateTime.Now;
                bd.Работа_анализатора.Add(analiz);
                bd.SaveChanges();

                var uslug = bd.Услуги_заказа.Where(w=>w.Код_услуги_заказа == userData.idService).FirstOrDefault();
                uslug.Результат = rnd.Next(0,12) + rnd.NextDouble();
                uslug.Среднее_отклонение = rnd.NextDouble();
                uslug.Код_статуса_услуги = 2;
                bd.SaveChanges();

                var time = bd.Услуга.Where(w => w.Код_услуги == kodeuslug).FirstOrDefault();
                inter = 100 / Convert.ToInt32(time.Срок_выполнения);

                json();
                pb.Value = 0;
                send.IsEnabled = false;
                timer.Start();
            }
        }

        private void json()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Код_анализатора", typeof(int));
            table.Columns.Add("Код_услуги_заказа", typeof(string));
            table.Columns.Add("Дата_и_время_выполнения_услуги", typeof(string));
            table.Rows.Add(userData.idAnalizator, userData.idService, DateTime.Now);
            string json = JsonConvert.SerializeObject(table, Formatting.Indented);
            File.WriteAllText("data.json", json);
        }
    }
}
