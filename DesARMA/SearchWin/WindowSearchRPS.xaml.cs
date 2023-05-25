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
    /// Interaction logic for WindowSearchRPS.xaml
    /// </summary>
    public partial class WindowSearchRPS : Window
    {
        List<PotentialRecords> potentialRecordsOwner = new();
        List<PotentialRecords> potentialRecordsOperator = new();
        Grid grid = null!;
        Grid grid2 = null!;
        SearchRPS searchRPS = null!;
        public WindowSearchRPS(List<PotentialRecords> potentialRecordsOwner, List<PotentialRecords> potentialRecordsOperator, SearchRPS searchRPS)
        {
            try
            {
                InitializeComponent();
                this.potentialRecordsOwner = potentialRecordsOwner;
                this.potentialRecordsOperator = potentialRecordsOperator;

                labelTitle.Content = $"{searchRPS.FullName}\nУ Державному реєстрі цивільних повітряних суден України знайдено наступна інформація за критерієм співпадінь з \"{searchRPS.NameFirst}\". Виберіть варіант, який відповідає Вашому критерію пошуку:";

                CreateTableOwner();
                CreateTableOperator();
                this.searchRPS = searchRPS;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void CreateTableOwner()
        {
            grid = new Grid();
            grid.Background = this.Resources["2ColorStyle"] as SolidColorBrush;
            if (potentialRecordsOwner != null)
            {
                scrol1.Content = grid;

                for (int i = 0; i < 4; i++)
                {
                    var col = new ColumnDefinition();
                    //col.Width = GridLength.Auto;

                    grid.ColumnDefinitions.Add(col);
                }
               
                int index = 0;
                foreach (var item in potentialRecordsOwner)
                {
                    grid.RowDefinitions.Add(new());

                    var labelNumb = new TextBlock()
                    {
                        Text = $"{index + 1}",
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
                    Grid.SetRow(borderNumb, index);
                    Grid.SetColumn(borderNumb, 0);

                    var labelName = new TextBlock()
                    {
                        Text = $"{item.Name}",
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
                    Grid.SetRow(borderName, index);
                    Grid.SetColumn(borderName, 1);

                    var labelCount = new TextBlock()
                    {
                        Text = $"{item.Count}",
                        Foreground = this.Resources["WhiteEmpty"] as SolidColorBrush,
                        TextAlignment = TextAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    var borderCount = new Border()
                    {
                        CornerRadius = new CornerRadius(10),
                        Child = labelCount,
                        Background = this.Resources["GreenEmpty"] as SolidColorBrush,
                        Margin = new Thickness(5),
                        Opacity = 0.9
                    };
                    Grid.SetRow(borderCount, index);
                    Grid.SetColumn(borderCount, 2);

                    var checkBox = new CheckBox()
                    {
                        IsChecked = item.isToExtract,
                        Foreground = this.Resources["WhiteEmpty"] as SolidColorBrush,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        LayoutTransform = new ScaleTransform(2, 2)
                    };
                    var bordercheckBox = new Border()
                    {
                        CornerRadius = new CornerRadius(10),
                        Child = checkBox,
                        Background = this.Resources["GreenEmpty"] as SolidColorBrush,
                        Margin = new Thickness(5),
                        Opacity = 0.9
                    };
                    Grid.SetRow(bordercheckBox, index);
                    Grid.SetColumn(bordercheckBox, 3);

                    grid.Children.Add(borderNumb);
                    grid.Children.Add(borderName);
                    grid.Children.Add(borderCount);
                    grid.Children.Add(bordercheckBox);

                    index++;
                }
            }
            
        }
        public void CreateTableOperator()
        {
            grid2 = new Grid();
            grid2.Background = this.Resources["2ColorStyle"] as SolidColorBrush;
            if (potentialRecordsOperator != null)
            {
                scrol2.Content = grid2;

                for (int i = 0; i < 4; i++)
                {
                    var col = new ColumnDefinition();
                    //col.Width = GridLength.Auto;

                    grid2.ColumnDefinitions.Add(col);
                }

                int index = 0;
                foreach (var item in potentialRecordsOperator)
                {
                    grid2.RowDefinitions.Add(new());

                    var labelNumb = new TextBlock()
                    {
                        Text = $"{index + 1}",
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
                    Grid.SetRow(borderNumb, index);
                    Grid.SetColumn(borderNumb, 0);

                    var labelName = new TextBlock()
                    {
                        Text = $"{item.Name}",
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
                    Grid.SetRow(borderName, index);
                    Grid.SetColumn(borderName, 1);

                    var labelCount = new TextBlock()
                    {
                        Text = $"{item.Count}",
                        Foreground = this.Resources["WhiteEmpty"] as SolidColorBrush,
                        TextAlignment = TextAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    var borderCount = new Border()
                    {
                        CornerRadius = new CornerRadius(10),
                        Child = labelCount,
                        Background = this.Resources["GreenEmpty"] as SolidColorBrush,
                        Margin = new Thickness(5),
                        Opacity = 0.9
                    };
                    Grid.SetRow(borderCount, index);
                    Grid.SetColumn(borderCount, 2);

                    var checkBox = new CheckBox()
                    {
                        IsChecked = item.isToExtract,
                        Foreground = this.Resources["WhiteEmpty"] as SolidColorBrush,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        LayoutTransform = new ScaleTransform(2, 2)
                    };
                    var bordercheckBox = new Border()
                    {
                        CornerRadius = new CornerRadius(10),
                        Child = checkBox,
                        Background = this.Resources["GreenEmpty"] as SolidColorBrush,
                        Margin = new Thickness(5),
                        Opacity = 0.9
                    };
                    Grid.SetRow(bordercheckBox, index);
                    Grid.SetColumn(bordercheckBox, 3);

                    grid2.Children.Add(borderNumb);
                    grid2.Children.Add(borderName);
                    grid2.Children.Add(borderCount);
                    grid2.Children.Add(bordercheckBox);

                    index++;
                }
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                searchRPS.OrderOwner = new();
                searchRPS.OrderOperator = new();

                for (int row = 0; row < grid.RowDefinitions.Count; row++)
                {
                    var column = 3; // індекс стовпця
                    var cell = grid.Children
                        .Cast<UIElement>()
                        .FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);

                    if (cell is Border border)
                    {
                        if(border.Child is CheckBox chIs)
                        {
                            if (chIs.IsChecked is bool chB && chB)
                            {
                                searchRPS.OrderOwner.Add(potentialRecordsOwner[row]);
                            }
                        }
                        
                    }
                }
                for (int row = 0; row < grid2.RowDefinitions.Count; row++)
                {
                    var column = 3; // індекс стовпця
                    var cell = grid.Children
                        .Cast<UIElement>()
                        .FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
                    if (cell is Border border)
                    {
                        if (border.Child is CheckBox chIs)
                        {
                            if (chIs.IsChecked is bool chB && chB)
                            {
                                searchRPS.OrderOperator.Add(potentialRecordsOperator[row]);
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

        private void Window_Closed(object sender, EventArgs e)
        {
            //try
            //{
            //    searchRPS.OrderOwner = new();
            //    searchRPS.OrderOperator = new();

            //    for (int row = 0; row < grid.RowDefinitions.Count; row++)
            //    {
            //        var column = 3; // індекс стовпця
            //        var cell = grid.Children
            //            .Cast<UIElement>()
            //            .FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);

            //        if (cell is CheckBox chIs)
            //        {
            //            if (chIs.IsChecked is bool chB && chB)
            //            {
            //                searchRPS.OrderOwner.Add(potentialRecordsOwner[row]);
            //            }
            //        }
            //    }
            //    for (int row = 0; row < grid2.RowDefinitions.Count; row++)
            //    {
            //        var column = 3; // індекс стовпця
            //        var cell = grid.Children
            //            .Cast<UIElement>()
            //            .FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);

            //        if (cell is CheckBox chIs)
            //        {
            //            if (chIs.IsChecked is bool chB && chB)
            //            {
            //                searchRPS.OrderOperator.Add(potentialRecordsOperator[row]);
            //            }
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
    }


}
