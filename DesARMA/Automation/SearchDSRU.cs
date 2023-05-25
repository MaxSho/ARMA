using DesARMA.ModelRPS_SK_SR;
using DesARMA.Models;
using DesARMA.Registers;
using DesARMA.SearchWin;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static NPOI.POIFS.Crypt.CryptoFunctions;
using System.Windows.Media.Media3D;
using static NPOI.HSSF.Util.HSSFColor;
using BitMiracle.LibTiff.Classic;

namespace DesARMA.Automation
{
    public class SearchDSRU
    {
        public List<PotentialRecordsDSRU> OrderOwner = new();
        ModelContextRPS_SK_SR modelContextRPS_SK_SR = new();
        string name;
        string code;
        bool isFiz;
        List<PotentialRecordsDSRU>? listOwnerGroup = null!;
        Figurant figurant = null!;
        string path;
        public string NameFirst { get; set; } = "";
        public string? FullName
        {
            get
            {
                return GetDefInString(figurant);
            }
        }
        public SearchDSRU(string name, string code, bool isFiz, ProgresWindow progresWindow, Figurant figurant, int numberR, string path) 
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
                    if (listOwnerGroup != null && listOwnerGroup.Count > 0)
                    {
                        WindowSearchDSRU windowSearchDSRU = new(listOwnerGroup, this);
                        windowSearchDSRU.ShowDialog();
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
        public void CreatePDFOwner()
        {
            try
            {
                if (OrderOwner.Count != 0)
                {
                    string nameFolder = $"\\{GetWhithout(name)}_{code}";
                    string nameFile;

                    if (code == null || code == "")
                        nameFile = $"\\Витяг_ДСРУ_{GetWhithout(name)}.docx";
                    else
                        nameFile = $"\\Витяг_ДСРУ_{code}.docx";

                    string nameFileTemp;
                    if (code == null || code == "")
                        nameFileTemp = $"\\Витяг_ДСРУ_{GetWhithout(name)}Temp.docx";
                    else
                        nameFileTemp = $"\\Витяг_ДСРУ_{code}Temp.docx";

                    if (!Directory.Exists(path + nameFolder))
                        Directory.CreateDirectory(path + nameFolder);

                    PDF pDF = new PDF();
                    pDF.CreateNamesFieldDSRU_PDF(GetListAboutOwnerSubPDF(), code == null || code == "" ? name : code,
                        System.IO.Path.Combine(Environment.CurrentDirectory + "\\FilesReestSh\\DSRUSh.docx"),
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
                var list = new List<InfShipRegN>();
                foreach (var item in OrderOwner)
                {

                    // MessageBox.Show(item.Keys.ToString());
                    List<InfShipRegN> query = new();
 
                    if (isFiz)
                    {
                        query = (from row in context.InfShipRegsN
                                 where item.Ipn == row.Ipn &&
                                 item.Pib == row.Pib &&
                                 item.Passp == row.Pass &&
                                 item.Addres == row.AddrU &&
                                 item.Dt == row.DtBirth
                                 select row).ToList();
                    }
                    else
                    {
                        query = (from row in context.InfShipRegsN
                                 where item.Ipn == row.Edrpou &&
                                 item.Pib == row.NameL &&
                                 item.Passp == row.Pass &&
                                 item.Addres == row.AddrU &&
                                 item.Dt == row.DtBirth
                                 select row).ToList();
                    }
                       

                    //List<InfShipRegN> query = new();
                    //foreach (var curVal in item.listAll)
                    //{
                    //    query.AddRange((from row in context.InfShipRegsN
                    //                    where curVal.Id == row.Id
                    //                    select row).ToList());
                    //}
                    
                    //List<InfShipRegN> query = null!;
                    //if (item.Ipn != null && item.Pib == null)
                    //{
                    //    query = (from row in context.InfShipRegsN
                    //             where item.Ipn == row.Ipn
                    //             select row).ToList();
                    //}
                    //else if (item.Pib != null && item.Ipn == null)
                    //{
                    //    query = (from row in context.InfShipRegsN
                    //             where item.Pib == row.Pib
                    //             select row).ToList();
                    //}
                    //else if (item.Pib != null && item.Ipn != null)
                    //{
                    //    query = (from row in context.InfShipRegsN
                    //             where item.Pib == row.Pib && item.Ipn == row.Ipn
                    //             select row).ToList();
                    //}

                    if(query != null)
                        list.AddRange(query);

                }

                foreach (var item in list)
                {
                    strings.Add(new List<string?>());


                    strings.Last().Add(item.DtFReg);
                    strings.Last().Add(item.RegNumb);
                    strings.Last().Add(item.DtReg);
                    strings.Last().Add(item.TReg);
                    strings.Last().Add(item.DocReg);
                    strings.Last().Add(item.Name);
                    strings.Last().Add(item.NameOld);
                    strings.Last().Add(item.Model);
                    strings.Last().Add(item.IdHull);
                    strings.Last().Add(item.Type);
                    strings.Last().Add(item.Funct);
                    strings.Last().Add(item.Port);
                    strings.Last().Add(item.Cntr);
                    strings.Last().Add(item.DtConst);
                    strings.Last().Add(item.Area);
                    strings.Last().Add(item.Class);
                    strings.Last().Add(item.Imo);
                    strings.Last().Add(item.Inmar);
                    strings.Last().Add(item.Yard);
                    strings.Last().Add(item.ConvInst);
                    strings.Last().Add(item.Pib);
                    strings.Last().Add(item.Ipn);
                    strings.Last().Add(item.Pass);
                    strings.Last().Add(item.DtBirth);
                    strings.Last().Add(item.NameL);
                    strings.Last().Add(item.Edrpou);
                    strings.Last().Add(item.AddrU);
                    strings.Last().Add(item.Chart);
                    strings.Last().Add(item.EdrpouC);
                    strings.Last().Add(item.PibC);
                    strings.Last().Add(item.DtBirthC);
                    strings.Last().Add(item.PassC);
                    strings.Last().Add(item.IpnC);
                    strings.Last().Add(item.AddrC);
                    strings.Last().Add(item.Length);
                    strings.Last().Add(item.Width);
                    strings.Last().Add(item.Height);
                    strings.Last().Add(item.Cpct);
                    strings.Last().Add(item.Mat);
                    strings.Last().Add(item.TEng);
                    strings.Last().Add(item.PEng);
                    strings.Last().Add(item.CEng);
                    strings.Last().Add(item.Crew);
                    strings.Last().Add(item.Passen);
                    strings.Last().Add(item.Ddw);
                    strings.Last().Add(item.Masts);
                    strings.Last().Add(item.DtTmp);
                    strings.Last().Add(item.DtCons);
                    strings.Last().Add(item.Eni);
                }
            }
            return strings;
        }
        public void GenerGroup()
        {
            try
            {
                string nameFirst = GetNameStrWithoutFormGospAbreviat(name);
                string nameFirst2 = GetNameStrWithoutFormGosp(nameFirst);
                //NameFirst = nameFirst2.Trim().Substring(0, nameFirst2.Trim().IndexOf(' '));

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
                    if (del.Length == 1)
                    {
                        NameFirst = del.First();
                    }
                    else if (del.Length >= 2)
                    {
                        NameFirst = $"{del[0]} {del[1]}";
                    }
                }
                else
                {
                    var str = nameFirst2.Trim();
                    var ind = str.IndexOf(' ');
                    if (ind != -1)
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
                    listOwnerGroup = ReqOwnerToDbInfShipRegsAll(NameFirst, code);
                    NameFirst += " і " + code;
                }
                else if (code != "")
                {
                    listOwnerGroup = ReqOwnerToDbInfShipRegsCode(code);
                    NameFirst = code;
                }
                else if (NameFirst != "")
                {
                    listOwnerGroup = ReqOwnerToDbInfShipRegs(NameFirst);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        List<PotentialRecordsDSRU>? ReqOwnerToDbInfShipRegsAll(string searchValueName, string searchValueCode)
        {
            try
            {
                //var groupQuery = modelContextRPS_SK_SR.InfShipRegsN
                //.Where(item => EF.Functions.Like(item.Ipn, $"%{searchValueCode}%") || EF.Functions.Like(item.Pib.ToLower(), $"%{searchValueName.ToLower()}%"))
                //.GroupBy(item => new { item.Pib, item.Ipn, item.Pass, item.DtBirth, item.AddrU });

                //var result = groupQuery
                //    .AsEnumerable()
                //               .Select(group => new PotentialRecordsDSRU
                //               {
                //                   Pib = group.Key.Pib,
                //                   Ipn = group.Key.Ipn,
                //                   Passp = group.Key.Pass,
                //                   Dt = group.Key.DtBirth,
                //                   Addres = group.Key.AddrU,
                //                   Count = group.Count(),
                //                   Keys = group
                //               })
                //               .ToList();

                List<PotentialRecordsDSRU> result = new();

                if(isFiz)
                {
                    result = modelContextRPS_SK_SR.InfShipRegsN
                                .Where(item => item.Ipn != null && EF.Functions.Like(item.Ipn, $"%{searchValueCode}%") || item.Pib != null && EF.Functions.Like(item.Pib.ToLower(), $"%{searchValueName.ToLower()}%"))
                                .GroupBy(item => new { item.Pib, item.Ipn, item.Pass, item.DtBirth, item.AddrU })
                                .Select(group => new PotentialRecordsDSRU
                                {
                                    Pib = group.Key.Pib,
                                    Ipn = group.Key.Ipn,
                                    Passp = group.Key.Pass,
                                    Dt = group.Key.DtBirth,
                                    Addres = group.Key.AddrU,
                                    Count = group.Count()
                                })
                                .ToList();
                }
                else
                {
                    result = modelContextRPS_SK_SR.InfShipRegsN
                               .Where(item => item.Edrpou != null && EF.Functions.Like(item.Edrpou, $"%{searchValueCode}%") || item.NameL != null && EF.Functions.Like(item.NameL.ToLower(), $"%{searchValueName.ToLower()}%"))
                               .GroupBy(item => new { item.NameL, item.Edrpou, item.Pass, item.DtBirth, item.AddrU })
                               .Select(group => new PotentialRecordsDSRU
                               {
                                   Pib = group.Key.NameL,
                                   Ipn = group.Key.Edrpou,
                                   Passp = group.Key.Pass,
                                   Dt = group.Key.DtBirth,
                                   Addres = group.Key.AddrU,
                                   Count = group.Count()
                               })
                               .ToList();
                }




                //var result = modelContextRPS_SK_SR.InfShipRegsN.AsEnumerable()
                //        .Where(item => item.Ipn != null &&  item.Ipn.Contains(searchValueCode) || item.Pib != null && item.Pib.ToLower().Contains(searchValueName.ToLower()))
                //        .GroupBy(item => new { item.Pib, item.Ipn, item.Pass, item.DtBirth, item.AddrU })
                //        .Select(group => new PotentialRecordsDSRU
                //        {
                //            Pib = group.Key.Pib,
                //            Ipn = group.Key.Ipn,
                //            Passp = group.Key.Pass,
                //            Dt = group.Key.DtBirth,
                //            Addres = group.Key.AddrU,
                //            Count = group.Count(),
                //            Keys = group
                //        })
                //        .ToList();


                return result;
                //return listFirtName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }

        }
        List<PotentialRecordsDSRU>? ReqOwnerToDbInfShipRegsCode(string searchValue)
        {
            try
            {
                // var groupQuery = modelContextRPS_SK_SR.InfShipRegsN
                //.Where(item => item.Ipn != null && EF.Functions.Like(item.Ipn.ToString().ToLower(), $"%{searchValue}%"))
                //.GroupBy(item => new { item.Pib, item.Ipn, item.Pass, item.DtBirth, item.AddrU });

                // var result = groupQuery
                //     .AsEnumerable()
                //                .Select(group => new PotentialRecordsDSRU
                //                {
                //                    Pib = group.Key.Pib,
                //                    Ipn = group.Key.Ipn,
                //                    Passp = group.Key.Pass,
                //                    Dt = group.Key.DtBirth,
                //                    Addres = group.Key.AddrU,
                //                    Count = group.Count(),
                //                    Keys = group
                //                })
                //                .ToList();


                List<PotentialRecordsDSRU> result = new();

                if(isFiz)
                {
                    result = modelContextRPS_SK_SR.InfShipRegsN
                            .Where(item => item.Ipn != null && EF.Functions.Like(item.Ipn, $"%{searchValue}%"))
                            .GroupBy(item => new { item.Pib, item.Ipn, item.Pass, item.DtBirth, item.AddrU })
                            .Select(group => new PotentialRecordsDSRU
                            {
                                Pib = group.Key.Pib,
                                Ipn = group.Key.Ipn,
                                Passp = group.Key.Pass,
                                Dt = group.Key.DtBirth,
                                Addres = group.Key.AddrU,
                                Count = group.Count()
                            })
                            .ToList();
                }
                else
                {
                    result = modelContextRPS_SK_SR.InfShipRegsN
                           .Where(item => item.Edrpou != null && EF.Functions.Like(item.Edrpou, $"%{searchValue}%"))
                           .GroupBy(item => new { item.NameL, item.Edrpou, item.Pass, item.DtBirth, item.AddrU })
                           .Select(group => new PotentialRecordsDSRU
                           {
                               Pib = group.Key.NameL,
                               Ipn = group.Key.Edrpou,
                               Passp = group.Key.Pass,
                               Dt = group.Key.DtBirth,
                               Addres = group.Key.AddrU,
                               Count = group.Count()
                           })
                           .ToList();
                }

               

                //var listFirtName = modelContextRPS_SK_SR.InfShipRegsN.AsEnumerable()
                //        .Where(item => item.Ipn != null && item.Ipn.ToLower().Contains(searchValue.ToLower()))
                //        .GroupBy(item => new { item.Pib, item.Ipn, item.Pass, item.DtBirth, item.AddrU })
                //        .Select(group => new PotentialRecordsDSRU
                //        {
                //            Pib = group.Key.Pib,
                //            Ipn = group.Key.Ipn,
                //            Passp = group.Key.Pass,
                //            Dt = group.Key.DtBirth,
                //            Addres = group.Key.AddrU,
                //            Count = group.Count(),
                //            Keys = group
                //        })
                //        .ToList();



                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }

        }
        List<PotentialRecordsDSRU>? ReqOwnerToDbInfShipRegs(string searchValue)
        {
            try
            {
                //  var groupQuery = modelContextRPS_SK_SR.InfShipRegsN
                //.Where(item => item.Pib != null && EF.Functions.Like(item.Pib.ToLower(), $"%{searchValue.ToLower()}%"))
                //.GroupBy(item => new { item.Pib, item.Ipn, item.Pass, item.DtBirth, item.AddrU });

                //  var result = groupQuery.AsEnumerable()
                //                 .Select(group => new PotentialRecordsDSRU
                //                 {
                //                     Pib = group.Key.Pib,
                //                     Ipn = group.Key.Ipn,
                //                     Passp = group.Key.Pass,
                //                     Dt = group.Key.DtBirth,
                //                     Addres = group.Key.AddrU,
                //                     Count = group.Count(),
                //                     Keys =  group
                //                 })
                //                 .ToList();

                List<PotentialRecordsDSRU> result = new();

                if (isFiz)
                {
                    result = modelContextRPS_SK_SR.InfShipRegsN
                            .Where(item => item.Pib != null && EF.Functions.Like(item.Pib.ToLower(), $"%{searchValue.ToLower()}%"))
                            .GroupBy(item => new { item.Pib, item.Ipn, item.Pass, item.DtBirth, item.AddrU })
                            .Select(group => new PotentialRecordsDSRU
                            {
                                Pib = group.Key.Pib,
                                Ipn = group.Key.Ipn,
                                Passp = group.Key.Pass,
                                Dt = group.Key.DtBirth,
                                Addres = group.Key.AddrU,
                                Count = group.Count()
                            })
                            .ToList();
                }
                else
                {
                    result = modelContextRPS_SK_SR.InfShipRegsN
                            .Where(item => item.NameL != null && EF.Functions.Like(item.NameL.ToLower(), $"%{searchValue.ToLower()}%"))
                            .GroupBy(item => new { item.NameL, item.Edrpou, item.Pass, item.DtBirth, item.AddrU })
                            .Select(group => new PotentialRecordsDSRU
                            {
                                Pib = group.Key.NameL,
                                Ipn = group.Key.Edrpou,
                                Passp = group.Key.Pass,
                                Dt = group.Key.DtBirth,
                                Addres = group.Key.AddrU,
                                Count = group.Count()
                            })
                            .ToList();
                }

                   

                //var listFirtName = modelContextRPS_SK_SR.InfShipRegsN
                //    .AsEnumerable()
                //       .Where(item => item.Pib != null && item.Pib.ToLower().Contains(searchValue.ToLower()))
                //       .GroupBy(item => new { item.Pib, item.Ipn, item.Pass, item.DtBirth, item.AddrU })
                //       .Select(group => new PotentialRecordsDSRU
                //       {
                //           Pib = group.Key.Pib,
                //           Ipn = group.Key.Ipn,
                //           Passp = group.Key.Pass,
                //           Dt = group.Key.DtBirth,
                //           Addres = group.Key.AddrU,
                //           Count = group.Count(),
                //           Keys = group
                //       })
                //       .ToList();

                return result;
            }
            catch (Exception ex)
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
    }
    public class PotentialRecordsDSRU
    {
        public string? Pib { get; set; } = null;
        public string? Ipn { get; set; } = null;
        public decimal Count { get; set; } = 0;
        public string? Passp { get; set; } = null;
        public string? Dt { get; set; } = null;
        public string? Addres { get; set; } = null;
        public bool isToExtract { get; set; } = false;
    }
}
