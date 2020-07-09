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
using static MD5.AddDisciplinaDialog;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace MD5
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AddPara : Page
    {
        public class ParaData
        {
            public string Login;
            public string Group;
            public string Discipline;
            public string Date;
            public string Student;
            public string Rating;
        }
        public sealed partial class AddPara : ContentDialog
        {
            public bool dateCheck = false;
            public AddPara()
            {
                this.InitializeComponent();
                LoadBox();
                SelectRating.Items.Add("Не был");
                for (int i = 2; i <= 5; i++) SelectRating.Items.Add(i);
            }
            public async void LoadBox()
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
                    foreach (Group group in list) SelectGroup.Items.Add(group.GroupName);
                }
                catch { }
                try
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
                    foreach (Discipline group in list) SelectDisc.Items.Add(group.name);
                }
                catch { }
            }
            private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
            {
                try
                {
                    if (SelectGroup.SelectedIndex < 0 || SelectDisc.SelectedIndex < 0 || dateCheck == false || SelectStudent.SelectedIndex < 0 || SelectRating.SelectedIndex < 0)
                    {
                        MessageDialog messageDialog = new MessageDialog("Вы что-то не выбрали");
                        await messageDialog.ShowAsync();
                    }
                    else
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
                        ParaData m = new ParaData()
                        {
                            Group = SelectGroup.SelectedItem.ToString(),
                            Discipline = SelectDisc.SelectedItem.ToString(),
                            Date = SelectDate.Date.Date.ToString("dd.MM.yyyy"),
                            Student = SelectStudent.SelectedItem.ToString(),
                            Rating = SelectRating.SelectedItem.ToString()
                        };
                        list.Add(m);
                        stream = await file.OpenStreamForWriteAsync();
                        x.Serialize(stream, list);
                        stream.Close();
                        MessageDialog messageDialog = new MessageDialog("Пара создана и оценка выставлена");
                        await messageDialog.ShowAsync();
                    }
                }
                catch
                {
                    MessageDialog messageDialog = new MessageDialog("Вы что-то не выбрали");
                    await messageDialog.ShowAsync();
                }
            }
            private async void SelectGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                SelectStudent.Items.Clear();
                StorageFolder f = ApplicationData.Current.LocalFolder;
                StorageFile f1 = await f.CreateFileAsync("MD5Account.xml", CreationCollisionOption.OpenIfExists);
                List<User> u;
                XmlSerializer x = new XmlSerializer(typeof(List<User>));
                Stream s = await f1.OpenStreamForReadAsync();
                u = (List<User>)x.Deserialize(s);
                foreach (User b in u)
                {
                    if (b.Select == "Студент")
                    {
                        if (b.Group == SelectGroup.SelectedItem.ToString()) SelectStudent.Items.Add(b.Name);
                    }
                }
                s.Close();
            }
            private void SelectDate_DateChanged(object sender, DatePickerValueChangedEventArgs e) => dateCheck = true;
        }
    }
}



