using DesARMA.Registers.EDR;
using Newtonsoft.Json;
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
using System.Windows;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using IronXL;
using System.Windows.Media;
using System.Xml.Linq;
using DesARMA.Registers;

namespace DesARMA.Automation
{
    public enum SearchType
    {
        Base, // - дані ЮО, ФО;
        Branch, // - дані ВП;
        Beneficiar, // - дані бенефіціарів (в тому числі в неструктурованому вигляді);
        Founder, // - дані засновників,
        Chief, // - дані керівника,
        Assignee // - дані представників
    }
    class SearchEDR: ISearch
    {
        HttpClient client = new HttpClient();

        private string? code;
        private string? name;
        private string? passport;
        private int limit;
        private SearchType? searchType;

        private string? Reqstr;
        private string? ReqstrId;

        private List<EDRClass>? subjects;
        private List<Subject>? subjectsMore;
        //ProgresWindow progresWindow = new ProgresWindow();

        private string path;
        public SearchEDR(string? code, string? name, string? passport, int limit, SearchType? searchType,
                        string path)
        {
            if (searchType == null)
                this.searchType = SearchType.Base;
            else
                this.searchType = searchType;

            this.code = code;
            this.name = name;
            this.passport = passport;
            this.limit = limit;

            if (!Directory.Exists(path))
                throw new Exception("Програма не може знайти шлях: " + path);

            this.path = path;

            SetParamsInReqstr();
            SetReqstrId();

            GetInfoSubjects();
            GetInfoSubjectsMore();

            

        }
        
