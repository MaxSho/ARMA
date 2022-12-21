using DesARMA.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DesARMA
{
    public class Req

    {
        public static string RunList(string code, int numPage)
        {
            string Response = "";
            try
            {
                HttpWebRequest _request;
                string _adress = $"http://dozvil.ndiop.kiev.ua/dozvil/search?number_permission%5B0%5D=&number_permission%5B1%5D=&number_permission%5B2%5D=&number_permission%5B3%5D=&number_permission%5B4%5D=&number_permission%5B5%5D=&issuing_permit=&status_taxpayer=0&code_taxpayer={code}&name_enterprise=&types_work%5B%5D=0&handling_objects%5B%5D=0&application_objects%5B%5D=0&resolution=0&expires=0&date_issue_permit=&date_issue_duplicate=&page={numPage}";
                _request = WebRequest.Create(_adress) as HttpWebRequest;
                _request.Method = "GET";




                using (HttpWebResponse response = _request.GetResponse() as HttpWebResponse)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        Response = new StreamReader(stream).ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return Response;
        }

        public static string RunEbout(int id)
        {
            string Response = "";
            try
            {
                HttpWebRequest _request;
                string _adress = $"http://dozvil.ndiop.kiev.ua/dozvil/get/?id={id}";
                _request = WebRequest.Create(_adress) as HttpWebRequest;
                _request.Method = "GET";



                using (HttpWebResponse response = _request.GetResponse() as HttpWebResponse)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        Response = new StreamReader(stream).ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return Response;
        }

        public static List<string> Pars(Figurant fig)
        {
            List<string> listOrg = new List<string>();
            string? code;
            if (fig.ResFiz != null)
                code = fig.Ipn;
            else
                code = fig.Code;

            var st = Req.RunList(code+"", 1);

            var stCountOnPage = st.Split("dle_per_page = ")[1];
            var indFinishStCount = stCountOnPage.IndexOf(';');
            var dle_per_page = Convert.ToInt32(stCountOnPage.Substring(0, indFinishStCount));
            //Console.WriteLine(dle_per_page);

            var stCountAll = st.Split("dle_records = ")[1];
            var indFinishStCountAll = stCountAll.IndexOf(';');
            var dle_records = Convert.ToInt32(stCountAll.Substring(0, indFinishStCountAll));
            //Console.WriteLine(dle_records);

            int CountP = 0;
            if (dle_records % dle_per_page == 0) CountP = dle_records / dle_per_page;
            else CountP = dle_records / dle_per_page + 1;
            for (int page = 1; page <= CountP; page++)
            {
                st = Req.RunList(code + "", page);
                var arr = st.Split("dozvil/get/?id=");
                for (int i = 1; i < arr.Length; i++)
                {


                    //Console.WriteLine(arr[1]);
                    var finishCode = arr[i].IndexOf('\"');
                    var id = Convert.ToInt32(arr[i].Substring(0, finishCode));
                    // Console.Write(id + ". ");

                    var stAbout = Req.RunEbout(id);
                    var arrStAbout = stAbout.Split("formresult");
                    var indexEndDiv = arrStAbout[2].IndexOf("</div>");
                    // Console.WriteLine(arrStAbout[2].Substring(2, indexEndDiv - 2));

                    listOrg.Add(arrStAbout[2].Substring(2, indexEndDiv - 2));
                    //Console.WriteLine(indexEndDiv);

                }

                //Console.WriteLine();
                //Console.WriteLine(new String('*', 100));
                //Console.WriteLine();
            }



            IEnumerable<string> distinctAges = listOrg.AsQueryable().Distinct();

            //Console.WriteLine("Distinct ages:");

            //foreach (var age in distinctAges)
            //    Console.WriteLine(age);

            return distinctAges.ToList();
        }

    }
}
