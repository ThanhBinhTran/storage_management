using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace storage_managements
{
    class Lib_Pdf
    {
        private static readonly BaseFont baseFont = BaseFont.CreateFont("c:/windows/fonts/Arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        private static readonly Font font = new Font(baseFont, 12);

        public static void CreatePdf(string filePath, List<DS_TransactionGrid> items, int separateBy = 0)
        {
            DS_Configuration pdfConfig = Lib_Json.ReadProgramConfiguration();
            if (pdfConfig is null)
            {
                pdfConfig = new DS_Configuration() {
                    page_top = 25,
                    page_bottom = 25,
                    page_left = 20,
                    page_right = 20,
                    pdfTableWidths = Program_Parameters.pdfTableWidths,
                };
            }
            // Create a document object landscape page
            // Set the page size to A4
            var document = new Document(PageSize.A4.Rotate(), pdfConfig.page_left, pdfConfig.page_right,
                pdfConfig.page_top, pdfConfig.page_bottom);
            // set document landscape page

            // Create a writer that listens to the document
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            // Open the document for writing
            document.Open();
            // Add a new page to the document
            document.NewPage();

            string docTitle = string.Format("In ngày: {0}", Lib_DateTime.GetDateTime(DateTime.Now));
            DocumentAddParagraph(doc: document, str: docTitle);
            // Add the table to the document
            DocumentAddTables(doc: document, transItems: items, separateBy: separateBy, pdfPage: pdfConfig);

            // Close the document
            document.Close();
        }
        private static PdfPTable CreatePdfTableHeader(float [] tableWidth)
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
            table.SetWidths(tableWidth);
            // headers field
            foreach (string itemStr in Program_Parameters.pdfHeader)
            {
                table.AddCell(new Paragraph(itemStr, font));
            }
            return table;
        }

        private static PdfPTable CreatePdfTableRow(DS_TransactionGrid item, float[] tableWidth)
        {
            PdfPTable table = new PdfPTable(7)
            {
                WidthPercentage = 100
            };
            // Set the relative widths of the table
            table.SetWidths(tableWidth);
            string time = Lib_DateTime.GetDateTime(item.transaction_time);
            string direction = item.transaction_direction;
            string taxID = item.taxID;
            if (Lib_FormText.IsStringEmpty(taxID))
            {
                taxID = "Không có";
            }
            string company_name = item.company_name;
            string item_name = item.item_name;
            string item_quantity = item.item_quantity.ToString();
            string item_unit = item.item_unit;
            //"Thời gian", "Mã Hóa đơn", "công ty/khách", "Giao dịch", "Sản Phẩm", "Số Lượng", "Quy cách"
            table.AddCell(new Paragraph(time, font));
            table.AddCell(new Paragraph(item.taxID, font));
            table.AddCell(new Paragraph(company_name, font));
            table.AddCell(new Paragraph(direction, font));
            table.AddCell(new Paragraph(item_name, font));
            PdfPCell quantity = new PdfPCell(new Phrase(item_quantity));
            quantity.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(quantity);
            table.AddCell(new Paragraph(item_unit, font));
            return table;
        }

        private static string IsSeparated(string company, string item_name, DateTime transDate, DS_TransactionGrid item, int separateBy = 0)
        {
            string result = "";
            if (separateBy == 0 && transDate.Date != item.transaction_time.Date) // date
            {
                result = string.Format("Ngày: {0}", Lib_DateTime.GetDateOnly(item.transaction_time.Date));
            }
            else if (separateBy == 1 && company != item.company_name) // date
            {
                result = string.Format("Công ty/Khách hàng: {0}", item.company_name);
            }
            else if (separateBy == 2 && item_name != item.item_name) // item name
            {
                result = string.Format("Sản phẩm: {0}", item.item_name);
            }
            result += "\n\n";
            return result;
        }
        private static void DocumentAddTables(Document doc,
            List<DS_TransactionGrid> transItems, DS_Configuration pdfPage, int separateBy = 0)
        {
            // separate by 0 (date), 1 (company), 2 (món hàng)
            string company_name = "";
            DateTime transactionDate = new DateTime();
            string item_name = "";

            foreach (DS_TransactionGrid item in transItems)
            {
                string textSeparate = IsSeparated(company: company_name, item_name: item_name,
                    transDate: transactionDate, item: item, separateBy: separateBy);
                if (!Lib_FormText.IsStringEmpty(textSeparate))
                {
                    company_name = item.company_name;
                    item_name = item.item_name;
                    transactionDate = item.transaction_time;
                    DocumentAddParagraph(doc: doc, str: textSeparate);
                    PdfPTable tableHeader = CreatePdfTableHeader(tableWidth:pdfPage.pdfTableWidths);
                    doc.Add(tableHeader);
                }
                PdfPTable tableRow = CreatePdfTableRow(item: item, tableWidth: pdfPage.pdfTableWidths);
                doc.Add(tableRow);
            }
        }
        private static void DocumentAddParagraph(Document doc, string str, int alignment = Element.ALIGN_LEFT)
        {
            Paragraph paragraphText = new Paragraph(str, font)
            {
                Alignment = alignment
            };
            doc.Add(paragraphText);
        }
    }
}
