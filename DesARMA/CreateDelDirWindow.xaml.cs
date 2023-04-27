using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace DesARMA
{
    /// <summary>
    /// Interaction logic for CreateDelDirWindow.xaml
    /// </summary>
    public partial class CreateDelDirWindow : Window
    {
        private readonly System.Windows.Forms.Timer inactivityTimer = new ();
        AllDirectories AllDirectories { get; set; } = null!;
        readonly UpdateDel updateDel = null!;
        public CreateDelDirWindow(AllDirectories allDirectories, UpdateDel updateDel, System.Windows.Forms.Timer inactivityTimer)
        {
            try
            {
                InitializeComponent();

                this.AllDirectories = allDirectories;
                this.updateDel = updateDel;
                this.inactivityTimer = inactivityTimer;
                this.inactivityTimer = inactivityTimer;
                ShowButtons();

                //string shif = ConfigurationManager.AppSettings["hv"].ToString();
                //inactivityTimer.Interval = 60_000 * Convert.ToInt32(shif);
                //inactivityTimer.Tick += (sender, args) =>
                //{
                //    Environment.Exit(0);
                //};

                inactivityTimer.Start();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }
        public void ShowButtons()
        {
            inactivityTimer.Stop();
            try
            {
                stackPanel1.Children.Clear();
                var list = AllDirectories.GetAllNotAvailableDirectories();
                for (int i = 0; i < list.Count; i++)
                {
                    Button b = new ();
                    b.Click += CreateDirectory;
                    b.Content = list[i];
                    stackPanel1.Children.Add(b);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            inactivityTimer.Start();
        }
        public void CreateDirectory(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                if (sender is Button curB)
                {
                    Directory.CreateDirectory($"{AllDirectories.@MainConfig.Folder}\\{curB.Content}");
                    ShowButtons();
                    updateDel(null!, null!);
                    //MessageBox.Show($"Папка {curB.Content.ToString()} створена за розташуванням:\n{allDirectories.mainConfig.Folder}\\{curB.Content.ToString()}");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
            inactivityTimer.Start();
        }
        private void CreateAll(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                for (int i = 0; i < stackPanel1.Children.Count; i++)
                {
                    if (stackPanel1.Children[i] is Button curB)
                    {
                        Directory.CreateDirectory($"{AllDirectories.@MainConfig.Folder}\\{curB.Content}");
                    }
                }
                ShowButtons();
                updateDel(null!, null!);
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
