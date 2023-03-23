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

namespace DesARMA.Registers
{
    public class PDF
    {
        

        private void CreateParHead(string item, XWPFDocument doc1)
        {
            // Створити новий параграф
            XWPFParagraph paragraph = doc1.CreateParagraph();

            // Додати текст в параграф
            XWPFRun run = paragraph.CreateRun();
            run.SetText(item);
            run.FontSize = 12;
            run.IsItalic = true;
            run.IsBold = true;
            run.FontFamily = "Times New Roman";

            // Додати додаткові властивості до параграфу, якщо потрібно
            paragraph.Alignment = ParagraphAlignment.LEFT; // вирівнювання по центру
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
            paragraph.Alignment = ParagraphAlignment.LEFT; // вирівнювання по центру
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
        public void CreateNamesFieldPDF(List<List<string?>?> list, string path = "C:\\app\\EDRSh.docx",
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
            foreach (var item in Reest.namesFieldPDF)
            {
                CreateParHead(item, doc1);
                var l = list[i];
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

            //string path3 = "C:\\app\\Відповідь\\Відповідь.docx";

            //Збереження звіта 
            using (FileStream sw = File.Create(path3))
            {
                doc1.Write(sw);
            }

            doc1.Close();
            File.Delete(path2);

        }
    }
        
}
