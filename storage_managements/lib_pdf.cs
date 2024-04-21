using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;
using System;

namespace storage_managements
{
    class lib_pdf
    {
        private static BaseFont baseFont = BaseFont.CreateFont("c:/windows/fonts/Arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        private static Font font = new Font(baseFont, 12);
        public static void CreatePdf(string filePath, List<DS_Transaction_Grid> items, int seperateby=0)
        {
            // Create a document object
            var document = new Document(PageSize.A4, 50, 50, 25, 25);

            // Create a writer that listens to the document
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            // Open the document for writing
            document.Open();
            // Add a new page to the document
            document.NewPage();

            // Step 4: Define a font that supports Vietnamese characters


            // Add content to the document

            // Add the table to the document
            List<string> transHeader = new List<string> { "Thời gian", "Giao dịch", "Đối tác","Sản Phẩm",
                "Số Lượng", "Quy cách"};
            DocumentAddTables(doc: document, headerList: transHeader, transItems: items, seperateby: seperateby);

            // Close the document
            document.Close();
        }
        private static PdfPTable CreatePdfTableHeader(List<string> headerList)
        {
            int headCount = headerList.Count;
            if (headCount <= 0)
            {
                return null;
            }
            PdfPTable table = new PdfPTable(headCount);
            // Define the relative widths of the columns
            // Make sure this array has the same number of elements as the number of columns in your table
            float[] widths = new float[] { 2.5f, 1f, 1f, 2f, 1f, 1f };


            // Set the relative widths of the table
            table.SetWidths(widths);
            // headers field
            foreach (string itemstr in headerList)
            {
                table.AddCell(new Paragraph(itemstr, font));
            }
            return table;
        }

        private static PdfPTable CreatePdfTableRow_new(DS_Transaction_Grid item)
        {
            PdfPTable table = new PdfPTable(6);
            // Define the relative widths of the columns
            // Make sure this array has the same number of elements as the number of columns in your table
            float[] widths = new float[] { 2.5f, 1f, 1f, 2f, 1f, 1f };


            // Set the relative widths of the table
            table.SetWidths(widths);
            string time = lib_date_time.GetTimeOnly(item.transaction_time);
            string direction = "Xuất";
            string company_name = item.company_name;
            string item_name = item.item_name;
            string item_quantity = item.item_quantity.ToString();
            string item_unit = item.item_unit;
            //"Thời gian", "Giao dịch", "Đối tác","Sản Phẩm", "Số Lượng", "Quy cách"
            table.AddCell(new Paragraph(time, font));
            table.AddCell(new Paragraph(direction, font));
            table.AddCell(new Paragraph(company_name, font));
            table.AddCell(new Paragraph(item_name, font));
            table.AddCell(new Paragraph(item_quantity, font));
            table.AddCell(new Paragraph(item_unit, font));
            return table;
        }

        private static void CreatePdfTableRow(PdfPTable table, DS_Transaction_Grid item)
        {
            string time = lib_date_time.GetTimeOnly(item.transaction_time);
            string direction = "Xuất";
            string company_name = item.company_name;
            string item_name = item.item_name;
            string item_quantity = item.item_quantity.ToString();
            string item_unit = item.item_unit;
            //"Thời gian", "Giao dịch", "Đối tác","Sản Phẩm", "Số Lượng", "Quy cách"
            table.AddCell(new Paragraph(time, font));
            table.AddCell(new Paragraph(direction, font));
            table.AddCell(new Paragraph(company_name, font));
            table.AddCell(new Paragraph(item_name, font));
            table.AddCell(new Paragraph(item_quantity, font));
            table.AddCell(new Paragraph(item_unit, font));
        }
        private static PdfPTable CreatePdfTableData(PdfPTable table, List<DS_Transaction_Grid> transItems)
        {
            //data field
            foreach (DS_Transaction_Grid item in transItems)
            {
                CreatePdfTableRow(table: table, item: item);
            }
            return table;
        }

        private static string IsSeperated(string company, string item_name, DateTime transDate, DS_Transaction_Grid item, int seperateby=0)
        {
            string result = "";
            if(seperateby == 0 && transDate.Date != item.transaction_time.Date) // date
            {
                result = string.Format("Ngày: {0}", lib_date_time.GetDateOnly(item.transaction_time.Date));
            }
            else if (seperateby == 1 && company != item.company_name) // date
            {
                result = string.Format("Đối tác: {0}", item.company_name);
            }
            else if (seperateby == 2 && item_name != item.item_name) // date
            {
                result = string.Format("Sản phẩm: {0}", item.company_name);
            }
            return result;
        }
        private static void DocumentAddTables(Document doc, List<string> headerList, 
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
                if (! lib_form_text.is_string_empty(seperate_text))
                { 
                    company_name = item.company_name;
                    item_name = item.item_name;
                    transactionDate = item.transaction_time;
                    DocumentAddParagraph(doc: doc, str: seperate_text);
                    PdfPTable headtable = CreatePdfTableHeader(headerList: headerList);
                    doc.Add(headtable);
                }
                PdfPTable rowtable = CreatePdfTableRow_new(item:item);
                doc.Add(rowtable);
            }
        }
        private static void DocumentAddParagraph(Document doc, string str)
        {
            doc.Add(new Paragraph(str, font));
        }
    }
}
