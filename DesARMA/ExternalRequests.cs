using DesARMA.Models;
using NPOI.OpenXml4Net.OPC;
using NPOI.XWPF.Model;
using NPOI.XWPF.UserModel;
using Org.BouncyCastle.Asn1.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesARMA
{
    public enum EnumExtReq
    {
        ExternalRequestsToDerzhpratsi,
        ExternalRequestsToBank,
        ExternalRequestsToAntymonopolnyi,
        ExternalRequestsToNAPZK,
        ExternalRequestsToIntelektualnyi,
        ExternalRequestsToFondovyi1,
        ExternalRequestsToFondovyiOsnovnyi2,
        ExternalRequestsToHeolohii,
        ExternalRequestsToMytna
    }
    public class ExternalRequests
    {
        //+++
        static public  void ExternalRequestsToDerzhpratsi(List<Figurant> figs, string? fold, string addr, string name)
        {
            XWPFDocument doc1;

            //Directory.CreateDirectory(Environment.CurrentDirectory + "\\Запити");
            Directory.CreateDirectory(fold + "\\Запити\\Держпраці");

            var exeFath = Environment.CurrentDirectory;
            var path = System.IO.Path.Combine(exeFath, "FilesSh\\Держпраці.docx");
            FileInfo fileInfo = new FileInfo(path);
            string path3;
            if (fold!=null)
                path3 = fold + $"\\Запити\\Держпраці\\Запит {name} {fold.Split('\\').Last()}.docx";
            else
                path3 = fold + $"\\Запити\\Держпраці\\Запит {name}.docx";

            fileInfo.CopyTo(path3, true);
            doc1 = new XWPFDocument(OPCPackage.Open(path3));

            Console.WriteLine(doc1.Paragraphs[14].Text + "\n");
            var par = doc1.Paragraphs[14];
            par.ReplaceText(par.Text, $"{name}");
            par = doc1.Paragraphs[16];
            par.ReplaceText(par.Text, addr);
            // Console.WriteLine(doc1.Paragraphs[21].Text);

            for (int i = 0; i < figs.Count; i++)
            {
                var tmpParagraph = doc1.CreateParagraph();
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                tmpParagraph.IndentationFirstLine = 855;
                var tmpRun = tmpParagraph.CreateRun();
                tmpRun.FontSize = 14;
            }

            // пересунуть 1 ліст
            for (int i = doc1.Paragraphs.Count - figs.Count - 1; i >= 24; i--)
            {
                var tmpParagraph = doc1.Paragraphs[i];
                doc1.SetParagraph(tmpParagraph, i + figs.Count);
            }

            // засунуть 1 ліст
            int count = 0;
            foreach (var item in figs)
            {
                var tmpParagraph = doc1.CreateParagraph();
                doc1.SetParagraph(tmpParagraph, 24 + count);
                tmpParagraph.IndentationFirstLine = 855;//570, 855
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                var tmpRun = tmpParagraph.CreateRun();

                var strD = GetDefInString(item);
                if (strD != null)
                {
                    if (count == figs.Count - 1)
                        tmpRun.AppendText($"- {strD}.");
                    else
                        tmpRun.AppendText($"- {strD};");
                }

                tmpRun.FontSize = 14;
                tmpRun.FontFamily = "Times New Roman";
                count++;
            }
            //remove
            for (int i = 0; i < figs.Count; i++)
            {
                int pPos = doc1.GetPosOfParagraph(doc1.Paragraphs[doc1.Paragraphs.Count - 1]);
                doc1.RemoveBodyElement(pPos);
            }

            var path4 = fold + $"\\Запити\\Держпраці\\Відповідь {name}.docx";

            //Збереження звіта 
            using (FileStream sw = File.Create(path4))
            {
                doc1.Write(sw);
                // doc1.Close();
            }
            doc1.Close();
            File.Delete(path4);
        }
        //+++
        static public  void ExternalRequestsToBank(List<Figurant> figs, string? fold, string numKP, string nameBank, string mfo, string addr)
        {
            int countDoc;

            if (figs.Count % 9 == 0) countDoc = figs.Count / 9;
            else countDoc = figs.Count / 9 + 1;


            for (int iCountDoc = 0; iCountDoc < countDoc; iCountDoc++)
            {
                int startFigsInDoc = iCountDoc * 9;
                int finishFigsInDoc = iCountDoc * 9 + 8;

                if (finishFigsInDoc >= figs.Count)
                    finishFigsInDoc = figs.Count - 1;


                XWPFDocument doc1;

                Directory.CreateDirectory(fold + "\\Запити\\Банки");

                var exeFath = Environment.CurrentDirectory;
                var path = System.IO.Path.Combine(exeFath, "FilesSh\\Банк.docx");
                FileInfo fileInfo = new FileInfo(path);

                string path3;
                if (fold != null)
                    path3 = fold + $"\\Запити\\Банки\\Запит {nameBank.Replace('\"', ' ')} - {iCountDoc + 1} {fold.Split('\\').Last()}.docx";
                else
                    path3 = fold + $"\\Запити\\Банки\\Запит {nameBank.Replace('\"', ' ')} - {iCountDoc + 1}.docx";


                fileInfo.CopyTo(path3, true);
                doc1 = new XWPFDocument(OPCPackage.Open(path3));


                Console.WriteLine(doc1.Paragraphs[14].Text + "\n");
                var par = doc1.Paragraphs[18];
                par.ReplaceText("42022010000000016", numKP);

                par = doc1.Paragraphs[14];
                par.ReplaceText("МФО 339500", $"МФО {mfo}");

                par = doc1.Paragraphs[13];
                par.ReplaceText("АТ «Таскомбанк»", $"{nameBank}");

                par = doc1.Paragraphs[15];
                par.ReplaceText(par.Text, $"{addr}");

                // Console.WriteLine(doc1.Paragraphs[21].Text);


                for (int i = startFigsInDoc; i <= finishFigsInDoc; i++)
                {
                    var tmpParagraph = doc1.CreateParagraph();
                    tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                    tmpParagraph.IndentationFirstLine = 570;
                    var tmpRun = tmpParagraph.CreateRun();
                    tmpRun.FontSize = 14;
                }

                // пересунуть 1 ліст
                for (int i = doc1.Paragraphs.Count - (finishFigsInDoc - startFigsInDoc + 1) - 1; i >= 18; i--)
                {
                    var tmpParagraph = doc1.Paragraphs[i];
                    doc1.SetParagraph(tmpParagraph, i + (finishFigsInDoc - startFigsInDoc + 1));
                }

                // засунуть 1 ліст
                int count = 0;
                for (int i = startFigsInDoc; i <= finishFigsInDoc; i++)
                {
                    var tmpParagraph = doc1.CreateParagraph();
                    doc1.SetParagraph(tmpParagraph, 19 + count);
                    tmpParagraph.IndentationFirstLine = 570;//570, 855
                    tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                    var tmpRun = tmpParagraph.CreateRun();

                    var strD = GetDefInString(figs[i]);
                    if (strD != null)
                    {
                        tmpRun.AppendText($"- {strD},");
                    }

                    tmpRun.FontSize = 14;
                    tmpRun.FontFamily = "Times New Roman";
                    count++;
                }
                //remove
                for (int i = startFigsInDoc; i <= finishFigsInDoc; i++)
                {
                    int pPos = doc1.GetPosOfParagraph(doc1.Paragraphs[doc1.Paragraphs.Count - 1]);
                    doc1.RemoveBodyElement(pPos);
                }


                var path4 = fold + $"\\Запити\\Банки\\Відповідь {nameBank.Replace('\"', ' ')} - " + (iCountDoc + 1) + ".docx";
                //Збереження звіта 
                using (FileStream sw = File.Create(path4))
                {
                    doc1.Write(sw);
                    // doc1.Close();
                }
                doc1.Close();
                File.Delete(path4);
            }
        }
        //+++
        static public  void ExternalRequestsToAntymonopolnyi(List<Figurant> figs, string? fold, string numKP)
        {
            XWPFDocument doc1;

            Directory.CreateDirectory(fold + "\\Запити\\АМК");

            var exeFath = Environment.CurrentDirectory;
            var path = System.IO.Path.Combine(exeFath, "FilesSh\\АМК запит.docx");
            FileInfo fileInfo = new FileInfo(path);


            string path3;
            if (fold != null)
                path3 = fold + $"\\Запити\\АМК\\Запит АМК {fold.Split('\\').Last()}.docx";
            else
                path3 = fold + "\\Запити\\АМК\\Запит АМК.docx";


            fileInfo.CopyTo(path3, true);
            doc1 = new XWPFDocument(OPCPackage.Open(path3));


            Console.WriteLine(doc1.Paragraphs[18].Text+"\n");
            var par = doc1.Paragraphs[18];
            par.ReplaceText("42022000000000634", numKP);
           // Console.WriteLine(doc1.Paragraphs[21].Text);


            for (int i = 0; i < figs.Count; i++)
            {
                var tmpParagraph = doc1.CreateParagraph();
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                tmpParagraph.IndentationFirstLine = 570;
                var tmpRun = tmpParagraph.CreateRun();
                tmpRun.FontSize = 14;
            }

            // пересунуть 1 ліст
            for (int i = doc1.Paragraphs.Count - figs.Count - 1; i >= 19; i--)
            {
                var tmpParagraph = doc1.Paragraphs[i];
                doc1.SetParagraph(tmpParagraph, i + figs.Count);
            }

            // засунуть 1 ліст
            int count = 0;
            foreach (var item in figs)
            {
                var tmpParagraph = doc1.CreateParagraph();
                doc1.SetParagraph(tmpParagraph, 19 + count);
                tmpParagraph.IndentationFirstLine = 570;//570, 855
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                var tmpRun = tmpParagraph.CreateRun();

                var strD = GetDefInString(item);
                if (strD != null)
                {
                    if (count == figs.Count - 1)
                        tmpRun.AppendText($"- {strD},");
                    else
                        tmpRun.AppendText($"- {strD};");
                }

                tmpRun.FontSize = 14;
                tmpRun.FontFamily = "Times New Roman";
                count++;
            }
            //remove
            for (int i = 0; i < figs.Count; i++)
            {
                int pPos = doc1.GetPosOfParagraph(doc1.Paragraphs[doc1.Paragraphs.Count - 1]);
                doc1.RemoveBodyElement(pPos);
            }


            var path4 = fold + "\\Запити\\АМК\\Відповідь АМК.docx";
            //Збереження звіта 
            using (FileStream sw = File.Create(path4))
            {
                doc1.Write(sw);
                // doc1.Close();
            }
            doc1.Close();
            File.Delete(path4);
        }
        //+++
        static public  void ExternalRequestsToNAPZK(List<Figurant> figs, string? fold, string numKP)
        {
            XWPFDocument doc1;

            Directory.CreateDirectory(fold + "\\Запити\\НАЗК");

            var exeFath = Environment.CurrentDirectory;
            var path = System.IO.Path.Combine(exeFath, "FilesSh\\НАПЗК.docx");
            FileInfo fileInfo = new FileInfo(path);





            string path3;
            if (fold != null)
                path3 = fold + $"\\Запити\\НАЗК\\Запит НАЗК {fold.Split('\\').Last()}.docx";
            else
                path3 = fold + "\\Запити\\НАЗК\\Запит НАЗК.docx";



            fileInfo.CopyTo(path3, true);
            doc1 = new XWPFDocument(OPCPackage.Open(path3));


            //Console.WriteLine(doc1.Paragraphs[21].Text+"\n");
            var par = doc1.Paragraphs[21];
            par.ReplaceText("62020100000000000", numKP);
            //Console.WriteLine(doc1.Paragraphs[21].Text);


            for (int i = 0; i < figs.Count; i++)
            {
                var tmpParagraph = doc1.CreateParagraph();
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
               // tmpParagraph.IndentationFirstLine = 570;
                var tmpRun = tmpParagraph.CreateRun();
                tmpRun.FontSize = 14;
            }

            // пересунуть 1 ліст
            for (int i = doc1.Paragraphs.Count - figs.Count - 1; i >= 22; i--)
            {
                var tmpParagraph = doc1.Paragraphs[i];
                doc1.SetParagraph(tmpParagraph, i + figs.Count);
            }

            // засунуть 1 ліст
            int count = 0;
            foreach (var item in figs)
            {
                var tmpParagraph = doc1.CreateParagraph();
                doc1.SetParagraph(tmpParagraph, 22 + count);
                tmpParagraph.IndentationFirstLine = 142; //570, 855
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                var tmpRun = tmpParagraph.CreateRun();

                var strD = GetDefInString(item);
                if (strD != null)
                {
                    if (count == figs.Count - 1)
                        tmpRun.AppendText($"- {strD}.");
                    else
                        tmpRun.AppendText($"- {strD};");
                }

                tmpRun.FontSize = 14;
                count++;
            }
            //remove
            for (int i = 0; i < figs.Count; i++)
            {
                int pPos = doc1.GetPosOfParagraph(doc1.Paragraphs[doc1.Paragraphs.Count - 1]);
                doc1.RemoveBodyElement(pPos);
            }


            var path4 = fold + "\\Запити\\НАЗК\\Відповідь НАЗК.docx";
            //Збереження звіта 
            using (FileStream sw = File.Create(path4))
            {
                doc1.Write(sw);
                // doc1.Close();
            }
            doc1.Close();
            File.Delete(path4);
        }
        //+++
        static public  void ExternalRequestsToIntelektualnyi(List<Figurant> figs, string? pathEnt)
        {
            XWPFDocument doc1;
           
                Directory.CreateDirectory(pathEnt + "\\Запити\\Укрпатент");

                var exeFath = Environment.CurrentDirectory;
                var path = System.IO.Path.Combine(exeFath, "FilesSh\\Запит Укрпатент.docx");
                FileInfo fileInfo = new FileInfo(path);



            string path3;
            if (pathEnt != null)
                path3 = pathEnt + $"\\Запити\\Укрпатент\\Запит Укрпатент {pathEnt.Split('\\').Last()}.docx";
            else
                path3 = pathEnt + $"\\Запити\\Укрпатент\\Запит Укрпатент.docx";



            
                fileInfo.CopyTo(path3, true);
                doc1 = new XWPFDocument(OPCPackage.Open(path3));


                for (int i = 0; i < figs.Count; i++)
                {
                    var tmpParagraph = doc1.CreateParagraph();
                    tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                    tmpParagraph.IndentationFirstLine = 570;
                    var tmpRun = tmpParagraph.CreateRun();
                    tmpRun.FontSize = 14;
                }

                // пересунуть 1 ліст
                for (int i = doc1.Paragraphs.Count - figs.Count - 1; i >= 23; i--)
                {
                    var tmpParagraph = doc1.Paragraphs[i];
                    doc1.SetParagraph(tmpParagraph, i + figs.Count);
                }

                // засунуть 1 ліст
                int count = 0;
                foreach (var item in figs)
                {
                    var tmpParagraph = doc1.CreateParagraph();
                    doc1.SetParagraph(tmpParagraph, 23 + count);
                    tmpParagraph.IndentationFirstLine = 855;//570
                    tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                    var tmpRun = tmpParagraph.CreateRun();

                    var strD = GetDefInString(item);
                    if (strD != null)
                    {
                        if (count == figs.Count - 1)
                            tmpRun.AppendText($"- {strD}.");
                        else
                            tmpRun.AppendText($"- {strD};");
                    }

                    tmpRun.FontSize = 14;
                    count++;
                }
                //remove
                for (int i = 0; i < figs.Count; i++)
                {
                    int pPos = doc1.GetPosOfParagraph(doc1.Paragraphs[doc1.Paragraphs.Count - 1]);
                    doc1.RemoveBodyElement(pPos);
                }


            var path4 = pathEnt + "\\Запити\\Укрпатент\\Відповідь Укрпатент.docx";
            //Збереження звіта 
            using (FileStream sw = File.Create(path4))
            {
                doc1.Write(sw);
              //doc1.Close();
            }
            doc1.Close();
            File.Delete(path4);
        }
        //+++
        static public  void ExternalRequestsToFondovyi1(List<Figurant> figs, string? pathEnt)
        {
            XWPFDocument doc1;

            Directory.CreateDirectory(pathEnt + "\\Запити\\НКЦПФР 1");

            var exeFath = Environment.CurrentDirectory;
            var path = System.IO.Path.Combine(exeFath, "FilesSh\\НКЦПФР 1.docx");
            FileInfo fileInfo = new FileInfo(path);




            string path3;
            if (pathEnt != null)
                path3 = pathEnt + $"\\Запити\\НКЦПФР 1\\Запит НКЦПФР {pathEnt.Split('\\').Last()}.docx";
            else
                path3 = pathEnt + "\\Запити\\НКЦПФР 1\\Запит НКЦПФР.docx";



            fileInfo.CopyTo(path3, true);
            doc1 = new XWPFDocument(OPCPackage.Open(path3));


            for (int i = 0; i < figs.Count; i++)
             {
                    var tmpParagraph = doc1.CreateParagraph();
                    tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                    tmpParagraph.IndentationFirstLine = 570;
                    var tmpRun = tmpParagraph.CreateRun();
                    tmpRun.FontSize = 14;
            }

                // пересунуть 1 ліст
            for (int i = doc1.Paragraphs.Count - figs.Count - 1; i >= 15; i--)
             {
                    var tmpParagraph = doc1.Paragraphs[i];
                    doc1.SetParagraph(tmpParagraph, i + figs.Count);
             }

                // засунуть 1 ліст
            int count = 0;
             foreach (var item in figs)
              {
                    var tmpParagraph = doc1.CreateParagraph();
                    doc1.SetParagraph(tmpParagraph, 15 + count);
                    tmpParagraph.IndentationFirstLine = 855;//570
                    tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                    var tmpRun = tmpParagraph.CreateRun();

                    var strD = GetDefInString(item);
                    if (strD != null)
                    {
                        if (count == figs.Count - 1)
                            tmpRun.AppendText($"- {strD}.");
                        else
                            tmpRun.AppendText($"- {strD};");
                    }

                    tmpRun.FontSize = 14;
                    count++;
              }
                //remove
             for (int i = 0; i < figs.Count; i++)
              {
                    int pPos = doc1.GetPosOfParagraph(doc1.Paragraphs[doc1.Paragraphs.Count - 1]);
                    doc1.RemoveBodyElement(pPos);
              }

            var path4 = pathEnt + "\\Запити\\НКЦПФР 1\\Відповідь НКЦПФР.docx";
            //Збереження звіта 
            using (FileStream sw = File.Create(path4))
            {
                doc1.Write(sw);
                // doc1.Close();
            }
            doc1.Close();
            File.Delete(path4);
        }
        //+++
        static public  void ExternalRequestsToFondovyiOsnovnyi2(List<Figurant> figs, string? pathEnt)
        {
            XWPFDocument doc1;

            Directory.CreateDirectory(pathEnt + "\\Запити\\НКЦПФР 2");

            var exeFath = Environment.CurrentDirectory;
            var path = System.IO.Path.Combine(exeFath, "FilesSh\\НКЦПФР2.docx");
            FileInfo fileInfo = new FileInfo(path);

            string path3;
            if (pathEnt != null)
                path3 = pathEnt + $"\\Запити\\НКЦПФР 2\\Запит НКЦПФР2 {pathEnt.Split('\\').Last()}.docx";
            else
                path3 = pathEnt + "\\Запити\\НКЦПФР 2\\Запит НКЦПФР2.docx";


            
            fileInfo.CopyTo(path3, true);
            doc1 = new XWPFDocument(OPCPackage.Open(path3));


            for (int i = 0; i < figs.Count; i++)
            {
                var tmpParagraph = doc1.CreateParagraph();
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                tmpParagraph.IndentationFirstLine = 570;
                var tmpRun = tmpParagraph.CreateRun();
                tmpRun.FontSize = 14;
            }

            // пересунуть 1 ліст
            for (int i = doc1.Paragraphs.Count - figs.Count - 1; i >= 15; i--)
            {
                var tmpParagraph = doc1.Paragraphs[i];
                doc1.SetParagraph(tmpParagraph, i + figs.Count);
            }

            // засунуть 1 ліст
            int count = 0;
            foreach (var item in figs)
            {
                var tmpParagraph = doc1.CreateParagraph();
                doc1.SetParagraph(tmpParagraph, 15 + count);
                tmpParagraph.IndentationFirstLine = 855;//570
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                var tmpRun = tmpParagraph.CreateRun();


                var strD = GetDefInString(item);
                if (strD != null)
                {
                    if (count == figs.Count - 1)
                        tmpRun.AppendText($"- {strD}.");
                    else
                        tmpRun.AppendText($"- {strD};");
                }
                tmpRun.FontSize = 14;
                count++;
            }
            //remove
            for (int i = 0; i < figs.Count; i++)
            {
                int pPos = doc1.GetPosOfParagraph(doc1.Paragraphs[doc1.Paragraphs.Count - 1]);
                doc1.RemoveBodyElement(pPos);
            }




            var path4 = pathEnt + "\\Запити\\НКЦПФР 2\\Відповідь НКЦПФР2.docx";
            //Збереження звіта 
            using (FileStream sw = File.Create(path4))
            {
                doc1.Write(sw);
                // doc1.Close();
            }
            doc1.Close();
            File.Delete(path4);
        }
        //+++
        static public  void ExternalRequestsToHeolohii(List<Figurant> figs, string? pathEnt)
        {
            XWPFDocument doc1;

            Directory.CreateDirectory(pathEnt + "\\Запити\\Геонадра");

            var exeFath = Environment.CurrentDirectory;
            var path = System.IO.Path.Combine(exeFath, "FilesSh\\Геонадра.docx");
            FileInfo fileInfo = new FileInfo(path);


            string path3;
            if (pathEnt != null)
                path3 = pathEnt + $"\\Запити\\Геонадра\\Запит Геонадра {pathEnt.Split('\\').Last()}.docx";
            else
                path3 = pathEnt + "\\Запити\\Геонадра\\Запит Геонадра.docx";

            fileInfo.CopyTo(path3, true);
            doc1 = new XWPFDocument(OPCPackage.Open(path3));


            for (int i = 0; i < figs.Count; i++)
            {
                var tmpParagraph = doc1.CreateParagraph();
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                tmpParagraph.IndentationFirstLine = 570;
                var tmpRun = tmpParagraph.CreateRun();
                tmpRun.FontSize = 14;
                
            }

            // пересунуть 1 ліст
            for (int i = doc1.Paragraphs.Count - figs.Count - 1; i >= 26; i--)
            {
                var tmpParagraph = doc1.Paragraphs[i];
                doc1.SetParagraph(tmpParagraph, i + figs.Count);
            }

            // засунуть 1 ліст
            int count = 0;
            foreach (var item in figs)
            {
                var tmpParagraph = doc1.CreateParagraph();
                doc1.SetParagraph(tmpParagraph, 26 + count);
                tmpParagraph.IndentationFirstLine = 855;    //570
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                //tmpParagraph.setSpacingBetween(0.5, LineSpacingRule.ATLEAST);
                var tmpRun = tmpParagraph.CreateRun();

                var strD = GetDefInString(item);
                if (strD != null)
                {
                    if (count == figs.Count - 1)
                        tmpRun.AppendText($"- {strD}.");
                    else
                        tmpRun.AppendText($"- {strD};");
                }

                tmpRun.FontSize = 14;
                tmpRun.FontFamily = "Times New Roman";
                count++;
            }
            //remove
            for (int i = 0; i < figs.Count; i++)
            {
                int pPos = doc1.GetPosOfParagraph(doc1.Paragraphs[doc1.Paragraphs.Count - 1]);
                doc1.RemoveBodyElement(pPos);
            }




            var path4 = pathEnt + "\\Запити\\Геонадра\\Відповідь Геонадра.docx";
            //Збереження звіта 
            using (FileStream sw = File.Create(path4))
            {
                doc1.Write(sw);
                // doc1.Close();
            }
            doc1.Close();
            File.Delete(path4);
        }
        //+++
        static public  void ExternalRequestsToMytna(List<Figurant> figs, string? pathEnt)
        {
            XWPFDocument doc1;
            Directory.CreateDirectory(pathEnt + "\\Запити\\Держмитслужба");

            var exeFath = Environment.CurrentDirectory;
            var path = System.IO.Path.Combine(exeFath, "FilesSh\\Держмитслужба.docx");
            FileInfo fileInfo = new FileInfo(path);

            string path3;
            if (pathEnt != null)
                path3 = pathEnt + $"\\Запити\\Держмитслужба\\Запит Держмитслужба {pathEnt.Split('\\').Last()}.docx";
            else
                path3 = pathEnt + "\\Запити\\Держмитслужба\\Запит Держмитслужба.docx";


            fileInfo.CopyTo(path3, true);
            doc1 = new XWPFDocument(OPCPackage.Open(path3));


            for (int i = 0; i < figs.Count; i++)
            {
                var tmpParagraph = doc1.CreateParagraph();
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                tmpParagraph.IndentationFirstLine = 570;
                var tmpRun = tmpParagraph.CreateRun();
                tmpRun.FontSize = 14;

            }

            // пересунуть 1 ліст
            for (int i = doc1.Paragraphs.Count - figs.Count - 1; i >= 24; i--)
            {
                var tmpParagraph = doc1.Paragraphs[i];
                doc1.SetParagraph(tmpParagraph, i + figs.Count);
            }

            // засунуть 1 ліст
            int count = 0;
            foreach (var item in figs)
            {
                var tmpParagraph = doc1.CreateParagraph();
                doc1.SetParagraph(tmpParagraph, 24 + count);
                tmpParagraph.IndentationFirstLine = 570;    //855
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                //tmpParagraph.setSpacingBetween(0.5, LineSpacingRule.ATLEAST);
                var tmpRun = tmpParagraph.CreateRun();

                var strD = GetDefInString(item);
                if(strD != null)
                {
                    if (count == figs.Count - 1)
                        tmpRun.AppendText($"- {strD}.");
                    else
                        tmpRun.AppendText($"- {strD};");
                }

                
                tmpRun.FontSize = 14;
                tmpRun.FontFamily = "Times New Roman";
                count++;
            }
            //remove
            for (int i = 0; i < figs.Count; i++)
            {
                int pPos = doc1.GetPosOfParagraph(doc1.Paragraphs[doc1.Paragraphs.Count - 1]);
                doc1.RemoveBodyElement(pPos);
            }




            var path4 = pathEnt + "\\Запити\\Держмитслужба\\Відповідь Держмитслужба.docx";
            //Збереження звіта 
            using (FileStream sw = File.Create(path4))
            {
                doc1.Write(sw);
                // doc1.Close();
            }
            doc1.Close();
            File.Delete(path4);
        }
    
        static public  string? GetDefInString(Figurant d)
        {
            if(d.ResFiz != null)
            {
                string strDt = "";
                if (d.DtBirth != null)
                {
                    return $"{d.Name}, {d.DtBirth.ToString().Substring(0, 10)} р.н., РНОКПП {d.Ipn}";
                }
                return $"{d.Name}, РНОКПП {d.Ipn}";
            }
            else
            {
                if(d.ResUr == false)
                    return $"{d.Name}";
                return $"{d.Name} (ЄДРПОУ {d.Code})";
            }
            return null;
        }
    }
        
}
