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
        public RequestsWindow(string inputNumbet, ModelContext modelContext)
        {
            InitializeComponent();
            this.modelContext = modelContext;
            this.inputNumbet = inputNumbet;

            string shif = ConfigurationManager.AppSettings["hv"].ToString();
            inactivityTimer.Interval = 60_000 * Convert.ToInt32(shif);
            inactivityTimer.Tick += (sender, args) =>
            {
                Environment.Exit(0);
            };
            inactivityTimer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DMSButton_Click1(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToMytna, "Держмитслужба");
            this.Hide();
            selectFigWindow.ShowDialog();
            this.Show();
            inactivityTimer.Start();
        }

        private void UPButton_Click2(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToIntelektualnyi, "Укрпатент");
            this.Hide();
            selectFigWindow.ShowDialog();
            this.Show();
            inactivityTimer.Start();
        }

        private void GNButton_Click3(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToHeolohii, "Геонадра");
            this.Hide();
            selectFigWindow.ShowDialog();
            this.Show();
            inactivityTimer.Start();
        }

        private void DPButton_Click4(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToDerzhpratsi, "Держпраці");
            this.Hide();
            selectFigWindow.ShowDialog();
            this.Show();
            inactivityTimer.Start();
        }

        private void AMKButton_Click5(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToAntymonopolnyi, "АМК");
            this.Hide();
            selectFigWindow.ShowDialog();
            this.Show();
            inactivityTimer.Start();
        }

        private void NKCPFRButton_Click6(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToFondovyi1, "НКЦПФР 1");
            this.Hide();
            selectFigWindow.ShowDialog();
            this.Show();
            inactivityTimer.Start();
        }

        private void NKCPFR2Button_Click7(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToFondovyiOsnovnyi2, "НКЦПФР 2");
            this.Hide();
            selectFigWindow.ShowDialog();
            this.Show();
            inactivityTimer.Start();
        }

        private void NAZKButton_Click8(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToNAPZK, "НАЗК");
            this.Hide();
            selectFigWindow.ShowDialog();
            this.Show();
            inactivityTimer.Start();
        }

        private void BankButton_Click9(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToBank, "Банки");
            this.Hide();
            selectFigWindow.ShowDialog();
            this.Show();
            inactivityTimer.Start();
        }
    }
}
