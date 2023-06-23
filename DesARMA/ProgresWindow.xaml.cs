using DesARMA.Automation;
using DesARMA.Models;
using ICSharpCode.SharpZipLib.Zip;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAnimatedGif;

namespace DesARMA
{
    /// <summary>
    /// Interaction logic for ProgresWindow.xaml
    /// </summary>
    public partial class ProgresWindow : Window
    {
        public List<Figurant> figurants = new List<Figurant>();
        Grid grid = new Grid();
        bool isDone = true;
        List<bool> listStatus = new List<bool>();
        List<bool> listCheck = new List<bool>();
        MainWindow mainWindow;
        int number;
        CancellationToken ct;
        CancellationTokenSource cts;
        public ProgresWindow(MainWindow mainWindow, int number, CancellationTokenSource cts)
        {
            InitializeComponent();
            Loaded += ProgresWindow_Loaded;

            this.mainWindow = mainWindow;
            //CreateFigurTable();
            this.tit.Content = "Пошук";
            this.number = number;
            
            this.cts = cts;
            this.ct = cts.Token;

            ((App)Application.Current).ChildWindows.Add(this);
        }
        public bool GetIsEnd()
        {
            bool isEnd = true;
            foreach (var l in listCheck) 
            {
                isEnd &= l;
            }
            return isEnd;
        }
        public void CreateFigurTable()
        {
            grid = new Grid();

            var col0 = new ColumnDefinition();
            col0.Width = GridLength.Auto;
            var col2 = new ColumnDefinition();
            col2.Width = GridLength.Auto;

            grid.ColumnDefinitions.Add(col0);
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(col2);

            foreach (var item in figurants)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }

