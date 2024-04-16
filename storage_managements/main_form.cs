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

		// Declare a list of items information
		public List<DataStruct_Database_Item> database_items = new List<DataStruct_Database_Item>();

		// Declare a list of going to _transaction
		public List<DataStruct_Storage_prepare_Item> pre_transaction_items = new List<DataStruct_Storage_prepare_Item>();

		// Declare a list of items in storage
		public List<DataStruct_Storage_Item> storage_items = new List<DataStruct_Storage_Item>();
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
			Program_Parameters.create_paths();

			// read item information
			lib_json.Read_Database_Item(database_items);

			// read storage items
			lib_json.Read_Storage_Item(storage_items);

			// show table info
			lib_datagrid.ShowTable_Item_Info(dgv: datagrid_items, itemlist: database_items);
			lib_datagrid.ShowTable_Item_Info(dgv: datagrid_storage_items, itemlist: database_items);
			
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
			int idx = lib_list.get_idx_database_item_by_ID(ID: ID, items: database_items);
			if (idx == -1) // add new
            {
				lib_list.add_database_item(database_items, ID: ID, name: name, unit: unit);
				lib_text.display_debug(label_debug, "add new item");
			}
			else // update existed one
            {
				lib_list.update_database_item_by_idx(database_items, ID: ID, name: name, unit: unit, idx:idx);
				lib_text.display_debug(label_debug, "update existed one at " + idx);
			}
			lib_json.Write_Database_Item(items: database_items);
			return true;
        }


		private void textbox_dis(string info_str)
        {
			textbox_display.Text =info_str;
        }
		/**************************************************************/
		private void button1_Click(object sender, EventArgs e)
		{
			lib_list.add_database_item(database_items, ID: "123", name: "Đường", unit: "Bao 50Kg");
			lib_list.add_database_item(database_items, ID: "456", name: "Bột", unit: "Bao 25Kg");
			lib_list.add_database_item(database_items, ID: "789", name: "Dầu", unit: "Canh");
			lib_json.Write_Database_Item(database_items);
			label_debug.Text = "write " + database_items.Count + " items";
		}



		private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
		{

		}

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
			lib_datagrid.ShowTable_Item_Info(dgv: datagrid_items, itemlist: database_items);
			//ShowTable_Item_Info(table_item, itemlist:item_list);

		}


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
			Add_item();
			lib_datagrid.ShowTable_Item_Info(dgv: datagrid_items, itemlist: database_items);
			//ShowTable_Item_Info(table_item, item_list);
		}

        private void tab_view_SelectedIndexChanged(object sender, EventArgs e)
        {
			lib_datagrid.ShowTable_Item_Info(dgv: datagrid_storage_items, itemlist: database_items);
		}

        private void button3_Click(object sender, EventArgs e)
        {
			pre_transaction_items.Clear();
			foreach (DataGridViewRow row in datagrid_storage_items.Rows)
			{

				bool isChecked = Convert.ToBoolean(row.Cells[0].Value);

				if (isChecked)
				{
					DataStruct_Storage_prepare_Item item = new DataStruct_Storage_prepare_Item();
					item.ID = row.Cells["ID"].Value.ToString();
					item.name = row.Cells["name"].Value.ToString();
					item.unit = row.Cells["unit"].Value.ToString();
					item.quantity = 1;
					item.transction_time = lib_date_time.get_currenttime();
					pre_transaction_items.Add(item);
				}
			}
			lib_datagrid.datagridview_source_storage(dgv: datagrid_storage_transaction, itemlist: pre_transaction_items);

		}

        private void button5_Click(object sender, EventArgs e)
        {
			lib_datagrid.datagridview_source_storage(dgv: datagrid_storage_transaction, itemlist: pre_transaction_items);
        }

        private void button4_Click(object sender, EventArgs e)
        {
			foreach (DataGridViewRow row in datagrid_storage_transaction.Rows)
			{
				try
				{
					DataStruct_Storage_Item item = new DataStruct_Storage_Item();
					string ID = row.Cells["ID"].Value.ToString();
					string name = row.Cells["name"].Value.ToString();
					string unit = row.Cells["unit"].Value.ToString();
					// Using int.Parse
					int quantity;
					quantity = int.Parse(row.Cells["quantity"].Value.ToString());
					DateTime transction_time = lib_date_time.get_currenttime();
					lib_list.record_storage_item_transaction(items: storage_items, ID: ID, name: name,
						unit: unit, quantity: quantity, in_out: 1);
					lib_json.Write_Storage_Item(items: storage_items);
				}
				catch (FormatException)
				{
					Console.WriteLine("12345_Invalid format");
				}


			}
		}
    }
}
