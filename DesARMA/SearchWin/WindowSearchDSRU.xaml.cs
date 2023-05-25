using DesARMA.Automation;
using System;
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

namespace DesARMA.SearchWin
{
    /// <summary>
    /// Interaction logic for WindowSearchDSRU.xaml
    /// </summary>
    public partial class WindowSearchDSRU : Window
    {
        List<PotentialRecordsDSRU> potentialRecordsOwner = new();
        Grid grid = null!;
        SearchDSRU searchDSRU = null!;
        public WindowSearchDSRU(List<PotentialRecordsDSRU> potentialRecordsOwner, SearchDSRU searchDSRU)
        {
            InitializeComponent();
            this.potentialRecordsOwner = potentialRecordsOwner;
            this.searchDSRU = searchDSRU;
            labelTitle.Content = $"{searchDSRU.FullName}\nУ Державному судновому реєстрі знайдено наступна інформація за критерієм співпадінь з \"{searchDSRU.NameFirst}\". Виберіть варіант, який відповідає Вашому критерію пошуку:";

            CreateTableOwner();
        }
        public void CreateTableOwner()
        {
            grid = new Grid();
            grid.Background = this.Resources["2ColorStyle"] as SolidColorBrush;
            if (potentialRecordsOwner != null)
            {
                scrol1.Content = grid;

                for (int i = 0; i < 8; i++)
                {
                    var col = new ColumnDefinition();

                    grid.ColumnDefinitions.Add(col);
                }

                int index = 0;
                foreach (var item in potentialRecordsOwner)
                {
                    grid.RowDefinitions.Add(new());

                    //0
                    Border borderNumb;
                    {
                        var labelNumb = new TextBlock()
                        {
                            Text = $"{index + 1}",
                            Foreground = this.Resources["WhiteEmpty"] as SolidColorBrush,
                            TextAlignment = TextAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center
                        };
                        borderNumb = new()
                        {
                            CornerRadius = new CornerRadius(10),
                            Child = labelNumb,
                            Background = this.Resources["GreenEmpty"] as SolidColorBrush,
                            Margin = new Thickness(5),
                            Opacity = 0.9
                        };
                        Grid.SetRow(borderNumb, index);
                        Grid.SetColumn(borderNumb, 0);
                    }

                    //1
                    Border borderPibName;
                    {
                        var labelName = new TextBlock()
                        {
                            Text = $"{item.Pib}",
                            Foreground = this.Resources["WhiteEmpty"] as SolidColorBrush,
                            TextAlignment = TextAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            TextWrapping = TextWrapping.Wrap
                        };
                        borderPibName = new()
                        {
                            CornerRadius = new CornerRadius(10),
                            Child = labelName,
                            Background = this.Resources["GreenEmpty"] as SolidColorBrush,
                            Margin = new Thickness(5),
                            Opacity = 0.9
                        };
                        Grid.SetRow(borderPibName, index);
                        Grid.SetColumn(borderPibName, 1);
                    }

                    //2
                    Border borderIpnCode;
                    {
                        var labelName = new TextBlock()
                        {
                            Text = $"{item.Ipn}",
                            Foreground = this.Resources["WhiteEmpty"] as SolidColorBrush,
                            TextAlignment = TextAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            TextWrapping = TextWrapping.Wrap
                        };
                        borderIpnCode = new()
                        {
                            CornerRadius = new CornerRadius(10),
                            Child = labelName,
                            Background = this.Resources["GreenEmpty"] as SolidColorBrush,
                            Margin = new Thickness(5),
                            Opacity = 0.9
                        };
                        Grid.SetRow(borderIpnCode, index);
                        Grid.SetColumn(borderIpnCode, 2);
                    }

                    //3
                    Border borderPassp;
                    {
                        var labelName = new TextBlock()
                        {
                            Text = $"{item.Passp }",
                            Foreground = this.Resources["WhiteEmpty"] as SolidColorBrush,
                            TextAlignment = TextAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            TextWrapping = TextWrapping.Wrap
                        };
                        
                        borderPassp = new()
                        {
                            CornerRadius = new CornerRadius(10),
                            Child = labelName,
                            Background = this.Resources["GreenEmpty"] as SolidColorBrush,
                            Margin = new Thickness(5),
                            Opacity = 0.9
                        };
                        Grid.SetRow(borderPassp, index);
                        Grid.SetColumn(borderPassp, 3);
                    }

                    //4
                    Border borderDt;
                    {
                        var labelName = new TextBlock()
                        {
                            Text = $"{item.Dt}",
                            Foreground = this.Resources["WhiteEmpty"] as SolidColorBrush,
                            TextAlignment = TextAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            TextWrapping = TextWrapping.Wrap
                        };
                        
                        borderDt = new()
                        {
                            CornerRadius = new CornerRadius(10),
                            Child = labelName,
                            Background = this.Resources["GreenEmpty"] as SolidColorBrush,
                            Margin = new Thickness(5),
                            Opacity = 0.9
                        };
                        Grid.SetRow(borderDt, index);
                        Grid.SetColumn(borderDt, 4);
                    }

                    //5
                    Border borderAddr;
                    {
                        var labelName = new TextBlock()
                        {
                            Text = $"{item.Addres}",
                            Foreground = this.Resources["WhiteEmpty"] as SolidColorBrush,
                            TextAlignment = TextAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            TextWrapping = TextWrapping.Wrap
                        };

                        borderAddr = new()
                        {
                            CornerRadius = new CornerRadius(10),
                            Child = labelName,
                            Background = this.Resources["GreenEmpty"] as SolidColorBrush,
                            Margin = new Thickness(5),
                            Opacity = 0.9
                        };
                        Grid.SetRow(borderAddr, index);
                        Grid.SetColumn(borderAddr, 5);
                    }


                    //6
                    Border borderCount;
                    {
                        var labelCount = new TextBlock()
                        {
                            Text = $"{item.Count}",
                            Foreground = this.Resources["WhiteEmpty"] as SolidColorBrush,
                            TextAlignment = TextAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center
                        };

                        borderCount = new()
                        {
                            CornerRadius = new CornerRadius(10),
                            Child = labelCount,
                            Background = this.Resources["GreenEmpty"] as SolidColorBrush,
                            Margin = new Thickness(5),
                            Opacity = 0.9
                        };
                        Grid.SetRow(borderCount, index);
                        Grid.SetColumn(borderCount, 6);
                    }

                    //7
                    Border bordercheckBox;
                    {
                        var checkBox = new CheckBox()
                        {
                            IsChecked = item.isToExtract,
                            Foreground = this.Resources["WhiteEmpty"] as SolidColorBrush,
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            LayoutTransform = new ScaleTransform(2, 2)
                        };
                        
                        bordercheckBox = new()
                        {
                            CornerRadius = new CornerRadius(10),
                            Child = checkBox,
                            Background = this.Resources["GreenEmpty"] as SolidColorBrush,
                            Margin = new Thickness(5),
                            Opacity = 0.9
                        };
                        Grid.SetRow(bordercheckBox, index);
                        Grid.SetColumn(bordercheckBox, 7);
                    }
                   

                    grid.Children.Add(borderNumb);
                    grid.Children.Add(borderPibName);
                    grid.Children.Add(borderIpnCode);

                    grid.Children.Add(borderPassp);
                    grid.Children.Add(borderDt);
                    grid.Children.Add(borderAddr);

                    grid.Children.Add(borderCount);
                    grid.Children.Add(bordercheckBox);

                    index++;
                }
            }

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                searchDSRU.OrderOwner = new();

                for (int row = 0; row < grid.RowDefinitions.Count; row++)
                {
                    var column = 7; // індекс стовпця
                    var cell = grid.Children
                        .Cast<UIElement>()
                        .FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);

                    if (cell is Border border)
                    {
                        if (border.Child is CheckBox chIs)
                        {
                            if (chIs.IsChecked is bool chB && chB)
                            {
                                searchDSRU.OrderOwner.Add(potentialRecordsOwner[row]);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
