using Newtonsoft.Json.Linq;
using NPOI.POIFS.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DesARMA.Automation
{
    public enum SearchType
    {
        Base, // - дані ЮО, ФО;
        Branch, // - дані ВП;
        Beneficiar, // - дані бенефіціарів (в тому числі в неструктурованому вигляді);
        Founder, // - дані засновників,
        Chief, // - дані керівника,
        Assignee, // - дані представників
    }
    class SearchEDR
    {
        private string? code;
        private string? name;
        private string? passport;
        private int limit;
        private SearchType? searchType;

        private string? Reqstr;

        public SearchEDR(string? code, string? name, string? passport, int limit, SearchType? searchType)
        {
            if (searchType == null)
                this.searchType = SearchType.Base;
            else
                this.searchType = searchType;

            this.code = code;
            this.name = name;
            this.passport = passport;
            this.limit = limit;
            

            SetParamsInReqstr();

        }
        private string? GetStr()
        {
            var appVal = ConfigurationManager.AppSettings["rs"];
            if (appVal != null)
            {
                string shif = appVal.ToString();
                List<byte> arrByteReturn = new List<byte>();
                List<byte> arrByteReturnDecrypt = new List<byte>();
                for (int i = 3; i <= shif.Length; i += 3)
                {
                    var subStr = shif.Substring(i - 3, 3);
                    arrByteReturn.Add(Convert.ToByte(subStr));
                }

                List<byte> key = new List<byte>();
                for (int i = arrByteReturn.Count - 8; i < arrByteReturn.Count; i++)
                {
                    key.Add(arrByteReturn[i]);
                }

                for (int i = 0; i < arrByteReturn.Count - key.Count; i++)
                {
                    arrByteReturnDecrypt.Add(Convert.ToByte(arrByteReturn[i] ^ key[i % key.Count]));
                }

                string result2 = System.Text.Encoding.UTF8.GetString(arrByteReturnDecrypt.ToArray());
                return result2;
            }
            return null;
        }
        private void SetParamsInReqstr()
        {
            this.Reqstr = GetStr();

            if (this.Reqstr == null)
            {
                throw new Exception("string in config error");
            }
            this.Reqstr = this.Reqstr.SetCode(code)
                .SetName(name)
                .SetPassport(passport)
                .SetLimit(limit)
                .SetSearchType(searchType);
        }
        
        public async Task GetResp()
        {
            var client = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(Reqstr!),
                Headers =
            {
                { "Authorization", "6c48e3a0948ec23c5de170299134e98ee2ff90e0" },
                //{"Accept", "application/json" }
            }
            };
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Token 6c48e3a0948ec23c5de170299134e98ee2ff90e0");
            
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                System.Windows.MessageBox.Show(content);
            }
            else
            {
                System.Windows.MessageBox.Show($"Request failed: {response.Content}");
            }
        }
    }
    public static class StringExtension
    {
        public static string? SetCode(this string? str, string? strIns)
        {
            if (str != null)
            {
                int ind = str.IndexOf("code");
                if (ind != -1)
                {
                    return str.Insert(ind + "code=".Length, "" + strIns);
                }
            }
            return str;
        }
        public static string? SetName(this string? str, string? strIns)
        {
            if (str != null)
            {
                int ind = str.IndexOf("name");
                if (ind != -1)
                {
                    return str.Insert(ind + "name=".Length, "" + strIns);
                }
            }
            return str;
        }
        public static string? SetPassport(this string? str, string? strIns)
        {
            if (str != null)
            {
                int ind = str.IndexOf("passport");
                if (ind != -1)
                {
                    return str.Insert(ind + "passport=".Length, ""+strIns);
                }
            }
            return str;
        }
        public static string? SetLimit(this string? str, int intIns)
        {
            if (str != null)
            {
                int ind = str.IndexOf("limit");
                if (ind != -1)
                {
                    return str.Insert(ind + "limit=".Length, "" + intIns);
                }
            }
            return str;
        }
        public static string? SetSearchType(this string? str, SearchType? stIns)
        {
            if (str != null)
            {
                int ind = str.IndexOf("search_type");
                if (ind != -1)
                {
                    return str.Insert(ind + "search_type=".Length, GetStringForSearchType(stIns));
                }
            }
            return str;
        }
        private static string GetStringForSearchType(SearchType? stIns)
        {
            if(stIns == null)
            {
                return "base";
            }
            else if (stIns == SearchType.Base)
            {
                return "base";
            }
            else if (stIns == SearchType.Branch)
            {
                return "branch";
            }
            else if (stIns == SearchType.Beneficiar)
            {
                return "beneficiar";
            }
            else if (stIns == SearchType.Founder)
            {
                return "founder";
            }
            else if (stIns == SearchType.Chief)
            {
                return "chief";
            }
            else if (stIns == SearchType.Assignee)
            {
                return "assignee";
            }
            return "base";
        }
    }
    
}
