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

namespace PhoneDirectory
{
    /// <summary>
    /// Логика взаимодействия для AddContact.xaml
    /// </summary>
    public partial class AddContact : Window
    {
        public AddContact()
        {
            InitializeComponent();
        }

        private void add_con1_Click(object sender, RoutedEventArgs e)
        {
            if (fam.Text == "" || im.Text == "" || ot.Text == "" || nom.Text == "" || em.Text == "" || kom.Text == "" || dol.Text == "" || grup.Text == "" || data.Text == "") 
            {
                MessageBox.Show("Внесите данные");
            }
            else
            {
                string FAM = fam.Text;
                string IM = im.Text;
                string OT = ot.Text;
                string NOM = nom.Text;
                string EM = em.Text;
                string KOM = kom.Text;
                string DOL = dol.Text;
                string GRUP = grup.Text;
                string DATA = data.Text;
                string connectionString = $@"Data Source = DESKTOP-09DGVTM\SQLEXPRESS; Initial Catalog = Телефонный справочник; Integrated Security = True";
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string query = $"insert into [Данные о контактах] (фамилия, имя, отчество, [номер телефона], [e-mail], компания, должность, [группа контактов], [дата рождения]) values ('{FAM}', '{IM}', '{OT}', '{NOM}', '{EM}', '{KOM}', '{DOL}', '{GRUP}', '{DATA}');";
                SqlCommand com = new SqlCommand(query, con);
                com.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Данные успешно внесены");
            }
        }

        private void show_contact_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = Show();
            datagrid1.ItemsSource = dt.DefaultView;
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

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Calendar calendar = sender as Calendar;
            if (calendar.SelectedDate.HasValue)
            {
                data.Text = calendar.SelectedDate.Value.ToShortDateString();
            }
        }
    }
}
