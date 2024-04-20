using System;
using System.IO;

namespace storage_managements
{
    /* for display items currently in store*/
    public enum display_relation
    {
        lessthan,
        greaterthan
    };
    class Program_Parameters
    {
        public const string dataPath = @"data\\";
        public const string backupPath = dataPath + @"backup\\";
        public const string filePath_goods = dataPath + @"goods.json";
        public const string filePath_storage = dataPath + @"storage.json";
        public const string filePath_company = dataPath + @"company.json";
        public const string filePath_consumer = dataPath + @"consumer.json";

        public const string filename_transaction = @"transaction.json";

        public const string message_company_empty = "Tên công ty hoặc khách hàng đang trống";



        /* create path for program if nonexist*/
        public static void create_paths()
        {
            string year = lib_date_time.GetYear();
            string month = lib_date_time.GetMonth();

            Create_Path(Program_Parameters.dataPath);
            Create_Path(Program_Parameters.backupPath);
            Create_Path(Program_Parameters.dataPath + year + "\\");
            Create_Path(Program_Parameters.dataPath + year + "\\" + month);
        }

        private static void Create_file(string filepath)
        {
            if (!File.Exists(filepath))
            {
                File.Create(filepath).Close();
            }
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
