using BitMiracle.LibTiff.Classic;
using DesARMA.ModelRPS_SK_SR;
using DesARMA.Models;
using DesARMA.Registers;
using DesARMA.SearchWin;
using Microsoft.EntityFrameworkCore;
using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DesARMA.Automation
{
    public class SearchSK
    {
        public List<PotentialRecordsSK> OrderOwner = new();
        ModelContextRPS_SK_SR modelContextRPS_SK_SR = new();
        string name;
        string code;
        bool isFiz;
        List<PotentialRecordsSK>? listOwnerGroup = null!;
        Figurant figurant = null!;
        string path;
        public string? FullName
        {
            get
            {
                return GetDefInString(figurant);
            }
        }
        public string NameFirst { get; set; } = "";
        public SearchSK(string name, string code, bool isFiz, ProgresWindow progresWindow, Figurant figurant, int numberR, string path)
        {
            try
            {
                this.name = name;
                this.code = code;
                this.isFiz = isFiz;
                this.figurant = figurant;
                this.path = path;
                GenerGroup();

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    if (listOwnerGroup !=null && listOwnerGroup.Count > 0)
                    {
                        WindowSearchSK windowSearchSK = new(listOwnerGroup, this);
                        windowSearchSK.ShowDialog();
                        if (OrderOwner != null && OrderOwner.Count > 0)
                        {
                            CreatePDFOwner();
                            progresWindow.SetDoneFigNow(figurant);
                        }
                        else
                        {
                            progresWindow.NotDataFigur(figurant);
                        }

                    }
                    else
                    {
                        progresWindow.NotDataFigur(figurant);
                    }

                    ToCheckFigInTree(numberR - 1);
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ToCheckFigInTree(int indR)
        {

            var modelContext = new ModelContext();
            var listC = AllDirectories.GetBoolsFromString(figurant.Control);
            var listS = AllDirectories.GetBoolsFromString(figurant.Shema);
            if (listC != null && listC.Count > indR && listS != null && listS.Count > indR)
            {
                if (OrderOwner != null && OrderOwner.Count > 0)
                {
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
                modelContext.SaveChanges();
            }
        }
        public void CreatePDFOwner()
        {
            try
            {
                if (OrderOwner.Count != 0)
                {
                    string nameFolder = $"\\{GetWhithout(name)}_{code}";
                    string nameFile;

                    if (code == null || code == "")
                        nameFile = $"\\Витяг_СК_{GetWhithout(name)}.docx";
                    else
                        nameFile = $"\\Витяг_СК_{code}.docx";

                    string nameFileTemp;
                    if (code == null || code == "")
                        nameFileTemp = $"\\Витяг_СК_{GetWhithout(name)}Temp.docx";
                    else
                        nameFileTemp = $"\\Витяг_СК_{code}Temp.docx";

                    if (!Directory.Exists(path + nameFolder))
                        Directory.CreateDirectory(path + nameFolder);

                    PDF pDF = new PDF();
                    pDF.CreateNamesFieldSK_PDF(GetListAboutOwnerSubPDF(), code == null || code == "" ? name : code,
                        System.IO.Path.Combine(Environment.CurrentDirectory + "\\FilesReestSh\\SKSh.docx"),
                        path + nameFolder + nameFileTemp,
                        path + nameFolder + nameFile
                        );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<List<string?>> GetListAboutOwnerSubPDF()
        {
            List<List<string?>> strings = new();

            using (var context = new ModelContextRPS_SK_SR())
            {
                var list = new List<InfShipBookN>();
                foreach (var item in OrderOwner)
                {

                    //var query = (from row in context.InfShipBooksN
                    //             where item.Name == row.Pib
                    //             select row).ToList();

                    List<InfShipBookN> query = new();
                   // { item.Pib, item.Ipn, item.Pass, item.DtBirth, item.Reg, item.Distr, item.Settl, item.Street, item.BuildNumb, item.CaseNumb, item.ApNumb }
                    if (isFiz)
                    {
                        query = (from row in context.InfShipBooksN
                                 where item.Ipn == row.Ipn &&
                                 item.Pib == row.Pib &&
                                 item.Passp == row.Pass &&
                                 item.Dt == row.DtBirth &&
                                 item.Reg == row.Reg &&
                                 item.Distr == row.Distr &&
                                 item.Settl == row.Settl &&
                                 item.Street == row.Street &&
                                 item.BuildNumb == row.BuildNumb &&
                                 item.CaseNumb == row.CaseNumb &&
                                 item.ApNumb == row.ApNumb
                                 select row).ToList();
                    }
                    else
                    {
                        query = (from row in context.InfShipBooksN
                                 where item.Ipn == row.Ipn &&
                                 item.Pib == row.Pib &&
                                 item.Passp == row.Pass &&
                                 item.Dt == row.DtBirth &&
                                 item.Reg == row.Reg &&
                                 item.Distr == row.Distr &&
                                 item.Settl == row.Settl &&
                                 item.Street == row.Street &&
                                 item.BuildNumb == row.BuildNumb &&
                                 item.CaseNumb == row.CaseNumb &&
                                 item.ApNumb == row.ApNumb
                                 select row).ToList();
                    }

                    if (query != null)
                        list.AddRange(query);

                }

                foreach (var item in list)
                {
                    strings.Add(new List<string?>());

                    strings.Last().Add(item.ShipTick);
                    strings.Last().Add(item.BookNumb);
                    strings.Last().Add(item.BookSnumb);
                    strings.Last().Add(item.DtReg);
                    strings.Last().Add(item.ShipNumb);
                    strings.Last().Add(item.ShipNumbOld);
                    strings.Last().Add(item.Brand);
                    strings.Last().Add(item.TypeOld);
                    strings.Last().Add(item.Type);
                    strings.Last().Add(item.Model);
                    strings.Last().Add(item.Funct);
                    strings.Last().Add(item.DtConst);
                    strings.Last().Add(item.Cntr);
                    strings.Last().Add(item.Place);
                    strings.Last().Add(item.FNumb);
                    strings.Last().Add(item.RegA);
                    strings.Last().Add(item.RegANew);
                    strings.Last().Add(item.DtEnd);
                    strings.Last().Add(item.Exclus);
                    strings.Last().Add(item.DtEndT);
                    strings.Last().Add(item.DtRen);
                    strings.Last().Add(item.TermChart);
                    strings.Last().Add(item.OwnerS);
                    strings.Last().Add(item.DtBirth);
                    strings.Last().Add(item.Pass);
                    strings.Last().Add(item.Pib);
                    strings.Last().Add(item.Ipn);
                    strings.Last().Add(item.NameO);
                    strings.Last().Add(item.Edrpou);
                    strings.Last().Add(item.Reg);
                    strings.Last().Add(item.Distr);
                    strings.Last().Add(item.Settl);
                    strings.Last().Add(item.Street);
                    strings.Last().Add(item.BuildNumb);
                    strings.Last().Add(item.CaseNumb + "");
                    strings.Last().Add(item.ApNumb + "");
                    strings.Last().Add(item.Pib2 + "");
                    strings.Last().Add(item.Ipn2 + "");
                    strings.Last().Add(item.NameO2 + "");
                    strings.Last().Add(item.Edrpou2 + "");
                    strings.Last().Add(item.Reg2 + "");
                    strings.Last().Add(item.Distr2 + "");
                    strings.Last().Add(item.Settl2 + "");
                    strings.Last().Add(item.Street2 + "");
                    strings.Last().Add(item.BuildNumb2 + "");
                    strings.Last().Add(item.CaseNumb2 + "");
                    strings.Last().Add(item.ApNumb2 + "");
                    strings.Last().Add(item.IssA + "");
                    strings.Last().Add(item.DtIss + "");
                    strings.Last().Add(item.Crew + "");
                    strings.Last().Add(item.Length + "");
                    strings.Last().Add(item.Width + "");
                    strings.Last().Add(item.Height + "");
                    strings.Last().Add(item.Mat + "");
                    strings.Last().Add(item.Speed + "");
                    strings.Last().Add(item.Cap + "");
                    strings.Last().Add(item.Engine + "");
                    strings.Last().Add(item.EngNumb + "");
                    strings.Last().Add(item.EngPower + "");
                }
            }
            return strings;
        }
        private string GetWhithout(string? str)
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
        private string GetNameStrWithoutFormGosp(string? str)
        {
            if (str == null) return "";

            var mylist = str.Trim().ToLower();

            foreach (string item in Reest.listOPFG)
            {
                mylist = mylist.Replace(item.ToLower(), "");
            }

            return mylist;   
        }
        private string GetNameStrWithoutFormGospAbreviat(string? str)
        {
            if (str == null) return "";

            var mylist = str.Trim();

            foreach (string item in Reest.listOPFG_Abbreviation)
            {
                mylist = mylist.Replace(item + " ", "");
                mylist = mylist.Replace(item + "\"", "");
            }

            return mylist;
        }
       
        public void GenerGroup()
        {
            try
            {
                string nameFirst = GetNameStrWithoutFormGospAbreviat(name);
                string nameFirst2 = GetNameStrWithoutFormGosp(nameFirst);

                if (isFiz)
                {
                    var del = nameFirst2
                        .Replace("0", "")
                        .Replace("1", "")
                        .Replace("2", "")
                        .Replace("3", "")
                        .Replace("4", "")
                        .Replace("5", "")
                        .Replace("6", "")
                        .Replace("7", "")
                        .Replace("8", "")
                        .Replace("9", "")
                        .Trim().Split(new char[] { ' ' });
                    if(del.Length == 1)
                    {
                        NameFirst = del.First();
                    }
                    else if(del.Length >= 2)
                    {
                        NameFirst = $"{del[0]} {del[1]}";
                    }
                }
                else
                {
                    var str = nameFirst2.Trim();
                    var ind = str.IndexOf(' ');
                    if(ind != -1)
                    {
                        NameFirst = str.Substring(0, ind);
                    }
                    else
                    {
                        NameFirst = str;
                    }
                }


                if (code != "" && NameFirst != "")
                {
                    listOwnerGroup = ReqOwnerToDbInfRegAviaAll(NameFirst, code);
                    NameFirst += " і " + code;
                }
                else if (code != "")
                {
                    listOwnerGroup = ReqOwnerToDbInfRegAviaCode(code);
                    NameFirst = code;
                }
                else if (NameFirst != "")
                {
                    listOwnerGroup = ReqOwnerToDbInfRegAvia(NameFirst);
                }

                if (listOwnerGroup != null)
                {
                    foreach (var item in listOwnerGroup)
                    {
                        item.Addres = GetAddresNorm(new List<List<string?>>()
                            {
                                new List<string?>() { "обл:", item.Reg },
                                new List<string?>() { "р-н:", item.Distr },
                                new List<string?>() { "н.п:", item.Settl },
                                new List<string?>() { "вул:", item.Street },
                                new List<string?>() { "№ буд:", item.BuildNumb },
                                new List<string?>() { "№ корп:", item.CaseNumb },
                                new List<string?>() { "№ кв:", item.ApNumb },
                            });
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<string> FindMatchingWords(string? originalString, string words)
        {
            originalString = originalString?.ToLower() ?? "";
            words = words.ToLower();


            char[] chs = { ',', '.', '-', '_', '/', '\\', '|', ';', ':', '(', ')', '[', ']', '{', '}', '<', '>', '?', '!', '@', '#', '$', '%', '^', '&', '*', '+', '=', '`', '~' };
            foreach (var item in chs)
            {
                originalString = originalString.Replace("" + item, "");
            }
            foreach (var item in chs)
            {
                words = words.Replace("" + item, "");
            }


            // Розбиваємо рядок зі словами на окремі слова та перетворюємо їх до нижнього регістру
            string[] searchWords = words.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Перевіряємо, які слова зустрічаються в оригінальному рядку
            List<string> matchingWords = new List<string>();
            foreach (string word in searchWords)
            {
                if (originalString.Contains(word))
                {
                    matchingWords.Add(word);
                }
            }

            return matchingWords;
        }
        List<PotentialRecordsSK>? ReqOwnerToDbInfRegAviaAll(string searchValueName, string searchValueCode)
        {
            try
            {
                //var listFirtName = modelContextRPS_SK_SR.InfShipBooksN
                //            .Where(item => EF.Functions.Like(item.Ipn + "", $"%{searchValueName}%") || EF.Functions.Like(item.Pib.ToString(), $"%{searchValueCode}%"))
                //            .GroupBy(item => item.Pib) // group by Owners
                //            .Select(group => new PotentialRecords
                //            {
                //                Name = group.Key,
                //                Count = group.Count()
                //            })
                //            .ToList();


                List<PotentialRecordsSK> result = new();

                if (isFiz)
                {
                    result = modelContextRPS_SK_SR.InfShipBooksN
                                .Where(item => item.Ipn != null && EF.Functions.Like(item.Ipn, $"%{searchValueCode}%") || item.Pib != null && EF.Functions.Like(item.Pib.ToLower(), $"%{searchValueName.ToLower()}%"))
                                .GroupBy(item => new { item.Pib, item.Ipn, item.Pass, item.DtBirth, item.Reg, item.Distr, item.Settl, item.Street, item.BuildNumb, item.CaseNumb, item.ApNumb })
                                .Select(group => new PotentialRecordsSK
                                {
                                    Pib = group.Key.Pib,
                                    Ipn = group.Key.Ipn,
                                    Passp = group.Key.Pass,
                                    Dt = group.Key.DtBirth,
                                    Addres = $"Обл. {group.Key.Reg}, р-н.{group.Key.Distr}, н.п. {group.Key.Settl}, вул. {group.Key.Street}, № буд. {group.Key.BuildNumb}, № корп. {group.Key.CaseNumb}, № кв.(оф.) {group.Key.ApNumb}",
                                    //Addres = "".GetAddNotEmp(group.Key.Reg, "обл.")
                                    //            .GetAddNotEmp(group.Key.Distr, "р-н.")
                                    //            .GetAddNotEmp(group.Key.Settl, "н.п.")
                                    //            .GetAddNotEmp(group.Key.Settl, "н.п."),
                                    Reg = group.Key.Reg,
                                    Distr = group.Key.Distr,
                                    Settl = group.Key.Settl,
                                    Street = group.Key.Street,
                                    BuildNumb = group.Key.BuildNumb,
                                    CaseNumb = group.Key.CaseNumb,
                                    ApNumb = group.Key.ApNumb,
                                    Count = group.Count()
                                })
                                .ToList();
                }
                else
                {
                    result = modelContextRPS_SK_SR.InfShipBooksN
                               .Where(item => item.Edrpou != null && EF.Functions.Like(item.Edrpou, $"%{searchValueCode}%") || item.NameO != null && EF.Functions.Like(item.NameO.ToLower(), $"%{searchValueName.ToLower()}%"))
                               .GroupBy(item => new { item.Edrpou, item.NameO, item.Pass, item.DtBirth, item.Reg, item.Distr, item.Settl, item.Street, item.BuildNumb, item.CaseNumb, item.ApNumb })
                               .Select(group => new PotentialRecordsSK
                               {
                                   Pib = group.Key.NameO,
                                   Ipn = group.Key.Edrpou,
                                   Passp = group.Key.Pass,
                                   Dt = group.Key.DtBirth,
                                   Addres = $"Обл. {group.Key.Reg}, р-н.{group.Key.Distr}, н.п. {group.Key.Settl}, вул. {group.Key.Street}, № буд. {group.Key.BuildNumb}, № корп. {group.Key.CaseNumb}, № кв.(оф.) {group.Key.ApNumb}",
                                   Reg = group.Key.Reg,
                                   Distr = group.Key.Distr,
                                   Settl = group.Key.Settl,
                                   Street = group.Key.Street,
                                   BuildNumb = group.Key.BuildNumb,
                                   CaseNumb = group.Key.CaseNumb,
                                   ApNumb = group.Key.ApNumb,
                                   Count = group.Count()
                               })
                               .ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }

        }
        List<PotentialRecordsSK>? ReqOwnerToDbInfRegAviaCode(string searchValue)
        {
            try
            {
                //var listFirtName = modelContextRPS_SK_SR.InfShipBooksN
                //            .Where(item => item.Pib != null && EF.Functions.Like(item.Ipn + "", $"%{searchValue}%"))
                //            .GroupBy(item => item.Pib) // group by Owners
                //            .Select(group => new PotentialRecords
                //            {
                //                Name = group.Key,
                //                Count = group.Count()
                //            })
                //            .ToList();

                List<PotentialRecordsSK> result = new();
                if (isFiz)
                {
                    result = modelContextRPS_SK_SR.InfShipBooksN
                                .Where(item => item.Ipn != null && EF.Functions.Like(item.Ipn, $"%{searchValue}%"))
                                .GroupBy(item => new { item.Pib, item.Ipn, item.Pass, item.DtBirth, item.Reg, item.Distr, item.Settl, item.Street, item.BuildNumb, item.CaseNumb, item.ApNumb })
                                .Select(group => new PotentialRecordsSK
                                {
                                    Pib = group.Key.Pib,
                                    Ipn = group.Key.Ipn,
                                    Passp = group.Key.Pass,
                                    Dt = group.Key.DtBirth,
                                    Addres = $"Обл. {group.Key.Reg}, р-н.{group.Key.Distr}, н.п. {group.Key.Settl}, вул. {group.Key.Street}, № буд. {group.Key.BuildNumb}, № корп. {group.Key.CaseNumb}, № кв.(оф.) {group.Key.ApNumb}",
                                    Reg = group.Key.Reg,
                                    Distr = group.Key.Distr,
                                    Settl = group.Key.Settl,
                                    Street = group.Key.Street,
                                    BuildNumb = group.Key.BuildNumb,
                                    CaseNumb = group.Key.CaseNumb,
                                    ApNumb = group.Key.ApNumb,
                                    Count = group.Count()
                                })
                                .ToList();
                }
                else
                {
                    result = modelContextRPS_SK_SR.InfShipBooksN
                               .Where(item => item.Edrpou != null && EF.Functions.Like(item.Edrpou, $"%{searchValue}%"))
                               .GroupBy(item => new { item.NameO, item.Edrpou, item.Pass, item.DtBirth, item.Reg, item.Distr, item.Settl, item.Street, item.BuildNumb, item.CaseNumb, item.ApNumb })
                               .Select(group => new PotentialRecordsSK
                               {
                                   Pib = group.Key.NameO,
                                   Ipn = group.Key.Edrpou,
                                   Passp = group.Key.Pass,
                                   Dt = group.Key.DtBirth,
                                   Addres = $"Обл. {group.Key.Reg}, р-н.{group.Key.Distr}, н.п. {group.Key.Settl}, вул. {group.Key.Street}, № буд. {group.Key.BuildNumb}, № корп. {group.Key.CaseNumb}, № кв.(оф.) {group.Key.ApNumb}",
                                   Reg = group.Key.Reg,
                                   Distr = group.Key.Distr,
                                   Settl = group.Key.Settl,
                                   Street = group.Key.Street,
                                   BuildNumb = group.Key.BuildNumb,
                                   CaseNumb = group.Key.CaseNumb,
                                   ApNumb = group.Key.ApNumb,
                                   Count = group.Count()
                               })
                               .ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }

        }
        List<PotentialRecordsSK>? ReqOwnerToDbInfRegAvia(string searchValue)
        {
            try
            {
                //var listFirtName = modelContextRPS_SK_SR.InfShipBooksN
                //            .Where(item => item.Pib != null && EF.Functions.Like(item.Pib.ToString(), $"%{searchValue}%"))
                //            .GroupBy(item => item.Pib) // group by Owners
                //            .Select(group => new PotentialRecords
                //            {
                //                Name = group.Key,
                //                Count = group.Count()
                //            })
                //            .ToList();

                List<PotentialRecordsSK> result = new();

                if (isFiz)
                {
                    result = modelContextRPS_SK_SR.InfShipBooksN
                                .Where(item =>  item.Pib != null && EF.Functions.Like(item.Pib.ToLower(), $"%{searchValue.ToLower()}%"))
                                .GroupBy(item => new { item.Pib, item.Ipn, item.Pass, item.DtBirth, item.Reg, item.Distr, item.Settl, item.Street, item.BuildNumb, item.CaseNumb, item.ApNumb })
                                .Select(group => new PotentialRecordsSK
                                {
                                    Pib = group.Key.Pib,
                                    Ipn = group.Key.Ipn,
                                    Passp = group.Key.Pass,
                                    Dt = group.Key.DtBirth,
                                    Addres = $"Обл. {group.Key.Reg}, р-н.{group.Key.Distr}, н.п. {group.Key.Settl}, вул. {group.Key.Street}, № буд. {group.Key.BuildNumb}, № корп. {group.Key.CaseNumb}, № кв.(оф.) {group.Key.ApNumb}",
                                    Reg = group.Key.Reg,
                                    Distr = group.Key.Distr,
                                    Settl = group.Key.Settl,
                                    Street = group.Key.Street,
                                    BuildNumb = group.Key.BuildNumb,
                                    CaseNumb = group.Key.CaseNumb,
                                    ApNumb = group.Key.ApNumb,
                                    Count = group.Count()
                                })
                                .ToList();
                }
                else
                {
                    result = modelContextRPS_SK_SR.InfShipBooksN
                               .Where(item =>  item.NameO != null && EF.Functions.Like(item.NameO.ToLower(), $"%{searchValue.ToLower()}%"))
                               .GroupBy(item => new { item.NameO, item.Edrpou, item.Pass, item.DtBirth, item.Reg, item.Distr, item.Settl, item.Street, item.BuildNumb, item.CaseNumb, item.ApNumb })
                               .Select(group => new PotentialRecordsSK
                               {
                                   Pib = group.Key.NameO,
                                   Ipn = group.Key.Edrpou,
                                   Passp = group.Key.Pass,
                                   Dt = group.Key.DtBirth,
                                   Addres = $"Обл. {group.Key.Reg}, р-н.{group.Key.Distr}, н.п. {group.Key.Settl}, вул. {group.Key.Street}, № буд. {group.Key.BuildNumb}, № корп. {group.Key.CaseNumb}, № кв.(оф.) {group.Key.ApNumb}",
                                   Reg = group.Key.Reg,
                                   Distr = group.Key.Distr,
                                   Settl = group.Key.Settl,
                                   Street = group.Key.Street,
                                   BuildNumb = group.Key.BuildNumb,
                                   CaseNumb = group.Key.CaseNumb,
                                   ApNumb = group.Key.ApNumb,
                                   Count = group.Count()
                               })
                               .ToList();
                }

                return result;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
            
        }
        
        static public string? GetDefInString(Figurant d)
        {
            if (d.ResFiz != null)
            {
                var birth = d.DtBirth;
                if (birth != null)
                {
                    return $"{d.Fio}, {birth.ToString()?[0..10] ?? ""} р.н., РНОКПП {d.Ipn}";
                }
                return $"{d.Fio}, РНОКПП {d.Ipn}";
            }
            else
            {
                if (d.Code == null || d.Code == "")
                    return $"{d.Name}";
                return $"{d.Name} (ЄДРПОУ {d.Code})";
            }
        }
        string  GetAddresNorm(List<List<string?>> listAddr)
        {
            string ret = "";
            bool isFirst = true;
            foreach (var item in listAddr)
            {
                var first = item.First();
                var second = item[1];
                if(second != null && second != "" && second != "0" && second.ToLower() != "не визначено" && second != "-" &&
                    first != null && first != "")
                {
                    if (isFirst)
                    {
                        var UppF = (first.First() + "").ToString().ToUpper() + first.Substring(1);


                        ret += $"{UppF} {second};\n";
                        isFirst = false;
                    }
                    else
                    {
                        ret += $"{first} {second};\n";
                    }
                    
                }
            }
            return ret;
        }
    }
   

    public class PotentialRecordsSK
    {
        public string? Pib { get; set; } = null;
        public string? Ipn { get; set; } = null;
        public decimal Count { get; set; } = 0;
        public string? Passp { get; set; } = null;
        public string? Dt { get; set; } = null;
        public string? Addres { get; set; } = null;
        public string? Reg { get; set; } = null;
        public string? Distr { get; set; } = null;
        public string? Settl { get; set; } = null;
        public string? Street { get; set; } = null;
        public string? BuildNumb { get; set; } = null;
        public string? CaseNumb { get; set; } = null;
        public string? ApNumb { get; set; } = null;
        public bool isToExtract { get; set; } = false;
    }
}
