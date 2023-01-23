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
        public Def_UO_Window(Figurant figurant, bool isConn)
        {
            try 
            { 
                InitializeComponent();
                this.figurant = figurant;

                nameTextBox.Text = figurant.Name;
                codeTextBox.Text = figurant.Code;
                residentTextBox.IsChecked = figurant.ResUr == 2;
                figurant.Status = isConn ? 2 : 1;

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

            if (nameTextBox.Text == "" ||  codeTextBox.Text == "")
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
    }
}
