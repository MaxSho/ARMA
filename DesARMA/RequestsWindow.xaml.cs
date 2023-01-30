using DesARMA.Models;
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
    /// Interaction logic for RequestsWindow.xaml
    /// </summary>
    public partial class RequestsWindow : Window
    {
        string inputNumbet;
        ModelContext modelContext;
        private System.Windows.Forms.Timer inactivityTimer = new System.Windows.Forms.Timer();
        public RequestsWindow(string inputNumbet, ModelContext modelContext, System.Windows.Forms.Timer inactivityTimer)
        {
            InitializeComponent();
            this.modelContext = modelContext;
            this.inputNumbet = inputNumbet;
            this.inactivityTimer = inactivityTimer;

            //string shif = ConfigurationManager.AppSettings["hv"].ToString();
            //inactivityTimer.Interval = 60_000 * Convert.ToInt32(shif);
            //inactivityTimer.Tick += (sender, args) =>
            //{
            //    Environment.Exit(0);
            //};
            inactivityTimer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DMSButton_Click1(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToMytna, "Держмитслужба", inactivityTimer);
                this.Hide();
                selectFigWindow.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            inactivityTimer.Start();
        }

        private void UPButton_Click2(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToIntelektualnyi, "Укрпатент", inactivityTimer);
                this.Hide();
                selectFigWindow.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            inactivityTimer.Start();
        }

        private void GNButton_Click3(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToHeolohii, "Геонадра", inactivityTimer);
                this.Hide();
                selectFigWindow.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            inactivityTimer.Start();
        }

        private void DPButton_Click4(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToDerzhpratsi, "Держпраці", inactivityTimer);
                this.Hide();
                selectFigWindow.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            inactivityTimer.Start();
        }

        private void AMKButton_Click5(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToAntymonopolnyi, "АМК", inactivityTimer);
                this.Hide();
                selectFigWindow.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            inactivityTimer.Start();
        }

        private void NKCPFRButton_Click6(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToFondovyi1, "НКЦПФР 1", inactivityTimer);
                this.Hide();
                selectFigWindow.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            inactivityTimer.Start();
        }

        private void NKCPFR2Button_Click7(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToFondovyiOsnovnyi2, "НКЦПФР 2", inactivityTimer);
                this.Hide();
                selectFigWindow.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            inactivityTimer.Start();
        }

        private void NAZKButton_Click8(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToNAPZK, "НАЗК", inactivityTimer);
                this.Hide();
                selectFigWindow.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            inactivityTimer.Start();
        }

        private void BankButton_Click9(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToBank, "Банки", inactivityTimer);
                this.Hide();
                selectFigWindow.ShowDialog();
                this.Show();
            }
            catch(Exception ex)
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
            inactivityTimer.Start();
        }
    }
}
