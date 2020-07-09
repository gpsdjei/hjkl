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
    public sealed partial class BlankPage2 : Page
    {
        public BlankPage2() => this.InitializeComponent();

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            StorageFolder f = ApplicationData.Current.LocalFolder;
            StorageFile f1 = await f.CreateFileAsync("Marks.xml", CreationCollisionOption.OpenIfExists);
            List<Marks> u;
            XmlSerializer x = new XmlSerializer(typeof(List<Marks>));
            Stream s = await f1.OpenStreamForReadAsync();
            try
            {
                u = (List<Marks>)x.Deserialize(s);
            }
            catch
            {
                u = new List<Marks>();
            }
            s.Close();
            u.Reverse();
            foreach (Marks h in u)
            {
                historyList.Items.Add($"{h.Date}  -  {h.Url}");
            }
            u.Reverse();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("Marks.xml", CreationCollisionOption.OpenIfExists);
            await file.DeleteAsync();
            historyList.Items.Clear();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e) => Frame.GoBack();

        private async void historyList_Tapped(object sender, TappedRoutedEventArgs e)
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
