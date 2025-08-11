using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace storage_managements
{
    /// <summary>
    /// Provides PDF generation utilities for transaction data.
    /// </summary>
    class Lib_Pdf
    {
        // Base font for PDF content
        private static readonly BaseFont baseFont = BaseFont.CreateFont("c:/windows/fonts/Arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        // Default font style
        private static readonly Font font = new Font(baseFont, 12);
        // List of tax IDs used for coloring table cells
        private static List<DS_Company> pdfTaxIDs;

        /// <summary>
        /// Creates a PDF file with transaction data.
        /// </summary>
        /// <param name="filePath">Output PDF file path</param>
        /// <param name="items">Transaction items to display</param>
        /// <param name="taxIDs">Tax IDs for coloring</param>
        /// <param name="separateBy">Grouping mode (0: date, 1: company, 2: item, 3: taxID)</param>
        public static void CreateTransactionPdf(string filePath, List<DS_TransactionGrid> items, List<DS_Company> taxIDs, int separateBy = 0)
        {
            // Read PDF configuration or use defaults
            DS_Configuration pdfConfig = Lib_Json.ReadProgramConfiguration();
            pdfTaxIDs = taxIDs != null ? new List<DS_Company>(taxIDs) : null;
            if (pdfConfig is null)
            {
                pdfConfig = new DS_Configuration()
                {
                    page_top = 25,
                    page_bottom = 25,
                    page_left = 20,
                    page_right = 20,
                    pdfTableWidths = Program_Parameters.pdfTransactionTableWidths,
                };
            }

            // Create a landscape A4 document
            var document = new Document(PageSize.A4.Rotate(), pdfConfig.page_left, pdfConfig.page_right,
                pdfConfig.page_top, pdfConfig.page_bottom);

            // Create a writer for the document
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            // Open and start the document
            document.Open();
            document.NewPage();

            // Add print date as title
            string docTitle = string.Format("In ngày: {0}", Lib_DateTime.GetDateTime(DateTime.Now));
            DocumentAddParagraph(doc: document, str: docTitle);

            // Add transaction tables
            DocumentAddTransactionTables(doc: document, transItems: items, separateBy: separateBy, pdfPage: pdfConfig);

            // Close the document
            document.Close();
        }

        public static void CreateStoragePdf(string filePath, List<DS_StorageItem> items)
        {
            // Read PDF configuration or use defaults
            DS_Configuration pdfConfig = Lib_Json.ReadProgramConfiguration();
            if (pdfConfig is null)
            {
                pdfConfig = new DS_Configuration()
                {
                    page_top = 25,
                    page_bottom = 25,
                    page_left = 20,
                    page_right = 20,
                    pdfTableWidths = Program_Parameters.pdfStorageTableWidths,
                };
            }

            // Create a landscape A4 document
            var document = new Document(PageSize.A4, pdfConfig.page_left, pdfConfig.page_right,
                pdfConfig.page_top, pdfConfig.page_bottom);

            // Create a writer for the document
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            // Open and start the document
            document.Open();
            document.NewPage();

            // Add print date as title
            string docTitle = string.Format("In ngày: {0}\n\n", Lib_DateTime.GetDateTime(DateTime.Now));
            DocumentAddParagraph(doc: document, str: docTitle);

            // Add transaction tables
            DocumentAddStorageTables(doc: document, storageItems: items, pdfPage: pdfConfig);

            // Close the document
            document.Close();
        }

        /// <summary>
        /// Creates the header row for the PDF table.
        /// </summary>
        private static PdfPTable CreatePdfTableHeader(float[] tableWidth, List<string> pdfHeader)
        {
            int headCount = pdfHeader.Count;
            if (headCount <= 0)
            {
                return null;
            }
            PdfPTable table = new PdfPTable(headCount)
            {
                WidthPercentage = 100
            };
            table.SetWidths(tableWidth);

            // Add header cells
            foreach (string itemStr in pdfHeader)
            {
                AddCellToTable(table, itemStr);
            }
            return table;
        }

        /// <summary>
        /// Creates a table cell with optional background color and alignment.
        /// </summary>
        private static PdfPCell CreatePdfTableCell(string text, BaseColor bgColor = null, int alignment = Element.ALIGN_LEFT)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = alignment;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            if (bgColor != null)
            {
                cell.BackgroundColor = bgColor;
            }
            return cell;
        }

        /// <summary>
        /// Adds a cell to the specified table.
        /// </summary>
        private static void AddCellToTable(PdfPTable table, string text, BaseColor bgColor = null, int alignment = Element.ALIGN_LEFT)
        {
            PdfPCell cell = CreatePdfTableCell(text, bgColor, alignment);
            table.AddCell(cell);
        }

        /// <summary>
        /// Creates a row for the PDF table based on a transaction item.
        /// </summary>
        private static PdfPTable CreatePdfTransactionTableRow(DS_TransactionGrid item, float[] tableWidth)
        {
            int headCount = tableWidth.Length;
            PdfPTable table = new PdfPTable(headCount)
            {
                WidthPercentage = 100
            };
            table.SetWidths(tableWidth);

            // Add transaction time
            string time = Lib_DateTime.GetDateTime(item.transaction_time);
            AddCellToTable(table, time);

            // Color taxID cell if it matches a special company
            var pdfTaxID = pdfTaxIDs?.Find(x => x.ID == item.taxID);
            if (pdfTaxID != null)
            {
                if (pdfTaxID.name == "+")
                {
                    // Light coral
                    AddCellToTable(table, item.taxID, bgColor: new BaseColor(255, 182, 193));
                }
                else if (pdfTaxID.name == "-")
                {
                    // Light yellow
                    AddCellToTable(table, item.taxID, bgColor: new BaseColor(255, 255, 224));
                }
                else
                {
                    AddCellToTable(table, item.taxID);
                }
            }
            else
            {
                AddCellToTable(table, item.taxID);
            }

            // Add remaining columns
            AddCellToTable(table, item.company_name);
            AddCellToTable(table, item.transaction_direction);
            AddCellToTable(table, item.item_name);
            AddCellToTable(table, item.item_quantity.ToString(), alignment: Element.ALIGN_RIGHT);
            AddCellToTable(table, item.item_unit);
            return table;
        }

        private static PdfPTable CreatePdfStorageTableRow(DS_StorageItem item, float[] tableWidth)
        {
            int headCount = tableWidth.Length;
            PdfPTable table = new PdfPTable(headCount)
            {
                WidthPercentage = 100
            };
            table.SetWidths(tableWidth);

            // Add remaining columns
            AddCellToTable(table, item.ID);
            AddCellToTable(table, item.name);
            AddCellToTable(table, item.quantity.ToString(), alignment: Element.ALIGN_RIGHT);
            AddCellToTable(table, item.unit);
            return table;
        }
        /// <summary>
        /// Determines if a new group should be started in the PDF (date/company/item/taxID).
        /// </summary>
        private static string IsSeparated(string company, string itemName, string taxID, DateTime transDate, DS_TransactionGrid item, int separateBy = 0)
        {
            string result = "";
            if (separateBy == 0 && transDate.Date != item.transaction_time.Date) // date
            {
                result = string.Format("Ngày: {0}", Lib_DateTime.GetDateOnly(item.transaction_time.Date));
            }
            else if (separateBy == 1 && company != item.company_name) // company
            {
                result = string.Format("Công ty/Khách hàng: {0}", item.company_name);
            }
            else if (separateBy == 2 && itemName != item.item_name) // item name
            {
                result = string.Format("Sản phẩm: {0}", item.item_name);
            }
            else if (separateBy == 3 && taxID != item.taxID)
            {
                result = string.Format("Số hóa đơn: {0}", item.taxID);
            }
            result += "\n\n";
            return result;
        }

        /// <summary>
        /// Adds tables of transaction data to the PDF document, grouping as specified.
        /// </summary>
        private static void DocumentAddTransactionTables(Document doc,
            List<DS_TransactionGrid> transItems, DS_Configuration pdfPage, int separateBy = 0)
        {
            // Track current group values
            string companyName = "";
            DateTime transactionDate = new DateTime();
            string itemName = "";
            string taxID = "";

            foreach (DS_TransactionGrid item in transItems)
            {
                // Check if a new group should start
                string textSeparate = IsSeparated(company: companyName, itemName: itemName,
                    taxID: taxID, transDate: transactionDate, item: item, separateBy: separateBy);
                if (!Lib_FormText.IsStringEmpty(textSeparate))
                {
                    // Update group values
                    companyName = item.company_name;
                    itemName = item.item_name;
                    transactionDate = item.transaction_time;
                    taxID = item.taxID;
                    // Add group header
                    DocumentAddParagraph(doc: doc, str: textSeparate);
                    PdfPTable tableHeader = CreatePdfTableHeader(tableWidth: pdfPage.pdfTableWidths, pdfHeader: Program_Parameters.pdfTransactionHeader);
                    doc.Add(tableHeader);
                }
                // Add transaction row
                PdfPTable tableRow = CreatePdfTransactionTableRow(item: item, tableWidth: pdfPage.pdfTableWidths);
                doc.Add(tableRow);
            }
        }

        private static void DocumentAddStorageTables(Document doc,
                    List<DS_StorageItem> storageItems, DS_Configuration pdfPage, int separateBy = 0)
        {
            // create header row
            PdfPTable tableHeader = CreatePdfTableHeader(tableWidth: pdfPage.pdfTableWidths, pdfHeader: Program_Parameters.newHeaderItems);
            doc.Add(tableHeader);

            foreach (DS_StorageItem item in storageItems)
            {
                PdfPTable tableRow = CreatePdfStorageTableRow(item: item, tableWidth: pdfPage.pdfTableWidths);
                doc.Add(tableRow);
            }
        }
        /// <summary>
        /// Adds a paragraph to the PDF document.
        /// </summary>
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
