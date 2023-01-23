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
        private System.Windows.Forms.Timer inactivityTimer = new System.Windows.Forms.Timer();
        AllDirectories allDirectories { get; set; } = null!;
        UpdateDel updateDel = null!;
        public CreateDelDirWindow(AllDirectories allDirectories, UpdateDel updateDel)
        {
            try
            {
                InitializeComponent();

                this.allDirectories = allDirectories;
                this.updateDel = updateDel;
                ShowButtons();

                string shif = ConfigurationManager.AppSettings["hv"].ToString();
                inactivityTimer.Interval = 60_000 * Convert.ToInt32(shif);
                inactivityTimer.Tick += (sender, args) =>
                {
                    Environment.Exit(0);
                };
                inactivityTimer.Start();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }
        public void ShowButtons()
        {
            
            stackPanel1.Children.Clear();
            var list = allDirectories.GetAllNotAvailableDirectories();
            for (int i = 0; i < list.Count; i++)
            {
                Button b = new Button();
                b.Click += CreateDirectory;
                b.Content = list[i];
                stackPanel1.Children.Add(b);
            }

            
        }
        public void CreateDirectory(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                var curB = sender as Button;
                if (curB != null)
                {
                    Directory.CreateDirectory($"{allDirectories.mainConfig.Folder}\\{curB.Content.ToString()}");
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
                    var curB = stackPanel1.Children[i] as Button;
                    if (curB != null)
                    {
                        Directory.CreateDirectory($"{allDirectories.mainConfig.Folder}\\{curB.Content.ToString()}");
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
    }
}
