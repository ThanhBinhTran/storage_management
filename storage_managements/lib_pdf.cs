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
        private static void DocumentAddTables(Document doc, List<string> headerList, List<DS_Transaction_Grid> transItems, int sortby = 0)
        {
            bool first = true;
            string company_name = "";
            string item_name = "";
            PdfPTable groupTable = CreatePdfTableHeader(headerList: headerList);
            foreach (DS_Transaction_Grid item in transItems)
            {
                if (company_name != item.company_name)
                {
                    company_name = item.company_name;
                    DocumentAddParagraph(doc: doc, str: item.company_name);
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        doc.Add(groupTable);
                        groupTable.Rows.Clear();

                        groupTable = CreatePdfTableHeader(headerList: headerList);
                    }

                }
                if (groupTable != null)
                {
                    CreatePdfTableRow(table: groupTable, item: item);
                }
            }
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