        private string? GetWhithout(string? str)
        {
            return str?.Replace('\'', '-')?.
                Replace('\"', '-')?
                .Replace('\\', '-')?
                .Replace('/', '-')?
                .Replace('*', '-')?
                .Replace(':', '-')?
                .Replace('?', '-')?
                .Replace('«', '-')?
                .Replace('<', '-')?
                .Replace('>', '-')?
                .Replace('|', '-');
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
        private string? GetStrId()
        {
            var appVal = ConfigurationManager.AppSettings["rsid"];
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
        private void SetReqstrId()
        {
            this.ReqstrId = GetStrId();
            if (this.Reqstr == null)
            {
                throw new Exception("stringId in config error");
            }
        }
        public void GetInfoSubjects()
        {
            string response = "";
            var task = Task.Run(async () => { // Викликаємо метод GetResp() в асинхронному таску
                response = await GetResp();
                subjects = JsonConvert.DeserializeObject<List<EDRClass>>(response);
            });
            task.Wait(); // Очікуємо завершення таску

            
            
        }
        public void GetInfoSubjectsMore()
        {
            subjectsMore = new List<Subject>();
            if (subjects != null)
                for (int i = 0; i < subjects.Count; i++)
                {
                    var id = subjects[i].id;
                    if (id != null)
                    {
                        string response = "";
                        var task = Task.Run(async () => { // Викликаємо метод GetResp() в асинхронному таску
                            response = await GetRespId((uint)id);
                            subjectsMore.Add(JsonConvert.DeserializeObject<Subject>(response));
                        });
                        task.Wait(); // Очікуємо завершення таску

                        
                    }
                }
        }
        public async Task<string> GetResp()
        {
            //var client = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(Reqstr!),
                Headers =
                {
                    { "Authorization", "Token 6c48e3a0948ec23c5de170299134e98ee2ff90e0" },
                }
            };
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            else
            {
                throw new Exception($"Request failed: {response.StatusCode}");
            }
        }
        private async Task<string> GetRespId(uint id)
        {
           // var client = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(ReqstrId + $"{id}"),
                Headers =
                {
                    { "Authorization", "Token 6c48e3a0948ec23c5de170299134e98ee2ff90e0" },
                }
            };
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            else
            {
                throw new Exception($"Request failed: {response.StatusCode}");
            }
        }
        public void CreateExel()
        {
            if(subjectsMore != null)
            {
                foreach (var item in subjectsMore)
                {
                    CreateExelSub(item);
                }
            }
        }
        public void CreatePDF()
        {
            if (subjectsMore != null)
            {
                foreach (var item in subjectsMore)
                {
                    var list = GetListAboutSub(item);
                    PDF pDF = new PDF();


                    pDF.Create(list, System.IO.Path.Combine(Environment.CurrentDirectory + "\\FilesReestSh\\EDRSh.docx"), 
                        path + $"\\{code},{GetWhithout(name)},{passport}\\i2d{item.id}.docx", 
                        path + $"\\{code},{GetWhithout(name)},{passport}\\id{item.id}.docx");

                    
                }
            }
            

            //PDF.CreatePDFEDR(new FileStream("C:\\app\\ReportName.pdf", FileMode.Create), GetListAboutSub(subjectsMore.FirstOrDefault()));
            //if (subjectsMore != null)
            //{
            //    foreach (var item in subjectsMore)
            //    {
            //        CreateExelSub(item);
            //    }
            //}
        }
        public void CreateExelSub(Subject subject)
        {
            var list = GetListAboutSub(subject);
            if (subjectsMore != null)
            {
                //Subject subject = subjectsMore.First();

                HSSFWorkbook workbook = new HSSFWorkbook();
                HSSFFont myFont = CreateFont(workbook);
                HSSFCellStyle borderedCellStyle = CellStyle(workbook, myFont);

                ISheet Sheet = workbook.CreateSheet("Report");
                IRow HeaderRow = Sheet.CreateRow(0);

                for (int i = 0; i < Reest.namesField.Count; i++)
                {
                    
                    CreateCell(HeaderRow, i, Reest.namesField[i], borderedCellStyle);
                    
                    
                }

                bool isNotOverMaxLen = true;
                int numRow = 1;
                while (isNotOverMaxLen)
                {
                    IRow HeaderRowCur = Sheet.CreateRow(numRow);
                    isNotOverMaxLen = false;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if(list[i]?.Count >= numRow)
                        {
                            isNotOverMaxLen = true;
                            CreateCell(HeaderRowCur, i, list[i]?[numRow - 1] ?? "", borderedCellStyle);
                        }
                    }
                    numRow++;
                }

                if(!Directory.Exists(path + $"\\{code},{GetWhithout(name)},{passport}"))
                    Directory.CreateDirectory(path + $"\\{code},{GetWhithout(name)},{passport}");

                using (var fileData = new FileStream(path + $"\\{code},{GetWhithout(name)},{passport}\\id{subject.id}.xls", FileMode.Create))
                {
                    workbook.Write(fileData);
                }

            }
            
        }
        private HSSFCellStyle CellStyle(HSSFWorkbook workbook, HSSFFont myFont)
        {
            HSSFCellStyle borderedCellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            borderedCellStyle.SetFont(myFont);
            borderedCellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            return borderedCellStyle;
        }
        private HSSFFont CreateFont(HSSFWorkbook workbook)
        {
            HSSFFont myFont = (HSSFFont)workbook.CreateFont();
            myFont.FontHeightInPoints = 11;
            myFont.FontName = "Times New Roman";
            return myFont;
        }
        private void CreateCell(IRow CurrentRow, int CellIndex, string Value, HSSFCellStyle Style)
        {
            ICell Cell = CurrentRow.CreateCell(CellIndex);
            Cell.SetCellValue(Value);
            Cell.CellStyle = Style;
        }
        public List<List<string?>?> GetListAboutSub(Subject subject)
        {
            //var subject = subjectsMore?.First() ?? new Subject();
            List<List<string?>?> strings = new List<List<string?>?>();
            //1
            strings.Add(new List<string?>() { subject.names?.display });
            //2
            strings.Add(new List<string?>() { subject.names?.@short });
            //3
            strings.Add(new List<string?>() { subject.code });
            //4
            strings.Add(new List<string?>() { subject.state_text });
            //5
            strings.Add(new List<string?>() { subject.olf_name });
            //6
            strings.Add(new List<string?>() { subject.executive_power?.name });
            //7
            strings.Add(new List<string?>() { subject.executive_power?.code });
            //8
            strings.Add(new List<string?>() { subject.address?.address });
            //9
            strings.Add(new List<string?>() { $"{subject.primary_activity_kind?.code} {subject.primary_activity_kind?.name}" });
            //10
            strings.Add(new List<string?>());
            if(subject.activity_kinds != null)
            {
                foreach (var item in subject.activity_kinds)
                {
                    var ispr = item?.is_primary;
                    if (ispr != null)
                    {
                        if (!ispr.Value)
                        {
                            strings.Last()?.Add($"{item?.code} {item?.name}");
                        }
                    }
                }
            }
            //11
            strings.Add(new List<string?>() { subject.management });
            //12-15
            strings.Add(new List<string?>());
            strings.Add(new List<string?>());
            strings.Add(new List<string?>());
            strings.Add(new List<string?>());
            if(subject.founders != null)
            {
                foreach (var item in subject.founders)
                {
                    strings[strings.Count - 4]?.Add(item?.name);
                    strings[strings.Count - 3]?.Add(item?.country);
                    strings[strings.Count - 2]?.Add(item?.address?.address);
                    strings[strings.Count - 1]?.Add($"{item?.capital}");
                }
            }
            //16
            strings.Add(new List<string?>() { subject.founding_document });
            //17 
            strings.Add(new List<string?>() { subject.registration?.record_date });
            //18
            strings.Add(new List<string?>() { subject.registration?.record_number });
            //19
            strings.Add(new List<string?>() { subject.object_name });
            //20-23
            strings.Add(new List<string?>());
            strings.Add(new List<string?>());
            strings.Add(new List<string?>());
            strings.Add(new List<string?>());
            if(subject.registrations != null)
            {
                foreach (var item in subject.registrations)
                {
                    strings[strings.Count - 4]?.Add(item?.start_date);
                    strings[strings.Count - 3]?.Add(item?.start_num);
                    strings[strings.Count - 2]?.Add(item?.name);
                    strings[strings.Count - 1]?.Add(item?.code);
                }
            }
            //24
            strings.Add(new List<string?>());
            if(subject.contacts != null)
            {
                if(subject.contacts.tel != null)
                {
                    foreach (var item in subject.contacts.tel)
                    {
                        strings.Last()?.Add(item);
                    }
                }
            }
            //25
            strings.Add(new List<string?>() { subject.contacts?.email });
            //26-37
            for (int i = 0; i < 12; i++)
            {
                strings.Add(new List<string?>());
            }
            if(subject.branches != null){
                foreach (var item in subject.branches)
                {
                    strings[strings.Count - 12]?.Add(item?.name);
                    strings[strings.Count - 11]?.Add(item?.code);
                    strings[strings.Count - 10]?.Add(item?.role_text);
                    strings[strings.Count - 9]?.Add(item?.type_text);
                    strings[strings.Count - 8]?.Add(item?.create_date);

                    Head? head = GetTheNewestHead(item?.heads);

                    strings[strings.Count - 7]?.Add(head?.name + " " + head?.first_middle_name);
                    strings[strings.Count - 6]?.Add(head?.appointment_date);
                    strings[strings.Count - 5]?.Add(head?.restriction);

                    strings[strings.Count - 4]?.Add(item?.address?.address);
                    string str = "";
                    if(item?.contacts != null)
                    {
                        if(item.contacts.tel != null)
                        {
                            foreach (var itemTel in item.contacts.tel)
                            {
                                str += itemTel + "; ";
                            }
                        }
                    }
                    strings[strings.Count - 3]?.Add(str);
                    strings[strings.Count - 2]?.Add(item?.contacts?.email);
                    strings[strings.Count - 1]?.Add(item?.contacts?.web_page);
                }
            }
            //38
            strings.Add(new List<string?>() { subject.termination?.date });
            //39
            strings.Add(new List<string?>() { subject.termination?.cause });
            //40-42
            strings.Add(new List<string?>());
            strings.Add(new List<string?>());
            strings.Add(new List<string?>());
            if(subject.heads != null)
            {
                foreach (var item in subject.heads)
                {
                    if (item?.role == 7 || item?.role == 8 || item?.role == 13 || item?.role == 18)
                    {
                        strings[strings.Count - 3]?.Add(item?.last_name + " " + item?.first_middle_name);
                        strings[strings.Count - 2]?.Add(item?.address?.address);
                        strings[strings.Count - 1]?.Add(item?.role_text);
                    }
                }
            }
            //43
            strings.Add(new List<string?>() { subject.prev_registration_end_term });
            //44-47
            strings.Add(new List<string?>() { subject.bankruptcy?.doc_number });
            strings.Add(new List<string?>() { subject.bankruptcy?.doc_date });
            strings.Add(new List<string?>() { subject.bankruptcy?.date_judge });
            strings.Add(new List<string?>() { subject.bankruptcy?.court_name });
            //48-53
            strings.Add(new List<string?>());
            strings.Add(new List<string?>());
            strings.Add(new List<string?>());
            strings.Add(new List<string?>());
            strings.Add(new List<string?>());
            strings.Add(new List<string?>());
            if(subject.assignees != null)
            {
                foreach (var item in subject.assignees)
                {
                    strings[strings.Count - 6]?.Add(item?.name);
                    strings[strings.Count - 5]?.Add(item?.code);
                    strings[strings.Count - 4]?.Add(item?.address?.address);
                    strings[strings.Count - 3]?.Add(item?.last_name + " " + item?.first_middle_name);
                    strings[strings.Count - 2]?.Add(item?.role_text);
                    strings[strings.Count - 1]?.Add(item?.url);
                }
            }
            //54 - 59
            strings.Add(new List<string?>());
            strings.Add(new List<string?>());
            strings.Add(new List<string?>());
            strings.Add(new List<string?>());
            strings.Add(new List<string?>());
            strings.Add(new List<string?>());
            if(subject.predecessors != null)
            {
                foreach (var item in subject.predecessors)
                {
                    strings[strings.Count - 6]?.Add(item?.name);
                    strings[strings.Count - 5]?.Add(item?.code);
                    strings[strings.Count - 4]?.Add(item?.address?.address);
                    strings[strings.Count - 3]?.Add(item?.last_name + " " + item?.first_middle_name);
                    strings[strings.Count - 2]?.Add(item?.role_text);
                    strings[strings.Count - 1]?.Add(item?.url);
                }
            }
            //60-67
            for (int i = 0; i < 8; i++)
            {
                strings.Add(new List<string?>());
            }
            if(subject.beneficiaries != null)
            {
                foreach (var item in subject.beneficiaries)
                {
                    if(item?.role == 19)
                    {
                        strings[strings.Count - 8]?.Add(item?.name);
                        strings[strings.Count - 7]?.Add(item?.code);
                        strings[strings.Count - 6]?.Add(item?.country);
                        strings[strings.Count - 5]?.Add(item?.address?.address);
                        strings[strings.Count - 4]?.Add(item?.last_name + " " + item?.first_middle_name ?? "");
                        strings[strings.Count - 3]?.Add(item?.beneficiaries_type == 5 ? "Прямий вирішальний вплив" :
                            item?.beneficiaries_type == 6 ? "Непрямий вирішальний вплив" :
                            item?.beneficiaries_type == 7 ? "Прямий та непрямий вирішальний вплив" : "");
                        strings[strings.Count - 2]?.Add(item?.role_text);
                        strings[strings.Count - 1]?.Add(item?.interest + "");
                    }
                }
            }
            //68-69
            strings.Add(new List<string?>());
            strings.Add(new List<string?>());
            if (subject.heads != null)
            {
                foreach (var head in subject.heads)
                {
                    if(head != null)
                    {
                        if(head.role == 2 ||
                            head.role == 3 ||
                            head.role == 6 ||
                            head.role == 7 ||
                            head.role == 8 ||
                            head.role == 11 ||
                            head.role == 13 ||
                            head.role == 14 ||
                            head.role == 15 ||
                            head.role == 18 ||
                            head.role == 12 ||
                            head.role == 17)
                        {
                            strings[strings.Count - 2]?.Add(head.last_name + " " + head.first_middle_name);
                            strings[strings.Count - 1]?.Add(head.role_text);
                        }
                    }
                }
            }
            //70
            strings.Add(new List<string?>() { subject.authorised_capital?.value + "" });
            //71





            return strings;
        }
        public static Head? GetTheNewestHead(List<Head?>? heads)
        {
            Head? head = null;
            int index = 0;
            if (heads != null)
            {
                if (heads.Count > 0)
                {
                    for (int i = 0; i < heads.Count; i++)
                    {
                        if (heads[i] != null && heads[index] != null)
                        {
                            if (Convert.ToDateTime(heads[i]?.appointment_date) > Convert.ToDateTime(heads[index]?.appointment_date))
                            {
                                head = heads[i];
                                index = i;
                            }
                        }
                    }
                }
            }

            return head;
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
