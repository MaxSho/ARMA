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
        public UO uo = new UO();
        public bool isDelete = false;
        public Def_UO_Window(UO uo, bool isConn)
        {
            InitializeComponent();
            this.uo = uo;

            nameTextBox.Text = uo.name;
            codeTextBox.Text = uo.code;
            residentTextBox.IsChecked = uo.isResid;
            uo.isConnectedPeople = isConn;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            uo.name = nameTextBox.Text;
            uo.code = codeTextBox.Text;
            var chbis = residentTextBox.IsChecked;
            if(chbis!=null)
                uo.isResid = (bool)(chbis);
            else
                uo.isResid = false;

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
