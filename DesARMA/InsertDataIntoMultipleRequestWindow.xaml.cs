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
    /// Interaction logic for InsertDataIntoMultipleRequestWindow.xaml
    /// </summary>
    public partial class InsertDataIntoMultipleRequestWindow : Window
    {
        ModelContext modelContext;
        User CurrentUser;
        List<string> listNumbIn;
        public InsertDataIntoMultipleRequestWindow(ModelContext modelContext, User CurrentUser)
        {
            this.modelContext = modelContext;
            this.CurrentUser = CurrentUser;

            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Hide();
            SelectionOfQueriesToInsertWindow selectionOfQueriesToInsertWindow =
                new SelectionOfQueriesToInsertWindow(modelContext, CurrentUser);

            selectionOfQueriesToInsertWindow.ShowDialog();
            this.Show();
            if (selectionOfQueriesToInsertWindow.listNumbIn.Count == 0)
            {
                listNumbIn = selectionOfQueriesToInsertWindow.listNumbIn;
                this.Close();
                //Environment.Exit(0);
            }
        }
        private void Button_Click_Insert(object sender, RoutedEventArgs e)
        {

        }
    }
}
