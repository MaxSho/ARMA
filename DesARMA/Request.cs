
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DesARMA
{
    public static class f
    {
        public static void SetSchemeDB(this Request request, int num, bool value)
        {
            switch (num)
            {
                case 1: request.isSchemeDB1 = value; break;
                case 2: request.isSchemeDB2 = value; break;
                case 3: request.isSchemeDB3 = value; break;
                case 4: request.isSchemeDB4 = value; break;
                case 5: request.isSchemeDB5 = value; break;
                case 6: request.isSchemeDB6 = value; break;
                case 7: request.isSchemeDB7 = value; break;
                case 8: request.isSchemeDB8 = value; break;
                case 9: request.isSchemeDB9 = value; break;
                case 10: request.isSchemeDB10 = value; break;
                case 11: request.isSchemeDB11 = value; break;
                case 12: request.isSchemeDB12 = value; break;
                case 13: request.isSchemeDB13 = value; break;
                case 14: request.isSchemeDB14 = value; break;
                case 15: request.isSchemeDB15 = value; break;
                case 16: request.isSchemeDB16 = value; break;
                case 17: request.isSchemeDB17 = value; break;
                case 18: request.isSchemeDB18 = value; break;
                case 19: request.isSchemeDB19 = value; break;
                case 20: request.isSchemeDB20 = value; break;
                case 21: request.isSchemeDB21 = value; break;
                case 22: request.isSchemeDB22 = value; break;
                case 23: request.isSchemeDB23 = value; break;
                case 24: request.isSchemeDB24 = value; break;
                case 25: request.isSchemeDB25 = value; break;
                case 26: request.isSchemeDB26 = value; break;
                case 27: request.isSchemeDB27 = value; break;
                case 28: request.isSchemeDB28 = value; break;
                case 29: request.isSchemeDB29 = value; break;
                case 30: request.isSchemeDB30 = value; break;
                case 31: request.isSchemeDB31 = value; break;
                default:
                    break;
            }
        }
        public static void SetDB(this Request request, int num, bool value)
        {
            switch (num)
            {
                case 1: request.isDB1 = value; break;
                case 2: request.isDB2 = value; break;
                case 3: request.isDB3 = value; break;
                case 4: request.isDB4 = value; break;
                case 5: request.isDB5 = value; break;
                case 6: request.isDB6 = value; break;
                case 7: request.isDB7 = value; break;
                case 8: request.isDB8 = value; break;
                case 9: request.isDB9 = value; break;
                case 10: request.isDB10 = value; break;
                case 11: request.isDB11 = value; break;
                case 12: request.isDB12 = value; break;
                case 13: request.isDB13 = value; break;
                case 14: request.isDB14 = value; break;
                case 15: request.isDB15 = value; break;
                case 16: request.isDB16 = value; break;
                case 17: request.isDB17 = value; break;
                case 18: request.isDB18 = value; break;
                case 19: request.isDB19 = value; break;
                case 20: request.isDB20 = value; break;
                case 21: request.isDB21 = value; break;
                case 22: request.isDB22 = value; break;
                case 23: request.isDB23 = value; break;
                case 24: request.isDB24 = value; break;
                case 25: request.isDB25 = value; break;
                case 26: request.isDB26 = value; break;
                case 27: request.isDB27 = value; break;
                case 28: request.isDB28 = value; break;
                case 29: request.isDB29 = value; break;
                case 30: request.isDB30 = value; break;
                case 31: request.isDB31 = value; break;
                default:
                    break;
            }
        }
        public static void SetNecessaryDB(this Request request, int num, bool value)
        {
            switch (num)
            {
                case 1: request.isNecessaryDB1 = value; break;
                case 2: request.isNecessaryDB2 = value; break;
                case 3: request.isNecessaryDB3 = value; break;
                case 4: request.isNecessaryDB4 = value; break;
                case 5: request.isNecessaryDB5 = value; break;
                case 6: request.isNecessaryDB6 = value; break;
                case 7: request.isNecessaryDB7 = value; break;
                case 8: request.isNecessaryDB8 = value; break;
                case 9: request.isNecessaryDB9 = value; break;
                case 10: request.isNecessaryDB10 = value; break;
                case 11: request.isNecessaryDB11 = value; break;
                case 12: request.isNecessaryDB12 = value; break;
                case 13: request.isNecessaryDB13 = value; break;
                case 14: request.isNecessaryDB14 = value; break;
                case 15: request.isNecessaryDB15 = value; break;
                case 16: request.isNecessaryDB16 = value; break;
                case 17: request.isNecessaryDB17 = value; break;
                case 18: request.isNecessaryDB18 = value; break;
                case 19: request.isNecessaryDB19 = value; break;
                case 20: request.isNecessaryDB20 = value; break;
                case 21: request.isNecessaryDB21 = value; break;
                case 22: request.isNecessaryDB22 = value; break;
                case 23: request.isNecessaryDB23 = value; break;
                case 24: request.isNecessaryDB24 = value; break;
                case 25: request.isNecessaryDB25 = value; break;
                case 26: request.isNecessaryDB26 = value; break;
                case 27: request.isNecessaryDB27 = value; break;
                case 28: request.isNecessaryDB28 = value; break;
                case 29: request.isNecessaryDB29 = value; break;
                case 30: request.isNecessaryDB30 = value; break;
                case 31: request.isNecessaryDB31 = value; break;
                default:
                    break;
            }
        }
        public static bool? GetDB(this Request request, int num)
        {
            switch (num)
            {
                case 1: return request.isDB1;  
                case 2: return request.isDB2;  
                case 3: return request.isDB3;  
                case 4: return request.isDB4;  
                case 5: return request.isDB5;  
                case 6: return request.isDB6;  
                case 7: return request.isDB7;  
                case 8: return request.isDB8;  
                case 9: return request.isDB9;  
                case 10: return request.isDB10; 
                case 11: return request.isDB11; 
                case 12: return request.isDB12; 
                case 13: return request.isDB13; 
                case 14: return request.isDB14; 
                case 15: return request.isDB15; 
                case 16: return request.isDB16; 
                case 17: return request.isDB17; 
                case 18: return request.isDB18; 
                case 19: return request.isDB19; 
                case 20: return request.isDB20; 
                case 21: return request.isDB21; 
                case 22: return request.isDB22; 
                case 23: return request.isDB23; 
                case 24: return request.isDB24; 
                case 25: return request.isDB25; 
                case 26: return request.isDB26; 
                case 27: return request.isDB27; 
                case 28: return request.isDB28; 
                case 29: return request.isDB29; 
                case 30: return request.isDB30; 
                case 31: return request.isDB31; 
                default:
                    break;
            }
            return null;
        }
        public static bool? GetNecessaryDB(this Request request, int num)
        {
            switch (num)
            {
                case 1: return request.isNecessaryDB1; 
                case 2: return request.isNecessaryDB2; 
                case 3: return request.isNecessaryDB3; 
                case 4: return request.isNecessaryDB4; 
                case 5: return request.isNecessaryDB5; 
                case 6: return request.isNecessaryDB6; 
                case 7: return request.isNecessaryDB7; 
                case 8: return request.isNecessaryDB8; 
                case 9: return request.isNecessaryDB9; 
                case 10: return request.isNecessaryDB10; 
                case 11: return request.isNecessaryDB11; 
                case 12: return request.isNecessaryDB12; 
                case 13: return request.isNecessaryDB13; 
                case 14: return request.isNecessaryDB14; 
                case 15: return request.isNecessaryDB15; 
                case 16: return request.isNecessaryDB16; 
                case 17: return request.isNecessaryDB17; 
                case 18: return request.isNecessaryDB18; 
                case 19: return request.isNecessaryDB19; 
                case 20: return request.isNecessaryDB20; 
                case 21: return request.isNecessaryDB21; 
                case 22: return request.isNecessaryDB22; 
                case 23: return request.isNecessaryDB23; 
                case 24: return request.isNecessaryDB24; 
                case 25: return request.isNecessaryDB25; 
                case 26: return request.isNecessaryDB26; 
                case 27: return request.isNecessaryDB27; 
                case 28: return request.isNecessaryDB28; 
                case 29: return request.isNecessaryDB29; 
                case 30: return request.isNecessaryDB30; 
                case 31: return request.isNecessaryDB31; 
                
                    
            }
            return null;
        }
        public static bool? GetSchemeDB(this Request request, int num)
        {
            switch (num)
            {
                case 1: return request.isSchemeDB1; 
                case 2: return request.isSchemeDB2; 
                case 3: return request.isSchemeDB3; 
                case 4: return request.isSchemeDB4; 
                case 5: return request.isSchemeDB5; 
                case 6: return request.isSchemeDB6; 
                case 7: return request.isSchemeDB7; 
                case 8: return request.isSchemeDB8; 
                case 9: return request.isSchemeDB9; 
                case 10: return request.isSchemeDB10; 
                case 11: return request.isSchemeDB11; 
                case 12: return request.isSchemeDB12; 
                case 13: return request.isSchemeDB13; 
                case 14: return request.isSchemeDB14; 
                case 15: return request.isSchemeDB15; 
                case 16: return request.isSchemeDB16; 
                case 17: return request.isSchemeDB17; 
                case 18: return request.isSchemeDB18; 
                case 19: return request.isSchemeDB19; 
                case 20: return request.isSchemeDB20; 
                case 21: return request.isSchemeDB21; 
                case 22: return request.isSchemeDB22; 
                case 23: return request.isSchemeDB23; 
                case 24: return request.isSchemeDB24; 
                case 25: return request.isSchemeDB25; 
                case 26: return request.isSchemeDB26; 
                case 27: return request.isSchemeDB27; 
                case 28: return request.isSchemeDB28; 
                case 29: return request.isSchemeDB29; 
                case 30: return request.isSchemeDB30; 
                case 31: return request.isSchemeDB31; 
                
                    
            }
            return null;
        }
    }
    public class EnumDBis: IEnumerable
    {
        Request request;
        public EnumDBis(Request request)
        {
            this.request = request;
        }
        public IEnumerable GetScheme()
        {
            yield return request.isSchemeDB1;
            yield return request.isSchemeDB2;
            yield return request.isSchemeDB3;
            yield return request.isSchemeDB4;
            yield return request.isSchemeDB5;
            yield return request.isSchemeDB6;
            yield return request.isSchemeDB7;
            yield return request.isSchemeDB8;
            yield return request.isSchemeDB9;
            yield return request.isSchemeDB10;
            yield return request.isSchemeDB11;
            yield return request.isSchemeDB12;
            yield return request.isSchemeDB13;
            yield return request.isSchemeDB14;
            yield return request.isSchemeDB15;
            yield return request.isSchemeDB16;
            yield return request.isSchemeDB17;
            yield return request.isSchemeDB18;
            yield return request.isSchemeDB19;
            yield return request.isSchemeDB20;
            yield return request.isSchemeDB21;
            yield return request.isSchemeDB22;
            yield return request.isSchemeDB23;
            yield return request.isSchemeDB24;
            yield return request.isSchemeDB25;
            yield return request.isSchemeDB26;
            yield return request.isSchemeDB27;
            yield return request.isSchemeDB28;
            yield return request.isSchemeDB29;
            yield return request.isSchemeDB30;
            yield return request.isSchemeDB31;
        }
        public IEnumerable GetNecessary()
        {
            yield return request.isNecessaryDB1;
            yield return request.isNecessaryDB2;
            yield return request.isNecessaryDB3;
            yield return request.isNecessaryDB4;
            yield return request.isNecessaryDB5;
            yield return request.isNecessaryDB6;
            yield return request.isNecessaryDB7;
            yield return request.isNecessaryDB8;
            yield return request.isNecessaryDB9;
            yield return request.isNecessaryDB10;
            yield return request.isNecessaryDB11;
            yield return request.isNecessaryDB12;
            yield return request.isNecessaryDB13;
            yield return request.isNecessaryDB14;
            yield return request.isNecessaryDB15;
            yield return request.isNecessaryDB16;
            yield return request.isNecessaryDB17;
            yield return request.isNecessaryDB18;
            yield return request.isNecessaryDB19;
            yield return request.isNecessaryDB20;
            yield return request.isNecessaryDB21;
            yield return request.isNecessaryDB22;
            yield return request.isNecessaryDB23;
            yield return request.isNecessaryDB24;
            yield return request.isNecessaryDB25;
            yield return request.isNecessaryDB26;
            yield return request.isNecessaryDB27;
            yield return request.isNecessaryDB28;
            yield return request.isNecessaryDB29;
            yield return request.isNecessaryDB30;
            yield return request.isNecessaryDB31;
        }
        public IEnumerator GetEnumerator()
        {
            yield return request.isDB1;
            yield return request.isDB2;
            yield return request.isDB3;
            yield return request.isDB4;
            yield return request.isDB5;
            yield return request.isDB6;
            yield return request.isDB7;
            yield return request.isDB8;
            yield return request.isDB9;
            yield return request.isDB10;
            yield return request.isDB11;
            yield return request.isDB12;
            yield return request.isDB13;
            yield return request.isDB14;
            yield return request.isDB15;
            yield return request.isDB16;
            yield return request.isDB17;
            yield return request.isDB18;
            yield return request.isDB19;
            yield return request.isDB20;
            yield return request.isDB21;
            yield return request.isDB22;
            yield return request.isDB23;
            yield return request.isDB24;
            yield return request.isDB25;
            yield return request.isDB26;
            yield return request.isDB27;
            yield return request.isDB28;
            yield return request.isDB29;
            yield return request.isDB30;
            yield return request.isDB31;
        }
        //IEnumerator<bool?> IEnumerable<bool?>.GetEnumerator()
        //{
        //    yield return request.isDB1;
        //    yield return request.isDB2;
        //    yield return request.isDB3;
        //    yield return request.isDB4;
        //    yield return request.isDB5;
        //    yield return request.isDB7;
        //    yield return request.isDB8;
        //    yield return request.isDB9;
        //    yield return request.isDB10;
        //    yield return request.isDB11;
        //    yield return request.isDB12;
        //    yield return request.isDB13;
        //    yield return request.isDB14;
        //    yield return request.isDB15;
        //    yield return request.isDB16;
        //    yield return request.isDB17;
        //    yield return request.isDB18;
        //    yield return request.isDB19;
        //    yield return request.isDB20;
        //    yield return request.isDB21;
        //    yield return request.isDB22;
        //    yield return request.isDB23;
        //    yield return request.isDB24;
        //    yield return request.isDB25;
        //    yield return request.isDB26;
        //    yield return request.isDB27;
        //    yield return request.isDB28;
        //    yield return request.isDB29;
        //    yield return request.isDB30;
        //}
    }
    public class Request
    {
        public int id { get; set; }
        public string numberKP { get; set; } = "";
        public string numberIn { get; set; } = "";
        public string dateIn { get; set; }  = "";
        public string dateControl { get; set; } = "";   
        public int typeOrgan { get; set; }
        public string numberRequest { get; set; } = "";
        public DateTime? dateRequest { get; set; }
        public string vidOrg { get; set; } = "";
        public string addressOrg { get; set; } = "";
        public string positionSub { get; set; } = "";
        public string nameSub { get; set; } = "";
        public string numberOut { get; set; } = "";
        public DateTime? dateOut { get; set; }
        public string co_executor { get; set; } = "";
        public string TEKA { get; set; } = "";
        public string article_CCU { get; set; } = "";
        public string note { get; set; } = "";
        public int typeAppea { get;set;}    
        public string connectedPeople { get; set; } = "";
        public string pathDirectory { get; set; } = "";
        public int count_Shemat { get; set; }   
        public bool isSubs { get; set; }
        public bool isDB1 { get; set; }
        public bool isDB2 { get; set; }
        public bool isDB3 { get; set; }
        public bool isDB4 { get; set; }
        public bool isDB5 { get; set; }
        public bool isDB6 { get; set; }
        public bool isDB7 { get; set; }
        public bool isDB8 { get; set; }
        public bool isDB9 { get; set; }
        public bool isDB10 { get; set; }
        public bool isDB11 { get; set; }
        public bool isDB12 { get; set; }
        public bool isDB13 { get; set; }
        public bool isDB14 { get; set; }
        public bool isDB15 { get; set; }
        public bool isDB16 { get; set; }
        public bool isDB17 { get; set; }
        public bool isDB18 { get; set; }
        public bool isDB19 { get; set; }
        public bool isDB20 { get; set; }
        public bool isDB21 { get; set; }
        public bool isDB22 { get; set; }
        public bool isDB23 { get; set; }
        public bool isDB24 { get; set; }
        public bool isDB25 { get; set; }
        public bool isDB26 { get; set; }
        public bool isDB27 { get; set; }
        public bool isDB28 { get; set; }
        public bool isDB29 { get; set; }
        public bool isDB30 { get; set; }
        public bool isDB31 { get; set; }
        public bool isNecessaryDB1 { get; set; }
        public bool isNecessaryDB2 { get; set; }
        public bool isNecessaryDB3 { get; set; }
        public bool isNecessaryDB4 { get; set; }
        public bool isNecessaryDB5 { get; set; }
        public bool isNecessaryDB6 { get; set; }
        public bool isNecessaryDB7 { get; set; }
        public bool isNecessaryDB8 { get; set; }
        public bool isNecessaryDB9 { get; set; }
        public bool isNecessaryDB10 { get; set; }
        public bool isNecessaryDB11 { get; set; }
        public bool isNecessaryDB12 { get; set; }
        public bool isNecessaryDB13 { get; set; }
        public bool isNecessaryDB14 { get; set; }
        public bool isNecessaryDB15 { get; set; }
        public bool isNecessaryDB16 { get; set; }
        public bool isNecessaryDB17 { get; set; }
        public bool isNecessaryDB18 { get; set; }
        public bool isNecessaryDB19 { get; set; }
        public bool isNecessaryDB20 { get; set; }
        public bool isNecessaryDB21 { get; set; }
        public bool isNecessaryDB22 { get; set; }
        public bool isNecessaryDB23 { get; set; }
        public bool isNecessaryDB24 { get; set; }
        public bool isNecessaryDB25 { get; set; }
        public bool isNecessaryDB26 { get; set; }
        public bool isNecessaryDB27 { get; set; }
        public bool isNecessaryDB28 { get; set; }
        public bool isNecessaryDB29 { get; set; }
        public bool isNecessaryDB30 { get; set; }
        public bool isNecessaryDB31 { get; set; }
        public bool isSchemeDB1 { get; set; }
        public bool isSchemeDB2 { get; set; }
        public bool isSchemeDB3 { get; set; }
        public bool isSchemeDB4 { get; set; }
        public bool isSchemeDB5 { get; set; }
        public bool isSchemeDB6 { get; set; }
        public bool isSchemeDB7 { get; set; }
        public bool isSchemeDB8 { get; set; }
        public bool isSchemeDB9 { get; set; }
        public bool isSchemeDB10 { get; set; }
        public bool isSchemeDB11 { get; set; }
        public bool isSchemeDB12 { get; set; }
        public bool isSchemeDB13 { get; set; }
        public bool isSchemeDB14 { get; set; }
        public bool isSchemeDB15 { get; set; }
        public bool isSchemeDB16 { get; set; }
        public bool isSchemeDB17 { get; set; }
        public bool isSchemeDB18 { get; set; }
        public bool isSchemeDB19 { get; set; }
        public bool isSchemeDB20 { get; set; }
        public bool isSchemeDB21 { get; set; }
        public bool isSchemeDB22 { get; set; }
        public bool isSchemeDB23 { get; set; }
        public bool isSchemeDB24 { get; set; }
        public bool isSchemeDB25 { get; set; }
        public bool isSchemeDB26 { get; set; }
        public bool isSchemeDB27 { get; set; }
        public bool isSchemeDB28 { get; set; }
        public bool isSchemeDB29 { get; set; }
        public bool isSchemeDB30 { get; set; }
        public bool isSchemeDB31 { get; set; }
        public List<SRod> sRods { get; set; }
        public List<SDav> sDavs { get; set; }
        public List<FO> fos { get; set; }
        public List<UO> uos { get; set; }
        public User user { get; set; }
        public Request()
        {
            this.sRods = new List<SRod>();
            this.sDavs = new List<SDav>();
            this.fos = new List<FO>();
            this.uos = new List<UO>(); 
            this.user = new User();
        }
    }

    public class SRod 
    {
        public int id { get; set; }
        public string name { get; set; } = "";
        public bool isDone { get; set; }
        public List<Request> inrequests { get; set; } = null!;
    }
    public class SDav
    {
        public int id { get; set; }
        public string name { get; set; } = "";
        public bool isDone { get; set; }
        public List<Request> inrequests { get; set; } = null!;
    }
}
