using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void show_contacts_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = Show();
            datagrid.ItemsSource = dt.DefaultView;
        }

        public DataTable Show()
        {
            string connectionString = $@"Data Source = DESKTOP-09DGVTM\SQLEXPRESS; Initial Catalog = Телефонный справочник; Integrated Security = True";
            SqlConnection con = new SqlConnection(connectionString);
            string query = "select фамилия, имя, отчество, [номер телефона], [e-mail], [название], [название должности], [группа контактов], [дата рождения] from [Данные о контактах] inner join Компании on [Данные о контактах].компания = Компании.[код компании] inner join Должности on [Данные о контактах].должность = Должности.[код должности]";
            SqlCommand com = new SqlCommand(query, con);
            SqlDataAdapter ad = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            ad.Fill(ds, "[Данные о контактах]");
            return ds.Tables["[Данные о контактах]"];
        }

        private void add_contact_Click(object sender, RoutedEventArgs e)
        {
            AddContact ac = new AddContact();
            ac.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EditContact ec = new EditContact();
            ec.ShowDialog();
        }

        private void delete_contact_Click(object sender, RoutedEventArgs e)
        {
            DeleteContact dc = new DeleteContact();
            dc.ShowDialog();
        }

        private void sort_Click(object sender, RoutedEventArgs e)
        {
            if (sor.Text == "")
            {
                MessageBox.Show("Введите поле для сортировки");
            }
            else
            {
                DataTable dt = Sort(sor.Text);
                datagrid.ItemsSource = dt.DefaultView;
            }
        }

        public DataTable Sort(string sort)
        {
            string connectionString = $@"Data Source = DESKTOP-09DGVTM\SQLEXPRESS; Initial Catalog = Телефонный справочник; Integrated Security = True";
            SqlConnection con = new SqlConnection(connectionString);
            string query = $"select фамилия, имя, отчество, [номер телефона], [e-mail], [название], [название должности], [группа контактов], [дата рождения] from [Данные о контактах]  inner join Компании on [Данные о контактах].компания = Компании.[код компании]  inner join Должности on [Данные о контактах].должность = Должности.[код должности] order by '{sort}'";
            SqlCommand com = new SqlCommand(query, con);
            SqlDataAdapter ad = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            ad.Fill(ds, "[Данные о контактах]");
            return ds.Tables["[Данные о контактах]"];
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = Search(sor.Text);
            datagrid.ItemsSource = dt.DefaultView;
        }

        public DataTable Search(string serchData)
        {
            string connectionString = $@"Data Source = DESKTOP-09DGVTM\SQLEXPRESS; Initial Catalog = Телефонный справочник; Integrated Security = True";
            SqlConnection con = new SqlConnection(connectionString);
            string query = $"select фамилия, имя, отчество, [номер телефона], [e-mail], [название], [название должности], [группа контактов], [дата рождения] from [Данные о контактах]  inner join Компании on [Данные о контактах].компания = Компании.[код компании]  inner join Должности on [Данные о контактах].должность = Должности.[код должности] where фамилия like '{serchData}' or имя like '{serchData}' or отчество like '{serchData}' or [группа контактов] like '{serchData}'";
            SqlCommand com = new SqlCommand(query, con);
            SqlDataAdapter ad = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            ad.Fill(ds, "[Данные о контактах]");
            return ds.Tables["[Данные о контактах]"];
        }
    }
}
