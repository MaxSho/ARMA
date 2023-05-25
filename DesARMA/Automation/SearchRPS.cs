using DesARMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesARMA.ModelRPS_SK_SR;
using System.Windows;
using System.Security.Cryptography;
using SixLabors.ImageSharp.Drawing.Processing;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using DesARMA.SearchWin;
using DesARMA.Registers.EDR;
using SixLabors.ImageSharp;
using System.IO;
using DesARMA.Registers;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.Record;

namespace DesARMA.Automation
{
    public class SearchRPS
    {
        public List<PotentialRecords> OrderOwner = new();
        public List<PotentialRecords> OrderOperator = new();
        ModelContextRPS_SK_SR modelContextRPS_SK_SR = new();
        string name;
        string code;
        bool isFiz;
        List<PotentialRecords>? listOwnerGroup = null!;
        List<PotentialRecords>? listOperatorGroup = null!;
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
        public SearchRPS(string name, string code, bool isFiz, ProgresWindow progresWindow, Figurant figurant, int numberR, string path)
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
                    if (IsHaveInfo())
                    {
                        WindowSearchRPS windowSearchRPS = new(listOwnerGroup, listOperatorGroup, this);
                        windowSearchRPS.ShowDialog();
                        if (IsHaveInfoOrder())
                        {
                            if (OrderOwner != null && OrderOwner.Count > 0)
                            {
                                CreatePDFOwner();
                                progresWindow.SetDoneFigNow(figurant);
                            }
                            if (OrderOperator != null && OrderOperator.Count > 0)
                            {
                                CreatePDFOperator();
                                progresWindow.SetDoneFigNow(figurant);
                            }
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
        bool IsHaveInfoOrder()
        {
            return OrderOwner == null && OrderOperator != null && OrderOperator.Count > 0 ||
                    OrderOwner != null && OrderOwner.Count > 0 ||
                    OrderOwner != null && OrderOwner.Count == 0 && OrderOperator != null && OrderOperator.Count > 0;
        }
        bool IsHaveInfo()
        {
            return listOwnerGroup == null && listOperatorGroup != null && listOperatorGroup.Count > 0 ||
                    listOwnerGroup != null && listOwnerGroup.Count > 0 ||
                    listOwnerGroup != null && listOwnerGroup.Count == 0 && listOperatorGroup != null && listOperatorGroup.Count > 0;

        }
        public void GenerGroup()
        {
            try
            {
                var nameFirst = name.Trim().Substring(0, name.Trim().IndexOf(' '));

                NameFirst = nameFirst;
                if (!isFiz)
                {
                    if (code == "")
                    {
                        listOwnerGroup = ReqOwnerToDbInfRegAvia(nameFirst);
                    }
                    else
                    {
                        listOwnerGroup = ReqOwnerToDbInfRegAvia(code);

                        if (listOwnerGroup?.Count == 0)
                        {
                            listOwnerGroup = ReqOwnerToDbInfRegAvia(nameFirst);
                        }
                    }
                }
                else
                {
                    listOwnerGroup = ReqOwnerToDbInfRegAvia(nameFirst);
                }

                if (!isFiz)
                {
                    if (code == "")
                    {
                        listOperatorGroup = ReqOperatorToDbInfRegAvia(nameFirst);
                    }
                    else
                    {
                        listOperatorGroup = ReqOperatorToDbInfRegAvia(code);

                        if (listOperatorGroup?.Count == 0)
                        {
                            listOperatorGroup = ReqOperatorToDbInfRegAvia(nameFirst);
                        }
                    }
                }
                else
                {
                    listOperatorGroup = ReqOperatorToDbInfRegAvia(nameFirst);
                }
            }
            catch (Exception ex)
            {
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
        List<PotentialRecords>? ReqOwnerToDbInfRegAvia(string searchValue)
        {
            var listFirtName = modelContextRPS_SK_SR.InfRegAvia
                            .AsEnumerable()
                            .Where(item =>
                            {
                                return FindMatchingWords(item.Owners?.ToString() ?? "", searchValue).Count > 0;
                            }
                            )
                            .GroupBy(item => item.Owners) // group by Owners
                            .Select(group => new PotentialRecords
                            {
                                Name = group.Key,
                                Count = group.Count()
                            })
                            .ToList();

            //foreach (var item in listFirtName)
            //{
            //    MessageBox.Show(item.Name + "");
            //}
            return listFirtName;
        }
        List<PotentialRecords>? ReqOperatorToDbInfRegAvia(string searchValue)
        {
            var listFirtName = modelContextRPS_SK_SR.InfRegAvia
                            .AsEnumerable()
                            .Where(item =>
                            {
                                return FindMatchingWords(item.Operator?.ToString() ?? "", searchValue).Count > 0;
                            }
                            )
                            .GroupBy(item => item.Operator) // group by Operator
                            .Select(group => new PotentialRecords
                            {
                                Name = group.Key,
                                Count = group.Count()
                            })
                            .ToList();

            //foreach (var item in listFirtName)
            //{
            //    MessageBox.Show(item.Name + "");
            //}
            return listFirtName;
        }
        public void CreateWin()
        {

        }
        public void ToCheckFigInTree(int indR)
        {
            
            var modelContext = new ModelContext();
            var listC = AllDirectories.GetBoolsFromString(figurant.Control);
            var listS = AllDirectories.GetBoolsFromString(figurant.Shema);
            if (listC != null && listC.Count > indR && listS != null && listS.Count > indR)
            {
                if (IsHaveInfo())
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
                if (OrderOwner.Count != 0 || OrderOperator.Count != 0)
                {
                    string nameFolder = $"\\{GetWhithout(name)}_{code}";
                    string nameFile;

                    if (code == null || code == "")
                        nameFile = $"\\Витяг_В_ДРУПСУ_{GetWhithout(name)}.docx";
                    else
                        nameFile = $"\\Витяг_В_ДРУПСУ_{code}.docx";

                    string nameFileTemp;
                    if (code == null || code == "")
                        nameFileTemp = $"\\Витяг_В_ДРУПСУ_{GetWhithout(name)}Temp.docx";
                    else
                        nameFileTemp = $"\\Витяг_В_ДРУПСУ_{code}Temp.docx";

                    if (!Directory.Exists(path + nameFolder))
                        Directory.CreateDirectory(path + nameFolder);

                    PDF pDF = new PDF();
                    pDF.CreateNamesFieldRPS_PDF(GetListAboutOwnerSubPDF(), name == null || name == "" ? code + "": name , "Власник",
                        System.IO.Path.Combine(Environment.CurrentDirectory + "\\FilesReestSh\\RPSSh.docx"),
                        path + nameFolder + nameFileTemp,
                        path + nameFolder + nameFile
                        );
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void CreatePDFOperator()
        {
            try
            {
                if (OrderOwner.Count != 0 || OrderOperator.Count != 0)
                {
                    string nameFolder = $"\\{GetWhithout(name)}_{code}";
                    string nameFile;

                    if (code == null || code == "")
                        nameFile = $"\\Витяг_Е_ДРУПСУ_{GetWhithout(name)}.docx";
                    else
                        nameFile = $"\\Витяг_Е_ДРУПСУ_{code}.docx";

                    string nameFileTemp;
                    if (code == null || code == "")
                        nameFileTemp = $"\\Витяг_Е_ДРУПСУ_{GetWhithout(name)}Temp.docx";
                    else
                        nameFileTemp = $"\\Витяг_Е_ДРУПСУ_{code}Temp.docx";

                    PDF pDF = new PDF();
                    pDF.CreateNamesFieldRPS_PDF(GetListAboutOperatorSubPDF(), name == null || name == "" ? code + "" : name, "Експлуататор",
                        System.IO.Path.Combine(Environment.CurrentDirectory + "\\FilesReestSh\\RPSSh.docx"),
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
        public List<List<string?>> GetListAboutOwnerSubPDF()
        {
            List<List<string?>> strings = new ();

            using (var context = new ModelContextRPS_SK_SR())
            {
                var query = from row in context.InfRegAvia.AsEnumerable()
                            where OrderOwner.Any(pr => pr.Name == row.Owners)
                            select row;

                foreach (var item in query)
                {
                    strings.Add(new List<string?>());
                    strings.Last().Add(item.Owners);
                    strings.Last().Add(item.Operator);
                    strings.Last().Add(item.PlaneType);
                    strings.Last().Add(item.RegMark + "");
                    strings.Last().Add(item.SNumb + "");
                    strings.Last().Add(item.DtMan+"");
                    strings.Last().Add(item.MaxWeight + "");
                    strings.Last().Add(item.RegDoc + "");
                    strings.Last().Add(item.DtReg + "");
                }
            }
            return strings;
        }
        public List<List<string?>> GetListAboutOperatorSubPDF()
        {
            List<List<string?>> strings = new();

            using (var context = new ModelContextRPS_SK_SR())
            {
                var query = from row in context.InfRegAvia.AsEnumerable()
                            where OrderOperator.Any(pr => pr.Name == row.Operator)
                            select row;

                foreach (var item in query)
                {
                    strings.Add(new List<string?>());
                    strings.Last().Add(item.Owners);
                    strings.Last().Add(item.Operator);
                    strings.Last().Add(item.PlaneType);
                    strings.Last().Add(item.RegMark + "");
                    strings.Last().Add(item.SNumb + "");
                    strings.Last().Add(item.DtMan + "");
                    strings.Last().Add(item.MaxWeight + "");
                    strings.Last().Add(item.RegDoc + "");
                    strings.Last().Add(item.DtReg + "");
                }
            }
            return strings;
        }
    }
    public class PotentialRecords
    {
        public string? Name { get; set; } = null;
        public decimal Count { get; set; } = 0;
        public bool isToExtract { get; set; } = false;
        public IGrouping<string?, InfShipRegN> Group { get; set; }

}
}
