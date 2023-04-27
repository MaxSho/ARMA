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
    /// Interaction logic for Def_UO_Window.xaml
    /// </summary>
    public partial class Def_UO_Window : Window
    {
        public Figurant figurant = new Figurant();
        public bool isDelete = false;
        private System.Windows.Forms.Timer inactivityTimer = new System.Windows.Forms.Timer();
        public Def_UO_Window(Figurant figurant, bool isConn, System.Windows.Forms.Timer inactivityTimer)
        {
            try 
            { 
                InitializeComponent();
                this.figurant = figurant;
                this.inactivityTimer = inactivityTimer;
                nameTextBox.Text = figurant.Name;
                codeTextBox.Text = figurant.Code;
                residentTextBox.IsChecked = figurant.ResUr == 2;
                figurant.Status = isConn ? 2 : 1;

                
                inactivityTimer.Start();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
}

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            figurant.Name = nameTextBox.Text;
            figurant.Code = codeTextBox.Text;
            var chbis = residentTextBox.IsChecked;

            if(chbis!=null)
                figurant.ResUr = chbis.Value ? 2 : 1;
            else
                figurant.ResUr = null;

            if (nameTextBox.Text == "")
            {
                MessageBox.Show("Не заповнені поля");
            }
            else
            {
                this.DialogResult = true;
            }
            inactivityTimer.Start();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            isDelete = true;
            this.DialogResult = true;
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

        private void nameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }
    }
}
