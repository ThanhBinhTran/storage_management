using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;


namespace storage_managements
{
    class Lib_Json
    {
        /*
		 * database for items information
		 */
        public static bool ReadDatabaseItem(List<DS_StorageItem> items)
        {
            string filepath = Program_Parameters.filePath_goods;
            try
            {
                List<DS_StorageItem> read_items = new List<DS_StorageItem>();
                // Read JSON data from file
                string json = File.ReadAllText(filepath);
                Console.WriteLine("read item information " + filepath);
                // Deserialize JSON to a list of Player objects
                read_items = JsonConvert.DeserializeObject<List<DS_StorageItem>>(json);
                Console.WriteLine("read item information: " + items.Count + "items");
                foreach (DS_StorageItem item in read_items)
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
        public static void WriteDatabaseItem(List<DS_StorageItem> items)
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
        public static bool ReadStorage(List<DS_StorageItem> items)
        {
            try
            {
                List<DS_StorageItem> read_items = new List<DS_StorageItem>();
                // Read JSON data from file
                string json = File.ReadAllText(Program_Parameters.filePath_storage);
                Console.WriteLine("read item information " + Program_Parameters.filePath_storage);
                // Deserialize JSON to a list of Player objects
                read_items = JsonConvert.DeserializeObject<List<DS_StorageItem>>(json);
                Console.WriteLine("read item information: " + items.Count + "items");
                foreach (DS_StorageItem item in read_items)
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
        public static void WriteStorage(List<DS_StorageItem> items)
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
        public static void WriteCompany(List<DS_Company> items)
        {
            WriteDSCompany(items: items, filepath: Program_Parameters.filePath_company);
        }
        public static void WriteConsumer(List<DS_Company> items)
        {
            WriteDSCompany(items: items, filepath: Program_Parameters.filePath_consumer);
        }
        private static void WriteDSCompany(List<DS_Company> items, string filepath)
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

        public static DS_Configuration ReadProgramConfiguration()
        {
            string filepath = Program_Parameters.filePath_configuration;
            try
            {
                DS_Configuration read_items = new DS_Configuration();
                // Read JSON data from file
                string json = File.ReadAllText(filepath);
                Console.WriteLine("read item information " + filepath);
                // Deserialize JSON to a list of Player objects
                read_items = JsonConvert.DeserializeObject<DS_Configuration>(json);
                return read_items;
            }
            catch (Exception ex)
            {
                Console.WriteLine("read item information [FAILED]" + ex.Message);
                return null;
            }
        }
        public static void WriteProgramConfiguration(DS_Configuration item)
        {
            string filepath = Program_Parameters.filePath_configuration;
            string json_str = JsonConvert.SerializeObject(item, Formatting.Indented);

            // Write JSON to a file
            File.WriteAllText(filepath, json_str);

            // Read the JSON file
            Console.WriteLine(File.ReadAllText(filepath));
        }

    }
}