            foreach (var item in figurants)
            {
                int i = figurants.IndexOf(item);

                var labelNumb = new TextBlock()
                {
                    Text = $"{i + 1}",
                    Foreground = this.Resources["WhiteEmpty"] as SolidColorBrush,
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                var borderNumb = new Border()
                {
                    CornerRadius = new CornerRadius(10),
                    Child = labelNumb,
                    Background = this.Resources["GreenEmpty"] as SolidColorBrush,
                    Margin = new Thickness(5),
                    Opacity = 0.9
                };
                Grid.SetRow(borderNumb, i);
                Grid.SetColumn(borderNumb, 0);


                var labelName = new TextBlock()
                {
                    Text = GetDefInString(item),
                    Foreground = this.Resources["WhiteEmpty"] as SolidColorBrush,
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap
                };

                var borderName = new Border()
                {
                    CornerRadius = new CornerRadius(10),
                    Child = labelName,
                    Background = this.Resources["GreenEmpty"] as SolidColorBrush,
                    Margin = new Thickness(5),
                    Opacity = 0.9
                };
                Grid.SetRow(borderName, i);
                Grid.SetColumn(borderName, 1);

                //System.Windows.Controls.Image myImage = new();
                //BitmapImage bitmap = new BitmapImage(new Uri($"pack://application:,,,/DesARMA;component/Drawings/download/download{(i+1 == 4 ? 6 : i+1)}.gif"));
                //myImage.Source = bitmap;
                //myImage.Width = 100;
                //ImageBehavior.SetAnimatedSource(myImage, bitmap);
                System.Windows.Controls.Image myImage = new();
                BitmapImage bitmap;
                //if (i % 2 == 0)
                //{
                //    bitmap = new BitmapImage(new Uri($"pack://application:,,,/DesARMA;component/Drawings/download1/download1{1}.png"));
                //}
                //else
                {
                    bitmap = new BitmapImage(new Uri($"pack://application:,,,/DesARMA;component/Drawings/download/download{6}.gif"));
                }

                myImage.Source = bitmap;
                myImage.Width = 50;
                myImage.Opacity = 0;
                ImageBehavior.SetAnimatedSource(myImage, bitmap);

                Grid.SetRow(myImage, i);
                Grid.SetColumn(myImage, 2);

                grid.Children.Add(borderNumb);
                grid.Children.Add(borderName);
                grid.Children.Add(myImage);
            }
            stack1.Children.Add(grid);
        }
        static public string? GetDefInString(Figurant d)
        {
            if (d.ResFiz != null)
            {
                string cont = $"РНОКПП {d.Ipn}, {d.Fio}";
                if (d.Fio == null)
                    cont = $"РНОКПП {d.Ipn}";
                if(d.Ipn == null)
                    cont = $"{d.Fio}";

                //return cont.Length > 60 ? cont.Substring(0, 60) + "..." : cont;
                return cont ;
            }
            else
            {
                string cont = $"ЄДРПОУ {d.Code}, {d.Name}";
                if (d.Name == null)
                    cont = $"ЄДРПОУ {d.Code}";
                if (d.Code == null)
                    cont = $"{d.Name}";

                //return cont.Length > 60 ? cont.Substring(0, 60) + "..." : cont;
                return cont;
            }
        }
        public async void CreateFinish()
        {
            await Task.Run(() =>
            {
                stack1.Dispatcher.Invoke(() => {
                    var b = new Button();
                    b.Click += (s, e) => {
                        //mainWindow.IsEnabled = ! mainWindow.IsEnabled;
                        if (mainWindow.treeView1.Items[number - 1] is StackPanel st)
                        {
                            if (st.Children[2] is Button b)
                            {
                                b.IsEnabled = true;
                            }
                        }
                        this.Close();
                    };
                    b.Content = "Закрити";
                    b.Opacity = 0.9;
                    stack1.Children.Add(b);



                    if (isDone)
                    {
                        bool isAllNotData = true;
                        bool isAllData = true;
                        foreach (var item in listStatus)
                        {
                            isAllNotData &= !item;
                            isAllData &= item;
                        }

                        if (isAllNotData)
                        {
                            var bi = new BitmapImage(new Uri($"pack://application:,,,/DesARMA;component/Drawings/notdone/notdone{1}.png"));
                            imageTop.Source = bi;
                            ImageBehavior.SetAnimatedSource(imageTop, bi);

                            var labelName = new TextBlock()
                            {
                                Text = "Процес пройшов успішно. Не знайдено даних ні про одного фігуранта",
                                Foreground = this.Resources["WhiteEmpty"] as SolidColorBrush,
                                TextAlignment = TextAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                TextWrapping = TextWrapping.Wrap
                            };

                            var borderName = new Border()
                            {
                                CornerRadius = new CornerRadius(10),
                                Child = labelName,
                                Background = this.Resources["GreenEmpty"] as SolidColorBrush,
                                Margin = new Thickness(5),
                                Opacity = 0.9
                            };
                            Grid.SetRow(borderName, 2);
                            Grid.SetColumnSpan(borderName, 2);
                            gridTop.Children.Add(borderName);
                        }
                        else if (isAllData)
                        {
                            var bi = new BitmapImage(new Uri($"pack://application:,,,/DesARMA;component/Drawings/done/done{2}.png"));
                            imageTop.Source = bi;
                            ImageBehavior.SetAnimatedSource(imageTop, bi);

                            var labelName = new TextBlock()
                            {
                                Text = "Процес пройшов успішно",
                                Foreground = this.Resources["WhiteEmpty"] as SolidColorBrush,
                                TextAlignment = TextAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                TextWrapping = TextWrapping.Wrap
                            };

                            var borderName = new Border()
                            {
                                CornerRadius = new CornerRadius(10),
                                Child = labelName,
                                Background = this.Resources["GreenEmpty"] as SolidColorBrush,
                                Margin = new Thickness(5),
                                Opacity = 0.9
                            };
                            Grid.SetRow(borderName, 2);
                            Grid.SetColumnSpan(borderName, 2);
                            gridTop.Children.Add(borderName);
                        }
                        else
                        {
                            var bi = new BitmapImage(new Uri($"pack://application:,,,/DesARMA;component/Drawings/done/done{2}.png"));
                            imageTop.Source = bi;
                            ImageBehavior.SetAnimatedSource(imageTop, bi);

                            var labelName = new TextBlock()
                            {
                                Text = "Процес пройшов успішно. Наявні фігуранти, по яким не знайдено дані",
                                Foreground = this.Resources["WhiteEmpty"] as SolidColorBrush,
                                TextAlignment = TextAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                TextWrapping = TextWrapping.Wrap
                            };

                            var borderName = new Border()
                            {
                                CornerRadius = new CornerRadius(10),
                                Child = labelName,
                                Background = this.Resources["GreenEmpty"] as SolidColorBrush,
                                Margin = new Thickness(5),
                                Opacity = 0.9
                            };
                            Grid.SetRow(borderName, 2);
                            Grid.SetColumnSpan(borderName, 2);
                            gridTop.Children.Add(borderName);
                        }
                    }
                    else
                    {
                        var bi = new BitmapImage(new Uri($"pack://application:,,,/DesARMA;component/Drawings/error/error{1}.png"));
                        imageTop.Source = bi;
                        ImageBehavior.SetAnimatedSource(imageTop, bi);

                        var labelName = new TextBlock()
                        {
                            Text = "Процес закінчився з помилками, наведіть на значок для детальної інформації",
                            Foreground = this.Resources["2ColorStyle"] as SolidColorBrush,
                            TextAlignment = TextAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            TextWrapping = TextWrapping.Wrap
                        };

                        var borderName = new Border()
                        {
                            CornerRadius = new CornerRadius(10),
                            Child = labelName,
                            Background = this.Resources["YellowEmpty"] as SolidColorBrush,
                            Margin = new Thickness(5),
                            Opacity = 0.9
                        };
                        Grid.SetRow(borderName, 2);
                        Grid.SetColumnSpan(borderName, 2);
                        gridTop.Children.Add(borderName);
                    }

                });
            }, ct).ContinueWith(_ =>
                            {
                                Thread.Sleep(5000);
                                this.Dispatcher.Invoke(() =>
                                {
                                    this.Close();
                                });
                                
                                


                            });
            //await Task.Run(() =>
            //{




            //}, ct);


        }
        public async void ClosedMe5secund()
        {
            await Task.Run(
                () => {
                    System.Windows.Application.Current.Dispatcher.Invoke(() => {
                        Thread.Sleep(20000);
                        this.Close();
                    });
                },
                ct
                );
            
        }
        public void CreateTitle(string title)
        {
            tit.Content = title;
        }
        public void CreateListFig(List<Figurant> figurants)
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.figurants = figurants;

