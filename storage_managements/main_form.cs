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
	public partial class main_form : Form
	{
		// for debug only
		public int i = 0;

		// Declare a list of datastruct_goods
		public List<DataStruct_Item> item_list = new List<DataStruct_Item>();

		// Create a new BindingSource object.
		public BindingSource bindingSource_item_list = new BindingSource();

		

		public main_form()
		{
			InitializeComponent();

			Initial_Program();
		}

		/* 
		 * DEFINE FUNCTIONS
		 */
		/* initial program */
		private void Initial_Program()
        {
			// create path
			//Create_Path(Program_Parameters.dataPath); (//BINHBINH OPEN)

			// read item information
			lib_item.Read_Items(item_list);
			if (item_list.Count > 0)
            {
				textbox_dis(item_list.First().name);
			}
			else
            {
				Console.WriteLine("No item found");
            }

			// Set the DataSource of the BindingSource.
			bindingSource_item_list.DataSource = item_list;

			// show table info
			ShowTable_Item_Info();
		}
		/* create path for program if nonexist*/
		private void Create_Path(string folderPath)
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

		/** read goods information from json file **/


		private bool Add_item()
        {
			if (lib_text.is_textbox_empty(item_textbox_new_ID) || 
				lib_text.is_textbox_empty(item_textbox_new_name) ||
				lib_text.is_textbox_empty(item_textbox_new_name))
            {
				label_debug.Text = "thieu cho trong";
				return false;
            }

			string ID = lib_text.get_textbox_text(item_textbox_new_ID);
			string name = lib_text.get_textbox_text(item_textbox_new_name);
			string unit = lib_text.get_textbox_text(item_textbox_new_unit);
			int idx = lib_list.is_exist_item_list_ID(ID: ID, items: item_list);
			if (idx == -1) // add new
            {
				lib_list.add_item(item_list, ID: ID, name: name, unit: unit);
				lib_text.display_debug(label_debug, "add new item");
			}
			else // update existed one
            {
				lib_list.update_item_idx(item_list, ID: ID, name: name, unit: unit, idx:idx);
				lib_text.display_debug(label_debug, "update existed one at " + idx);
			}
			lib_item.Write_Item(item_list: item_list);
			return true;
        }


		private void textbox_dis(string info_str)
        {
			textbox_display.Text =info_str;
        }
		/**************************************************************/
		private void button1_Click(object sender, EventArgs e)
		{
			lib_list.add_item(item_list, ID: "123", name: "Đường", unit: "Bao 50Kg");
			lib_list.add_item(item_list, ID: "456", name: "Bột", unit: "Bao 25Kg");
			lib_list.add_item(item_list, ID: "789", name: "Dầu", unit: "Canh");
			lib_item.Write_Item(item_list);
			label_debug.Text = "write " + item_list.Count + " items";
		}



		private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
		{

		}

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
			ShowTable_Item_Info();

		}
		private void ShowTable_Item_Info()
        {
			bindingSource_item_list.DataSource = item_list;
			label_debug.Text = "read table " + item_list.Count + " items";
			lib_list.print_item_list(item_List: item_list);
			
			table_item.DataSource = bindingSource_item_list;
			table_item.Columns["ID"].HeaderText = "Mã Sản Phẩm";
			table_item.Columns["name"].HeaderText = "Tên Sản Phẩm";
			table_item.Columns["unit"].HeaderText = "Đơn vị";
			table_item.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
		}

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
			Add_item();
			ShowTable_Item_Info();
		}
    }
}
