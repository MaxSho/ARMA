using DesARMA.Models;
using NPOI.OpenXml4Net.OPC;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace DesARMA
{
    public class DocResponse
    {
        MainConfig prevM = null!;
        List<MainConfig> listPrevM = null!;
        private readonly List<int> listParamInt = null!;
        private readonly List<string> listParamString = null!;
        private ModelContext modelContext;

        public DocResponse(List<MainConfig> listPrevM, List<int> listParamInt, List<string> listParamString,
            ModelContext modelContext)
        {
            this.listPrevM = listPrevM;
            this.prevM = listPrevM[0];
            this.listParamInt = listParamInt;
            this.listParamString = listParamString;
            this.modelContext = modelContext;
        }
        public void CreateResponseMINIuST()
        {
            string path3 = "";
            XWPFDocument doc1;
            if (prevM != null && prevM.Folder != null)
            {
                Directory.CreateDirectory(prevM.Folder + "\\Відповідь");

                var exeFath = Environment.CurrentDirectory;
                var path = System.IO.Path.Combine(exeFath, "Files\\Мінюст ДСК.docx");

                FileInfo fileInfo = new FileInfo(path);

                var path2 = System.IO.Path.Combine(exeFath, "FilesRet\\Мінюст ДСК2.docx");
                path3 = prevM.Folder + $"\\Відповідь\\Відповідь {prevM.Folder.Split('\\').Last()}.docx";

                fileInfo.CopyTo(path3, true);

                doc1 = new XWPFDocument(OPCPackage.Open(path3));
            }
            else
            {
                System.Windows.MessageBox.Show($"Не вдалося відкрити контекст запиту. Не знайдено його");
                throw new Exception();
            }

            bool isExReq = false;
            if (Directory.Exists(prevM.Folder + "\\Запити"))
                if (!(Directory.GetDirectories(prevM.Folder + "\\Запити").Length == 0 &&
                                    Directory.GetFiles(prevM.Folder + "\\Запити").Length == 0)
                     )
                {
                    isExReq = true;
                }

            //Створення списків реєстрів родовий і давальний
            if(listParamInt == null || listParamString == null)
            {
                System.Windows.MessageBox.Show($"Помилка коду: порожні списки listParamInt, listParamString");
                return;
            }
            if (listParamInt.Count > 8 && listParamString.Count > 2)
            {
                System.Windows.MessageBox.Show($"Помилка коду: малий розмір списків listParamInt, listParamString");
                return;
            }

            int indexSub = listParamInt[0] + 2;    // isCountFig > 1 ? 0 : 1;
            int whatIndex = listParamInt[1];    // typeorgansList.SelectedIndex;
            string name = listParamString[0]; // nameSubTextBox.Text;
            string address1 = listParamString[1]; // addressOrgTextBox.Text;
            string date1 = listParamString[2]; //dateRequestDatePicker.Text;
            string date2 = listParamString[3]; //dateInTextBox.Text.Substring(0, 10);
            string number1 = listParamString[4]; //numberRequestTextBox.Text;
            string number2 = listParamString[5]; //numberInTextBox.Text;
            string vidOrgan = listParamString[6]; //vidOrgTextBox.Text;
            string positionSub = listParamString[7]; //positionSubTextBox.Text;
            int count_Shemat = listParamInt[2];

            //Створення списків реєстрів родовий і давальний
            List<string> listSRod = new List<string>();
            List<string> listSDav = new List<string>();

            //Збереження даних наявності реєстрів
            if (Directory.Exists(prevM.Folder))
            {
                int i = 1;
                foreach (string itemAbbreviatedName in Reest.abbreviatedName)
                {
                    string[] dirs = Directory.GetDirectories(prevM.Folder);
                    foreach (var itemDir in dirs)
                    {
                        if ((new DirectoryInfo(itemDir)).Name == $"{i}. " + itemAbbreviatedName)
                        {
                            if (Directory.GetDirectories(itemDir).Length > 0 ||
                                 Directory.GetFiles(itemDir).Length > 0
                            )
                            {
                                listSRod.Add(Reest.sRodov[Reest.abbreviatedName.IndexOf(itemAbbreviatedName)]);
                            }
                            else
                            {

                                listSDav.Add(Reest.sDav[Reest.abbreviatedName.IndexOf(itemAbbreviatedName)]);
                            }
                        }
                    }
                    i++;
                }
                var d = Directory.GetDirectories(prevM.Folder);
                foreach (var item in d)
                {
                    if ((new DirectoryInfo(item)).Name == $"{i}. Схеми")
                    {
                        var countD = Directory.GetDirectories(item).Length;
                        var countF = Directory.GetFiles(item).Length;
                        count_Shemat = countD + countF;
                        break;
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Не знайдено папку запиту");
                doc1.Close();
                return;
            }

            //створення множини ідентичних груп додатків
            var listNumering = Orders(listSRod);
            HashSet<int> hset = new HashSet<int>();
            foreach (var item in listNumering)
            {
                hset.Add(item);
            }

            //Формування параграфів


            //MessageBox.Show($"{Reest.sub2[indexSub]}\n\n{doc1.Paragraphs[17].Text}\n\n{doc1.Paragraphs[20].Text}\n\n{doc1.Paragraphs[21].Text}\n\n{doc1.Paragraphs[19].Text}");
            var par = doc1.Paragraphs[17];
            par.ReplaceText("зазначеній", Reest.sub[indexSub]);
            par.ReplaceText("особі", Reest.sub2[indexSub]);

            par.ReplaceText("16.11.2022", $"{date1}");
            par.ReplaceText("108397/38.4.4/11-22", $"{number1}");
            par.ReplaceText("8490/27-22", $"{number2}");
            par.ReplaceText("23.11.2022", $"{date2}");

            par = doc1.Paragraphs[19];
            par.ReplaceText("зазначеній", Reest.sub[indexSub]);
            par.ReplaceText("особі", Reest.sub2[indexSub]);

            par = doc1.Paragraphs[20];

            if (count_Shemat > 0)
                par.ReplaceText("додаток 10-11", $"додаток {hset.Count + 1}");

            par.ReplaceText("зазначених", Reest.sub[indexSub]);
            par.ReplaceText("осіб", Reest.sub2[indexSub]);

            par = doc1.Paragraphs[21];
            par.ReplaceText("зазначеній", Reest.sub[indexSub]);
            par.ReplaceText("особі", Reest.sub2[indexSub]);


            //par = doc1.Paragraphs[33];
            //par.ReplaceText("зазначених", Reest.sub[indexSub]);
            //par.ReplaceText("осіб", Reest.sub2[indexSub]);

            //System.Windows.MessageBox.Show(par.Text);

            //par = doc1.Paragraphs[34];
            //if (whatIndex != -1)
            //    par.ReplaceText(par.Text, Reest.organs[whatIndex]);
            //else
            //    par.ReplaceText(par.Text, Reest.organs[0]);

            //System.Windows.MessageBox.Show(doc1.Paragraphs[23].Text);
            for (int i = 0; i < listSRod.Count; i++)
            {
                var tmpParagraph = doc1.CreateParagraph();
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                tmpParagraph.IndentationFirstLine = 570;
                
                var tmpRun = tmpParagraph.CreateRun();
                tmpRun.FontSize = 14;
                tmpRun.FontFamily = "Times New Roman";
            }
            // пересунуть 1 ліст
            for (int i = doc1.Paragraphs.Count - listSRod.Count - 1; i >= 20; i--)
            {
                var tmpParagraph = doc1.Paragraphs[i];
                doc1.SetParagraph(tmpParagraph, i + listSRod.Count);
            }
            
            // засунуть 1 ліст
            int count_dodat = 0;
            //var listNumering = Orders(listSRod);
            foreach (var item in listSRod)
            {
                var tmpParagraph = doc1.CreateParagraph();
                doc1.SetParagraph(tmpParagraph, 20 + count_dodat);
                tmpParagraph.IndentationFirstLine = 570;
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                tmpParagraph.SpacingAfter = 1;
                var tmpRun = tmpParagraph.CreateRun();
                tmpRun.FontFamily = "Times New Roman";
                tmpRun.AppendText($"{count_dodat + 1}) ");
                if (count_dodat == listSRod.Count - 1)
                    tmpRun.AppendText($"{item} (додаток {listNumering[count_dodat]}).");
                else
                    tmpRun.AppendText($"{item} (додаток {listNumering[count_dodat]});");


                tmpRun.FontSize = 14;
                count_dodat++;
            }

            
            
            //remove
            for (int i = 0; i < listSRod.Count; i++)
            {
                int pPos = doc1.GetPosOfParagraph(doc1.Paragraphs[doc1.Paragraphs.Count - 1]);
                doc1.RemoveBodyElement(pPos);
            }
            
            //System.Windows.MessageBox.Show(doc1.Paragraphs[23].Text);
            // в кінець 2 ліст пустий
            for (int i = 0; i < listSDav.Count; i++)
            {
                var tmpParagraph = doc1.CreateParagraph();
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                tmpParagraph.IndentationFirstLine = 570;
                var tmpRun = tmpParagraph.CreateRun();
                tmpRun.FontFamily = "Times New Roman";
                tmpRun.FontSize = 14;
            }

            
           
            // пересунуть 2 ліст
            for (int i = doc1.Paragraphs.Count - listSDav.Count - 1; i >= 22 + listSRod.Count; i--)
            {
                var tmpParagraph = doc1.Paragraphs[i];
                doc1.SetParagraph(tmpParagraph, i + listSDav.Count);
            }

            
            //System.Windows.MessageBox.Show(doc1.Paragraphs[23].Text);
            // встувить 2 ліст
            for (int i = 0; i < listSDav.Count; i++)
            {
                var tmpParagraph = doc1.CreateParagraph();
                doc1.SetParagraph(tmpParagraph, 22 + listSRod.Count + i);
                tmpParagraph.IndentationFirstLine = 570;
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                tmpParagraph.SpacingAfter = 1;
                var tmpRun = tmpParagraph.CreateRun();
                tmpRun.FontFamily = "Times New Roman";


                if (i == listSDav.Count - 1)
                    tmpRun.AppendText($"- {listSDav[i]}.");
                else
                    tmpRun.AppendText($"- {listSDav[i]};");



                tmpRun.FontSize = 14;
            }

            
            //System.Windows.MessageBox.Show(doc1.Paragraphs[23].Text);
            //remove
            for (int i = 0; i < listSDav.Count; i++)
            {
                int pPos = doc1.GetPosOfParagraph(doc1.Paragraphs[doc1.Paragraphs.Count - 1]);
                doc1.RemoveBodyElement(pPos);
            }

            
            

            //
            int indexDel = 0;
            if (count_Shemat == 0)
            {
                indexDel = 22 + listSRod.Count + listSDav.Count;
                for (int i = 21 + listSRod.Count; i < doc1.Paragraphs.Count; i++)
                {
                    var tmpParagraph = doc1.Paragraphs[i];
                    doc1.SetParagraph(tmpParagraph, i - 1);
                }
                doc1.SetParagraph(doc1.Paragraphs[0], doc1.Paragraphs.Count - 1);
            }
            else
            {
                indexDel = 23 + listSRod.Count + listSDav.Count;
            }

            if (!isExReq)
            {
                for (int i = indexDel; i < doc1.Paragraphs.Count; i++)
                {
                    var tmpParagraph = doc1.Paragraphs[i];
                    doc1.SetParagraph(tmpParagraph, i - 1);
                }
                doc1.SetParagraph(doc1.Paragraphs[0], doc1.Paragraphs.Count - 1);
            }

            if(listSRod.Count == 0)
            {
                for (int i = 20; i < doc1.Paragraphs.Count; i++)
                {
                    var tmpParagraph = doc1.Paragraphs[i];
                    doc1.SetParagraph(tmpParagraph, i - 1);
                }
                doc1.SetParagraph(doc1.Paragraphs[0], doc1.Paragraphs.Count - 1);
            }
            var path4 = prevM.Folder + "\\Відповідь\\Відповідь 2.docx";
           
            //Збереження звіта 
            using (FileStream sw = File.Create(path4))
            {
                doc1.Write(sw);
                // doc1.Close();
            }

            doc1.Close();
            File.Delete(path4);
            System.Windows.MessageBox.Show($"Відповідь збережено в папку:\n{path3}");


        }
        public void CreateResponseOther()
        {
            string path3 = "";

            //Створення документу звіту
            XWPFDocument doc1;
            if (prevM != null && prevM.Folder != null)
            {
                Directory.CreateDirectory(prevM.Folder + "\\Відповідь");

                var exeFath = /*AppDomain.CurrentDomain.BaseDirectory*/  Environment.CurrentDirectory;
                var path = System.IO.Path.Combine(exeFath, "Files\\1.docx");

                FileInfo fileInfo = new FileInfo(path);

                var path2 = System.IO.Path.Combine(exeFath, "FilesRet\\2.docx");
                path3 = prevM.Folder + $"\\Відповідь\\Відповідь {prevM.Folder.Split('\\').Last()}.docx";

                fileInfo.CopyTo(path3, true);

                doc1 = new XWPFDocument(OPCPackage.Open(path3));
            }
            else
            {
                System.Windows.MessageBox.Show($"Не вдалося відкрити контекст запиту. Не знайдено його");
                return;
            }

            bool isExReq = false;
            if (Directory.Exists(prevM.Folder + "\\Запити"))
                if (!(Directory.GetDirectories(prevM.Folder + "\\Запити").Length == 0 &&
                                    Directory.GetFiles(prevM.Folder + "\\Запити").Length == 0)
                     )
                {
                    isExReq = true;
                }


            //Створення зміних, що вставляються в звіт   
            int indexSub = listParamInt[0];    // isCountFig > 1 ? 0 : 1;
            int whatIndex = listParamInt[1];    // typeorgansList.SelectedIndex;
            string name = listParamString[0]; // nameSubTextBox.Text;
            string address1 = listParamString[1]; // addressOrgTextBox.Text;
            string date1 = listParamString[2]; //dateRequestDatePicker.Text;
            string date2 = listParamString[3]; //dateInTextBox.Text.Substring(0, 10);
            string number1 = listParamString[4]; //numberRequestTextBox.Text;
            string number2 = listParamString[5]; //numberInTextBox.Text;
            string vidOrgan = listParamString[6]; //vidOrgTextBox.Text;
            string positionSub = listParamString[7]; //positionSubTextBox.Text;
            int count_Shemat = listParamInt[2];


            //Створення списків реєстрів родовий і давальний
            List<string> listSRod = new List<string>();
            List<string> listSDav = new List<string>();


            //Збереження даних наявностей реєстрів
            if (Directory.Exists(prevM.Folder))
            {
                int i = 1;
                foreach (string itemAbbreviatedName in Reest.abbreviatedName)
                {
                    string[] dirs = Directory.GetDirectories(prevM.Folder);
                    foreach (var itemDir in dirs)
                    {
                        if ((new DirectoryInfo(itemDir)).Name == $"{i}. " + itemAbbreviatedName)
                        {
                            if (Directory.GetDirectories(itemDir).Length > 0 ||
                                 Directory.GetFiles(itemDir).Length > 0
                            )
                            {
                                listSRod.Add(Reest.sRodov[Reest.abbreviatedName.IndexOf(itemAbbreviatedName)]);
                            }
                            else
                            {

                                listSDav.Add(Reest.sDav[Reest.abbreviatedName.IndexOf(itemAbbreviatedName)]);
                            }
                        }
                    }
                    i++;
                }
                var d = Directory.GetDirectories(prevM.Folder);
                foreach (var item in d)
                {
                    if ((new DirectoryInfo(item)).Name == $"{i}. Схеми")
                    {
                        var countD = Directory.GetDirectories(item).Length;
                        var countF = Directory.GetFiles(item).Length;
                        count_Shemat = countD + countF;
                        break;
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Не знайдено папку запиту");
                doc1.Close();
                return;
            }

            //створення множини ідентичних груп додатків
            var listNumering = Orders(listSRod);
            HashSet<int> hset = new HashSet<int>();
            foreach (var item in listNumering)
            {
                hset.Add(item);
            }



            //DocResponse docResponse = new DocResponse(prevM, new List<int>() { indexSub, whatIndex, count_Shemat }, new List<string>() {
            //    name,
            //    address1,
            //    date1,
            //    date2,
            //    number1,
            //    number2,
            //    vidOrgan,
            //    positionSub
            //    });




            //Формування параграфів
            var par = doc1.Paragraphs[22];
            par.ReplaceText(par.Text, positionSub);


            par = doc1.Paragraphs[21];
            par.ReplaceText(par.Text, vidOrgan);

            par = doc1.Paragraphs[23];
            par.ReplaceText(par.Text, name);

            par = doc1.Paragraphs[25];
            par.ReplaceText(par.Text, address1);

            par = doc1.Paragraphs[26];
            par.ReplaceText(par.Text, "");

            par = doc1.Paragraphs[30];
            //par.ReplaceText("14.12.2021 № 65/16/6133 (вх. № 6018/27-21 від 21.12.2022)", $"{date1} № {number1} (вх. № {number2} від {date2})");
            par.ReplaceText("14.12.2021", $"{date1}");
            par.ReplaceText("65/16/6133", $"{number1}");
            par.ReplaceText("6018/27-21", $"{number2}");
            par.ReplaceText("21.12.2022", $"{date2}");

            par.ReplaceText("зазначених", Reest.sub[indexSub]);
            par.ReplaceText("осіб", Reest.sub2[indexSub]);


            par = doc1.Paragraphs[31];

            if (count_Shemat > 0)
                par.ReplaceText("додаток 10-11", $"додаток {hset.Count + 1}");

            par.ReplaceText("зазначених", Reest.sub[indexSub]);
            par.ReplaceText("осіб", Reest.sub2[indexSub]);

            par = doc1.Paragraphs[32];
            par.ReplaceText("зазначених", Reest.sub[indexSub]);
            par.ReplaceText("осіб", Reest.sub2[indexSub]);


            par = doc1.Paragraphs[33];
            par.ReplaceText("зазначених", Reest.sub[indexSub]);
            par.ReplaceText("осіб", Reest.sub2[indexSub]);

            //System.Windows.MessageBox.Show(par.Text);

            par = doc1.Paragraphs[34];
            if (whatIndex != -1)
                par.ReplaceText(par.Text, Reest.organs[whatIndex]);
            else
                par.ReplaceText(par.Text, Reest.organs[0]);


            for (int i = 0; i < listSRod.Count; i++)
            {
                var tmpParagraph = doc1.CreateParagraph();
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                tmpParagraph.IndentationFirstLine = 570;
                var tmpRun = tmpParagraph.CreateRun();
                tmpRun.FontSize = 14;
            }
            // пересунуть 1 ліст
            for (int i = doc1.Paragraphs.Count - listSRod.Count - 1; i >= 31; i--)
            {
                var tmpParagraph = doc1.Paragraphs[i];
                doc1.SetParagraph(tmpParagraph, i + listSRod.Count);
            }

            // засунуть 1 ліст
            int count_dodat = 0;
            //var listNumering = Orders(listSRod);

            foreach (var item in listSRod)
            {
                var tmpParagraph = doc1.CreateParagraph();
                doc1.SetParagraph(tmpParagraph, 31 + count_dodat);
                tmpParagraph.IndentationFirstLine = 570;
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                var tmpRun = tmpParagraph.CreateRun();
                tmpRun.AppendText($"{count_dodat + 1}) ");
                if (count_dodat == listSRod.Count - 1)
                    tmpRun.AppendText($"{item} (додаток {listNumering[count_dodat]}).");
                else
                    tmpRun.AppendText($"{item} (додаток {listNumering[count_dodat]});");


                tmpRun.FontSize = 14;
                count_dodat++;
            }
            //remove
            for (int i = 0; i < listSRod.Count; i++)
            {
                int pPos = doc1.GetPosOfParagraph(doc1.Paragraphs[doc1.Paragraphs.Count - 1]);
                doc1.RemoveBodyElement(pPos);
            }

            // в кінець 2 ліст пустий
            for (int i = 0; i < listSDav.Count; i++)
            {
                var tmpParagraph = doc1.CreateParagraph();
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                tmpParagraph.IndentationFirstLine = 570;
                var tmpRun = tmpParagraph.CreateRun();
                tmpRun.FontSize = 14;
            }

            // пересунуть 2 ліст
            for (int i = doc1.Paragraphs.Count - listSDav.Count - 1; i >= 33 + listSRod.Count; i--)
            {
                var tmpParagraph = doc1.Paragraphs[i];
                doc1.SetParagraph(tmpParagraph, i + listSDav.Count);
            }



            // встувить 2 ліст
            for (int i = 0; i < listSDav.Count; i++)
            {
                var tmpParagraph = doc1.CreateParagraph();
                doc1.SetParagraph(tmpParagraph, 33 + listSRod.Count + i);
                tmpParagraph.IndentationFirstLine = 570;
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                var tmpRun = tmpParagraph.CreateRun();



                if (i == listSDav.Count - 1)
                    tmpRun.AppendText($"- {listSDav[i]}.");
                else
                    tmpRun.AppendText($"- {listSDav[i]};");



                tmpRun.FontSize = 14;
            }


            //remove
            for (int i = 0; i < listSDav.Count; i++)
            {
                int pPos = doc1.GetPosOfParagraph(doc1.Paragraphs[doc1.Paragraphs.Count - 1]);
                doc1.RemoveBodyElement(pPos);
            }


            par = doc1.Paragraphs[doc1.Paragraphs.Count - 7];
            par.ReplaceText(par.Text, $"Примірник № 1 - {vidOrgan}");




            int indexDel = 0;
            if (count_Shemat == 0)
            {
                indexDel = 33 + listSRod.Count + listSDav.Count;
                for (int i = 32 + listSRod.Count; i < doc1.Paragraphs.Count; i++)
                {
                    var tmpParagraph = doc1.Paragraphs[i];
                    doc1.SetParagraph(tmpParagraph, i - 1);
                }
                doc1.SetParagraph(doc1.Paragraphs[0], doc1.Paragraphs.Count - 1);
            }
            else
            {
                indexDel = 34 + listSRod.Count + listSDav.Count;
            }


            if (!isExReq)
            {
                for (int i = indexDel; i < doc1.Paragraphs.Count; i++)
                {
                    var tmpParagraph = doc1.Paragraphs[i];
                    doc1.SetParagraph(tmpParagraph, i - 1);
                }
                doc1.SetParagraph(doc1.Paragraphs[0], doc1.Paragraphs.Count - 1);
            }


            var path4 = prevM.Folder + "\\Відповідь\\Відповідь.docx";

            //Збереження звіта 
            using (FileStream sw = File.Create(path4))
            {
                doc1.Write(sw);
                // doc1.Close();
            }

            doc1.Close();
            File.Delete(path4);
            System.Windows.MessageBox.Show($"Відповідь збережено в папку:\n{path3}");
        }

        public void CreateResponseCombinedOther(List<int> listColor)
        {
            string path3 = "";

            //Створення документу звіту
            XWPFDocument doc1;

            if (prevM != null && prevM.Folder != null)
            {
                Directory.CreateDirectory(prevM.Folder + "\\Об'єднана відповідь");

                var exeFath = /*AppDomain.CurrentDomain.BaseDirectory*/  Environment.CurrentDirectory;
                var path = System.IO.Path.Combine(exeFath, "Files\\1.docx");

                FileInfo fileInfo = new FileInfo(path);

                var path2 = System.IO.Path.Combine(exeFath, "FilesRet\\2.docx");
                path3 = prevM.Folder + $"\\Об'єднана відповідь\\Об'єднана відповідь {prevM.Folder.Split('\\').Last()}.docx";

                fileInfo.CopyTo(path3, true);

                doc1 = new XWPFDocument(OPCPackage.Open(path3));
            }
            else
            {
                return;
            }

            bool isExReq = false;
            if (Directory.Exists(prevM.Folder + "\\Запити"))
                if (!(Directory.GetDirectories(prevM.Folder + "\\Запити").Length == 0 &&
                                    Directory.GetFiles(prevM.Folder + "\\Запити").Length == 0)
                     )
                {
                    isExReq = true;
                }
            //Створення зміних, що вставляються в звіт   
            int indexSub =  listParamInt[0];          // isCountFig > 1 ? 0 : 1;
            int whatIndex = listParamInt[1];         // typeorgansList.SelectedIndex;
            string name =   listParamString[0];        // nameSubTextBox.Text;
            string address1 = listParamString[1];    // addressOrgTextBox.Text;
            string date1 =  listParamString[2];       // dateRequestDatePicker.Text;
            string date2 = listParamString[3];       // dateInTextBox.Text.Substring(0, 10);
            string number1 = listParamString[4];     // numberRequestTextBox.Text;
            string number2 = listParamString[5];     // numberInTextBox.Text;
            string vidOrgan = listParamString[6];    // vidOrgTextBox.Text;
            string positionSub = listParamString[7]; // positionSubTextBox.Text;
            int count_Shemat = listParamInt[2];


            //Створення списків реєстрів родовий і давальний
            List<string> listSRod = new List<string>();
            List<string> listSDav = new List<string>();


            //Збереження даних наявностей реєстрів
            for (int i = 0; i < Reest.abbreviatedName.Count; i++)
            {
                if (listColor[i] == 3)
                {
                    listSRod.Add(Reest.sRodov[i]);
                }
                else if(listColor[i] == 2)
                {
                    listSDav.Add(Reest.sDav[i]);
                }
            }

            //створення множини ідентичних груп додатків
            var listNumering = Orders(listSRod);
            HashSet<int> hset = new HashSet<int>();
            foreach (var item in listNumering)
            {
                hset.Add(item);
            }

            //Формування параграфів
            var par = doc1.Paragraphs[22];
            par.ReplaceText(par.Text, positionSub);


            par = doc1.Paragraphs[21];
            par.ReplaceText(par.Text, vidOrgan);

            par = doc1.Paragraphs[23];
            par.ReplaceText(par.Text, name);

            par = doc1.Paragraphs[25];
            par.ReplaceText(par.Text, address1);

            par = doc1.Paragraphs[26];
            par.ReplaceText(par.Text, "");

            par = doc1.Paragraphs[30];
            //par.ReplaceText("14.12.2021 № 65/16/6133 (вх. № 6018/27-21 від 21.12.2022)", $"{date1} № {number1} (вх. № {number2} від {date2})");
            par.ReplaceText("14.12.2021", $"{date1}");
            par.ReplaceText("65/16/6133", $"{number1}");
            par.ReplaceText("6018/27-21", $"{number2}");
            par.ReplaceText("21.12.2022", $"{date2}");


            par.ReplaceText(par.Text, AddReqInString(par.Text, "щодо"));


            par.ReplaceText("зазначених", Reest.sub[indexSub]);
            par.ReplaceText("осіб", Reest.sub2[indexSub]);


            par = doc1.Paragraphs[31];
            if (count_Shemat > 0)
                par.ReplaceText("додаток 10-11", $"додаток {hset.Count + 1}");

            par.ReplaceText("зазначених", Reest.sub[indexSub]);
            par.ReplaceText("осіб", Reest.sub2[indexSub]);

            par = doc1.Paragraphs[32];
            par.ReplaceText("зазначених", Reest.sub[indexSub]);
            par.ReplaceText("осіб", Reest.sub2[indexSub]);


            par = doc1.Paragraphs[33];
            par.ReplaceText("зазначених", Reest.sub[indexSub]);
            par.ReplaceText("осіб", Reest.sub2[indexSub]);

            //System.Windows.MessageBox.Show(par.Text);

            par = doc1.Paragraphs[34];
            if (whatIndex != -1)
                par.ReplaceText(par.Text, Reest.organs[whatIndex]);
            else
                par.ReplaceText(par.Text, Reest.organs[0]);


            for (int i = 0; i < listSRod.Count; i++)
            {
                var tmpParagraph = doc1.CreateParagraph();
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                tmpParagraph.IndentationFirstLine = 570;
                var tmpRun = tmpParagraph.CreateRun();
                tmpRun.FontSize = 14;
            }
            // пересунуть 1 ліст
            for (int i = doc1.Paragraphs.Count - listSRod.Count - 1; i >= 31; i--)
            {
                var tmpParagraph = doc1.Paragraphs[i];
                doc1.SetParagraph(tmpParagraph, i + listSRod.Count);
            }

            // засунуть 1 ліст
            int count_dodat = 0;
            //var listNumering = Orders(listSRod);

            foreach (var item in listSRod)
            {
                var tmpParagraph = doc1.CreateParagraph();
                doc1.SetParagraph(tmpParagraph, 31 + count_dodat);
                tmpParagraph.IndentationFirstLine = 570;
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                var tmpRun = tmpParagraph.CreateRun();
                tmpRun.AppendText($"{count_dodat + 1}) ");
                if (count_dodat == listSRod.Count - 1)
                    tmpRun.AppendText($"{item} (додаток {listNumering[count_dodat]}).");
                else
                    tmpRun.AppendText($"{item} (додаток {listNumering[count_dodat]});");


                tmpRun.FontSize = 14;
                count_dodat++;
            }
            //remove
            for (int i = 0; i < listSRod.Count; i++)
            {
                int pPos = doc1.GetPosOfParagraph(doc1.Paragraphs[doc1.Paragraphs.Count - 1]);
                doc1.RemoveBodyElement(pPos);
            }

            // в кінець 2 ліст пустий
            for (int i = 0; i < listSDav.Count; i++)
            {
                var tmpParagraph = doc1.CreateParagraph();
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                tmpParagraph.IndentationFirstLine = 570;
                var tmpRun = tmpParagraph.CreateRun();
                tmpRun.FontSize = 14;
            }

            // пересунуть 2 ліст
            for (int i = doc1.Paragraphs.Count - listSDav.Count - 1; i >= 33 + listSRod.Count; i--)
            {
                var tmpParagraph = doc1.Paragraphs[i];
                doc1.SetParagraph(tmpParagraph, i + listSDav.Count);
            }
            // встувить 2 ліст
            for (int i = 0; i < listSDav.Count; i++)
            {
                var tmpParagraph = doc1.CreateParagraph();
                doc1.SetParagraph(tmpParagraph, 33 + listSRod.Count + i);
                tmpParagraph.IndentationFirstLine = 570;
                tmpParagraph.Alignment = ParagraphAlignment.BOTH;
                var tmpRun = tmpParagraph.CreateRun();



                if (i == listSDav.Count - 1)
                    tmpRun.AppendText($"- {listSDav[i]}.");
                else
                    tmpRun.AppendText($"- {listSDav[i]};");



                tmpRun.FontSize = 14;
            }


            //remove
            for (int i = 0; i < listSDav.Count; i++)
            {
                int pPos = doc1.GetPosOfParagraph(doc1.Paragraphs[doc1.Paragraphs.Count - 1]);
                doc1.RemoveBodyElement(pPos);
            }


            par = doc1.Paragraphs[doc1.Paragraphs.Count - 7];
            par.ReplaceText(par.Text, $"Примірник № 1 - {vidOrgan}");




            int indexDel = 0;
            if (count_Shemat == 0)
            {
                indexDel = 33 + listSRod.Count + listSDav.Count;
                for (int i = 32 + listSRod.Count; i < doc1.Paragraphs.Count; i++)
                {
                    var tmpParagraph = doc1.Paragraphs[i];
                    doc1.SetParagraph(tmpParagraph, i - 1);
                }
                doc1.SetParagraph(doc1.Paragraphs[0], doc1.Paragraphs.Count - 1);
            }
            else
            {
                indexDel = 34 + listSRod.Count + listSDav.Count;
            }


            if (!isExReq)
            {
                for (int i = indexDel; i < doc1.Paragraphs.Count; i++)
                {
                    var tmpParagraph = doc1.Paragraphs[i];
                    doc1.SetParagraph(tmpParagraph, i - 1);
                }
                doc1.SetParagraph(doc1.Paragraphs[0], doc1.Paragraphs.Count - 1);
            }


            var path4 = prevM.Folder + "\\Об'єднана відповідь\\Відповідь.docx";

            //Збереження звіта 
            using (FileStream sw = File.Create(path4))
            {
                doc1.Write(sw);
                // doc1.Close();
            }

            doc1.Close();
            File.Delete(path4);

            for (int i = 1; i < listPrevM.Count; i++)
            {
                if (Directory.Exists(listPrevM[i].Folder))
                {
                    Directory.CreateDirectory(listPrevM[i].Folder + "\\Об'єднана відповідь");
                    new FileInfo(path3).CopyTo(listPrevM[i].Folder + $"\\Об'єднана відповідь\\{path3.Split('\\').Last()}", true);
                }
            }


            //System.Windows.MessageBox.Show($"Відповідь збережено в папку:\n{path3}");

        }
        private string GetSomeNumbInp()
        {
            if (listPrevM == null || listPrevM.Count == 0) return "";
            string retStr = listPrevM.FirstOrDefault()!.NumbInput.Replace('/', '-');
            foreach (var item in listPrevM)
            {
                retStr += ", " + item.NumbInput.Replace('/', '-');
            }
            return retStr;
        }
        private List<int> Orders(List<string> listSRod)
        {
            int index = 1;

            List<int> listGr = new List<int>();
            List<int> listReturnNumb = new List<int>();

            foreach (var item in listSRod)
            {
                // індекс назви реєстра в робовому 
                var indexReest = Reest.sRodov.IndexOf(item);

                // listGr додається групу цього реєстру
                listGr.Add(Reest.sGrupa[indexReest]);
            }

            foreach (var item in listSRod)
            {
                if (isHaveBefore(listSRod.IndexOf(item), listGr))
                {
                    listReturnNumb.Add(listGr[listSRod.IndexOf(item)]);
                }
                else
                {
                    listReturnNumb.Add(index++);
                }
            }

            return listReturnNumb;
        }
        private bool isHaveBefore(int indexRee, List<int> listGr)
        {
            for (int i = 0; i < indexRee; i++)
            {
                if (listGr[i] == listGr[indexRee])
                {
                    return true;
                }
            }
            return false;
        }
        private string AddReqInString(string str, string pred)
        {
            //14.12.2021 № 65/16/6133 (вх. № 6018/27-21 від 21.12.2022)
            for (int i = 1; i < listPrevM.Count; i++)
            {
                var main = (from m in modelContext.Mains where m.NumbInput == listPrevM[i].NumbInput select m).First();
                str = str.Insert(str.IndexOf(pred) - 1, $", від {GetStringFromDateTime(main.DtOut)} № {main.NumbOutInit} (вх. № {main.NumbInput} від {GetStringFromDateTime(main.DtInput)})");
            }
            return str;
        }
        private string GetStringFromDateTime(DateTime? dateTime)
        {
            if(dateTime == null)
            {
                return "";
            }
            else
            {
                var strD = dateTime.ToString();
                if(strD == null)
                {
                    return "";
                }
                else
                {
                    return strD.Substring(0, 10);
                }
            }
        }
        public void ToDiskCombined(List<int> listColor, int numColorShema)
        {
            if (Directory.Exists(prevM.Folder))
            {
                var listR = new List<string>();
                var listInd = new List<int>();
                for (int i = 0; i < listColor.Count; i++)
                {
                    if (listColor[i] == 3)
                    {
                        listR.Add(Reest.sRodov[i]);
                        listInd.Add(i);
                    }
                }

                var listNumering = Orders(listR);

                //перестворити папку на диск
                if (Directory.Exists(prevM.Folder + $"\\Об'єднана відповідь\\На диск"))
                {
                    Directory.Delete(prevM.Folder + $"\\Об'єднана відповідь\\На диск", true);
                }
                Directory.CreateDirectory(prevM.Folder + $"\\Об'єднана відповідь\\На диск");

                

                for (int i = 0; i < listNumering.Count; i++)
                {
                    Directory.CreateDirectory(prevM.Folder + $"\\Об'єднана відповідь\\На диск\\Додаток {listNumering[i]}");
                    for (int j = 0; j < listPrevM.Count; j++)
                    {
                        if(Directory.Exists(listPrevM[j].Folder + $"\\{listInd[i] + 1}. " + Reest.abbreviatedName[listInd[i]]))
                            perebor_updates(listPrevM[j].Folder + $"\\{listInd[i] + 1}. " + Reest.abbreviatedName[listInd[i]], prevM.Folder + $"\\Об'єднана відповідь\\На диск\\Додаток {listNumering[i]}");
                    }
                }

                if (numColorShema == 3)
                {
                    Directory.CreateDirectory(prevM.Folder + $"\\Об'єднана відповідь\\На диск\\Додаток {listNumering.LastOrDefault() + 1}");
                    for (int j = 0; j < listPrevM.Count; j++)
                    {
                        if (Directory.Exists(listPrevM[j].Folder + $"\\{Reest.abbreviatedName.Count + 1}. Схеми"))
                             perebor_updates(listPrevM[j].Folder + $"\\{Reest.abbreviatedName.Count + 1}. Схеми", prevM.Folder + $"\\Об'єднана відповідь\\На диск\\Додаток {listNumering.LastOrDefault() + 1}");
                    }
                }

                for (int i = 1; i < listPrevM.Count; i++)
                {
                    perebor_updates(prevM.Folder + $"\\Об'єднана відповідь\\На диск", listPrevM[i].Folder + $"\\Об'єднана відповідь\\На диск");
                }
            }
        }
        void perebor_updates(string begin_dir, string end_dir)
        {
            //Берём нашу исходную папку
            DirectoryInfo dir_inf = new DirectoryInfo(begin_dir);
            //Перебираем все внутренние папки
            foreach (DirectoryInfo dir in dir_inf.GetDirectories())
            {
                //Проверяем - если директории не существует, то создаем;
                if (Directory.Exists(end_dir + "\\" + dir.Name) != true)
                {
                    Directory.CreateDirectory(end_dir + "\\" + dir.Name);
                }

                //Рекурсия (перебираем вложенные папки и делаем для них то-же самое).
                perebor_updates(dir.FullName, end_dir + "\\" + dir.Name);
            }

            //Перебираем файлы в папке источнике.
            foreach (string file in Directory.GetFiles(begin_dir))
            {
                //Определяем (отделяем) имя файла с расширением - без пути (но с слешем "").
                string filik = file.Substring(file.LastIndexOf('\\'), file.Length - file.LastIndexOf('\\'));
                //Копируем файл с перезаписью из источника в приёмник.
                File.Copy(file, end_dir + "\\" + filik, true);
            }
        }
    }
}
