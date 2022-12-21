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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DesARMA
{
    /// <summary>
    /// Interaction logic for ListDefendantsWindow.xaml
    /// </summary>
    public partial class ListDefendantsWindow : System.Windows.Window
    {
        List<Figurant>  listConnectedPeople = new List<Figurant>();
        Dictionary<decimal, Figurant> listDictionaryConnectedPeople = new Dictionary<decimal, Figurant>();
        public List<Figurant> listDefendants = new List<Figurant>();
        public Dictionary<decimal, Figurant> listDictionaryDefendants = new Dictionary<decimal, Figurant>();
        ModelContext modelContext;
        string numberIn;
        public bool isConn;
        Main? curM = null!;
        public ListDefendantsWindow(ModelContext modelContext, string numberIn, string tit, bool isConn)
        {
            InitializeComponent();
            //this.curRnum = curRnum;
            this.modelContext = modelContext;
            this.numberIn = numberIn;
            this.Title = tit;
            this.isConn = isConn;
            //this.listConnectedPeople = new List<Def>();
            //this.listDictionaryConnectedPeople = new Dictionary<decimal, Def>();
            //this.listDefendants = new List<Def>();
            //this.listDictionaryDefendants = new Dictionary<decimal, Def>();

            curM = modelContext.Mains.Find(numberIn);
            if (curM == null)
                Close();

            //var fos = from r in db.Requests
            //             from rin in r.fos
            //             where r.id == curRnum
            //             select rin;
            //var uos = from r in db.Requests
            //           from rin in r.uos
            //           where r.id == curRnum
            //           select rin;

            //foreach (var item in fos)
            //{
            //    if (item.isConnectedPeople)
            //    {
            //        listConnectedPeople.Add(item);
            //        listDictionaryConnectedPeople.Add(item.IdinGlobDB, item);
            //    }
            //    else
            //    {
            //        listDefendants.Add(item);
            //        listDictionaryDefendants.Add(item.IdinGlobDB, item);
            //    }
            //}

            //    foreach (var item in uos)
            //    {
            //        if (item.isConnectedPeople)
            //        {
            //            listConnectedPeople.Add(item);
            //            listDictionaryConnectedPeople.Add(item.IdinGlobDB, item);
            //        }
            //        else
            //        {
            //            listDefendants.Add(item);
            //            listDictionaryDefendants.Add(item.IdinGlobDB, item);
            //        }
            //    }

            GetFig(numberIn);

            UpdateListButton();

        }
        private void GetFig(string numberIn)
        {
            var fos = from f in modelContext.Figurants
                      where f.NumbInput == numberIn && f.Status == isConn
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


        //private void Add_Click(object sender, RoutedEventArgs e)
        //{
        //    //this.DialogResult = true;
        //}

        //public string Password
        //{
        //    get { return ""; }
        //}

        //private void Add_ClickFO(object sender, RoutedEventArgs e)
        //{
        //    Def_FO_Window def_FO_Window = new Def_FO_Window(new FO(), isConn);

        //    if (def_FO_Window.ShowDialog() == true)
        //    {
        //       // MessageBox.Show($"{def_FO_Window.fo.name} \n {def_FO_Window.fo.code} \n {def_FO_Window.fo.dateB} \n {def_FO_Window.fo.isResid}");
        //        if (!def_FO_Window.isDelete)
        //        {
        //            if (def_FO_Window.fo.isConnectedPeople) 
        //            {
        //                listConnectedPeople.Add(def_FO_Window.fo);
        //                var temp = new Figurant() { NumbInput = numberIn, Name = def_FO_Window.fo.name, Ipn = def_FO_Window.fo.code, ResFiz = def_FO_Window.fo.isResid, DtBirth = IsCorectDate(def_FO_Window.fo.dateB) };

        //                modelContext.Figurants.Add(temp);
        //                modelContext.SaveChanges();
        //                listDictionaryConnectedPeople.Add(temp.Id, def_FO_Window.fo);

        //                def_FO_Window.fo.IdinGlobDB = temp.Id;
        //                if(curR != null)
        //                {
        //                    curR.fos.Add(def_FO_Window.fo);
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Не знайдено запит");
        //                }

        //                db.SaveChanges();
        //            }
        //            else
        //            {
        //                listDefendants.Add(def_FO_Window.fo);
        //                var temp = new Figurant() { NumbInput = numberIn, Name = def_FO_Window.fo.name, Ipn = def_FO_Window.fo.code, ResFiz = def_FO_Window.fo.isResid, DtBirth = IsCorectDate(def_FO_Window.fo.dateB) };
        //                modelContext.Figurants.Add(temp);
        //                modelContext.SaveChanges();
        //                listDictionaryDefendants.Add(temp.Id, def_FO_Window.fo);

        //                def_FO_Window.fo.IdinGlobDB = temp.Id;
        //                if (curR != null)
        //                {
        //                    curR.fos.Add(def_FO_Window.fo);
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Не знайдено запит");
        //                }
        //                db.SaveChanges();
        //            }
        //        }
        //        UpdateListButton();
        //    }
        //    else
        //    {
        //       // MessageBox.Show("Авторизация не пройдена");
        //    }
        //}
        private void Add_ClickFO(object sender, RoutedEventArgs e)
        {
            Def_FO_Window def_FO_Window = new Def_FO_Window(new Figurant(), isConn);
            def_FO_Window.figurant.NumbInput = this.numberIn;

            if (def_FO_Window.ShowDialog() == true)
            {
                //MessageBox.Show($"{def_UO_Window.uo.name} \n {def_UO_Window.uo.code}  \n {def_UO_Window.uo.isResid}");
                if (!def_FO_Window.isDelete)
                {
                    if (def_FO_Window.figurant.Status == true)
                    {
                        listConnectedPeople.Add(def_FO_Window.figurant);
                        //var temp = new Figurant() { Status = true, NumbInput = numberIn, Name = def_UO_Window.figurant.Name, Ipn = def_UO_Window.figurant.Code, ResFiz = def_UO_Window.figurant.Status };
                        modelContext.Figurants.Add(def_FO_Window.figurant);
                        modelContext.SaveChanges();
                        // listDictionaryConnectedPeople.Add(temp.Id, def_UO_Window.uo);

                        //def_UO_Window.uo.IdinGlobDB = temp.Id;

                    }
                    else
                    {
                        listDefendants.Add(def_FO_Window.figurant);
                        //var temp = new Figurant() { Status = true, NumbInput = numberIn, Name = def_UO_Window.figurant.Name, Ipn = def_UO_Window.figurant.Code, ResFiz = def_UO_Window.figurant.Status };
                        modelContext.Figurants.Add(def_FO_Window.figurant);
                        modelContext.SaveChanges();
                        // listDictionaryDefendants.Add(temp.Id, def_UO_Window.figurant);
                    }

                }
                UpdateListButton();
            }
            else
            {
                // MessageBox.Show("Авторизация не пройдена");
            }
        }
        private void Add_ClickUO(object sender, RoutedEventArgs e)
        {
            Def_UO_Window def_UO_Window = new Def_UO_Window(new Figurant(), isConn);
            def_UO_Window.figurant.NumbInput = this.numberIn;
            if (def_UO_Window.ShowDialog() == true)
            {
                //MessageBox.Show($"{def_UO_Window.uo.name} \n {def_UO_Window.uo.code}  \n {def_UO_Window.uo.isResid}");
                if (!def_UO_Window.isDelete)
                {
                    if (def_UO_Window.figurant.Status == true)
                    {
                        listConnectedPeople.Add(def_UO_Window.figurant);
                        //var temp = new Figurant() { Status = true, NumbInput = numberIn, Name = def_UO_Window.figurant.Name, Ipn = def_UO_Window.figurant.Code, ResFiz = def_UO_Window.figurant.Status };
                        modelContext.Figurants.Add(def_UO_Window.figurant);
                        modelContext.SaveChanges();
                        // listDictionaryConnectedPeople.Add(temp.Id, def_UO_Window.uo);

                        //def_UO_Window.uo.IdinGlobDB = temp.Id;

                    }
                    else
                    {
                        listDefendants.Add(def_UO_Window.figurant);
                        //var temp = new Figurant() { Status = true, NumbInput = numberIn, Name = def_UO_Window.figurant.Name, Ipn = def_UO_Window.figurant.Code, ResFiz = def_UO_Window.figurant.Status };
                        modelContext.Figurants.Add(def_UO_Window.figurant);
                        modelContext.SaveChanges();
                       // listDictionaryDefendants.Add(temp.Id, def_UO_Window.figurant);
                    }

                }
                UpdateListButton();
            }
            else
            {
                // MessageBox.Show("Авторизация не пройдена");
            }
        }
        private void DefClickButton(object sender, RoutedEventArgs e)
        {
            var b = sender as System.Windows.Controls.Button;
            if (b != null)
            {
                int ind = (int)b.Tag;
                var def = listDefendants[ind] as Figurant;
                
                if(def.ResUr != null)
                {
                    Def_UO_Window def_UO_Window = new Def_UO_Window(def, isConn);
                    if (def_UO_Window.ShowDialog() == true)
                    {
                        //MessageBox.Show($"{def_UO_Window.uo.name} \n {def_UO_Window.uo.code}  \n {def_UO_Window.uo.isResid}");
                        if (!def_UO_Window.isDelete)
                        {
                            
                        }
                        else
                        {
                            modelContext.Figurants.Remove(def_UO_Window.figurant);
                            listDefendants.RemoveAt(ind);
                        }
                        modelContext.SaveChanges();
                        UpdateListButton();
                    }
                    else
                    {
                        // MessageBox.Show("Авторизация не пройдена");
                    }
                }
                else
                {
                    Def_FO_Window def_FO_Window = new Def_FO_Window(def, isConn);
                    if (def_FO_Window.ShowDialog() == true)
                    {
                        //MessageBox.Show($"{def_UO_Window.uo.name} \n {def_UO_Window.uo.code}  \n {def_UO_Window.uo.isResid}");
                        if (!def_FO_Window.isDelete)
                        {
                            
                        }
                        else
                        {
                            modelContext.Figurants.Remove(def_FO_Window.figurant);
                            listDefendants.RemoveAt(ind);
                        }
                        modelContext.SaveChanges();
                        UpdateListButton();
                    }
                    else
                    {
                        // MessageBox.Show("Авторизация не пройдена");
                    }
                }

            }
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
                    if(item.ResFiz != null)
                    {
                        b.Content = $"{i++}. {item.Name} {item.Ipn}";
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
                        b.Content = $"{i++}. {item.Name} {item.Ipn}";
                    }
                    else
                    {
                        b.Content = $"{i++}. {item.Name} {item.Code}";
                    }
                    b.Click += DefClickButton;
                    stackPanel1.Children.Add(b);
                }

        }
        //private DateTime? IsCorectDate(string value)
        //{
        //    if (value.Length != 10) return null;

        //    string day = value.Substring(0, 2);
        //    string month = value.Substring(3, 2);
        //    string year = value.Substring(6, 4);

        //    int dayInt = 0;
        //    int monthInt = 0;
        //    int yearInt = 0;

        //    bool successDay = int.TryParse(day, out dayInt);
        //    bool successMath = int.TryParse(month, out monthInt);
        //    bool successYear = int.TryParse(year, out yearInt);

        //    if (successDay && successMath && successYear)
        //        return new DateTime(yearInt, monthInt, dayInt);


        //    return null;

        //}
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
