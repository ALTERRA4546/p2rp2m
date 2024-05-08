using System;
using System.Collections.Generic;
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

namespace MedLaboratory
{
    /// <summary>
    /// Логика взаимодействия для LoginHistory.xaml
    /// </summary>
    public partial class LoginHistory : Window
    {
        public LoginHistory()
        {
            InitializeComponent();
            Initial();
        }

        private void Initial()
        {
            using (var bd = new MedLaboratoryEntities())
            {
                var history = bd.История.Select( s=> new { s.Код_истории, s.Код_пользователя, s.Дата_и_время_входа, s.IP_адрес });

                dgrid.ItemsSource = history.ToList();
            }
        }
    }
}
