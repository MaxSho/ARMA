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
    /// Interaction logic for Def_UO_Window.xaml
    /// </summary>
    public partial class Def_UO_Window : Window
    {
        public Figurant figurant = new Figurant();
        public bool isDelete = false;
        public Def_UO_Window(Figurant figurant, bool isConn)
        {
            InitializeComponent();
            this.figurant = figurant;

            nameTextBox.Text = figurant.Name;
            codeTextBox.Text = figurant.Code;
            residentTextBox.IsChecked = figurant.ResUr == null ? false: figurant.ResUr;
            figurant.Status = isConn;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            figurant.Name = nameTextBox.Text;
            figurant.Code = codeTextBox.Text;
            var chbis = residentTextBox.IsChecked;
            if(chbis!=null)
                figurant.ResUr = (bool)(chbis);
            else
                figurant.ResUr = false;

            if (nameTextBox.Text == "" ||  codeTextBox.Text == "")
            {
                MessageBox.Show("Не заповнені поля");
            }
            else
            {
                this.DialogResult = true;
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            isDelete = true;
            this.DialogResult = true;
        }
    }
}
