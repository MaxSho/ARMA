using DesARMA.Models;
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

namespace DesARMA
{
    /// <summary>
    /// Interaction logic for RequestsWindow.xaml
    /// </summary>
    public partial class RequestsWindow : Window
    {
        string inputNumbet;
        ModelContext modelContext;
        public RequestsWindow(string inputNumbet, ModelContext modelContext)
        {
            InitializeComponent();
            this.modelContext = modelContext;
            this.inputNumbet = inputNumbet;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DMSButton_Click1(object sender, RoutedEventArgs e)
        {
            SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToMytna, "Держмитслужба");
            this.Hide();
            selectFigWindow.ShowDialog();
            this.Show();
        }

        private void UPButton_Click2(object sender, RoutedEventArgs e)
        {
            SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToIntelektualnyi, "Укрпатент");
            this.Hide();
            selectFigWindow.ShowDialog();
            this.Show();
        }

        private void GNButton_Click3(object sender, RoutedEventArgs e)
        {
            SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToHeolohii, "Геонадра");
            this.Hide();
            selectFigWindow.ShowDialog();
            this.Show();
        }

        private void DPButton_Click4(object sender, RoutedEventArgs e)
        {
            //SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToDerzhpratsi, "Держпраці");
            //this.Hide();
            //selectFigWindow.ShowDialog();
            //this.Show();
        }

        private void AMKButton_Click5(object sender, RoutedEventArgs e)
        {
            SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToAntymonopolnyi, "АМК");
            this.Hide();
            selectFigWindow.ShowDialog();
            this.Show();
        }

        private void NKCPFRButton_Click6(object sender, RoutedEventArgs e)
        {
            SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToFondovyi1, "НКЦПФР 1");
            this.Hide();
            selectFigWindow.ShowDialog();
            this.Show();
        }

        private void NKCPFR2Button_Click7(object sender, RoutedEventArgs e)
        {
            SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToFondovyiOsnovnyi2, "НКЦПФР 2");
            this.Hide();
            selectFigWindow.ShowDialog();
            this.Show();
        }

        private void NAZKButton_Click8(object sender, RoutedEventArgs e)
        {
            SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToNAPZK, "НАЗК");
            this.Hide();
            selectFigWindow.ShowDialog();
            this.Show();
        }

        private void BankButton_Click9(object sender, RoutedEventArgs e)
        {
            SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumbet, modelContext, EnumExtReq.ExternalRequestsToBank, "Банки");
            this.Hide();
            selectFigWindow.ShowDialog();
            this.Show();
        }
    }
}
