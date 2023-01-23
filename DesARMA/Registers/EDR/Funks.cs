using System;
using System.Collections.Generic;

namespace DesARMA.Registers.EDR
{
    public static class Funks
    {
        public static Head GetTheNewestHead(List<Head> heads)
        {
            Head head = null;
            int index = 0;
            if(heads != null)
            if (heads.Count > 0)
            {
                for (int i = 0; i < heads.Count; i++)
                {
                    if (Convert.ToDateTime(heads[i].appointment_date) > Convert.ToDateTime(heads[index].appointment_date))
                    {
                        head = heads[i];
                        index = i;
                    }
                }
            }
            return head;
        }
    }
}
