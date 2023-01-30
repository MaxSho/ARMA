using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        private System.Windows.Forms.Timer inactivityTimer = new System.Windows.Forms.Timer();
        public AuthWindow(System.Windows.Forms.Timer inactivityTimer)
        {
            try
            {
                InitializeComponent();

                this.inactivityTimer = inactivityTimer;
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
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string Password
        {
            get { return passwordBox.Password; }
        }
        public string Login
        {
            get { return loginBox.Text; }
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

        private void ContentChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }
    }
}
