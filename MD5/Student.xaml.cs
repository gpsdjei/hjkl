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
using static MD5.AddPara;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace MD5
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Student : Page
    {
        public Student()
        {
            this.InitializeComponent();
            Start();
        }
        public async void Start()
        {
            StorageFolder storage = ApplicationData.Current.LocalFolder;
            StorageFile file = await storage.CreateFileAsync("ParaData.xml", CreationCollisionOption.OpenIfExists);
            List<ParaData> list;
            XmlSerializer x = new XmlSerializer(typeof(List<ParaData>));
            Stream stream = await file.OpenStreamForReadAsync();
            try
            {
                list = (List<ParaData>)x.Deserialize(stream);
            }
            catch
            {
                list = new List<ParaData>();
            }
            stream.Close();
            foreach (ParaData para in list)
            {
                for (int i = 0; i < selDISCCMB.Items.Count; i++)
                {
                    if (selDISCCMB.Items[i].ToString() == para.Discipline)
                    {
                        return;
                    }
                }
                selDISCCMB.Items.Add(para.Discipline);
            }
        }

        private async void selDISCCMB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listRATING.Items.Clear();
            StorageFolder storage = ApplicationData.Current.LocalFolder;
            StorageFile file = await storage.CreateFileAsync("ParaData.xml", CreationCollisionOption.OpenIfExists);
            StorageFolder storage1 = ApplicationData.Current.LocalFolder;
            StorageFile file1 = await storage.CreateFileAsync("studentNAME", CreationCollisionOption.OpenIfExists);
            List<ParaData> list;
            XmlSerializer x = new XmlSerializer(typeof(List<ParaData>));
            Stream stream = await file.OpenStreamForReadAsync();
            try
            {
                list = (List<ParaData>)x.Deserialize(stream);
            }
            catch
            {
                list = new List<ParaData>();
            }
            stream.Close();
            foreach (ParaData para in list)
            {
                if (para.Discipline == selDISCCMB.SelectedItem.ToString() && para.Student == File.ReadAllText(file1.Path))
                {
                    listRATING.Items.Add(para.Rating);
                }
            }
        }
    }
}
