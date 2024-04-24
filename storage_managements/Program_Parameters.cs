using System;
using System.Collections.Generic;
using System.IO;

namespace storage_managements
{
    /* for display items currently in store*/
    public enum Display_relation
    {
        lessthan,
        greaterthan
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

        public const string filename_transaction = @"transaction.json";

        public const string message_empty_fields = "Thiếu thông tin\nĐiền vào phần tô đậm";
        public const string message_empty_items = "Thiếu sản phẩm\nChọn sản phẩm giao dịch";

        public static List<string> oldHeaderitems = new List<string> { "ID", "name", "quantity", "unit" };
        public static List<string> newHeaderitems = new List<string> { "Mã sản phẩm", "Tên sản phẩm", "Số lượng", "Đơn vị" };

        public static List<string> oldHeadercompany = new List<string> { "ID", "name" };
        public static List<string> newHeadercompany = new List<string> { "Mã cty", "Tên cty" };
        public static List<string> newHeaderconsumer = new List<string> { "Mã khách", "Tên khách" };

        public static List<string> oldHeadertransaction = new List<string> {"transaction_direction", "company_name", "item_ID", "item_name", "item_quantity", "item_unit", "transaction_time" };
        public static List<string> newHeadertransaction = new List<string> {"Nhập/Xuất", "Đối tác", "Mã sản phẩm", "Tên sản phẩm", "Số lượng", "Quy cách", "Thời gian" };

        public static List<string> pdfHeader = new List<string> { "Thời gian", "Đối tác", "Giao dịch", "Sản phẩm", "Số lượng", "Quy cách" };
        public static float[] pdfTableWidths = new float[] { 24.0f, 26f, 7.0f, 28f, 15f, 12.5f };
        /* create path for program if nonexist*/
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
