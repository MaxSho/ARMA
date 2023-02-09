using DesARMA.Models;
using NPOI.SS.Formula.Functions;
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
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DesARMA.CombinedResponseWindows
{
    /// <summary>
    /// Interaction logic for EntryOfPersonsInvolvedInTheCombinedRegistersWindow.xaml
    /// </summary>
    /// 
    enum CheckEnum
    {
        Control,
        Shema,
        Yes,
        No
    }
    public partial class EntryOfPersonsInvolvedInTheCombinedRegistersWindow : System.Windows.Window
    {
        List<string> listNumIn;
        public List<int> numbColorInReestr;
        ModelContext modelContext;
        Main main;
        public List<Figurant> figurants;
        public EntryOfPersonsInvolvedInTheCombinedRegistersWindow(ModelContext modelContext, Main main, List<string> listNumIn)
        {
            InitializeComponent();

            this.listNumIn = listNumIn;
            this.modelContext = modelContext;
            this.main = main;
            figurants = (from f in modelContext.Figurants where listNumIn.Contains(f.NumbInput) select f).ToList();

            ToCheckFolders();
            CreateTreeView1();
            SetColor();

        }
        private void SetColor()
        {
            for (int i = 1; i < treeView1.Items.Count; i++)
            {
                var st = treeView1.Items[i] as StackPanel;
                if(st != null)
                {
                    var ti = st.Children[2] as TreeViewItem;
                    if(ti != null)
                    {
                        var tiH = ti.Header as TextBlock;
                        if(tiH != null)
                        {
                            if (numbColorInReestr[i - 1] == 3)
                            {
                                tiH.Foreground = this.Resources["GreenEmpty"] as SolidColorBrush; ;
                            }
                            else if (numbColorInReestr[i - 1] == 2)
                            {
                                tiH.Foreground = this.Resources["4ColorStyle"] as SolidColorBrush;// new SolidColorBrush(Colors.White);
                            }
                            else if (numbColorInReestr[i - 1] == 1)
                            {
                                tiH.Foreground = this.Resources["RedEmpty"] as SolidColorBrush; //new SolidColorBrush(Colors.Red);
                            }
                        }
                        foreach (var itemF in ti.Items)
                        {
                            var stF = itemF as StackPanel;
                            if(stF != null)
                            {
                                foreach (var itemTB in stF.Children)
                                {
                                    var cutTB = itemTB as TextBlock;
                                    if(cutTB != null)
                                    {
                                        if (numbColorInReestr[i - 1] == 3)
                                        {
                                            cutTB.Foreground = this.Resources["GreenEmpty"] as SolidColorBrush; ;
                                        }
                                        else if (numbColorInReestr[i - 1] == 2)
                                        {
                                            cutTB.Foreground = this.Resources["4ColorStyle"] as SolidColorBrush;// new SolidColorBrush(Colors.White);
                                        }
                                        else if (numbColorInReestr[i - 1] == 1)
                                        {
                                            cutTB.Foreground = this.Resources["RedEmpty"] as SolidColorBrush; //new SolidColorBrush(Colors.Red);
                                        }
                                    }
                                }
                            }
                        }
                    } 
                }

            }
        }
        private void CreateTreeView1()
        {
            treeView1.Items.Add($"Контроль/Схема");


            for (int i = 0; i < Reest.abbreviatedName.Count; i++)
            {
                treeView1.Items.Add(CreateItem(i + 1));
            }
        }
        private void ToCheckFolders()
        {
            List<int> numbColorInReestr = new List<int>();

            foreach (var item in Reest.abbreviatedName)
            {
                numbColorInReestr.Add(0);
            }


            foreach (var itemNumbIn in listNumIn)
            {
                var folderCurrentNumbIn = (from f in modelContext.MainConfigs where f.NumbInput == itemNumbIn select f.Folder).First();
                if(folderCurrentNumbIn != null)
                {
                    if (Directory.Exists(folderCurrentNumbIn))
                    {
                        for (int i = 0; i < Reest.abbreviatedName.Count; i++)
                        {
                            if (Directory.Exists($"{folderCurrentNumbIn}\\{i + 1}. {Reest.abbreviatedName[i]}"))
                            {
                                if (Directory.GetFiles($"{folderCurrentNumbIn}\\{i + 1}. {Reest.abbreviatedName[i]}").Length==0
                                    && Directory.GetDirectories($"{folderCurrentNumbIn}\\{i + 1}. {Reest.abbreviatedName[i]}").Length 
                                    == 0)
                                {
                                    numbColorInReestr[i] = Math.Max(numbColorInReestr[i], 2);
                                }
                                else
                                {
                                    numbColorInReestr[i] = Math.Max(numbColorInReestr[i], 3);
                                }
                            }
                            else
                            {
                                numbColorInReestr[i] = Math.Max(numbColorInReestr[i], 1);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Відсутня папка запиту за розташуванням {folderCurrentNumbIn}");
                    }
                }
                else
                {
                    MessageBox.Show($"В базі даних не знайдено шлях до папки в запиті {itemNumbIn}");
                }
            }
            this.numbColorInReestr = numbColorInReestr;
        }

        private StackPanel CreateItem(int num)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.Margin = new Thickness(0, 10, 0, 0);

            stackPanel.Children.Add(CreateCheckBox(CheckEnum.Control, num));
            stackPanel.Children.Add(CreateCheckBox(CheckEnum.Shema, num));
            stackPanel.Children.Add(CreateTreeViewItem(num));

            return stackPanel;
        }
        private CheckBox CreateCheckBox(CheckEnum checkEnum, int num)
        {
            CheckBox checkBox = new CheckBox();
            checkBox.Margin = new Thickness(5, 0, 0, 0);
            checkBox.Tag = new Tuple<CheckEnum, int>(checkEnum, num);
            checkBox.Click += CheckBoxClick;
            return checkBox;   
        }
        private List<UIElement> CreateUIElementFigurant(Figurant figurant)
        {
            List<UIElement> listRet = new List<UIElement>();
            string str = GetDefInString(figurant);

            if (str != null)
            {
                var list = str!.Split('^');
                if (list.Length == 6)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = list[0];
                    textBlock.FontSize = 14;
                    textBlock.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock.Text); } catch (Exception ex) { } };
                    textBlock.MouseEnter += (w, r) => { textBlock.Opacity = 0.5; };
                    textBlock.MouseLeave += (w, r) => { textBlock.Opacity = 1; };
                    textBlock.Padding = new Thickness(20, 0, 0, 0);
                    textBlock.ToolTip = "Натисніть, щоб скопіювати";
                    listRet.Add(textBlock);


                    TextBlock textBlockTemp = new TextBlock();
                    textBlockTemp.Text = list[1];
                    textBlockTemp.FontSize = 14;
                    listRet.Add(textBlockTemp);



                    TextBlock textBlock1 = new TextBlock();
                    textBlock1.Text = list[2];
                    textBlock1.FontSize = 14;
                    textBlock1.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock1.Text); } catch (Exception ex) { } };
                    textBlock1.MouseEnter += (w, r) => { textBlock1.Opacity = 0.5; };
                    textBlock1.MouseLeave += (w, r) => { textBlock1.Opacity = 1; };
                    textBlock1.ToolTip = "Натисніть, щоб скопіювати";
                    listRet.Add(textBlock1);

                    TextBlock textBlockTemp2 = new TextBlock();
                    textBlockTemp2.Text = list[3];
                    textBlockTemp2.FontSize = 14;
                    listRet.Add(textBlockTemp2);

                    TextBlock textBlockTemp3 = new TextBlock();
                    textBlockTemp3.Text = list[4];
                    textBlockTemp3.FontSize = 14;
                    listRet.Add(textBlockTemp3);

                    TextBlock textBlock2 = new TextBlock();
                    textBlock2.Text = list[5];
                    textBlock2.FontSize = 14;
                    textBlock2.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock2.Text); } catch (Exception ex) { } };
                    textBlock2.MouseEnter += (w, r) => { textBlock2.Opacity = 0.5; };
                    textBlock2.MouseLeave += (w, r) => { textBlock2.Opacity = 1; };
                    textBlock2.ToolTip = "Натисніть, щоб скопіювати";
                    listRet.Add(textBlock2);
                }
                else if (list.Length == 4)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = list[0];
                    textBlock.FontSize = 14;
                    textBlock.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock.Text); } catch (Exception ex) { } };
                    textBlock.MouseEnter += (w, r) => { textBlock.Opacity = 0.5; };
                    textBlock.MouseLeave += (w, r) => { textBlock.Opacity = 1; };
                    textBlock.Padding = new Thickness(20, 0, 0, 0);
                    textBlock.ToolTip = "Натисніть, щоб скопіювати";
                    listRet.Add(textBlock);


                    TextBlock textBlockTemp = new TextBlock();
                    textBlockTemp.Text = list[1];
                    textBlockTemp.FontSize = 14;
                    listRet.Add(textBlockTemp);



                    TextBlock textBlock1 = new TextBlock();
                    textBlock1.Text = list[2];
                    textBlock1.FontSize = 14;
                    textBlock1.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock1.Text); } catch (Exception ex) { } };
                    textBlock1.MouseEnter += (w, r) => { textBlock1.Opacity = 0.5; };
                    textBlock1.MouseLeave += (w, r) => { textBlock1.Opacity = 1; };
                    textBlock1.ToolTip = "Натисніть, щоб скопіювати";
                    listRet.Add(textBlock1);

                    TextBlock textBlockTemp2 = new TextBlock();
                    textBlockTemp2.Text = list[3];
                    textBlockTemp2.FontSize = 14;
                    listRet.Add(textBlockTemp2);
                }
                else if (list.Length == 3)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = list[0];
                    textBlock.FontSize = 14;
                    textBlock.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock.Text); } catch (Exception ex) { } };
                    textBlock.MouseEnter += (w, r) => { textBlock.Opacity = 0.5; };
                    textBlock.MouseLeave += (w, r) => { textBlock.Opacity = 1; };
                    textBlock.Padding = new Thickness(20, 0, 0, 0);
                    textBlock.ToolTip = "Натисніть, щоб скопіювати";
                    listRet.Add(textBlock);


                    TextBlock textBlockTemp = new TextBlock();
                    textBlockTemp.Text = list[1];
                    textBlockTemp.FontSize = 14;
                    listRet.Add(textBlockTemp);



                    TextBlock textBlock1 = new TextBlock();
                    textBlock1.Text = list[2];
                    textBlock1.FontSize = 14;
                    textBlock1.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock1.Text); } catch (Exception ex) { } };
                    textBlock1.MouseEnter += (w, r) => { textBlock1.Opacity = 0.5; };
                    textBlock1.MouseLeave += (w, r) => { textBlock1.Opacity = 1; };
                    textBlock1.ToolTip = "Натисніть, щоб скопіювати";
                    listRet.Add(textBlock1);
                }
                else if (list.Length == 1)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = list[0];
                    textBlock.FontSize = 14;
                    textBlock.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock.Text); } catch (Exception ex) { } };
                    textBlock.MouseEnter += (w, r) => { textBlock.Opacity = 0.5; };
                    textBlock.MouseLeave += (w, r) => { textBlock.Opacity = 1; };
                    textBlock.Padding = new Thickness(20, 0, 0, 0);
                    textBlock.ToolTip = "Натисніть, щоб скопіювати";
                    listRet.Add(textBlock);
                }
            }
            return listRet;
        }
        private TextBlock CreateTextBlockFigurant(Figurant figurant)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Margin = new Thickness(10, -2, 0, 0);
            textBlock.Text = GetDefInString(figurant);
            return textBlock;
        }
        private TextBlock CreateTextBlockReest(int num)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Margin = new Thickness(10, 0, 0, 0);
            textBlock.Text = $"{num}. {Reest.abbreviatedName[num - 1]}";
            return textBlock;
        }
        private StackPanel CreateStackPanelFigurants(Figurant figurant, int num)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;

            
            stackPanel.Children.Add(CreateCheckBox(CheckEnum.Yes, num));
            stackPanel.Children.Add(CreateCheckBox(CheckEnum.No, num));
            
            foreach (var item in CreateUIElementFigurant(figurant))
            {
                stackPanel.Children.Add(item);
            }
            return stackPanel;
        }
        private TreeViewItem CreateTreeViewItem(int num)
        {
            TreeViewItem treeViewItem = new TreeViewItem();

            treeViewItem.Margin = new Thickness(0, -2, 0, 0);
            treeViewItem.Header = CreateTextBlockReest(num);

            treeViewItem.Items.Add(CreateTextBoxYesNo());
            foreach (var itemF in figurants)
            {
                treeViewItem.Items.Add(CreateStackPanelFigurants(itemF, num));
            }
            return treeViewItem;
        }
        private TextBlock CreateTextBoxYesNo()
        {
            TextBlock textBlockYesNo = new TextBlock();
            textBlockYesNo.Text = "так   ні";
            textBlockYesNo.FontSize = 11;
            textBlockYesNo.Margin = new Thickness(5, 0, 0, 0);
            return textBlockYesNo;
        }
        private void CheckBoxClick(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if(checkBox != null)
            {
                if(checkBox.Tag != null)
                {
                    var checkTuple = checkBox.Tag as Tuple<CheckEnum, int>;
                    if(checkTuple != null)
                    {
                        if (checkTuple.Item1 == CheckEnum.Control)
                        {
                            //checkBox.IsChecked = false;
                            IfhaveTwoCheck(checkTuple.Item2, CheckEnum.Control);
                        }
                        else if (checkTuple.Item1 == CheckEnum.Shema)
                        {
                            if (numbColorInReestr[checkTuple.Item2 - 1] == 1)
                            {
                                checkBox.IsChecked = false;
                            }
                        }
                        else if (checkTuple.Item1 == CheckEnum.Yes)
                        {
                            IfhaveTwoCheck(checkTuple.Item2, CheckEnum.Yes);
                        }
                        else if (checkTuple.Item1 == CheckEnum.No)
                        {
                            IfhaveTwoCheck(checkTuple.Item2, CheckEnum.No);
                        }
                    }
                }
            }
        }
        void IfhaveTwoCheck(int num, CheckEnum checkEnum)
        {
            var st = treeView1.Items[num] as StackPanel;
            if (st != null)
            {
                var chC = st.Children[0] as CheckBox;
                var chS = st.Children[1] as CheckBox;
                var tri = st.Children[2] as TreeViewItem;
                if(tri != null && chC != null)
                {
                    var tiH = tri.Header as TextBlock;
                    if (tiH != null)
                    {
                        if (numbColorInReestr[num - 1] == 3)
                        {
                            tiH.Foreground = this.Resources["GreenEmpty"] as SolidColorBrush;
                        }
                        else if (numbColorInReestr[num - 1] == 2)
                        {
                            tiH.Foreground = this.Resources["4ColorStyle"] as SolidColorBrush;  // new SolidColorBrush(Colors.White);
                        }
                        else if (numbColorInReestr[num - 1] == 1)
                        {
                            tiH.Foreground = this.Resources["RedEmpty"] as SolidColorBrush;     //new SolidColorBrush(Colors.Red);
                        }
                    }
                    var isAllCh = true;
                    foreach (var itemF in tri.Items)
                    {
                        var stF = itemF as StackPanel;
                        if (stF != null)
                        {
                            var chYes = stF.Children[0] as CheckBox;
                            var chNo = stF.Children[1] as CheckBox;
                            if(chYes.IsChecked.Value && chNo.IsChecked.Value)
                            {
                                if (checkEnum == CheckEnum.Yes)
                                {
                                    chNo.IsChecked  = false;
                                }
                                else if (checkEnum == CheckEnum.No)
                                {
                                    chYes.IsChecked = false;
                                }
                            }

                            if(!chNo.IsChecked.Value && !chYes.IsChecked.Value)
                            {
                                foreach (var itemTB in stF.Children)
                                {
                                    var cutTB = itemTB as TextBlock;
                                    if (cutTB != null)
                                    {
                                        if (numbColorInReestr[num - 1] == 3)
                                        {
                                            cutTB.Foreground = this.Resources["GreenEmpty"]  as SolidColorBrush;
                                        }
                                        else if (numbColorInReestr[num - 1] == 2)
                                        {
                                            cutTB.Foreground = this.Resources["4ColorStyle"] as SolidColorBrush; //new SolidColorBrush(Colors.White);
                                        }
                                        else if (numbColorInReestr[num - 1] == 1)
                                        {

                                            cutTB.Foreground = this.Resources["RedEmpty"]    as SolidColorBrush; //new SolidColorBrush(Colors.Red);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if(numbColorInReestr[num - 1] != 1)
                                {
                                    foreach (var itemTB in stF.Children)
                                    {
                                        var cutTB = itemTB as TextBlock;
                                        if (cutTB != null)
                                        {
                                            cutTB.Foreground = this.Resources["1ColorStyle"] as SolidColorBrush;
                                        }
                                    }
                                }
                                else
                                {
                                    chNo.IsChecked = false;
                                    chYes.IsChecked = false;
                                }
                                
                            }
                            
                            isAllCh = isAllCh && (chYes.IsChecked.Value || chNo.IsChecked.Value);
                        }
                    }
                    
                    
                    chC.IsChecked = isAllCh;
                }
            }
        }
        void update()
        {
            List<bool> listIsExpanded = new List<bool>();
            foreach (var item in treeView1.Items)
            {
                var st = item as StackPanel;
                if(st != null)
                {
                    var tri = st.Children[2] as TreeViewItem;
                    if(tri != null)
                    {
                        listIsExpanded.Add(tri.IsExpanded);

                    }
                }
            }


            foreach (var item in treeView1.Items)
            {
                var st = item as StackPanel;
                if (st != null)
                {
                    var chC = st.Children[0] as CheckBox;
                    var chS = st.Children[1] as CheckBox;
                    var tri = st.Children[2] as TreeViewItem;
                    if(chC != null && chS != null && tri != null)
                    {
                        foreach (var itemF in tri.Items)
                        {
                            var stF = itemF as StackPanel;
                            if (stF != null)
                            {
                                var chYes = stF.Children[0] as CheckBox;
                                var chNo = stF.Children[1] as CheckBox;
                                if(chYes!=null && chNo != null)
                                {
                                    if (chYes.IsChecked.Value || chNo.IsChecked.Value)
                                    {
                                        chC.IsChecked = true;
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }
        static public string? GetDefInString(Figurant d)
        {
            if (d.ResFiz != null)
            {
                string strDt = "";
                var birth = d.DtBirth;
                if (birth != null)
                {
                    return $"{d.Fio}^, ^{birth.ToString().Substring(0, 10)}^ р.н.^, РНОКПП ^{d.Ipn}";
                }
                return $"{d.Fio}^, РНОКПП ^{d.Ipn}";
            }
            else
            {
                if (d.Code == null || d.Code == "")
                    return $"{d.Name}";
                return $"{d.Name}^ (ЄДРПОУ ^{d.Code}^)";
            }
        }
        private bool? GetCheckBoxControl(int num)
        {
            var st = treeView1.Items[num - 1] as StackPanel;
            if (st != null)
            {
                var chC = st.Children[0] as CheckBox;
                //var chS = st.Children[1] as CheckBox;
                return chC.IsChecked.Value;
            }
            return null;
        }
        private bool? GetCheckBoxShema(int num)
        {
            var st = treeView1.Items[num - 1] as StackPanel;
            if (st != null)
            {
                //var chC = st.Children[0] as CheckBox;
                var chS = st.Children[1] as CheckBox;
                return chS.IsChecked.Value;
            }
            return null;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var listNoCont = GetListNoControl();
            if (listNoCont.Count==0)
                this.DialogResult = true;
            else
            {
                string strReest = "";
                foreach (var item in listNoCont)
                {
                    strReest += item + "\n";
                }
                MessageBox.Show("Не відмічено контроль в таких реєстрах:\n" + strReest);
            }
        }
        private List<string> GetListNoControl()
        {
            List<string> listNoContr = new List<string>();
            for (int i = 1; i < treeView1.Items.Count; i++)
            {
                if (numbColorInReestr[i-1] != 1)
                {
                    var st = treeView1.Items[i] as StackPanel;
                    if(st != null)
                    {
                        var chC = st.Children[0] as CheckBox;
                        if (!chC!.IsChecked!.Value)
                        {
                            listNoContr.Add(i + ". " +Reest.abbreviatedName[i - 1]);
                        }
                    }
                }
            }
            return listNoContr;
        }
    }
}
