using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Text.RegularExpressions;
using System.Windows.Markup;

namespace PhoneDirectory
{
    /// <summary>
    /// Логика взаимодействия для DeleteContact.xaml
    /// </summary>
    public partial class DeleteContact : Window
    {
        public DeleteContact()
        {
            InitializeComponent();
        }

        private void show_contacts_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = Show();
            datagrid2.ItemsSource = dt.DefaultView;
        }

        public DataTable Show()
        {
            string connectionString = $@"Data Source = DESKTOP-09DGVTM\SQLEXPRESS; Initial Catalog = Телефонный справочник; Integrated Security = True";
            SqlConnection con = new SqlConnection(connectionString);
            string query = "select [код контакта], фамилия, имя, отчество, [номер телефона], [e-mail], [название], [название должности], [группа контактов], [дата рождения] from [Данные о контактах] inner join Компании on [Данные о контактах].компания = Компании.[код компании] inner join Должности on [Данные о контактах].должность = Должности.[код должности]";
            SqlCommand com = new SqlCommand(query, con);
            SqlDataAdapter ad = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            ad.Fill(ds, "[Данные о контактах]");
            return ds.Tables["[Данные о контактах]"];
        }

        private void delete_contact_Click(object sender, RoutedEventArgs e)
        {
            if (kod.Text == "")
            {
                MessageBox.Show("Введите код контакта для удаления");
            }
            else
            {
                string KOD = kod.Text;
                Delete(KOD);
                MessageBox.Show("Данные успешно удалены");
            }
        }

        public int Delete(string KOD)
        {
            string connectionString = $@"Data Source = DESKTOP-09DGVTM\SQLEXPRESS; Initial Catalog = Телефонный справочник; Integrated Security = True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string query = $"delete from [Данные о контактах] where [код контакта] = '{KOD}'";
            SqlCommand com = new SqlCommand(query, con);
            int rowsAffected = com.ExecuteNonQuery();
            con.Close();
            return rowsAffected;
        }

        public void RemoveDeleteData()
        {
            string connectionString = $@"Data Source = DESKTOP-09DGVTM\SQLEXPRESS; Initial Catalog = Телефонный справочник; Integrated Security = True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string query = $"SET IDENTITY_INSERT dbo.[Данные о контактах] ON\r\nINSERT INTO [Данные о контактах] ([код контакта], фамилия, имя, отчество, [номер телефона], [e-mail], компания, должность, [группа контактов], [дата рождения])\r\nVALUES(12, 'Мачалкин', 'Денис', 'Сергеевич', '+7(489)489-78-78', 'den@mail.ru', 1, 1, 'Друзья', '12-02-2000');\r\nSET IDENTITY_INSERT dbo.[Данные о контактах] OFF";
            SqlCommand com = new SqlCommand(query, con);
            int rowsAffected = com.ExecuteNonQuery();
            con.Close();
        }
    }
}
