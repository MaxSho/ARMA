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
using DesARMA.Model3;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Asn1.Misc;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace DesARMA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        ApplicationContext db = new ApplicationContext();
        System.Windows.Controls.Button currentButton = new System.Windows.Controls.Button();
        bool isLoad = false;
        public List<User> users = new List<User>();
        ModelContext modelContext = new ModelContext();
        public User CurrentUser { get; set; } = null!;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                //db.Database.EnsureDeleted();
                
                db.Database.EnsureCreated();


                foreach (var item in db.Users)
                {
                    users.Add(item);
                }

                //auth
                while (true)
                {
                    AuthWindow authWindow = new AuthWindow();
                    if (authWindow.ShowDialog() == true)
                    {
                        bool sh = false;
                        foreach (var item in users)
                        {
                            if (item.Login == authWindow.Login)
                            {
                                if (item.Hash == authWindow.Password)
                                {
                                    CurrentUser = item;
                                    sh = true;
                                    break;

                                }
                            }
                        }
                        if (sh)
                            break;
                        else
                            MessageBox.Show($"Неправильний логін або пароль ");
                    }
                    else
                    {
                        //MessageBox.Show("Авторизація не пройдена");
                        Environment.Exit(0);
                    }
                }

                currentButton = AddButton;
                LoadDb();
                Button_ClickUpdate(new object(), new RoutedEventArgs());
                isLoad = true;

               

            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void LoadDb()
        {
            try 
            { 

            int i = 0;

            foreach (var r in db.Requests)
            {
                if (r.user == CurrentUser)
                {
                    var itemButton = new System.Windows.Controls.Button();
                    itemButton.Click += Button_Any_Click_Req;

                    var NumForeground = 4;  // stackPanel1.Children.Count % 2 == 1 ? 4 : 1;
                    var NumBackground = 2;  // stackPanel1.Children.Count % 2 == 1 ? 2 : 3;

                    itemButton.Foreground = this.Resources[$"{NumForeground}ColorStyle"] as SolidColorBrush;
                    itemButton.Background = this.Resources[$"{NumBackground}ColorStyle"] as SolidColorBrush;

                    numberKPTextBox.Text = r.numberKP;
                    numberInTextBox.Text = r.numberIn;
                    dateInTextBox.Text = r.dateIn;
                    dateControlTextBox.Text = r.dateControl;
                    typeorgansList.SelectedIndex = r.typeOrgan;
                    numberRequestTextBox.Text = r.numberRequest;

                    //    if(r.dateRequest!="")
                    //dateRequestDatePicker.Text = /*$"{r.dateRequest.Substring(0,2)}.{r.dateRequest.Substring(3, 2)}.{r.dateRequest.Substring(6,4)}"*/r.dateRequest;
                    dateRequestDatePicker.SelectedDate = r.dateRequest;

                    vidOrgTextBox.Text = r.vidOrg;
                    addressOrgTextBox.Text = r.addressOrg;
                    positionSubTextBox.Text = r.positionSub;
                    nameSubTextBox.Text = r.nameSub;
                    numberOutTextBox.Text = r.numberOut;

                    //    if(r.dateOut!=null)
                    //dateOutDatePicker.Text = /*$"{r.dateOut.Substring(0, 2)}.{r.dateOut.Substring(3, 2)}.{r.dateOut.Substring(6,4)}"*/ 
                    dateOutDatePicker.SelectedDate = r.dateOut;

                        co_executorTextBox.Text = r.co_executor;
                    TEKATextBox.Text = r.TEKA;
                    article_CCUTextBox.Text = r.article_CCU;
                    noteTextBox.Text = r.note;
                    typeAppealList.SelectedIndex = r.typeAppea;
                    itemButton.Tag = r.id;


                    itemButton.Content = $"Запит {i + 1}: {r.numberIn}";
                    stackPanel1.Children.Insert(0, itemButton);

                    currentButton = itemButton;

                    treeView1.Items.Clear();
                    int index = 1;

                    foreach (var item in Reest.sNazyv)
                    {
                            CheckBox checkBox = new CheckBox();
                            CheckBox checkBox2 = new CheckBox();

                            var parentItem = new TreeViewItem();
                            parentItem.Header = $"{index++}. " + item;
                            parentItem.Foreground = this.Resources[$"4ColorStyle"] as SolidColorBrush;
                            
                            checkBox.Content = parentItem;
                            checkBox.Margin = new Thickness(0, 1, 0, 0);
                            checkBox2.Content = checkBox;
                            treeView1.Items.Add(checkBox2);
                    }
                        CheckBox checkBoxS = new CheckBox();
                        CheckBox checkBoxS2 = new CheckBox();

                        var treeS = new TreeViewItem();
                        treeS.Foreground = new SolidColorBrush(Color.FromRgb(230, 78, 78));
                        treeS.Header = $"{index}. Схеми";

                        checkBoxS.Content = treeS;
                        checkBoxS.Margin = new Thickness(0, 1, 0, 0);
                        checkBoxS2.Content = checkBoxS;
                        treeView1.Items.Add(checkBoxS2);


                        EnumDBis enumDBis = new EnumDBis(r);
                        int k = 0;
                        foreach (var item in enumDBis)
                        {
                            if (treeView1.Items.Count >= k)
                            {
                                var trV = treeView1.Items[k++] as CheckBox;
                                if (trV != null)
                                {
                                    trV.IsChecked = item as bool?;
                                }
                            }
                        }

                        k = 0;
                        foreach (var item in enumDBis.GetScheme())
                        {
                            if (treeView1.Items.Count >= k)
                            {
                                var trV = treeView1.Items[k++] as CheckBox;
                                if (trV != null)
                                {
                                    var trVC = trV.Content as CheckBox;
                                    if (trVC != null)
                                    {
                                        trVC.IsChecked = item as bool?;
                                    }
                                }
                            }
                        }
                }

                    i++;  
                }

                
                for (int j = 1; j < i; j++)
                {
                    var b = stackPanel1.Children[j] as System.Windows.Controls.Button;
                    if(b != null)
                    {
                        b.Background = this.Resources[$"3ColorStyle"] as SolidColorBrush;
                        b.Foreground = this.Resources[$"1ColorStyle"] as SolidColorBrush;
                    }
                 }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
        private void Button_Any_Click_Req(object sender, RoutedEventArgs e)
        {
            try { 
                
                var itemBut = (System.Windows.Controls.Button)sender;

                if (itemBut == currentButton) return;

                var prevR = db.Requests.Find(Convert.ToInt32(currentButton.Tag));

                if(prevR != null)
                {
                    prevR.numberKP = numberKPTextBox.Text;
                    prevR.numberIn = numberInTextBox.Text;
                    prevR.dateIn = dateInTextBox.Text;
                    prevR.dateControl = dateControlTextBox.Text;
                    prevR.typeOrgan = typeorgansList.SelectedIndex;
                    prevR.numberRequest = numberRequestTextBox.Text;
                    prevR.dateRequest = dateRequestDatePicker.SelectedDate;
                    prevR.vidOrg = vidOrgTextBox.Text;
                    prevR.addressOrg = addressOrgTextBox.Text;
                    prevR.positionSub = positionSubTextBox.Text;
                    prevR.nameSub = nameSubTextBox.Text;
                    prevR.numberOut = numberOutTextBox.Text;
                    prevR.dateOut = dateOutDatePicker.SelectedDate;
                    prevR.co_executor = co_executorTextBox.Text;
                    prevR.TEKA = TEKATextBox.Text;
                    prevR.article_CCU = article_CCUTextBox.Text;
                    prevR.note = noteTextBox.Text;
                    prevR.typeAppea = typeAppealList.SelectedIndex;

                    for (int i = 0; i < treeView1.Items.Count; i++)
                    {
                        var chB = treeView1.Items[i] as CheckBox;
                        if (chB != null)
                        {
                            var isCh = chB.IsChecked;
                            if (isCh != null)
                            {
                                prevR.SetDB(i + 1, (bool)isCh);
                            }
                            else
                            {
                                MessageBox.Show($"Значення {i+1}-го прапорця контролю не визначено(null). Він не зберігся в лакальну базу");
                            }

                            var chBCon = chB.Content as CheckBox;

                            if(chBCon != null)
                            {
                                var isChCon = chBCon.IsChecked;
                                if(isChCon != null)
                                {
                                    prevR.SetSchemeDB(i + 1, (bool)isChCon);
                                }
                                else
                                {
                                    MessageBox.Show($"Значення {i + 1}-го прапорця схеми не визначено(null). Він не зберігся в лакальну базу");
                                }
                            }
                            else
                            {
                                MessageBox.Show($"{i + 1}-й прапорець схеми не знайдено(null). Він не зберігся в лакальну базу");
                            }


                        }
                        else
                        {
                            MessageBox.Show($"{i+1}-й прапорець контролю не знайдено(null). Він не зберігся в лакальну базу");
                        }
                    }  
                }
                else
                {
                    MessageBox.Show("В локальну базу дані не збережено");
                }

                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Виникли помилки запису в локальну базу даних. Можливо відбулось зміна полів або інше");
                }

                try
                {
                    LocalToGlob();
                }
                catch 
                {
                    MessageBox.Show("Виникли помилки запису в глобальну базу даних. Можливо відбулось зміна полів, або невірний формат або інше");
                }

                currentButton = itemBut;

                //Button_ClickUpdate(null, null);

                var r = db.Requests.Find(Convert.ToInt32(itemBut.Tag));

                if (r != null)
                {
                    numberKPTextBox.Text = r.numberKP;
                    numberInTextBox.Text = r.numberIn;
                    dateInTextBox.Text = r.dateIn;
                    dateControlTextBox.Text = r.dateControl;
                    typeorgansList.SelectedIndex = r.typeOrgan;
                    numberRequestTextBox.Text = r.numberRequest;
                    
                    dateRequestDatePicker.SelectedDate = r.dateRequest;


                    vidOrgTextBox.Text = r.vidOrg;
                    addressOrgTextBox.Text = r.addressOrg;
                    positionSubTextBox.Text = r.positionSub;
                    nameSubTextBox.Text = r.nameSub;
                    numberOutTextBox.Text = r.numberOut;


                    dateOutDatePicker.SelectedDate = r.dateOut;

                    co_executorTextBox.Text = r.co_executor;
                    TEKATextBox.Text = r.TEKA;
                    article_CCUTextBox.Text = r.article_CCU;
                    noteTextBox.Text = r.note;
                    typeAppealList.SelectedIndex = r.typeAppea;



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
                            MessageBox.Show("Не вдалося задати колір фону та тексту кнопкам запиту");
                        }
                    }

                    itemBut.Background = this.Resources[$"2ColorStyle"] as SolidColorBrush;
                    itemBut.Foreground = this.Resources[$"4ColorStyle"] as SolidColorBrush;


                    EnumDBis enumDBis = new EnumDBis(r);
                    int k = 0;
                    foreach (var item in enumDBis)
                    {
                        var chB = treeView1.Items[k++] as CheckBox;
                        if(chB!= null)
                        {
                            chB.IsChecked = item as bool?;
                        }
                        else
                        {
                            MessageBox.Show($"Не вдалося задати значення {k}-прапорцю контроля новому запиту. Прапорець null");
                        }
                    }

                    k = 0;
                    foreach (var item in enumDBis.GetScheme())
                    {
                        var chB = treeView1.Items[k++] as CheckBox;
                        if(chB!= null)
                        {
                            var chBCon = chB.Content as CheckBox;
                            if (chBCon != null)
                            {
                                chBCon.IsChecked = item as bool?;
                            }
                            else
                            {
                                MessageBox.Show($"Не вдалося задати значення {k}-прапорцю схеми новому запиту. Прапорець null");
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Не вдалося задати значення {k}-прапорцю схеми новому запиту. Прапорець контролю null");
                        }

                    }

                    Button_ClickUpdate(new object(), new RoutedEventArgs());
                }
                else
                {
                    MessageBox.Show("Не вдалося завантажити запит із контексту локальної БД");
                }
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.Message);
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            { 

                CreateRequestWindow createRequestWindow = new CreateRequestWindow();

                if (createRequestWindow.ShowDialog() == false)
                {
                    MessageBox.Show("Запит не створено");
                    
                    return;
                }

                var blogs = from b in modelContext.Mains
                            where b.NumbInput.Equals(createRequestWindow.CodeRequest)
                            select b;

                int count = 0;
                Main main = null!;
                foreach (var item in blogs)
                {
                    main = item;
                    count++;
                }

                if (count == 0)
                {
                    MessageBox.Show("Не вдалось відкрити запит, бо він не відкритий(не створений) в глобільній базі");
                    return;
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

                //перевірка ідентичності запиту
                var cod = from b in db.Requests
                          where b.numberIn.Equals(createRequestWindow.CodeRequest)
                          select b;

                foreach (var item in cod)
                {
                    MessageBox.Show("Запит уже створено");
                    return;
                }

                MyForm.FolderBrowserDialog FBD = new MyForm.FolderBrowserDialog();
                if (FBD.ShowDialog() == MyForm.DialogResult.OK)
                {
                    if (currentButton != AddButton)
                    {
                        var prevR = db.Requests.Find(Convert.ToInt32(currentButton.Tag));

                        if(prevR != null) 
                        {
                            prevR.numberKP = numberKPTextBox.Text;
                            prevR.numberIn = numberInTextBox.Text;
                            prevR.dateIn = dateInTextBox.Text;
                            prevR.dateControl = dateControlTextBox.Text;
                            prevR.typeOrgan = typeorgansList.SelectedIndex;
                            prevR.numberRequest = numberRequestTextBox.Text;
                            prevR.dateRequest = dateRequestDatePicker.SelectedDate;
                            prevR.vidOrg = vidOrgTextBox.Text;
                            prevR.addressOrg = addressOrgTextBox.Text;
                            prevR.positionSub = positionSubTextBox.Text;
                            prevR.nameSub = nameSubTextBox.Text;
                            prevR.numberOut = numberOutTextBox.Text;
                            prevR.dateOut = dateOutDatePicker.SelectedDate;
                            prevR.co_executor = co_executorTextBox.Text;
                            prevR.TEKA = TEKATextBox.Text;
                            prevR.article_CCU = article_CCUTextBox.Text;
                            prevR.note = noteTextBox.Text;
                            prevR.typeAppea = typeAppealList.SelectedIndex;
                        }
                        else 
                        { 
                            MessageBox.Show("Не вдалося відкрити запит з контексту локальної бази для збереження поточного запиту"); 
                        }

                        try
                        {
                            db.SaveChanges();
                        }
                        catch
                        {
                            MessageBox.Show("Не вдалося зберегти контекст локальної бази");
                        }

                        try
                        {
                            LocalToGlob();
                        }
                        catch
                        {
                            MessageBox.Show("Не вдалося зберегти дані з локальної бази до глобальної");
                        }
                    }

                    numberInTextBox.Text = createRequestWindow.CodeRequest;

                    var dr = main.DtOutInit;
                    var drstr = "";
                    if(dr != null)
                    {
                        drstr = dr.ToString();
                        if (drstr != null)
                        {
                            drstr = drstr.Substring(0, 10);
                        }
                        else
                        {
                            drstr = "";
                        }
                    }
                    var dOut = main.DtOutInit;
                    var dOutstr = "";
                    if (dOut != null)
                    {
                        dOutstr = dr.ToString();
                        if (dOutstr != null)
                        {
                            dOutstr = dOutstr.Substring(0, 10);
                        }
                        else
                        {
                            dOutstr = "";
                        }
                    }
                    var r = new Request()
                    {
                        pathDirectory = FBD.SelectedPath + $"\\{codeRequest}",
                        numberIn = createRequestWindow.CodeRequest,
                        count_Shemat = 0,
                        typeOrgan = 1,
                        isSubs = false,
                        numberKP = main.CpNumber == null ? "" : main.CpNumber,
                        dateIn = InStrDate(main.DtInput),
                        dateControl = InStrDate(main.DtCheck),
                        numberRequest = main.NumbOutInit == null ? "" : main.NumbOutInit,
                        dateRequest = main.DtOutInit,
                        vidOrg = main.AgencyDep == null ? "" : main.AgencyDep,
                        addressOrg = main.Addr == null ? "" : main.Addr,
                        positionSub = main.Work == null ? "" : main.Work,
                        nameSub = main.ExecutorInit == null ? "" : main.ExecutorInit,
                        numberOut = main.NumbOut == null ? "" : main.NumbOut,
                        dateOut = main.DtOut,
                        co_executor = main.CoExecutor == null ? "" : main.CoExecutor,
                        TEKA = main.Folder == null? "": main.Folder,
                        article_CCU = main.Art == null ? "" : main.Art,
                        note = main.Notes == null ? "" : main.Notes,
                        typeAppea = main.Status == "2" ? 1 : 0,
                        connectedPeople = "",
                        user = CurrentUser
                    };

                    numberKPTextBox.Text = r.numberKP;
                    numberInTextBox.Text = r.numberIn;
                    dateInTextBox.Text = r.dateIn;
                    dateControlTextBox.Text = r.dateControl;
                    typeorgansList.SelectedIndex = r.typeOrgan;
                    numberRequestTextBox.Text = r.numberRequest;

                    dateRequestDatePicker.SelectedDate = r.dateRequest;

                    vidOrgTextBox.Text = r.vidOrg;
                    addressOrgTextBox.Text = r.addressOrg;
                    positionSubTextBox.Text = r.positionSub;
                    nameSubTextBox.Text = r.nameSub;
                    numberOutTextBox.Text = r.numberOut;

                    
                    dateOutDatePicker.SelectedDate = r.dateOut;

                    co_executorTextBox.Text = r.co_executor;
                    TEKATextBox.Text = r.TEKA;
                    article_CCUTextBox.Text = r.article_CCU;
                    noteTextBox.Text = r.note;
                    typeAppealList.SelectedIndex = r.typeAppea;

                    db.Requests.Add(r);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        MessageBox.Show("Не вдалося зберегти контекст локальної бази");
                    }




                    int ind = stackPanel1.Children.IndexOf((System.Windows.Controls.Button)sender);
                    var itemButton = new System.Windows.Controls.Button();
                    itemButton.Click += Button_Any_Click_Req;
                    itemButton.Tag = r.id;
                    var NumForeground = 4;  // stackPanel1.Children.Count % 2 == 1 ? 4 : 1;
                    var NumBackground = 2;  // stackPanel1.Children.Count % 2 == 1 ? 2 : 3;


                    itemButton.Foreground = this.Resources[$"{NumForeground}ColorStyle"] as SolidColorBrush;
                    itemButton.Background = this.Resources[$"{NumBackground}ColorStyle"] as SolidColorBrush;
                    itemButton.Content = $"Запит {ind + 1}: {createRequestWindow.CodeRequest}";


                    for (int i = 0; i < ind; i++)
                    {
                        var but = stackPanel1.Children[i] as System.Windows.Controls.Button;
                        if(but != null)
                        {
                            but.Background = this.Resources[$"3ColorStyle"] as SolidColorBrush;
                            but.Foreground = this.Resources[$"1ColorStyle"] as SolidColorBrush;
                        }
                        else
                        {
                            MessageBox.Show($"Не вдалося встановити фон і колір тексту {i + 1}кнопки запиту. Кнопка не знайдена null");
                        }
                    }

                    stackPanel1.Children.Insert(0, itemButton);


                    Directory.CreateDirectory(FBD.SelectedPath + $"\\{codeRequest}");
                    int index = 1;
                    treeView1.Items.Clear();
                    foreach (var item in Reest.sNazyv)
                    {
                        Directory.CreateDirectory(FBD.SelectedPath + $"\\{codeRequest}\\{index}. {item}");
                        CheckBox checkBox = new CheckBox();
                        CheckBox checkBox2 = new CheckBox();

                        var parentItem = new TreeViewItem();
                        parentItem.Header = $"{index++}. " + item;
                        parentItem.Foreground = this.Resources[$"4ColorStyle"] as SolidColorBrush;
                        // treeView1.Items.Add(parentItem);
                        checkBox.Content = parentItem;
                        checkBox.Margin = new Thickness(0, 1, 0, 0);
                        checkBox2.Content = checkBox;
                        treeView1.Items.Add(checkBox2);
                    }
                    CheckBox checkBoxS = new CheckBox();
                    CheckBox checkBoxS2 = new CheckBox();

                    var treeS = new TreeViewItem();
                    treeS.Foreground = this.Resources[$"4ColorStyle"] as SolidColorBrush;
                    treeS.Header = $"{index}. Схеми";

                    checkBoxS.Content = treeS;
                    checkBoxS.Margin = new Thickness(0, 1, 0, 0);
                    checkBoxS2.Content = checkBoxS;
                    treeView1.Items.Add(checkBoxS2);
                    Directory.CreateDirectory(FBD.SelectedPath + $"\\{codeRequest}\\{index}. Схеми");

                    currentButton = itemButton;
                    try
                    {
                        LocalToGlob();
                    }
                    catch
                    {
                        MessageBox.Show("Не вдалося завантажити дані з локальної бази до глобальної");
                    }
               
                }   
                else
                {
                    MessageBox.Show("Запит не створено");
                    return;
                }
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.Message);
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


                var prevR = db.Requests.Find(Convert.ToInt32(currentButton.Tag));
                if(prevR != null)
                {
                    for (int i = 0; i < treeView1.Items.Count; i++)
                    {
                        var chB = treeView1.Items[i] as CheckBox;
                        if(chB != null)
                        {
                            var chBin = chB.IsChecked;
                            if(chBin != null)
                            {
                                prevR.SetDB(i + 1, (bool)chBin);
                            }
                            else
                            {
                                prevR.SetDB(i + 1, false);
                            }

                            var chBCon = chB.Content as CheckBox;
                            if(chBCon != null)
                            {
                                var chBConin  = chBCon.IsChecked;
                                if (chBConin != null)
                                {
                                    prevR.SetSchemeDB(i + 1, (bool)chBConin);
                                }
                                else
                                {
                                    prevR.SetSchemeDB(i + 1, false);
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Не вдалося відкрити(null) {i + 1}-й прапорець схеми. Схоже не існує");
                            }


                        }
                        else
                        {
                            MessageBox.Show($"Не вдалося відкрити(null) {i + 1}-й прапорець контролю. Схоже не існує");
                        }
                       
                       
                    }
                
                


                //Перевірка наявності папки запиту
                if (Directory.Exists(prevR.pathDirectory))
                {
                    //Отримання масиву вмісту папки запиту(реєстрів)
                    string[] dirs = Directory.GetDirectories(prevR.pathDirectory);

                    // Створення нового дерева
                    treeView1.Items.Clear();
                    int i = 1;

                    // Прохід по кожному реєстрі зі списку
                    foreach (string itemSNazyv in Reest.sNazyv)
                    {
                        //Створення вузла
                        TreeViewItem tree = new TreeViewItem();


                        CheckBox checkBox = new CheckBox();
                        checkBox.IsChecked = prevR.GetSchemeDB(i);
                        CheckBox checkBox2 = new CheckBox();
                        checkBox2.IsChecked = prevR.GetDB(i);
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
                        //CheckBox checkBox = new CheckBox();
                        //checkBox.IsChecked = prevR.GetNecessaryDB(i);
                        //CheckBox checkBox2 = new CheckBox();
                        //checkBox2.IsChecked = prevR.GetDB(i);
                        //checkBox.Content = tree;
                        //checkBox.Margin = new Thickness(0, 1, 0, 0);
                        //checkBox2.Content = checkBox;
                        ////  treeView1.Items.Add(tree);
                        treeView1.Items.Add(checkBox2);
                        i++;
                    }

                    TreeViewItem treeS = new TreeViewItem();

                    CheckBox checkBoxS = new CheckBox();
                    checkBoxS.IsChecked = prevR.GetSchemeDB(i);
                    CheckBox checkBoxS2 = new CheckBox();
                    checkBoxS2.IsChecked = prevR.GetDB(i);
                    checkBoxS.Content = treeS;
                    checkBoxS.Margin = new Thickness(0, 1, 0, 0);
                    checkBoxS2.Content = checkBoxS;

                    treeS.Foreground = new SolidColorBrush(Color.FromRgb(230, 78, 78));
                    treeS.Header = $"{i}. Схеми";
                    foreach (var item in dirs)
                    {
                        if((new DirectoryInfo(item)).Name == $"{i}. Схеми")
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
                    if (isLoad)
                    {
                        MessageBox.Show($"Папка запиту не знайдена");
                    }


                }
                }
                else
                {
                    MessageBox.Show("Не вдалося зберегти поточний запит. Не знайдено запит в контексті локальної бази");
                }
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.Message);
            }
        }
        private void Button_ClickRespon(object sender, RoutedEventArgs e)
        {
            try 
            {

                //MessageBox.Show("Запит не створено");
                if (currentButton == AddButton) return;
                int iPrap = 1;
                foreach (var item in treeView1.Items)
                {
                        var chB = item as CheckBox;
                        if(chB != null)
                        {
                            var b = chB.IsChecked;
                            if(b != null)
                            {
                                if (!(bool)b)
                                {
                                    MessageBox.Show($"Не відмічено контроль");
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Не відмічено контроль(null)");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Прапорець номер {iPrap} контролю не визначений");
                            return;
                        }
                    iPrap++;
                }

                //Створення документу звіту
                var prevR = db.Requests.Find(Convert.ToInt32(currentButton.Tag));
                XWPFDocument doc1;
                if (prevR != null)
                {
                    Directory.CreateDirectory(prevR.pathDirectory + "\\Шаблон");

                    var exeFath = /*AppDomain.CurrentDomain.BaseDirectory*/  Environment.CurrentDirectory;
                    var path = System.IO.Path.Combine(exeFath, "Files\\1.docx");

                    FileInfo fileInfo = new FileInfo(path);

                    //FileInfo fileInfo = new FileInfo("C:\\ProjectARMA\\DesARMA\\DesARMA\\Files\\1.docx");
                    var path2 = System.IO.Path.Combine(exeFath, "FilesRet\\2.docx");
                    var path3 = prevR.pathDirectory + "\\Шаблон\\Шаблон відповіді.docx";
                    //fileInfo.CopyTo("C:\\ProjectARMA\\DesARMA\\DesARMA\\FilesRet\\2.docx", true);

                    fileInfo.CopyTo(path3, true);
                    //XWPFDocument doc1 = new XWPFDocument(OPCPackage.Open("C:\\ProjectARMA\\DesARMA\\DesARMA\\FilesRet\\2.docx"));

                    doc1 = new XWPFDocument(OPCPackage.Open(path3));
                }
                else
                {
                    MessageBox.Show($"Не вдалося відкрити контекст запиту. Не знайдено його");
                    return;
                }
               

                var indexSub = 1; 
                //Створення зміних, що вставляються в звіт
                int whatIndex = 0; //type organ
                whatIndex = typeorgansList.SelectedIndex;
                indexSub = 1;
                string name = nameSubTextBox.Text;
                string address1 = addressOrgTextBox.Text;
                string date1 = dateRequestDatePicker.Text;
                string date2 = dateInTextBox.Text.Substring(0, 10);
                string number1 = numberRequestTextBox.Text;
                string number2 = numberInTextBox.Text;
                int count_Shemat = 0;
                string vidOrgan = vidOrgTextBox.Text;
                string positionSub =   positionSubTextBox.Text;

                //Збереження параметрів в контекст бд
                var r = db.Requests.Find(Convert.ToInt32(currentButton.Tag));

                if(r != null)
                {
                    //перевірка к-сті фігурантів
                    var fos = from rf in db.Requests
                              from rin in rf.fos
                              where rf.id == r.id
                              select rin;
                    var uos = from ru in db.Requests
                              from rin in ru.uos
                              where ru.id == r.id
                              select rin;

                    var countFig = 0;
                    foreach (var item in fos)
                    {
                        if (!item.isConnectedPeople)
                        {
                            countFig++;
                        }
                    }

                    foreach (var item in uos)
                    {
                        if (!item.isConnectedPeople)
                        {
                            countFig++;
                        }
                    }
                    if (countFig > 1)
                    {
                        indexSub = 0;
                    }
                    if (countFig == 1)
                    {
                        indexSub = 1;
                    }
                    if (countFig == 0)
                    {
                        MessageBox.Show("Не введено ні одного фігуранта");
                        return;
                    }

                    r.isSubs = indexSub == 1;
                    r.numberKP = numberKPTextBox.Text;
                    r.numberIn = numberInTextBox.Text;
                    r.dateIn = dateInTextBox.Text;
                    r.dateControl = dateControlTextBox.Text;
                    r.typeOrgan = typeorgansList.SelectedIndex;
                    r.numberRequest = numberRequestTextBox.Text;
                    r.dateRequest = dateRequestDatePicker.SelectedDate;
                    r.vidOrg = vidOrgTextBox.Text;
                    r.addressOrg = addressOrgTextBox.Text;
                    r.positionSub = positionSubTextBox.Text;
                    r.nameSub = nameSubTextBox.Text;
                    r.numberOut = numberOutTextBox.Text;
                    r.dateOut = dateOutDatePicker.SelectedDate;
                    r.co_executor = co_executorTextBox.Text;
                    r.TEKA = TEKATextBox.Text;
                    r.article_CCU = article_CCUTextBox.Text;
                    r.note = noteTextBox.Text;
                    r.typeAppea = typeAppealList.SelectedIndex;

                    for (int i = 0; i < treeView1.Items.Count; i++)
                    {
                        var chB = treeView1.Items[i] as CheckBox;

                        if(chB != null)
                        {
                            var chBisCh = chB.IsChecked;
                            if (chBisCh != null)
                            {
                                r.SetDB(i + 1, (bool)chBisCh);
                            }
                            else
                            {
                                r.SetDB(i + 1, false);
                            }
                            var chBCon = chB.Content as CheckBox;
                            if( chBCon != null)
                            {
                                var chBConIs = chBCon.IsChecked;
                                if (chBConIs != null)
                                {
                                    r.SetSchemeDB(i + 1, (bool)chBConIs);
                                    if ((bool)chBConIs)
                                    {
                                        count_Shemat++;
                                    }
                                }
                                else
                                {
                                    r.SetSchemeDB(i + 1, false);
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {

                        }

                    }

                    r.count_Shemat = count_Shemat;

                    try 
                    {
                        db.SaveChanges();
                    } 
                    catch
                    {
                        MessageBox.Show($"Не вдалося зберегти контекст локальної бази");
                    }
                    try
                    {
                        LocalToGlob();
                    }
                    catch
                    {
                        MessageBox.Show($"Не записати даны з локальної бази в глобальну");
                    }


                    //Створення списків реєстрів родовий і давальний
                    List<string> listSRod = new List<string>();
                    List<string> listSDav = new List<string>();


                    //Збереження даних наявностей реєстрів
                    if (Directory.Exists(r.pathDirectory))
                    {
                        int i = 1;
                        foreach (string itemSNazyv in Reest.sNazyv)
                        {
                            string[] dirs = Directory.GetDirectories(r.pathDirectory);
                            foreach (var itemDir in dirs)
                            {
                               

                                if ((new DirectoryInfo(itemDir)).Name == $"{i}. " + itemSNazyv)
                                {
                                    if (Directory.GetDirectories(itemDir).Length > 0 ||
                                         Directory.GetFiles(itemDir).Length > 0
                                    )
                                    {
                                        listSRod.Add(Reest.sRodov[Reest.sNazyv.IndexOf(itemSNazyv)]);

                                        r.SetNecessaryDB(i, true);
                                    }
                                    else
                                    {
                                        if (Reest.sNazyv.IndexOf(itemSNazyv) != 29)
                                            listSDav.Add(Reest.sDav[Reest.sNazyv.IndexOf(itemSNazyv)]);

                                        r.SetNecessaryDB(i, true);
                                    }
                                }
                            }
                            i++;
                        }
                        var d = Directory.GetDirectories(r.pathDirectory);
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
                        MessageBox.Show("Не знайдено папку запиту");
                        doc1.Close();
                        return;
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        MessageBox.Show($"Не вдалося зберегти контекст локальної бази");
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


                    par = doc1.Paragraphs[34];
                    par.ReplaceText(par.Text, Reest.organs[whatIndex]);

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
                    Console.WriteLine(listSDav.Count);

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



                    var path4 = prevR.pathDirectory + "\\Шаблон\\Відповідь.docx";

                    //Збереження звіта 
                    using (FileStream sw = File.Create(path4))
                    {
                        doc1.Write(sw);
                        // doc1.Close();
                    }


                    doc1.Close();

                    MessageBox.Show($"Відповідь збережено в папку:\n{path4}");
                }
                else
                {
                    MessageBox.Show($"Не вдалося відкрити контекст запиту. Не знайдено його");
                    return;
                }

            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.Message);
               
            }


        }
        private void ButtonSearchClick(object sender, RoutedEventArgs e)
        {
            try { 
            var blogs = from b in db.Requests
                        where b.numberIn.Equals(textBoxSearch.Text) && b.user.Id == CurrentUser.Id
                        select b;
            var s = "";
            int ind = -1;

            foreach (var item in blogs)
            {
                ind = item.id;

                s += item.id + ";\n";

                break;
            }
            if (ind != -1)
                CreateButton(ind);
                // MessageBox.Show(s);
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.Message);
            }
        }
        private void ClickDefendantsButton(object sender, RoutedEventArgs e)
        {
            try { 
            var r = db.Requests.Find(Convert.ToInt32(currentButton.Tag));

            if (r != null)
            {
                
                ListDefendantsWindow listDefendantsWindow = new ListDefendantsWindow(Convert.ToInt32(currentButton.Tag),
                    db, modelContext, numberInTextBox.Text, "Перелік фігурантів", false);

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
                MessageBox.Show(e2.Message);
            }
        }
        private void CreateButton(int indB)
        {
            try {
                var reqItAdd = db.Requests.Find(indB);
                if (reqItAdd != null)
                {
                    stackPanel1.Children.Clear();

                    int i = 1;
                    foreach (var r in db.Requests)
                    {
                        if (r.id != indB)
                        {
                            var itemButton = new System.Windows.Controls.Button();
                            itemButton.Click += Button_Any_Click_Req;


                            var NumForeground2 = 4;  // stackPanel1.Children.Count % 2 == 1 ? 4 : 1;
                            var NumBackground2 = 2;  // stackPanel1.Children.Count % 2 == 1 ? 2 : 3;

                            itemButton.Foreground = this.Resources[$"{NumForeground2}ColorStyle"] as SolidColorBrush;
                            itemButton.Background = this.Resources[$"{NumBackground2}ColorStyle"] as SolidColorBrush;


                            itemButton.Tag = r.id;

                            itemButton.Content = $"Запит {r.id}: {r.numberIn}";
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



                    itemButton2.Tag = indB;
                    itemButton2.Content = $"Запит {indB}: {reqItAdd.numberIn}";
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
                MessageBox.Show(e2.Message);
            }
        }
        private void ClickСonnectedPeopleButton(object sender, RoutedEventArgs e)
        {
            try { 
            var r = db.Requests.Find(Convert.ToInt32(currentButton.Tag));

            if (r != null)
            {
                ListDefendantsWindow listDefendantsWindow = new ListDefendantsWindow(Convert.ToInt32(currentButton.Tag), db, modelContext, numberInTextBox.Text, "Перелік пов'язаних осіб", true);

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
                MessageBox.Show(e2.Message);
            }
        }
        private void GlobToLocal()
        {
            var mainR = from b in modelContext.Mains
                        where b.NumbInput.Equals(numberInTextBox.Text)
                        select b;

            var r = db.Requests.Find(Convert.ToInt32(currentButton.Tag));
            if(r!= null)
            {
                foreach (var item in mainR)
                {

                    r.numberKP = "" + item.CpNumber;
                    r.numberIn = item.NumbInput;
                    r.dateIn = "" + item.DtInput.ToString();
                    r.dateControl = "" + item.DtCheck.ToString();
                    r.numberRequest = "" + item.NumbOutInit;
                    r.dateRequest = item.DtOutInit;
                    r.vidOrg = "" + item.AgencyDep;
                    r.addressOrg = "" + item.Addr;
                    r.positionSub = "" + item.Work;
                    r.nameSub = "" + item.ExecutorInit;
                    r.numberOut = "" + item.NumbOut;
                    r.dateOut =  item.DtOut;
                    r.co_executor = "" + item.CoExecutor;
                    r.TEKA = "" + item.Folder;
                    r.article_CCU = "" + item.Art;
                    r.note = "" + item.Notes;
                    r.typeAppea = item.Status == "2" ? 1 : 0;

                    break;
                }
            }
            
            db.SaveChanges();
            modelContext.SaveChanges();
        }
        private void LocalToGlob()
        {
            try 
            { 
                var mainR = from b in modelContext.Mains
                            where b.NumbInput.Equals(numberInTextBox.Text)
                            select b;

                var r = db.Requests.Find(Convert.ToInt32(currentButton.Tag));
                    if (r != null)
                    {
                        Main m = null!;
                        foreach (var item in mainR)
                        {
                            m = item;  
                            break;
                        }
                        if (m != null)
                        {
                            m.NumbOutInit = r.numberRequest;
                            m.DtOutInit = r.dateRequest;
                            m.AgencyDep = r.vidOrg;
                            m.Addr = r.addressOrg;
                            m.Work = r.positionSub;
                            m.ExecutorInit = r.nameSub;
                            m.NumbOut = r.numberOut;
                            m.DtOut = r.dateOut;
                            m.CoExecutor = r.co_executor;
                            m.Folder = r.TEKA;
                            m.Art = r.article_CCU;
                            m.Notes = r.note;
                            m.Status = (r.typeAppea + 1).ToString() /*== 1 ? "в роботі": "закрито"*/;
                        }
                     }
                try
                {
                    modelContext.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Виникла проблема збереження контексту глобальної бази.");
                }
                    
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.Message);
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
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }
        private void textBox_MouseDown(object sender, MouseEventArgs e)
        {
            try 
            {
                System.Windows.Controls.TextBox dp = (System.Windows.Controls.TextBox)sender;
                dp.SelectionStart = 0;
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }
        private void SaveReqLocalGlob()
        {
            try
            {
                var prevR = db.Requests.Find(Convert.ToInt32(currentButton.Tag));
                if (prevR != null)
                {
                    prevR.numberKP = numberKPTextBox.Text;
                    prevR.numberIn = numberInTextBox.Text;
                    prevR.dateIn = dateInTextBox.Text;
                    prevR.dateControl = dateControlTextBox.Text;
                    prevR.typeOrgan = typeorgansList.SelectedIndex;
                    prevR.numberRequest = numberRequestTextBox.Text;
                    prevR.dateRequest = dateRequestDatePicker.SelectedDate;
                    prevR.vidOrg = vidOrgTextBox.Text;
                    prevR.addressOrg = addressOrgTextBox.Text;
                    prevR.positionSub = positionSubTextBox.Text;
                    prevR.nameSub = nameSubTextBox.Text;
                    prevR.numberOut = numberOutTextBox.Text;
                    prevR.dateOut = dateOutDatePicker.SelectedDate;
                    prevR.co_executor = co_executorTextBox.Text;
                    prevR.TEKA = TEKATextBox.Text;
                    prevR.article_CCU = article_CCUTextBox.Text;
                    prevR.note = noteTextBox.Text;
                    prevR.typeAppea = typeAppealList.SelectedIndex;

                    for (int i = 0; i < treeView1.Items.Count; i++)
                    {
                        var chB = treeView1.Items[i] as CheckBox;
                        if (chB != null)
                        {
                            var chBisCh = chB.IsChecked;
                            if (chBisCh != null)
                            {
                                prevR.SetDB(i + 1, (bool)chBisCh);
                            }
                            else
                            {
                                prevR.SetDB(i + 1, false);
                            }

                            var chBCon = chB.Content as CheckBox;
                            if (chBCon != null)
                            {
                                var chBConisCh = chBCon.IsChecked;
                                if (chBConisCh != null)
                                {
                                    prevR.SetSchemeDB(i + 1, (bool)chBConisCh);
                                }
                                else
                                {
                                    prevR.SetSchemeDB(i + 1, false);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Прапорець контролю не збережено в базу");
                        }
                    }

                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        MessageBox.Show($"Не вдалося зберегти контекст локальної бази");
                    }
                    try
                    {
                        LocalToGlob();
                    }
                    catch
                    {
                        MessageBox.Show($"Не записати даны з локальної бази в глобальну");
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void App_Closing(object sender, CancelEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show(this, "Вийти?",
                "Закрити", MessageBoxButton.YesNo, MessageBoxImage.Question);
            SaveReqLocalGlob();
            if (MessageBoxResult.No == result)
            {
                e.Cancel = true;
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
                if(t!=null)
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
                MessageBox.Show(exp.Message);
            }
        }

        private void ClickReqButton(object sender, RoutedEventArgs e)
        {
            RequestsWindow requestsWindow = new RequestsWindow();

            if (requestsWindow.ShowDialog() == true)
            {

            }
            else
            {

            }

        }
    }

}
