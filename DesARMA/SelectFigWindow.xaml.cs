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
using static NPOI.HSSF.Util.HSSFColor;

namespace DesARMA
{
    /// <summary>
    /// Interaction logic for SelectFigWindow.xaml
    /// </summary>
    public partial class SelectFigWindow : Window
    {
        string inputNumber = "";
        ModelContext modelContext;
        EnumExtReq enumExtReq;
        public SelectFigWindow(string inputNumbet, ModelContext modelContext, EnumExtReq enumExtReq, string nameWin)
        {
            InitializeComponent();
            try
            {

            this.inputNumber = inputNumbet;
            this.modelContext = modelContext;
            this.enumExtReq = enumExtReq;

            this.Title += ". " + nameWin;

            var items = from f in modelContext.Figurants
                       where f.NumbInput == inputNumber && f.Status == false
                       select f;

            
            foreach (var item in items)
            {
                CheckBox check = new CheckBox();
                check.IsChecked = true;
                check.Margin = new Thickness(10);
                check.Padding = new Thickness(10, - 4, 0, 0);
                check.Background = this.Resources[$"3ColorStyle"] as SolidColorBrush;
                check.Tag = item.Id;

                Label label = new Label();
                label.Content = $"{item.Name} {item.Ipn}";
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

                var itemBankName =  stackPanelAddElse.Children[1] as ComboBox;
                if(itemBankName != null)
                {
                    List<string> listBankName = (from b in modelContext.DictBanks
                                select b.Nb).ToList();
                    listBankName.Sort();
                    itemBankName.ItemsSource = listBankName;
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
                else if(d != null)
                {
                    str += $"{d.Tag}\n";
                }
                else if(l != null)
                {
                    str += $"{l.Tag}\n";
                }
            }

            }
            catch (Exception exp1)
            {
                MessageBox.Show("" + exp1.Message);
            }
            //MessageBox.Show(str);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < stackPanelAdd.Children.Count; i++)
            {
                var ch = stackPanelAdd.Children[i] as CheckBox;
                if (ch != null)
                {
                    ch.IsChecked = chAll.IsChecked;
                }
            }
            
        }

        private void CreateReq(object sender, RoutedEventArgs e)
        {
            try
            {
                var listF = GetNumInpSelFig();
                var items = from f in modelContext.Figurants
                            where f.NumbInput == inputNumber && f.Status == false
                            select f;

                List<Figurant> figs = new List<Figurant>();

                
                var mc = modelContext.MainConfigs.Find(inputNumber);
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void exMeth(List<Figurant> figs, string? fold)
        {
            if(enumExtReq == EnumExtReq.ExternalRequestsToMytna)
            {
                ExternalRequests.ExternalRequestsToMytna(figs, fold);
                MessageBox.Show($"Збережено за розташуванням {fold}\\Запити\\Запит Держмитслужба");
            }
            else if(enumExtReq == EnumExtReq.ExternalRequestsToIntelektualnyi)
            {
                ExternalRequests.ExternalRequestsToIntelektualnyi(figs, fold);
                MessageBox.Show($"Збережено за розташуванням {fold}\\Запити\\Запит Укрпатент.docx");
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToHeolohii)
            {
                ExternalRequests.ExternalRequestsToHeolohii(figs, fold);
                MessageBox.Show($"Збережено за розташуванням {fold}\\Запити\\Запит Геонадра.docx");
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToDerzhpratsi)
            {
                //var addr = stackPanelAddElse.Children[1] as TextBox;
                //var name = stackPanelAddElse.Children[3] as TextBox;
                //if (addr != null && name!= null)
                //{
                //    ExternalRequests.ExternalRequestsToDerzhpratsi(figs, fold, addr.Text, name.Text);
                //    MessageBox.Show($"Збережено за розташуванням {fold}\\Запити\\Держпраці\\Відповідь {name.Text}.docx");
                //}
            }
            else if(enumExtReq == EnumExtReq.ExternalRequestsToAntymonopolnyi)
            {
                var m = modelContext.Mains.Find(inputNumber);
                if(m != null)
                {
                    var numKP = m.CpNumber;

                    if (numKP != null)
                    {
                        ExternalRequests.ExternalRequestsToAntymonopolnyi(figs, fold, numKP);
                        MessageBox.Show($"Збережено за розташуванням {fold}\\Запити\\АМК\\Запит АМК.docx");
                    }
                }
            }
            else if(enumExtReq == EnumExtReq.ExternalRequestsToFondovyi1)
            {
                ExternalRequests.ExternalRequestsToFondovyi1(figs, fold);
                MessageBox.Show($"Збережено за розташуванням {fold}\\Запити\\НКЦПФР 1\\Запит НКЦПФР.docx");
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToFondovyiOsnovnyi2)
            {
                ExternalRequests.ExternalRequestsToFondovyiOsnovnyi2(figs, fold);
                MessageBox.Show($"Збережено за розташуванням {fold}\\Запити\\НКЦПФР 2\\Запит НКЦПФР2.docx");
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToNAPZK)
            {
                var m = modelContext.Mains.Find(inputNumber);
                if (m != null)
                {
                    var numKP = m.CpNumber;

                    if (numKP != null)
                    {
                        ExternalRequests.ExternalRequestsToNAPZK(figs, fold, numKP);
                        MessageBox.Show($"Збережено за розташуванням {fold}\\Запити\\НАЗК\\Запит НАЗК.docx");
                    }
                }
                
            }
            else if (enumExtReq == EnumExtReq.ExternalRequestsToBank)
            {
                var nameBank = stackPanelAddElse.Children[1] as ComboBox;

                var m = modelContext.Mains.Find(inputNumber);
                if (m != null && nameBank != null)
                {
                    if(nameBank.SelectedIndex == -1)
                    {
                        MessageBox.Show("Оберіть назву банку");
                        return;
                    }

                    var itemBank = (from b1 in modelContext.DictBanks
                                    where b1.Nb == nameBank.Text
                                    select b1).ToList();
                    string? addr = "";
                    decimal? mfo = 0;
                    foreach (var item in itemBank)
                    {
                        addr = $"{item.Adress}, м. {item.Np}, {GetStringPI(item.Pi)}";
                        mfo = item.Mfo;
                    }

                    var numKP = m.CpNumber;

                    if (numKP != null)
                    {
                        ExternalRequests.ExternalRequestsToBank(figs, fold,  numKP, nameBank.Text, mfo + "", addr);
                        MessageBox.Show($"Збережено за розташуванням {fold}\\Запити\\Банки\\Запит {nameBank.Text.Replace('\"', ' ')} - ... .docx");

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
        private void GenerationFromTheSiteButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var listF = GetNumInpSelFig();
                var items = from f in modelContext.Figurants
                            where f.NumbInput == inputNumber && f.Status == false
                            select f;

                List<Figurant> figs = new List<Figurant>();


                var mc = modelContext.MainConfigs.Find(inputNumber);
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
                            var reqPars = Req.Pars(itemFig);
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
                            res +=  "*******************\n";
                        }
                        MessageBox.Show(res);

                        foreach (var item in resStr)
                        {
                            //TODO adress from DB
                            ExternalRequests.ExternalRequestsToDerzhpratsi(item.Value, fold, "адреса по фрейду", item.Key);
                            MessageBox.Show($"Збережено за розташуванням {fold}\\Запити\\Держпраці\\Відповідь {item.Key}.docx");

                        }

                        //exMeth(figs, fold);

                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Не вдалося згенерувати. Помилка:\n\n"+ex.Message);
            }
            
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
    }
}
