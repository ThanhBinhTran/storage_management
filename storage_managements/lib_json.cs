using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace storage_managements
{
    class lib_json
	{
		/*
		 * database for items information
		 */
		public static bool Read_Database_Item(List<DataStruct_Database_Item> items)
		{
			try
			{
				List<DataStruct_Database_Item> read_items = new List<DataStruct_Database_Item>();
				// Read JSON data from file
				string json = File.ReadAllText(Program_Parameters.filePath_goods);
				Console.WriteLine("read item information " + Program_Parameters.filePath_goods);
				// Deserialize JSON to a list of Player objects
				read_items = JsonConvert.DeserializeObject<List<DataStruct_Database_Item>>(json);
				Console.WriteLine("read item information: " + items.Count + "items");
                foreach (DataStruct_Database_Item item in read_items)
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
		public static void Write_Database_Item(List<DataStruct_Database_Item> items)
		{
			string fileName = Program_Parameters.filePath_goods;
			string json_str = JsonConvert.SerializeObject(items, Formatting.Indented);

			// Write JSON to a file
			File.WriteAllText(fileName, json_str);

			// Read the JSON file
			Console.WriteLine(File.ReadAllText(fileName));
		}

		/*
		 * storage information
		 */
		public static bool Read_Storage_Item(List<DataStruct_Storage_Item> items)
		{
			try
			{
				List<DataStruct_Storage_Item> read_items = new List<DataStruct_Storage_Item>();
				// Read JSON data from file
				string json = File.ReadAllText(Program_Parameters.filePath_storage);
				Console.WriteLine("read item information " + Program_Parameters.filePath_storage);
				// Deserialize JSON to a list of Player objects
				read_items = JsonConvert.DeserializeObject<List<DataStruct_Storage_Item>>(json);
				Console.WriteLine("read item information: " + items.Count + "items");
				foreach (DataStruct_Storage_Item item in read_items)
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
		public static void Write_Storage_Item(List<DataStruct_Storage_Item> items)
		{
			string fileName = Program_Parameters.filePath_storage;
			string json_str = JsonConvert.SerializeObject(items, Formatting.Indented);

			// Write JSON to a file
			File.WriteAllText(fileName, json_str);

			// Read the JSON file
			Console.WriteLine(File.ReadAllText(fileName));
		}
	}
}
