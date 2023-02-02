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

namespace DesARMA.CombinedResponseWindows
{
    /// <summary>
    /// Interaction logic for SelectionOfCombinedQueryFieldsWindow.xaml
    /// </summary>
    public partial class SelectionOfCombinedQueryFieldsWindow : Window
    {
        public List<string> listNumIn;
        ModelContext modelContext;
        Main main;
        public SelectionOfCombinedQueryFieldsWindow(ModelContext modelContext, Main main, List<string> listNumIn)
        {
            InitializeComponent();

            this.listNumIn = listNumIn;
            this.modelContext = modelContext;
            this.main = main;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = true;
                //CombinedResponseWindows.EntryOfPersonsInvolvedInTheCombinedRegistersWindow 
                //    entryOfPersonsInvolvedInTheCombinedRegistersWindow
                //    = new CombinedResponseWindows.EntryOfPersonsInvolvedInTheCombinedRegistersWindow(modelContext, main, listNumIn);


                ////this.Hide();
                //this.Visibility = Visibility.Hidden;
                //if (entryOfPersonsInvolvedInTheCombinedRegistersWindow.ShowDialog() == true)
                //{
                //    this.DialogResult = true;
                //    //this.Close();
                //}
                //else
                //{
                //    this.Visibility = Visibility.Visible;
                //}

                ////this.Hide();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
