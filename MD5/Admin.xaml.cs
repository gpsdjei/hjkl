using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static MD5.AddDisciplinaDialog;
using static MD5.AddPara;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace MD5
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public class Group
    {
        public string GroupName;
    }
    public sealed partial class Admin : Page
    {
        TextBox info = new TextBox();
        ContentDialog delete = new ContentDialog();
        ContentDialog addGroup = new ContentDialog();
        TextBox addGroupTXT = new TextBox();
        public bool boo = true;
        public Admin()
        {
            InitializeComponent();
            addGroup.Title = "Добавить группу";
            addGroupTXT.HorizontalAlignment = HorizontalAlignment.Stretch;
            addGroup.Content = addGroupTXT;
            addGroup.CloseButtonText = "Отмена";
            addGroup.PrimaryButtonText = "Создать";
            addGroup.PrimaryButtonClick += AddGroup_PrimaryButtonClick;
            delete.Title = "Удалить пользователя";
            info.Width = 270;
            info.Height = 33;
            delete.Content = info;
            delete.IsPrimaryButtonEnabled = true;
            delete.PrimaryButtonText = "Удалить";
            delete.IsSecondaryButtonEnabled = true;
            delete.SecondaryButtonText = "Закрыть";
            delete.PrimaryButtonClick += Delete_PrimaryButtonClick;
        }

        private async void AddGroup_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
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
            foreach (Group group in list)
            {
                if (group.GroupName.ToLower() == addGroupTXT.Text.ToLower())
                {
                    MessageDialog dialog = new MessageDialog("Такая группа уже существует");
                    await dialog.ShowAsync();
                    return;
                }
            }
            Group m = new Group()
            {
                GroupName = addGroupTXT.Text
            };
            list.Add(m);
            stream = await file.OpenStreamForWriteAsync();
            x.Serialize(stream, list);
            stream.Close();
            groupList.Items.Add(addGroupTXT.Text);
            MessageDialog success = new MessageDialog("Группа создана");
            await success.ShowAsync();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel viewModel = new ViewModel();
                dataG.ItemsSource = viewModel.data;
                LoadDisciplineList();
                LoadGroupList();
            }
            catch { }
        }
        public async void LoadGroupList()
        {
            try
            {
                groupList.Items.Clear();
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
                foreach (Group group in list) groupList.Items.Add(group.GroupName);
            }
            catch { }
        }
        public async void LoadDisciplineList()
        {
            try
            {
                listDisp.Items.Clear();
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
                    listDisp.Items.Add(discipline.name);
                }
            }
            catch { }
        }
        //Добавление дисциплины
        private async void addDisciplineBTNCLICK(object sender, RoutedEventArgs e)
        {
            try
            {
                AddDisciplineDialog dialog = new AddDisciplineDialog();
                await dialog.ShowAsync();
            }
            catch { }
        }
        //Удаление дисциплин
        private async void delDisciplineBTNCLICK(object sender, RoutedEventArgs e)
        {
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
                int result = 0;
                string[] text = File.ReadAllLines(file.Path);
                for (int i = 0; i < text.Length; i++) if (text[i].Contains(listDisp.SelectedItem.ToString())) result = i;
                text[result + 1] = String.Empty;
                text[result] = String.Empty;
                text[result - 1] = String.Empty;
                File.WriteAllLines(file.Path, text);
                listDisp.Items.Clear();
                LoadDisciplineList();
            }
            catch { }
        }
        //Редактирование дисциплин
        private void editDisciplineBTNCLICK(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listDisp.SelectedItem != null)
                {
                    if (boo == true)
                    {
                        editDisciplineTXT.Visibility = Visibility.Visible;
                        editDisciplineTXT.Text = listDisp.SelectedItem.ToString();
                        editDisciplineTXT.SelectAll();
                    }
                    else editDisciplineTXT.Visibility = Visibility.Collapsed;
                    boo = !boo;
                }
            }
            catch { }
        }
        //Редактирование дисциплин (текстбокс)
        private async void editDisciplineTXTKEYDOWN(object sender, KeyRoutedEventArgs e)
        {
            try
            {
                if (e.Key == VirtualKey.Enter)
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
                    int result = 0;
                    string[] text = File.ReadAllLines(file.Path);
                    for (int i = 0; i < text.Length; i++) if (text[i].Contains(listDisp.SelectedItem.ToString())) result = i;
                    string stroka = text[result];
                    text[result] = $"<name>{editDisciplineTXT.Text}</name>";
                    File.WriteAllLines(file.Path, text);
                    listDisp.Items.Clear();
                    editDisciplineTXT.Visibility = Visibility.Collapsed;
                    boo = true;
                    LoadDisciplineList();
                }
            }
            catch { }
        }
        //Обновление списка дисциплин
        private void refreshDisciplineBTNCLICK(object sender, RoutedEventArgs e) => LoadDisciplineList();

        private async void addUserBTNCLICK(object sender, RoutedEventArgs e)
        {
            try
            {
                AddUser addUserDialog = new AddUser();
                await addUserDialog.ShowAsync();
            }
            catch { }
        }
        public ObservableCollection<Tablic> tablics { get; set; }
        private async void delUserBTNCLICK(object sender, RoutedEventArgs e)
        {
            try
            {
                await delete.ShowAsync();
            }
            catch { }
        }

        private async void Delete_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
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
                int result = 0;
                string[] text = File.ReadAllLines(file.Path);
                for (int i = 0; i < text.Length; i++) if (text[i].Contains($"<Login>{info.Text}</Login>")) result = i;
                for (int i = -1; i <= 8; i++) text[result + i] = string.Empty;
                File.WriteAllLines(file.Path, text);
                string deleteAll = File.ReadAllText(file.Path);
                if (!deleteAll.Contains("<User>")) File.Delete(file.Path);
                ViewModel viewModel = new ViewModel();
                dataG.ItemsSource = viewModel.data;
            }
            catch { }
        }
        private async void editUserBTNCLICK(object sender, RoutedEventArgs e)
        {
            try
            {
                EditUser editUserDialog = new EditUser();
                await editUserDialog.ShowAsync();
            }
            catch { }
        }
        public class Tablic
        {
            public string Логин { get; set; }
            public string Имя { get; set; }
            public string Пол { get; set; }
            public string Датарождения { get; set; }
            public string Номертелефона { get; set; }
            public string Должность { get; set; }
            public string Группа { get; set; }

            public Tablic(string login, string name, string sex, string date, string nomor, string select, string group)
            {
                Логин = login;
                Имя = name;
                Пол = sex;
                Датарождения = date;
                Номертелефона = nomor;
                Должность = select;
                Группа = group;
            }
        }
        public class ViewModel
        {
            public ObservableCollection<Tablic> data { get; set; }
            public ViewModel()
            {
                data = new ObservableCollection<Tablic>();
                CreateTablic();
            }
            private async void CreateTablic()
            {
                try
                {
                    StorageFolder f = ApplicationData.Current.LocalFolder;
                    StorageFile f1 = await f.CreateFileAsync("MD5Account.xml", CreationCollisionOption.OpenIfExists);
                    List<User> u;
                    XmlSerializer x = new XmlSerializer(typeof(List<User>));
                    Stream s = await f1.OpenStreamForReadAsync();
                    u = (List<User>)x.Deserialize(s);
                    foreach (User b in u) data.Add(new Tablic(b.Login, b.Name, b.Pol, b.BirthDay, b.Nomor, b.Select, b.Group));
                    s.Close();
                }
                catch { }
            }
        }
        private void refreshUserBTNCLICK(object sender, RoutedEventArgs e)
        {
            ViewModel viewModel = new ViewModel();
            dataG.ItemsSource = viewModel.data;
        }

        private async void addGroupBTNCLICK(object sender, RoutedEventArgs e)
        {
            try
            {
                await addGroup.ShowAsync();
            }
            catch { }
        }

        private async void delGroupBTNCLICK(object sender, RoutedEventArgs e)
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
                int result = 0;
                string[] text = File.ReadAllLines(file.Path);
                for (int i = 0; i < text.Length; i++) if (text[i].Contains(groupList.SelectedItem.ToString())) result = i;
                text[result + 1] = String.Empty;
                text[result] = String.Empty;
                text[result - 1] = String.Empty;
                File.WriteAllLines(file.Path, text);
                groupList.Items.Clear();
                LoadGroupList();
            }
            catch { }
        }
        //private void editGroupBTNCLICK(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (groupList.SelectedItem != null)
        //        {
        //            editGroupTXT.Visibility = Visibility.Visible;
        //            editGroupTXT.Text = groupList.SelectedItem.ToString();
        //            editGroupTXT.SelectAll();
        //            return;
        //        }
        //    }
        //    catch { }
        //}

        private void refreshGroupBTNCLICK(object sender, RoutedEventArgs e) => LoadGroupList();

        private void backBTN_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void addExcelBTN_Click(object sender, RoutedEventArgs ee)
        {
            if (groupList.SelectedItem == null)
            {
                MessageDialog message = new MessageDialog("Вы не выбрали группу");
                await message.ShowAsync();
                return;
            }
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
            //========================================
            StorageFolder storage1 = ApplicationData.Current.LocalFolder;
            StorageFile file1 = await storage1.CreateFileAsync("ParaData.xml", CreationCollisionOption.OpenIfExists);
            List<ParaData> list1;
            XmlSerializer x1 = new XmlSerializer(typeof(List<ParaData>));
            Stream stream1 = await file1.OpenStreamForReadAsync();
            try
            {
                list1 = (List<ParaData>)x1.Deserialize(stream1);
            }
            catch
            {
                list1 = new List<ParaData>();
            }
            stream1.Close();
            string[] mas = File.ReadAllLines(file.Path);
            int xs = 0;
            for (int i = 0; i < mas.Length; i++)
            {
                if (mas[i].Contains("<Discipline>"))
                {
                    xs++;
                }
            }
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            application.DefaultVersion = ExcelVersion.Excel2016;
            IWorkbook workbook = application.Workbooks.Create(xs);
            int xss = 0;
            foreach (Discipline discipline in list)
            {
                IWorksheet worksheet = workbook.Worksheets[xss];
                worksheet.Name = discipline.name;
                xss++;
            }
            int xsss = 2;
            int sx = 2;
            int sxx = 2;
            int sxxx = 1;
            for (int i = 0; i < xs; i++)
            {
                foreach (ParaData data in list1)
                {
                    if (groupList.SelectedItem.ToString() == data.Group && workbook.Worksheets[i].Name == data.Discipline)
                    {
                        workbook.Worksheets[i].Range[sx, 1].Text = data.Student;
                        workbook.Worksheets[i].Range[sxxx, 2].Text = data.Date;
                        workbook.Worksheets[i].Range[xsss, sxx].Text = data.Rating;
                        xsss++;
                        sxx++;
                        sxxx++;
                        sx++;
                    }
                }
                xsss = 2;
                sxx = 2;
                sx = 2;
                sxxx = 1;
            }
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.Desktop;
            savePicker.SuggestedFileName = groupList.SelectedItem.ToString();
            savePicker.FileTypeChoices.Add("Excel Files", new List<string>() { ".xlsx" });
            StorageFile storageFile = await savePicker.PickSaveFileAsync();
            await workbook.SaveAsAsync(storageFile);
        }

        //private async void editGroupTXTKEYDOWN(object sender, KeyRoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Key == VirtualKey.Enter)
        //        {
        //            StorageFolder storage = ApplicationData.Current.LocalFolder;
        //            StorageFile file = await storage.CreateFileAsync("Group.xml", CreationCollisionOption.OpenIfExists);
        //            List<Group> list;
        //            XmlSerializer x = new XmlSerializer(typeof(List<Group>));
        //            Stream stream = await file.OpenStreamForReadAsync();
        //            try
        //            {
        //                list = (List<Group>)x.Deserialize(stream);
        //            }
        //            catch
        //            {
        //                list = new List<Group>();
        //            }
        //            stream.Close();
        //            int result = 0;
        //            string[] text = File.ReadAllLines(file.Path);
        //            for (int i = 0; i < text.Length; i++) if (text[i].Contains(groupList.SelectedItem.ToString())) result = i;
        //            string stroka = text[result];
        //            text[result] = $"<GroupName>{editGroupTXT.Text}</GroupName>";
        //            File.WriteAllLines(file.Path, text);
        //            listDisp.Items.Clear();
        //            editDisciplineTXT.Visibility = Visibility.Collapsed;
        //            editGroupTXT.Visibility = Visibility.Collapsed;
        //            LoadGroupList();
        //        }
        //    }
        //    catch { }
        //}
    }
}
