using DesARMA.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Security.Policy;
using NPOI.SS.Formula.Functions;
using static NPOI.HSSF.Util.HSSFColor;
using System.Windows.Documents;
using System.Diagnostics;
using SixLabors.Fonts.Tables.AdvancedTypographic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using SixLabors.ImageSharp.Drawing;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Windows.Automation;
using DesARMA.Automation;
using DesARMA.Registers;
using System.Windows.Threading;
using DesARMA.Registers.EDR;

namespace DesARMA
{
    public enum FigYesNo
    {
        Yes,
        No
    }
    public class AllDirectories
    {
        public MainConfig @MainConfig { get; private set; } = null!;
        public List<Figurant>? Figurants { get; private set; } = null!;
        readonly ModelContext modelContext = null!;
        readonly UpdateDel updatePanel = null!;
        readonly System.Windows.Controls.TreeView treeView1 = null!;

        List<bool> listContr;
        List<bool> listSh;
        List<List<bool>?> listFigurantCheckListSHEMA;
        List<List<bool>?> listFigurantCheckListCONTROL;

        readonly SolidColorBrush RedSolidColorBrush = null!;
        readonly SolidColorBrush WhiteSolidColorBrush = null!;
        readonly SolidColorBrush GreenSolidColorBrush = null!;
        readonly List<System.Windows.Controls.Button> buttonList = new();

