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
    /// 

    public enum TypeOfAppeal
    {
        NotСombined,
        Сombined
    }
    public partial class RequestsWindow : Window
    {
        List<string> inputNumberList;
        ModelContext modelContext;
        private System.Windows.Forms.Timer inactivityTimer = new System.Windows.Forms.Timer();
        TypeOfAppeal typeOfAppeal;
        public RequestsWindow(List<string> inputNumberList, ModelContext modelContext, TypeOfAppeal typeOfAppeal,
            System.Windows.Forms.Timer inactivityTimer
            )
        {
            InitializeComponent();
            this.modelContext = modelContext;
            this.inputNumberList = inputNumberList;
            this.inactivityTimer = inactivityTimer;
            this.typeOfAppeal = typeOfAppeal;

            if(typeOfAppeal == TypeOfAppeal.Сombined)
            {
                this.Title = "Запити Об'єднаної відповіді";
            }
            //string shif = ConfigurationManager.AppSettings["hv"].ToString();
            //inactivityTimer.Interval = 60_000 * Convert.ToInt32(shif);
            //inactivityTimer.Tick += (sender, args) =>
            //{
            //    Environment.Exit(0);
            //};
            inactivityTimer.Start();
        }
        private void ButtonClick(EnumExtReq enumExtReq, string title)
        {
            SelectFigWindow selectFigWindow = new SelectFigWindow(inputNumberList, modelContext, enumExtReq, title, typeOfAppeal, inactivityTimer);
            this.Hide();
            selectFigWindow.ShowDialog();
            this.Show();
        }
        private void DMSButton_Click1(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                ButtonClick(EnumExtReq.ExternalRequestsToMytna, "Держмитслужба");
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
                ButtonClick(EnumExtReq.ExternalRequestsToIntelektualnyi, "Укрпатент");
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
                ButtonClick(EnumExtReq.ExternalRequestsToHeolohii, "Геонадра");
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
                ButtonClick(EnumExtReq.ExternalRequestsToDerzhpratsi, "Держпраці");
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
                ButtonClick(EnumExtReq.ExternalRequestsToAntymonopolnyi, "АМК");
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
                ButtonClick(EnumExtReq.ExternalRequestsToFondovyi1, "НКЦПФР 1");
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
                ButtonClick(EnumExtReq.ExternalRequestsToFondovyiOsnovnyi2, "НКЦПФР 2");
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
                ButtonClick(EnumExtReq.ExternalRequestsToNAPZK, "НАЗК");
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
                ButtonClick(EnumExtReq.ExternalRequestsToBank, "Банки");
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
