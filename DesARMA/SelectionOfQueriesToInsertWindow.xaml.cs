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
    /// Interaction logic for SelectionOfQueriesToInsertWindow.xaml
    /// </summary>
    public partial class SelectionOfQueriesToInsertWindow : Window
    {
        public List<string> listNumbIn = new List<string>();
        ModelContext modelContext;
        public User CurrentUser { get; set; } = null!;
        public SelectionOfQueriesToInsertWindow(ModelContext modelContext, User CurrentUser)
        {
            InitializeComponent();

            this.modelContext = modelContext;
            this.CurrentUser = CurrentUser;

            var mains = (from b in modelContext.Mains
                         where b.Executor == CurrentUser.IdUser
            &&
                (from o in modelContext.MainConfigs
                 where o.NumbInput == b.NumbInput
                 select o).Count() == 1
                         select b
                    ).ToList();

            stackPanel1.Children.Clear();
            foreach (var main in mains)
            {
                CheckBox ch = new CheckBox();
                ch.Content = $"{main.NumbInput}";
                ch.Tag = main.NumbInput;
                ch.HorizontalAlignment = HorizontalAlignment.Center;
                stackPanel1.Children.Add(ch);
            }
        }
        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            listNumbIn.Clear();
            foreach (var item in stackPanel1.Children)
            {
                var ch = item as CheckBox;
                if (ch != null)
                {
                    if (ch.IsChecked!.Value)
                    {
                        listNumbIn.Add(ch.Content.ToString());
                    }
                }
            }
            if(listNumbIn.Count == 0)
            {
                MessageBox.Show("Не вибрано жодного запиту");
            }
            else
            {
                this.Close();
            }
        }
    }
}
