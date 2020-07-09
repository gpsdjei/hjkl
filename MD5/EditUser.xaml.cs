using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class EditUser : Page
    {
        public EditUser()
        {
            try
            {
                InitializeComponent();
                cmbSel.Items.Add("Администратор");
                cmbSel.Items.Add("Преподаватель");
                cmbSel.Items.Add("Студент");
                Name.IsReadOnly = true;
                Pol.IsEnabled = false;
                Number.IsReadOnly = true;
                Birth.IsEnabled = false;
                cmbSel.IsEnabled = false;
                IsPrimaryButtonEnabled = false;
            }
            catch { }
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                StorageFolder storage = ApplicationData.Current.LocalFolder;
                StorageFile file = await storage.CreateFileAsync("MD5Account.xml", CreationCollisionOption.OpenIfExists);
                List<User> d;
                XmlSerializer x = new XmlSerializer(typeof(List<User>));
                Stream stream = await file.OpenStreamForReadAsync();
                try
                {
                    d = (List<User>)x.Deserialize(stream);
                }
                catch
                {
                    d = new List<User>();
                }
                stream.Close();
                foreach (User user in d)
                {
                    if (user.Login == LoginForRename.Text)
                    {
                        user.Name = Name.Text;
                        user.Pol = Pol.Content.ToString();
                        user.Nomor = Number.Text;
                        user.BirthDay = Birth.Date.Date.ToString("dd.MM.yyyy");
                        user.Select = cmbSel.SelectedItem.ToString();
                    }
                }
                stream = await file.OpenStreamForWriteAsync();
                x.Serialize(stream, d);
                stream.Close();
            }
            catch { }
        }
        private void LoginForRename_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            try
            {
                IsPrimaryButtonEnabled = false;
                Name.IsReadOnly = true;
                Pol.IsEnabled = false;
                Number.IsReadOnly = true;
                Birth.IsEnabled = false;
                cmbSel.IsEnabled = false;
                Name.Text = String.Empty;
                Number.Text = String.Empty;
                Pol.Content = "Пол";
                cmbSel.SelectedIndex = -1;
            }
            catch { }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StorageFolder storage = ApplicationData.Current.LocalFolder;
                StorageFile file = await storage.CreateFileAsync("MD5Account.xml", CreationCollisionOption.OpenIfExists);
                List<User> d;
                XmlSerializer x = new XmlSerializer(typeof(List<User>));
                Stream stream = await file.OpenStreamForReadAsync();
                try
                {
                    d = (List<User>)x.Deserialize(stream);
                }
                catch
                {
                    d = new List<User>();
                }
                stream.Close();
                foreach (User user in d)
                {
                    if (user.Login == LoginForRename.Text)
                    {
                        IsPrimaryButtonEnabled = true;
                        Name.IsReadOnly = false;
                        Pol.IsEnabled = true;
                        Number.IsReadOnly = false;
                        Birth.IsEnabled = true;
                        cmbSel.IsEnabled = true;
                        Name.Text = user.Name;
                        Pol.Content = user.Pol;
                        if (user.Pol == "Мужской") Pol.IsChecked = true;
                        Number.Text = user.Nomor;
                        Birth.Date = Convert.ToDateTime(user.BirthDay);
                        if (user.Select == "Администратор") { cmbSel.SelectedIndex = 0; }
                        if (user.Select == "Преподаватель") { cmbSel.SelectedIndex = 1; }
                        if (user.Select == "Студент") { cmbSel.SelectedIndex = 2; }
                        return;
                    }
                    else
                    {
                        Name.IsReadOnly = true;
                        Pol.IsEnabled = false;
                        Number.IsReadOnly = true;
                        Birth.IsEnabled = false;
                        cmbSel.IsEnabled = false;
                    }
                }
            }
            catch { }
        }
        private void Pol_Checked(object sender, RoutedEventArgs e) => Pol.Content = "Мужской";
        private void Pol_Unchecked(object sender, RoutedEventArgs e) => Pol.Content = "Женский";

        private void Number_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
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

        internal Task ShowAsync()
        {
            throw new NotImplementedException();
        }
    }
}
