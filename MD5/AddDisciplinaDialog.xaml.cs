using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class AddDisciplinaDialog : Page
    {
        public class Discipline
        {
            public string name;
        }
        public sealed partial class AddDisciplineDialog : ContentDialog
        {
            public AddDisciplineDialog() => InitializeComponent();

            private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
            {
                StorageFolder storage = ApplicationData.Current.LocalFolder;
                StorageFile file = await storage.CreateFileAsync("Discipline.xml", CreationCollisionOption.OpenIfExists);
                List<Discipline> list;
                XmlSerializer x = new XmlSerializer(typeof(List<Discipline>));
                Stream stream = await file.OpenStreamForReadAsync();
                try
                {
                    list = (List<Discipline>)x.Deserialize(stream);
                }
                catch
                {
                    list = new List<Discipline>();
                }
                stream.Close();
                foreach (Discipline discipline in list)
                {
                    if (discipline.name.ToLower() == txtDis.Text.ToLower())
                    {
                        MessageDialog dialog = new MessageDialog("Такая дисциплина уже существует");
                        await dialog.ShowAsync();
                        return;
                    }
                }
                Discipline m = new Discipline()
                {
                    name = txtDis.Text
                };
                list.Add(m);
                stream = await file.OpenStreamForWriteAsync();
                x.Serialize(stream, list);
                stream.Close();
            }
        }
    }
}
}
