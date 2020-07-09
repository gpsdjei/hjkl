using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
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
    public sealed partial class Vhod : Page
    {

        public Vhod()
        {
            this.InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private async void finalOCHKA(object sender, RoutedEventArgs e)
        {
            StorageFolder f = ApplicationData.Current.LocalFolder;
            StorageFile f1 = await f.CreateFileAsync("MD5Account.xml", CreationCollisionOption.OpenIfExists);
            FileInfo file = new FileInfo(f1.Path);
            if (file.Length <= 0)
            {
                return;
            }
            List<User> u;
            XmlSerializer x = new XmlSerializer(typeof(List<User>));
            Stream s = await f1.OpenStreamForReadAsync();
            u = (List<User>)x.Deserialize(s);
            byte[] hashenc = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(passwordBox.Password));
            string result = "";
            foreach (var bb in hashenc)
            {
                result += bb.ToString("x2");
            }
            foreach (User b in u)
            {
                if (textBox.Text == b.Login && result == b.Pass)
                {
                    if (b.Select == "Администратор") Frame.Navigate(typeof(Admin));
                    if (b.Select == "Преподователь") Frame.Navigate(typeof(Prepodavatel));
                    if (b.Select == "Студент")
                    {
                        Frame.Navigate(typeof(Student));
                        StorageFolder ff = ApplicationData.Current.LocalFolder;
                        StorageFile ff1 = await ff.CreateFileAsync("studentNAME", CreationCollisionOption.OpenIfExists);
                        File.WriteAllText(ff1.Path, b.Name);
                    }
                    return;
                }
            }
            MessageDialog err = new MessageDialog("Вы походу ошиблись. Нажмите кнопку \"закрыть\" для продолжения");
            await err.ShowAsync();
        }
    }
}
