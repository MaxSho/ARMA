using DesARMA.Model3;
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
    /// Interaction logic for ListDefendantsWindow.xaml
    /// </summary>
    public partial class ListDefendantsWindow : Window
    {
        List<Def>  listConnectedPeople = new List<Def>();
        Dictionary<decimal, Def> listDictionaryConnectedPeople = new Dictionary<decimal, Def>();
        public List<Def> listDefendants = new List<Def>();
        ApplicationContext db;
        public Dictionary<decimal, Def> listDictionaryDefendants = new Dictionary<decimal, Def>();
        ModelContext modelContext;
        string numberIn;
        public bool isConn;
        Request? curR;
        int curRnum;
        public ListDefendantsWindow(int curRnum, ApplicationContext db, ModelContext modelContext, 
            string numberIn, string tit, bool isConn)
        {
            InitializeComponent();
            this.curRnum = curRnum;
            this.modelContext = modelContext;
            this.numberIn = numberIn;
            this.Title = tit;
            this.isConn = isConn;
            this.db = db;
            this.listConnectedPeople = new List<Def>();
            this.listDictionaryConnectedPeople = new Dictionary<decimal, Def>();
            this.listDefendants = new List<Def>();
            this.listDictionaryDefendants = new Dictionary<decimal, Def>();

            curR = db.Requests.Find(curRnum);
            if (curR==null)
                Close();

            var fos = from r in db.Requests
                         from rin in r.fos
                         where r.id == curRnum
                         select rin;
            var uos = from r in db.Requests
                       from rin in r.uos
                       where r.id == curRnum
                       select rin;

            foreach (var item in fos)
            {
                if (item.isConnectedPeople)
                {
                    listConnectedPeople.Add(item);
                    listDictionaryConnectedPeople.Add(item.IdinGlobDB, item);
                }
                else
                {
                    listDefendants.Add(item);
                    listDictionaryDefendants.Add(item.IdinGlobDB, item);
                }
            }

                foreach (var item in uos)
                {
                    if (item.isConnectedPeople)
                    {
                        listConnectedPeople.Add(item);
                        listDictionaryConnectedPeople.Add(item.IdinGlobDB, item);
                    }
                    else
                    {
                        listDefendants.Add(item);
                        listDictionaryDefendants.Add(item.IdinGlobDB, item);
                    }
                }
            
            UpdateListButton();

        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = true;
        }

        public string Password
        {
            get { return ""; }
        }

        private void Add_ClickFO(object sender, RoutedEventArgs e)
        {
            Def_FO_Window def_FO_Window = new Def_FO_Window(new FO(), isConn);

            if (def_FO_Window.ShowDialog() == true)
            {
               // MessageBox.Show($"{def_FO_Window.fo.name} \n {def_FO_Window.fo.code} \n {def_FO_Window.fo.dateB} \n {def_FO_Window.fo.isResid}");
                if (!def_FO_Window.isDelete)
                {
                    if (def_FO_Window.fo.isConnectedPeople) 
                    {
                        listConnectedPeople.Add(def_FO_Window.fo);
                        var temp = new Figurant() { NumbInput = numberIn, Name = def_FO_Window.fo.name, Ipn = def_FO_Window.fo.code, ResFiz = def_FO_Window.fo.isResid, DtBirth = IsCorectDate(def_FO_Window.fo.dateB) };
                        
                        modelContext.Figurants.Add(temp);
                        modelContext.SaveChanges();
                        listDictionaryConnectedPeople.Add(temp.Id, def_FO_Window.fo);

                        def_FO_Window.fo.IdinGlobDB = temp.Id;
                        if(curR != null)
                        {
                            curR.fos.Add(def_FO_Window.fo);
                        }
                        else
                        {
                            MessageBox.Show("Не знайдено запит");
                        }
                        
                        db.SaveChanges();
                    }
                    else
                    {
                        listDefendants.Add(def_FO_Window.fo);
                        var temp = new Figurant() { NumbInput = numberIn, Name = def_FO_Window.fo.name, Ipn = def_FO_Window.fo.code, ResFiz = def_FO_Window.fo.isResid, DtBirth = IsCorectDate(def_FO_Window.fo.dateB) };
                        modelContext.Figurants.Add(temp);
                        modelContext.SaveChanges();
                        listDictionaryDefendants.Add(temp.Id, def_FO_Window.fo);

                        def_FO_Window.fo.IdinGlobDB = temp.Id;
                        if (curR != null)
                        {
                            curR.fos.Add(def_FO_Window.fo);
                        }
                        else
                        {
                            MessageBox.Show("Не знайдено запит");
                        }
                        db.SaveChanges();
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
            Def_UO_Window def_UO_Window = new Def_UO_Window(new UO(), isConn);

            if (def_UO_Window.ShowDialog() == true)
            {
                //MessageBox.Show($"{def_UO_Window.uo.name} \n {def_UO_Window.uo.code}  \n {def_UO_Window.uo.isResid}");
                if (!def_UO_Window.isDelete)
                {
                    if (def_UO_Window.uo.isConnectedPeople)
                    {
                        listConnectedPeople.Add(def_UO_Window.uo);
                        var temp = new Figurant() { NumbInput = numberIn, Name = def_UO_Window.uo.name, Ipn = def_UO_Window.uo.code, ResFiz = def_UO_Window.uo.isResid };
                        modelContext.Figurants.Add(temp);
                        modelContext.SaveChanges();
                        listDictionaryConnectedPeople.Add(temp.Id, def_UO_Window.uo);

                        def_UO_Window.uo.IdinGlobDB = temp.Id;
                        if (curR != null)
                        {
                            curR.uos.Add(def_UO_Window.uo);
                        }
                        else
                        {
                            MessageBox.Show("Не знайдено запит");
                        }
                        
                        db.SaveChanges();
                    }
                    else{
                        listDefendants.Add(def_UO_Window.uo);
                        var temp = new Figurant() { NumbInput = numberIn, Name = def_UO_Window.uo.name, Ipn = def_UO_Window.uo.code, ResFiz = def_UO_Window.uo.isResid };
                        modelContext.Figurants.Add(temp);
                        modelContext.SaveChanges();
                        listDictionaryDefendants.Add(temp.Id, def_UO_Window.uo);


                        def_UO_Window.uo.IdinGlobDB = temp.Id;
                        if (curR != null)
                        {
                            curR.uos.Add(def_UO_Window.uo);
                        }
                        else
                        {
                            MessageBox.Show("Не знайдено запит");
                        }
                        db.SaveChanges();
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
            var b = sender as Button;
            if (b != null)
            {
                int ind = (int)b.Tag;
                var def = listDefendants[ind] as Def;
                var uo = def as UO;
                var fo = def as FO;
                if (uo!=null)
                {
                    Def_UO_Window def_UO_Window = new Def_UO_Window(uo, isConn);
                    if (def_UO_Window.ShowDialog() == true)
                    {
                        //MessageBox.Show($"{def_UO_Window.uo.name} \n {def_UO_Window.uo.code}  \n {def_UO_Window.uo.isResid}");
                        if (!def_UO_Window.isDelete)
                        {

                            foreach (var ValInDic in listDictionaryDefendants)
                            {
                                if (ValInDic.Value == uo)
                                {
                                    var editItem = modelContext.Figurants.Find(ValInDic.Key);
                                    if (editItem != null)
                                    {
                                        editItem.Code = uo.code;
                                        editItem.Name = uo.name;
                                        editItem.ResUr = uo.isResid;
                                        modelContext.SaveChanges();


                                        UO uOLocal = null!;
                                        if (curR != null)
                                        {
                                            foreach (var item in curR.uos)
                                            {
                                                if (item.IdinGlobDB == uo.IdinGlobDB)
                                                {
                                                    uOLocal = item;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Не знайдено запит");
                                        }
                                        
                                        if (uOLocal != null)
                                        {
                                            uOLocal.code = uo.code;
                                            uOLocal.name = uo.name;
                                            uOLocal.isResid = uo.isResid;
                                            db.SaveChanges();
                                        }
                                    }
                                    listDefendants[ind] = uo;
                                }
                            }
                        }
                        else
                        {
                            db.UOs.Remove(uo);
                            db.SaveChanges();
                            //curR.uos.Remove(uo);
                            listDefendants.RemoveAt(ind);

                            foreach (var ValInDic in listDictionaryDefendants)
                            {
                                if (ValInDic.Value == uo)
                                {
                                    var editItem = modelContext.Figurants.Find(ValInDic.Key);
                                    if (editItem != null)
                                    {
                                        modelContext.Figurants.Remove(editItem);
                                        modelContext.SaveChanges();
                                    }
                                }
                            }
                        }
                        UpdateListButton();
                    }
                    else
                    {
                        // MessageBox.Show("Авторизация не пройдена");
                    }
                }
                if (fo != null)
                {
                    Def_FO_Window def_FO_Window = new Def_FO_Window(fo, isConn);
                    if (def_FO_Window.ShowDialog() == true)
                    {
                        //MessageBox.Show($"{def_UO_Window.uo.name} \n {def_UO_Window.uo.code}  \n {def_UO_Window.uo.isResid}");
                        if (!def_FO_Window.isDelete)
                        {
                            foreach (var ValInDic in listDictionaryDefendants)
                            {
                                if (ValInDic.Value == fo)
                                {
                                    var editItem = modelContext.Figurants.Find(ValInDic.Key);
                                    if (editItem != null)
                                    {
                                        editItem.Code = fo.code;
                                        editItem.Name = fo.name;
                                        editItem.ResUr = fo.isResid;
                                        modelContext.SaveChanges();


                                        FO fOLocal = null!;
                                        if (curR != null)
                                        {
                                            foreach (var item in curR.fos)
                                            {
                                                if (item.IdinGlobDB == fo.IdinGlobDB)
                                                {
                                                    fOLocal = item;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Запит не створенно");
                                        }
                                        
                                        if (fOLocal != null)
                                        {
                                            fOLocal.code = fo.code;
                                            fOLocal.name = fo.name;
                                            fOLocal.isResid = fo.isResid;
                                            db.SaveChanges();
                                        }
                                    }
                                    listDefendants[ind] = fo;
                                }
                            }
                        }
                        else
                        {
                            db.FOs.Remove(fo);
                            db.SaveChanges();
                            listDefendants.RemoveAt(ind);
                            foreach (var ValInDic in listDictionaryDefendants)
                            {
                                if (ValInDic.Value == fo)
                                {
                                    var editItem = modelContext.Figurants.Find(ValInDic.Key);
                                    if (editItem != null)
                                    {
                                        modelContext.Figurants.Remove(editItem);
                                        modelContext.SaveChanges();
                                    }
                                }
                            }
                        }
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
            if(isConn)
                foreach (var item in listConnectedPeople)
                {
                    var b = new Button();
                    b.FontSize = 16;
                    b.Tag = i - 1;
                    b.Content = $"{i++}. {item.name} {item.code}";
                    b.Click += DefClickButton;
                    stackPanel1.Children.Add(b);
                }
            else
                foreach (var item in listDefendants)
                {
                    var b = new Button();
                    b.FontSize = 16;
                    b.Tag = i - 1;
                    b.Content = $"{i++}. {item.name} {item.code}";
                    b.Click += DefClickButton;
                    stackPanel1.Children.Add(b);
                }
            
        }
        private DateTime? IsCorectDate(string value)
        {
            if (value.Length != 10) return null;

            string day = value.Substring(0, 2);
            string month = value.Substring(3, 2);
            string year = value.Substring(6, 4);

            int dayInt = 0;
            int monthInt = 0;
            int yearInt = 0;

            bool successDay = int.TryParse(day, out dayInt);
            bool successMath = int.TryParse(month, out monthInt);
            bool successYear = int.TryParse(year, out yearInt);

            if (successDay && successMath && successYear)
                return new DateTime(yearInt, monthInt, dayInt);


            return null;

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
