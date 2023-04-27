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
using SixLabors.ImageSharp.Drawing;
using DesARMA.Models;
using DesARMA.ModelCentextEDR;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using NPOI.SS.Formula.Functions;
using System.Diagnostics;
using System.Security.Cryptography;

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

        public List<EDRClass>? subjects;
        private List<Subject>? subjectsMore;
        ProgresWindow progresWindow;

        private string path;
        private bool isFO;
        Figurant figurant;

        private ModelContext modelContext;
        private ModelContextEDR modelContextEDR;
        int numberR;
        public SearchEDR(string? code, string? name, string? passport, int limit, SearchType? searchType,
                        string path, ProgresWindow progresWindow, Figurant figurant, ModelContext modelContext, bool isFO = false, int numberR = 15)
        {
            try
            {
                client.Timeout = TimeSpan.FromMinutes(10);
                this.modelContext = modelContext;
                this.modelContextEDR = new ModelContextEDR();
                this.numberR = numberR;
                this.progresWindow = progresWindow;
                if (searchType == null)
                    this.searchType = SearchType.Base;
                else
                    this.searchType = searchType;

                this.figurant = figurant;
                this.code = code;
                this.name = name;
                this.passport = passport;
                this.limit = limit;

                if (!Directory.Exists(path))
                    throw new Exception("Програма не може знайти шлях: " + path);

                this.path = path;
                this.isFO = isFO;

                SetParamsInReqstr();
                SetReqstrId();

                GetInfoSubjects();
                GetInfoSubjectsMore();


                //if (subjects != null && subjects?.Count == 0)
                //{
                //    progresWindow.NotDataFigur(figurant);
                //}
                //else
                //{
                //    progresWindow.SetDoneFigNow(figurant);
                //}

                //ToCheckFigInTree(numberR - 1);

                
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void SaveToDB()
        {
            if(subjectsMore != null)
            foreach (var item in subjectsMore)
            {
                modelContextEDR.Subjects.Add(item);
                modelContextEDR.SaveChanges();
                }
        }
        public void ToCheckFigInTree(int indR)
        {
            var listC = AllDirectories.GetBoolsFromString(figurant.Control);
            var listS = AllDirectories.GetBoolsFromString(figurant.Shema);
            if (subjects != null && subjects.Count > 0)
            {
                //figurant.Control = 
                listC[indR] = true;
                listS[indR] = false;
            }
            else
            {
                listC[indR] = false;
                listS[indR] = true;
            }
            figurant.Control = AllDirectories.GetStringFromBools(listC);
            figurant.Shema = AllDirectories.GetStringFromBools(listS);
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
                .SetName(code == null || code == "" ? name : null)
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

            try
            {
                response = GetRespS();
                subjects = JsonConvert.DeserializeObject<List<EDRClass>>(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            //string response = "";

            //var task = Task.Run(async () => { // Викликаємо метод GetResp() в асинхронному таску
            //    try
            //    {
            //        response = await GetResp();
            //        subjects = JsonConvert.DeserializeObject<List<EDRClass>>(response);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }

            //});
            //await task;
            //task.Wait();
        }
        private string GetStrToken()
        {
            string shif = ConfigurationManager.AppSettings["token"].ToString();
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
        public async void GetInfoSubjectsMore()
        {
            //first app
            //{
            //    subjectsMore = new List<Subject>();
            //    if (subjects != null)
            //        for (int i = 0; i < subjects.Count; i++)
            //        {
            //            var id = subjects[i].id;
            //            if (id != null)
            //            {
            //                string response = "";

            //                var task = Task.Run(async () => { // Викликаємо метод GetResp() в асинхронному таску
            //                    try
            //                    {
            //                        response = await GetRespId((uint)id);
            //                        subjectsMore.Add(JsonConvert.DeserializeObject<Subject>(response));
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        Debug.WriteLine($"\t THROW {i} - subMore {DateTime.Now}");
            //                        throw ex;
            //                    }
            //                });
            //                Debug.WriteLine($"\t {i} - start subMore {DateTime.Now}");
            //                task.Wait();
            //                Debug.WriteLine($"\t {i} - finish subMore {DateTime.Now}");
            //            }
            //        }
            //}

            //second
            {
                subjectsMore = new List<Subject>();
                if (subjects != null)
                {
                    var tasks = new List<Task>(); //Task[subjects.Count];

                    for (int i = 0; i < subjects.Count; i++)
                    {
                        var id = subjects[i].id;
                        if (id != null)
                        {
                            string response = "";

                            tasks.Add(Task.Run(async () =>
                            {
                                try
                                {
                                    response = await GetRespId((uint)id);
                                    subjectsMore.Add(JsonConvert.DeserializeObject<Subject>(response));
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine($"\t THROW {i} - subMore {DateTime.Now}");
                                    throw ex;
                                }
                            }
                            ));
                        }

                    }
                    Debug.WriteLine($"\t - start subMore ALL {DateTime.Now}");
                    await Task.WhenAll(tasks).ContinueWith( _ =>
                    {
                        Debug.WriteLine($"\t - finish subMore ALL {DateTime.Now}");
                        CreatePDF();
                        if (subjects != null && subjects?.Count == 0)
                        {
                            progresWindow.NotDataFigur(figurant);
                                
                        }
                        else
                        {
                            progresWindow.SetDoneFigNow(figurant);
                        }
                        ToCheckFigInTree(numberR - 1);
                        SaveToDB();
                        
                    }
                        );
                    //foreach (var item in tasks)
                    //{
                    //    await item;
                    //}
                   
                    //Task.WaitAll(tasks);
                }
            }
            //not synh
            { 
                
            }
        }
        public string GetRespS()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(Reqstr!),
                Headers =
                {
                    { "Authorization", GetStrToken()},
                }
            };
            var response = client.SendAsync(request).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = response.Content.ReadAsStringAsync().Result;
               // var content = response.Content.ReadAsStream().Read() ToString();
                return content;
            }
            else
            {
                throw new Exception($"Request failed: {response.StatusCode}");

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
                    { "Authorization", GetStrToken() },
                }
            };
            var response = await client.SendAsync(request);
            
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
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
                    { "Authorization", GetStrToken() },
                }
            };
            var response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
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
            if (subjectsMore != null && subjectsMore.Count > 0)
            {
                List<List<List<string?>?>> listAll = new();
                List<bool> listBoolIsFiz = new();
                foreach (var item in subjectsMore)
                {
                    listBoolIsFiz.Add(item.code == null);

                    if (item.code != null)
                        listAll.Add(GetListAboutSubPDF(item));
                    else
                        listAll.Add(GetListAboutSubPDFFiz(item));
                    //GetListAboutSubPDFFiz
                }

                PDF pDF = new PDF();

                string nameFolder = $"\\{GetWhithout(name)}_{code}";
                string nameFile;
                if (code == null)
                    nameFile = $"\\Витяг_{StringExtension.GetStringForSearchTypeUKR(searchType, isFO)}_{GetWhithout(name)}.docx";
                else
                    nameFile = $"\\Витяг_{StringExtension.GetStringForSearchTypeUKR(searchType, isFO)}_{code}.docx";

                string nameFileTemp;
                if (code == null)
                    nameFileTemp = $"\\Витяг_{StringExtension.GetStringForSearchTypeUKR(searchType, isFO)}_{GetWhithout(name)}Temp.docx";
                else
                    nameFileTemp = $"\\Витяг_{StringExtension.GetStringForSearchTypeUKR(searchType, isFO)}_{code}Temp.docx";

                if (!Directory.Exists(path + nameFolder))
                    Directory.CreateDirectory(path + nameFolder);

                pDF.CreateNamesFieldPDF(listAll, new object[] { (figurant.Ipn == null && figurant.Fio == null), code ?? "не заданий", searchType ?? SearchType.Base },
                    listBoolIsFiz,
                    System.IO.Path.Combine(Environment.CurrentDirectory + "\\FilesReestSh\\EDRSh.docx"),
                    path + nameFolder + nameFileTemp,
                    path + nameFolder + nameFile
                    );


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


                string nameFolder = $"\\{GetWhithout(name)}_{code}";
                string nameFile;
                if (code == null)
                    nameFile = $"\\Витяг_{StringExtension.GetStringForSearchTypeUKR(searchType, isFO)}_{GetWhithout(name)}.xls";
                else
                    nameFile = $"\\Витяг_{StringExtension.GetStringForSearchTypeUKR(searchType, isFO)}_{code}.xls";

                if (!Directory.Exists(path + nameFolder))
                    Directory.CreateDirectory(path + nameFolder);

                using (var fileData = new FileStream(path + nameFolder + nameFile, FileMode.Create))
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
            strings.Add(new List<string?>() { subject.founding_document_name });
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
            if (subject.beneficiaries is JArray arr)
            {
                var listBenef = JsonConvert.DeserializeObject<List<Beneficiaries>>(arr.ToString());
                if(listBenef != null)
                {
                    foreach (var item in listBenef)
                    {
                        if (item?.role == 19)
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
            }
            //if (subject.beneficiaries is JObject obj)
            //{
            //    var reason = JsonConvert.DeserializeObject<Reason>(obj.ToString());
                
            //}

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
        public List<List<string?>?> GetListAboutSubPDF(Subject subject)
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
            //6 + 7  -- 6
            if(subject.executive_power?.name == null && subject.executive_power?.code == null)
                strings.Add(new List<string?>());
            else
                strings.Add(new List<string?>() { $"{subject.executive_power?.name}, {subject.executive_power?.code}" });
            //8  -- 7
            strings.Add(new List<string?>() { subject.address?.address });
            //24 -- 7.1
            strings.Add(new List<string?>());
            if (subject.contacts != null)
            {
                if (subject.contacts.tel != null)
                {
                    foreach (var item in subject.contacts.tel)
                    {
                        strings.Last()?.Add(item);
                    }
                }
            }
            //25 -- 7.2
            strings.Add(new List<string?>() { subject.contacts?.email });
            //9  -- 8
            strings.Add(new List<string?>() { $"{subject.primary_activity_kind?.code} {subject.primary_activity_kind?.name}" });
            //10 -- 9
            strings.Add(new List<string?>());
            if (subject.activity_kinds != null)
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
            //11 -- 10
            strings.Add(new List<string?>() { subject.management });
            //12-15 -- 11
            strings.Add(new List<string?>());
            if (subject.founders != null)
            {
                foreach (var item in subject.founders)
                {
                    strings.Last()?.Add($"{item?.name}, {item?.country}, {item?.address?.address}, {item?.capital}");
                }
            }
            //60-67 -- 12
            strings.Add(new List<string?>());
            if (subject.beneficiaries is JArray arr)
            {
                var listBenef = JsonConvert.DeserializeObject<List<Beneficiaries>>(arr.ToString());
                if (listBenef != null)
                {
                    foreach (var item in listBenef)
                    {
                        if (item?.role == 19)
                        {
                            var str = item?.beneficiaries_type == 5 ? "Прямий вирішальний вплив" :
                                item?.beneficiaries_type == 6 ? "Непрямий вирішальний вплив" :
                                item?.beneficiaries_type == 7 ? "Прямий та непрямий вирішальний вплив" : "";

                            strings.Last()?.Add(
                                $"{item?.name}, " +
                                $"{item?.code}, " +
                                $"{item?.country}, " +
                                $"{item?.address?.address} " +
                                $"{item?.last_name + " " + item?.first_middle_name ?? ""}, " +
                                $"{str} " +
                                $"{item?.role_text}, " +
                                $"{item?.interest}, ");
                        }
                    }
                }
            }
            if (subject.beneficiaries is JObject obj)
            {
                var reason = JsonConvert.DeserializeObject<Reason>(obj.ToString());
                strings.Last()?.Add(reason.reason);
            }
            //68-69 -- 13
            strings.Add(new List<string?>());
            if (subject.heads != null)
            {
                foreach (var head in subject.heads)
                {
                    if (head != null)
                    {
                        if (head.role == 2 ||
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
                            strings.Last()?.Add($"{head.last_name + " " + head.first_middle_name}, {head.role_text}");
                        }
                    }
                }
            }
            //70 -- 14
            if (subject.authorised_capital?.value == null)
                strings.Add(new List<string?>());
            else
                strings.Add(new List<string?>() { subject.authorised_capital?.value + "" });
            //16 -- 15
            strings.Add(new List<string?>() { subject.founding_document_name });
            //17 -- 16
            strings.Add(new List<string?>() { subject.registration?.record_date });
            //26-37 -- 17
            strings.Add(new List<string?>());
            if (subject.branches != null)
            {
                foreach (var item in subject.branches)
                {
                    Head? head = GetTheNewestHead(item?.heads);

                    string str = "";
                    if (item?.contacts != null)
                    {
                        if (item.contacts.tel != null)
                        {
                            foreach (var itemTel in item.contacts.tel)
                            {
                                str += itemTel + "; ";
                            }
                        }
                    }

                    strings.Last()?.Add(
                        $"{item?.name}, " +
                        $"{item?.code}, " +
                        $"{item?.role_text}, " +
                        $"{item?.type_text}, " +
                        $"{item?.create_date}, " +
                        $"{head?.name + " " + head?.first_middle_name}, " +
                        $"{head?.appointment_date}, " +
                        $"{head?.restriction}, " +
                        $"{item?.address?.address}, " +
                        $"{str}, " +
                        $"{item?.contacts?.email}, " +
                        $"{item?.contacts?.web_page}"
                        );
                }
            }
            //38 -- 18
            strings.Add(new List<string?>() { subject.termination?.date });
            //39 -- 19
            strings.Add(new List<string?>() { subject.termination?.cause });
            //40-42 -- 20
            strings.Add(new List<string?>());
            if (subject.heads != null)
            {
                foreach (var item in subject.heads)
                {
                    if (item?.role == 7 || item?.role == 8 || item?.role == 13 || item?.role == 18)
                    {
                        strings.Last()?.Add($"{item?.last_name + " " + item?.first_middle_name}, {item?.address?.address} {item?.role_text}");
                    }
                }
            }
            //43 -- 21
            strings.Add(new List<string?>() { subject.prev_registration_end_term });
            //48-53 -- 22
            strings.Add(new List<string?>());
            if (subject.assignees != null)
            {
                foreach (var item in subject.assignees)
                {
                    strings.Last()?.Add($"{item?.name}, {item?.code}, {item?.address?.address}, {item?.last_name + " " + item?.first_middle_name}, {item?.role_text}, {item?.url}");
                }
            }
            //54 - 59 -- 23
            strings.Add(new List<string?>());
            if (subject.predecessors != null)
            {
                foreach (var item in subject.predecessors)
                {
                    strings.Last()?.Add($"{item?.name}, {item?.code}, {item?.address?.address}, {item?.last_name + " " + item?.first_middle_name}, {item?.role_text}, {item?.url}");
                }
            }
            //44-47 -- 24-27
            strings.Add(new List<string?>() { subject.bankruptcy?.doc_number });
            strings.Add(new List<string?>() { subject.bankruptcy?.doc_date });
            strings.Add(new List<string?>() { subject.bankruptcy?.date_judge });
            strings.Add(new List<string?>() { subject.bankruptcy?.court_name });
            //19 -- 28
            strings.Add(new List<string?>() { subject.object_name });
            //18 -- 29
            strings.Add(new List<string?>() { subject.registration?.record_number });
            //20-23 -- 30
            strings.Add(new List<string?>());
            if (subject.registrations != null)
            {
                foreach (var item in subject.registrations)
                {
                    strings.Last()?.Add($"{item?.start_date}, {item?.start_num}, {item?.name}, {item?.code}");
                }
            }
           
            return strings;
        }
        public List<List<string?>?> GetListAboutSubPDFFiz(Subject subject)
        {
            //var subject = subjectsMore?.First() ?? new Subject();
            List<List<string?>?> strings = new List<List<string?>?>();
            //1
            strings.Add(new List<string?>() { subject.names?.display });
            //4
            strings.Add(new List<string?>() { subject.state_text });
            // nonName
            strings.Add(new List<string?>() { subject.country });
            //8  -- 7
            strings.Add(new List<string?>() { subject.address?.address });
            //24 -- 8
            strings.Add(new List<string?>());
            if (subject.contacts != null)
            {
                if (subject.contacts.tel != null)
                {
                    foreach (var item in subject.contacts.tel)
                    {
                        strings.Last()?.Add(item);
                    }
                }
            }
            //25 -- 9
            strings.Add(new List<string?>() { subject.contacts?.email });
            //9  -- 10
            strings.Add(new List<string?>() { $"{subject.primary_activity_kind?.code} {subject.primary_activity_kind?.name}" });
            //10 -- 11
            strings.Add(new List<string?>());
            if (subject.activity_kinds != null)
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
            //17 -- 18
            strings.Add(new List<string?>() { subject.registration?.record_date });
            //19 -- 30
            strings.Add(new List<string?>() { subject.object_name });
            //18 -- 31
            strings.Add(new List<string?>() { subject.registration?.record_number });
            //20-23 -- 32
            strings.Add(new List<string?>());
            if (subject.registrations != null)
            {
                foreach (var item in subject.registrations)
                {
                    strings.Last()?.Add($"{item?.start_date}, {item?.start_num}, {item?.name}, {item?.code}");
                }
            }

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
        public static string GetStringForSearchTypeUKR(SearchType? stIns, bool isFO)
        {
            if (stIns == null)
            {
                if(isFO)
                    return "ФО";
                return "ЮО";
            }
            else if (stIns == SearchType.Base)
            {
                if (isFO)
                    return "ФО";
                return "ЮО";
            }
            else if (stIns == SearchType.Branch)
            {
                return "ВП";
            }
            else if (stIns == SearchType.Beneficiar)
            {
                return "бенефіціари";
            }
            else if (stIns == SearchType.Founder)
            {
                return "засновники";
            }
            else if (stIns == SearchType.Chief)
            {
                return "керівники";
            }
            else if (stIns == SearchType.Assignee)
            {
                return "представники";
            }
            return "base";
        }
    }
    
}
