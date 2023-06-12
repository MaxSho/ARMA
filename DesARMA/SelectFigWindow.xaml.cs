using DesARMA.Models;
using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using static NPOI.HSSF.Util.HSSFColor;
using System.Timers;
using System.Configuration;
using System.Collections.ObjectModel;
using DesARMA.Log;
using DesARMA.Log.Data;
using System.Runtime.CompilerServices;

namespace DesARMA
{
    /// <summary>
    /// Interaction logic for SelectFigWindow.xaml
    /// </summary>
    public partial class SelectFigWindow : Window
    {
        //string inputNumber = "";
        List<string> inputNumberList = new List<string>();
        ModelContext modelContext;
        EnumExtReq enumExtReq;
        TypeOfAppeal typeOfAppeal;
        private System.Windows.Forms.Timer inactivityTimer = new System.Windows.Forms.Timer();
        public SelectFigWindow(List<string> inputNumberList, ModelContext modelContext, EnumExtReq enumExtReq, string nameWin,
            TypeOfAppeal typeOfAppeal, System.Windows.Forms.Timer inactivityTimer)
        {
            InitializeComponent();
            try
            {
                this.inactivityTimer = inactivityTimer;
                this.inputNumberList = inputNumberList;
                this.modelContext = modelContext;
                this.enumExtReq = enumExtReq;
                this.typeOfAppeal = typeOfAppeal;
                this.Title += ". " + nameWin;


                Init();
                
                

                inactivityTimer.Start();
            }
            catch (Exception exp1)
            {
                MessageBox.Show("" + exp1.Message);
            }

            //string shif = ConfigurationManager.AppSettings["hv"].ToString();
            //inactivityTimer.Interval = 60_000 * Convert.ToInt32(shif);
            //inactivityTimer.Tick += (sender, args) =>
            //{
            //    Environment.Exit(0);
            //};
            inactivityTimer.Start();
        }
        private void Init()
        {
            List<Figurant> items;
            if(typeOfAppeal == TypeOfAppeal.NotСombined)
            {
                items = (from f in modelContext.Figurants
                            where f.NumbInput == inputNumberList.First() && f.Status == 1
                            select f).ToList();
            }
            else
            {
                items = (from f in modelContext.Figurants
                            where inputNumberList.Contains(f.NumbInput) && f.Status == 1
                            select f).ToList();
            }
            
            foreach (var item in items)
            {
                CheckBox check = new CheckBox();
                check.IsChecked = true;
                check.Margin = new Thickness(10);
                check.Padding = new Thickness(10, -4, 0, 0);
                check.Background = this.Resources[$"3ColorStyle"] as SolidColorBrush;
                check.Tag = item.Id;

                Label label = new Label();
                if (item.ResFiz != null)
                    label.Content = $"{item.Fio} {item.Ipn}";
                else
                    label.Content = $"{item.Name} {item.Code}";

                label.Padding = new Thickness(0);
                label.Margin = new Thickness(0);
                label.FontSize = 16;

                check.Content = label;
                stackPanelAdd.Children.Add(check);
            }

            if (enumExtReq == EnumExtReq.ExternalRequestsToMytna)
            {
                stackPanelAddElse.Children.Clear();
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToIntelektualnyi)
            {
                stackPanelAddElse.Children.Clear();
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToHeolohii)
            {
                stackPanelAddElse.Children.Clear();
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToDerzhpratsi)
            {
                var l = stackPanelAddElse.Children[0] as Label;
                var itemOrganName = stackPanelAddElse.Children[1] as ComboBox;

                if (l != null && itemOrganName != null)
                {
                    l.Content = "Назва органу";
                    List<string> listBankName = (from b in modelContext.DictAgWorks
                                                 select b.Name).ToList();
                    listBankName.Sort();
                    itemOrganName.ItemsSource = listBankName;
                }
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToAntymonopolnyi)
            {
                stackPanelAddElse.Children.Clear();
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToFondovyi1)
            {
                stackPanelAddElse.Children.Clear();
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToFondovyiOsnovnyi2)
            {
                stackPanelAddElse.Children.Clear();
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToNAPZK)
            {
                stackPanelAddElse.Children.Clear();
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToBank)
            {
                stackPanelAddElse.Children.RemoveAt(2);

                var itemBankNameComboBox = stackPanelAddElse.Children[1] as ComboBox;
                if (itemBankNameComboBox != null)
                {
                    List<string> listBankName = (from b in modelContext.DictBanks
                                                 select b.Nb).ToList();
                    listBankName.Sort();
                    ObservableCollection<string> listBanks = new ObservableCollection<string>();
                    foreach (string bankName in listBankName)
                    {
                        listBanks.Add(bankName);
                    }

                    CollectionViewSource cvs = new CollectionViewSource();
                    cvs.Source = listBanks;

                    //itemBankName.ItemsSource = listBankName;
                    itemBankNameComboBox.ItemsSource = cvs.View;

                    itemBankNameComboBox.KeyUp += (s, e) =>
                    {
                        cvs.View.Filter = (item) => (item as string).ToLower().Contains(itemBankNameComboBox.Text.ToLower());
                        itemBankNameComboBox.IsDropDownOpen = true;
                    };
                }
            }
            else
            {
                stackPanelAddElse.Children.RemoveAt(stackPanelAddElse.Children.Count - 1);
            }

            string str = "";
            foreach (var item in stackPanelAddElse.Children)
            {
                var l = item as Label;
                var t = item as TextBox;
                var d = item as DatePicker;

                if (t != null)
                {
                    str += $"{t.Tag}\n";
                }
                else if (d != null)
                {
                    str += $"{d.Tag}\n";
                }
                else if (l != null)
                {
                    str += $"{l.Tag}\n";
                }
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            for (int i = 0; i < stackPanelAdd.Children.Count; i++)
            {
                var ch = stackPanelAdd.Children[i] as CheckBox;
                if (ch != null)
                {
                    ch.IsChecked = chAll.IsChecked;
                }
            }
            inactivityTimer.Start();
        }
        private void CreateReq(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                var listF = GetNumInpSelFig();

                List<Figurant> figs = new List<Figurant>();

                List<Figurant> items = new List<Figurant>();

                if (typeOfAppeal == TypeOfAppeal.NotСombined)
                {
                    items = (from f in modelContext.Figurants
                             where f.NumbInput == inputNumberList.First() && f.Status == 1
                             select f).ToList();

                    

                    var mc = modelContext.MainConfigs.Find(inputNumberList.First());
                    if (mc != null)
                    {
                        string? fold = mc.Folder;

                        foreach (var item in items)
                        {
                            if (listF.IndexOf(item.Id) != -1)
                                figs.Add(item);
                        }

                        if (figs.Count == 0)
                            MessageBox.Show("Не вибрано жодного фігуранта!");
                        else
                        {
                            exMeth(figs, fold);
                            

                        }
                    }
                }
                else
                {
                    items = (from f in modelContext.Figurants
                            where inputNumberList.Contains(f.NumbInput) && f.Status == 1
                            select f).ToList();

                    foreach (var itemNI in inputNumberList)
                    {
                        var mc = modelContext.MainConfigs.Find(itemNI);
                        if (mc != null)
                        {
                            string? fold = mc.Folder + "\\Об'єднана відповідь";

                            foreach (var item in items)
                            {
                                if (listF.IndexOf(item.Id) != -1)
                                    figs.Add(item);
                            }

                            if (figs.Count == 0)
                                MessageBox.Show("Не вибрано жодного фігуранта!");
                            else
                            {
                                exMethComb(figs, fold, itemNI);
                            }
                        }
                    }
                    

                }   
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            inactivityTimer.Start();
        }
        private void exMeth(List<Figurant> figs, string? fold)
        {
            if (enumExtReq == EnumExtReq.ExternalRequestsToMytna)
            {
                var m = modelContext.Mains.Find(inputNumberList.First());
                if (m != null)
                {
                    var numKP = m.CpNumber;
                    if (numKP != null)
                    {
                        ExternalRequests.ExternalRequestsToMytna(figs, fold, numKP);
                        MessageBox.Show($"Запит збережено за розташуванням {fold}\\Запити\\Запит Держмитслужба");
                        App.CurUser.LogInf(new CreateRequestData(App.CurUser.LoginName, TypeLogData.Access, inputNumberList.First(), $"Запит збережено за розташуванням {fold}\\Запити\\Запит Держмитслужба"));
                    }
                }   
            }
            else if(enumExtReq == EnumExtReq.ExternalRequestsToIntelektualnyi)
            {
                ExternalRequests.ExternalRequestsToIntelektualnyi(figs, fold);
                MessageBox.Show($"Запит збережено за розташуванням {fold}\\Запити\\Запит Укрпатент.docx");
                App.CurUser.LogInf(new CreateRequestData(App.CurUser.LoginName, TypeLogData.Access, inputNumberList.First(), $"Запит збережено за розташуванням {fold}\\Запити\\Запит Укрпатент.docx"));

            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToHeolohii)
            {
                ExternalRequests.ExternalRequestsToHeolohii(figs, fold);
                MessageBox.Show($"Запит збережено за розташуванням {fold}\\Запити\\Запит Геонадра.docx");
                App.CurUser.LogInf(new CreateRequestData(App.CurUser.LoginName, TypeLogData.Access, inputNumberList.First(), $"Запит збережено за розташуванням {fold}\\Запити\\Запит Геонадра.docx"));
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToDerzhpratsi)
            {
                var itemOrganName = stackPanelAddElse.Children[1] as ComboBox;


                if (itemOrganName != null)
                {
                    if(itemOrganName.SelectedIndex == -1)
                    {
                        MessageBox.Show("Не вибрано орган");
                        return;
                    }
                    var addr = (from b in modelContext.DictAgWorks
                                where b.Name == itemOrganName.SelectedItem as string
                                select b.Addr).ToList();

                    var dfs = itemOrganName.SelectedItem as string;
                    if (addr != null && dfs != null)
                    {
                        ExternalRequests.ExternalRequestsToDerzhpratsi(figs, fold, addr[0], dfs);
                        MessageBox.Show($"Запит збережено за розташуванням {fold}\\Запити\\Держпраці\\Відповідь {itemOrganName.SelectedItem}.docx");
                        App.CurUser.LogInf(new CreateRequestData(App.CurUser.LoginName, TypeLogData.Access, inputNumberList.First(), $"Запит збережено за розташуванням {fold}\\Запити\\Держпраці\\Відповідь {itemOrganName.SelectedItem}.docx"));

                    }

                }
            }
            else if(enumExtReq == EnumExtReq.ExternalRequestsToAntymonopolnyi)
            {
                var m = modelContext.Mains.Find(inputNumberList.First());
                if(m != null)
                {
                    var numKP = m.CpNumber;

                    if (numKP != null)
                    {
                        ExternalRequests.ExternalRequestsToAntymonopolnyi(figs, fold, numKP);
                        MessageBox.Show($"Запит збережено за розташуванням {fold}\\Запити\\АМК\\Запит АМК.docx");
                        App.CurUser.LogInf(new CreateRequestData(App.CurUser.LoginName, TypeLogData.Access, inputNumberList.First(), $"Запит збережено за розташуванням {fold}\\Запити\\АМК\\Запит АМК.docx"));
                    }
                }
            }
            else if(enumExtReq == EnumExtReq.ExternalRequestsToFondovyi1)
            {
                ExternalRequests.ExternalRequestsToFondovyi1(figs, fold);
                MessageBox.Show($"Запит збережено за розташуванням {fold}\\Запити\\НКЦПФР 1\\Запит НКЦПФР.docx");
                App.CurUser.LogInf(new CreateRequestData(App.CurUser.LoginName, TypeLogData.Access, inputNumberList.First(), $"Запит збережено за розташуванням {fold}\\Запити\\НКЦПФР 1\\Запит НКЦПФР.docx"));

            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToFondovyiOsnovnyi2)
            {
                ExternalRequests.ExternalRequestsToFondovyiOsnovnyi2(figs, fold);
                MessageBox.Show($"Запит збережено за розташуванням {fold}\\Запити\\НКЦПФР 2\\Запит НКЦПФР2.docx");
                App.CurUser.LogInf(new CreateRequestData(App.CurUser.LoginName, TypeLogData.Access, inputNumberList.First(), $"Запит збережено за розташуванням {fold}\\Запити\\НКЦПФР 2\\Запит НКЦПФР2.docx"));

            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToNAPZK)
            {
                var m = modelContext.Mains.Find(inputNumberList.First());
                if (m != null)
                {
                    var numKP = m.CpNumber;

                    if (numKP != null)
                    {
                        ExternalRequests.ExternalRequestsToNAPZK(figs, fold, numKP);
                        MessageBox.Show($"Запит збережено за розташуванням {fold}\\Запити\\НАЗК\\Запит НАЗК.docx");
                        App.CurUser.LogInf(new CreateRequestData(App.CurUser.LoginName, TypeLogData.Access, inputNumberList.First(), $"Запит збережено за розташуванням {fold}\\Запити\\НАЗК\\Запит НАЗК.docx"));

                    }
                }
                
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToBank)
            {
                var nameBank = stackPanelAddElse.Children[1] as ComboBox;

                var m = modelContext.Mains.Find(inputNumberList.First());
                if (m != null && nameBank != null)
                {
                    if(nameBank.SelectedIndex == -1)
                    {
                        MessageBox.Show("Оберіть назву банку");
                        return;
                    }

                    var itemBank = (from b1 in modelContext.DictBanks
                                    where b1.Nb == nameBank.Text
                                    select b1).ToList().First();
                    string? addr = "";
                    decimal? mfo = 0;
                    

                    addr = $"{itemBank.Adress}, м. {itemBank.Np}, {GetStringPI(itemBank.Pi)}";
                    mfo = itemBank.Mfo;

                    var numKP = m.CpNumber;

                    if (numKP != null)
                    {
                        ExternalRequests.ExternalRequestsToBank(figs, fold,  numKP, nameBank.Text, mfo + "", addr);
                        MessageBox.Show($"Запит збережено за розташуванням {fold}\\Запити\\Банки\\Запит {nameBank.Text.Replace('\"', ' ')} - ... .docx");
                        App.CurUser.LogInf(new CreateRequestData(App.CurUser.LoginName, TypeLogData.Access, inputNumberList.First(), $"Запит збережено за розташуванням {fold}\\Запити\\Банки\\Запит {nameBank.Text.Replace('\"', ' ')} - ... .docx"));
                    }
                }
            }
        }
        private void exMethComb(List<Figurant> figs, string? fold, string numberInput)
        {
            if (enumExtReq == EnumExtReq.ExternalRequestsToMytna)
            {
                var m = modelContext.Mains.Find(numberInput);
                if (m != null)
                {
                    var numKP = m.CpNumber;
                    if (numKP != null)
                    {
                        ExternalRequests.ExternalRequestsToMytna(figs, fold, numKP);
                        MessageBox.Show($"Запит збережено за розташуванням {fold}\\Запити\\Запит Держмитслужба");
                        App.CurUser.LogInf(new CreateRequestData(App.CurUser.LoginName, TypeLogData.Access, inputNumberList.First(), $"Об'єднаний запит збережено за розташуванням {fold}\\Запити\\Запит Держмитслужба"));

                    }
                }
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToIntelektualnyi)
            {
                ExternalRequests.ExternalRequestsToIntelektualnyi(figs, fold);
                MessageBox.Show($"Запит збережено за розташуванням {fold}\\Запити\\Запит Укрпатент.docx");
                App.CurUser.LogInf(new CreateRequestData(App.CurUser.LoginName, TypeLogData.Access, inputNumberList.First(), $"Об'єднаний запит збережено за розташуванням {fold}\\Запити\\Запит Укрпатент.docx"));
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToHeolohii)
            {
                ExternalRequests.ExternalRequestsToHeolohii(figs, fold);
                MessageBox.Show($"Запит збережено за розташуванням {fold}\\Запити\\Запит Геонадра.docx");
                App.CurUser.LogInf(new CreateRequestData(App.CurUser.LoginName, TypeLogData.Access, inputNumberList.First(), $"Об'єднаний запит збережено за розташуванням {fold}\\Запити\\Запит Геонадра.docx"));
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToDerzhpratsi)
            {
                var itemOrganName = stackPanelAddElse.Children[1] as ComboBox;


                if (itemOrganName != null)
                {
                    if (itemOrganName.SelectedIndex == -1)
                    {
                        MessageBox.Show("Не вибрано орган");
                        return;
                    }
                    var addr = (from b in modelContext.DictAgWorks
                                where b.Name == itemOrganName.SelectedItem as string
                                select b.Addr).ToList();

                    var dfs = itemOrganName.SelectedItem as string;
                    if (addr != null && dfs != null)
                    {
                        ExternalRequests.ExternalRequestsToDerzhpratsi(figs, fold, addr[0], dfs);
                        MessageBox.Show($"Об'єднаний запит збережено за розташуванням {fold}\\Запити\\Держпраці\\Відповідь {itemOrganName.SelectedItem}.docx");
                        App.CurUser.LogInf(new CreateRequestData(App.CurUser.LoginName, TypeLogData.Access, inputNumberList.First(), $"Запит збережено за розташуванням {fold}\\Запити\\Держпраці\\Відповідь {itemOrganName.SelectedItem}.docx"));

                    }

                }
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToAntymonopolnyi)
            {
                var m = modelContext.Mains.Find(numberInput);
                if (m != null)
                {
                    var numKP = m.CpNumber;

                    if (numKP != null)
                    {
                        ExternalRequests.ExternalRequestsToAntymonopolnyi(figs, fold, numKP);
                        MessageBox.Show($"Об'єднаний запит збережено за розташуванням {fold}\\Запити\\АМК\\Запит АМК.docx");
                        App.CurUser.LogInf(new CreateRequestData(App.CurUser.LoginName, TypeLogData.Access, inputNumberList.First(), $"Запит збережено за розташуванням {fold}\\Запити\\АМК\\Запит АМК.docx"));

                    }
                }
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToFondovyi1)
            {
                ExternalRequests.ExternalRequestsToFondovyi1(figs, fold);
                MessageBox.Show($"Об'єднаний запит збережено за розташуванням {fold}\\Запити\\НКЦПФР 1\\Запит НКЦПФР.docx");
                App.CurUser.LogInf(new CreateRequestData(App.CurUser.LoginName, TypeLogData.Access, inputNumberList.First(), $"Запит збережено за розташуванням {fold}\\Запити\\НКЦПФР 1\\Запит НКЦПФР.docx"));
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToFondovyiOsnovnyi2)
            {
                ExternalRequests.ExternalRequestsToFondovyiOsnovnyi2(figs, fold);
                MessageBox.Show($"Об'єднаний запит збережено за розташуванням {fold}\\Запити\\НКЦПФР 2\\Запит НКЦПФР2.docx");
                App.CurUser.LogInf(new CreateRequestData(App.CurUser.LoginName, TypeLogData.Access, inputNumberList.First(), $"Запит збережено за розташуванням {fold}\\Запити\\НКЦПФР 2\\Запит НКЦПФР2.docx"));
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToNAPZK)
            {
                var m = modelContext.Mains.Find(numberInput);
                if (m != null)
                {
                    var numKP = m.CpNumber;

                    if (numKP != null)
                    {
                        ExternalRequests.ExternalRequestsToNAPZK(figs, fold, numKP);
                        MessageBox.Show($"Об'єднаний запит збережено за розташуванням {fold}\\Запити\\НАЗК\\Запит НАЗК.docx");
                        App.CurUser.LogInf(new CreateRequestData(App.CurUser.LoginName, TypeLogData.Access, inputNumberList.First(), $"Запит збережено за розташуванням {fold}\\Запити\\НАЗК\\Запит НАЗК.docx"));
                    }
                }

            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToBank)
            {
                var nameBank = stackPanelAddElse.Children[1] as ComboBox;

                var m = modelContext.Mains.Find(numberInput);
                if (m != null && nameBank != null)
                {
                    if (nameBank.SelectedIndex == -1)
                    {
                        MessageBox.Show("Оберіть назву банку");
                        return;
                    }

                    var itemBank = (from b1 in modelContext.DictBanks
                                    where b1.Nb == nameBank.Text
                                    select b1).ToList().First();
                    string? addr = "";
                    decimal? mfo = 0;


                    addr = $"{itemBank.Adress}, м. {itemBank.Np}, {GetStringPI(itemBank.Pi)}";
                    mfo = itemBank.Mfo;

                    var numKP = m.CpNumber;

                    if (numKP != null)
                    {
                        ExternalRequests.ExternalRequestsToBank(figs, fold, numKP, nameBank.Text, mfo + "", addr);
                        MessageBox.Show($"Об'єднаний запит збережено за розташуванням {fold}\\Запити\\Банки\\Запит {nameBank.Text.Replace('\"', ' ')} - ... .docx");
                        App.CurUser.LogInf(new CreateRequestData(App.CurUser.LoginName, TypeLogData.Access, inputNumberList.First(), $"Запит збережено за розташуванням {fold}\\Запити\\Банки\\Запит {nameBank.Text.Replace('\"', ' ')} - ... .docx"));

                    }
                }
            }
        }
        private List<decimal> GetNumInpSelFig()
        {
            List<decimal> list = new List<decimal>();
            for (int i = 0; i < stackPanelAdd.Children.Count; i++)
            {
                var ch = stackPanelAdd.Children[i] as CheckBox;
                if (ch != null)
                {
                    var chIsCh = ch.IsChecked;
                    if (chIsCh!=null)
                    {
                        if (chIsCh == true)
                        {
                            var  chT = Convert.ToDecimal(ch.Tag);
                            list.Add(chT);
                        }
                    }
                }
            }
            return list;
        }
        private async void GenerationFromTheSiteButtonClick(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                var listF = GetNumInpSelFig();

                List<Figurant> figs = new List<Figurant>();

                List<Figurant> items = new List<Figurant>();
                if (typeOfAppeal == TypeOfAppeal.NotСombined)
                {
                    items = (from f in modelContext.Figurants
                             where f.NumbInput == inputNumberList.First() && f.Status == 1
                             select f).ToList();

                    var mc = modelContext.MainConfigs.Find(inputNumberList.First());

                    if (mc != null)
                    {
                        string? fold = mc.Folder;

                        foreach (var item in items)
                        {
                            if (listF.IndexOf(item.Id) != -1)
                                figs.Add(item);
                        }
                        if (figs.Count == 0)
                            MessageBox.Show("Не вибрано жодного фігуранта!");
                        else
                        {
                            string res = "";
                            Dictionary<Figurant, List<string>> resFigUpr = new Dictionary<Figurant, List<string>>();
                            Dictionary<string, List<Figurant>> resStr = new Dictionary<string, List<Figurant>>();

                            foreach (var itemFig in figs)
                            {

                                var reqPars = await Req.Pars(itemFig);


                                if (reqPars != null)
                                {
                                    resFigUpr.Add(itemFig, reqPars);

                                    foreach (var itemReqPars in reqPars)
                                    {
                                        if (resStr.ContainsKey(itemReqPars))
                                        {
                                            resStr[itemReqPars].Add(itemFig);
                                        }
                                        else
                                        {
                                            resStr.Add(itemReqPars, new List<Figurant>());
                                            resStr[itemReqPars].Add(itemFig);
                                        }
                                        res += itemReqPars + "\n";
                                    }
                                    res += "*******************\n";
                                }

                            }
                            MessageBox.Show(res);

                            string strAnsw = "";

                            foreach (var item in resStr)
                            {
                                // is has convert db
                                var dictWorks = (from b in modelContext.DictWorks where b.Name == item.Key select b).ToList();

                                if (dictWorks != null)
                                {
                                    if (dictWorks.Count > 0)
                                    {
                                        var addr = dictWorks.First().Addr;
                                        var nameMian = dictWorks.First().NameMain;
                                        if (addr != null)
                                        {
                                            ExternalRequests.ExternalRequestsToDerzhpratsi(item.Value, fold, addr, nameMian);
                                            MessageBox.Show($"Запит збережено за розташуванням {fold}\\Запити\\Держпраці\\Відповідь {nameMian}.docx");
                                        }
                                        else
                                        {
                                            ExternalRequests.ExternalRequestsToDerzhpratsi(item.Value, fold, "", nameMian);
                                            MessageBox.Show($"Запит збережено за розташуванням {fold}\\Запити\\Держпраці\\Відповідь {nameMian}.docx");
                                        }
                                    }
                                    else
                                    {
                                        ExternalRequests.ExternalRequestsToDerzhpratsi(item.Value, fold, "", item.Key);
                                        strAnsw += item.Key + ";\n";
                                        MessageBox.Show($"Запит збережено за розташуванням {fold}\\Запити\\Держпраці\\Відповідь {item.Key}.docx");
                                    }
                                }
                                else
                                {
                                    ExternalRequests.ExternalRequestsToDerzhpratsi(item.Value, fold, "", item.Key);
                                    strAnsw += item.Key + ";\n";
                                    MessageBox.Show($"Запит збережено за розташуванням {fold}\\Запити\\Держпраці\\Відповідь {item.Key}.docx");
                                }
                            }
                            if (strAnsw != "")
                                MessageBox.Show("Не знайдено в базі даних органи:\n" + strAnsw + "\n\nВ запит вписано орган, отриманий зі сайту без адреси");

                            //exMeth(figs, fold);

                        }
                    }
                }
                else
                {
                    items = (from f in modelContext.Figurants
                             where inputNumberList.Contains(f.NumbInput) && f.Status == 1
                             select f).ToList();

                    foreach (var itemNI in inputNumberList)
                    {
                        var mc = modelContext.MainConfigs.Find(itemNI);

                        if (mc != null)
                        {
                            string? fold = mc.Folder;

                            foreach (var item in items)
                            {
                                if (listF.IndexOf(item.Id) != -1)
                                    figs.Add(item);
                            }
                            if (figs.Count == 0)
                                MessageBox.Show("Не вибрано жодного фігуранта!");
                            else
                            {
                                string res = "";
                                Dictionary<Figurant, List<string>> resFigUpr = new Dictionary<Figurant, List<string>>();
                                Dictionary<string, List<Figurant>> resStr = new Dictionary<string, List<Figurant>>();

                                foreach (var itemFig in figs)
                                {

                                    var reqPars = await Req.Pars(itemFig);


                                    if (reqPars != null)
                                    {
                                        resFigUpr.Add(itemFig, reqPars);

                                        foreach (var itemReqPars in reqPars)
                                        {
                                            if (resStr.ContainsKey(itemReqPars))
                                            {
                                                resStr[itemReqPars].Add(itemFig);
                                            }
                                            else
                                            {
                                                resStr.Add(itemReqPars, new List<Figurant>());
                                                resStr[itemReqPars].Add(itemFig);
                                            }
                                            res += itemReqPars + "\n";
                                        }
                                        res += "*******************\n";
                                    }

                                }
                                MessageBox.Show(res);

                                string strAnsw = "";

                                foreach (var item in resStr)
                                {
                                    // is has convert db
                                    var dictWorks = (from b in modelContext.DictWorks where b.Name == item.Key select b).ToList();

                                    if (dictWorks != null)
                                    {
                                        if (dictWorks.Count > 0)
                                        {
                                            var addr = dictWorks.First().Addr;
                                            var nameMian = dictWorks.First().NameMain;
                                            if (addr != null)
                                            {
                                                ExternalRequests.ExternalRequestsToDerzhpratsi(item.Value, fold, addr, nameMian);
                                                MessageBox.Show($"Запит збережено за розташуванням {fold}\\Запити\\Держпраці\\Відповідь {nameMian}.docx");
                                            }
                                            else
                                            {
                                                ExternalRequests.ExternalRequestsToDerzhpratsi(item.Value, fold, "", nameMian);
                                                MessageBox.Show($"Запит збережено за розташуванням {fold}\\Запити\\Держпраці\\Відповідь {nameMian}.docx");
                                            }
                                        }
                                        else
                                        {
                                            ExternalRequests.ExternalRequestsToDerzhpratsi(item.Value, fold, "", item.Key);
                                            strAnsw += item.Key + ";\n";
                                            MessageBox.Show($"Запит збережено за розташуванням {fold}\\Запити\\Держпраці\\Відповідь {item.Key}.docx");
                                        }
                                    }
                                    else
                                    {
                                        ExternalRequests.ExternalRequestsToDerzhpratsi(item.Value, fold, "", item.Key);
                                        strAnsw += item.Key + ";\n";
                                        MessageBox.Show($"Запит збережено за розташуванням {fold}\\Запити\\Держпраці\\Відповідь {item.Key}.docx");
                                    }
                                }
                                if (strAnsw != "")
                                    MessageBox.Show("Не знайдено в базі даних органи:\n" + strAnsw + "\n\nВ запит вписано орган, отриманий зі сайту без адреси");

                                //exMeth(figs, fold);

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Не вдалося згенерувати. Помилка:\n\n"+ex.Message);
            }
            inactivityTimer.Start();
        }
        private string GetStringPI(decimal? pi)
        {
            if(pi!= null)
            {
                var str = pi.ToString();
                if(str != null)
                {
                    while (str.Length < 5)
                    {
                        str = "0" + str;
                    }
                    return str;
                }
               
            }
            return "";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            inactivityTimer.Stop();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }

        private void Window_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }

        private void ComboBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }

        private void ComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }
    }
}
