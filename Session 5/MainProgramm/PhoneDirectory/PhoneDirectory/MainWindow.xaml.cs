using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace PhoneDirectory
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Initial();
        }

        public static class UserData
        { 
            public static int idUser { get; set; }
            public static bool read {  get; set; }
        }

        private void Initial()
        {
            try
            {
                UserData.read = true;
                using (var bd = new PhoneDirectoryEntities())
                {
                    var phone = from p in bd.Контакты
                                select new
                                {
                                    p.Код_контакта,
                                    p.Фамилия,
                                    p.Имя,
                                    p.Отчество,
                                    p.Номер_телефона,
                                    p.Код_группы_контактов,
                                };
                    dgrid.ItemsSource = phone.ToList();
                    dgrid.Columns[5].Visibility = Visibility.Collapsed;

                    var groupData = bd.Группа_контактов.Select(s => s.Наименование).ToList();
                    groupData.Add("Все");
                    group.ItemsSource = groupData;
                    group.SelectedItem = "Все";

                    Colored();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Colored()
        {
            try
            {
                using (var bd = new PhoneDirectoryEntities())
                {
                    var idGroup = bd.Группа_контактов.Where(w => w.Наименование == "Родственики").FirstOrDefault();
                    DataTrigger trigger = new DataTrigger { Binding = new Binding("Код_группы_контактов") };
                    trigger.Value = idGroup.Код_группы_компании;
                    trigger.Setters.Add(new Setter { Property = DataGridRow.BackgroundProperty, Value = Brushes.Green });
                    Style style = new Style { TargetType = typeof(DataGridRow) };
                    style.Triggers.Add(trigger);
                    dgrid.RowStyle = style;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            UserData.read = true;
            try
            {
                dgrid.SelectedItem = null;
                UserData.idUser = -1;
                AddContacts ac = new AddContacts();
                ac.ShowDialog();
                Filter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            UserData.read = true;
            try
            {
                if (dgrid.SelectedIndex <= 0)
                {
                    MessageBox.Show("Элемент в таблице не выбран");
                    return;
                }
                DataGridRow row = (DataGridRow)dgrid.ItemContainerGenerator.ContainerFromIndex(dgrid.SelectedIndex);
                DataGridCell cell = dgrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                UserData.idUser = Convert.ToInt32(((TextBlock)cell.Content).Text);
                AddContacts ac = new AddContacts();
                ac.ShowDialog();
                Filter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var bd = new PhoneDirectoryEntities())
                {
                    if (dgrid.SelectedIndex <= 0)
                    {
                        MessageBox.Show("Элемент в таблице не выбран");
                        return;
                    }
                    DataGridRow row = (DataGridRow)dgrid.ItemContainerGenerator.ContainerFromIndex(dgrid.SelectedIndex);
                    DataGridCell cell = dgrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                    int idUser = Convert.ToInt32(((TextBlock)cell.Content).Text);
                    if (MessageBox.Show("Вы действительно хотите удалить эту запись", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        var deletePhone = bd.Контакты.Where(w => w.Код_контакта == idUser).FirstOrDefault();
                        bd.Контакты.Remove(deletePhone);
                        bd.SaveChanges();
                        MessageBox.Show("Запись удалена");
                    }
                    Filter();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var bd = new PhoneDirectoryEntities())
                {
                    var phoneData = (from p in bd.Контакты
                                     join
                                     d in bd.Должности on p.Код_должности equals d.Код_должности
                                     join
                                     c in bd.Компания on p.Код_компании equals c.Код_комапнии
                                     join
                                     gp in bd.Группа_контактов on p.Код_группы_контактов equals gp.Код_группы_компании
                                     select new
                                     {
                                         p.Код_контакта,
                                         p.Фамилия,
                                         p.Имя,
                                         p.Отчество,
                                         p.Номер_телефона,
                                         p.E_mail,
                                         Компания = c.Название,
                                         Должность = d.Наименование,
                                         Группа_контактов = gp.Наименование,
                                     }).ToList();

                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                    saveFileDialog.FileName = "PhoneContacts.csv";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string filePath = saveFileDialog.FileName;

                        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                        {
                            writer.WriteLine("Код контакта,Фамилия,Имя,Отчество,Номер телефона,E-mail,Компания,Должность,Группа контактов");
                            foreach (var item in phoneData)
                            {
                                writer.WriteLine($"{item.Код_контакта},{item.Фамилия},{item.Имя},{item.Отчество},{item.Номер_телефона},{item.E_mail},{item.Компания},{item.Должность},{item.Группа_контактов}");
                            }
                        }

                        MessageBox.Show("Данные сохранены в файл CSV.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Filter()
        {
            try
            {
                using (var bd = new PhoneDirectoryEntities())
                {
                    var goupdata = bd.Группа_контактов.Where(w => w.Наименование == group.SelectedItem.ToString()).FirstOrDefault();

                    var phone = from p in bd.Контакты
                                select new
                                {
                                    p.Код_контакта,
                                    p.Фамилия,
                                    p.Имя,
                                    p.Отчество,
                                    p.Номер_телефона,
                                    p.Код_группы_контактов,
                                };
                    if (group.SelectedItem.ToString() == "Все")
                    {
                        dgrid.ItemsSource = phone.ToList();
                    }
                    else
                    {
                        var idGroup = bd.Группа_контактов.Where(w => w.Наименование == group.SelectedItem.ToString()).FirstOrDefault();
                        var filterData = phone.Where(w => w.Код_группы_контактов == idGroup.Код_группы_компании).ToList();
                        dgrid.ItemsSource = filterData;
                    }
                    dgrid.Columns[5].Visibility = Visibility.Collapsed;
                    Colored();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void group_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void find_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void dgrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataGridRow row = (DataGridRow)dgrid.ItemContainerGenerator.ContainerFromIndex(dgrid.SelectedIndex);
                DataGridCell cell = dgrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                UserData.idUser = Convert.ToInt32(((TextBlock)cell.Content).Text);
                UserData.read = false;
                AddContacts ac = new AddContacts();
                ac.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
