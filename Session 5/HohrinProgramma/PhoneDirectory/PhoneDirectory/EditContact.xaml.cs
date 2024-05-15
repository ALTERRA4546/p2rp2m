using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PhoneDirectory
{
    /// <summary>
    /// Логика взаимодействия для EditContact.xaml
    /// </summary>
    public partial class EditContact : Window
    {
        public EditContact()
        {
            InitializeComponent();
        }

        private void edit_contact_Click(object sender, RoutedEventArgs e)
        {
            if (fam.Text == "" || im.Text == "" || ot.Text == "" || nom.Text == "" || em.Text == "" || kom.Text == "" || dol.Text == "" || grup.Text == "" || data.Text == "")
            {
                MessageBox.Show("Внесите данные");
            }
            else
            {
                string KOD = kod.Text;
                string FAM = fam.Text;
                string IM = im.Text;
                string OT = ot.Text;
                string NOM = nom.Text;
                string EM = em.Text;
                string KOM = kom.Text;
                string DOL = dol.Text;
                string GRUP = grup.Text;
                string DATA = data.Text;
                Edit(KOD, FAM, IM, OT, NOM, EM, KOM, DOL, GRUP, DATA);
                MessageBox.Show("Данные успешно изменены");
            }
        }

        public int Edit(string KOD, string FAM, string IM, string OT, string NOM, string EM, string KOM, string DOL, string GRUP, string DATA)
        {
            string connectionString = $@"Data Source = DESKTOP-09DGVTM\SQLEXPRESS; Initial Catalog = Телефонный справочник; Integrated Security = True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string query = $"update [Данные о контактах] set фамилия = '{FAM}', имя = '{IM}', отчество = '{OT}', [номер телефона] = '{NOM}', [e-mail] = '{EM}', компания = '{KOM}', должность = '{DOL}', [группа контактов] = '{GRUP}', [дата рождения] = '{DATA}' where [код контакта] = '{KOD}'";
            SqlCommand com = new SqlCommand(query, con);
            int rowsAffected = com.ExecuteNonQuery();
            con.Close();
            return rowsAffected;
        }
    }
}
