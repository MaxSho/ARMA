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
            try
            {
                InitializeComponent();

                this.modelContext = modelContext;
                this.CurrentUser = CurrentUser;
                this.main = main;
                this.inactivityTimer = inactivityTimer;

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
                        //.Where(b => { return b.NumbInput.Split(new char[] { '-' }, 2)[1] == DateTime.Now.Year.ToString().Substring(2, 2); })
                        .Where(b => { 
                            if(b.DtInput != null)
                                return b.DtInput.Value.Year == DateTime.Now.Year;
                            return false;
                        })
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

                    if (mainItem.NumbInput == main.NumbInput)
                    {
                        ch.IsChecked = true;
                    }

                    ch.Content = $"{mainItem.NumbInput}";
                    ch.Tag = mainItem.NumbInput;
                    ch.Click += (x, y) => {
                        inactivityTimer.Stop();
                        var ch = x as CheckBox;
                        if (ch != null)
                        {
                            if (ch.Content.ToString() == main.NumbInput)
                            {
                                ch.IsChecked = true;
                            }
                        }
                        inactivityTimer.Start();
                    };
                    ch.HorizontalAlignment = HorizontalAlignment.Center;
                    stackPanel1.Children.Add(ch);
                }
                inactivityTimer.Start();


                headerTextBlock.Text = $"На основі запиту № {main.NumbInput} знайдено наступні запити, що мають спільний з ним номер КП, за 2023 рік:";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

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
                    //MessageBox.Show("Перевірка ...");
                    
                    CombinedResponseWindows.SelectionOfCombinedQueryFieldsWindow win =
                        new CombinedResponseWindows.SelectionOfCombinedQueryFieldsWindow(modelContext, main, listNumbIn, inactivityTimer);
                    
                    //this.Hide();
                    //this.Visibility = Visibility.Hidden;
                    if (win.ShowDialog() == true)
                    {
                        CombinedResponseWindows.EntryOfPersonsInvolvedInTheCombinedRegistersWindow
                        entryOfPersonsInvolvedInTheCombinedRegistersWindow
                        = new CombinedResponseWindows.EntryOfPersonsInvolvedInTheCombinedRegistersWindow(modelContext, main, listNumbIn, inactivityTimer);
                        
                        if (entryOfPersonsInvolvedInTheCombinedRegistersWindow.ShowDialog() == true)
                        {
                            //CreateResp();
                            DocResponse docResponse = new DocResponse(GetMainConfigsList(),
                                new List<int>() { (entryOfPersonsInvolvedInTheCombinedRegistersWindow.figurants.Count > 1 ? 0 : 1), 
                                    win.idAcc.SelectedIndex == 2 ? 3 : win.idAcc.SelectedIndex, 
                                    entryOfPersonsInvolvedInTheCombinedRegistersWindow.numbColorShema == 3 ? 1:0 }, 
                                new List<string>() {
                                    win.executorInit.Text,
                                    win.addr.Text,
                                    GetStringFromDateTime(main.DtOutInit),
                                    GetStringFromDateTime(main.DtInput),
                                    main!.NumbOutInit!,
                                    main.NumbInput,
                                    win.agencyDep.Text,
                                    win.work.Text
                                    },
                                modelContext);
                            docResponse.CreateResponseCombinedOther(entryOfPersonsInvolvedInTheCombinedRegistersWindow.numbColorInReestr);
                            docResponse.ToDiskCombined(entryOfPersonsInvolvedInTheCombinedRegistersWindow.numbColorInReestr,
                                entryOfPersonsInvolvedInTheCombinedRegistersWindow.numbColorShema
                                );
                            //MessegeAboutCreate();
                            win.SaveAllInReq();
                            this.DialogResult = true;
                        }
                        else
                        {
                            this.DialogResult = false;
                        }
                    }
                    else
                    {

                        this.DialogResult = false;
                    }
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
        
        private List<MainConfig> GetMainConfigsList()
        {
            var l = (from f in modelContext.MainConfigs 
                     where listNumbIn.Contains(f.NumbInput) /*&& f.NumbInput != main.NumbInput*/ 
                     select f).ToList();


            for (int i = 0; i < l.Count; i++)
            {
                if (l[i].NumbInput == main.NumbInput)
                {
                    var m = l[i];
                    for (int j = i; j > 0; j--)
                    {
                        l[j] = l[j - 1];
                    }
                    l[0] = m;
                    break;
                }
            }


            return l;
        }
        private string GetStringFromDateTime(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return "";
            }
            else
            {
                var strD = dateTime.ToString();
                if (strD == null)
                {
                    return "";
                }
                else
                {
                    return strD.Substring(0, 10);
                }
            }
        }
        public void MessegeAboutCreate()
        {
            string retStr = listNumbIn.FirstOrDefault();
            for (int i = 1; i < listNumbIn.Count; i++)
            {
                retStr += ", " + listNumbIn[i];
            }
            MessageBox.Show("Створено папку \"Об'єднана відповідь\" разом з додатками (Об'єднана відповідь\\На диск) в папках таких запитів:\n" + retStr);
        }
    }
}
