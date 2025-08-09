using System;
using System.Collections.Generic;
using System.IO;

namespace storage_managements
{
    /* for display items currently in store*/
    public enum Display_relation
    {
        lesThan,
        greaterThan
    };
    class Program_Parameters
    {
        public const string dataPath = @"data\\";
        public const string pdfPath = dataPath + @"pdf\\";
        public const string filePath_goods = dataPath + @"goods.json";
        public const string filePath_storage = dataPath + @"storage.json";
        public const string filePath_company = dataPath + @"company.json";
        public const string filePath_consumer = dataPath + @"consumer.json";
        public const string filePath_configuration = dataPath + @"configuration.json";
        public const string filePath_taxID = dataPath + @"taxID.json";

        public const string filename_transaction = @"transaction.json";

        /*
        * transaction messages
        */ 
        public const string message_successful_transaction =  "Giao dịch thành công";
        public const string message_failed_transaction =  "Giao dịch KHÔNG thành công!";
        public const string message_successful_add =  "Thêm thành công";
        public const string message_failed_add =  "Thêm CHƯA thành công";
        public const string message_empty_fields = "Thiếu thông tin\nĐiền vào phần tô đậm";
        public const int maxTaxID = 30;
        public const string message_empty_items = "Thiếu sản phẩm\nChọn sản phẩm giao dịch";

        public static List<string> oldHeaderItems = new List<string> { "ID", "name", "quantity", "unit" };
        public static List<string> newHeaderItems = new List<string> { "Mã sản phẩm", "Tên sản phẩm", "Số lượng", "Đơn vị" };

        public static List<string> oldHeaderCompany = new List<string> { "ID", "name" };
        public static List<string> newHeaderCompany = new List<string> { "Mã công ty", "Tên công ty" };
        public static List<string> newHeaderConsumer = new List<string> { "Mã khách", "Tên khách" };

        public static List<string> oldHeaderTransaction = new List<string> {"transaction_direction", "company_name", "item_ID", "item_name", "item_quantity", "item_unit", "transaction_time", "taxID" };
        public static List<string> newHeaderTransaction = new List<string> {"Nhập/Xuất", "C.ty/Khách", "Mã sản phẩm", "Tên sản phẩm", "Số lượng", "Quy cách", "Thời gian", "Số hóa đơn" };

        public static List<string> pdfHeader = new List<string> { "Thời gian", "Số hóa đơn", "Công ty/khách hàng", "Giao dịch", "Sản phẩm", "Số lượng", "Quy cách" };
        public static float[] pdfTableWidths = new float[] { 15f, 15f, 26f, 9f, 16f, 8f, 8f };
        /* create path for program if non exist*/
        public static void Create_paths()
        {
            string year = Lib_DateTime.GetYear();
            string month = Lib_DateTime.GetMonth();

            Create_Path(Program_Parameters.dataPath);
            Create_Path(Program_Parameters.pdfPath);
            Create_Path(Program_Parameters.dataPath + year + "\\");
            Create_Path(Program_Parameters.dataPath + year + "\\" + month);
        }

        private static void Create_Path(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    Console.WriteLine($"Directory '{folderPath}' created successfully.");
                }
                else
                {
                    Console.WriteLine($"Directory '{folderPath}' already exists.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating directory: {ex.Message}");
            }
        }
    }
}
