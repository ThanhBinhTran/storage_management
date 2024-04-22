using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;


namespace storage_managements
{
    class lib_json
    {
        /*
		 * database for items information
		 */
        public static bool ReadDatabaseItem(List<DS_Storage_Item> items)
        {
            string filepath = Program_Parameters.filePath_goods;
            try
            {
                List<DS_Storage_Item> read_items = new List<DS_Storage_Item>();
                // Read JSON data from file
                string json = File.ReadAllText(filepath);
                Console.WriteLine("read item information " + filepath);
                // Deserialize JSON to a list of Player objects
                read_items = JsonConvert.DeserializeObject<List<DS_Storage_Item>>(json);
                Console.WriteLine("read item information: " + items.Count + "items");
                foreach (DS_Storage_Item item in read_items)
                {
                    items.Add(item);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("read item information [FAILED]" + ex.Message);
                return false;
            }
        }
        public static void WriteDatabaseItem(List<DS_Storage_Item> items)
        {
            string filepath = Program_Parameters.filePath_goods;
            string json_str = JsonConvert.SerializeObject(items, Formatting.Indented);

            // Write JSON to a file
            File.WriteAllText(filepath, json_str);

            // Read the JSON file
            Console.WriteLine(File.ReadAllText(filepath));
        }

        /*
		 * storage information
		 */
        public static bool ReadStorageItem(List<DS_Storage_Item> items)
        {
            try
            {
                List<DS_Storage_Item> read_items = new List<DS_Storage_Item>();
                // Read JSON data from file
                string json = File.ReadAllText(Program_Parameters.filePath_storage);
                Console.WriteLine("read item information " + Program_Parameters.filePath_storage);
                // Deserialize JSON to a list of Player objects
                read_items = JsonConvert.DeserializeObject<List<DS_Storage_Item>>(json);
                Console.WriteLine("read item information: " + items.Count + "items");
                foreach (DS_Storage_Item item in read_items)
                {
                    items.Add(item);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("read item information [FAILED]" + ex.Message);
                return false;
            }
        }
        public static void WriteStorageItem(List<DS_Storage_Item> items)
        {
            string filepath = Program_Parameters.filePath_storage;
            string json_str = JsonConvert.SerializeObject(items, Formatting.Indented);

            // Write JSON to a file
            File.WriteAllText(filepath, json_str);

            // Read the JSON file
            Console.WriteLine(File.ReadAllText(filepath));
        }

        /*
		 * company infomations
		 */
        public static bool ReadCompany(List<DS_Company> items)
        {
            return ReadDSCompany(items: items, filepath: Program_Parameters.filePath_company);
        }
        public static bool ReadConsumer(List<DS_Company> items)
        {
            return ReadDSCompany(items: items, filepath: Program_Parameters.filePath_consumer);
        }
        private static bool ReadDSCompany(List<DS_Company> items, string filepath)
        {
            try
            {
                List<DS_Company> read_items = new List<DS_Company>();
                // Read JSON data from file
                string json = File.ReadAllText(filepath);
                Console.WriteLine("read item information " + filepath);
                // Deserialize JSON to a list of Player objects
                read_items = JsonConvert.DeserializeObject<List<DS_Company>>(json);
                Console.WriteLine("read item information: " + items.Count + "items");
                foreach (DS_Company item in read_items)
                {
                    items.Add(item);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("read item information [FAILED]" + ex.Message);
                return false;
            }
        }
        public static void Write_Company(List<DS_Company> items)
        {
            WriteDSCompany(items: items, filepath: Program_Parameters.filePath_company);
        }
        public static void WriteConsumer(List<DS_Company> items)
        {
            WriteDSCompany(items: items, filepath: Program_Parameters.filePath_consumer);
        }
        public static void WriteDSCompany(List<DS_Company> items, string filepath)
        {
            string json_str = JsonConvert.SerializeObject(items, Formatting.Indented);

            // Write JSON to a file
            File.WriteAllText(filepath, json_str);

            // Read the JSON file
            Console.WriteLine(File.ReadAllText(filepath));
        }

        /*
		 * Transactions
		 */
        public static bool ReadTransactionsFromTO(List<DS_Transaction> items, DateTime dateFrom, DateTime dateTo)
        {
            List<DateTime> transaction_dates = lib_DateTime.GetAllDatesBetween(startDate: dateFrom, endDate: dateTo);
            foreach (DateTime dt in transaction_dates)
            {
                List<DS_Transaction> trans_items = new List<DS_Transaction>();
                string filepath = lib_DateTime.DateToTransactionPath(dt);
                ReadTransactions(items: items, filepath: filepath);
            }
            return true;
        }
        public static bool ReadTransactions(List<DS_Transaction> items, string filepath)
        {

            try
            {
                List<DS_Transaction> read_items = new List<DS_Transaction>();
                // Read JSON data from file
                string json = File.ReadAllText(filepath);
                Console.WriteLine("read item information " + filepath);
                // Deserialize JSON to a list of Player objects
                read_items = JsonConvert.DeserializeObject<List<DS_Transaction>>(json);
                Console.WriteLine("read item information: " + items.Count + "items");
                foreach (DS_Transaction item in read_items)
                {
                    items.Add(item);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("read item information [FAILED]" + ex.Message);
                return false;
            }
        }
        public static void WriteTransaction(List<DS_Transaction> items, string filepath)
        {
            string json_str = JsonConvert.SerializeObject(items, Formatting.Indented);

            // Write JSON to a file
            File.WriteAllText(filepath, json_str);

            // Read the JSON file
            Console.WriteLine(File.ReadAllText(filepath));
        }
    }
}
