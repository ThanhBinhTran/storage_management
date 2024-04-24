using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace storage_managements
{
    class Lib_Pdf
    {
        private static readonly BaseFont baseFont = BaseFont.CreateFont("c:/windows/fonts/Arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        private static readonly Font font = new Font(baseFont, 12);

        public static void CreatePdf(string filePath, List<DS_TransactionGrid> items, int seperateby = 0)
        {
            DS_Configuration pdfconfig = Lib_Json.ReadProgramConfiguration();
            if (pdfconfig is null)
            {
                pdfconfig = new DS_Configuration() {
                    page_top = 25,
                    page_bottom = 25,
                    page_left = 20,
                    page_right = 20,
                    pdfTableWidths = Program_Parameters.pdfTableWidths,
                };
            }    
            // Create a document object
            var document = new Document(PageSize.A4, pdfconfig.page_left, pdfconfig.page_right,
                pdfconfig.page_top, pdfconfig.page_bottom);

            // Create a writer that listens to the document
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            // Open the document for writing
            document.Open();
            // Add a new page to the document
            document.NewPage();

            string docTitle = string.Format("In ngày: {0}", Lib_DateTime.GetDateTime(DateTime.Now));
            DocumentAddParagraph(doc: document, str: docTitle);
            // Add the table to the document
            DocumentAddTables(doc: document, transItems: items, seperateby: seperateby, pdfpage:pdfconfig);

            // Close the document
            document.Close();
        }
        private static PdfPTable CreatePdfTableHeader(float [] tablewidth)
        {
            int headCount = Program_Parameters.pdfHeader.Count;
            if (headCount <= 0)
            {
                return null;
            }
            PdfPTable table = new PdfPTable(headCount)
            {
                WidthPercentage = 100
            };
            // Set the relative widths of the table
            table.SetWidths(tablewidth);
            // headers field
            foreach (string itemstr in Program_Parameters.pdfHeader)
            {
                table.AddCell(new Paragraph(itemstr, font));
            }
            return table;
        }

        private static PdfPTable CreatePdfTableRow(DS_TransactionGrid item, float[] tablewidth)
        {
            PdfPTable table = new PdfPTable(6)
            {
                WidthPercentage = 100
            };
            // Set the relative widths of the table
            table.SetWidths(tablewidth);

            string time = Lib_DateTime.GetDateTime(item.transaction_time);
            string direction = item.transaction_direction;
            string company_name = item.company_name;
            string item_name = item.item_name;
            string item_quantity = item.item_quantity.ToString();
            string item_unit = item.item_unit;
            //"Thời gian", "Đối tác", "Giao dịch", "Sản Phẩm", "Số Lượng", "Quy cách"
            table.AddCell(new Paragraph(time, font));

            table.AddCell(new Paragraph(company_name, font));
            table.AddCell(new Paragraph(direction, font));
            table.AddCell(new Paragraph(item_name, font));
            Paragraph quantity_para = new Paragraph(item_quantity, font)
            {
                Alignment = Element.ALIGN_RIGHT // Set the alignment to right
            };
            table.AddCell(quantity_para);
            table.AddCell(new Paragraph(item_unit, font));
            return table;
        }

        private static string IsSeperated(string company, string item_name, DateTime transDate, DS_TransactionGrid item, int seperateby = 0)
        {
            string result = "";
            if (seperateby == 0 && transDate.Date != item.transaction_time.Date) // date
            {
                result = string.Format("Ngày: {0}", Lib_DateTime.GetDateOnly(item.transaction_time.Date));
            }
            else if (seperateby == 1 && company != item.company_name) // date
            {
                result = string.Format("Đối tác: {0}", item.company_name);
            }
            else if (seperateby == 2 && item_name != item.item_name) // item name
            {
                result = string.Format("Sản phẩm: {0}", item.item_name);
            }
            result += "\n\n";
            return result;
        }
        private static void DocumentAddTables(Document doc,
            List<DS_TransactionGrid> transItems, DS_Configuration pdfpage, int seperateby = 0)
        {
            // seperate by 0 (date), 1 (company), 2 (món hàng)
            string company_name = "";
            DateTime transactionDate = new DateTime();
            string item_name = "";

            foreach (DS_TransactionGrid item in transItems)
            {
                string seperate_text = IsSeperated(company: company_name, item_name: item_name,
                    transDate: transactionDate, item: item, seperateby: seperateby);
                if (!Lib_FormText.IsStringEmpty(seperate_text))
                {
                    company_name = item.company_name;
                    item_name = item.item_name;
                    transactionDate = item.transaction_time;
                    DocumentAddParagraph(doc: doc, str: seperate_text);
                    PdfPTable headtable = CreatePdfTableHeader(tablewidth:pdfpage.pdfTableWidths);
                    doc.Add(headtable);
                }
                PdfPTable rowtable = CreatePdfTableRow(item: item, tablewidth: pdfpage.pdfTableWidths);
                doc.Add(rowtable);
            }
        }
        private static void DocumentAddParagraph(Document doc, string str, int alignment = Element.ALIGN_LEFT)
        {
            Paragraph ptext = new Paragraph(str, font)
            {
                Alignment = alignment
            };
            doc.Add(ptext);
        }
    }
}