                    for (int i = 0; i < figurants.Count; i++)
                    {
                        listStatus.Add(true);
                    }
                    for (int i = 0; i < figurants.Count; i++)
                    {
                        listCheck.Add(false);
                    }

                    CreateFigurTable();
                });
            }, ct);
            
        }
        public void SetDounloadFigNow(Figurant figurant)
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var figInd = figurants.IndexOf(figurant);
                    if (figInd != -1)
                    {
                        if (grid.Children
                                 .Cast<UIElement>()
                                 .FirstOrDefault(e => Grid.GetRow(e) == figInd && Grid.GetColumn(e) == 2) is System.Windows.Controls.Image image)
                        {
                            var b = new BitmapImage(new Uri($"pack://application:,,,/DesARMA;component/Drawings/download/download{1}.gif"));
                            image.Opacity = 0.9;
                            image.Source = b;
                            ImageBehavior.SetAnimatedSource(image, b);
                        }
                    }
                });
            }, ct);
           
        }
        public void SetDoneFigNow(Figurant figurant, bool isDontHaveFIO = false)
        {
            if (listStatus[figurants.IndexOf(figurant)])
            {
                Task.Run(() =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var figInd = figurants.IndexOf(figurant);
                        if (figInd != -1)
                        {
                            if (grid.Children
                                     .Cast<UIElement>()
                                     .FirstOrDefault(e => Grid.GetRow(e) == figInd && Grid.GetColumn(e) == 2) is System.Windows.Controls.Image image)
                            {
                                var b = new BitmapImage(new Uri($"pack://application:,,,/DesARMA;component/Drawings/done/done{3}.png"));

                                if (isDontHaveFIO)
                                {
                                    b = new BitmapImage(new Uri($"pack://application:,,,/DesARMA;component/Drawings/done/doneRed4.png"));
                                }
                                image.Source = b;
                                ImageBehavior.SetAnimatedSource(image, b);
                                listCheck[figInd] = true;
                            }
                        }
                    });
                }, ct);
            }
        }
        public void ErrorFigur(Figurant figurant, Exception ex)
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var figInd = figurants.IndexOf(figurant);
                    if (figInd != -1)
                    {
                        System.Windows.Controls.Image? image = null;
                        image = grid.Children
                                        .Cast<UIElement>()
                                        .FirstOrDefault(e => Grid.GetRow(e) == figInd && Grid.GetColumn(e) == 2) as System.Windows.Controls.Image;
                        if (image != null)
                        {
                            var b = new BitmapImage(new Uri($"pack://application:,,,/DesARMA;component/Drawings/error/error{1}.png"));
                            image.Source = b;
                            image.ToolTip = $"Помилка: {ex}";
                            ImageBehavior.SetAnimatedSource(image, b);
                            listStatus[figurants.IndexOf(figurant)] = false;
                            listCheck[figInd] = true;
                            isDone = false;
                        }
                    }
                });
            }, ct);
            
        }
        public void NotDataFigur(Figurant figurant)
        {
            var figInd = figurants.IndexOf(figurant);
            if (figInd != -1)
            {
                System.Windows.Controls.Image? image = null;
                Task.Run(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        image = grid.Children
                                    .Cast<UIElement>()
                                    .FirstOrDefault(e => Grid.GetRow(e) == figInd && Grid.GetColumn(e) == 2) as System.Windows.Controls.Image;
                        var b = new BitmapImage(new Uri($"pack://application:,,,/DesARMA;component/Drawings/notdone/notdone{1}.png"));
                        image.Source = b;
                        ImageBehavior.SetAnimatedSource(image, b);
                        listStatus[figurants.IndexOf(figurant)] = false;
                        listCheck[figInd] = true;
                    });
                }, ct);
                

                //if (image != null)
                //{
                //    Task.Run(() =>
                //    {
                //        Dispatcher.Invoke(() =>
                //        {
                            
                //        });
                //    }, ct);
                    
                //}
            }
        }
        private void ProgresWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Show();

        }

        private void ProgresWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cts.Cancel();

        }
        private void ProgressWindow_Deactivated(object sender, EventArgs e)
        {
            //// Приховуємо вікно при деактивації
            //Hide();
        }

        private void ProgressWindow_Activated(object sender, EventArgs e)
        {
            //// Показуємо вікно та приводимо його на передній план при активації
            //ShowDialog();
            //Activate();
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow.treeView1.Items[number - 1] is StackPanel st)
            {
                if (st.Children[2] is Button b)
                {
                    b.IsEnabled = true;
                }
            }
            this.Close();
        }
    }
}
