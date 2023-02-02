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
        List<int> listInt = null!;
        List<string> listString = null!;
        public DocResponse(List<MainConfig> listPrevM, List<int> listInt, List<string> listString)
        {
            this.listPrevM = listPrevM;
            this.prevM = listPrevM[0];
            this.listInt = listInt;
            this.listString = listString;
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
            if(listInt == null || listString == null)
            {
                System.Windows.MessageBox.Show($"Помилка коду: порожні списки listInt, listString");
                return;
            }
            if (listInt.Count > 8 && listString.Count > 2)
            {
                System.Windows.MessageBox.Show($"Помилка коду: малий розмір списків listInt, listString");
                return;
            }

            int indexSub = listInt[0] + 2;    // isCountFig > 1 ? 0 : 1;
            int whatIndex = listInt[1];    // typeorgansList.SelectedIndex;
            string name = listString[0]; // nameSubTextBox.Text;
            string address1 = listString[1]; // addressOrgTextBox.Text;
            string date1 = listString[2]; //dateRequestDatePicker.Text;
            string date2 = listString[3]; //dateInTextBox.Text.Substring(0, 10);
            string number1 = listString[4]; //numberRequestTextBox.Text;
            string number2 = listString[5]; //numberInTextBox.Text;
            string vidOrgan = listString[6]; //vidOrgTextBox.Text;
            string positionSub = listString[7]; //positionSubTextBox.Text;
            int count_Shemat = listInt[2];

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


            MessageBox.Show($"{Reest.sub2[indexSub]}\n\n{doc1.Paragraphs[17].Text}\n\n{doc1.Paragraphs[20].Text}\n\n{doc1.Paragraphs[21].Text}\n\n{doc1.Paragraphs[19].Text}");
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
            int indexSub = listInt[0];    // isCountFig > 1 ? 0 : 1;
            int whatIndex = listInt[1];    // typeorgansList.SelectedIndex;
            string name = listString[0]; // nameSubTextBox.Text;
            string address1 = listString[1]; // addressOrgTextBox.Text;
            string date1 = listString[2]; //dateRequestDatePicker.Text;
            string date2 = listString[3]; //dateInTextBox.Text.Substring(0, 10);
            string number1 = listString[4]; //numberRequestTextBox.Text;
            string number2 = listString[5]; //numberInTextBox.Text;
            string vidOrgan = listString[6]; //vidOrgTextBox.Text;
            string positionSub = listString[7]; //positionSubTextBox.Text;
            int count_Shemat = listInt[2];


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
        public void CreateResponseCombined(List<string> listColor)
        {
            string path3 = "";

            //Створення документу звіту
            XWPFDocument doc1;


            //Створення зміних, що вставляються в звіт   
            int indexSub =  listInt[0];          // isCountFig > 1 ? 0 : 1;
            int whatIndex = listInt[1];         // typeorgansList.SelectedIndex;
            string name =   listString[0];        // nameSubTextBox.Text;
            string address1 = listString[1];    // addressOrgTextBox.Text;
            string date1 =  listString[2];       // dateRequestDatePicker.Text;
            string date2 = listString[3];       // dateInTextBox.Text.Substring(0, 10);
            string number1 = listString[4];     // numberRequestTextBox.Text;
            string number2 = listString[5];     // numberInTextBox.Text;
            string vidOrgan = listString[6];    // vidOrgTextBox.Text;
            string positionSub = listString[7]; // positionSubTextBox.Text;
            int count_Shemat = listInt[2];



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
        
    }

}
