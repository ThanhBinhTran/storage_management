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
    class lib_item
	{
		public static bool Read_Items(List<DataStruct_Item> item_list)
		{
			List<DataStruct_Item> items = new List<DataStruct_Item>();
			try
			{
				// Read JSON data from file
				string json = File.ReadAllText(Program_Parameters.filePath_goods);
				Console.WriteLine("read item information " + Program_Parameters.filePath_goods);
				// Deserialize JSON to a list of Player objects
				items = JsonConvert.DeserializeObject<List<DataStruct_Item>>(json);
				Console.WriteLine("read item information: " + item_list.Count + "items");
                foreach (DataStruct_Item item in items)
                {
					item_list.Add(item);
                }
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("read item information [FAILED]" + ex.Message);
				return false;
			}
		}
		public static void Write_Item(List<DataStruct_Item> item_list)
		{
			string fileName = Program_Parameters.filePath_goods;
			string json_str = JsonConvert.SerializeObject(item_list, Formatting.Indented);

			// Write JSON to a file
			File.WriteAllText(fileName, json_str);

			// Read the JSON file
			Console.WriteLine(File.ReadAllText(fileName));
		}
	}
}