        MainWindow mainWindow;
        public AllDirectories(Main main, @MainConfig @MainConfig, UpdateDel updatePanel,
            SolidColorBrush RedSolidColorBrush, SolidColorBrush WhiteSolidColorBrush, SolidColorBrush GreenSolidColorBrush,
            System.Windows.Controls.TreeView treeView1, ModelContext modelContext, MainWindow mainWindow
            )
        {
            this.mainWindow = mainWindow;
            this.@MainConfig = @MainConfig;
            this.RedSolidColorBrush = RedSolidColorBrush;
            this.WhiteSolidColorBrush = WhiteSolidColorBrush;
            this.GreenSolidColorBrush = GreenSolidColorBrush;
            this.treeView1 = treeView1;
            this.modelContext = modelContext;
            Figurants = (from f in modelContext.Figurants where f.NumbInput == main.NumbInput && f.Status == 1 select f).ToList();


            listContr = GetBoolsFromString(@MainConfig.Control);
            listSh = GetBoolsFromString(@MainConfig.Shema);

            listFigurantCheckListSHEMA = GetFigurantCheckListNO();
            listFigurantCheckListCONTROL = GetFigurantCheckListYES();
            this.updatePanel = updatePanel;

        }
        public System.Windows.Controls.TreeView CreateNewTree()
        {
            treeView1!.Items!.Clear();

            listContr = GetBoolsFromString(@MainConfig.Control);
            listSh = GetBoolsFromString(@MainConfig.Shema);

            for (int i = 0; i < Reest.abbreviatedName.Count; i++)
            {
                treeView1.Items.Add(СreatePosition(i + 1, Reest.abbreviatedName[i]));
            }
            treeView1.Items.Add(СreatePosition(Reest.abbreviatedName.Count + 1, "Схеми"));
            Update();

            return treeView1;
        }
        private StackPanel CreateTextBlockesYesNo(int num)
        {
            StackPanel st = new ()
            {
                Orientation = System.Windows.Controls.Orientation.Horizontal
            };

            TextBlock t1 = new()
            {
                Text = "так ",
                Tag = num
            };
            t1.PreviewMouseDown += ClickAllFigurantYes;
            t1.MouseEnter += (w, r) => { t1.Opacity = 0.5; };
            t1.MouseLeave += (w, r) => { t1.Opacity = 1; };

            TextBlock t2 = new() 
            {
                Text = "ні ",
                Tag = num
            };
            t2.PreviewMouseDown += ClickAllFigurantNo;
            t2.MouseEnter += (w, r) => { t2.Opacity = 0.5; };
            t2.MouseLeave += (w, r) => { t2.Opacity = 1; };

            st.Children.Add(t1);
            st.Children.Add(t2);

            return st;
        }
        private void ClickAllFigurantYes(object sender, RoutedEventArgs e)
        {
            if(sender is TextBlock tb)
            {
                int id = (int)tb.Tag;
                var listFigChB = GetFigListTupleCheckBoxes(id);
                if(listFigChB != null)
                {
                    foreach (var itemF in listFigChB)
                    {
                        var chN = itemF.Item1;
                        var chY = itemF.Item2;
                        if (chN != null && chY != null)
                        {
                            chN.IsChecked = true;
                            chY.IsChecked = false;
                        }
                    }
                }
                GetControlCheckBox(id)!.IsChecked = true;
            }
        }
        private void ClickAllFigurantNo(object sender, RoutedEventArgs e)
        {
            if (sender is TextBlock tb)
            {
                int id = (int)tb.Tag;
                var listFigChB = GetFigListTupleCheckBoxes(id);
                if (listFigChB != null)
                {
                    foreach (var itemF in listFigChB)
                    {
                        var chN = itemF.Item1;
                        var chY = itemF.Item2;
                        if (chN != null && chY != null)
                        {
                            chN.IsChecked = false;
                            chY.IsChecked = true;
                        }
                    }
                }
                GetControlCheckBox(id)!.IsChecked = true;
            }
        }
        private StackPanel СreatePosition(int idNum, string name)
        {
            StackPanel stackPanel = new () 
            {
                Orientation = Orientation.Horizontal
            };


            TreeViewItem tree = new ();

            var checkBoxIn = new System.Windows.Controls.CheckBox();
            {
                if (listSh != null)
                    if (listSh.Count >= idNum)
                        checkBoxIn.IsChecked = listSh[idNum - 1];
            }
            
            var checkBox = new System.Windows.Controls.CheckBox();
            {
                checkBox.Tag = false;
                checkBox.IsHitTestVisible = false;
                if (listContr != null)
                    if (listContr.Count >= idNum)
                        checkBox.IsChecked = listContr[idNum - 1];
            }
            var button = new System.Windows.Controls.Button();
            {
                button.Background = Brushes.Transparent;
                button.FontSize = 12;
                button.BorderThickness = new Thickness(0);
                button.HorizontalContentAlignment = HorizontalAlignment.Center;
                button.Height = 30;
                button.Tag = idNum;
                button.Margin = new Thickness(10, 0, 10, 5);
                button.Click += AutomationHendler;



                var image = new Image();
                image.Source = new BitmapImage(new Uri($"pack://application:,,,/DesARMA;component/Drawings/ExtractFromTheRegister/extract.png"));
                image.Stretch = Stretch.UniformToFill;
                button.Content = image;
                

                buttonList.Add(button);
            }
            
            // на самому початку задано червоний колір
            
            tree.Header = $"{idNum}. " + name;

            if (IsAvailableDirectory(idNum, name))
            {
                tree.Foreground = GreenSolidColorBrush;

                if (IsEmptyDirectory(idNum, name))
                {
                    tree.Foreground = WhiteSolidColorBrush;
                }
            }
            else
            {
                tree.Foreground = RedSolidColorBrush;
            }


            if (Figurants != null)
            {
                tree.Items.Add(CreateTextBlockesYesNo(idNum));
            }

            if (idNum < Reest.abbreviatedName.Count)
            {
                var contextmenu = new ContextMenu();
                tree.ContextMenu = contextmenu;

                var mi = new MenuItem()
                {
                    Header = "Видалити",
                    Tag = $"{idNum}. {Reest.abbreviatedName[idNum - 1]}"
                };
               
                mi.Click += DeleteDir;
                contextmenu.Items.Add(mi);
            }


            for (int i = 0; i < Figurants!.Count; i++)
            {
                    var sdhjgf = CreatePozFig(idNum, i + 1);
                    if (sdhjgf != null)
                    {
                        tree.Items.Add(sdhjgf);
                    }
            }
            stackPanel.Children.Add(checkBox);
            stackPanel.Children.Add(checkBoxIn);
            stackPanel.Children.Add(button);
            stackPanel.Children.Add(tree);
            return stackPanel;
        }
        private void ListButtonReestrChangeEnabled()
        {
            //foreach (var item in buttonList)
            //{
            //    item.IsEnabled = !item.IsEnabled;
            //}
            //mainWindow.IsEnabled = !mainWindow.IsEnabled;
        }
        private async void AutomationHendler(object sender, RoutedEventArgs e)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;
            try
            {
                if (sender is System.Windows.Controls.Button b)
                {

                    int numberR = (int)b.Tag;
                    if (numberR < Reest.abbreviatedName.Count)
                    {
                        if (IsAvailableDirectory(numberR, Reest.abbreviatedName[numberR - 1]) && (numberR >= 15 && numberR <= 20 || numberR >= 38 && numberR <= 40))
                        {
                            b.IsEnabled = false;
                            

                            ProgresWindow progresWindow = new (mainWindow, numberR, cts);
                            //System.Windows.Application.Current.Dispatcher.Invoke(() =>
                            //{
                            //    progresWindow.Show();
                            //    progresWindow.CreateTitle($"Пошук по: {numberR}. {Reest.abbreviatedName[numberR - 1]}");
                            //}, ct);

                            await Task.Run(() =>
                            {
                                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                {
                                    progresWindow.Show();
                                    progresWindow.CreateTitle($"Пошук по: {numberR}. {Reest.abbreviatedName[numberR - 1]}");
                                });

                            }, ct);


                            

                            //{
                            //    Base, // - дані ЮО, ФО;
                            //    Branch, // - дані ВП;
                            //    Beneficiar, // - дані бенефіціарів (в тому числі в неструктурованому вигляді);
                            //    Founder, // - дані засновників,
                            //    Chief, // - дані керівника,
                            //    Assignee // - дані представників
                            //}

                            if (numberR == 15)
                            {
                                if (Figurants != null)
                                {
                                    var figs = (from f in Figurants where f.Ipn != null || f.Fio != null select f).ToList();
                                    var figsNotNeeded = (from f in Figurants where f.Code != null || f.Name != null select f).ToList();
                                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        progresWindow.CreateListFig(figs);
                                    });
                                    foreach (var item in figs)
                                    {
                                        string path = (from mc in modelContext.@MainConfigs where mc.NumbInput == item.NumbInput select mc.Folder).First();

                                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            progresWindow.SetDounloadFigNow(item);
                                        });

                                        var searchEDRTask = Task.Run(() =>
                                        {
                                            try
                                            {
                                                var dsd = new SearchEDR(cts, item.Ipn, item.Fio, null, 500,
                                                SearchType.Beneficiar, path + $"\\{numberR}. {Reest.abbreviatedName[numberR - 1]}",
                                                progresWindow, item, modelContext, true, numberR);

                                            }
                                            catch(Exception ex)
                                            {
                                                progresWindow.ErrorFigur(item, ex);
                                                return;
                                            }
                                            

                                        });
                                        await searchEDRTask;


                                    }
                                    //foreach (var item in figsNotNeeded)
                                    //{
                                    //    var listC = AllDirectories.GetBoolsFromString(item.Control);
                                    //    var listS = AllDirectories.GetBoolsFromString(item.Shema);

                                    //    listC[numberR - 1] = false;
                                    //    listS[numberR - 1] = true;

                                    //    item.Control = AllDirectories.GetStringFromBools(listC);
                                    //    item.Shema = AllDirectories.GetStringFromBools(listS);
                                    //    SaveToDB();
                                    //}
                                }
                            }
                            else if (numberR == 16)
                            {
                                if (Figurants != null)
                                {
                                    var figs = Figurants;
                                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        progresWindow.CreateListFig(figs);
                                    });
                                    foreach (var item in figs)
                                    {
                                        string path = (from mc in modelContext.@MainConfigs where mc.NumbInput == item.NumbInput select mc.Folder).First();

                                        //var progTask = Task.Run(() =>
                                        //{
                                        //    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                        //    {
                                        //        progresWindow.SetDounloadFigNow(item);
                                        //    });
                                        //});
                                        //progTask.Wait();
                                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            progresWindow.SetDounloadFigNow(item);
                                        });
                                        if (item.Ipn != null || item.Fio != null)
                                        {
                                            var searchEDRTask = Task.Run(() =>
                                            {
                                                try
                                                {
                                                    var dsd = new SearchEDR(cts,  item.Ipn, item.Fio, null, 500,
                                                    SearchType.Founder, path + $"\\{numberR}. {Reest.abbreviatedName[numberR - 1]}",
                                                    progresWindow, item, modelContext, true);
                                                    //dsd.CreateExel();
                                                    //dsd.CreatePDF();

                                                    //if (dsd.subjects != null && dsd.subjects?.Count == 0)
                                                    //{
                                                    //    progresWindow.NotDataFigur(item);
                                                    //}
                                                    //else
                                                    //{
                                                    //    progresWindow.SetDoneFigNow(item);
                                                    //}
                                                    //dsd.ToCheckFigInTree(numberR - 1);
                                                }
                                                catch(Exception ex)
                                                {
                                                    progresWindow.ErrorFigur(item, ex);
                                                    return;
                                                }

                                                
                                            });
                                            await searchEDRTask;

                                            //searchEDRTask.Wait(TimeSpan.FromDays(1));
                                            //var completedTask = await Task.WhenAny(searchEDRTask, Task.Delay(TimeSpan.FromMinutes(15)));
                                            //if (completedTask == searchEDRTask)
                                            //{
                                            //    // Завдання виконане до таймауту
                                            //    await searchEDRTask;
                                            //}
                                            //else
                                            //{
                                            //    // Завдання не встигло виконатися до таймауту
                                            //    throw new TimeoutException("The task has timed out.");
                                            //}
                                        }
                                        if (item.Code != null || item.Name != null)
                                        {
                                            var searchEDRTask = Task.Run(() =>
                                            {
                                                try
                                                {
                                                    var dsd = new SearchEDR(cts, item.Code, item.Name, null, 500,
                                                    SearchType.Founder, path + $"\\{numberR}. {Reest.abbreviatedName[numberR - 1]}", 
                                                    progresWindow, item, modelContext);
                                                    //dsd.CreateExel();
                                                    //dsd.CreatePDF();

                                                    //if (dsd.subjects != null && dsd.subjects?.Count == 0)
                                                    //{
                                                    //    progresWindow.NotDataFigur(item);
                                                    //}
                                                    //else
                                                    //{
                                                    //    progresWindow.SetDoneFigNow(item);
                                                    //}
                                                    //dsd.ToCheckFigInTree(numberR - 1);
                                                }
                                                catch (Exception ex)
                                                {
                                                    progresWindow.ErrorFigur(item, ex);
                                                    return;
                                                }
                                            });
                                            await searchEDRTask;

                                            //searchEDRTask.Wait(TimeSpan.FromDays(1));
                                            //searchEDRTask.Wait(TimeSpan.FromMinutes(15));
                                            //var completedTask = await Task.WhenAny(searchEDRTask, Task.Delay(TimeSpan.FromMinutes(15)));
                                            //if (completedTask == searchEDRTask)
                                            //{
                                            //    // Завдання виконане до таймауту
                                            //    await searchEDRTask;
                                            //}
                                            //else
                                            //{
                                            //    // Завдання не встигло виконатися до таймауту
                                            //    throw new TimeoutException("The task has timed out.");
                                            //}
                                        }

                                        //System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                        //{
                                        //    progresWindow.SetDoneFigNow(item);
                                        //});
                                        
                                    }
                                }
                            }
                            else if (numberR == 17)
                            {
                                if (Figurants != null)
                                {
                                    var figs = (from f in Figurants where f.Ipn != null || f.Fio != null select f).ToList();
                                    var figsNotNeeded = (from f in Figurants where f.Code != null || f.Name != null select f).ToList();

                                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        progresWindow.CreateListFig(figs);
                                    });
                                    foreach (var item in figs)
                                    {
                                        string path = (from mc in modelContext.@MainConfigs where mc.NumbInput == item.NumbInput select mc.Folder).First();

                                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            progresWindow.SetDounloadFigNow(item);
                                        });

                                        var searchEDRTask = Task.Run(() =>
                                        {
                                            try
                                            {
                                                var dsd = new SearchEDR(cts, item.Ipn, item.Fio, null, 500,
                                                SearchType.Chief, path + $"\\{numberR}. {Reest.abbreviatedName[numberR - 1]}",
                                                progresWindow, item, modelContext, true);
                                                //dsd.CreateExel();
                                                //dsd.CreatePDF();

                                               
                                            }
                                            catch (Exception ex)
                                            {
                                                progresWindow.ErrorFigur(item, ex);
                                                return;
                                            }
                                            

                                        });
                                        await searchEDRTask;

                                        //System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                        //{
                                        //    progresWindow.SetDoneFigNow(item);
                                        //});
                                    }
                                    //foreach (var item in figsNotNeeded)
                                    //{
                                    //    var listC = AllDirectories.GetBoolsFromString(item.Control);
                                    //    var listS = AllDirectories.GetBoolsFromString(item.Shema);

                                    //    listC[numberR - 1] = false;
                                    //    listS[numberR - 1] = true;

                                    //    item.Control = AllDirectories.GetStringFromBools(listC);
                                    //    item.Shema = AllDirectories.GetStringFromBools(listS);
                                    //}
                                }
                            }
                            else if (numberR == 18)
                            {
                                if (Figurants != null)
                                {
                                    var figs = (from f in Figurants where f.Ipn != null || f.Fio != null select f).ToList();
                                    var figsNotNeeded = (from f in Figurants where f.Code != null || f.Name != null select f).ToList();
                                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        progresWindow.CreateListFig(figs);
                                    });
                                    foreach (var item in figs)
                                    {
                                        string path = (from mc in modelContext.@MainConfigs where mc.NumbInput == item.NumbInput select mc.Folder).First();

                                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            progresWindow.SetDounloadFigNow(item);
                                        });

                                        var searchEDRTask = Task.Run(() =>
                                        {
                                            try
                                            {
                                                var dsd = new SearchEDR(cts, item.Ipn, item.Fio, null, 500,
                                                SearchType.Assignee, path + $"\\{numberR}. {Reest.abbreviatedName[numberR - 1]}",
                                                progresWindow, item, modelContext, true);
                                            }
                                            catch (Exception ex)
                                            {
                                                progresWindow.ErrorFigur(item, ex);
                                                return;
                                            }
                                            

                                        });
                                        await searchEDRTask;

                                        //System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                        //{
                                        //    progresWindow.SetDoneFigNow(item);
                                        //});
                                    }
                                    //foreach (var item in figsNotNeeded)
                                    //{
                                    //    var listC = AllDirectories.GetBoolsFromString(item.Control);
                                    //    var listS = AllDirectories.GetBoolsFromString(item.Shema);

                                    //    listC[numberR - 1] = false;
                                    //    listS[numberR - 1] = true;

                                    //    item.Control = AllDirectories.GetStringFromBools(listC);
                                    //    item.Shema = AllDirectories.GetStringFromBools(listS);
                                    //}
                                }
                            }
                            else if (numberR == 19)
                            {
                                if (Figurants != null)
                                {
                                    var figs = (from f in Figurants where f.Code != null || f.Name != null select f).ToList();
                                    var figsNotNeeded = (from f in Figurants where f.Ipn != null || f.Fio != null select f).ToList();

                                    await Task.Run(() =>
                                    {
                                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            progresWindow.CreateListFig(figs);
                                        });
                                    }, ct);

                                    
                                    foreach (var item in figs)
                                    {
                                        string path = (from mc in modelContext.@MainConfigs where mc.NumbInput == item.NumbInput select mc.Folder).First();

                                        await Task.Run(() =>
                                        {
                                            System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                            {
                                                progresWindow.SetDounloadFigNow(item);
                                            });
                                        }, ct);
                                        

                                        var searchEDRTask = Task.Run(() =>
                                        {
                                            try
                                            {
                                                var dsd = new SearchEDR(cts, item.Code, item.Name, null, 500,
                                                SearchType.Base, path + $"\\{numberR}. {Reest.abbreviatedName[numberR - 1]}",
                                                progresWindow, item, modelContext);
                                            }
                                            catch (Exception ex)
                                            {
                                                progresWindow.ErrorFigur(item, ex);
                                                return;
                                            }
                                        }, ct);
                                        await searchEDRTask;

                                        //System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                        //{
                                        //    progresWindow.SetDoneFigNow(item);
                                        //});
                                    }
                                    //foreach (var item in figsNotNeeded)
                                    //{
                                    //    var listC = AllDirectories.GetBoolsFromString(item.Control);
                                    //    var listS = AllDirectories.GetBoolsFromString(item.Shema);

                                    //    listC[numberR - 1] = false;
                                    //    listS[numberR - 1] = true;

                                    //    item.Control = AllDirectories.GetStringFromBools(listC);
                                    //    item.Shema = AllDirectories.GetStringFromBools(listS);
                                    //}
                                }
                            }
                            else if (numberR == 20)
                            {
                                if (Figurants != null)
                                {
                                    var figs = (from f in Figurants where f.Ipn != null || f.Fio != null select f).ToList();
                                    var figsNotNeeded = (from f in Figurants where f.Name != null || f.Name != null select f).ToList();

                                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        progresWindow.CreateListFig(figs);
                                    });

                                    foreach (var item in figs)
                                    {
                                        string path = (from mc in modelContext.@MainConfigs where mc.NumbInput == item.NumbInput select mc.Folder).First();

                                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            progresWindow.SetDounloadFigNow(item);
                                        });


                                        var searchEDRTask = Task.Run(() =>
                                        {
                                            try
                                            {
                                                var dsd = new SearchEDR(cts, item.Ipn, item.Fio, null, 500,
                                                SearchType.Base, path + $"\\{numberR}. {Reest.abbreviatedName[numberR - 1]}",
                                                progresWindow, item, modelContext, true);

                                               
                                            }
                                            catch (Exception ex)
                                            {
                                                progresWindow.ErrorFigur(item, ex);
                                                return;
                                            }
                                        });
                                        await searchEDRTask;

                                    }

                                    //foreach (var item in figsNotNeeded)
                                    //{
                                    //    var listC = AllDirectories.GetBoolsFromString(item.Control);
                                    //    var listS = AllDirectories.GetBoolsFromString(item.Shema);

                                    //    listC[numberR - 1] = false;
                                    //    listS[numberR - 1] = true;

                                    //    item.Control = AllDirectories.GetStringFromBools(listC);
                                    //    item.Shema = AllDirectories.GetStringFromBools(listS);
                                    //}
                                }
                            }
                            else if (numberR == 38)
                            {
                                if (Figurants != null)
                                {
                                    var figs = Figurants;

                                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        progresWindow.CreateListFig(figs);
                                    });

                                    foreach (var item in figs)
                                    {
                                        string path = (from mc in modelContext.@MainConfigs where mc.NumbInput == item.NumbInput select mc.Folder).First();

                                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            progresWindow.SetDounloadFigNow(item);
                                        });


                                        var searchEDRTask = Task.Run(() =>
                                        {
                                            try
                                            {
                                                //var dsd = new SearchEDR(cts, item.Ipn, item.Fio, null, 500,
                                                //SearchType.Base, path + $"\\{numberR}. {Reest.abbreviatedName[numberR - 1]}",
                                                //progresWindow, item, modelContext, true, numberR);
                                                if (item.Ipn != null || item.Fio != null)
                                                {
                                                    var searchRPS = new SearchRPS(item.Fio + "", item.Ipn + "", true, progresWindow, item, numberR, path + $"\\{numberR}. {Reest.abbreviatedName[numberR - 1]}");
                                                }
                                                else
                                                {
                                                    var searchRPS = new SearchRPS(item.Name + "", item.Code + "", false, progresWindow, item, numberR, path + $"\\{numberR}. {Reest.abbreviatedName[numberR - 1]}");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                progresWindow.ErrorFigur(item, ex);
                                                return;
                                            }
                                        });
                                        await searchEDRTask;
                                    }
                                }
                            }
                            else if (numberR == 39)
                            {
                                if (Figurants != null)
                                {
                                    var figs = Figurants;

                                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        progresWindow.CreateListFig(figs);
                                    });

                                    foreach (var item in figs)
                                    {
                                        string path = (from mc in modelContext.@MainConfigs where mc.NumbInput == item.NumbInput select mc.Folder).First();

                                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            progresWindow.SetDounloadFigNow(item);
                                        });

                                        var searchEDRTask = Task.Run(() =>
                                        {
                                            try
                                            {
                                                if (item.Ipn != null || item.Fio != null)
                                                {
                                                    var searchSK = new SearchSK(item.Fio + "", item.Ipn + "", true, progresWindow, item, numberR, path + $"\\{numberR}. {Reest.abbreviatedName[numberR - 1]}");
                                                }
                                                else
                                                {
                                                    var searchSK = new SearchSK(item.Name + "", item.Code + "", false, progresWindow, item, numberR, path + $"\\{numberR}. {Reest.abbreviatedName[numberR - 1]}");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                progresWindow.ErrorFigur(item, ex);
                                                return;
                                            }
                                        });
                                        await searchEDRTask;
                                    }
                                }
                            }
                            else if (numberR == 40)
                            {
                                if (Figurants != null)
                                {
                                    var figs = Figurants;

                                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        progresWindow.CreateListFig(figs);
                                    });

                                    foreach (var item in figs)
                                    {
                                        string path = (from mc in modelContext.@MainConfigs where mc.NumbInput == item.NumbInput select mc.Folder).First();


                                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            progresWindow.SetDounloadFigNow(item);
                                        });

                                        var searchEDRTask = Task.Run(() =>
                                        {
                                            try
                                            {
                                                if (item.Ipn != null || item.Fio != null)
                                                {
                                                    var searchDSRU = new SearchDSRU(item.Fio + "", item.Ipn + "", true, progresWindow, item, numberR, path + $"\\{numberR}. {Reest.abbreviatedName[numberR - 1]}");
                                                }
                                                else
                                                {
                                                    var searchDSRU = new SearchDSRU(item.Name + "", item.Code + "", false, progresWindow, item, numberR, path + $"\\{numberR}. {Reest.abbreviatedName[numberR - 1]}");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                progresWindow.ErrorFigur(item, ex);
                                                return;
                                            }
                                        });
                                        await searchEDRTask;
                                    }
                                }
                            }
                            await Task.Run(() =>
                            {
                                while (true)
                                {
                                    if (progresWindow.GetIsEnd())
                                    {
                                        
                                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            foreach (var item in figsNotNeeded)
                                            {
                                                var listC = AllDirectories.GetBoolsFromString(item.Control);
                                                var listS = AllDirectories.GetBoolsFromString(item.Shema);

                                                listC[numberR - 1] = false;
                                                listS[numberR - 1] = true;

                                                item.Control = AllDirectories.GetStringFromBools(listC);
                                                item.Shema = AllDirectories.GetStringFromBools(listS);
                                            }

                                            //SaveToDB();
                                            progresWindow.CreateFinish();
                                            modelContext.SaveChanges();
                                            CreateNewTree();

                                            if (mainWindow.treeView1.Items[numberR - 1] is StackPanel st)
                                            {
                                                if (st.Children[2] is System.Windows.Controls.Button bnew)
                                                {
                                                    bnew.IsEnabled = false;
                                                }
                                            }

                                        });
                                        break;
                                    }
                                    else
                                    {
                                        Thread.Sleep(500);
                                    }
                                }
                            }, ct);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if(ex.Message != "A task was canceled.")
                    System.Windows.MessageBox.Show(ex.Message);

                cts.Cancel();
            }
        }
        
        private string? СreatePositionFigurantText(int idNumFig)
        {
            if (Figurants != null)
            {
                var w1 = Figurants[idNumFig - 1];

                if (w1 != null)
                {
                    return GetDefInString(w1);
                }
            }
            return null;
        }
        public StackPanel CreatePozFig(int idNumReestr, int idNumFig)
        {
            StackPanel stackPanel = new() 
            {
                Orientation = Orientation.Horizontal
            };

            var checkBox = new CheckBox();
            if (Figurants != null)
            {
                var w1 = Figurants[idNumFig - 1];

                if (w1 != null)
                {
                    if (w1.Control != null)
                    {
                        var w2 = GetBoolsFromString(w1.Control);
                        if (w2 != null)
                        {
                            checkBox.IsChecked = w2[idNumReestr - 1];
                        }
                        checkBox.Click += ClickCheckBoxFigurantYES;
                        checkBox.Tag = new Tuple<int, int, FigYesNo>(idNumFig, idNumReestr, FigYesNo.Yes);
                        checkBox.Foreground = WhiteSolidColorBrush;
                        checkBox.Padding = new Thickness(0, 0, 10, 0);
                    }
                    else
                    {
                        checkBox.IsChecked = false;
                        checkBox.Click += ClickCheckBoxFigurantYES;
                        checkBox.Tag = new Tuple<int, int, FigYesNo>(idNumFig, idNumReestr, FigYesNo.Yes);
                        checkBox.Foreground = WhiteSolidColorBrush;
                        checkBox.Padding = new Thickness(0, 0, 10, 0);
                    }
                }
            }

            var checkBox2 = new CheckBox();
            if (Figurants != null)
            {
                var w1 = Figurants[idNumFig - 1];

                if (w1 != null)
                {
                    if (w1.Shema != null)
                    {
                        var w2 = GetBoolsFromString(w1.Shema);
                        if (w2 != null)
                        {
                            checkBox2.IsChecked = w2[idNumReestr - 1];
                        }
                        checkBox2.Click += ClickCheckBoxFigurantNO;
                        checkBox2.Tag = new Tuple<int, int, FigYesNo>(idNumFig, idNumReestr, FigYesNo.No);
                        checkBox2.Foreground = WhiteSolidColorBrush;
                        //checkBox2.Padding = new Thickness(100, 0, 0, 0);
                    }
                    else
                    {
                        checkBox2.IsChecked = false;
                        checkBox2.Click += ClickCheckBoxFigurantNO;
                        checkBox2.Tag = new Tuple<int, int, FigYesNo>(idNumFig, idNumReestr, FigYesNo.No);
                        checkBox2.Foreground = WhiteSolidColorBrush;
                        checkBox2.Padding = new Thickness(0, 0, 10, 0);
                    }
                }
            }

            
            stackPanel.Children.Add(checkBox);
            stackPanel.Children.Add(checkBox2);



            var listTB = CreateControls(СreatePositionFigurantText(idNumFig));

            foreach (var item in listTB)
            {
                stackPanel.Children.Add(item);
            }


            return stackPanel;
        }

        private void ButtonAutomation_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static List<UIElement> CreateControls(string? str)
        {
            List<UIElement> listRet = new();
            if(str != null)
            {
                var list = str!.Split('^');
                if (list.Length == 6)
                {
                    TextBlock textBlock = new ()
                    {
                        Text = list[0],
                        FontSize = 14,
                        Padding = new Thickness(20, 0, 0, 0),
                        ToolTip = "Натисніть, щоб скопіювати"
                    };
                    textBlock.PreviewMouseDown += (w, r) => {try{System.Windows.Clipboard.SetText(textBlock.Text);}catch (Exception ex) { }};
                    textBlock.MouseEnter += (w, r) => {  textBlock.Opacity = 0.5; };
                    textBlock.MouseLeave += (w, r) => {  textBlock.Opacity = 1; };
                    listRet.Add(textBlock);


                    TextBlock textBlockTemp = new()
                    {
                        Text = list[1],
                        FontSize = 14
                    };
                    listRet.Add(textBlockTemp);



                    TextBlock textBlock1 = new () 
                    {
                        Text = list[2],
                        FontSize = 14,
                        ToolTip = "Натисніть, щоб скопіювати"
                    };
                    textBlock1.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock1.Text); } catch (Exception ex) { } };
                    textBlock1.MouseEnter += (w, r) => { textBlock1.Opacity = 0.5; };
                    textBlock1.MouseLeave += (w, r) => { textBlock1.Opacity = 1; };
                    listRet.Add(textBlock1);

                    TextBlock textBlockTemp2 = new () { Text = list[3], FontSize = 14 };
                    listRet.Add(textBlockTemp2);

                    TextBlock textBlockTemp3 = new () { Text = list[4], FontSize = 14 };
                    listRet.Add(textBlockTemp3);

                    TextBlock textBlock2 = new () { Text = list[5], FontSize = 14, ToolTip = "Натисніть, щоб скопіювати" }; 
                    textBlock2.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock2.Text); } catch (Exception ex) { } };
                    textBlock2.MouseEnter += (w, r) => { textBlock2.Opacity = 0.5; };
                    textBlock2.MouseLeave += (w, r) => { textBlock2.Opacity = 1;};
                    listRet.Add(textBlock2);
                }
                else if(list.Length == 4)
                {
                    TextBlock textBlock = new ()
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


                    TextBlock textBlockTemp = new ()
                    {
                        Text = list[1],
                        FontSize = 14
                    };
                    textBlockTemp.Text = list[1];
                    textBlockTemp.FontSize = 14;
                    listRet.Add(textBlockTemp);



                    TextBlock textBlock1 = new ()
                    {
                        Text = list[2],
                        FontSize = 14,
                        ToolTip = "Натисніть, щоб скопіювати"
                    };
                    textBlock1.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock1.Text); } catch (Exception ex) { } };
                    textBlock1.MouseEnter += (w, r) => { textBlock1.Opacity = 0.5; };
                    textBlock1.MouseLeave += (w, r) => { textBlock1.Opacity = 1; };
                    listRet.Add(textBlock1);

                    TextBlock textBlockTemp2 = new ()
                    {
                        Text = list[3],
                        FontSize = 14,
                    };
                    listRet.Add(textBlockTemp2);
                }
                else if(list.Length == 3)
                {
                    TextBlock textBlock = new ()
                    {
                        Text = list[0],
                        FontSize = 14,
                        Padding = new Thickness(20, 0, 0, 0),
                        ToolTip = "Натисніть, щоб скопіювати"
                    };
                    textBlock.Text = list[0];
                    textBlock.FontSize = 14;
                    textBlock.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock.Text); } catch (Exception ex) { } };
                    textBlock.MouseEnter += (w, r) => { textBlock.Opacity = 0.5; };
                    textBlock.MouseLeave += (w, r) => { textBlock.Opacity = 1; };
                    textBlock.Padding = new Thickness(20, 0, 0, 0);
                    textBlock.ToolTip = "Натисніть, щоб скопіювати";
                    listRet.Add(textBlock);


                    TextBlock textBlockTemp = new ()
                    {
                        Text = list[1],
                        FontSize = 14
                    };
                    textBlockTemp.Text = list[1];
                    textBlockTemp.FontSize = 14;
                    listRet.Add(textBlockTemp);



                    TextBlock textBlock1 = new ()
                    {
                        Text = list[2],
                        FontSize = 14,
                        Padding = new Thickness(20, 0, 0, 0),
                    };
                    textBlock1.PreviewMouseDown += (w, r) => { try { System.Windows.Clipboard.SetText(textBlock1.Text); } catch (Exception ex) { } };
                    textBlock1.MouseEnter += (w, r) => { textBlock1.Opacity = 0.5; };
                    textBlock1.MouseLeave += (w, r) => { textBlock1.Opacity = 1; };
                    listRet.Add(textBlock1);
                }
                else if(list.Length == 1)
                {
                    TextBlock textBlock = new ()
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
        private void DeleteDir(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is MenuItem mi)
                {
                    object tag = mi.Tag;
                    if (tag != null)
                    {
                        if (tag is string nameD)
                        {
                            if (Directory.Exists(@MainConfig.Folder + $"\\{nameD}"))
                            {
                                Directory.Delete(@MainConfig.Folder + $"\\{nameD}", true);

                                int index = Convert.ToInt32(nameD.Split('.').First()) - 1;


                                if (treeView1.Items[index] is StackPanel itemCh)
                                {
                                    if (itemCh.Children[0] is CheckBox che)
                                    {
                                        che.IsChecked = false;
                                    }
                                    if (itemCh.Children[1] is CheckBox che2)
                                    {
                                        che2.IsChecked = false;
                                    }
                                }
                                SaveToDB();
                                CreateNewTree();
                            }
                        }
                    }
                }
                Update();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static List<bool> GetBoolsFromString(string? str)
        {
            if (str == null) return null!;

            List<bool> ret = new ();
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
            while (ret.Count < Reest.abbreviatedName.Count + 1)
            {
                ret.Add(false);
            }
            return ret;
        }
        public static string? GetStringFromBools(List<bool> list)
        {
            if(list == null) return null!;
            var st = "";

            foreach (var item in list)
            {
                st += item ? "1" : "0";
            }
            if(st == "")
                return null!;
            return st;
        }
        public List<string> GetAllAvailableDirectories()
        {
            var newlist = new List<string>();
            for (int i = 0; i < Reest.abbreviatedName.Count; i++)
            {
                if (IsAvailableDirectory(i+1, Reest.abbreviatedName[i]))
                {
                    newlist.Add($"{i + 1}. {Reest.abbreviatedName[i]}");
                }
            }
            return newlist;
        }
        public List<string> GetAllNotAvailableDirectories()
        {
            var newlist = new List<string>();
            for (int i = 0; i < Reest.abbreviatedName.Count; i++)
            {
                if (!IsAvailableDirectory(i + 1, Reest.abbreviatedName[i]))
                {
                    newlist.Add($"{i + 1}. {Reest.abbreviatedName[i]}");
                }
            }
            return newlist;
        }
        public List<List<bool>?> GetFigurantCheckListNO()
        {
            List<List<bool>?> ret = new ();

            List<string>? listStringsFig = null;

            if(Figurants != null)
            listStringsFig = (from f in Figurants select f.Shema).ToList();

            if (listStringsFig != null)
            {
                foreach (var itemStringFig in listStringsFig)
                {
                    var itemBoolsFig = GetBoolsFromString(itemStringFig);
                    ret.Add(itemBoolsFig);
                }
            }

            return ret;
        }
        public List<List<bool>?> GetFigurantCheckListYES()
        {
            List<List<bool>?> ret = new ();

            List<string>? listStringsFig = null;

            if (Figurants != null)
                listStringsFig = (from f in Figurants select f.Control).ToList();

            if (listStringsFig != null)
            {
                foreach (var itemStringFig in listStringsFig)
                {
                    var itemBoolsFig = GetBoolsFromString(itemStringFig);
                    ret.Add(itemBoolsFig);
                }
            }
            return ret;
        }
        public bool IsAvailableDirectory(int id, string abbreviatedName)
        {
            return Directory.Exists($"{@MainConfig.Folder}\\{id}. {abbreviatedName}");
        }
        public bool IsEmptyDirectory(int id, string abbreviatedName)
        {
            return Directory.GetDirectories($"{@MainConfig.Folder}\\{id}. {abbreviatedName}").Length == 0 &&
                Directory.GetFiles($"{@MainConfig.Folder}\\{id}. {abbreviatedName}").Length == 0;
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
        private void ClickCheckBoxFigurantYES(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("ClickCheckBoxFigurantYes");

            if (sender is CheckBox checkBox)
            {
                var idNumReestr = checkBox.Tag;
                if (idNumReestr != null)
                {
                    Tuple<int, int, FigYesNo> tag = (Tuple<int, int, FigYesNo>)idNumReestr;

                    if(tag.Item2 - 1 < Reest.abbreviatedName.Count)
                    {
                        if (!Directory.Exists(@MainConfig.Folder + $"\\{tag.Item2}. {Reest.abbreviatedName[tag.Item2 - 1]}"))
                        {
                            //MessageBox.Show("Yeeees");
                            checkBox.IsChecked = false;
                        }
                        else
                        {
                            var list = GetFigListTupleCheckBoxes(tag.Item2);
                            if (list != null)
                            {
                                var tr = list[tag.Item1];
                                if (tr != null)
                                {
                                    if(tr.Item1 is CheckBox ch && tr.Item2 is CheckBox ch2)
                                    {
                                        if (ch.IsChecked!.Value && ch2.IsChecked!.Value)
                                        {
                                            ch2.IsChecked = false;
                                        }
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        if (!Directory.Exists(@MainConfig.Folder + $"\\{Reest.abbreviatedName.Count + 1}. Схеми"))
                        {
                            //MessageBox.Show("Yeeees");
                            checkBox.IsChecked = false;
                        }
                        else
                        {
                            var list = GetFigListTupleCheckBoxes(tag.Item2);
                            if (list != null)
                            {
                                var tr = list[tag.Item1];
                                if (tr != null)
                                {
                                    if (tr.Item1 is CheckBox ch && tr.Item2 is CheckBox ch2)
                                    {
                                        if (ch.IsChecked!.Value && ch2.IsChecked!.Value)
                                        {
                                            ch2.IsChecked = false;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            Update();
        }
        private void ClickCheckBoxFigurantNO(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("ClickCheckBoxNo");

            if (sender is CheckBox checkBox)
            {
                var idNumReestr = checkBox.Tag;
                if(idNumReestr != null)
                {
                    Tuple<int, int, FigYesNo> tag = (Tuple<int, int, FigYesNo>)idNumReestr;
                    if(Reest.abbreviatedName.Count > tag.Item2 - 1)
                    {
                        if (!Directory.Exists(@MainConfig.Folder + $"\\{tag.Item2}. {Reest.abbreviatedName[tag.Item2 - 1]}"))
                        {
                            checkBox.IsChecked = false;
                        }
                        else
                        {
                            var list = GetFigListTupleCheckBoxes(tag.Item2);
                            if (list != null)
                            {
                                var tr = list[tag.Item1];
                                if (tr != null)
                                {
                                    if (tr.Item1 is CheckBox ch && tr.Item2 is CheckBox ch2)
                                    {
                                        if (ch.IsChecked!.Value && ch2.IsChecked!.Value)
                                        {
                                            ch.IsChecked = false;
                                        }
                                    } 
                                }
                            }

                        }
                    }
                    else
                    {
                        if (!Directory.Exists(@MainConfig.Folder + $"\\{Reest.abbreviatedName.Count + 1}. Схеми"))
                        {
                            checkBox.IsChecked = false;
                        }
                        else
                        {
                            var list = GetFigListTupleCheckBoxes(tag.Item2);
                            if (list != null)
                            {
                                var tr = list[tag.Item1];
                                if (tr != null)
                                {
                                    if (tr.Item1 is CheckBox ch && tr.Item2 is CheckBox ch2)
                                    {
                                        if (ch.IsChecked!.Value && ch2.IsChecked!.Value)
                                        {
                                            ch.IsChecked = false;
                                        }
                                    }
                                }
                            }

                        }
                    }   
                }
            }
            Update();
            
        }
        public void SaveToDB()
        {
            try
            {
                Update();

                if (Figurants != null && listFigurantCheckListCONTROL.Count != 0 && listFigurantCheckListSHEMA.Count != 0)
                for (int i = 0; i < Figurants.Count; i++)
                {
                   
                        if(listFigurantCheckListCONTROL.Count > i)
                        {
                            var listCONTROL = listFigurantCheckListCONTROL[i];
                            if (listCONTROL != null)
                            {
                                Figurants[i].Control = GetStringFromBools(listCONTROL);
                            }
                        }
                        else
                        {

                        }

                        if(listFigurantCheckListSHEMA.Count > i)
                        {
                            var listShema = listFigurantCheckListSHEMA[i];
                            if (listShema != null)
                            {
                                Figurants[i].Shema = GetStringFromBools(listShema);
                            }
                            else
                            {

                            }
                        }
                    
                }
                @MainConfig.Control = GetStringFromBools(listContr);
                @MainConfig.Shema = GetStringFromBools(listSh);
                modelContext.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Update()
        {
            try
            {
                if (Figurants != null)
                {
                    var listBoolControl = new List<List<bool>?>();
                    var listBoolShema = new List<List<bool>?>();
                    foreach (var item in Figurants)
                    {
                        listBoolControl.Add(new List<bool>());
                        listBoolShema.Add(new List<bool>());
                    }
                    var listControl = new List<bool>();
                    var listShema = new List<bool>();

                    // in reestr
                    for (int i = 0; i < treeView1.Items.Count; i++)
                    {
                        if (treeView1.Items[i] is StackPanel stackPanel1)
                        {
                            var treeViewItem = stackPanel1.Children[3] as TreeViewItem;
                            bool isControlNice = true;
                            if (treeViewItem!.Items.Count == 0)
                                isControlNice = false;
                            //in figur
                            for (int iTVI = 0; iTVI < treeViewItem!.Items.Count; iTVI++)
                            {

                                if (treeViewItem.Items[iTVI] is StackPanel treeViewFigStackPanel)
                                {

                                    if (treeViewFigStackPanel.Children[1] is CheckBox treeViewFigShema 
                                        &&
                                        treeViewFigStackPanel.Children[0] is CheckBox treeViewFigControl)
                                    {
                                        var IsCheckedTreeViewFigControl = treeViewFigControl.IsChecked;
                                        if (IsCheckedTreeViewFigControl != null)
                                        {
                                            if (listBoolControl.Count < iTVI)
                                            {
                                                listBoolControl.Add(new List<bool>());
                                            }
                                            listBoolControl[iTVI - 1]?.Add(IsCheckedTreeViewFigControl.Value);
                                        }
                                        else
                                        {
                                            if (listBoolControl.Count < iTVI)
                                            {
                                                listBoolControl.Add(new List<bool>());
                                            }
                                            listBoolControl[iTVI - 1]?.Add(false);
                                        }

                                        var IsCheckedTreeViewFigShema = treeViewFigShema.IsChecked;
                                        if (IsCheckedTreeViewFigShema != null)
                                        {
                                            if (listBoolShema.Count < iTVI)
                                            {
                                                listBoolShema.Add(new List<bool>());
                                            }
                                            listBoolShema[iTVI - 1]!.Add(IsCheckedTreeViewFigShema.Value);
                                        }
                                        else
                                        {
                                            if (listBoolShema.Count < iTVI)
                                            {
                                                listBoolShema.Add(new List<bool>());
                                            }
                                            listBoolShema[iTVI - 1]!.Add(false);
                                        }

                                        if (!listBoolShema[iTVI - 1]!.Last() && !listBoolControl[iTVI - 1]!.Last())
                                        {
                                            isControlNice = false;
                                        }
                                        else
                                        {
                                            //TODO 
                                            for (int indexF = 2; indexF < treeViewFigStackPanel.Children.Count; indexF++)
                                            {
                                                if(treeViewFigStackPanel.Children[indexF] is TextBlock itemF)
                                                {
                                                    itemF.Foreground = Brushes.CornflowerBlue;
                                                }
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    if(iTVI > 0)
                                    {
                                        listBoolControl[iTVI - 1]!.Add(false);
                                        listBoolShema[iTVI - 1]!.Add(false);
                                        isControlNice = false;
                                    }
                                    
                                }

                            }

                            if (stackPanel1.Children[0] is CheckBox checkContr &&
                                stackPanel1.Children[1] is CheckBox checkShema)
                            {
                                if (i < Reest.abbreviatedName.Count)
                                {
                                    if (!Directory.Exists(@MainConfig.Folder + "\\" + $"{i + 1}. " + Reest.abbreviatedName[i]))
                                    {
                                        checkContr.IsChecked = false;
                                        checkShema.IsChecked = false;
                                    }
                                    else
                                        checkContr.IsChecked = isControlNice;
                                }
                                else
                                {
                                    if (!Directory.Exists(@MainConfig.Folder + "\\" + $"{i + 1}. Схеми"))
                                    {
                                        checkContr.IsChecked = false;
                                        checkShema.IsChecked = false;
                                    }
                                    else
                                        checkContr.IsChecked = isControlNice;
                                }
                                
                                listControl.Add(checkContr.IsChecked!.Value);
                            }
                            if (stackPanel1.Children[1] is CheckBox checkShem)
                            {
                                listShema.Add(checkShem.IsChecked!.Value);
                            }
                        }
                    }
                    listFigurantCheckListCONTROL = listBoolControl;
                    listFigurantCheckListSHEMA = listBoolShema;
                    listContr = listControl;
                    listSh = listShema;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine($"StackTrace: {ex.StackTrace}\n\n");

                var trace = new StackTrace(ex, true);

                foreach (var frame in trace.GetFrames())
                {
                    var sb = new StringBuilder();

                    sb.AppendLine($"Файл: {frame.GetFileName()}");
                    sb.AppendLine($"Строка: {frame.GetFileLineNumber()}");
                    sb.AppendLine($"Столбец: {frame.GetFileColumnNumber()}");
                    sb.AppendLine($"Метод: {frame.GetMethod()}");

                    Console.WriteLine(sb);
                }
            }
        }
        public Tuple<bool, bool> IsHaveEmptyCheckBoxes(int idNumReestr)
        {
            bool isEmpty = true;
            bool isEmpty2 = true;
            if (treeView1.Items.Count > idNumReestr - 1)
            {
                if (treeView1.Items[idNumReestr - 1] is CheckBox chCONTROL)
                {
                    if (chCONTROL.Content is CheckBox chSHEMA)
                    {
                        if (chSHEMA.Content is TreeViewItem viewItem)
                        {
                            var listFig = viewItem.Items;
                            if (listFig != null)
                            {
                                foreach (var item in listFig)
                                {
                                    if(item is CheckBox chListFig)
                                    {
                                        var isChListFig = chListFig.IsChecked;
                                        if(isChListFig != null)
                                        {
                                            if (isChListFig.Value)
                                            {
                                                isEmpty = false;
                                            }
                                        }
                                        if(chListFig.Content is CheckBox chIn)
                                        {
                                            var isChInListFig = chIn.IsChecked;
                                            if( isChInListFig != null)
                                            {
                                                if (isChInListFig.Value)
                                                {
                                                    isEmpty2 = false;
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
            return new Tuple<bool, bool>(isEmpty, isEmpty2);
        }
        public bool IsAllFigChecked(Tuple<int, int, FigYesNo> tag)
        {
            if(tag != null)
            {
                var tList = GetFigListTupleCheckBoxes(tag.Item2);

                foreach (var item in tList!)
                {
                    if(!item.Item1!.IsChecked!.Value && !item.Item2!.IsChecked!.Value)
                    {
                        return false;
                    }

                }

                return true;
            }
            return false;
        }
        public CheckBox? GetControlCheckBox(int id)
        {
            return (treeView1.Items[id - 1] as StackPanel).Children[0] as CheckBox;
        }
        public CheckBox? GetShemaCheckBox(int id)
        {
            return (treeView1.Items[id - 1] as StackPanel).Children[1] as CheckBox;
        }
        public TreeViewItem? GetTreeViewItem(int id)
        {
            return (treeView1.Items[id - 1] as StackPanel).Children[3] as TreeViewItem;
        }
        public List<Tuple<CheckBox?, CheckBox?>>? GetFigListTupleCheckBoxes(int id)
        {
            TreeViewItem? treeViewItem = GetTreeViewItem(id);
            List<Tuple<CheckBox?, CheckBox?>> listRet = new();

            if (treeViewItem != null)
            {
                foreach (var item in treeViewItem.Items)
                {
                    if(item is StackPanel stack)
                    {
                        listRet.Add(Tuple.Create(stack.Children[0] as CheckBox, stack.Children[1] as CheckBox));
                    }
                }
                return listRet;
            }
            return null;
        }
    }
}
