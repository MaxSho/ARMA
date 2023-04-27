﻿using DesARMA.Models;
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
    enum StatusFolder
    {
        Undefined,
        NotCreated,
        Empty,
        NotEmpty
    }
    public partial class EntryOfPersonsInvolvedInTheCombinedRegistersWindow : System.Windows.Window
    {
        List<string> listNumIn;
        public List<int> numbColorInReestr;
        public int numbColorShema;
        readonly ModelContext modelContext;
        readonly Main main;
        public List<Figurant> figurants;
        private readonly System.Windows.Forms.Timer inactivityTimer = new System.Windows.Forms.Timer();
        public EntryOfPersonsInvolvedInTheCombinedRegistersWindow(ModelContext modelContext, Main main, List<string> listNumIn,
            System.Windows.Forms.Timer inactivityTimer)
        {
            try
            {
                InitializeComponent();

                this.listNumIn = listNumIn;
                this.modelContext = modelContext;
                this.main = main;
                this.inactivityTimer = inactivityTimer;

                inactivityTimer.Start();

                figurants = (from f in modelContext.Figurants where listNumIn.Contains(f.NumbInput) select f).ToList();

                InitField();

                //ToCheckFolders();
                //ToCheckFoldersShema();

                CreateTreeView1();
                CreateTreeViewShema();

                //SetColor();
                //SetColorShema();

                LoadCheckBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadCheckBox()
        {
            for (int i = 1; i <= figurants.Count; i++)
            {
                var yesCh = figurants[i - 1].Control;
                var noCh = figurants[i - 1].Shema;

                var listYes = GetChFromString(yesCh);
                var listNo = GetChFromString(noCh);


                for (int j = 0; j < Reest.abbreviatedName.Count; j++)
                {
                    var yes = GetFigCheckYesReestr(j + 1, i);
                    var no = GetFigCheckNoReestr(j + 1, i);

                    if(yes!= null && no!= null && j < listYes.Count && j < listNo.Count)
                    {
                        yes.IsChecked = listYes[j];
                        no.IsChecked = listNo[j];

                        CheckBoxClick(yes, null);
                        //IfhaveTwoCheck(j + 1, CheckEnum.Yes);
                    }
                }

                var yesS = GetFigCheckYesReestr(Reest.abbreviatedName.Count + 1, i);
                var noS = GetFigCheckNoReestr(Reest.abbreviatedName.Count + 1, i);

                if (yesS != null && noS != null && Reest.abbreviatedName.Count < listYes.Count && Reest.abbreviatedName.Count < listNo.Count)
                {
                    yesS.IsChecked = listYes[Reest.abbreviatedName.Count];
                    noS.IsChecked = listNo[Reest.abbreviatedName.Count];

                    CheckBoxClickShema(yesS, null);
                    //IfhaveTwoCheck(j + 1, CheckEnum.Yes);
                }
            }
        }
        private void InitField()
        {
            numbColorInReestr = new List<int>();

            foreach (var item in Reest.abbreviatedName)
            {
                numbColorInReestr.Add(0);
            }

            numbColorShema = 0;
        }
        private void SetColor()
        {
            for (int i = 1; i < treeView1.Items.Count; i++)
            {
                if(treeView1.Items[i] is StackPanel st)
                {
                    if(st.Children[2] is TreeViewItem ti)
                    {
                        if(ti.Header is TextBlock tiH)
                        {
                            if (numbColorInReestr[i - 1] == 3)
                            {
                                tiH.Foreground = this.Resources["GreenEmpty"] as SolidColorBrush; ;
                            }
                            else if (numbColorInReestr[i - 1] == 2)
                            {
                                tiH.Foreground = this.Resources["4ColorStyle"] as SolidColorBrush; // new SolidColorBrush(Colors.White);
                            }
                            else if (numbColorInReestr[i - 1] == 1)
                            {
                                tiH.Foreground = this.Resources["RedEmpty"] as SolidColorBrush; //new SolidColorBrush(Colors.Red);
                            }
                        }
                        foreach (var itemF in ti.Items)
                        {
                            if(itemF is StackPanel stF)
                            {
                                foreach (var itemTB in stF.Children)
                                {
                                    if(itemTB is TextBlock cutTB)
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
        //private void SetColorShema()
        //{

        //    var st = treeViewShema.Items[0] as StackPanel;
        //    if (st != null)
        //    {
        //        var ti = st.Children[2] as TreeViewItem;
        //        if (ti != null)
        //        {
        //            var tiH = ti.Header as TextBlock;
        //            if (tiH != null)
        //            {
        //                if (numbColorShema == 3)
        //                {
        //                    tiH.Foreground = this.Resources["GreenEmpty"] as SolidColorBrush; ;
        //                }
        //                else if (numbColorShema == 2)
        //                {
        //                    tiH.Foreground = this.Resources["4ColorStyle"] as SolidColorBrush;// new SolidColorBrush(Colors.White);
        //                }
        //                else if (numbColorShema == 1)
        //                {
        //                    tiH.Foreground = this.Resources["RedEmpty"] as SolidColorBrush; //new SolidColorBrush(Colors.Red);
        //                }
        //            }
        //            foreach (var itemF in ti.Items)
        //            {
        //                var stF = itemF as StackPanel;
        //                if (stF != null)
        //                {
        //                    foreach (var itemTB in stF.Children)
        //                    {
        //                        var cutTB = itemTB as TextBlock;
        //                        if (cutTB != null)
        //                        {
        //                            if (numbColorShema == 3)
        //                            {
        //                                cutTB.Foreground = this.Resources["GreenEmpty"] as SolidColorBrush; ;
        //                            }
        //                            else if (numbColorShema == 2)
        //                            {
        //                                cutTB.Foreground = this.Resources["4ColorStyle"] as SolidColorBrush;// new SolidColorBrush(Colors.White);
        //                            }
        //                            else if (numbColorShema == 1)
        //                            {
        //                                cutTB.Foreground = this.Resources["RedEmpty"] as SolidColorBrush; //new SolidColorBrush(Colors.Red);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        private void CreateTreeView1()
        {
            treeView1.Items.Add($"Контроль/Схема");


            for (int i = 0; i < Reest.abbreviatedName.Count; i++)
            {
                treeView1.Items.Add(CreateItem(i + 1));
            }
        }
        private void CreateTreeViewShema()
        {
            treeViewShema.Items.Add(CreateItemShema(Reest.abbreviatedName.Count + 1));
            
        }
        private void ToCheckFolders()
        {
            //List<int> numbColorInReestr = new List<int>();

            //foreach (var item in Reest.abbreviatedName)
            //{
            //    numbColorInReestr.Add(0);
            //}


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
                                    && 
                                    Directory.GetDirectories($"{folderCurrentNumbIn}\\{i + 1}. {Reest.abbreviatedName[i]}").Length == 0)
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
        private void ToCheckFoldersShema()
        {
            int curNumCol = 0;
            foreach (var itemNumbIn in listNumIn)
            {
                var folderCurrentNumbIn = (from f in modelContext.MainConfigs where f.NumbInput == itemNumbIn select f.Folder).First();
                if (folderCurrentNumbIn != null)
                {
                    if (Directory.Exists(folderCurrentNumbIn))
                    {
                        if(Directory.Exists($"{folderCurrentNumbIn}\\{Reest.abbreviatedName.Count + 1}. Схеми"))
                        {
                            if (Directory.GetDirectories($"{folderCurrentNumbIn}\\{Reest.abbreviatedName.Count + 1}. Схеми").Length == 0
                                && Directory.GetFiles($"{folderCurrentNumbIn}\\{Reest.abbreviatedName.Count + 1}. Схеми").Length == 0)
                            {
                                curNumCol = Math.Max(curNumCol, 2);
                            }
                            else
                            {
                                curNumCol = Math.Max(curNumCol, 3);
                            }
                        }
                        else
                        {
                            curNumCol = Math.Max(curNumCol, 1);
                        }
                    }
                }
            }

                       
            this.numbColorShema = curNumCol;
        }
        private StackPanel CreateItem(int num)
        {
            StackPanel stackPanel = new()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 10, 0, 0)
            };

            stackPanel.Children.Add(CreateCheckBox(CheckEnum.Control, num));
            stackPanel.Children.Add(CreateCheckBox(CheckEnum.Shema, num));
            stackPanel.Children.Add(CreateTreeViewItem(num));

            return stackPanel;
        }
        private StackPanel CreateItemShema(int num)
        {
            StackPanel stackPanel = new()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 10, 0, 0)
            };
            
            stackPanel.Children.Add(CreateCheckBoxShema(CheckEnum.Control, num));
            stackPanel.Children.Add(CreateCheckBoxShema(CheckEnum.Shema, num));
            stackPanel.Children.Add(CreateTreeViewItemShema(num));

            return stackPanel;
        }
        private CheckBox CreateCheckBox(CheckEnum checkEnum, int num)
        {
            CheckBox checkBox = new()
            {
                Margin = new Thickness(5, 0, 0, 0),
                Tag = new Tuple<CheckEnum, int>(checkEnum, num)
            };
            checkBox.Click += CheckBoxClick;
            return checkBox;   
        }
        private CheckBox CreateCheckBoxShema(CheckEnum checkEnum, int num)
        {
            CheckBox checkBox = new()
            {
                Margin = new Thickness(5, 0, 0, 0),
                Tag = new Tuple<CheckEnum, int>(checkEnum, num)
            };
            checkBox.Click += CheckBoxClickShema;
            return checkBox;
        }
        private static List<UIElement> CreateUIElementFigurant(Figurant figurant)
        {
            List<UIElement> listRet = new List<UIElement>();
            string str = GetDefInString(figurant);

            if (str != null)
            {
                var list = str!.Split('^');
                if (list.Length == 6)
                {
                    TextBlock textBlock = new()
                    {
                        Text = list[0],
                        FontSize = 14,
                        Padding = new Thickness(20, 0, 0, 0),
                        ToolTip = "Натисніть, щоб скопіювати"
                    };
                    textBlock.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock.Text); } catch (Exception ex) { } };
                    textBlock.MouseEnter += (w, r) => { textBlock.Opacity = 0.5; };
                    textBlock.MouseLeave += (w, r) => { textBlock.Opacity = 1; };
                    listRet.Add(textBlock);


                    TextBlock textBlockTemp = new()
                    {
                        Text = list[1],
                        FontSize = 14
                    };
                    listRet.Add(textBlockTemp);



                    TextBlock textBlock1 = new()
                    {
                        Text = list[2],
                        FontSize = 14,
                        ToolTip = "Натисніть, щоб скопіювати"
                    };
                    textBlock1.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock1.Text); } catch (Exception ex) { } };
                    textBlock1.MouseEnter += (w, r) => { textBlock1.Opacity = 0.5; };
                    textBlock1.MouseLeave += (w, r) => { textBlock1.Opacity = 1; };
                    listRet.Add(textBlock1);

                    TextBlock textBlockTemp2 = new()
                    {
                        Text = list[3],
                        FontSize = 14
                    };
                    textBlockTemp2.Text = list[3];
                    textBlockTemp2.FontSize = 14;
                    listRet.Add(textBlockTemp2);

                    TextBlock textBlockTemp3 = new()
                    {
                        Text = list[4],
                        FontSize = 14
                    };
                    textBlockTemp3.Text = list[4];
                    textBlockTemp3.FontSize = 14;
                    listRet.Add(textBlockTemp3);

                    TextBlock textBlock2 = new()
                    {
                        Text = list[5],
                        FontSize = 14,
                        ToolTip = "Натисніть, щоб скопіювати"
                    };
                    textBlock2.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock2.Text); } catch (Exception ex) { } };
                    textBlock2.MouseEnter += (w, r) => { textBlock2.Opacity = 0.5; };
                    textBlock2.MouseLeave += (w, r) => { textBlock2.Opacity = 1; };
                    listRet.Add(textBlock2);
                }
                else if (list.Length == 4)
                {
                    TextBlock textBlock = new()
                    {
                        Text = list[0],
                        FontSize = 14,
                        Padding = new Thickness(20, 0, 0, 0),
                        ToolTip = "Натисніть, щоб скопіювати"
                    };
                    textBlock.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock.Text); } catch (Exception ex) { } };
                    textBlock.MouseEnter += (w, r) => { textBlock.Opacity = 0.5; };
                    textBlock.MouseLeave += (w, r) => { textBlock.Opacity = 1; };
                    listRet.Add(textBlock);


                    TextBlock textBlockTemp = new()
                    {
                        Text = list[1],
                        FontSize = 14
                    };

                    textBlockTemp.Text = list[1];
                    textBlockTemp.FontSize = 14;
                    listRet.Add(textBlockTemp);



                    TextBlock textBlock1 = new()
                    {
                        Text = list[2],
                        FontSize = 14,
                        ToolTip = "Натисніть, щоб скопіювати"
                    };
                    textBlock1.Text = list[2];
                    textBlock1.FontSize = 14;
                    textBlock1.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock1.Text); } catch (Exception ex) { } };
                    textBlock1.MouseEnter += (w, r) => { textBlock1.Opacity = 0.5; };
                    textBlock1.MouseLeave += (w, r) => { textBlock1.Opacity = 1; };
                    textBlock1.ToolTip = "Натисніть, щоб скопіювати";
                    listRet.Add(textBlock1);

                    TextBlock textBlockTemp2 = new()
                    {
                        Text = list[0],
                        FontSize = 14
                    };
                    textBlockTemp2.Text = list[3];
                    textBlockTemp2.FontSize = 14;
                    listRet.Add(textBlockTemp2);
                }
                else if (list.Length == 3)
                {
                    TextBlock textBlock = new()
                    {
                        Text = list[0],
                        FontSize = 14,
                        Padding = new Thickness(20, 0, 0, 0),
                        ToolTip = "Натисніть, щоб скопіювати"
                    };

                    textBlock.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock.Text); } catch (Exception ex) { } };
                    textBlock.MouseEnter += (w, r) => { textBlock.Opacity = 0.5; };
                    textBlock.MouseLeave += (w, r) => { textBlock.Opacity = 1; };
                    listRet.Add(textBlock);


                    TextBlock textBlockTemp = new()
                    {
                        Text = list[1],
                        FontSize = 14
                    };

                    textBlockTemp.Text = list[1];
                    textBlockTemp.FontSize = 14;
                    listRet.Add(textBlockTemp);

                    TextBlock textBlock1 = new()
                    {
                        Text = list[2],
                        FontSize = 14,
                        ToolTip = "Натисніть, щоб скопіювати"
                    };

                    textBlock1.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock1.Text); } catch (Exception ex) { } };
                    textBlock1.MouseEnter += (w, r) => { textBlock1.Opacity = 0.5; };
                    textBlock1.MouseLeave += (w, r) => { textBlock1.Opacity = 1; };
                    listRet.Add(textBlock1);
                }
                else if (list.Length == 1)
                {
                    TextBlock textBlock = new()
                    {
                        Text = list[0],
                        FontSize = 14,
                        Padding = new Thickness(20, 0, 0, 0),
                        ToolTip = "Натисніть, щоб скопіювати"
                    };

                    textBlock.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock.Text); } catch (Exception ex) { } };
                    textBlock.MouseEnter += (w, r) => { textBlock.Opacity = 0.5; };
                    textBlock.MouseLeave += (w, r) => { textBlock.Opacity = 1; };
                    listRet.Add(textBlock);
                }
            }
            return listRet;
        }
        private static TextBlock CreateTextBlockFigurant(Figurant figurant)
        {
            TextBlock textBlock = new()
            {
                Margin = new Thickness(10, -2, 0, 0),
                Text = GetDefInString(figurant)
        };
            return textBlock;
        }
        private static TextBlock CreateTextBlockReest(int num)
        {
            TextBlock textBlock = new()
            {
                Margin = new Thickness(10, 0, 0, 0),
                Text = $"{num}. {Reest.abbreviatedName[num - 1]}"
            };
            return textBlock;
        }
        private static TextBlock CreateTextBlockShema(int num)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Margin = new Thickness(10, 0, 0, 0);
            textBlock.Text = $"{num}. Схеми";
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
        private StackPanel CreateStackPanelFigurantsShema(Figurant figurant, int num)
        {
            StackPanel stackPanel = new()
            {
                Orientation = Orientation.Horizontal,
            };


            stackPanel.Children.Add(CreateCheckBoxShema(CheckEnum.Yes, num));
            stackPanel.Children.Add(CreateCheckBoxShema(CheckEnum.No, num));

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

            treeViewItem.Items.Add(CreateTextBoxYesNo(num));
            foreach (var itemF in figurants)
            {
                treeViewItem.Items.Add(CreateStackPanelFigurants(itemF, num));
            }
            return treeViewItem;
        }
        private TreeViewItem CreateTreeViewItemShema(int num)
        {
            TreeViewItem treeViewItem = new TreeViewItem();

            treeViewItem.Margin = new Thickness(0, -2, 0, 0);
            treeViewItem.Header = CreateTextBlockShema(num);

            treeViewItem.Items.Add(CreateTextBoxYesNoShema(num));
            foreach (var itemF in figurants)
            {
                treeViewItem.Items.Add(CreateStackPanelFigurantsShema(itemF, num));
            }
            return treeViewItem;
        }
        private StackPanel CreateTextBoxYesNo(int num)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;

            TextBlock textBlockYes = new TextBlock();
            textBlockYes.Text = "так";
            textBlockYes.Tag = num;
            textBlockYes.PreviewMouseDown += ClickAllFigurant;
            textBlockYes.MouseEnter += (w, r) => { textBlockYes.Opacity = 0.5; };
            textBlockYes.MouseLeave += (w, r) => { textBlockYes.Opacity = 1; };
            textBlockYes.FontSize = 11;
            textBlockYes.Margin = new Thickness(5, 0, 0, 0);

            TextBlock textBlockNo = new TextBlock();
            textBlockNo.Text = "ні";
            textBlockNo.Tag = num;
            textBlockNo.PreviewMouseDown += ClickAllFigurant;
            textBlockNo.MouseEnter += (w, r) => { textBlockNo.Opacity = 0.5; };
            textBlockNo.MouseLeave += (w, r) => { textBlockNo.Opacity = 1; };
            textBlockNo.FontSize = 11;
            textBlockNo.Margin = new Thickness(8, 0, 0, 0);

            stackPanel.Children.Add(textBlockYes);
            stackPanel.Children.Add(textBlockNo);


            return stackPanel;
        }
        private StackPanel CreateTextBoxYesNoShema(int num)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;

            TextBlock textBlockYes = new TextBlock();
            textBlockYes.Text = "так";
            textBlockYes.Tag = num;
            textBlockYes.PreviewMouseDown += ClickAllFigurantShema;
            textBlockYes.MouseEnter += (w, r) => { textBlockYes.Opacity = 0.5; };
            textBlockYes.MouseLeave += (w, r) => { textBlockYes.Opacity = 1; };
            textBlockYes.FontSize = 11;
            textBlockYes.Margin = new Thickness(5, 0, 0, 0);

            TextBlock textBlockNo = new()
            {
                Text = "ні",
                Tag = num,
                FontSize = 11,
                Margin = new Thickness(8, 0, 0, 0)
            };
            textBlockNo.PreviewMouseDown += ClickAllFigurantShema;
            textBlockNo.MouseEnter += (w, r) => { textBlockNo.Opacity = 0.5; };
            textBlockNo.MouseLeave += (w, r) => { textBlockNo.Opacity = 1; };

            stackPanel.Children.Add(textBlockYes);
            stackPanel.Children.Add(textBlockNo);


            return stackPanel;
        }
        private void ClickAllFigurant(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                if (sender is TextBlock tb)
                {
                    int num = (int)tb.Tag;
                    if (treeView1.Items.Count >= num + 1)
                    {
                        if (treeView1.Items[num] is StackPanel stRe)
                        {
                            if (stRe.Children[2] is TreeViewItem treeItem)
                            {
                                foreach (var itemF in treeItem.Items)
                                {
                                    if (itemF is StackPanel stF)
                                    {
                                        if (stF.Children[0] is CheckBox chYes && stF.Children[1] is CheckBox chNo)
                                        {
                                            if (tb.Text == "так")
                                            {
                                                chYes.IsChecked = true;
                                                chNo.IsChecked = false;
                                                IfhaveTwoCheck(num, CheckEnum.Yes);
                                            }
                                            else
                                            {
                                                chYes.IsChecked = false;
                                                chNo.IsChecked = true;
                                                IfhaveTwoCheck(num, CheckEnum.No);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            inactivityTimer.Start();
        }
        private void ClickAllFigurantShema(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                if (sender is TextBlock tb)
                {
                    int num = (int)tb.Tag;
                    if (treeViewShema.Items[0] is StackPanel stRe)
                    {
                        if (stRe.Children[2] is TreeViewItem treeItem)
                        {
                            foreach (var itemF in treeItem.Items)
                            {
                                if (itemF is StackPanel stF)
                                {
                                    if (stF.Children[0] is CheckBox chYes && stF.Children[1] is CheckBox chNo)
                                    {
                                        if (tb.Text == "так")
                                        {
                                            chYes.IsChecked = true;
                                            chNo.IsChecked = false;
                                            IfhaveTwoCheckShema(num, CheckEnum.Yes);
                                        }
                                        else
                                        {
                                            chYes.IsChecked = false;
                                            chNo.IsChecked = true;
                                            IfhaveTwoCheckShema(num, CheckEnum.No);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            inactivityTimer.Start();
        }
        private void CheckBoxClick(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                if (sender is CheckBox checkBox)
                {
                    if (checkBox.Tag != null)
                    {
                        if (checkBox.Tag is Tuple<CheckEnum, int> checkTuple)
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            inactivityTimer.Start();
        }
        private void CheckBoxClickShema(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                if (sender is CheckBox checkBox)
                {
                    if (checkBox.Tag != null)
                    {
                        if (checkBox.Tag is Tuple<CheckEnum, int> checkTuple)
                        {
                            if (checkTuple.Item1 == CheckEnum.Control)
                            {
                                //checkBox.IsChecked = false;
                                IfhaveTwoCheckShema(checkTuple.Item2, CheckEnum.Control);
                            }
                            else if (checkTuple.Item1 == CheckEnum.Shema)
                            {
                                if (numbColorShema == 1)
                                {
                                    checkBox.IsChecked = false;
                                }
                            }
                            else if (checkTuple.Item1 == CheckEnum.Yes)
                            {
                                IfhaveTwoCheckShema(checkTuple.Item2, CheckEnum.Yes);
                            }
                            else if (checkTuple.Item1 == CheckEnum.No)
                            {
                                IfhaveTwoCheckShema(checkTuple.Item2, CheckEnum.No);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            inactivityTimer.Start();
        }
        void IfhaveTwoCheck(int num, CheckEnum checkEnum)
        {
            if (treeView1.Items[num] is StackPanel st)
            {
                if(st.Children[2] is TreeViewItem tri 
                    &&
                    st.Children[0] is CheckBox chC)
                {
                    if (tri.Header is TextBlock tiH)
                    {
                        if (numbColorInReestr[num - 1] == (int)StatusFolder.NotEmpty)
                        {
                            tiH.Foreground = this.Resources["GreenEmpty"] as SolidColorBrush;
                        }
                        else if (numbColorInReestr[num - 1] == (int)StatusFolder.Empty)
                        {
                            tiH.Foreground = this.Resources["4ColorStyle"] as SolidColorBrush;  // new SolidColorBrush(Colors.White);
                        }
                        else if (numbColorInReestr[num - 1] == (int)StatusFolder.NotCreated)
                        {
                            tiH.Foreground = this.Resources["RedEmpty"] as SolidColorBrush;     //new SolidColorBrush(Colors.Red);
                        }
                        else if(numbColorInReestr[num - 1] == (int)StatusFolder.Undefined)
                        {
                            tiH.Foreground = this.Resources["4ColorStyle"] as SolidColorBrush;
                        }
                    }
                    var isAllCh = true;
                    foreach (var itemF in tri.Items)
                    {
                        if (itemF is StackPanel stF)
                        {
                            if(stF.Children[0] is CheckBox chYes && stF.Children[1] is CheckBox chNo)
                            {
                                if (chYes.IsChecked.Value && chNo.IsChecked.Value)
                                {
                                    if (checkEnum == CheckEnum.Yes)
                                    {
                                        chNo.IsChecked = false;
                                    }
                                    else if (checkEnum == CheckEnum.No)
                                    {
                                        chYes.IsChecked = false;
                                    }
                                }

                                if (!chNo.IsChecked.Value && !chYes.IsChecked.Value)
                                {
                                    foreach (var itemTB in stF.Children)
                                    {
                                        if (itemTB is TextBlock cutTB)
                                        {
                                            if (numbColorInReestr[num - 1] == (int)StatusFolder.NotEmpty)
                                            {
                                                cutTB.Foreground = this.Resources["GreenEmpty"] as SolidColorBrush;
                                            }
                                            else if (numbColorInReestr[num - 1] == (int)StatusFolder.Empty)
                                            {
                                                cutTB.Foreground = this.Resources["4ColorStyle"] as SolidColorBrush; //new SolidColorBrush(Colors.White);
                                            }
                                            else if (numbColorInReestr[num - 1] == (int)StatusFolder.NotCreated)
                                            {

                                                cutTB.Foreground = this.Resources["RedEmpty"] as SolidColorBrush; //new SolidColorBrush(Colors.Red);
                                            }
                                            else if (numbColorInReestr[num - 1] == (int)StatusFolder.Undefined)
                                            {
                                                cutTB.Foreground = this.Resources["4ColorStyle"] as SolidColorBrush;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (numbColorInReestr[num - 1] != 1)
                                    {
                                        foreach (var itemTB in stF.Children)
                                        {
                                            if (itemTB is TextBlock cutTB)
                                            {
                                                cutTB.Foreground = this.Resources["BlueCheck"] as SolidColorBrush;
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
                    }
                    
                    
                    chC.IsChecked = isAllCh;
                }
            }
        }
        void IfhaveTwoCheckShema(int num, CheckEnum checkEnum)
        {
            if (treeViewShema.Items[0] is StackPanel st)
            {
                if (st.Children[2] is TreeViewItem tri &&
                    st.Children[0] is CheckBox chC)
                {
                    if (tri.Header is TextBlock tiH)
                    {
                        if (numbColorShema == (int)StatusFolder.NotEmpty)
                        {
                            tiH.Foreground = this.Resources["GreenEmpty"] as SolidColorBrush;
                        }
                        else if (numbColorShema == (int)StatusFolder.Empty)
                        {
                            tiH.Foreground = this.Resources["4ColorStyle"] as SolidColorBrush;  // new SolidColorBrush(Colors.White);
                        }
                        else if (numbColorShema == (int)StatusFolder.NotCreated)
                        {
                            tiH.Foreground = this.Resources["RedEmpty"] as SolidColorBrush;     //new SolidColorBrush(Colors.Red);
                        }
                        else if(numbColorShema == (int)StatusFolder.Undefined)
                        {
                            tiH.Foreground = this.Resources["4ColorStyle"] as SolidColorBrush;
                        }
                    }
                    var isAllCh = true;
                    foreach (var itemF in tri.Items)
                    {
                        if (itemF is StackPanel stF)
                        {
                            if(stF.Children[0] is CheckBox chYes && stF.Children[1] is CheckBox chNo)
                            {
                                if (chYes.IsChecked.Value && chNo.IsChecked.Value)
                                {
                                    if (checkEnum == CheckEnum.Yes)
                                    {
                                        chNo.IsChecked = false;
                                    }
                                    else if (checkEnum == CheckEnum.No)
                                    {
                                        chYes.IsChecked = false;
                                    }
                                }

                                if (!chNo.IsChecked.Value && !chYes.IsChecked.Value)
                                {
                                    foreach (var itemTB in stF.Children)
                                    {
                                        if (itemTB is TextBlock cutTB)
                                        {
                                            if (numbColorShema == (int)StatusFolder.NotEmpty)
                                            {
                                                cutTB.Foreground = this.Resources["GreenEmpty"] as SolidColorBrush;
                                            }
                                            else if (numbColorShema == (int)StatusFolder.Empty)
                                            {
                                                cutTB.Foreground = this.Resources["4ColorStyle"] as SolidColorBrush; //new SolidColorBrush(Colors.White);
                                            }
                                            else if (numbColorShema == (int)StatusFolder.NotCreated)
                                            {
                                                cutTB.Foreground = this.Resources["RedEmpty"] as SolidColorBrush; //new SolidColorBrush(Colors.Red);
                                            }
                                            else if(numbColorShema == (int)StatusFolder.Undefined)
                                            {
                                                cutTB.Foreground = this.Resources["4ColorStyle"] as SolidColorBrush;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (numbColorShema != 1)
                                    {
                                        foreach (var itemTB in stF.Children)
                                        {
                                            if (itemTB is TextBlock cutTB)
                                            {
                                                cutTB.Foreground = this.Resources["BlueCheck"] as SolidColorBrush;
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
                    }


                    chC.IsChecked = isAllCh;
                }
            }
        }
        void Update()
        {
            List<bool> listIsExpanded = new List<bool>();
            foreach (var item in treeView1.Items)
            {
                if(item is StackPanel st)
                {
                    if(st.Children[2] is TreeViewItem tri)
                    {
                        listIsExpanded.Add(tri.IsExpanded);

                    }
                }
            }


            foreach (var item in treeView1.Items)
            {
                if (item is StackPanel st)
                {
                    if(st.Children[0] is CheckBox chC  &&
                        st.Children[1] is CheckBox chS &&
                        st.Children[2] is TreeViewItem tri)
                    {
                        foreach (var itemF in tri.Items)
                        {
                            if (itemF is StackPanel stF)
                            {
                                if(stF.Children[0] is CheckBox chYes &&
                                    stF.Children[1] is CheckBox chNo)
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
                var birth = d.DtBirth;
                if (birth != null)
                {
                    return $"{d.Fio}^, ^{birth.ToString()?[0..10] ?? ""}^ р.н.^, РНОКПП ^{d.Ipn}";
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
        static public string? GetDefInStringWithout(Figurant d)
        {
            return GetDefInStringWithoutCircumflex(d) + $" по № {d.NumbInput}";
        }
        static public string? GetDefInStringWithoutCircumflex(Figurant d)
        {
            if (d.ResFiz != null)
            {
                var birth = d.DtBirth;
                if (birth != null)
                {
                    return $"{d.Fio}, {birth.ToString()?[0..10] ?? ""} р.н., РНОКПП {d.Ipn}";
                }
                return $"{d.Fio}, РНОКПП {d.Ipn}";
            }
            else
            {
                if (d.Code == null || d.Code == "")
                    return $"{d.Name}";
                return $"{d.Name} (ЄДРПОУ {d.Code})";
            }
        }
        //private bool? GetCheckBoxControl(int num)
        //{
        //    var st = treeView1.Items[num - 1] as StackPanel;
        //    if (st != null)
        //    {
        //        var chC = st.Children[0] as CheckBox;
        //        //var chS = st.Children[1] as CheckBox;
        //        return chC.IsChecked.Value;
        //    }
        //    return null;
        //}
        //private bool? GetCheckBoxShema(int num)
        //{
        //    var st = treeView1.Items[num - 1] as StackPanel;
        //    if (st != null)
        //    {
        //        //var chC = st.Children[0] as CheckBox;
        //        var chS = st.Children[1] as CheckBox;
        //        return chS.IsChecked.Value;
        //    }
        //    return null;
        //}
        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                ToCheckFolders();
                ToCheckFoldersShema();
                var listNoLogic = GetListNoLogic();
                if (IsListNoLogicEmpty(listNoLogic))
                {
                    SaveCheckBoxesFig();
                    this.DialogResult = true;
                }
                else
                {
                    string strReest = "Інформація не відповідає дійсності в реєстрах:\n";

                    for (int i = 0; i < Reest.abbreviatedName.Count; i++)
                    {
                        if (listNoLogic[i] != "")
                            strReest += $"\n{i + 1}. {Reest.abbreviatedName[i]} по фігурантам:\n{listNoLogic[i]}";
                    }
                    if (listNoLogic[Reest.abbreviatedName.Count] != "")
                        strReest += $"\n{Reest.abbreviatedName.Count + 1}. Схеми по фігурантам:\n{listNoLogic[Reest.abbreviatedName.Count]}";
                    MessageBox.Show(strReest, "Помилка");
                    InitField();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            inactivityTimer.Start();
        }
        private List<string> GetListNoControl()
        {
            List<string> listNoContr = new List<string>();
            for (int i = 1; i < treeView1.Items.Count; i++)
            {
                if (numbColorInReestr[i-1] != 1)
                {
                    if(treeView1.Items[i] is StackPanel st)
                    {
                        var chC = st.Children[0] as CheckBox;
                        if (!chC!.IsChecked!.Value)
                        {
                            listNoContr.Add(i + ". " +Reest.abbreviatedName[i - 1]);
                        }
                    }
                }
            }
            if(numbColorShema != 1)
            {
                if (treeViewShema.Items[0] is StackPanel st)
                {
                    var chC = st.Children[0] as CheckBox;
                    if (!chC!.IsChecked!.Value)
                    {
                        listNoContr.Add(Reest.abbreviatedName.Count + 1 + ". Схеми");
                    }
                }
            }
            return listNoContr;
        }
        private List<string> GetListNoLogic()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < Reest.abbreviatedName.Count + 1; i++)
            {
                list.Add("");
            }

            for (int i = 1; i <= figurants.Count; i++)
            {
                for (int j = 0; j < Reest.abbreviatedName.Count; j++)
                {
                    var yes = GetFigCheckYesReestr(j + 1, i);
                    var no = GetFigCheckNoReestr(j + 1, i);
                    if(yes!=null && no != null)
                    {
                        var statusReestrForFig = GetStatusFolder(i, j + 1);
                        if (statusReestrForFig == StatusFolder.NotCreated)
                        {
                            if (yes.IsChecked!.Value || no.IsChecked!.Value)
                            {
                                list[j] += $"{GetDefInStringWithout(figurants[i - 1])};\n";
                            }
                        }
                        else if (statusReestrForFig == StatusFolder.Empty)
                        {
                            if (!(!yes.IsChecked!.Value && no.IsChecked!.Value))
                            {
                                list[j] += $"{GetDefInStringWithout(figurants[i - 1])};\n";
                            }
                        }
                        else if (statusReestrForFig == StatusFolder.NotEmpty)
                        {
                            if (!(yes.IsChecked!.Value && !no.IsChecked!.Value))
                            {
                                list[j] += $"{GetDefInStringWithout(figurants[i - 1])};\n";
                            }
                        }
                    }
                }
                var yesS = GetFigCheckYesShema(i);
                var noS = GetFigCheckNoShema(i);
                if (yesS != null && noS != null)
                {
                    var statusReestrForFig = GetStatusFolderShema(i);
                    if (statusReestrForFig == StatusFolder.NotCreated)
                    {
                        if (yesS.IsChecked!.Value || noS.IsChecked!.Value)
                        {
                            list[Reest.abbreviatedName.Count] += $"{GetDefInStringWithout(figurants[i - 1])};\n";
                        }
                    }
                    else if (statusReestrForFig == StatusFolder.Empty)
                    {
                        if (!(!yesS.IsChecked!.Value && noS.IsChecked!.Value))
                        {
                            list[Reest.abbreviatedName.Count] += $"{GetDefInStringWithout(figurants[i - 1])};\n";
                        }
                    }
                    else if (statusReestrForFig == StatusFolder.NotEmpty)
                    {
                        if (!(yesS.IsChecked!.Value && !noS.IsChecked!.Value))
                        {
                            list[Reest.abbreviatedName.Count] += $"{GetDefInStringWithout(figurants[i - 1])};\n";
                        }
                    }
                }

            }

            return list;
        }
        private static bool IsListNoLogicEmpty(List<string> list)
        {
            foreach (var item in list)
            {
                if (item != "")
                    return false;
            }
            return true;
        }
        private StatusFolder GetStatusFolder(int numFig, int numReest)
        {
            var folderCurrentNumbIn = (from mc in modelContext.MainConfigs where mc.NumbInput == figurants[numFig - 1].NumbInput select mc.Folder).First();
            if (folderCurrentNumbIn != null)
            {
                if (Directory.Exists(folderCurrentNumbIn))
                {
                    if(Directory.Exists($"{folderCurrentNumbIn}\\{numReest}. {Reest.abbreviatedName[numReest - 1]}"))
                    {
                        if (Directory.GetFiles($"{folderCurrentNumbIn}\\{numReest}. {Reest.abbreviatedName[numReest - 1]}").Length == 0
                                   &&
                                   Directory.GetDirectories($"{folderCurrentNumbIn}\\{numReest}. {Reest.abbreviatedName[numReest - 1]}").Length == 0)
                        {
                            return StatusFolder.Empty;
                        }
                        else
                        {
                            return StatusFolder.NotEmpty;
                        }
                    }
                    else
                    {
                        return StatusFolder.NotCreated;
                    }
                }
                return StatusFolder.Undefined;
            }
            return StatusFolder.Undefined;
        }
        private StatusFolder GetStatusFolderShema(int numFig)
        {
            var folderCurrentNumbIn = (from mc in modelContext.MainConfigs where mc.NumbInput == figurants[numFig - 1].NumbInput select mc.Folder).First();
            if (folderCurrentNumbIn != null)
            {
                if (Directory.Exists(folderCurrentNumbIn))
                {
                    if (Directory.Exists($"{folderCurrentNumbIn}\\{Reest.abbreviatedName.Count + 1}. Схеми"))
                    {
                        if (Directory.GetFiles($"{folderCurrentNumbIn}\\{Reest.abbreviatedName.Count + 1}. Схеми").Length == 0
                                   &&
                                   Directory.GetDirectories($"{folderCurrentNumbIn}\\{Reest.abbreviatedName.Count + 1}. Схеми").Length == 0)
                        {
                            return StatusFolder.Empty;
                        }
                        else
                        {
                            return StatusFolder.NotEmpty;
                        }
                    }
                    else
                    {
                        return StatusFolder.NotCreated;
                    }
                }
                return StatusFolder.Undefined;
            }
            return StatusFolder.Undefined;
        }
        private StackPanel? GetStackPanelReestr(int num)
        {
            if(num < treeView1.Items.Count)
            {
                return treeView1.Items[num] as StackPanel;
            }
            if(num == treeView1.Items.Count) 
            {
                return treeViewShema.Items[0] as StackPanel;
            }
            return null;
        }
        private StackPanel? GetStackPanelShema()
        {
         if(treeViewShema != null)   
                return treeViewShema.Items[0] as StackPanel;
            return null;
        }
        //private CheckBox? GetCheckControlReestr(int num)
        //{
        //    var st = GetStackPanelReestr(num);
        //    if (st != null)
        //        if (st.Children.Count > 0)
        //        {
        //            return st.Children[0] as CheckBox;
        //        }
        //    return null;
        //}
        //private CheckBox? GetCheckShemaReestr(int num)
        //{
        //    var st = GetStackPanelReestr(num);
        //    if (st != null)
        //        if (st.Children.Count > 1)
        //        {
        //            return st.Children[1] as CheckBox;
        //        }
        //    return null;
        //}
        private TreeViewItem? GetTreeViewItemReestr(int num)
        {
            var st = GetStackPanelReestr(num);
            if(st!=null)
                if(st.Children.Count > 2)
                {
                    return st.Children[2] as TreeViewItem;
                }
            return null;
        }
        private TreeViewItem? GetTreeViewItemShema()
        {
            var st = GetStackPanelShema();
            if (st != null)
                if (st.Children.Count > 2)
                {
                    return st.Children[2] as TreeViewItem;
                }
            return null;
        }
        private StackPanel? GetFigStackPanelReestr(int num, int numFig)
        {
            var tvi = GetTreeViewItemReestr(num);
            if (tvi != null)
                if (tvi.Items.Count > numFig)
                {
                    return tvi.Items[numFig] as StackPanel;
                }
            return null;
        }
        private StackPanel? GetFigStackPanelShema(int numFig)
        {
            var tvi = GetTreeViewItemShema();
            if (tvi != null)
                if (tvi.Items.Count > numFig)
                {
                    return tvi.Items[numFig] as StackPanel;
                }
            return null;
        }
        private CheckBox? GetFigCheckYesReestr(int num, int numFig)
        {
            var STtvi = GetFigStackPanelReestr(num, numFig);
            if (STtvi != null)
                if (STtvi.Children.Count > 0)
                {
                    return STtvi.Children[0] as CheckBox;
                }
            return null;
        }
        private CheckBox? GetFigCheckYesShema(int numFig)
        {
            var STtvi = GetFigStackPanelShema(numFig);
            if (STtvi != null)
                if (STtvi.Children.Count > 0)
                {
                    return STtvi.Children[0] as CheckBox;
                }
            return null;
        }
        private CheckBox? GetFigCheckNoReestr(int num, int numFig)
        {
            var STtvi = GetFigStackPanelReestr(num, numFig);
            if (STtvi != null)
                if (STtvi.Children.Count > 1)
                {
                    return STtvi.Children[1] as CheckBox;
                }
            return null;
        }
        private CheckBox? GetFigCheckNoShema(int numFig)
        {
            var STtvi = GetFigStackPanelShema(numFig);
            if (STtvi != null)
                if (STtvi.Children.Count > 1)
                {
                    return STtvi.Children[1] as CheckBox;
                }
            return null;
        }
        private void SaveCheckBoxesFig()
        {
            for (int i = 0; i < figurants.Count; i++)
            {
                List<bool?> listYes = new ();
                List<bool?> listNo = new ();

                for (int j = 0; j < Reest.abbreviatedName.Count + 1; j++)
                {
                    listYes.Add(GetFigCheckYesReestr(j + 1, i + 1)!.IsChecked!.Value);
                    listNo.Add(GetFigCheckNoReestr(j + 1, i + 1)!.IsChecked!.Value);
                }
                figurants[i].Control = GetStringFromCh(listYes);
                figurants[i].Shema = GetStringFromCh(listNo);

            }
            modelContext.SaveChanges();
        }
        private static string GetStringFromCh(List<bool?> listb)
        {
            string retS = "";

            foreach (var item in listb)
            {
                if (item != null)
                {
                    if((bool)item)
                        retS += "1";
                    else
                        retS += "0";
                }
                else
                {
                    retS += "0";
                }
            }

            while (retS.Length < Reest.abbreviatedName.Count + 1)
            {
                retS += "0";
            }
            return retS;
        }
        private static List<bool?> GetChFromString(string? str)
        {
            List<bool?> ret = new List<bool?>();

            for (int i = 0; i < Reest.abbreviatedName.Count + 1 ; i++)
            {
                if(str != null)
                {
                    if (i < str.Length)
                    {
                        ret.Add(str[i] == '1');
                    }
                    else
                    {
                        ret.Add(false);
                    }
                }
                else
                {
                    ret.Add(false);
                }
            }
            return ret;
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                ToCheckFolders();
                ToCheckFoldersShema();

                var listNoLogic = GetListNoLogic();
                if (IsListNoLogicEmpty(listNoLogic))
                {
                    SaveCheckBoxesFig();
                    MessageBox.Show("Дані збережено!");
                }
                else
                {
                    string strReest = "Інформація не відповідає дійсності в реєстрах:\n";

                    for (int i = 0; i < Reest.abbreviatedName.Count; i++)
                    {
                        if (listNoLogic[i] != "")
                            strReest += $"\n{i + 1}. {Reest.abbreviatedName[i]} по фігурантам:\n{listNoLogic[i]}";
                    }
                    if (listNoLogic[Reest.abbreviatedName.Count] != "")
                        strReest += $"\n{Reest.abbreviatedName.Count + 1}. Схеми по фігурантам:\n{listNoLogic[Reest.abbreviatedName.Count]}";
                    MessageBox.Show(strReest, "Помилка");
                }
                InitField();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            inactivityTimer.Start();
        }
        private void ButtonReq_Click(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                RequestsWindow requestsWindow = new RequestsWindow(listNumIn, modelContext, TypeOfAppeal.Сombined,
                        inactivityTimer);
                if (requestsWindow.ShowDialog() == true)
                {

                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            inactivityTimer.Start();
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
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            inactivityTimer.Stop();
        }
    }
}
