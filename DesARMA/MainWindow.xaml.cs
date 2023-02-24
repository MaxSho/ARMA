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
using System.Printing;
using NPOI.XSSF.Streaming.Values;
using DesARMA.Registers.EDR;
using DesARMA.Entities;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Media.Animation;

namespace DesARMA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public delegate void UpdateDel(object sender, RoutedEventArgs e);
    public delegate void LoadDel();
    public partial class MainWindow : System.Windows.Window
    {
        public bool isOpenWindFig = false;
        System.Windows.Controls.Button currentButton = new System.Windows.Controls.Button();
        private Timer inactivityTimer = new Timer();
        Main CurrentMainDB = null!;
        ModelContext modelContext = new ModelContext();
        public User CurrentUser { get; set; } = null!;
        public MainWindow()
        {
            try
            {
                InitializeComponent();

                CreateTimer();
                Auth();
                currentButton = AddButton;
                currentButton = OpenButton;
                DownloadReest();
                LoadDb();

                //CreateButtonsGetData();

            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
                Environment.Exit(0);
            }
        }
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash1
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes); // .NET 5 +
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    this.Hide();
            //    CreateTimer();
            //    Auth();
            //    currentButton = AddButton;
            //    DownloadReest();
            //    LoadDb();
            //    //CreateButtonsGetData();
            //    this.Show();
            //}
            //catch (Exception e2)
            //{
            //    System.Windows.MessageBox.Show(e2.Message);
            //    Environment.Exit(0);
            //}
        }
        private StackPanel CreateContShLabel(string path)
        {
            TextBlock textb = new TextBlock();
            textb.Text = "Контроль / Схема  Розташування папки: ";
            textb.FontSize = 14;

            TextBlock textb2 = new TextBlock();
            textb2.Text = path;
            textb2.MouseEnter += (w, r)=>{ textb2.Opacity = 0.5; };
            textb2.MouseLeave += (w, r) => { textb2.Opacity = 1; };
            textb2.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textb2.Text); } catch (Exception ex) { } };
            textb2.FontSize = 14;

            StackPanel stack = new StackPanel();
            stack.Orientation = System.Windows.Controls.Orientation.Horizontal;
            stack.Children.Add(textb);
            stack.Children.Add(textb2);

            return stack;
        }
        public void CreateTimer()
        {
            inactivityTimer = new Timer();
            string shif = ConfigurationManager.AppSettings["hv"].ToString();
            //string shif = "60";
            inactivityTimer.Interval = 60_000 * Convert.ToInt32(shif);
            //inactivityTimer.Interval = 5_000;

            inactivityTimer.Tick += (sender, args) =>
            {
                Environment.Exit(0);
                //System.Windows.MessageBox.Show("Time over");
            };
            inactivityTimer.Start();
        }
        public string GetStrFromByte(byte b)
        {
            string ret = ""+b;
            if (b < 10)
            {
                ret = "00" + b;
            }
            else if(b < 100)
            {
                ret = "0" + b;
            }
            return ret;
        }
        private void CreateButtonsGetData()
        {
            for (int i = 1; i <= Reest.sNazyv.Count; i++)
            {
                System.Windows.Controls.Button curB = new System.Windows.Controls.Button();
                curB.Background = this.Resources[$"3ColorStyle"] as SolidColorBrush;
                curB.Foreground = this.Resources[$"1ColorStyle"] as SolidColorBrush;
                curB.Tag = i;
                curB.Content = "" + i;
                curB.Click += Button_Click_GetData;
                
                stackPanelButtonsGetData.Children.Add(curB);
            }
        }
        private void Button_Click_GetData(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                System.Windows.MessageBox.Show("in");
                RegisterEDR registerEDR = new RegisterEDR();
                if (CurrentMainDB != null)
                {
                    registerEDR.requestProgram = new RequestProgram(CurrentMainDB, modelContext);
                    registerEDR.GetData();
                }
                if(CurrentMainDB != null)
                    System.Windows.MessageBox.Show($"{CurrentMainDB.NumbInput}");
            }
            catch (Exception e2)
            {
                System.Windows.MessageBox.Show(e2.Message);
            }
            inactivityTimer.Start();
        }
        private void DownloadReest()
        {
            try 
            { 
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


                var Reestr = from b in modelContext.DictCommons
                             where b.Domain == "REGISTER"
                             select b;

                List<string> sNazyv = new List<string>();
                List<string> sRodov = new List<string>();
                List<string> sDav = new List<string>();
                List<int> sGrupa = new List<int>();
                List<string> aString = new List<string>();

                var l = Reestr.ToList();
                for (int i = 1; i <= l.Count; i++)
                {
                    var item = (from b in modelContext.DictCommons
                                where b.Domain == "REGISTER" && b.Code == i + ""
                                select b).ToList().First();
                    if (item.Name != null)
                    {
                        var arr = item.Name.Split('^');

                        if (arr.Length >= 5)
                        {
                            sNazyv.Add(arr[0]);
                            sRodov.Add(arr[1]);
                            sDav.Add(arr[2]);

                            int variable = -1;
                            int.TryParse(arr[3], out variable);
                            sGrupa.Add(variable);

                            aString.Add(arr[4]);
                        }
                        else if (arr.Length == 4)
                        {
                            sNazyv.Add(arr[0]);
                            sRodov.Add(arr[1]);
                            sDav.Add(arr[2]);

                            int variable = -1;
                            int.TryParse(arr[3], out variable);
                            sGrupa.Add(variable);

                            aString.Add("");
                        }
                        else if (arr.Length == 3)
                        {
                            sNazyv.Add(arr[0]);
                            sRodov.Add(arr[1]);
                            sDav.Add(arr[2]);
                            sGrupa.Add(-1);
                            aString.Add("");
                        }
                        else if (arr.Length == 2)
                        {
                            sNazyv.Add(arr[0]);
                            sRodov.Add(arr[1]);
                            sDav.Add("");
                            sGrupa.Add(-1);
                            aString.Add("");
                        }
                        else if (arr.Length == 1)
                        {
                            sNazyv.Add(arr[0]);
                            sRodov.Add("");
                            sDav.Add("");
                            sGrupa.Add(-1);
                            aString.Add("");
                        }
                        else
                        {
                            sNazyv.Add("");
                            sRodov.Add("");
                            sDav.Add("");
                            sGrupa.Add(-1);
                            aString.Add("");
                        }
                    }
                    else
                    {
                        sNazyv.Add("");
                        sRodov.Add("");
                        sDav.Add("");
                        sGrupa.Add(-1);
                        aString.Add("");
                    }

                }
                Reest.sNazyv = sNazyv;
                Reest.sRodov = sRodov;
                Reest.sDav = sDav;
                Reest.sGrupa = sGrupa;
                Reest.abbreviatedName = aString;
            }
            catch (Exception e)
            {
                 System.Windows.MessageBox.Show(e.Message);
                 Environment.Exit(0);
            }
}
        private void Auth()
        {
            inactivityTimer.Stop();
            List<User> users = new List<User>();

            foreach (var item in modelContext.Users)
            {
                users.Add(item);
            }
            while (true)
            {
                AuthWindow authWindow = new AuthWindow(inactivityTimer);
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
            inactivityTimer.Start();
        }
        private void CreateLeftPanelButtonsRequestes(List<Main> m)
        {
            stackPanel1.Children.Clear();
            //var AddButton = new System.Windows.Controls.Button();
            //AddButton.Tag = "0";
            //AddButton.Content = "Відкрити запит";
            //AddButton.Click += Button_Click_1;
            //stackPanel1.Children.Add(AddButton);

            for (int i = 0; i < m.Count; i++)
            {
                var itemButton = new System.Windows.Controls.Button();
                itemButton.Click += Button_Any_Click_Req;

                var NumForeground = 4;
                var NumBackground = 2;

                itemButton.Foreground = this.Resources[$"{NumForeground}ColorStyle"] as SolidColorBrush;
                itemButton.Background = this.Resources[$"{NumBackground}ColorStyle"] as SolidColorBrush;

                itemButton.Tag = m[i].Id;
                itemButton.Content = $"Запит: {m[i].NumbInput}";
                stackPanel1.Children.Insert(0, itemButton);
                // stackPanel1.Children.Add(itemButton);
                if (i == m.Count - 1)
                {
                    currentButton = itemButton;
                }
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
        }
        private void LoadDb()
        {
            try
            {
                var mains = (from b in modelContext.Mains
                             where b.Executor == CurrentUser.IdUser
                    &&
                        (from o in modelContext.MainConfigs
                         where o.NumbInput == b.NumbInput
                         select o).Count() == 1
                    //orderby /*b.NumbInput.Substring(8, 2),*/
                    //        b.NumbInput.Split(new char[] { '/' }, 1)[0]//CreateCombinedResponseWindow.GetStringWithZero(b.NumbInput)
                             select b
                    )
                    .AsEnumerable()
                    .OrderBy(b => {
                        int result;
                        if (int.TryParse(b.NumbInput.Split(new char[] { '/' }, 2)[0], out result))
                        {
                            return result;
                        }
                        else
                        {
                            return 0;
                        }
                    })
                    .OrderBy(b => {
                        int result;
                        if (int.TryParse(b.NumbInput.Split(new char[] { '-' }, 2)[1], out result))
                        {
                            return result;
                        }
                        else
                        {
                            return 0;
                        }
                    })
                    
                    .ToList();

                //stackPanel1.Children.Clear();
                //var AddButton = new System.Windows.Controls.Button();
                //AddButton.Tag = "0";
                //AddButton.Content = "Відкрити запит";
                //AddButton.Click += Button_Click_1;
                //stackPanel1.Children.Add(AddButton);

                CreateLeftPanelButtonsRequestes(mains);

                var mcIs = modelContext.MainConfigs.Find(mains.Last().NumbInput);
                //if (mcIs == null) continue;

                contShLabel.Content = /*$"Контроль/Схема  Розташування папки: {mcIs.Folder}";*/ CreateContShLabel(mcIs!.Folder);

                numberKPTextBox.Text = mains.Last().CpNumber;
                numberInTextBox.Text = mains.Last().NumbInput;
                dateInTextBox.Text = InStrDate(mains.Last().DtInput);
                dateControlTextBox.Text = InStrDate(mains.Last().DtCheck);

                ReadFromMainDBToCenter(mains.Last());

                CurrentMainDB = mains.Last();

                treeView1.Items.Clear();


                AllDirectories allDirectories = new AllDirectories(mains.Last(), mcIs, ClickOnCheckBox, this.Resources["RedEmpty"] as SolidColorBrush, this.Resources[$"4ColorStyle"] as SolidColorBrush,
                                        this.Resources["GreenEmpty"] as SolidColorBrush
                                        , treeView1, modelContext
                                       );
                allDirectories.CreateNewTree();

                //for (int i = 0; i < mains.Count; i++)
                //{ 
                //    var mcIs = modelContext.MainConfigs.Find(mains[i].NumbInput);
                //    if (mcIs == null) continue;

                //    contShLabel.Content = /*$"Контроль/Схема  Розташування папки: {mcIs.Folder}";*/ CreateContShLabel(mcIs.Folder);

                //    //Кнопка запиту зліва
                //    //var itemButton = new System.Windows.Controls.Button();
                //    //itemButton.Click += Button_Any_Click_Req;

                //    //var NumForeground = 4;
                //    //var NumBackground = 2;

                //    //itemButton.Foreground = this.Resources[$"{NumForeground}ColorStyle"] as SolidColorBrush;
                //    //itemButton.Background = this.Resources[$"{NumBackground}ColorStyle"] as SolidColorBrush;

                //    numberKPTextBox.Text = mains[i].CpNumber;
                //    numberInTextBox.Text = mains[i].NumbInput;
                //    dateInTextBox.Text = InStrDate(mains[i].DtInput);
                //    dateControlTextBox.Text = InStrDate(mains[i].DtCheck);

                //    ReadFromMainDBToCenter(mains[i]);

                //    //itemButton.Tag = mains[i].Id;
                //    //itemButton.Content = $"Запит: {mains[i].NumbInput}";
                //    //stackPanel1.Children.Insert(0, itemButton);

                //    //currentButton = itemButton;
                //    CurrentMainDB = mains[i];

                //    treeView1.Items.Clear();
                   

                //    AllDirectories allDirectories = new AllDirectories(mains[i], mcIs, ClickOnCheckBox, this.Resources["RedEmpty"] as SolidColorBrush, this.Resources[$"4ColorStyle"] as SolidColorBrush,
                //                            this.Resources["GreenEmpty"] as SolidColorBrush
                //                            , treeView1, modelContext
                //                           );
                //    allDirectories.CreateNewTree();
                //}

                //for (int j = 1; j < stackPanel1.Children.Count; j++)
                //{
                //    var b = stackPanel1.Children[j] as System.Windows.Controls.Button;
                //    if (b != null)
                //    {
                //        b.Background = this.Resources[$"3ColorStyle"] as SolidColorBrush;
                //        b.Foreground = this.Resources[$"1ColorStyle"] as SolidColorBrush;
                //    }
                //}

                Button_ClickUpdate(new object(), new RoutedEventArgs());
                // System.Windows.MessageBox.Show("" + treeView1.Items.Count);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }

        }
        private void LoadDbDate()

        {
            try
            {
                var mains = (from b in modelContext.Mains
                             where b.Executor == CurrentUser.IdUser
                    &&
                        (from o in modelContext.MainConfigs
                         where o.NumbInput == b.NumbInput
                         select o).Count() == 1
                    orderby b.DtInput
                             select b
                    )
                    .ToList();

               

                CreateLeftPanelButtonsRequestes(mains);

                var mcIs = modelContext.MainConfigs.Find(mains.Last().NumbInput);

                contShLabel.Content =  CreateContShLabel(mcIs!.Folder);

                numberKPTextBox.Text = mains.Last().CpNumber;
                numberInTextBox.Text = mains.Last().NumbInput;
                dateInTextBox.Text = InStrDate(mains.Last().DtInput);
                dateControlTextBox.Text = InStrDate(mains.Last().DtCheck);

                ReadFromMainDBToCenter(mains.Last());

                CurrentMainDB = mains.Last();

                treeView1.Items.Clear();


                AllDirectories allDirectories = new AllDirectories(mains.Last(), mcIs, ClickOnCheckBox, this.Resources["RedEmpty"] as SolidColorBrush, this.Resources[$"4ColorStyle"] as SolidColorBrush,
                                        this.Resources["GreenEmpty"] as SolidColorBrush
                                        , treeView1, modelContext
                                       );
                allDirectories.CreateNewTree();

                Button_ClickUpdate(new object(), new RoutedEventArgs());
                // System.Windows.MessageBox.Show("" + treeView1.Items.Count);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }

        }
        private void ClearMainCenter()
        {
            typeorgansList.SelectedIndex = -1;
            numberRequestTextBox.Text = "";
            dateRequestDatePicker.SelectedDate = new DateTime();
            vidOrgTextBox.Text = "";
            addressOrgTextBox.Text = "";
            positionSubTextBox.Text = "";
            nameSubTextBox.Text = "";
            numberOutTextBox.Text = "";
            dateOutDatePicker.SelectedDate = new DateTime();
            co_executorTextBox.Text = "";
            TEKATextBox.Text = "";
            article_CCUTextBox.Text = "";
            noteTextBox.Text = "";
            typeAppealList.SelectedIndex = -1;
            numberKPTextBox.Text = "";
            dateInTextBox.Text = null;
            dateControlTextBox.Text = null;
            numberInTextBox.Text = "";
        }
        private void Button_Any_Click_Req(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
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
                currentButton = itemBut;
                
                //Button_ClickUpdate(null, null);
                var m = (from b in modelContext.Mains
                            where b.Id.Equals((decimal)currentButton.Tag)
                            select b).First();


                if (m != null)
                {
                    CurrentMainDB = m;
                    ClearMainCenter();

                    numberKPTextBox.Text = m.CpNumber;
                    dateInTextBox.Text = InStrDate(m.DtInput);
                    dateControlTextBox.Text = InStrDate(m.DtCheck);
                    numberInTextBox.Text = m.NumbInput;
                    ReadFromMainDBToCenter(m);

                    
                    for (int i = 0; i < stackPanel1.Children.Count ; i++)
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

                    var mc = modelContext.MainConfigs.Find(m.NumbInput);
                    if(mc!=null)
                        contShLabel.Content =/* $"Контроль/Схема  Розташування папки: {mc.Folder}"; */CreateContShLabel(mc.Folder);

                   
                    ReadFromStringDBToCheckBoxes(mc);

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
            inactivityTimer.Start();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
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
                    if (currentButton != OpenButton)
                    {

                        SaveAllDB();

                    }

                    main.Executor = CurrentUser.IdUser;
                    main.LoginName = CurrentUser.LoginName; // new

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
                    itemButton.Content = $"Запит: {createRequestWindow.CodeRequest}";


                    for (int i = 0; i < stackPanel1.Children.Count; i++)
                    {
                        var but = stackPanel1.Children[i] as System.Windows.Controls.Button;
                        if (but != null)
                        {
                            but.Background = this.Resources[$"3ColorStyle"] as SolidColorBrush;
                            but.Foreground = this.Resources[$"1ColorStyle"] as SolidColorBrush;
                        }
                        else
                        {
                            System.Windows.MessageBox.Show($"Не вдалося встановити фон і колір тексту {i + 1}-й кнопки запиту. Кнопка не знайдена null");
                        }
                    }

                    stackPanel1.Children.Insert(0, itemButton);


                    Directory.CreateDirectory(FBD.SelectedPath + $"\\{codeRequest}");
                    int index = 1;
                    treeView1.Items.Clear();
                    foreach (var item in Reest.abbreviatedName)
                    {
                        Directory.CreateDirectory(FBD.SelectedPath + $"\\{codeRequest}\\{index}. {item}");
                        //var checkBox = new System.Windows.Controls.CheckBox();
                        //var checkBox2 = new System.Windows.Controls.CheckBox();

                        //var parentItem = new TreeViewItem();
                        //parentItem.Header = $"{index++}. " + item;
                        //parentItem.Foreground = this.Resources[$"4ColorStyle"] as SolidColorBrush;
                        //checkBox.Content = parentItem;
                        //checkBox.Margin = new Thickness(0, 1, 0, 0);
                        //checkBox2.Content = checkBox;
                        //treeView1.Items.Add(checkBox2);
                    }
                    //var checkBoxS = new System.Windows.Controls.CheckBox();
                    //var checkBoxS2 = new System.Windows.Controls.CheckBox();

                    //var treeS = new TreeViewItem();
                    //treeS.Foreground = this.Resources[$"4ColorStyle"] as SolidColorBrush;
                    //treeS.Header = $"{index}. Схеми";

                    //checkBoxS.Content = treeS;
                    //checkBoxS.Margin = new Thickness(0, 1, 0, 0);
                    //checkBoxS2.Content = checkBoxS;
                    //treeView1.Items.Add(checkBoxS2);

                    Directory.CreateDirectory(FBD.SelectedPath + $"\\{codeRequest}\\{index}. Схеми");

                    
                    currentButton = itemButton;
                    CurrentMainDB = main;
                    //treeView1.Items.Count
                    modelContext.MainConfigs.Add(new MainConfig() { Control = new string('0', treeView1.Items.Count), Folder = FBD.SelectedPath+ $"\\{codeRequest}" , NumbInput = main.NumbInput, Shema = new string('0', treeView1.Items.Count) });
                    contShLabel.Content = /*$"Контроль/Схема  Розташування папки: {FBD.SelectedPath + $"\\{codeRequest}"}";*/ CreateContShLabel(FBD.SelectedPath + $"\\{codeRequest}");
                    
                    modelContext.SaveChanges();

                    AllDirectories allDirectories = new AllDirectories(main, (from b in modelContext.MainConfigs
                                                                             where b.NumbInput.Equals(createRequestWindow.CodeRequest)
                                                                             select b).First(),
                        ClickOnCheckBox, this.Resources["RedEmpty"] as SolidColorBrush, this.Resources[$"4ColorStyle"] as SolidColorBrush,
                        this.Resources["GreenEmpty"] as SolidColorBrush
                        , treeView1, modelContext
                       );
                    allDirectories.CreateNewTree();

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
            inactivityTimer.Start();
        }
        private void Button_ClickUpdate(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                //if (currentButton == AddButton) return;
                if (currentButton == OpenButton) return;

                var prevMc = modelContext.MainConfigs.Find(numberInTextBox.Text);
                
                var main = modelContext.Mains.Find(numberInTextBox.Text);
                ReadFromCheckBoxesToStringDB(prevMc);

                if(prevMc != null && main != null)
                {
                    AllDirectories allDirectories = new AllDirectories(main, prevMc, ClickOnCheckBox, this.Resources["RedEmpty"] as SolidColorBrush, this.Resources[$"4ColorStyle"] as SolidColorBrush,
                        this.Resources["GreenEmpty"] as SolidColorBrush
                        , treeView1, modelContext
                       );
                    allDirectories.CreateNewTree();
                }


               
            }
            catch (Exception e2)
            {
                System.Windows.MessageBox.Show(e2.Message);
                System.Windows.MessageBox.Show($"StackTrace: {e2.StackTrace}\n\n");

                var trace = new StackTrace(e2, true);

                foreach (var frame in trace.GetFrames())
                {
                    var sb = new StringBuilder();

                    sb.AppendLine($"Файл: {frame.GetFileName()}");
                    sb.AppendLine($"Строка: {frame.GetFileLineNumber()}");
                    sb.AppendLine($"Столбец: {frame.GetFileColumnNumber()}");
                    sb.AppendLine($"Метод: {frame.GetMethod()}");

                    System.Windows.MessageBox.Show(sb.ToString());
                }
            }
            inactivityTimer.Start();
        }
        private bool isHaveBefore(int indexRee, List<int> listGr)
        {
            for (int i = 0; i < indexRee; i++)
            {
                if (listGr[i] == listGr[indexRee])
                {
                    return true;
                }
            }
            return false;
        }
        private List<int> Orders(List<string> listSRod)
        {
            int index = 1;

            List<int> listGr = new List<int>();
            List<int> listReturnNumb = new List<int>();

            foreach (var item in listSRod)
            {
                // індекс назви реєстра в робовому 
                var indexReest = Reest.sRodov.IndexOf(item);

                // listGr додається групу цього реєстру
                listGr.Add(Reest.sGrupa[indexReest]);
            }

            foreach (var item in listSRod)
            {
                if (isHaveBefore(listSRod.IndexOf(item), listGr))
                {
                    listReturnNumb.Add(listGr[listSRod.IndexOf(item)]);
                }
                else
                {
                    listReturnNumb.Add(index++);
                }
            }

            return listReturnNumb;


            //List<int> listUnikalGr = new List<int>();

            //foreach (var item in listGr)
            //{

            //    if (Reest.sGrupa.IndexOf(item) == Reest.sGrupa.LastIndexOf(item))
            //    {
            //        listUnikalGr.Add(item);
            //    }
            //    else{
                    
            //    }
            //}


            //List<int> listIndexes = new List<int>();

            
            //foreach (var item in listGr)
            //{

            //    if (Reest.sGrupa.IndexOf(item) == Reest.sGrupa.LastIndexOf(item))
            //    {
            //        listUnikalGr.Add(index);
            //    }
            //    else
            //    {

            //    }
            //    index++;
            //}

        }
        private bool isCanReturnRespons(MainConfig? prevM)
        {
            //if (currentButton == AddButton) return false;
            if (currentButton == OpenButton) return false;
            int iPrap = 1;
            if (prevM == null) return false;

            var listNotCheckControl = new List<string>();
            foreach (var item in treeView1.Items)
            {
                var sp = item as System.Windows.Controls.StackPanel;
                if (sp != null)
                {
                    var chB = sp.Children[0] as System.Windows.Controls.CheckBox;
                    if (chB != null)
                    {
                        var b = chB.IsChecked;
                        if (b != null)
                        {
                            if (!(bool)b)
                            {
                                var nameReest = sp.Children[2] as System.Windows.Controls.TreeViewItem;

                                if (nameReest != null)
                                {
                                    if (Directory.Exists(prevM.Folder + "\\" + nameReest.Header))
                                    {

                                        listNotCheckControl.Add(nameReest.Header.ToString());
                                        //return;
                                    }
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show($"Не знайдено назву реэстру");
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show($"Не відмічено контроль(null)");
                            return false;
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show($"Прапорець номер {iPrap} контролю не визначений");
                        return false;
                    }
                }

                iPrap++;
            }

            if (listNotCheckControl.Count > 0)
            {

                string str = $"Не відмічено контроль:";

                foreach (var item in listNotCheckControl)
                {
                    str += "\n" + item;
                }
                System.Windows.MessageBox.Show(str);
                return false;
            }
            return true;
        }
        private void Button_ClickRespon(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                var prevM = modelContext.MainConfigs.Find(numberInTextBox.Text);
                

                if (!isCanReturnRespons(prevM))
                {
                    return;
                }

                var isCountFig = (from b in modelContext.Figurants
                                  where b.NumbInput == numberInTextBox.Text &&
                                                b.Status == 1
                                  select b).ToList().Count;

                if (isCountFig == 0)
                {
                    System.Windows.MessageBox.Show($"Не вибрано жодного фігуранта");
                    return;
                }

                //Створення зміних, що вставляються в звіт   
                var indexSub = isCountFig > 1 ? 0 : 1;
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


                DocResponse docResponse = new DocResponse(new List<MainConfig>() { prevM }, 
                    new List<int>() { indexSub, whatIndex, count_Shemat }, //TODO визначити кількість схематичного відображення
                    new List<string>() {
                    name,
                    address1,
                    date1,
                    date2,
                    number1,
                    number2,
                    vidOrgan,
                    positionSub
                }, modelContext);


                if (whatIndex == 2)
                {
                    docResponse.CreateResponseMINIuST();
                    
                }
                else
                {
                    docResponse.CreateResponseOther();
                    //System.Windows.MessageBox.Show($"Відповідь збережено в папку:\n{path3}");
                }   

               
            }
            catch (Exception e2)
            {
                System.Windows.MessageBox.Show(e2.Message);
               // doc1.Close();
            }

            inactivityTimer.Start();
        }
        private void ButtonSearchClick(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                var mcCur = modelContext.MainConfigs.Find(textBoxSearch.Text);

                if(mcCur == null)
                {
                    System.Windows.MessageBox.Show("Запит " + textBoxSearch.Text + " не знайдено серед витягнутих");
                    return;
                }

                    CreateButton(mcCur.NumbInput);
                 //System.Windows.MessageBox.Show(s);
            }
            catch (Exception e2)
            {
                System.Windows.MessageBox.Show(e2.Message);
            }
            inactivityTimer.Start();
        }
        private void ClickDefendantsButton(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                
                var m = (from b in modelContext.Mains
                            where b.Id.Equals((decimal)currentButton.Tag)
                            select b).ToList().First();


                if (m != null)
                {
                    
                        ListDefendantsWindow listDefendantsWindow = new ListDefendantsWindow(modelContext,
                        numberInTextBox.Text, "Перелік фігурантів", false, inactivityTimer);
                        listDefendantsWindow.Owner = this;
                        listDefendantsWindow.Show();

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
            inactivityTimer.Start();
        }
        private List<decimal> GetIdButon()
        {
            var list = new List<decimal>();

            foreach (var item in stackPanel1.Children)
            {
                var item2 = item as System.Windows.Controls.Button;
                if(item2 != null)
                {
                    var item3 = Convert.ToDecimal(item2.Tag);
                    if(item3 != 0)
                        list.Add(item3);
                }
            }
            return list;
        }
        private void CreateButton(string numbInput)
        {
            try
            {
                var reqItAdd = modelContext.Mains.Find(numbInput);

                if (reqItAdd != null)
                {
                    
                    var list = GetIdButon();
                    stackPanel1.Children.Clear();
                    int i = 1;

                    foreach (var itemId in list)
                    {
                        var mC = (from b in modelContext.Mains where b.Id == itemId select b).ToList();
                        if(mC.Count > 0 && reqItAdd.Id != itemId)
                        {
                            var m = mC.Last();

                            var itemButton = new System.Windows.Controls.Button();
                            itemButton.Click += Button_Any_Click_Req;


                            var NumForeground2 = 4;  // stackPanel1.Children.Count % 2 == 1 ? 4 : 1;
                            var NumBackground2 = 2;  // stackPanel1.Children.Count % 2 == 1 ? 2 : 3;

                            itemButton.Foreground = this.Resources[$"{NumForeground2}ColorStyle"] as SolidColorBrush;
                            itemButton.Background = this.Resources[$"{NumBackground2}ColorStyle"] as SolidColorBrush;


                            itemButton.Tag = m.Id;

                            itemButton.Content = $"Запит: {m.NumbInput}";
                            //stackPanel1.Children.Insert(0, itemButton);
                            stackPanel1.Children.Add(itemButton);
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


                    //var itemButtonAdd = new System.Windows.Controls.Button();
                    //itemButtonAdd.Click += Button_Click_1;
                    //itemButtonAdd.Name = "AddButton";
                    ////itemButtonAdd.Name = "OpenButton";
                    //itemButtonAdd.Tag = "0";
                    //itemButtonAdd.Content = "Відкрити запит";
                    //itemButtonAdd.Foreground = this.Resources[$"{1}ColorStyle"] as SolidColorBrush;
                    //itemButtonAdd.Background = this.Resources[$"{4}ColorStyle"] as SolidColorBrush;
                    //stackPanel1.Children.Add(itemButtonAdd);


                    // Button_Any_Click_Req(currentButton, new RoutedEventArgs());
                    numberKPTextBox.Text = reqItAdd.CpNumber;
                    dateInTextBox.Text = InStrDate(reqItAdd.DtInput);
                    dateControlTextBox.Text = InStrDate(reqItAdd.DtCheck);
                    numberInTextBox.Text = reqItAdd.NumbInput;
                    ReadFromMainDBToCenter(reqItAdd);

                    

                    //TODO Save chackB


                    Button_ClickUpdate(new object(), new RoutedEventArgs());
                }
            }
            catch (Exception e2)
            {
                System.Windows.MessageBox.Show(e2.Message);
            }
        }
        private void ClickСonnectedPeopleButton(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                var m = (from b in modelContext.Mains
                            where b.Id.Equals((decimal)currentButton.Tag)
                            select b).ToList().First();

                if (m != null)
                {
                    
                    ListDefendantsWindow listDefendantsWindow = new ListDefendantsWindow(modelContext, numberInTextBox.Text, "Перелік пов'язаних осіб", true, inactivityTimer);
                    listDefendantsWindow.Owner = this;
                    listDefendantsWindow.Show();

                    //listDefendantsWindow.Close();
                }
            }
            catch (Exception e2)
            {
                System.Windows.MessageBox.Show(e2.Message);
            }
            inactivityTimer.Start();
        }
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            inactivityTimer.Stop();
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
            inactivityTimer.Start();
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
            inactivityTimer.Stop();
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
            inactivityTimer.Start();
        }
        private void ClickReqButton(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            if (!(numberInTextBox.Text == null || numberInTextBox.Text == ""))
            {
                var items = from f in modelContext.Figurants
                            where f.NumbInput == numberInTextBox.Text && f.Status == 1
                            select f;
                if (items.ToList().Count == 0)
                {
                    System.Windows.MessageBox.Show("Не додано фігурантів");
                }
                else
                {
                    RequestsWindow requestsWindow = new RequestsWindow(new List<string>() { numberInTextBox.Text.ToString() }, modelContext, TypeOfAppeal.NotСombined,
                        inactivityTimer);
                    if (requestsWindow.ShowDialog() == true)
                    {

                    }
                    else
                    {

                    }
                }
            }
            inactivityTimer.Start();

        }
        private bool SaveAllDB()
        {
            bool ret = true;

            var m = (from b in modelContext.Mains
                        where b.NumbInput.Equals(numberInTextBox.Text)
                        select b).ToList().FirstOrDefault();

            if (m != null)
            {
                ret = ret && ReadFromCenterToMainDB(m);

                var mc = (from b in modelContext.MainConfigs
                          where b.NumbInput.Equals(numberInTextBox.Text)
                          select b).First();

                ret = ret && ReadFromCheckBoxesToStringDB(mc);
            }
            else
            {
                return false;
            }
            

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
                typeAppealList.SelectedIndex = typeA - 1;

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
                m.LoginName = CurrentUser.LoginName;
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
                m.Status = typeAppealList.SelectedIndex == -1 ? null : (typeAppealList.SelectedIndex + 1).ToString();//(typeAppealList.SelectedIndex + 1).ToString() /*== 1 ? "в роботі": "закрито"*/;
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
            //if (mc == null) return false;

            //var listCont = GetBoolsFromString(mc.Control);

            //int k = 0;
            //if(listCont != null)
            //foreach (var item in listCont)
            //{
            //    if (treeView1.Items.Count >= k)
            //    {
            //        var trV = treeView1.Items[k++] as System.Windows.Controls.CheckBox;
            //        if (trV != null)
            //        {
            //            trV.IsChecked = item as bool?;
            //        }
            //    }
            //}

            //var listShema = GetBoolsFromString(mc.Shema);

            //k = 0;
            //if(listShema != null)
            //foreach (var item in listShema)
            //{
            //    if (treeView1.Items.Count >= k)
            //    {
            //        var trV = treeView1.Items[k++] as System.Windows.Controls.CheckBox;
            //        if (trV != null)
            //        {
            //            var trVC = trV.Content as System.Windows.Controls.CheckBox;
            //            if (trVC != null)
            //            {
            //                trVC.IsChecked = item as bool?;
            //            }
            //        }
            //    }
            //}

            var main = (from m in modelContext.Mains where m.NumbInput == mc.NumbInput select m).First();
            AllDirectories allDirectories = new AllDirectories(main, mc, ClickOnCheckBox, this.Resources["RedEmpty"] as SolidColorBrush, this.Resources[$"4ColorStyle"] as SolidColorBrush,
                    this.Resources["GreenEmpty"] as SolidColorBrush, treeView1, modelContext
                     );

            allDirectories.CreateNewTree();
            return true;
        }
        private bool ReadFromCheckBoxesToStringDB(MainConfig? mc)
        {
            //bool ret = true;

            //if (mc == null) return false;
            //List<bool> listC = new List<bool>();
            //List<bool> listS = new List<bool>();
            //foreach (var item in treeView1.Items)
            //{
            //    if(item != null)
            //    {
            //        var cb = item as System.Windows.Controls.CheckBox;
            //        if (cb != null)
            //        {
            //            var isc = cb.IsChecked;
            //            if (isc != null)
            //            {
            //                listC.Add((bool)isc);
            //            }
            //            else
            //            {
            //                listS.Add(false);
            //            }

            //            var cbC = cb.Content as System.Windows.Controls.CheckBox;
            //            if (cbC!=null)
            //            {
            //                var iscbC = cbC.IsChecked;
            //                if(iscbC != null)
            //                {
            //                    listS.Add((bool)iscbC);
            //                }
            //                else
            //                {
            //                    listS.Add(false);
            //                }
                            
            //            }
            //        }
            //        else
            //        {
            //            ret = false;
            //        }


            //    }
            //    else
            //    {
            //        ret = false;
            //    }
                
                
            //}

            //var strC = GetStringFromBools(listC);
            //var strS = GetStringFromBools(listS);

            //mc.Control = strC;
            //mc.Shema = strS;

            var main = (from m in modelContext.Mains where m.NumbInput == mc.NumbInput select m).First();
            AllDirectories allDirectories = new AllDirectories(main, mc, ClickOnCheckBox, this.Resources["RedEmpty"] as SolidColorBrush, this.Resources[$"4ColorStyle"] as SolidColorBrush,
                    this.Resources["GreenEmpty"] as SolidColorBrush, treeView1, modelContext
                     );

            allDirectories.SaveToDB();

            modelContext.SaveChanges();

            return true;
        }
        private void Button_ClickSave(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
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
                System.Windows.MessageBox.Show("Виникла помилка збереження\n" + e2.Message);
            }
            inactivityTimer.Start();
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
            inactivityTimer.Stop();
            try
            {
                if (currentButton == AddButton) return;
                if (currentButton == OpenButton) return;

                var prevMc = modelContext.MainConfigs.Find(numberInTextBox.Text);

                if(prevMc != null)
                if (Directory.Exists(prevMc.Folder))
                {
                    //Отримання масиву вмісту папки запиту(реєстрів)
                    string[] dirs = Directory.GetDirectories(prevMc.Folder);
                    int i = 1;

                    var listR = new List<string>();
                    // Прохід по кожному реєстрі зі списку
                    foreach (string itemAbbreviatedName in Reest.abbreviatedName)
                    {
                        // пошук реєстру за порядеом в папці 
                        foreach (var itemDir in dirs)
                        {
                            // якщо знайдено папку з поточним реєстром
                            if ((new DirectoryInfo(itemDir)).Name == $"{i}. " + itemAbbreviatedName)
                            {
                               
                                if (!(Directory.GetDirectories(itemDir).Length == 0 &&
                                     Directory.GetFiles(itemDir).Length == 0)
                                )
                                {
                                    listR.Add(Reest.sRodov[Reest.abbreviatedName.IndexOf(itemAbbreviatedName)]);
                                    //Directory.CreateDirectory(prevMc.Folder + $"\\На диск\\Додаток {numDod}");
                                    //perebor_updates(prevMc.Folder + $"\\{i}. " + itemSNazyv, prevMc.Folder + $"\\На диск\\Додаток {numDod++}");

                                }
                            }

                        }
                        i++;
                    }

                    var listNumering = Orders(listR);
                    i = 1;
                    int ind = 0;

                    if (Directory.Exists(prevMc.Folder + $"\\На диск")) 
                    {
                        Directory.Delete(prevMc.Folder + $"\\На диск", true);
                    }
                    
                    
                    Directory.CreateDirectory(prevMc.Folder + $"\\На диск");

                    foreach (string itemAbbreviatedName in Reest.abbreviatedName)
                    {
                        // пошук реєстру за порядком в папці 
                        foreach (var itemDir in dirs)
                        {
                            // якщо знайдено папку з поточним реєстром
                            if ((new DirectoryInfo(itemDir)).Name == $"{i}. " + itemAbbreviatedName)
                            {

                                if (!(Directory.GetDirectories(itemDir).Length == 0 &&
                                     Directory.GetFiles(itemDir).Length == 0)
                                )
                                {
                                    Directory.CreateDirectory(prevMc.Folder + $"\\На диск\\Додаток {listNumering[ind]}");
                                    perebor_updates(prevMc.Folder + $"\\{i}. " + itemAbbreviatedName, prevMc.Folder + $"\\На диск\\Додаток {listNumering[ind]}");
                                    ind++;
                                }
                            }

                        }
                        i++;
                    }

                        //int ind = 0;
                        //foreach (var item in listNumering)
                        //{
                        //    Directory.CreateDirectory(prevMc.Folder + $"\\На диск\\Додаток {item}");
                        //    perebor_updates(prevMc.Folder + $"\\{i}. " + Reest.sNazyv[Reest.sRodov.IndexOf(listNumering)] itemSNazyv, prevMc.Folder + $"\\На диск\\Додаток {numDod++}");
                        //    ind++;
                        //}


                        
                    if (!(Directory.GetDirectories(prevMc.Folder + "\\" + $"{i}. Схеми").Length == 0 &&
                                     Directory.GetFiles(prevMc.Folder + "\\" + $"{i}. Схеми").Length == 0)){

                        Directory.CreateDirectory(prevMc.Folder + $"\\На диск\\Додаток {listNumering.Last() + 1}");
                        perebor_updates(prevMc.Folder + $"\\{i}. Схеми", prevMc.Folder + $"\\На диск\\Додаток {listNumering.Last() + 1}");

                    }

                    System.Windows.MessageBox.Show("Додатки збережено в папку: " + prevMc.Folder + "\\На диск");
                }
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message.ToString());
            }
            inactivityTimer.Start();
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
            inactivityTimer.Stop();
            try
            {
                if (currentButton == AddButton) return;
                if (currentButton == OpenButton) return;

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
                                contShLabel.Content = /*$"Контроль/Схема  Розташування папки: " + FBD.SelectedPath + "\\" + strF;*/ CreateContShLabel(FBD.SelectedPath + "\\" + strF);
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
                                foreach (var item in Reest.abbreviatedName)
                                {
                                    Directory.CreateDirectory(FBD.SelectedPath + $"\\{strF}\\{index++}. {item}");
                                }
                                Directory.CreateDirectory(FBD.SelectedPath + $"\\{strF}\\{index}. Схеми");

                                prevMc.Folder = FBD.SelectedPath + "\\" + strF;
                                modelContext.SaveChanges();
                                contShLabel.Content = $"Контроль/Схема  Розташування папки: " + FBD.SelectedPath + "\\" + strF; CreateContShLabel(FBD.SelectedPath + "\\" + strF);
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
            inactivityTimer.Start();
        }
        public void ClickOnCheckBox(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try 
            {
                
                var prevMc = modelContext.MainConfigs.Find(numberInTextBox.Text);
                if(prevMc != null)
                {
                    //натиснутий прапорець
                    var ch = sender as System.Windows.Controls.CheckBox;

                    System.Windows.MessageBox.Show(ch.Tag + "");
                    if (ch != null)
                    {
                        var chIn = ch.Content as System.Windows.Controls.CheckBox;
                        //чи має натиснутий прапорець в контенті прапорець
                        //якщо має то це прапорець контролю
                        if (chIn != null)
                        {
                            var item = chIn.Content as System.Windows.Controls.TreeViewItem;
                            if (item != null)
                            {
                                    if (!Directory.Exists(prevMc.Folder + "\\" + item.Header))
                                    {
                                        ch.IsChecked = false;
                                    }
                                    else
                                    {
                                        //var boolTag = ch.Tag;
                                        //if(ch.Tag != null)
                                        //if ((bool)ch.Tag)
                                        //{
                                        //        ch.IsChecked = !ch.IsChecked;
                                            
                                        //}
                                        //else
                                        //{
                                        //        ch.Tag = true;
                                        //        // The event was triggered by code
                                        //}
                                    }
                               
                                }
                        }
                        //якщо не має, то прапорець схем
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
            inactivityTimer.Start();
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void panelMouseWheel(object sender, MouseWheelEventArgs e)
        {
            System.Windows.MessageBox.Show("scroll");
        }
        private void Button_ClickCreateDelDir(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                var mc = modelContext.MainConfigs.Find(numberInTextBox.Text);
                var main = modelContext.Mains.Find(numberInTextBox.Text);
                if (mc != null && main != null)
                {
                    AllDirectories allDirectories = new AllDirectories(main, mc, ClickOnCheckBox,
                        this.Resources["RedEmpty"] as SolidColorBrush, this.Resources[$"4ColorStyle"] as SolidColorBrush,
                        this.Resources["GreenEmpty"] as SolidColorBrush
                        , treeView1, modelContext
                         );

                    //allDirectories.CreateNewTree(treeView1);
                    //System.Windows.MessageBox.Show("Finish");

                    CreateDelDirWindow c = new CreateDelDirWindow(allDirectories, Button_ClickUpdate, inactivityTimer);
                    c.Owner = this;
                    c.ShowDialog();
                }
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message.ToString());
            }
            inactivityTimer.Start();
        }
        private void ContentChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }
        private void ContentComboBoxChangedEventHandler(object sender, SelectionChangedEventArgs e)
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }
        private void Button_ClickInsertDataIntoMultipleRequest(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            //System.Windows.MessageBox.Show("В процесі розробки");
            InsertDataIntoMultipleRequestWindow insertDataIntoMultipleRequestWindow =
                    new InsertDataIntoMultipleRequestWindow(modelContext, CurrentUser, LoadDb, inactivityTimer);
            insertDataIntoMultipleRequestWindow.ShowDialog();
            inactivityTimer.Start();
        }
        private void Button_ClickCombinedRespon(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();

            var main = (from m in modelContext.Mains where m.NumbInput == numberInTextBox.Text select m).First();
            if(main != null)
            {
                if(main.CpNumber == null)
                {
                    System.Windows.MessageBox.Show("Не можливо створити об'єднаний запит з порожнім КП");
                }
                else
                {
                    CreateCombinedResponseWindow createCombinedResponseWindow = new CreateCombinedResponseWindow(modelContext, CurrentUser,
                    modelContext!.Mains!.Find(numberInTextBox.Text)!, inactivityTimer);

                    if (createCombinedResponseWindow.ShowDialog() == true)
                    {
                        //System.Windows.MessageBox.Show("true");
                        LoadDb();
                        createCombinedResponseWindow.MessegeAboutCreate();
                    }
                    else
                    {
                        //System.Windows.MessageBox.Show("false");
                    }
                }
            }
            inactivityTimer.Start();
        }
        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //System.Windows.MessageBox.Show("Hello");\
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }
        private void Window_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }
        private void ButtonSearchABCClick(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                LoadDb();
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            inactivityTimer.Start();
        }
        private void ButtonSearchDAteClick(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                LoadDbDate();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            inactivityTimer.Start();
        }
    }
}
