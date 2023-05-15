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

namespace DesARMA.Automation
{
    public class SearchDSRU
    {
        public List<PotentialRecords> OrderOwner = new();
        ModelContextRPS_SK_SR modelContextRPS_SK_SR = new();
        string name;
        string code;
        bool isFiz;
        List<PotentialRecords>? listOwnerGroup = null!;
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
                    pDF.CreateNamesFieldSK_PDF(GetListAboutOwnerSubPDF(), name == null || name == "" ? code + "" : name,
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
                var list = new List<InfShipReg>();
                foreach (var item in OrderOwner)
                {

                    var query = (from row in context.InfShipRegs
                                 where item.Name == row.Pib
                                 select row).ToList();
                    list.AddRange(query);

                }

                foreach (var item in list)
                {
                    strings.Add(new List<string?>());
                    strings.Last().Add(item.DtFReg.ToString());
                    strings.Last().Add(item.RegNumb);
                    strings.Last().Add(item.DtReg.ToString());
                    strings.Last().Add(item.TReg);
                    strings.Last().Add(item.Name);
                    strings.Last().Add(item.Model);
                    strings.Last().Add(item.IdHull);
                    strings.Last().Add(item.Type);
                    strings.Last().Add(item.Funct);
                    strings.Last().Add(item.Port);
                    strings.Last().Add(item.Cntr);
                    strings.Last().Add(item.Area);
                    strings.Last().Add(item.Class);
                    strings.Last().Add(item.Imo.ToString());
                    strings.Last().Add(item.Inmar);
                    strings.Last().Add(item.Pib);
                    strings.Last().Add(item.NameL);
                    strings.Last().Add(item.Chart);
                    strings.Last().Add(item.Length.ToString());
                    strings.Last().Add(item.Width.ToString());
                    strings.Last().Add(item.Height.ToString());
                    strings.Last().Add(item.Cpct.ToString());
                    strings.Last().Add(item.Mat);
                    strings.Last().Add(item.TEng);
                    strings.Last().Add(item.PEng);
                    strings.Last().Add(item.CEng.ToString());
                    strings.Last().Add(item.DtTmp);
                    strings.Last().Add(item.DtCons);

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

                NameFirst = nameFirst2;

                if(nameFirst != "")
                {
                    listOwnerGroup = ReqOwnerToDbInfShipRegs(nameFirst2);
                }
                else if(code != "")
                {
                    listOwnerGroup = ReqOwnerToDbInfShipRegs(code);
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
        List<PotentialRecords>? ReqOwnerToDbInfShipRegs(string searchValue)
        {
            try
            {
                var listFirtName = modelContextRPS_SK_SR.InfShipRegs
                            .Where(item => item.Pib != null && EF.Functions.Like(item.Pib.ToString().ToLower(), $"%{searchValue}%"))
                            .GroupBy(item => item.Pib) 
                            .Select(group => new PotentialRecords
                            {
                                Name = group.Key,
                                Count = group.Count()
                            })
                            .ToList();



                return listFirtName;
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
}
