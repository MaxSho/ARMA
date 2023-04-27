using iTextSharp.text;
using iTextSharp.text.pdf;
using static NPOI.HSSF.Util.HSSFColor;
using System.IO;
using NPOI.OpenXml4Net.OPC;
using NPOI.XWPF.UserModel;
using System;
using NPOI.XWPF.Model;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Forms;
using DesARMA.Automation;
using SixLabors.ImageSharp.Drawing;

namespace DesARMA.Registers
{
    public class PDF
    {
        

        private void CreateParHead(string item, XWPFDocument doc1, bool isBreak = false)
        {
            // Створити новий параграф
            XWPFParagraph paragraph = doc1.CreateParagraph();
            paragraph.IsPageBreak = isBreak;
            // Додати текст в параграф
            XWPFRun run = paragraph.CreateRun();
            run.SetText(item);
            run.FontSize = 12;
            run.IsItalic = true;
            run.IsBold = true;
            run.FontFamily = "Times New Roman";

            // Додати додаткові властивості до параграфу, якщо потрібно
            paragraph.Alignment = ParagraphAlignment.LEFT; // вирівнювання 
        }
        private void CreateParInHead(string item, XWPFDocument doc1)
        {
            // Створити новий параграф
            XWPFParagraph paragraph = doc1.CreateParagraph();

            // Додати текст в параграф
            XWPFRun run = paragraph.CreateRun();
            run.SetText(item);
            run.FontSize = 12;
            run.FontFamily = "Times New Roman";

            // Додати додаткові властивості до параграфу, якщо потрібно
            paragraph.Alignment = ParagraphAlignment.LEFT; // вирівнювання по центру
        }
        private void CreateParEndHead(XWPFDocument doc1)
        {
            XWPFParagraph paragraph = doc1.CreateParagraph();

            // Додати текст в параграф
            XWPFRun run = paragraph.CreateRun();
            run.SetText("Єдиний державний реєстр юридичних осіб, фізичних осіб-підприємців та громадських формувань знаходиться у стані формування. Інформація про юридичних осіб, фізичних осіб-підприємців та громадських формувань та зареєстрованих до 01.07.2004 та не включених до Єдиного державного реєстру юридичних осіб, фізичних осібпідприємців та громадських формувань отримується в органі виконавчої влади, в якому проводилась державна реєстрація.");
            run.FontSize = 10;
            run.FontFamily = "Times New Roman";

            // Додати додаткові властивості до параграфу, якщо потрібно
            paragraph.Alignment = ParagraphAlignment.BOTH; // вирівнювання по центру
            paragraph.IndentationFirstLine = 900;
        }
        private void CreateParEnter(XWPFDocument doc1)
        {
            // Створити новий параграф
            XWPFParagraph paragraph = doc1.CreateParagraph();

            // Додати текст в параграф
            XWPFRun run = paragraph.CreateRun();
            run.SetText("");
            run.FontSize = 12;
            run.FontFamily = "Times New Roman";

            // Додати додаткові властивості до параграфу, якщо потрібно
            paragraph.Alignment = ParagraphAlignment.BOTH; // вирівнювання по центру
        }
        private void Shapka(XWPFDocument doc1)
        {
            CreateParEnter(doc1);
            CreateParEnter(doc1);
            CreateParEnter(doc1);

            {
                // Створити новий параграф
                XWPFParagraph paragraph = doc1.CreateParagraph();

                // Додати текст в параграф
                XWPFRun run = paragraph.CreateRun();
                run.SetText("ВИТЯГ");
                run.FontSize = 12;
                run.FontFamily = "Times New Roman";

                // Додати додаткові властивості до параграфу, якщо потрібно
                paragraph.Alignment = ParagraphAlignment.CENTER; // вирівнювання
            }
            {
                // Створити новий параграф
                XWPFParagraph paragraph = doc1.CreateParagraph();

                // Додати текст в параграф
                XWPFRun run = paragraph.CreateRun();
                run.SetText("з Єдиного державного реєстру юридичних осіб, фізичних осіб-підприємців та громадських формувань");
                run.FontSize = 12;
                run.FontFamily = "Times New Roman";

                // Додати додаткові властивості до параграфу, якщо потрібно
                paragraph.Alignment = ParagraphAlignment.CENTER; // вирівнювання
            }
            {
                // Створити новий параграф
                XWPFParagraph paragraph = doc1.CreateParagraph();

                // Додати текст в параграф
                XWPFRun run = paragraph.CreateRun();
                run.SetText("Станом на 09.03.2023 16:39:21 відповідно до наступних \tкритеріїв пошуку: Код ЄДРПОУ засновника (учасника) юридичної особи: 39759997 надається інформація з Єдиного державного реєстру юридичних осіб, фізичних осіб-підприємців та громадських формувань (ЄДР) у кількості 4 записів:");
                run.FontSize = 12;
                run.FontFamily = "Times New Roman";

                // Додати додаткові властивості до параграфу, якщо потрібно
                paragraph.Alignment = ParagraphAlignment.CENTER; // вирівнювання
            }

            CreateParEnter(doc1);

        }
        public void CreateNamesField(List<List<string?>?> list, string path = "C:\\app\\EDRSh.docx", 
            string path2 = "C:\\app\\Відповідь\\Відповідь 1.docx",
            string path3 = "C:\\app\\Відповідь\\Відповідь.docx"
            )
        {
            XWPFDocument doc1;
            //string path = "C:\\app\\EDRSh.docx";
            //string path2 = $"C:\\app\\Відповідь\\Відповідь {1}.docx";
            FileInfo fileInfo = new FileInfo(path);
            fileInfo.CopyTo(path2, true);

            

            doc1 = new XWPFDocument(new FileStream(path2, FileMode.Open));
            //doc1 = new XWPFDocument(OPCPackage.Open(path2));

            int i = 0;
            foreach (var item in Reest.namesField)
            {
                CreateParHead(item, doc1);
                var l = list[i];
                if (l != null)
                {
                    if(l.Count > 0)
                    {
                        foreach (var itemIn in l)
                        {
                            CreateParInHead(itemIn ?? "Відомості відсутні", doc1);
                        }
                    }
                    else
                    {
                        CreateParInHead("Відомості відсутні", doc1);
                    }
                    
                }
                else
                {
                    CreateParInHead("Відомості відсутні", doc1);
                }
                
                CreateParEnter(doc1);

                i++;
            }

            //string path3 = "C:\\app\\Відповідь\\Відповідь.docx";

            //Збереження звіта 
            using (FileStream sw = File.Create(path3))
            {
                doc1.Write(sw);
            }

            doc1.Close();
            File.Delete(path2);

        }

