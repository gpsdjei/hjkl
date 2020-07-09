using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace MD5
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AddUser : Page
    {
        public string check1 = "[ABCDEFGHIJKLMNOPQRSTUVWXYZАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ]";
        public string check2 = "[abcdefghijklmnopqrstuvwxyzабвгдеёжзийклмнопрстуфхцчшщъыьэюя]";
        public string check3 = "[1234567890]";
        public string check4 = "[№!#$%&'()*+,-.\"\\/:;<=>?@^_`{|}~]";
        public MessageDialog checkrepeatlogin;
        public MessageDialog err = new MessageDialog("");
        public AddUser()
        {
            try
            {
                InitializeComponent();
                cmbSelect.Items.Add("Администратор");
                cmbSelect.Items.Add("Преподователь");
                cmbSelect.Items.Add("Студент");
                cmbGroup.Items.Add("Без группы");
                cmBoxLoad();
            }
            catch { }
        }
        public async void cmBoxLoad()
        {
            try
            {
                StorageFolder storage = ApplicationData.Current.LocalFolder;
                StorageFile file = await storage.CreateFileAsync("Group.xml", CreationCollisionOption.OpenIfExists);
                List<Group> list;
                XmlSerializer x = new XmlSerializer(typeof(List<Group>));
                Stream stream = await file.OpenStreamForReadAsync();
                try
                {
                    list = (List<Group>)x.Deserialize(stream);
                }
                catch
                {
                    list = new List<Group>();
                }
                stream.Close();
                foreach (Group group in list) cmbGroup.Items.Add(group.GroupName);
            }
            catch { }
        }
        private void pol_Checked(object sender, RoutedEventArgs e) => pol.Content = "Мужской";
        private void pol_Unchecked(object sender, RoutedEventArgs e) => pol.Content = "Женский";
        private void loginTextChange(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            try
            {
                for (int i = 0; i < args.NewText.Length; i++)
                {
                    if (!char.IsLetterOrDigit(args.NewText[i]))
                    {
                        args.Cancel = true;
                        break;
                    }
                }
            }
            catch { }
        }
        private void namberChange(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            try
            {
                for (int i = 0; i < args.NewText.Length; i++)
                {
                    if (!char.IsDigit(args.NewText[i]))
                    {
                        args.Cancel = true;
                        break;
                    }
                }
            }
            catch { }
        }
        public async void MessageDialoGG(string value)
        {
            try
            {
                MessageDialog dialog = new MessageDialog(value);
                await dialog.ShowAsync();
                return;
            }
            catch { }
        }
        public async void CheckLogin()
        {
            try
            {
                StorageFolder f = ApplicationData.Current.LocalFolder;
                StorageFile f1 = await f.CreateFileAsync("MD5Account.xml", CreationCollisionOption.OpenIfExists);
                FileInfo file = new FileInfo(f1.Path);
                if (file.Length <= 0)
                {
                    if (login.Text.Length >= 6)
                    {
                        CheckPass();
                        return;
                    }
                    else MessageDialoGG("Логин должен содержать хотя бы 6 символов");
                }
                List<User> u;
                XmlSerializer x = new XmlSerializer(typeof(List<User>));
                Stream s = await f1.OpenStreamForReadAsync();
                u = (List<User>)x.Deserialize(s);
                foreach (User b in u) if (login.Text == b.Login) MessageDialoGG("Такой логин занят");
                if (login.Text.Length >= 6) CheckPass();
                else MessageDialoGG("Логин должен содержать хотя бы 6 символов");
            }
            catch { }
        }
        public void CheckPass()
        {
            try
            {
                if (Regex.IsMatch(pass1.Password, check1) && Regex.IsMatch(pass1.Password, check2) && Regex.IsMatch(pass1.Password, check3) && Regex.IsMatch(pass1.Password, check4) && pass1.Password.Length >= 6) CheckRepeatPass();
                else MessageDialoGG("Вы неправильно ввели пароль. Пароль должен соответствовать следующим критериям:\n1. Минимум 8 символов\n2. Содержать строчную и прописную букву\n3. Содержать хотя бы 1 цифру\n4. Содержать хотя бы 1 специальный символ");
            }
            catch { }
        }
        public void CheckRepeatPass()
        {
            try
            {
                if (pass1.Password == pass2.Password) CheckDate();
                else MessageDialoGG("Вы неправильно повторили пароль");
            }
            catch { }
        }
        public void CheckDate()
        {
            try
            {
                if (date.Date < DateTime.Now) CheckSex();
                else MessageDialoGG("Вы не выбрали дату или еще не родились");
            }
            catch { }
        }
        public void CheckSex()
        {
            try
            {
                if (pol.Content.ToString() != "Пол") CheckName();
                else MessageDialoGG("Вы не выбрали пол");
            }
            catch { }
        }
        public void CheckName()
        {
            try
            {
                if (name.Text.Length > 1) CheckNomor();
                else MessageDialoGG("Имя содержит слишком мало символов ( < 2)");
            }
            catch { }

        }
        public void CheckNomor()
        {
            try
            {
                if (mobile.Text.Length == 11 && mobile.Text[0] == Convert.ToChar("8")) CheckSelect();
                else MessageDialoGG("Номер телефона не имеет достаточное количество символов\nили начинается на цифру отличающуюся от 8");
            }
            catch { }
        }
        public void CheckSelect()
        {
            try
            {
                if (cmbSelect.SelectedItem.ToString() == "Студент") CheckGroup();
                if (cmbSelect.SelectedItem.ToString() == "Администратор" || cmbSelect.SelectedItem.ToString() == "Преподователь") create();
                if (cmbSelect.SelectedIndex < 0) MessageDialoGG("Вы не выбрали должность");
            }
            catch { }
        }
        public void CheckGroup()
        {
            try
            {
                if (cmbSelect.SelectedItem.ToString() == "Студент")
                {
                    if (cmbGroup.SelectedIndex > -1) create();
                    else MessageDialoGG("Вы не выбрали группу");
                }
            }
            catch { }
        }
        public async void create()
        {
            try
            {
                StorageFolder f = ApplicationData.Current.LocalFolder;
                StorageFile f1 = await f.CreateFileAsync("MD5Account.xml", CreationCollisionOption.OpenIfExists);
                List<User> u;
                XmlSerializer x = new XmlSerializer(typeof(List<User>));
                Stream s = await f1.OpenStreamForReadAsync();
                try
                {
                    u = (List<User>)x.Deserialize(s);
                }
                catch
                {
                    u = new List<User>();
                }
                s.Close();
                byte[] hash = Encoding.ASCII.GetBytes(pass1.Password);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] hashenc = md5.ComputeHash(hash);
                string result = "";
                foreach (var b in hashenc) result += b.ToString("x2");
                User user = new User()
                {
                    Login = login.Text,
                    Pass = result,
                    BirthDay = date.Date.Date.ToString("dd.MM.yyyy"),
                    Pol = pol.IsChecked.Value ? "Мужской" : "Женский",
                    Nomor = mobile.Text,
                    Name = name.Text,
                    Select = cmbSelect.SelectedItem.ToString(),
                    Group = cmbGroup.SelectedItem.ToString()
                };
                u.Add(user);
                s = await f1.OpenStreamForWriteAsync();
                x.Serialize(s, u);
                s.Close();
                var yes = new MessageDialog("Пользователь создан");
                await yes.ShowAsync();
            }
            catch { }
        }
        private void cmbSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbSelect.SelectedIndex < 2) cmbGroup.SelectedIndex = 0;
            }
            catch { }
        }

        private void cmbGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbSelect.SelectedIndex < 2) cmbGroup.SelectedIndex = 0;
            }
            catch { }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) => CheckLogin();

        internal Task ShowAsync()
        {
            throw new NotImplementedException();
        }
    }
}

