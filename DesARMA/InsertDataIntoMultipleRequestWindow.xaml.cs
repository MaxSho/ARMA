using DesARMA.Models;
using System;
using System.CodeDom.Compiler;
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
        List<string> listNumbIn = new List<string>();
        List<CheckBox> checkBoxItems = new List<CheckBox>();
        LoadDel loadDel;
        private System.Windows.Forms.Timer inactivityTimer = new System.Windows.Forms.Timer();
        public InsertDataIntoMultipleRequestWindow(ModelContext modelContext, User CurrentUser, LoadDel loadDel, System.Windows.Forms.Timer inactivityTimer)
        {
            this.modelContext = modelContext;
            this.CurrentUser = CurrentUser;
            this.loadDel = loadDel;
            this.inactivityTimer = inactivityTimer;

            InitializeComponent();

            checkBoxItems.Add(checkBoxItem1);
            //checkBoxItems.Add(checkBoxItem2);
            checkBoxItems.Add(checkBoxItem3);
            checkBoxItems.Add(checkBoxItem4);
            checkBoxItems.Add(checkBoxItem5);
            checkBoxItems.Add(checkBoxItem6);
            checkBoxItems.Add(checkBoxItem7);
            checkBoxItems.Add(checkBoxItem8);
            checkBoxItems.Add(checkBoxItem9);
            checkBoxItems.Add(checkBoxItem10);
            //checkBoxItems.Add(checkBoxItem11);
            checkBoxItems.Add(checkBoxItem12);
            //checkBoxItems.Add(checkBoxItem13);
            //checkBoxItems.Add(checkBoxItem14);
            //checkBoxItems.Add(checkBoxItem15);
            //checkBoxItems.Add(checkBoxItem16);

            InsertItem1.ItemsSource = Reest.organsName;

            foreach (var item in checkBoxItems)
            {
                item.Click += (w, r) => 
                {
                    bool ret = true;
                    foreach (var item in checkBoxItems)
                    {
                        ret = ret && item.IsChecked.Value;
                    }
                    totalCheckBox.IsChecked = isAllCheck();
                };
            }

            

            inactivityTimer.Start();
        }
        private bool isAllCheck()
        {
            bool ret = true;
            foreach (var item in checkBoxItems)
            {
                ret = ret && item.IsChecked.Value;
            }
            return ret;
        }
        private bool isAllUnCheck()
        {
            bool ret = true;
            foreach (var item in checkBoxItems)
            {
                ret = ret && !item.IsChecked.Value;
            }
            return ret;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                this.Hide();
                SelectionOfQueriesToInsertWindow selectionOfQueriesToInsertWindow =
                    new SelectionOfQueriesToInsertWindow(modelContext, CurrentUser, inactivityTimer);

                selectionOfQueriesToInsertWindow.ShowDialog();
                this.Show();
                listNumbIn = selectionOfQueriesToInsertWindow.listNumbIn;
                if (selectionOfQueriesToInsertWindow.listNumbIn.Count == 0)
                {
                    this.Close();
                    //Environment.Exit(0);
                }
                else
                {
                    stackPanelReq.Children.Clear();
                    foreach (var item in listNumbIn)
                    {
                        Button b = new Button();
                        b.Content = item;
                        b.Background =  this.Resources[$"3ColorStyle"] as SolidColorBrush;
                        b.Foreground = this.Resources[$"1ColorStyle"] as SolidColorBrush;
                        stackPanelReq.Children.Add(b);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            inactivityTimer.Start();
        }
        private void Button_Click_Insert(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                if (isAllUnCheck())
                {
                    MessageBox.Show("Не вибрано жодного поля");
                }
                else
                {
                    Save();
                    loadDel();
                    Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            inactivityTimer.Start();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();
            try
            {
                var ch = sender as CheckBox;
                if (ch != null)
                {
                    foreach (var item in checkBoxItems)
                    {
                        item.IsChecked = ch.IsChecked;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            inactivityTimer.Start();

        }
        private void Save()
        {
            try
            {
                var mains = (from m in modelContext.Mains where listNumbIn.Contains(m.NumbInput) select m).ToList();
                if (mains != null)
                    foreach (var main in mains)
                    {
                        if (checkBoxItem1.IsChecked.Value)
                        {
                            main.IdAcc = GetIdFromDicForNameTypeOrgan(InsertItem1.SelectedIndex);
                        }
                        //if (checkBoxItem2.IsChecked.Value)
                        //{
                        //    main.NumbOutInit = InsertItem2.Text;
                        //}
                        if (checkBoxItem3.IsChecked.Value)
                        {
                            main.DtOutInit = InsertItem3.SelectedDate;
                        }
                        if (checkBoxItem4.IsChecked.Value)
                        {
                            main.AgencyDep = InsertItem4.Text;
                        }
                        if (checkBoxItem5.IsChecked.Value)
                        {
                            main.Addr = InsertItem5.Text;
                        }
                        if (checkBoxItem6.IsChecked.Value)
                        {
                            main.Work = InsertItem6.Text;
                        }
                        if (checkBoxItem7.IsChecked.Value)
                        {
                            main.ExecutorInit = InsertItem7.Text;
                        }
                        if (checkBoxItem8.IsChecked.Value)
                        {
                            main.NumbOut = InsertItem8.Text;
                        }
                        if (checkBoxItem9.IsChecked.Value)
                        {
                            main.DtOut = InsertItem9.SelectedDate;
                        }
                        if (checkBoxItem10.IsChecked.Value)
                        {
                            main.CoExecutor = InsertItem10.Text;
                        }
                        //if (checkBoxItem11.IsChecked.Value)
                        //{
                        //    main.Folder = InsertItem11.Text;
                        //}
                        if (checkBoxItem12.IsChecked.Value)
                        {
                            main.Art = InsertItem12.Text;
                        }
                        //if (checkBoxItem13.IsChecked.Value)
                        //{
                        //    main.Notes = InsertItem13.Text;
                        //}
                        //if (checkBoxItem14.IsChecked.Value)
                        //{
                        //    main.Status = InsertItem14.SelectedIndex == -1 ? null : (InsertItem14.SelectedIndex + 1).ToString();
                        //}
                        //if (checkBoxItem15.IsChecked.Value)
                        //{
                        //    //TODO save figurants
                        //}
                        //if (checkBoxItem16.IsChecked.Value)
                        //{
                        //    //TODO save connected people
                        //}
                    }
                modelContext.SaveChanges();
                MessageBox.Show("Дані збережено");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
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

        private void InsertItem_TextChanged(object sender, TextChangedEventArgs e)
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }

        private void InsertItem_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }
        private decimal? GetIdFromDicForNameTypeOrgan(int indLoc)
        {

            if (indLoc != -1)
            {
                var blogs = from b in modelContext.DictCommons
                            where b.Domain == "ACCOST" && b.Code == InsertItem1.SelectedValue
                            select b;

                foreach (var item in blogs)
                {
                    return item.Id;
                }

            }
            return null;
        }
    }
}
