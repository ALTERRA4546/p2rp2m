using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static PhoneDirectory.MainWindow;
using static System.Net.Mime.MediaTypeNames;

namespace PhoneDirectory
{
    /// <summary>
    /// Логика взаимодействия для AddContacts.xaml
    /// </summary>
    public partial class AddContacts : Window
    {
        public AddContacts()
        {
            InitializeComponent();
            Initial();
        }

        public string imagePath;

        private void Initial()
        {
            try
            {
                using (var bd = new PhoneDirectoryEntities())
                {
                    photo.Visibility = Visibility.Collapsed;
                    this.Height = 380;
                    var companyData = bd.Компания.Select(s => s.Название).ToList();
                    company.ItemsSource = companyData;
                    var postData = bd.Должности.Select(s => s.Наименование).ToList();
                    post.ItemsSource = postData;
                    var groupData = bd.Группа_контактов.Select(s => s.Наименование).ToList();
                    group.ItemsSource = groupData;

                    if (UserData.idUser > -1)
                    {
                        var phoneData = (from p in bd.Контакты
                                         where (p.Код_контакта == UserData.idUser)
                                         select new
                                         {
                                             p.Фамилия,
                                             p.Имя,
                                             p.Отчество,
                                             p.Номер_телефона,
                                             p.E_mail,
                                             p.Дата_рождения,
                                             p.Код_компании,
                                             p.Код_должности,
                                             p.Код_группы_контактов,
                                             p.Фотография,
                                         }).FirstOrDefault();

                        family.Text = phoneData.Фамилия;
                        name.Text = phoneData.Имя;
                        otchestvo.Text = phoneData.Отчество;
                        phone.Text = phoneData.Номер_телефона;
                        email.Text = phoneData.E_mail;
                        birthday.SelectedDate = phoneData.Дата_рождения;

                        var selctCompany = bd.Компания.Where(w=>w.Код_комапнии == phoneData.Код_компании).Select(s=>s.Название).FirstOrDefault();
                        if (selctCompany != null)
                        {
                            company.SelectedItem = selctCompany;
                        }
                        var selectPost = bd.Должности.Where(w => w.Код_должности == phoneData.Код_должности).Select(s => s.Наименование).FirstOrDefault();
                        if (selectPost != null)
                        {
                            post.SelectedItem = selectPost;
                        }
                        var selectGroup = bd.Группа_контактов.Where(w => w.Код_группы_компании == phoneData.Код_группы_контактов).Select(s => s.Наименование).FirstOrDefault();
                        if (selectGroup != null)
                        {
                            group.SelectedItem = selectGroup;
                        }

                        if (phoneData.Фотография != null)
                        {
                            byte[] photoData = phoneData.Фотография;
                            BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = new MemoryStream(photoData);
                            bitmapImage.EndInit();
                            photo.Source = bitmapImage;

                            photo.Visibility = Visibility.Visible;
                            this.Height = 440;
                        }
                    }

                    if (UserData.read == false)
                    {
                        family.IsReadOnly = true;
                        name.IsReadOnly = true;
                        otchestvo.IsReadOnly = true;
                        phone.IsReadOnly = true;
                        email.IsReadOnly = true;
                        birthday.IsEnabled = false;
                        company.IsEnabled = false;
                        post.IsEnabled = false;
                        group.IsEnabled = false;
                        AddPhoto.IsEnabled = false;
                        Add.IsEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
                using (var bd = new PhoneDirectoryEntities())
                {
                    if (family.Text == "" || name.Text == "" || otchestvo.Text == "" || phone.Text == "")
                    {
                        MessageBox.Show("Обязательные данные не были введенны");
                        return;
                    }

                    if (!email.Text.Contains("@mail.ru") && email.Text != "")
                    {
                        MessageBox.Show("Email введен неверно");
                        return;
                    }

                    string pattern = @"^\+7\(\d{3}\)\d{3}-\d{2}-\d{2}$";
                    if (!Regex.IsMatch(phone.Text, pattern))
                    {
                        MessageBox.Show("Телефон не соотвествует формату");
                        return;
                    }

                    if (UserData.idUser != -1)
                    {
                        var phoneData = bd.Контакты.Where(w => w.Код_контакта == UserData.idUser).FirstOrDefault();

                        phoneData.Фамилия = family.Text;
                        phoneData.Имя = name.Text;
                        phoneData.Отчество = otchestvo.Text;
                        phoneData.Номер_телефона = phone.Text;

                    if (email.Text != "")
                    {
                        phoneData.E_mail = email.Text;
                    }

                        phoneData.Дата_рождения = birthday.SelectedDate;
                        if (company.SelectedIndex >= 0)
                        {
                            var companyData = bd.Компания.Where(w => w.Название == company.SelectedItem.ToString()).FirstOrDefault();
                            phoneData.Код_компании = companyData.Код_комапнии;
                        }
                        if (post.SelectedIndex >= 0)
                        {
                            var postData = bd.Должности.Where(w => w.Наименование == post.SelectedItem.ToString()).FirstOrDefault();
                            phoneData.Код_должности = postData.Код_должности;
                        }
                        if (group.SelectedIndex >= 0)
                        {
                            var groupData = bd.Группа_контактов.Where(w => w.Наименование == group.SelectedItem.ToString()).FirstOrDefault();
                            phoneData.Код_группы_контактов = groupData.Код_группы_компании;
                        }
                        if (imagePath != null)
                        {
                            byte[] imageData = File.ReadAllBytes(imagePath);
                            phoneData.Фотография = imageData;
                        }
                    }
                    else
                    {
                        var newPhone = new Контакты();
                        newPhone.Фамилия = family.Text;
                        newPhone.Имя = name.Text;
                        newPhone.Отчество = otchestvo.Text;
                        newPhone.Номер_телефона = phone.Text;
                    if (email.Text != "")
                    {
                        newPhone.E_mail = email.Text;
                    }

                    newPhone.Дата_рождения = birthday.SelectedDate;
                        if (company.SelectedIndex >= 0)
                        {
                            var companyData = bd.Компания.Where(w => w.Название == company.SelectedItem.ToString()).FirstOrDefault();
                            newPhone.Код_компании = companyData.Код_комапнии;
                        }
                        if (post.SelectedIndex >= 0)
                        {
                            var postData = bd.Должности.Where(w => w.Наименование == post.SelectedItem.ToString()).FirstOrDefault();
                            newPhone.Код_должности = postData.Код_должности;
                        }
                        if (group.SelectedIndex >= 0)
                        {
                            var groupData = bd.Группа_контактов.Where(w => w.Наименование == group.SelectedItem.ToString()).FirstOrDefault();
                            newPhone.Код_группы_контактов = groupData.Код_группы_компании;
                        }
                        if (imagePath != null)
                        {
                            byte[] imageData = File.ReadAllBytes(imagePath);
                            newPhone.Фотография = imageData;
                        }
                        bd.Контакты.Add(newPhone);
                    }
                    bd.SaveChanges();
                    MessageBox.Show("Данные сохраненны");
                    this.Close();
                }
}

        private void AddPhoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Выберите файл";
                openFileDialog.InitialDirectory = @"C:\Users\Public\Pictures";
                openFileDialog.Filter = "JPEG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png";

                if (openFileDialog.ShowDialog() == true)
                {
                    imagePath = openFileDialog.FileName;
                    ImageSource imageSource = new BitmapImage(new Uri(openFileDialog.FileName));
                    photo.Source = imageSource;

                    photo.Visibility = Visibility.Visible;
                    this.Height = 440;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
