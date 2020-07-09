using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace WEB
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public class History
    {
        public string Date;
        public string Url;
    }
    public class Marks
    {
        public string Url;
        public string Date;
    }
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            try { NewVkladOCHKA(); }
            catch (Exception) { }
        }
        public async void NewVkladOCHKA()
        {
            try
            {
                string dsd = "";
                StorageFolder f2 = ApplicationData.Current.LocalFolder;
                StorageFile f12 = await f2.CreateFileAsync("historyPer.txt", CreationCollisionOption.OpenIfExists);
                dsd = File.ReadAllText(f12.Path);
                File.Delete(f12.Path);
                if (dsd == "") dsd = "https://google.com";
                TabViewItem item = new TabViewItem();
                item.Header = "Загрузка...";
                WebView w = new WebView();
                w.DOMContentLoaded += W_DOMContentLoaded;
                w.NewWindowRequested += WebView_NewWindowRequested;
                w.HorizontalAlignment = HorizontalAlignment.Stretch;
                w.VerticalAlignment = VerticalAlignment.Stretch;
                item.Content = w;
                item.IsSelected = true;
                tab.TabItems.Add(item);
                w.Navigate(new Uri(dsd));
                searchBox.Text = w.Source.AbsoluteUri;
            }
            catch (Exception) { }
        }
        public async void W_DOMContentLoaded(WebView s, WebViewDOMContentLoadedEventArgs args)
        {
            try
            {
                searchBox.Text = args.Uri.ToString();
                (s.Parent as TabViewItem).Header = s.DocumentTitle;
            }
            catch (Exception) { }
            try
            {
                searchBox.Text = s.Source.ToString();
                StorageFolder f2 = ApplicationData.Current.LocalFolder;
                StorageFile f12 = await f2.CreateFileAsync("Marks.xml", CreationCollisionOption.OpenIfExists);
                List<Marks> u3;
                XmlSerializer x2 = new XmlSerializer(typeof(List<Marks>));
                Stream s4 = await f12.OpenStreamForReadAsync();
                u3 = (List<Marks>)x2.Deserialize(s4);
                foreach (Marks b3 in u3)
                {
                    if (s.Source.ToString() == b3.Url)
                    {
                        zak.Symbol = Symbol.SolidStar;
                        break;
                    }
                    else
                    {
                        zak.Symbol = Symbol.OutlineStar;
                        break;
                    }
                }
                s4.Close();
            }
            catch (Exception) { }
            if (s.Source.ToString() != "https://www.google.com/")
            {
                try
                {
                    searchBox.Text = args.Uri.ToString();
                    (s.Parent as TabViewItem).Header = s.DocumentTitle;
                    StorageFolder f = ApplicationData.Current.LocalFolder;
                    StorageFile f1 = await f.CreateFileAsync("History.xml", CreationCollisionOption.OpenIfExists);
                    List<History> u;
                    XmlSerializer x = new XmlSerializer(typeof(List<History>));
                    Stream s1 = await f1.OpenStreamForReadAsync();
                    try
                    {
                        u = (List<History>)x.Deserialize(s1);
                    }
                    catch
                    {
                        u = new List<History>();
                    }
                    s1.Close();
                    History history = new History()
                    {
                        Date = DateTime.Now.ToString(),
                        Url = searchBox.Text,
                    };
                    u.Add(history);
                    s1 = await f1.OpenStreamForWriteAsync();
                    x.Serialize(s1, u);
                    s1.Close();
                }
                catch (Exception) { }
            }
        }
        private void TabView_AddTabButtonClick_1(TabView sender, object args)
        {
            NewVkladOCHKA();
        }
        private void WebView_NewWindowRequested(WebView s, WebViewNewWindowRequestedEventArgs a)
        {
            try
            {
                s.Navigate(a.Uri);
                a.Handled = true;
                s.NewWindowRequested += WebView_NewWindowRequested;
            }
            catch (Exception) { }
        }
        private void Button_Click(object sender, RoutedEventArgs e) => rgg(1);
        private void Button_Click_1(object sender, RoutedEventArgs e) => rgg(2);
        private void Button_Click_2(object sender, RoutedEventArgs e) => rgg(3);
        private void tab_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            try
            {
                if (tab.TabItems.Count == 1)
                {
                    sender.TabItems.Remove(args.Tab);
                    NewVkladOCHKA();
                }
                else sender.TabItems.Remove(args.Tab);
            }
            catch (Exception) { }
        }
        private async void downloadOpen(object sender, RoutedEventArgs e)
        {
            try
            {
                ContentDialog1 f = new ContentDialog1();
                await f.ShowAsync();
            }
            catch (Exception) { }
        }
        private void Button_Click_4(object sender, RoutedEventArgs e) => ((tab.SelectedItem as TabViewItem).Content as WebView).Navigate(new Uri("https://google.com"));
        private async void searchBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            try
            {
                if (e.Key == VirtualKey.Enter) chetotam();
            }
            catch
            {
                MessageDialog er = new MessageDialog("Ссылка введена неправильно");
                await er.ShowAsync();
            }
        }
        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                chetotam();
            }
            catch (Exception)
            {
                MessageDialog er = new MessageDialog("Ссылка введена неправильно");
                await er.ShowAsync();
            }
        }
        private void bookmarksOpen(object sender, RoutedEventArgs e) => Frame.Navigate(typeof(BlankPage2));
        private void historyOpen(object sender, RoutedEventArgs e) => Frame.Navigate(typeof(BlankPage1));
        public void chetotam()
        {
            try
            {
                WebView w = (tab.SelectedItem as TabViewItem).Content as WebView;
                w.Navigate(new Uri(searchBox.Text));
                w.DOMContentLoaded += W_DOMContentLoaded;
                w.NewWindowRequested += WebView_NewWindowRequested;
            }
            catch (Exception) { }
        }
        public void rgg(int d)
        {
            try
            {
                WebView w = (tab.SelectedItem as TabViewItem).Content as WebView;
                switch (d)
                {
                    case 1:
                        w.GoBack();
                        break;
                    case 2:
                        w.GoForward();
                        break;
                    case 3:
                        w.Refresh();
                        break;
                }
                w.DOMContentLoaded += W_DOMContentLoaded;
            }
            catch (Exception) { }
        }
        private async void zakClick(object sender, RoutedEventArgs e)
        {
            WebView web = (tab.SelectedItem as TabViewItem).Content as WebView;
            if (zak.Symbol == Symbol.OutlineStar)
            {
                zak.Symbol = Symbol.SolidStar;
                StorageFolder f = ApplicationData.Current.LocalFolder;
                StorageFile f1 = await f.CreateFileAsync("Marks.xml", CreationCollisionOption.OpenIfExists);
                List<Marks> u;
                XmlSerializer x = new XmlSerializer(typeof(List<Marks>));
                Stream s1 = await f1.OpenStreamForReadAsync();
                try
                {
                    u = (List<Marks>)x.Deserialize(s1);
                }
                catch
                {
                    u = new List<Marks>();
                }
                s1.Close();
                Marks m = new Marks()
                {
                    Url = web.Source.ToString(),
                    Date = DateTime.Now.ToString()
                };
                u.Add(m);
                s1 = await f1.OpenStreamForWriteAsync();
                x.Serialize(s1, u);
                s1.Close();
                return;
            }
            if (zak.Symbol == Symbol.SolidStar)
            {
                zak.Symbol = Symbol.OutlineStar;
                StorageFolder f = ApplicationData.Current.LocalFolder;
                StorageFile f1 = await f.CreateFileAsync("Marks.xml", CreationCollisionOption.OpenIfExists);
                List<Marks> u;
                XmlSerializer x = new XmlSerializer(typeof(List<Marks>));
                Stream s = await f1.OpenStreamForWriteAsync();
                u = (List<Marks>)x.Deserialize(s);
                foreach (Marks b in u)
                {
                    if (web.Source.ToString() == b.Url.ToString())
                    {
                        u.Remove(b);
                        break;
                    }
                }
                s.Close();
                await f1.DeleteAsync();
                StorageFile f2 = await f.CreateFileAsync("Marks.xml", CreationCollisionOption.OpenIfExists);
                Stream s2 = await f2.OpenStreamForWriteAsync();
                x.Serialize(s2, u);
                s2.Close();
            }
        }
        private void tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                WebView web = (tab.SelectedItem as TabViewItem).Content as WebView;
                searchBox.Text = web.Source.ToString();
            }
            catch (Exception) { }
        }
        private void searchBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            try
            {
                TabViewItem s = tab.SelectedItem as TabViewItem;
                s.Header = (s.Content as WebView).DocumentTitle;
            }
            catch (Exception) { }
        }
    }
}
