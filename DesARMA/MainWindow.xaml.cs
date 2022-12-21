using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;
using MyForm = System.Windows.Forms;
using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.Util;
using NPOI.XWPF.Model;
using NPOI.XWPF.UserModel;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using static NPOI.HSSF.Util.HSSFColor;
using System.Reflection;
using NPOI.SS.Formula.Functions;
using DesARMA.Models;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Asn1.Misc;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.ComponentModel.Design.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace DesARMA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        //ApplicationContext db = new ApplicationContext();
        System.Windows.Controls.Button currentButton = new System.Windows.Controls.Button();
        //bool isLoad = false;
        public List<User> users = new List<User>();
        ModelContext modelContext = new ModelContext();
        public User CurrentUser { get; set; } = null!;
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes); // .NET 5 +

                // Convert the byte array to hexadecimal string prior to .NET 5
                // StringBuilder sb = new System.Text.StringBuilder();
                // for (int i = 0; i < hashBytes.Length; i++)
                // {
                //     sb.Append(hashBytes[i].ToString("X2"));
                // }
                // return sb.ToString();
            }
        }
        public MainWindow()
        {
            try
            {
                InitializeComponent();
            
                foreach (var item in modelContext.Users)
                {
                    users.Add(item);
                }

                //auth
               Auth();

                currentButton = AddButton;


                var blogs = from b in modelContext.DictCommons
                            where b.Domain == "ACCOST"
                            select b;


                List<string?> listTypeOrg = new List<string?>();
                List<string?> listOrgans = new List<string?>();
                foreach (var item in blogs)
                {
                    listTypeOrg.Add(item.Code);
                    listOrgans.Add(item.Name);
                }
                typeorgansList.ItemsSource = listTypeOrg;
                Reest.organsName = listTypeOrg;
                Reest.organs = listOrgans;





                LoadDb();
                //Button_ClickUpdate(new object(), new RoutedEventArgs());
                //isLoad = true;



            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
        }
        private void Auth()
        {
            while (true)
            {
                AuthWindow authWindow = new AuthWindow();
                if (authWindow.ShowDialog() == true)
                {
                    bool sh = false;
                    foreach (var item in users)
                    {
                        if (item.LoginName == authWindow.Login)
                        {
                            if (item.Password == CreateMD5(authWindow.Password))
                            {
                                CurrentUser = item;
                                sh = true;
                                nameVykon.Content = item.Name;
                                break;
                            }
                        }
                    }
                    if (sh)
                        break;
                    else
                        System.Windows.MessageBox.Show($"Неправильний логін або пароль ");
                }
                else
                {
                    //System.Windows.MessageBox.Show("Авторизація не пройдена");
                    Environment.Exit(0);
                }
            }
        }
        private void LoadDb()
        {
            try
            {
                int i = 0;
                foreach (var main in modelContext.Mains)
                {
                    
                    if (main.Executor == CurrentUser.IdUser)
                    {
                        var mcIs = modelContext.MainConfigs.Find(main.NumbInput);
                        if (mcIs == null) continue;

                        contShLabel.Content = $"Контроль/Схема  Розташування папки: {mcIs.Folder}";

                        //Кнопка запиту зліва
                        var itemButton = new System.Windows.Controls.Button();
                        itemButton.Click += Button_Any_Click_Req;

                        var NumForeground = 4;
                        var NumBackground = 2;

                        itemButton.Foreground = this.Resources[$"{NumForeground}ColorStyle"] as SolidColorBrush;
                        itemButton.Background = this.Resources[$"{NumBackground}ColorStyle"] as SolidColorBrush;

                        numberKPTextBox.Text = main.CpNumber;
                        numberInTextBox.Text = main.NumbInput;
                        dateInTextBox.Text = InStrDate(main.DtInput);
                        dateControlTextBox.Text = InStrDate(main.DtCheck);

                        ReadFromMainDBToCenter(main);

                        itemButton.Tag = main.Id;
                        itemButton.Content = $"Запит: {main.NumbInput}";
                        stackPanel1.Children.Insert(0, itemButton);

                        currentButton = itemButton;

                        treeView1.Items.Clear();
                        int index = 1;

                        foreach (var item in Reest.sNazyv)
                        {
                            var checkBox = new System.Windows.Controls.CheckBox();
                            checkBox.Checked += ClickOnCheckBox;
                            var checkBox2 = new System.Windows.Controls.CheckBox();
                            checkBox2.Checked += ClickOnCheckBox;

                            var parentItem = new TreeViewItem();
                            parentItem.Header = $"{index++}. " + item;
                            parentItem.Foreground = this.Resources[$"4ColorStyle"] as SolidColorBrush;

                            checkBox.Content = parentItem;
                            checkBox.Margin = new Thickness(0, 1, 0, 0);
                            checkBox2.Content = checkBox;
                            treeView1.Items.Add(checkBox2);
                        }
                        var checkBoxS = new System.Windows.Controls.CheckBox();
                        checkBoxS.Checked += ClickOnCheckBox;
                        var checkBoxS2 = new System.Windows.Controls.CheckBox();
                        checkBoxS2.Checked += ClickOnCheckBox;

                        var treeS = new TreeViewItem();
                        treeS.Foreground = new SolidColorBrush(Color.FromRgb(230, 78, 78));
                        treeS.Header = $"{index}. Схеми";

                        checkBoxS.Content = treeS;
                        checkBoxS.Margin = new Thickness(0, 1, 0, 0);
                        checkBoxS2.Content = checkBoxS;
                        treeView1.Items.Add(checkBoxS2);

                        //TODO read is checkb
                        var mcs = from b in modelContext.MainConfigs
                                 where b.NumbInput.Equals(numberInTextBox.Text)
                                 select b;

                        MainConfig? mc = null!;
                        foreach (var c in mcs)
                        {
                            mc = c;
                            break;
                        }

                        if (!ReadFromStringDBToCheckBoxes(mc))
                        {
                            modelContext.MainConfigs.Add(new MainConfig()
                            {
                                Control = new string('0', Reest.sNazyv.Count + 1),
                                Shema = new string('0', Reest.sNazyv.Count + 1),
                                Folder = null,
                                NumbInput = main.NumbInput
                            });
                        }
                        modelContext.SaveChanges();
                    }
                    i++;
                }


                for (int j = 1; j < stackPanel1.Children.Count; j++)
                {
                    var b = stackPanel1.Children[j] as System.Windows.Controls.Button;
                    if (b != null)
                    {
                        b.Background = this.Resources[$"3ColorStyle"] as SolidColorBrush;
                        b.Foreground = this.Resources[$"1ColorStyle"] as SolidColorBrush;
                    }
                }

                Button_ClickUpdate(new object(), new RoutedEventArgs());


            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }

        }
        private void Button_Any_Click_Req(object sender, RoutedEventArgs e)
        {
            try {

                var itemBut = (System.Windows.Controls.Button)sender;

                if (itemBut == currentButton) return;

                System.Windows.MessageBoxResult isSaveRes = System.Windows.MessageBox.Show(this, "Зберегти дані поточного запиту?",
                "Закрити", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);

                if (System.Windows.MessageBoxResult.Yes == isSaveRes)
                {
                    if (!SaveAllDB())
                        System.Windows.MessageBox.Show("Виникла помилка збереження");
                }


                //var prevM = modelContext.Mains.Find(numberInTextBox.Text);

                //if (prevM != null)
                //{
                //    SaveAllDB();
                //}
                //else
                //{
                //    System.Windows.MessageBox.Show("В базу дані не збережено");
                //}

                currentButton = itemBut;

                //Button_ClickUpdate(null, null);
                var mainR = from b in modelContext.Mains
                            where b.Id.Equals((decimal)currentButton.Tag)
                            select b;

                Main m = null!;
                foreach (var item in mainR)
                {
                    m = item;
                    break;
                }

                if (m != null)
                {
                    numberKPTextBox.Text = m.CpNumber;
                    dateInTextBox.Text = InStrDate(m.DtInput);
                    dateControlTextBox.Text = InStrDate(m.DtCheck);
                    numberInTextBox.Text = m.NumbInput;
                    ReadFromMainDBToCenter(m);

                    


                    for (int i = 0; i < stackPanel1.Children.Count - 1; i++)
                    {
                        var butI = stackPanel1.Children[i] as System.Windows.Controls.Button;
                        if (butI != null)
                        {
                            butI.Background = this.Resources[$"3ColorStyle"] as SolidColorBrush;
                            butI.Foreground = this.Resources[$"1ColorStyle"] as SolidColorBrush;
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Не вдалося задати колір фону та тексту кнопкам запиту");
                        }
                    }

                    itemBut.Background = this.Resources[$"2ColorStyle"] as SolidColorBrush;
                    itemBut.Foreground = this.Resources[$"4ColorStyle"] as SolidColorBrush;


                    //TODO Save chackB



                 
                    Button_ClickUpdate(new object(), new RoutedEventArgs());
                }
                else
                {
                    System.Windows.MessageBox.Show("Не вдалося завантажити запит із контексту локальної БД");
                }
            }
            catch (Exception e2)
            {
                System.Windows.MessageBox.Show(e2.Message);
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateRequestWindow createRequestWindow = new CreateRequestWindow();

                if (createRequestWindow.ShowDialog() == false)
                {
                    return;
                }

                var OracleMain = from b in modelContext.Mains
                                 where b.NumbInput.Equals(createRequestWindow.CodeRequest)
                                 select b;

                int count = 0;
                Main main = null!;
                foreach (var item in OracleMain)
                {
                    main = item;
                    count++;
                }

                if (count == 0)
                {
                    System.Windows.MessageBox.Show("Не вдалось відкрити запит, бо він не відкритий(не створений) в глобільній базі");
                    return;
                }
                if (!(main.Executor == CurrentUser.IdUser || main.Executor == null)) 
                {
                    var Us = modelContext.Users.Find(main.Executor);
                    if(Us!= null)
                    {
                        System.Windows.MessageBox.Show($"Не вдалось відкрити запит, бо він відкритий іншим виконавцем: {Us.Name}");
                    }
                    return;
                }
                var chif = modelContext.Users.Find(main.Chief);
                if(chif != null)
                {
                    if(chif.Employee != CurrentUser.Employee)
                    {
                        System.Windows.MessageBox.Show("Запит не доступний. Він відноситься до іншого управління");
                        return;
                    }
                }

                    var codeRequest = createRequestWindow.CodeRequest.Replace('/', '-')
                    .Replace('\\', '-')
                    .Replace(':', '-')
                    .Replace('*', '-')
                    .Replace('?', '-')
                    .Replace('\"', '-')
                    .Replace('<', '-')
                    .Replace('>', '-')
                    .Replace('|', '-')
                    ;


                

                var mcs = from b in modelContext.MainConfigs
                          where b.NumbInput.Equals(createRequestWindow.CodeRequest)
                          select b;

                foreach (var item in mcs)
                {
                    System.Windows.MessageBox.Show("Запит уже відкривався");
                    return;
                }
                


                MyForm.FolderBrowserDialog FBD = new MyForm.FolderBrowserDialog();
                if (FBD.ShowDialog() == MyForm.DialogResult.OK)
                {
                    if (currentButton != AddButton)
                    {

                        SaveAllDB();

                    }

                    main.Executor = CurrentUser.IdUser;

                    numberInTextBox.Text = createRequestWindow.CodeRequest;
                    numberKPTextBox.Text = main.CpNumber;
                    dateInTextBox.Text = InStrDate(main.DtInput);
                    dateControlTextBox.Text = InStrDate(main.DtCheck);
                    ReadFromMainDBToCenter(main);


                    int ind = stackPanel1.Children.IndexOf((System.Windows.Controls.Button)sender);
                    var itemButton = new System.Windows.Controls.Button();
                    itemButton.Click += Button_Any_Click_Req;
                    itemButton.Tag = main.Id;
                    var NumForeground = 4;  // stackPanel1.Children.Count % 2 == 1 ? 4 : 1;
                    var NumBackground = 2;  // stackPanel1.Children.Count % 2 == 1 ? 2 : 3;


                    itemButton.Foreground = this.Resources[$"{NumForeground}ColorStyle"] as SolidColorBrush;
                    itemButton.Background = this.Resources[$"{NumBackground}ColorStyle"] as SolidColorBrush;
                    itemButton.Content = $"Запит {ind + 1}: {createRequestWindow.CodeRequest}";


                    for (int i = 0; i < ind; i++)
                    {
                        var but = stackPanel1.Children[i] as System.Windows.Controls.Button;
                        if (but != null)
                        {
                            but.Background = this.Resources[$"3ColorStyle"] as SolidColorBrush;
                            but.Foreground = this.Resources[$"1ColorStyle"] as SolidColorBrush;
                        }
                        else
                        {
                            System.Windows.MessageBox.Show($"Не вдалося встановити фон і колір тексту {i + 1}кнопки запиту. Кнопка не знайдена null");
                        }
                    }

                    stackPanel1.Children.Insert(0, itemButton);


                    Directory.CreateDirectory(FBD.SelectedPath + $"\\{codeRequest}");
                    int index = 1;
                    treeView1.Items.Clear();
                    foreach (var item in Reest.sNazyv)
                    {
                        Directory.CreateDirectory(FBD.SelectedPath + $"\\{codeRequest}\\{index}. {item}");
                        var checkBox = new System.Windows.Controls.CheckBox();
                        var checkBox2 = new System.Windows.Controls.CheckBox();

                        var parentItem = new TreeViewItem();
                        parentItem.Header = $"{index++}. " + item;
                        parentItem.Foreground = this.Resources[$"4ColorStyle"] as SolidColorBrush;
                        checkBox.Content = parentItem;
                        checkBox.Margin = new Thickness(0, 1, 0, 0);
                        checkBox2.Content = checkBox;
                        treeView1.Items.Add(checkBox2);
                    }
                    var checkBoxS = new System.Windows.Controls.CheckBox();
                    var checkBoxS2 = new System.Windows.Controls.CheckBox();

                    var treeS = new TreeViewItem();
                    treeS.Foreground = this.Resources[$"4ColorStyle"] as SolidColorBrush;
                    treeS.Header = $"{index}. Схеми";

                    checkBoxS.Content = treeS;
                    checkBoxS.Margin = new Thickness(0, 1, 0, 0);
                    checkBoxS2.Content = checkBoxS;
                    treeView1.Items.Add(checkBoxS2);
                    Directory.CreateDirectory(FBD.SelectedPath + $"\\{codeRequest}\\{index}. Схеми");

                    currentButton = itemButton;
                    //treeView1.Items.Count
                    modelContext.MainConfigs.Add(new MainConfig() { Control = new string('0', treeView1.Items.Count), Folder = FBD.SelectedPath+ $"\\{codeRequest}" , NumbInput = main.NumbInput, Shema = new string('0', treeView1.Items.Count) });
                    modelContext.SaveChanges();
                }
                else
                {
                    System.Windows.MessageBox.Show("Запит не створено");
                    return;
                }
            }
            catch (Exception e2)
            {
                System.Windows.MessageBox.Show(e2.Message);
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            //int ind = stackPanelDelete.Children.IndexOf((Button)sender);
            //stackPanel1.Children.RemoveAt(ind);
            //stackPanelDelete.Children.RemoveAt(ind);

            //if(ind > 0)
            //    Button_Any_Click_Req(stackPanel1.Children[ind - 1], null);
            //else
            //{
            //    currentButton = AddButton;
            //    treeView1.Items.Clear();

            //    organTextBox.Text = "";
            //    nameTextBox.Text = "";
            //    address1TextBox.Text = "";
            //    date1TextBox.Text = "";
            //    date2TextBox.Text = "";
            //    number1TextBox.Text = "";
            //    number2TextBox.Text = "";
            //    count_ShematTextBox.Text = "";
            //    vidOrganTextBox.Text = "";
            //    typeorgansList.SelectedIndex = -1;
            //    indexSubCheckBox.IsChecked = false;
            //}
        }
        private void Button_ClickUpdate(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentButton == AddButton) return;


                var prevMc = modelContext.MainConfigs.Find(numberInTextBox.Text);
                

                var main = modelContext.Mains.Find(numberInTextBox.Text);
                ReadFromCheckBoxesToStringDB(prevMc);

                if (prevMc != null)
                {

                    var listContr = GetBoolsFromString(prevMc.Control);
                    var listSh = GetBoolsFromString(prevMc.Shema);

                    // Перевірка наявності папки запиту
                    if (Directory.Exists(prevMc.Folder))
                    {
                        //Отримання масиву вмісту папки запиту(реєстрів)
                        string[] dirs = Directory.GetDirectories(prevMc.Folder);

                        // Створення нового дерева
                        treeView1.Items.Clear();
                        int i = 1;

                        // Прохід по кожному реєстрі зі списку
                        foreach (string itemSNazyv in Reest.sNazyv)
                        {
                            //Створення вузла
                            TreeViewItem tree = new TreeViewItem();

                            var checkBox = new System.Windows.Controls.CheckBox();
                            checkBox.Checked += ClickOnCheckBox;
                            checkBox.IsChecked = listSh[i - 1];
                            var checkBox2 = new System.Windows.Controls.CheckBox();
                            checkBox2.Checked += ClickOnCheckBox;
                            checkBox2.IsChecked = listContr[i - 1];
                            checkBox.Content = tree;
                            checkBox.Margin = new Thickness(0, 1, 0, 0);
                            checkBox2.Content = checkBox;

                            // на самому початку задано червоний колір
                            tree.Foreground = new SolidColorBrush(Color.FromRgb(230, 78, 78));
                            tree.Header = $"{i}. " + itemSNazyv;

                            // пошук реєстру за порядеом в папці 
                            foreach (var itemDir in dirs)
                            {
                                // якщо знайдено папку з поточним реєстром
                                if ((new DirectoryInfo(itemDir)).Name == $"{i}. " + itemSNazyv)
                                {
                                    // перезадано на зелений колір
                                    tree.Foreground = new SolidColorBrush(Color.FromRgb(33, 194, 92));

                                    if (Directory.GetDirectories(itemDir).Length == 0 &&
                                         Directory.GetFiles(itemDir).Length == 0
                                    )
                                    {
                                        // якщо папка пуста перезадано на колір білий
                                        tree.Foreground = this.Resources[$"4ColorStyle"] as SolidColorBrush;

                                    }
                                    else
                                    {
                                        // якщо не пуста додаємо в середину вузли
                                        //tree.Items.Clear();
                                        foreach (var itemDIn in Directory.GetDirectories(itemDir))
                                        {
                                            TreeViewItem treeIn = new TreeViewItem();
                                            treeIn.Foreground = new SolidColorBrush(Color.FromRgb(33, 194, 92));
                                            treeIn.Header = new DirectoryInfo(itemDIn).Name;
                                            tree.Items.Add(treeIn);
                                        }
                                        foreach (var itemFIn in Directory.GetFiles(itemDir))
                                        {
                                            TreeViewItem treeIn = new TreeViewItem();
                                            treeIn.Foreground = new SolidColorBrush(Color.FromRgb(33, 194, 92));
                                            treeIn.Header = new FileInfo(itemFIn).Name;
                                            tree.Items.Add(treeIn);
                                        }
                                    }
                                    checkBox.Content = tree;
                                }

                            }
                            treeView1.Items.Add(checkBox2);
                            i++;
                        }

                        TreeViewItem treeS = new TreeViewItem();

                        var checkBoxS = new System.Windows.Controls.CheckBox();
                        checkBoxS.Checked += ClickOnCheckBox;
                        checkBoxS.IsChecked = listSh[i - 1];
                        var checkBoxS2 = new System.Windows.Controls.CheckBox();
                        checkBoxS2.Checked += ClickOnCheckBox;
                        checkBoxS2.IsChecked = listContr[i - 1];
                        checkBoxS.Content = treeS;
                        checkBoxS.Margin = new Thickness(0, 1, 0, 0);
                        checkBoxS2.Content = checkBoxS;

                        treeS.Foreground = new SolidColorBrush(Color.FromRgb(230, 78, 78));
                        treeS.Header = $"{i}. Схеми";
                        foreach (var item in dirs)
                        {
                            if ((new DirectoryInfo(item)).Name == $"{i}. Схеми")
                            {
                                treeS.Foreground = new SolidColorBrush(Color.FromRgb(33, 194, 92));
                                if (Directory.GetDirectories(item).Length == 0 &&
                                        Directory.GetFiles(item).Length == 0
                                   )
                                {
                                    // якщо папка пуста перезадано на колір білий
                                    treeS.Foreground = this.Resources[$"4ColorStyle"] as SolidColorBrush;

                                }
                                else
                                {
                                    foreach (var itemDIn in Directory.GetDirectories(item))
                                    {
                                        TreeViewItem treeIn = new TreeViewItem();
                                        treeIn.Foreground = new SolidColorBrush(Color.FromRgb(33, 194, 92));
                                        treeIn.Header = new DirectoryInfo(itemDIn).Name;
                                        treeS.Items.Add(treeIn);
                                    }
                                    foreach (var itemFIn in Directory.GetFiles(item))
                                    {
                                        TreeViewItem treeIn = new TreeViewItem();
                                        treeIn.Foreground = new SolidColorBrush(Color.FromRgb(33, 194, 92));
                                        treeIn.Header = new FileInfo(itemFIn).Name;
                                        treeS.Items.Add(treeIn);
                                    }
                                }
                                treeView1.Items.Add(checkBoxS2);
                            }
                        }
                    }
                    else
                    {
                        foreach (TreeViewItem item in treeView1.Items)
                        {
                            item.Foreground = new SolidColorBrush(Color.FromRgb(230, 78, 78));
                        }
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Не вдалося зберегти поточний запит. Не знайдено запит в контексті локальної бази");
                }
            }
            catch (Exception e2)
            {
                System.Windows.MessageBox.Show(e2.Message);
            }
        }
        private void Button_ClickRespon(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentButton == AddButton) return;
                int iPrap = 1;
                var prevM = modelContext.MainConfigs.Find(numberInTextBox.Text);
                if(prevM == null) return;

                foreach (var item in treeView1.Items)
                {
                    var chB = item as System.Windows.Controls.CheckBox;
                    if (chB != null)
                    {
                        var b = chB.IsChecked;
                        if (b != null)
                        {
                            if (!(bool)b)
                            {
                                var b2 = chB.Content as System.Windows.Controls.CheckBox;

                                if(b2 != null)
                                {
                                    var tr = b2.Content as System.Windows.Controls.TreeViewItem;
                                    if(tr != null)
                                    {
                                        if (Directory.Exists(prevM.Folder + "\\" + tr.Header)) 
                                        {
                                            System.Windows.MessageBox.Show($"Не відмічено контроль");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        System.Windows.MessageBox.Show($"Помилка CheckBox");
                                        return;
                                    }
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show($"Помилка CheckBox");
                                    return;
                                } 
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show($"Не відмічено контроль(null)");
                            return;
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show($"Прапорець номер {iPrap} контролю не визначений");
                        return;
                    }
                    iPrap++;
                }

                var isCountFig = (from b in modelContext.Figurants
                           where b.NumbInput == numberInTextBox.Text &&
                                         b.Status == false
                           select b).ToList().Count;
                if (isCountFig == 0)
                {
                    System.Windows.MessageBox.Show($"Не вибрано жодного фігуранта");
                    return;
                }


                string path3 = "";
                //Створення документу звіту
                
                XWPFDocument doc1;
                if (prevM != null)
                {
                    Directory.CreateDirectory(prevM.Folder + "\\Відповідь");

                    var exeFath = /*AppDomain.CurrentDomain.BaseDirectory*/  Environment.CurrentDirectory;
                    var path = System.IO.Path.Combine(exeFath, "Files\\1.docx");

                    FileInfo fileInfo = new FileInfo(path);

                    var path2 = System.IO.Path.Combine(exeFath, "FilesRet\\2.docx");
                    path3 = prevM.Folder + $"\\Відповідь\\Відповідь {prevM.Folder.Split('\\').Last()}.docx";

                    fileInfo.CopyTo(path3, true);
                    
                    doc1 = new XWPFDocument(OPCPackage.Open(path3));
                }
                else
                {
                    System.Windows.MessageBox.Show($"Не вдалося відкрити контекст запиту. Не знайдено його");
                    return;
                }

                var indexSub = isCountFig > 1 ? 0 : 1;
                //Створення зміних, що вставляються в звіт
                int whatIndex = 0; //type organ
                whatIndex = typeorgansList.SelectedIndex;
                string name = nameSubTextBox.Text;
                string address1 = addressOrgTextBox.Text;
                string date1 = dateRequestDatePicker.Text;
                string date2 = dateInTextBox.Text.Substring(0, 10);
                string number1 = numberRequestTextBox.Text;
                string number2 = numberInTextBox.Text;
                int count_Shemat = 0;
                string vidOrgan = vidOrgTextBox.Text;
                string positionSub = positionSubTextBox.Text;

                    //Створення списків реєстрів родовий і давальний
                    List<string> listSRod = new List<string>();
                    List<string> listSDav = new List<string>();


                //Збереження даних наявностей реєстрів
                if (Directory.Exists(prevM.Folder))
                    {
                    int i = 1;
                    foreach (string itemSNazyv in Reest.sNazyv)
                    {
                        string[] dirs = Directory.GetDirectories(prevM.Folder);
                        foreach (var itemDir in dirs)
                        {
                            if ((new DirectoryInfo(itemDir)).Name == $"{i}. " + itemSNazyv)
                            {
                                if (Directory.GetDirectories(itemDir).Length > 0 ||
                                     Directory.GetFiles(itemDir).Length > 0
                                )
                                {
                                    listSRod.Add(Reest.sRodov[Reest.sNazyv.IndexOf(itemSNazyv)]);
                                }
                                else
                                {
                                    listSDav.Add(Reest.sDav[Reest.sNazyv.IndexOf(itemSNazyv)]);
                                }
                            }
                        }
                        i++;
                    }
                    var d = Directory.GetDirectories(prevM.Folder);
                    foreach (var item in d)
                    {
                        if ((new DirectoryInfo(item)).Name == $"{i}. Схеми")
                        {
                            var countD = Directory.GetDirectories(item).Length;
                            var countF = Directory.GetFiles(item).Length;
                            count_Shemat = countD + countF;
                            break;
                        }
                    }
                }
                else
                {
                        System.Windows.MessageBox.Show("Не знайдено папку запиту");
                        doc1.Close();
                        return;
                }



                    //Формування параграфів
                    var par = doc1.Paragraphs[22];
                    par.ReplaceText(par.Text, positionSub);


                    par = doc1.Paragraphs[21];
                    par.ReplaceText(par.Text, vidOrgan);

                    par = doc1.Paragraphs[23];
                    par.ReplaceText(par.Text, name);

                    par = doc1.Paragraphs[25];
                    par.ReplaceText(par.Text, address1);

                    par = doc1.Paragraphs[26];
                    par.ReplaceText(par.Text, "");

                    par = doc1.Paragraphs[30];
                    par.ReplaceText("14.12.2021 № 65/16/6133 (вх. № 6018/27-21 від 21.12.2022)", $"{date1} № {number1} (вх. № {number2} від {date2})");
                    par.ReplaceText("зазначених", Reest.sub[indexSub]);
                    par.ReplaceText("осіб", Reest.sub2[indexSub]);


                    par = doc1.Paragraphs[31];

                    if (count_Shemat > 0)
                        par.ReplaceText("додаток 10-11", $"додаток {listSRod.Count + 1}");

                    par.ReplaceText("зазначених", Reest.sub[indexSub]);
                    par.ReplaceText("осіб", Reest.sub2[indexSub]);

                    par = doc1.Paragraphs[32];
                    par.ReplaceText("зазначених", Reest.sub[indexSub]);
                    par.ReplaceText("осіб", Reest.sub2[indexSub]);

                    

                    par = doc1.Paragraphs[33];
                    par.ReplaceText("зазначених", Reest.sub[indexSub]);
                    par.ReplaceText("осіб", Reest.sub2[indexSub]);

                    //System.Windows.MessageBox.Show(par.Text);

                    par = doc1.Paragraphs[34];
                        if(whatIndex != -1)
                            par.ReplaceText(par.Text, Reest.organs[whatIndex]);
                        else
                            par.ReplaceText(par.Text, Reest.organs[0]);

                     for (int i = 0; i < listSRod.Count; i++)
                        {
                            var tmpParagraph = doc1.CreateParagraph();
                            tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                            tmpParagraph.IndentationFirstLine = 570;
                            var tmpRun = tmpParagraph.CreateRun();
                            tmpRun.FontSize = 14;
                        }
                    // пересунуть 1 ліст
                    for (int i = doc1.Paragraphs.Count - listSRod.Count - 1; i >= 31; i--)
                    {
                        var tmpParagraph = doc1.Paragraphs[i];
                        doc1.SetParagraph(tmpParagraph, i + listSRod.Count);
                    }

                    // засунуть 1 ліст
                    int count_dodat = 0;
                    foreach (var item in listSRod)
                    {
                        var tmpParagraph = doc1.CreateParagraph();
                        doc1.SetParagraph(tmpParagraph, 31 + count_dodat);
                        tmpParagraph.IndentationFirstLine = 570;
                        tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                        var tmpRun = tmpParagraph.CreateRun();
                        tmpRun.AppendText($"{count_dodat + 1}) ");
                        if (count_dodat == listSRod.Count - 1)
                            tmpRun.AppendText($"{item} (додаток {count_dodat + 1}).");
                        else
                            tmpRun.AppendText($"{item} (додаток {count_dodat + 1});");
                        tmpRun.FontSize = 14;
                        count_dodat++;
                    }
                    //remove
                    for (int i = 0; i < listSRod.Count; i++)
                    {
                        int pPos = doc1.GetPosOfParagraph(doc1.Paragraphs[doc1.Paragraphs.Count - 1]);
                        doc1.RemoveBodyElement(pPos);
                    }

                    // в кінець 2 ліст пустий
                    for (int i = 0; i < listSDav.Count; i++)
                    {
                        var tmpParagraph = doc1.CreateParagraph();
                        tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                        tmpParagraph.IndentationFirstLine = 570;
                        var tmpRun = tmpParagraph.CreateRun();
                        tmpRun.FontSize = 14;
                    }

                    // пересунуть 2 ліст
                    for (int i = doc1.Paragraphs.Count - listSDav.Count - 1; i >= 33 + listSRod.Count; i--)
                    {
                        var tmpParagraph = doc1.Paragraphs[i];
                        doc1.SetParagraph(tmpParagraph, i + listSDav.Count);
                    }

                    // встувить 2 ліст
                    for (int i = 0; i < listSDav.Count; i++)
                    {
                        var tmpParagraph = doc1.CreateParagraph();
                        doc1.SetParagraph(tmpParagraph, 33 + listSRod.Count + i);
                        tmpParagraph.IndentationFirstLine = 570;
                        tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                        var tmpRun = tmpParagraph.CreateRun();

                        if (i == listSDav.Count - 1)
                            tmpRun.AppendText($"- {listSDav[i]}.");
                        else
                            tmpRun.AppendText($"- {listSDav[i]};");

                        tmpRun.FontSize = 14;
                    }
                   

                    //remove
                    for (int i = 0; i < listSDav.Count; i++)
                    {
                        int pPos = doc1.GetPosOfParagraph(doc1.Paragraphs[doc1.Paragraphs.Count - 1]);
                        doc1.RemoveBodyElement(pPos);
                    }


                    par = doc1.Paragraphs[doc1.Paragraphs.Count - 7];
                    par.ReplaceText(par.Text, $"Примірник № 1 - {vidOrgTextBox.Text}");


                    if (count_Shemat == 0)
                    {
                        for (int i = 32 + listSRod.Count; i < doc1.Paragraphs.Count; i++)
                        {
                            var tmpParagraph = doc1.Paragraphs[i];
                            doc1.SetParagraph(tmpParagraph, i - 1);
                        }
                        doc1.SetParagraph(doc1.Paragraphs[0], doc1.Paragraphs.Count - 1);
                    }



                    var path4 = prevM.Folder + "\\Відповідь\\Відповідь.docx";

                    //Збереження звіта 
                    using (FileStream sw = File.Create(path4))
                    {
                        doc1.Write(sw);
                        // doc1.Close();
                    }

                    doc1.Close();
                    File.Delete(path4);
                    System.Windows.MessageBox.Show($"Відповідь збережено в папку:\n{path3}");
               
            }
            catch (Exception e2)
            {
                System.Windows.MessageBox.Show(e2.Message);
               // doc1.Close();
            }


        }
        private void ButtonSearchClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var blogs = from b in modelContext.Mains
                            where b.NumbInput.Equals(textBoxSearch.Text) && b.Executor == CurrentUser.IdUser
                            select b;
                var s = "";

                foreach (var item in blogs)
                {
                   // ind = item.Id;

                    s = item.NumbInput;

                    break;
                }
                if (s != "")
                    CreateButton(s);
                 //System.Windows.MessageBox.Show(s);
            }
            catch (Exception e2)
            {
                System.Windows.MessageBox.Show(e2.Message);
            }
        }
        private void ClickDefendantsButton(object sender, RoutedEventArgs e)
        {
            
            try
            {
                var mainR = from b in modelContext.Mains
                            where b.Id.Equals((decimal)currentButton.Tag)
                            select b;

                Main m = null!;
                foreach (var item in mainR)
                {
                    m = item;
                    break;
                }

                if (m != null) 
                {

                    ListDefendantsWindow listDefendantsWindow = new ListDefendantsWindow(modelContext, 
                        numberInTextBox.Text, "Перелік фігурантів", false);

                    if (listDefendantsWindow.ShowDialog() == true)
                    {

                    }
                    else
                    {

                    }

                }
                else
                {
                    System.Windows.MessageBox.Show("Не визначено запит.");
                }

            }
            catch (Exception e2)
            {
                System.Windows.MessageBox.Show(e2.Message);
            }
        }
        private void CreateButton(string numbInput)
        {
            try
            {
                var reqItAdd = modelContext.Mains.Find(numbInput);

                if (reqItAdd != null)
                {
                    stackPanel1.Children.Clear();

                    int i = 1;
                    foreach (var m in modelContext.Mains)
                    {
                        if (m.NumbInput != numbInput && m.Executor == CurrentUser.IdUser)
                        {
                            var itemButton = new System.Windows.Controls.Button();
                            itemButton.Click += Button_Any_Click_Req;


                            var NumForeground2 = 4;  // stackPanel1.Children.Count % 2 == 1 ? 4 : 1;
                            var NumBackground2 = 2;  // stackPanel1.Children.Count % 2 == 1 ? 2 : 3;

                            itemButton.Foreground = this.Resources[$"{NumForeground2}ColorStyle"] as SolidColorBrush;
                            itemButton.Background = this.Resources[$"{NumBackground2}ColorStyle"] as SolidColorBrush;


                            itemButton.Tag = m.Id;

                            itemButton.Content = $"Запит: {m.NumbInput}";
                            stackPanel1.Children.Insert(0, itemButton);

                            i++;
                        }

                    }
                    for (int j = 0; j < stackPanel1.Children.Count; j++)
                    {
                        var b = stackPanel1.Children[j] as System.Windows.Controls.Button;
                        if (b != null)
                        {
                            b.Background = this.Resources[$"3ColorStyle"] as SolidColorBrush;
                            b.Foreground = this.Resources[$"1ColorStyle"] as SolidColorBrush;
                        }
                    }


                    var itemButton2 = new System.Windows.Controls.Button();
                    itemButton2.Click += Button_Any_Click_Req;

                    var NumForeground = 4;  // stackPanel1.Children.Count % 2 == 1 ? 4 : 1;
                    var NumBackground = 2;  // stackPanel1.Children.Count % 2 == 1 ? 2 : 3;

                    itemButton2.Foreground = this.Resources[$"{NumForeground}ColorStyle"] as SolidColorBrush;
                    itemButton2.Background = this.Resources[$"{NumBackground}ColorStyle"] as SolidColorBrush;



                    itemButton2.Tag = reqItAdd.Id;
                    itemButton2.Content = $"Запит: {reqItAdd.NumbInput}";
                    stackPanel1.Children.Insert(0, itemButton2);

                    //currentButton = itemButton2;


                    var itemButtonAdd = new System.Windows.Controls.Button();
                    itemButtonAdd.Click += Button_Click_1;
                    itemButtonAdd.Name = "AddButton";
                    itemButtonAdd.Content = "+";
                    itemButtonAdd.Foreground = this.Resources[$"{1}ColorStyle"] as SolidColorBrush;
                    itemButtonAdd.Background = this.Resources[$"{4}ColorStyle"] as SolidColorBrush;
                    stackPanel1.Children.Add(itemButtonAdd);

                    Button_Any_Click_Req(currentButton, new RoutedEventArgs());
                }
            }
            catch (Exception e2)
            {
                System.Windows.MessageBox.Show(e2.Message);
            }
        }
        private void ClickСonnectedPeopleButton(object sender, RoutedEventArgs e)
        {
            try
            {
                var mainR = from b in modelContext.Mains
                            where b.Id.Equals((decimal)currentButton.Tag)
                            select b;

                Main m = null!;
                foreach (var item in mainR)
                {
                    m = item;
                    break;
                }

                if (m != null)
                {
                    
                    ListDefendantsWindow listDefendantsWindow = new ListDefendantsWindow(modelContext, numberInTextBox.Text, "Перелік фігурантів", true);

                    if (listDefendantsWindow.ShowDialog() == true)
                    {

                    }
                    else
                    {

                    }


                }
            }
            catch (Exception e2)
            {
                System.Windows.MessageBox.Show(e2.Message);
            }
        }
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                System.Windows.Controls.TextBox dp = (System.Windows.Controls.TextBox)sender;
                var i = dp.SelectionStart;
                string newstr = "";
                if ("0123456789".IndexOf(dp.Text[0]) == -1)
                {
                    newstr += "_";
                }
                else
                {
                    newstr += dp.Text[0];
                }

                if (dp.Text.Length > 1)
                    if ("0123456789".IndexOf(dp.Text[1]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[1];
                    }
                newstr += ".";

                if (dp.Text.Length > 3)
                    if ("0123456789".IndexOf(dp.Text[3]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[3];
                    }

                if (dp.Text.Length > 4)
                    if ("0123456789".IndexOf(dp.Text[4]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[4];
                    }


                newstr += ".";

                if (dp.Text.Length > 6)
                    if ("0123456789".IndexOf(dp.Text[6]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[6];
                    }

                if (dp.Text.Length > 7)
                    if ("0123456789".IndexOf(dp.Text[7]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[7];
                    }

                if (dp.Text.Length > 8)
                    if ("0123456789".IndexOf(dp.Text[8]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[8];
                    }

                if (dp.Text.Length > 9)
                    if ("0123456789".IndexOf(dp.Text[9]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[9];
                    }

                dp.Text = newstr;
                if (i == 2 && dp.Text[2] == '.') i++;
                if (i == 5 && dp.Text[5] == '.') i++;
                dp.SelectionStart = i;

            }
            catch (Exception exp)
            {
                System.Windows.MessageBox.Show(exp.Message);
            }
        }
        private void textBox_MouseDown(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                System.Windows.Controls.TextBox dp = (System.Windows.Controls.TextBox)sender;
                dp.SelectionStart = 0;
            }
            catch (Exception exp)
            {
                System.Windows.MessageBox.Show(exp.Message);
            }
        }
        private void App_Closing(object sender, CancelEventArgs e)
        {

            System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show(this, "Вийти?",
                "Закрити", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
            //SaveReqLocalGlob();
            if (System.Windows.MessageBoxResult.No == result)
            {
                e.Cancel = true;
            }
            else
            {
                System.Windows.MessageBoxResult isSaveRes = System.Windows.MessageBox.Show(this, "Зберегти дані поточного запиту?",
                "Закрити", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);

                if(System.Windows.MessageBoxResult.Yes == isSaveRes)
                {
                    if (!SaveAllDB())
                        System.Windows.MessageBox.Show("Виникла помилка збереження");
                }
            }

        }
        private DateTime? IsCorectDate(string value)
        {
            if (value.Length != 10) return null;

            string day = value.Substring(0, 2);
            string month = value.Substring(3, 2);
            string year = value.Substring(6, 4);

            int dayInt = 0;
            int monthInt = 0;
            int yearInt = 0;

            bool successDay = int.TryParse(day, out dayInt);
            bool successMath = int.TryParse(month, out monthInt);
            bool successYear = int.TryParse(year, out yearInt);

            if (successDay && successMath && successYear)
                return new DateTime(yearInt, monthInt, dayInt);

            return null;
        }
        private string InStrDate(DateTime? dateTime)
        {
            if (dateTime != null)
            {
                var t = dateTime.ToString();
                if (t != null)
                    return $"{t[0]}{t[1]}.{t[3]}{t[4]}.{t[6]}{t[7]}{t[8]}{t[9]}";
            }

            return "";
        }
        private void DatePicker_TextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                System.Windows.Controls.DatePicker dp = (System.Windows.Controls.DatePicker)sender;

                string newstr = "";
                if ("0123456789".IndexOf(dp.Text[0]) == -1)
                {
                    newstr += "_";
                }
                else
                {
                    newstr += dp.Text[0];
                }

                if (dp.Text.Length > 1)
                    if ("0123456789".IndexOf(dp.Text[1]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[1];
                    }


                newstr += ".";

                if (dp.Text.Length > 3)
                    if ("0123456789".IndexOf(dp.Text[3]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[3];
                    }

                if (dp.Text.Length > 4)
                    if ("0123456789".IndexOf(dp.Text[4]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[4];
                    }


                newstr += ".";

                if (dp.Text.Length > 6)
                    if ("0123456789".IndexOf(dp.Text[6]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[6];
                    }

                if (dp.Text.Length > 7)
                    if ("0123456789".IndexOf(dp.Text[7]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[7];
                    }

                if (dp.Text.Length > 8)
                    if ("0123456789".IndexOf(dp.Text[8]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[8];
                    }

                if (dp.Text.Length > 9)
                    if ("0123456789".IndexOf(dp.Text[9]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[9];
                    }

                dp.Text = newstr;

            }
            catch (Exception exp)
            {
                System.Windows.MessageBox.Show(exp.Message);
            }
        }
        private void ClickReqButton(object sender, RoutedEventArgs e)
        {
            if(!(numberInTextBox.Text == null || numberInTextBox.Text == ""))
            {
                var items = from f in modelContext.Figurants
                            where f.NumbInput == numberInTextBox.Text && f.Status == false
                            select f;
                if (items.ToList().Count == 0)
                {
                    System.Windows.MessageBox.Show("Не додано фігурантів");
                }
                else
                {
                    RequestsWindow requestsWindow = new RequestsWindow(numberInTextBox.Text.ToString(), modelContext);
                   // ProgresWindow progresWindow = new ProgresWindow();
                   // progresWindow.ShowDialog();
                    if (requestsWindow.ShowDialog() == true)
                    {

                    }
                    else
                    {

                    }
                }
            }
            


            //RequestsWindow requestsWindow = new RequestsWindow(numberInTextBox.Text.ToString(), modelContext);

            //if (requestsWindow.ShowDialog() == true)
            //{

            //}
            //else
            //{

            //}
        }
        private bool SaveAllDB()
        {
            bool ret = true;

            var mainR = from b in modelContext.Mains
                        where b.NumbInput.Equals(numberInTextBox.Text)
                        select b;

            Main m = null!;
            foreach (var item in mainR)
            {
                m = item;
                break;
            }

            ret = ret && ReadFromCenterToMainDB(m);
            
                var mcs = from b in modelContext.MainConfigs
                          where b.NumbInput.Equals(numberInTextBox.Text)
                          select b;

                MainConfig mc = null!;
                foreach (var item in mcs)
                {
                    mc = item;
                    break;
                }

            ret = ret && ReadFromCheckBoxesToStringDB(mc);

            return ret;
        }
        private bool ReadFromMainDBToCenter(Main m)
        {
            if (m != null)
            {
                typeorgansList.SelectedIndex = GetSelIndexFromTypeOrgan(m.IdAcc);
                numberRequestTextBox.Text = m.NumbOutInit;
                dateRequestDatePicker.SelectedDate = m.DtOutInit;
                vidOrgTextBox.Text = m.AgencyDep;
                addressOrgTextBox.Text = m.Addr;
                positionSubTextBox.Text = m.Work;
                nameSubTextBox.Text = m.ExecutorInit;
                numberOutTextBox.Text = m.NumbOut;
                dateOutDatePicker.SelectedDate = m.DtOut;
                co_executorTextBox.Text = m.CoExecutor;
                TEKATextBox.Text = m.Folder;
                article_CCUTextBox.Text = m.Art;
                noteTextBox.Text = m.Notes;
               // typeorgansList.SelectedIndex = GetTypeOrgFromDB(m);
                int typeA = -1;
                int.TryParse(m.Status, out typeA);
                //if (!)
                //{System.Windows.MessageBox.Show($"Не вдалося зчитати Статус звернення з бази запату {m.NumbInput}");
                //}
                typeAppealList.SelectedIndex = typeA;

                return true;
            }
            else
                return false;
        }
        private bool ReadFromCenterToMainDB(Main m)
        {
            if (m != null)
            {
                m.IdAcc = GetIdFromDicForNameTypeOrgan(typeorgansList.SelectedIndex);
                m.NumbOutInit = numberRequestTextBox.Text;
                m.DtOutInit = dateRequestDatePicker.SelectedDate;
                m.AgencyDep = vidOrgTextBox.Text;
                m.Addr = addressOrgTextBox.Text;
                m.Work = positionSubTextBox.Text;
                m.ExecutorInit = nameSubTextBox.Text;
                m.NumbOut = numberOutTextBox.Text;
                m.DtOut = dateOutDatePicker.SelectedDate;
                m.CoExecutor = co_executorTextBox.Text;
                m.Folder = TEKATextBox.Text;
                m.Art = article_CCUTextBox.Text;
                m.Notes = noteTextBox.Text;
                m.Status = (typeAppealList.SelectedIndex + 1).ToString() /*== 1 ? "в роботі": "закрито"*/;
                modelContext.SaveChanges();
                return true;
            }
            else
                return false;
        }
        private int GetSelIndexFromTypeOrgan(decimal? idFromDic)
        {
            if(idFromDic == null) return -1;


            foreach (var item in modelContext.DictCommons)
            {
                if(item.Id == idFromDic)
                {
                    for (int i = 0; i < typeorgansList.Items.Count; i++)
                    {
                        if (item.Code == typeorgansList.Items[i].ToString())
                        {
                            return i;
                        }
                    }
                }
            }

           
            return -1;
        }
        private decimal? GetIdFromDicForNameTypeOrgan(int indLoc)
        {

            if(indLoc != -1)
            {
                var blogs = from b in modelContext.DictCommons
                            where b.Domain == "ACCOST" && b.Code == typeorgansList.SelectedValue
                            select b;

                foreach (var item in blogs)
                {
                    return item.Id;
                }

            }
            return null;
        }
        private bool ReadFromStringDBToCheckBoxes(MainConfig? mc)
        {
            if (mc == null) return false;

            var listCont = GetBoolsFromString(mc.Control);

            int k = 0;
            foreach (var item in listCont)
            {
                if (treeView1.Items.Count >= k)
                {
                    var trV = treeView1.Items[k++] as System.Windows.Controls.CheckBox;
                    if (trV != null)
                    {
                        trV.IsChecked = item as bool?;
                    }
                }
            }

            var listShema = GetBoolsFromString(mc.Shema);

            k = 0;
            foreach (var item in listShema)
            {
                if (treeView1.Items.Count >= k)
                {
                    var trV = treeView1.Items[k++] as System.Windows.Controls.CheckBox;
                    if (trV != null)
                    {
                        var trVC = trV.Content as System.Windows.Controls.CheckBox;
                        if (trVC != null)
                        {
                            trVC.IsChecked = item as bool?;
                        }
                    }
                }
            }
            return true;
        }
        private bool ReadFromCheckBoxesToStringDB(MainConfig? mc)
        {
            bool ret = true;

            if (mc == null) return false;
            List<bool> listC = new List<bool>();
            List<bool> listS = new List<bool>();
            foreach (var item in treeView1.Items)
            {
                if(item != null)
                {
                    var cb = item as System.Windows.Controls.CheckBox;
                    if (cb != null)
                    {
                        var isc = cb.IsChecked;
                        if (isc != null)
                        {
                            listC.Add((bool)isc);
                        }
                        else
                        {
                            listS.Add(false);
                        }

                        var cbC = cb.Content as System.Windows.Controls.CheckBox;
                        if (cbC!=null)
                        {
                            var iscbC = cbC.IsChecked;
                            if(iscbC != null)
                            {
                                listS.Add((bool)iscbC);
                            }
                            else
                            {
                                listS.Add(false);
                            }
                            
                        }
                    }
                    else
                    {
                        ret = false;
                    }


                }
                else
                {
                    ret = false;
                }
                
                
            }


            var strC = GetStringFromBools(listC);
            var strS = GetStringFromBools(listS);

            mc.Control = strC;
            mc.Shema = strS;
            modelContext.SaveChanges();

            return true;
        }
        private void Button_ClickSave(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show(this, "Зберегти дані запиту?",
                "Закрити", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);

                if (System.Windows.MessageBoxResult.Yes == result)
                {
                    if (SaveAllDB())
                        System.Windows.MessageBox.Show("Дані запиту збережено");
                    else
                        System.Windows.MessageBox.Show("Виникла помилка збереження");
                }
            }
            catch (Exception e2)
            {
                System.Windows.MessageBox.Show(e2.Message);
            }

        }
        private List<bool> GetBoolsFromString(string? str)
        {
            if (str == null) return null!;

            List<bool> ret = new List<bool>();
            foreach (var item in str)
            {
                if (item == '1')
                {
                    ret.Add(true);
                }
                else
                {
                    ret.Add(false);
                }
            }
            return ret;
        }
        private string GetStringFromBools(List<bool> list)
        {
            var st = "";

            foreach (var item in list)
            {
                st += item? "1" : "0";
            }
            return st;
        }

        private void ToDiskButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentButton == AddButton) return;

                var prevMc = modelContext.MainConfigs.Find(numberInTextBox.Text);

                if (Directory.Exists(prevMc.Folder))
                {
                    //Отримання масиву вмісту папки запиту(реєстрів)
                    string[] dirs = Directory.GetDirectories(prevMc.Folder);
                    int i = 1;
                    int numDod = 1;
                    // Прохід по кожному реєстрі зі списку
                    foreach (string itemSNazyv in Reest.sNazyv)
                    {
                        // пошук реєстру за порядеом в папці 
                        foreach (var itemDir in dirs)
                        {
                            // якщо знайдено папку з поточним реєстром
                            if ((new DirectoryInfo(itemDir)).Name == $"{i}. " + itemSNazyv)
                            {
                               
                                if (!(Directory.GetDirectories(itemDir).Length == 0 &&
                                     Directory.GetFiles(itemDir).Length == 0)
                                )
                                {
                                    Directory.CreateDirectory(prevMc.Folder + $"\\На диск\\Додаток {numDod}");
                                    perebor_updates(prevMc.Folder + $"\\{i}. " + itemSNazyv, prevMc.Folder + $"\\На диск\\Додаток {numDod++}");

                                }
                            }

                        }
                        i++;
                       
                    }
                    System.Windows.MessageBox.Show("Додатки збережено в папку: " + prevMc.Folder + "\\На диск");
                }
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
            }
        }

        void perebor_updates(string begin_dir, string end_dir)
        {
            //Берём нашу исходную папку
            DirectoryInfo dir_inf = new DirectoryInfo(begin_dir);
            //Перебираем все внутренние папки
            foreach (DirectoryInfo dir in dir_inf.GetDirectories())
            {
                //Проверяем - если директории не существует, то создаем;
                if (Directory.Exists(end_dir + "\\" + dir.Name) != true)
                {
                    Directory.CreateDirectory(end_dir + "\\" + dir.Name);
                }

                //Рекурсия (перебираем вложенные папки и делаем для них то-же самое).
                perebor_updates(dir.FullName, end_dir + "\\" + dir.Name);
            }

            //Перебираем файлы в папке источнике.
            foreach (string file in Directory.GetFiles(begin_dir))
            {
                //Определяем (отделяем) имя файла с расширением - без пути (но с слешем "").
                string filik = file.Substring(file.LastIndexOf('\\'), file.Length - file.LastIndexOf('\\'));
                //Копируем файл с перезаписью из источника в приёмник.
                File.Copy(file, end_dir + "\\" + filik, true);
            }
        }

        private void ChangeFolderButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentButton == AddButton) return;

                var prevMc = modelContext.MainConfigs.Find(numberInTextBox.Text);

                if (prevMc != null)
                {
                    if (Directory.Exists(prevMc.Folder))
                    {
                        System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show(this, "Папку запиту знайдено. Скопіювати і зберегти дані?",
                    "Закрити", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);

                        if (System.Windows.MessageBoxResult.Yes == result)
                        {
                            MyForm.FolderBrowserDialog FBD = new MyForm.FolderBrowserDialog();
                            if (FBD.ShowDialog() == MyForm.DialogResult.OK)
                            {
                                var strF = numberInTextBox.Text.Replace('/', '-')
                                        .Replace('\\', '-')
                                        .Replace(':', '-')
                                        .Replace('*', '-')
                                        .Replace('?', '-')
                                        .Replace('\"', '-')
                                        .Replace('<', '-')
                                        .Replace('>', '-')
                                        .Replace('|', '-');
                                perebor_updates(prevMc.Folder, FBD.SelectedPath + "\\" + strF);
                                prevMc.Folder = FBD.SelectedPath + "\\" + strF;
                                modelContext.SaveChanges();
                                contShLabel.Content = $"Контроль/Схема  Розташування папки: " + FBD.SelectedPath + "\\" + strF;
                                System.Windows.MessageBox.Show("Папку запиту переміщено до " + FBD.SelectedPath + "\\" + strF);
                            }
                        }
                    }
                    else
                    {
                        System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show(this, "Папку запиту не знайдено. Перестворити і зберегти дані?",
                    "Закрити", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
                        if (System.Windows.MessageBoxResult.Yes == result)
                        {
                            MyForm.FolderBrowserDialog FBD = new MyForm.FolderBrowserDialog();
                            if (FBD.ShowDialog() == MyForm.DialogResult.OK)
                            {
                                var strF = numberInTextBox.Text.Replace('/', '-')
                                        .Replace('\\', '-')
                                        .Replace(':', '-')
                                        .Replace('*', '-')
                                        .Replace('?', '-')
                                        .Replace('\"', '-')
                                        .Replace('<', '-')
                                        .Replace('>', '-')
                                        .Replace('|', '-');
                                Directory.CreateDirectory(FBD.SelectedPath + $"\\{strF}");
                                int index = 1;
                                foreach (var item in Reest.sNazyv)
                                {
                                    Directory.CreateDirectory(FBD.SelectedPath + $"\\{strF}\\{index++}. {item}");
                                }
                                Directory.CreateDirectory(FBD.SelectedPath + $"\\{strF}\\{index}. Схеми");

                                prevMc.Folder = FBD.SelectedPath + "\\" + strF;
                                modelContext.SaveChanges();
                                contShLabel.Content = $"Контроль/Схема  Розташування папки: " + FBD.SelectedPath + "\\" + strF;
                                System.Windows.MessageBox.Show("Папку запиту створено за посиланням " + FBD.SelectedPath + "\\" + strF);
                                Button_ClickUpdate(new object(), new RoutedEventArgs());
                            }
                        }
                    }
                }
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
            }
        }
        
        private void ClickOnCheckBox(object sender, RoutedEventArgs e)
        {
            try 
            { 
            var prevMc = modelContext.MainConfigs.Find(numberInTextBox.Text);
            if(prevMc != null)
            {
                var ch = sender as System.Windows.Controls.CheckBox;

                if (ch != null)
                {
                    var chIn = ch.Content as System.Windows.Controls.CheckBox;
                    if (chIn != null)
                    {
                        var item = chIn.Content as System.Windows.Controls.TreeViewItem;
                        if (item != null)
                        {
                            
                                if (!Directory.Exists(prevMc.Folder + "\\" + item.Header))
                                {
                                    ch.IsChecked = false;
                                }
                        }
                    }
                    else
                    {
                        var itemTreeViewItem = ch.Content as System.Windows.Controls.TreeViewItem;
                        if (itemTreeViewItem != null)
                        {
                            if (!Directory.Exists(prevMc.Folder + "\\" + itemTreeViewItem.Header))
                            {
                                ch.IsChecked = false;
                            }
                            else
                            {
                                var chLast = treeView1.Items[treeView1.Items.Count - 1] as System.Windows.Controls.CheckBox;
                                if (chLast != null)
                                {
                                    chLast.IsChecked = true;
                                }
                            }

                        }
                    }
                }
            }

            }
            catch (Exception exp1)
            {
                System.Windows.MessageBox.Show("" + exp1.Message);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