        /// <summary>
        /// it's summary
        /// </summary>
        /// <param name="listAll"></param>
        /// <param name="arrInfo">
        /// 0 - isUO
        /// 1 - code
        /// 2 - SearchType
        /// </param>
        /// <param name="path"></param>
        /// <param name="path2"></param>
        /// <param name="path3"></param>
        public void CreateNamesFieldPDF(List<List<List<string?>?>> listAll, object[] arrInfo,
             List<bool> isFisList,
                string path = "C:\\app\\EDRSh.docx",
                string path2 = "C:\\app\\Відповідь\\Відповідь 1.docx",
                string path3 = "C:\\app\\Відповідь\\Відповідь.docx"
               )
        {
            XWPFDocument doc1;
            //string path = "C:\\app\\EDRSh.docx";
            //string path2 = $"C:\\app\\Відповідь\\Відповідь {1}.docx";
            FileInfo fileInfo = new FileInfo(path);
            fileInfo.CopyTo(path2, true);



            
            

            string[] arrTypeSearch = new string[] { "засновника(-ів)", "бенефіціара(-ів)", "керівника(-ів)",  "представника(-ів)"};
            string[] arr = new string[] { "юридичної особи", "фізичної особи" };
            var isUO = (bool)arrInfo[0];
            var code = (string)arrInfo[1];
            var typeSearch = (SearchType)arrInfo[2];
            var countRecord = listAll.Count;
            string SType = "";
            switch (typeSearch)
            {
                case SearchType.Beneficiar:
                    SType = arrTypeSearch[1];
                    break;
                case SearchType.Founder:
                    SType = arrTypeSearch[0];
                    break;
                case SearchType.Chief:
                    SType = arrTypeSearch[2];
                    break;
                case SearchType.Assignee:
                    SType = arrTypeSearch[3];
                    break;
                default:
                    break;
            }
            
            doc1 = new XWPFDocument(new FileStream(path2, FileMode.Open));
            var par = doc1.Paragraphs[6];

            //MessageBox.Show($"{par.Text}");
            par.ReplaceText("09.03.2023 16:39:21", DateTime.Now.ToString());
            par.ReplaceText("ЄДРПОУ", isUO ? "ЄДРПОУ" : "РНОКПП");
            par.ReplaceText("39759997", code);
            par.ReplaceText("4 записів", $"{AddRecordWordZAPYS(countRecord)}");
            par.ReplaceText("юридичної особи", isUO ? arr[0] : arr[1]);
            par.ReplaceText("засновника (учасника)", SType);


            int curZap = 1;
            foreach (var itemList in listAll)
            {
                int i = 0;
                
                CreateParHead($"Запис {curZap++}", doc1, curZap != 2);

                
                var sear = isFisList[curZap - 2] ? Reest.namesFieldPDFFiz : Reest.namesFieldPDF;

                foreach (var item in sear)
                {
                    CreateParHead(item, doc1);
                    var l = itemList[i];
                    if (l != null)
                    {
                        if (l.Count > 0)
                        {
                            foreach (var itemIn in l)
                            {
                                CreateParInHead(itemIn ?? "Відомості відсутні", doc1);
                            }
                        }
                        else
                        {
                            CreateParInHead("Відомості відсутні", doc1);
                        }

                    }
                    else
                    {
                        CreateParInHead("Відомості відсутні", doc1);
                    }

                    CreateParEnter(doc1);

                    i++;
                }
            }

            CreateParEndHead(doc1);
            //string path3 = "C:\\app\\Відповідь\\Відповідь.docx";

            //Збереження звіта 
            using (FileStream sw = File.Create(path3))
            {
                doc1.Write(sw);
            }

            doc1.Close();
            File.Delete(path2);

        }
        public static string AddRecordWordZAPYS(int number)
        {
            string word = "запис";
            if (number % 100 >= 11 && number % 100 <= 14)
            {
                // Відмінок множини за винятком 11-14
                return $"{number} {word}ів";
            }
            else
            {
                // Визначаємо відмінок за останньою цифрою числа
                switch (number % 10)
                {
                    case 1:
                        // Відмінок однини
                        return $"{number} {word}";
                    case 2:
                    case 3:
                    case 4:
                        // Відмінок множини за винятком 2-4
                        return $"{number} {word}и";
                    default:
                        // Відмінок множини за винятком 1 та 11-14
                        return $"{number} {word}ів";
                }
            }
        }
    }
        
}
