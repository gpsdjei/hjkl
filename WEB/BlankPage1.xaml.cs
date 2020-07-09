using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace WEB
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        public BlankPage1() => this.InitializeComponent();
        private async void deleteClick(object sender, RoutedEventArgs e)
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("History.xml", CreationCollisionOption.OpenIfExists);
            await file.DeleteAsync();
            historyList.Items.Clear();
        }
        private void historyExit(object sender, RoutedEventArgs e) => this.Frame.GoBack();
        private async void Load(object sender, RoutedEventArgs e)
        {
            StorageFolder f = ApplicationData.Current.LocalFolder;
            StorageFile f1 = await f.CreateFileAsync("History.xml", CreationCollisionOption.OpenIfExists);
            List<History> u;
            XmlSerializer x = new XmlSerializer(typeof(List<History>));
            Stream s = await f1.OpenStreamForReadAsync();
            try
            {
                u = (List<History>)x.Deserialize(s);
            }
            catch
            {
                u = new List<History>();
            }
            s.Close();
            u.Reverse();
            foreach (History h in u)
            {
                historyList.Items.Add($"{h.Date}  -  {h.Url}");
            }
            u.Reverse();
        }
        public async void historyList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (historyList.SelectedItem != null)
                {
                    string[] s = historyList.SelectedItem.ToString().Split("-  ", StringSplitOptions.None);
                    string x = "";
                    for (int i = 1; i < s.Length; i++)
                    {
                        x += s[i];
                    }
                    StorageFolder f2 = ApplicationData.Current.LocalFolder;
                    StorageFile f12 = await f2.CreateFileAsync("historyPer.txt", CreationCollisionOption.OpenIfExists);
                    File.WriteAllText(f12.Path, x);
                    Frame.GoBack();
                }
            }
            catch { }
        }
    }
}
