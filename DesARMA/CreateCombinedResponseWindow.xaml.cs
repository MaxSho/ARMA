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
    /// Interaction logic for CreateCombinedResponseWindow.xaml
    /// </summary>
    public partial class CreateCombinedResponseWindow : Window
    {
        public List<string> listNumbIn = new List<string>();
        ModelContext modelContext;
        public User CurrentUser { get; set; } = null!;
        private System.Windows.Forms.Timer inactivityTimer = new System.Windows.Forms.Timer();
        Main main;
        public CreateCombinedResponseWindow(ModelContext modelContext, User CurrentUser, Main main, System.Windows.Forms.Timer inactivityTimer)
        {
            InitializeComponent();

            this.modelContext = modelContext;
            this.CurrentUser = CurrentUser;
            this.main = main;
            this.inactivityTimer = inactivityTimer;


            //var mains = (from b in modelContext.Mains
            //             where b.Executor == CurrentUser.IdUser
            //&&
            //    (from o in modelContext.MainConfigs
            //     where o.NumbInput == b.NumbInput
            //     select o).Count() == 1
            //&&
            //    b.CpNumber == main.CpNumber
            //orderby b.NumbInput.Substring(b.NumbInput.Length - 2, 2) descending,
            //        GetStringWithZero(b.NumbInput.Split(new char[] {'/'}).First()) descending
            //             select b
            //).ToList();
            var mains = (from b in modelContext.Mains
                         where b.Executor == CurrentUser.IdUser
                &&
                    (from o in modelContext.MainConfigs
                     where o.NumbInput == b.NumbInput
                     select o).Count() == 1
                         //orderby /*b.NumbInput.Substring(8, 2),*/
                         //        b.NumbInput.Split(new char[] { '/' }, 1)[0]//CreateCombinedResponseWindow.GetStringWithZero(b.NumbInput)
                &&
                    b.CpNumber == main.CpNumber
                         select b
                    )
                    .AsEnumerable()
                    .Where(b => { return b.NumbInput.Split(new char[] { '-' }, 2)[1] == DateTime.Now.Year.ToString().Substring(2, 2); })
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
            foreach (var mainItem in mains)
            {
                CheckBox ch = new CheckBox();
                ch.Content = $"{mainItem.NumbInput}";
                ch.Tag = mainItem.NumbInput;
                ch.Click += (x,y) => {
                    inactivityTimer.Stop();
                    inactivityTimer.Start();
                };
                ch.HorizontalAlignment = HorizontalAlignment.Center;
                stackPanel1.Children.Add(ch);
            }
            inactivityTimer.Start();
        }
        public static string GetStringWithZero(string str)
        {
            string ret = "";

            foreach (var item in str)
            {
                if (item == '/') break;
                ret += item;
            }

            while (ret.Length < 7)
            {
                ret = "0" + str;
            }
            return ret;
        }
        private void Create_Button_Click(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Start();
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
                    MessageBox.Show("Перевірка ...");
                    
                    CombinedResponseWindows.SelectionOfCombinedQueryFieldsWindow win =
                        new CombinedResponseWindows.SelectionOfCombinedQueryFieldsWindow(modelContext, main, listNumbIn);
                    this.Hide();
                    win.Show();
                    //this.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            inactivityTimer.Stop();
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
