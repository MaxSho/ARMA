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
        System.Windows.Forms.Timer inactivityTimer;
        public User CurrentUser { get; set; } = null!;
        public SelectionOfQueriesToInsertWindow(ModelContext modelContext, User CurrentUser, System.Windows.Forms.Timer inactivityTimer)
        {
            InitializeComponent();

            this.modelContext = modelContext;
            this.CurrentUser = CurrentUser;
            this.inactivityTimer = inactivityTimer;

            //var mains = (from b in modelContext.Mains
            //             where b.Executor == CurrentUser.IdUser
            //&&
            //    (from o in modelContext.MainConfigs
            //     where o.NumbInput == b.NumbInput
            //     select o).Count() == 1
            //             select b
            //        ).ToList();




            var mains = (from b in modelContext.Mains
                         where b.Executor == CurrentUser.IdUser
                &&
                    (from o in modelContext.MainConfigs
                     where o.NumbInput == b.NumbInput
                     select o).Count() == 1
                         //orderby /*b.NumbInput.Substring(8, 2),*/
                         //        b.NumbInput.Split(new char[] { '/' }, 1)[0]//CreateCombinedResponseWindow.GetStringWithZero(b.NumbInput)
                         select b
                   )
                   .AsEnumerable()
                   .OrderByDescending(b => {
                       int result;
                       if (int.TryParse(b.NumbInput.Split(new char[] { '/' }, 2)[0], out result))
                       {
                           return result;
                       }
                       else
                       {
                           return 0;
                       }
                   })
                   .OrderByDescending(b => {
                       int result;
                       if (int.TryParse(b.NumbInput.Split(new char[] { '-' }, 2)[1], out result))
                       {
                           return result;
                       }
                       else
                       {
                           return 0;
                       }
                   })
                   .ToList();

            stackPanel1.Children.Clear();
            foreach (var main in mains)
            {
                CheckBox ch = new CheckBox();
                ch.Content = $"{main.NumbInput}";
                ch.Tag = main.NumbInput;
                ch.HorizontalAlignment = HorizontalAlignment.Center;
                stackPanel1.Children.Add(ch);
            }
            inactivityTimer.Start();
        }
        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
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
                if (listNumbIn.Count == 0)
                {
                    MessageBox.Show("Не вибрано жодного запиту");
                }
                else
                {
                    this.Close();
                }
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
        }
    }
}
