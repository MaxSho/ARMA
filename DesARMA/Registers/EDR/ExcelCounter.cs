using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesARMA.Registers.EDR
{
    internal class ExcelCounter
    {
        public int counter;
        public ExcelCounter()
        {
            counter = 1;
        }
        public string GetGetStringForNumber(int c)
        {
            string ret = "";
            while (c > 0)
            {
                int ost = c % 26;
                if (ost == 0)
                {
                    ret = 'Z' + ret;
                    c = c / 26 - 1;
                }
                else
                {
                    ret = Convert.ToChar('A' + ost - 1) + ret;
                    c = c / 26;
                }

            }
            return ret;
        }
        public string GetStringCounter()
        {
            int c = counter;
            string ret = "";
            while (c > 0)
            {
                int ost = c % 26;
                if(ost == 0)
                {
                    ret = 'Z' + ret;
                    c = c / 26 - 1;
                }
                else
                {
                    ret = Convert.ToChar('A' + ost - 1) + ret;
                    c = c / 26;
                }
                
            }

            counter++;
            return ret;
        }
    }
}
