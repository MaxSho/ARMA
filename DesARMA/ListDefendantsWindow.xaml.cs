using DesARMA.Log;
using DesARMA.Log.Data;
using DesARMA.Models;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DesARMA
{
    /// <summary>
    /// Interaction logic for ListDefendantsWindow.xaml
    /// </summary>
    /// 
    public partial class ListDefendantsWindow : System.Windows.Window
    {
        public static ListDefendantsWindow? Instance { get; private set; }
        private System.Windows.Forms.Timer inactivityTimer = new System.Windows.Forms.Timer();
        List<Figurant>  listConnectedPeople = new List<Figurant>();
        Dictionary<decimal, Figurant> listDictionaryConnectedPeople = new Dictionary<decimal, Figurant>();
        public List<Figurant> listDefendants = new List<Figurant>();
        public Dictionary<decimal, Figurant> listDictionaryDefendants = new Dictionary<decimal, Figurant>();
        ModelContext modelContext;
        string numberIn;
        public bool isConn;
        Main? curM = null!;
        public static bool isOpenWindFig = true;
        MainWindow? win;
        bool isMeInWin = false;
        UpdateDel updateDel;

        public ListDefendantsWindow(ModelContext modelContext, string numberIn, string tit, bool isConn, UpdateDel updateDel, System.Windows.Forms.Timer inactivityTimer)
        {
            try
            {
                InitializeComponent();

                this.modelContext = modelContext;
                this.numberIn = numberIn;
                this.Title = tit;
                this.isConn = isConn;
                this.updateDel = updateDel;



                curM = modelContext.Mains.Find(numberIn);
                if (curM == null)
                    this.Close();

                GetFig(numberIn);

                UpdateListButton();

                //string shif = ConfigurationManager.AppSettings["hv"].ToString();
                //inactivityTimer.Interval = 60_000 * Convert.ToInt32(shif);
                //inactivityTimer.Tick += (sender, args) =>
                //{
                //    Environment.Exit(0);
                //};
                inactivityTimer.Start();
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }
        
   
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (isMeInWin)
                    (this.Owner as MainWindow).isOpenWindFig = false;
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            inactivityTimer.Stop();
        }
        void OnLoad(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((this.Owner as MainWindow).isOpenWindFig)
                {
                    isMeInWin = false;
                    this.Close();
                }
                else
                {
                    isMeInWin = true;
                    (this.Owner as MainWindow).isOpenWindFig = true;
                }
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                this.Close();
            }
        }
        private void GetFig(string numberIn)
        {
            var fos = from f in modelContext.Figurants
                      where f.NumbInput == numberIn && f.Status == (isConn ? 2 : 1)
                      select f;

            foreach (var item in fos)
            {
                if (isConn)
                {
                    listConnectedPeople.Add(item);
                    //listDictionaryConnectedPeople.Add(item.IdinGlobDB, item);
                }
                else
                {
                    listDefendants.Add(item);
                    //listDictionaryDefendants.Add(item.IdinGlobDB, item);
                }
            }

        }

        private void Add_ClickFO(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();

            try 
            { 
                Def_FO_Window def_FO_Window = new Def_FO_Window(new Figurant(), isConn, inactivityTimer);
                def_FO_Window.figurant.NumbInput = this.numberIn;
                def_FO_Window.figurant.Control = new string('0', Reest.abbreviatedName.Count + 1);
                def_FO_Window.figurant.Shema   = new string('0', Reest.abbreviatedName.Count + 1);

                if (def_FO_Window.ShowDialog() == true)
                {
                    if (!def_FO_Window.isDelete)
                    {
                        if (def_FO_Window.figurant.Status == 2 )
                        {
                            def_FO_Window.figurant.LoginName = curM!.LoginName;
                            listConnectedPeople.Add(def_FO_Window.figurant);
                            modelContext.Figurants.Add(def_FO_Window.figurant);
                            modelContext.SaveChanges();
                            App.CurUser.LogInf(new FigurantsData(App.CurUser.LoginName, TypeLogData.Access, numberIn, $"Додано пов'язану особу (ФО) {def_FO_Window.figurant.Id}"));

                        }
                        else
                        {
                            def_FO_Window.figurant.LoginName = curM.LoginName;
                            listDefendants.Add(def_FO_Window.figurant); modelContext.Figurants.Add(def_FO_Window.figurant);
                            modelContext.SaveChanges();
                            App.CurUser.LogInf(new FigurantsData(App.CurUser.LoginName, TypeLogData.Access, numberIn, $"Додано фігуранта (ФО) {def_FO_Window.figurant.Id}"));

                        }

                    }
                    UpdateListButton();
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }

            inactivityTimer.Start();
        }
        private void Add_ClickUO(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();

            try
            {
                Def_UO_Window def_UO_Window = new Def_UO_Window(new Figurant(), isConn, inactivityTimer);
                def_UO_Window.figurant.NumbInput = this.numberIn;

                def_UO_Window.figurant.Control = new string('0', Reest.abbreviatedName.Count + 1);
                def_UO_Window.figurant.Shema = new string('0', Reest.abbreviatedName.Count + 1);

                if (def_UO_Window.ShowDialog() == true)
                {
                    //MessageBox.Show($"{def_UO_Window.uo.name} \n {def_UO_Window.uo.code}  \n {def_UO_Window.uo.isResid}");
                    if (!def_UO_Window.isDelete)
                    {
                        if (def_UO_Window.figurant.Status == 2)
                        {
                            var prevMc = modelContext.MainConfigs.Find(curM.NumbInput);


                            def_UO_Window.figurant.LoginName = curM!.LoginName;
                            listConnectedPeople.Add(def_UO_Window.figurant);
                            modelContext.Figurants.Add(def_UO_Window.figurant);
                            modelContext.SaveChanges();
                            App.CurUser.LogInf(new FigurantsData(App.CurUser.LoginName, TypeLogData.Access, numberIn, $"Додано пов'язану особу (ЮО) {def_UO_Window.figurant.Id}"));

                        }
                        else
                        {
                            def_UO_Window.figurant.LoginName = curM!.LoginName;
                            listDefendants.Add(def_UO_Window.figurant);
                            modelContext.Figurants.Add(def_UO_Window.figurant);
                            modelContext.SaveChanges();
                            App.CurUser.LogInf(new FigurantsData(App.CurUser.LoginName, TypeLogData.Access, numberIn, $"Додано фігуранта (ЮО) {def_UO_Window.figurant.Id}"));

                        }

                    }
                    UpdateListButton();
                }
                else
                {
                    // MessageBox.Show("Авторизация не пройдена");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }

            inactivityTimer.Start();
        }
        private void DefClickButton(object sender, RoutedEventArgs e)
        {
            inactivityTimer.Stop();

            try
            {
                var b = sender as System.Windows.Controls.Button;
                if (b != null)
                {
                    if (isConn)
                    {
                        int ind = (int)b.Tag;
                        var def = listConnectedPeople[ind] as Figurant;

                        if (def.ResUr != null)
                        {
                            Def_UO_Window def_UO_Window = new Def_UO_Window(def, isConn, inactivityTimer);
                            if (def_UO_Window.ShowDialog() == true)
                            {
                                if (!def_UO_Window.isDelete)
                                {

                                }
                                else
                                {
                                    modelContext.Figurants.Remove(def_UO_Window.figurant);
                                    listConnectedPeople.RemoveAt(ind);
                                    App.CurUser.LogInf(new FigurantsData(App.CurUser.LoginName, TypeLogData.Access, numberIn, $"Видалено (ЮО) {def.Id}"));
                                }
                                modelContext.SaveChanges();
                                UpdateListButton();

                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            Def_FO_Window def_FO_Window = new Def_FO_Window(def, isConn, inactivityTimer);
                            if (def_FO_Window.ShowDialog() == true)
                            {
                                if (!def_FO_Window.isDelete)
                                {

                                }
                                else
                                {
                                    modelContext.Figurants.Remove(def_FO_Window.figurant);
                                    listConnectedPeople.RemoveAt(ind);
                                    App.CurUser.LogInf(new FigurantsData(App.CurUser.LoginName, TypeLogData.Access, numberIn, $"Видалено (ФО) {def.Id} "));
                                }
                                modelContext.SaveChanges();
                                UpdateListButton();

                            }
                            else
                            {

                            }
                        }
                    }
                    else
                    {
                        int ind = (int)b.Tag;
                        var def = listDefendants[ind] as Figurant;

                        if (def.ResUr != null)
                        {
                            Def_UO_Window def_UO_Window = new Def_UO_Window(def, isConn, inactivityTimer);
                            if (def_UO_Window.ShowDialog() == true)
                            {
                                if (!def_UO_Window.isDelete)
                                {

                                }
                                else
                                {
                                    modelContext.Figurants.Remove(def_UO_Window.figurant);
                                    listDefendants.RemoveAt(ind);
                                    App.CurUser.LogInf(new FigurantsData(App.CurUser.LoginName, TypeLogData.Access, numberIn, $"Видалено (ЮО) {def.Id}"));
                                }
                                modelContext.SaveChanges();
                                UpdateListButton();

                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            Def_FO_Window def_FO_Window = new Def_FO_Window(def, isConn, inactivityTimer);
                            if (def_FO_Window.ShowDialog() == true)
                            {
                                if (!def_FO_Window.isDelete)
                                {

                                }
                                else
                                {
                                    modelContext.Figurants.Remove(def_FO_Window.figurant);
                                    listDefendants.RemoveAt(ind);
                                    App.CurUser.LogInf(new FigurantsData(App.CurUser.LoginName, TypeLogData.Access, numberIn, $"Видалено (ФО) {def.Id}"));
                                }
                                modelContext.SaveChanges();
                                UpdateListButton();

                            }
                            else
                            {

                            }
                        }
                    }
                  
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }

            inactivityTimer.Start();
        }
        private void UpdateListButton()
        {
            stackPanel1.Children.Clear();
            int i = 1;
            if (isConn)
                foreach (var item in listConnectedPeople)
                {
                    var b = new System.Windows.Controls.Button();
                    b.FontSize = 16;
                    b.Tag = i - 1;
                    if (item.ResFiz != null)
                    {
                        b.Content = $"{i++}. {item.Fio} {item.Ipn}";
                    }
                    else
                    {
                        b.Content = $"{i++}. {item.Name} {item.Code}";
                    }

                    b.Click += DefClickButton;
                    stackPanel1.Children.Add(b);
                }
            else
                foreach (var item in listDefendants)
                {
                    var b = new System.Windows.Controls.Button();
                    b.FontSize = 16;
                    b.Tag = i - 1;
                    if (item.ResFiz != null)
                    {
                        b.Content = $"{i++}. {item.Fio} {item.Ipn}";
                    }
                    else
                    {
                        b.Content = $"{i++}. {item.Name} {item.Code}";
                    }
                    b.Click += DefClickButton;
                    stackPanel1.Children.Add(b);
                }
            updateDel(new object(), new RoutedEventArgs());
        }

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }

        private void Window_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }
    }

    public interface Def
    {
        public string name { get; set; }
        public string code { get; set; }
    }
    public class FO: Def
    {
        public int id { get; set; }
        public string name { get; set; } = "";
        public string code { get; set; } = "";
        public string dateB { get; set; } = "";
        public bool isResid { get; set; }
        public bool isConnectedPeople { get; set; }
        public decimal IdinGlobDB { get; set; }
    }
    public class UO: Def
    {
        public int id { get; set; }
        public string name { get; set; } = "";
        public string code { get; set; } = "";
        public bool isResid { get; set; }
        public bool isConnectedPeople { get; set; }
        public decimal IdinGlobDB { get; set; }
    }

}
