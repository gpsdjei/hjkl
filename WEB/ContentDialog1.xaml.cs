using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace WEB
{
    public sealed partial class ContentDialog1 : ContentDialog
    {
        public ContentDialog1()
        {
            this.InitializeComponent();
            downloadBox.IsEnabled = false;
            downloadButton.IsEnabled = false;
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            DownloadOperation d = null;
            try
            {
                d = new BackgroundDownloader().CreateDownload(new Uri(searchBox.Text), await DownloadsFolder.CreateFileAsync(downloadBox.Text + extensionBox.Text));
            }
            catch (Exception)
            {
                d = new BackgroundDownloader().CreateDownload(new Uri(searchBox.Text), await DownloadsFolder.CreateFileAsync(downloadBox.Text + random.Next(0, 1000000) + extensionBox.Text));
            }
            downloadBox.IsEnabled = false;
            downloadButton.IsEnabled = false;
            await d.StartAsync().AsTask(new Progress<DownloadOperation>(operation =>
            {
                ulong u = d.Progress.BytesReceived * 1000 / d.Progress.TotalBytesToReceive;
                progressDownload.Value = u;
            }));
            MessageDialog messageDialog = new MessageDialog("Файл по идее скачан...");
            await messageDialog.ShowAsync();
            searchBox.IsEnabled = true;
            searchButton.IsEnabled = true;
            downloadBox.IsEnabled = false;
            downloadButton.IsEnabled = false;
            searchBox.Text = "";
            extensionBox.Text = ".xxx";
            progressSearch.Value = 0;
            downloadBox.Text = "";
            progressDownload.Value = 0;
        }
        private async void Button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpWebRequest http = (HttpWebRequest)WebRequest.Create(searchBox.Text);
                HttpWebResponse http1 = (HttpWebResponse)http.GetResponse();
                string url = http1.Headers.Get("Content-Disposition");
                url = HttpUtility.UrlDecode(url);
                url = url.Substring(url.LastIndexOf("."));
                downloadBox.IsEnabled = true;
                downloadButton.IsEnabled = true;
                searchBox.IsEnabled = false;
                searchButton.IsEnabled = false;
                extensionBox.Text = url;
                progressSearch.Value = 1;
            }
            catch (Exception ex)
            {
                MessageDialog v = new MessageDialog(ex.ToString());
                await v.ShowAsync();
            }
        }
    }
}
