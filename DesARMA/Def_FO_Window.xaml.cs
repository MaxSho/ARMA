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
    /// Interaction logic for Def_FO_Window.xaml
    /// </summary>
    public partial class Def_FO_Window : Window
    {
        public FO fo = new FO();
        public bool isDelete;

        public Def_FO_Window(FO fo, bool isConn)
        {
            InitializeComponent();
            this.fo = fo;
            nameTextBox.Text = fo.name;
            codeTextBox.Text = fo.code;
            if (fo.dateB != "" && fo.dateB != null)
                dateDatePicker.Text = $"{fo.dateB.Substring(0, 2)}.{fo.dateB.Substring(3, 2)}.{fo.dateB.Substring(6,4)}" /*r.dateOut*/;
            fo.isConnectedPeople = isConn;
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            fo.name = nameTextBox.Text;
            fo.code = codeTextBox.Text;
            fo.dateB = dateDatePicker.Text;
            var chbIsch = residentTextBox.IsChecked;

            if (chbIsch != null)
            {
                fo.isResid = (bool)(chbIsch);
            }
            else
            {
                fo.isResid = false;
            }

            if(nameTextBox.Text == "" ||  dateDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Не заповнені поля");
            }
            else
            {
                this.DialogResult = true;
            }
        }
      

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            isDelete = true;

            this.DialogResult = true;
        }
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox dp = (TextBox)sender;
                var i = dp.SelectionStart;
                //Regex regex = new Regex("^[0-9]{1,2}.[0-9]{1,2}.[0-9]{4}$"); //regex that matches allowed text
                //e.Handled = regex.IsMatch(e.Text);

                // MessageBox.Show(i+"");
                string newstr = "";
                if ("0123456789".IndexOf(dp.Text[0]) == -1)
                {
                    newstr += "_";
                }
                else
                {
                    newstr += dp.Text[0];
                }

                if (dp.Text.Length > 1)
                    if ("0123456789".IndexOf(dp.Text[1]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[1];
                    }


                newstr += ".";

                if (dp.Text.Length > 3)
                    if ("0123456789".IndexOf(dp.Text[3]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[3];
                    }

                if (dp.Text.Length > 4)
                    if ("0123456789".IndexOf(dp.Text[4]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[4];
                    }


                newstr += ".";

                if (dp.Text.Length > 6)
                    if ("0123456789".IndexOf(dp.Text[6]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[6];
                    }

                if (dp.Text.Length > 7)
                    if ("0123456789".IndexOf(dp.Text[7]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[7];
                    }

                if (dp.Text.Length > 8)
                    if ("0123456789".IndexOf(dp.Text[8]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[8];
                    }

                if (dp.Text.Length > 9)
                    if ("0123456789".IndexOf(dp.Text[9]) == -1)
                    {
                        newstr += "_";
                    }
                    else
                    {
                        newstr += dp.Text[9];
                    }

                dp.Text = newstr;
                if (i == 2 && dp.Text[2] == '.') i++;
                if (i == 5 && dp.Text[5] == '.') i++;
                dp.SelectionStart = i;
                
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void textBox_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                TextBox dp = (TextBox)sender;
                dp.SelectionStart = 0;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void CalcDateBut(object sender, RoutedEventArgs e)
        {
            if (codeTextBox.Text.Length<5)
            {
                MessageBox.Show("ІПН менше 5 цифр");
                return;
            }

            var days = codeTextBox.Text.Substring(0, 5);
            var dt = new DateTime(1899,12,31);
            int daysInt = 0;

            bool successDays = int.TryParse(days, out daysInt);

            if (successDays)
            {
                var dt2 = dt.AddDays(daysInt);

               // MessageBox.Show(dt2+"");
                dateDatePicker.SelectedDate = dt2;
            }
            else
            {
                MessageBox.Show("Невдалось конвертувати");
            }
                
        }
    }
}
