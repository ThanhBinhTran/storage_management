using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;

namespace storage_managements
{
    class lib_pdf
    {
        private static BaseFont baseFont = BaseFont.CreateFont("c:/windows/fonts/Arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        private static Font font = new Font(baseFont, 12);
        public static void CreatePdf(string filePath, List<DS_Transaction_Grid> items)
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
            DocumentAddTables(doc: document, headerList: transHeader, transItems: items);
            DocumentAddParagraph(doc: document, str: "Ngay 12/12/2024");
            DocumentAddTables(doc: document, headerList: transHeader, transItems: items);

            // Close the document
            document.Close();
        }
        private static PdfPTable CreatePdfTableHeader(List<string> headerList)
        {
            PdfPTable table;
            int headCount = headerList.Count;
            if (headCount <= 0)
            {
                return null;
            }
            table = new PdfPTable(headCount);
            // headers field
            foreach (string itemstr in headerList)
            {
                table.AddCell(new Paragraph(itemstr, font));
            }
            return table;
        }
        private static PdfPTable CreatePdfTableData(PdfPTable table, List<DS_Transaction_Grid> transItems)
        {
            //data field
            foreach (DS_Transaction_Grid item in transItems)
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
            return table;
        }
        private static void DocumentAddTables(Document doc, List<string> headerList, List<DS_Transaction_Grid> transItems)
        {
            PdfPTable table = CreatePdfTableHeader(headerList: headerList);
            if (table != null)
            {
                CreatePdfTableData(table: table, transItems: transItems);
                doc.Add(table);
            }
        }
        private static void DocumentAddParagraph(Document doc, string str)
        {
            doc.Add(new Paragraph(str, font));
        }
    }
}
