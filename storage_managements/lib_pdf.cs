using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace storage_managements
{
    class lib_pdf
    {
        private static BaseFont baseFont = BaseFont.CreateFont("c:/windows/fonts/Arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        private static Font font = new Font(baseFont, 12);
        public static void CreatePdf(string filePath, List<DS_Transaction_Grid> items, int seperateby = 0)
        {
            // Create a document object
            var document = new Document(PageSize.A4, 30, 30, 25, 25);

            // Create a writer that listens to the document
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            // Open the document for writing
            document.Open();
            // Add a new page to the document
            document.NewPage();

            string docTitle = string.Format("In ngày: {0}", lib_DateTime.GetDateTime(DateTime.Now));
            DocumentAddParagraph(doc: document, str: docTitle);
            // Add the table to the document
            DocumentAddTables(doc: document, transItems: items, seperateby: seperateby);

            // Close the document
            document.Close();
        }
        private static PdfPTable CreatePdfTableHeader()
        {
            int headCount = Program_Parameters.pdfHeader.Count;
            if (headCount <= 0)
            {
                return null;
            }
            PdfPTable table = new PdfPTable(headCount);
            table.WidthPercentage = 100;
            // Set the relative widths of the table
            table.SetWidths(Program_Parameters.pdfTableWidths);
            // headers field
            foreach (string itemstr in Program_Parameters.pdfHeader)
            {
                table.AddCell(new Paragraph(itemstr, font));
            }
            return table;
        }

        private static PdfPTable CreatePdfTableRow(DS_Transaction_Grid item)
        {
            PdfPTable table = new PdfPTable(6);

            table.WidthPercentage = 100;
            // Set the relative widths of the table
            table.SetWidths(Program_Parameters.pdfTableWidths);

            string time = lib_DateTime.GetDateTime(item.transaction_time);
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
            Paragraph quantity_para = new Paragraph(item_quantity, font);
            quantity_para.Alignment = Element.ALIGN_RIGHT; // Set the alignment to right
            table.AddCell(quantity_para);
            table.AddCell(new Paragraph(item_unit, font));
            return table;
        }

        private static string IsSeperated(string company, string item_name, DateTime transDate, DS_Transaction_Grid item, int seperateby = 0)
        {
            string result = "";
            if (seperateby == 0 && transDate.Date != item.transaction_time.Date) // date
            {
                result = string.Format("Ngày: {0}", lib_DateTime.GetDateOnly(item.transaction_time.Date));
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
            List<DS_Transaction_Grid> transItems, int seperateby = 0)
        {
            // seperate by 0 (date), 1 (company), 2 (món hàng)
            string company_name = "";
            DateTime transactionDate = new DateTime();
            string item_name = "";

            foreach (DS_Transaction_Grid item in transItems)
            {
                string seperate_text = IsSeperated(company: company_name, item_name: item_name,
                    transDate: transactionDate, item: item, seperateby: seperateby);
                if (!lib_form_text.is_string_empty(seperate_text))
                {
                    company_name = item.company_name;
                    item_name = item.item_name;
                    transactionDate = item.transaction_time;
                    DocumentAddParagraph(doc: doc, str: seperate_text);
                    PdfPTable headtable = CreatePdfTableHeader();
                    doc.Add(headtable);
                }
                PdfPTable rowtable = CreatePdfTableRow(item: item);
                doc.Add(rowtable);
            }
        }
        private static void DocumentAddParagraph(Document doc, string str, int alignment = Element.ALIGN_LEFT)
        {
            Paragraph ptext = new Paragraph(str, font);
            ptext.Alignment = alignment;
            doc.Add(ptext);
        }
    }
}
