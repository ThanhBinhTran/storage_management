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
		/*
		 * declaeration 
		 */
		// list of items information
		public List<DS_Database_Item> database_items = new List<DS_Database_Item>();

		// list of pre-transaction items
		public List<DS_Storage_prepare_Item> pre_transaction_items = new List<DS_Storage_prepare_Item>();

		// list of storage items
		public List<DS_Item> storage_items = new List<DS_Item>();

		// list of consumers
		public List<DS_Company> consumers = new List<DS_Company>();
		// list of companys
		public List<DS_Company> companies = new List<DS_Company>();
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

			// read all infomation (storage, item, companies, consumers)
			read_all_information();

			// show comsumer and company info
			lib_comboBox.add_items(comboBox_company, companies);
			lib_comboBox.add_items(comboBox_consumer, consumers);
			// show table info
			lib_datagrid.ShowTable_Item_Info(dgv: datagrid_information, items: database_items);
			lib_datagrid.ShowTable_Item_Info(dgv: datagrid_storage_items_info, items: database_items);
			
		}
		
		private void read_all_information()
        {
			// read item information
			lib_json.Read_Database_Item(database_items);

			// read storage items
			lib_json.Read_Storage_Item(storage_items);

			// read company 
			lib_json.Read_Company(companies);
			lib_json.Read_Consumer(consumers);
		}


		/** read goods information from json file **/


		private bool Add_Database_Item()
        {
			if (lib_text.is_textbox_empty(textbox_new_item_ID) || 
				lib_text.is_textbox_empty(textbox_new_item_name) ||
				lib_text.is_textbox_empty(textbox_new_item_unit))
            {
				return false;
            }

			string ID = lib_text.get_textbox_text(textbox_new_item_ID);
			string name = lib_text.get_textbox_text(textbox_new_item_name);
			string unit = lib_text.get_textbox_text(textbox_new_item_unit);
			lib_list.do_add_update_database_item(items: database_items, ID: ID, name: name, unit: unit);
			lib_json.Write_Database_Item(items: database_items);
			return true;
        }

		private bool Add_Conpany()
		{
			bool result;
			if (lib_text.is_textbox_empty(textbox_new_conpany_ID) ||
				lib_text.is_textbox_empty(textbox_new_company_name))
			{
				lib_message.show_messagebox(mstr: "Mã, tên công ty chưa có", mbutton: MessageBoxButtons.OK, micon: MessageBoxIcon.Error);
				return false;
			}

			string ID = lib_text.get_textbox_text(textbox_new_conpany_ID);
			string name = lib_text.get_textbox_text(textbox_new_company_name);
			result = lib_list.do_add_update_conpany(items: companies, ID: ID, name: name);
			if (result)
			{
				lib_json.Write_Company(items: companies);
			}
			return true;
		}
		private bool Add_Consumer()
		{
			bool result;
			if (lib_text.is_textbox_empty(textbox_new_consumer_ID) ||
				lib_text.is_textbox_empty(textbox_new_consumer_name))
			{
				lib_message.show_messagebox(mstr: "Mã, tên khách chưa có", mbutton: MessageBoxButtons.OK, micon: MessageBoxIcon.Error);
				return false;
			}

			string ID = lib_text.get_textbox_text(textbox_new_consumer_ID);
			string name = lib_text.get_textbox_text(textbox_new_consumer_name);
			result = lib_list.do_add_update_conpany(items: consumers, ID: ID, name: name);
			if (result)
            {
				lib_json.Write_Consumer(items: consumers);
			}				
			
			return true;
		}

		private bool do_transaction(int in_out, string company_name) // in = import, out = export
        {
			string pre_fix = "";
			if(lib_text.is_string_empty(company_name))
            {
				lib_message.show_messagebox(mstr: Program_Parameters.message_company_empty,mbutton:MessageBoxButtons.OK,
					micon:MessageBoxIcon.Error);
				return false;
            }
			List < DS_Transaction> transactions = new List<DS_Transaction>();
			DS_Transaction transaction = new DS_Transaction();
			List<DS_Storage_prepare_Item> transaction_items = new List<DS_Storage_prepare_Item>();
			foreach (DS_Storage_prepare_Item item in pre_transaction_items)
            {
				string ID = item.ID;
				string name = item.name;
				string unit = item.unit;
				int quantity = item.quantity;
				DS_Storage_prepare_Item s_preitem = new DS_Storage_prepare_Item { ID = ID, 
					name = name, unit = unit, quantity = quantity };
				transaction_items.Add(s_preitem);

				lib_list.do_add_update_storage_item(items: storage_items, ID: ID, name: name,
						unit: unit, quantity: quantity, in_out: in_out);
			}
			if (in_out == 1)
			{
				pre_fix = "N";
				transaction.transaction_direction = direction.import;
			}
			else
			{
				pre_fix = "X";
				transaction.transaction_direction = direction.export;
			}
			transaction.transaction_items = transaction_items;
			transaction.ID = pre_fix + lib_date_time.getID_byDateTime();
			transaction.company_name = company_name;
			transaction.transaction_time = lib_date_time.get_currenttime();

			transaction.print_item();
			// get exist transaction in the same day.
			lib_json.Read_Transactions(items: transactions);
			transactions.Add(transaction);
			
			lib_json.Write_Transaction(items: transactions);
			lib_json.Write_Storage_Item(items: storage_items);

			return true;
		}

		private void watch_storage(int quantity)
        {
			List<DS_Storage_prepare_Item> tempstorage = new List<DS_Storage_prepare_Item>();
			foreach (DS_Item item in storage_items)
			{
				if (item.quantity <= quantity)
				{
					DS_Storage_prepare_Item preitem = new DS_Storage_prepare_Item{ID = item.storage_item.ID,
						name = item.storage_item.name, unit = item.storage_item.unit, quantity = item.quantity };
					tempstorage.Add(preitem);
				}
			}
			lib_datagrid.datagridview_source_storage(dgv: datagrid_storage, items: tempstorage);
		}
		/**************************************************************/
		private void button1_Click(object sender, EventArgs e)
		{
			lib_list.add_database_item(database_items, ID: "123", name: "Đường", unit: "Bao 50Kg");
			lib_list.add_database_item(database_items, ID: "456", name: "Bột", unit: "Bao 25Kg");
			lib_list.add_database_item(database_items, ID: "789", name: "Dầu", unit: "Canh");
			lib_json.Write_Database_Item(database_items);
		}



		private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
		{

		}

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
			lib_datagrid.ShowTable_Item_Info(dgv: datagrid_information, items: database_items);
			//ShowTable_Item_Info(table_item, itemlist:item_list);

		}


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
			Add_Database_Item();
			lib_datagrid.ShowTable_Item_Info(dgv: datagrid_information, items: database_items);
		}

        private void tab_view_SelectedIndexChanged(object sender, EventArgs e)
        {
			lib_datagrid.ShowTable_Item_Info(dgv: datagrid_storage_items_info, items: database_items);
		}

        private void button3_Click(object sender, EventArgs e)
        {
			pre_transaction_items.Clear();
			foreach (DataGridViewRow row in datagrid_storage_items_info.Rows)
			{

				bool isChecked = Convert.ToBoolean(row.Cells[0].Value);

				if (isChecked)
				{
					DS_Storage_prepare_Item item = new DS_Storage_prepare_Item();
					item.ID = row.Cells["ID"].Value.ToString();
					item.name = row.Cells["name"].Value.ToString();
					item.unit = row.Cells["unit"].Value.ToString();
					item.quantity = 1;
					pre_transaction_items.Add(item);
				}
			}
			lib_datagrid.datagridview_source_storage(dgv: datagrid_storage_transaction, items: pre_transaction_items);

		}

        private void button5_Click(object sender, EventArgs e)
        {
			do_transaction(in_out: -1, company_name: textBox_transaction_consumer.Text);
			label_message.Text = lib_date_time.getID_byDateTime();
		}

        private void button4_Click(object sender, EventArgs e)
        {
			
			do_transaction(in_out: 1, company_name: textBox_transaction_company.Text);
			label_message.Text = lib_date_time.getID_byDateTime();
			
		}

        private void button6_Click(object sender, EventArgs e)
        {
			watch_storage(int.MaxValue);
        }

        private void button7_Click(object sender, EventArgs e)
        {
			pre_transaction_items.Clear();
			lib_datagrid.datagridview_source_storage(dgv: datagrid_storage_transaction, items: pre_transaction_items);
        }

        private void button8_Click(object sender, EventArgs e)
        {
			watch_storage(0);
		}

        private void button9_Click(object sender, EventArgs e)
        {
			watch_storage((int)numericUpDown1.Value);
		}

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button_add_company_Click(object sender, EventArgs e)
        {
			lib_datagrid.datagridview_source_company(dgv: datagrid_information, items: companies);
			List<string> oldlheader = new List<string> { "ID", "name" };
			List<string> newlheader = new List<string> { "Mã c.ty", "Tên cty" };
			lib_datagrid.datagridview_rename_header(dgv: datagrid_information, oldHeader: oldlheader, newHeader: newlheader);
		}

        private void button_add_consumer_Click(object sender, EventArgs e)
        {
			lib_datagrid.datagridview_source_company(dgv: datagrid_information, items: consumers);
			List<string> oldlheader = new List<string> { "ID", "name" };
			List<string> newlheader = new List<string> { "Mã Khách", "Tên Khách" };
			lib_datagrid.datagridview_rename_header(dgv: datagrid_information, oldHeader: oldlheader, newHeader: newlheader);
		}

        private void comboBox_company_SelectedIndexChanged(object sender, EventArgs e)
        {
			textBox_transaction_company.Text = comboBox_company.SelectedItem.ToString();
		}

        private void comboBox_consumer_SelectedIndexChanged(object sender, EventArgs e)
        {
			textBox_transaction_consumer.Text = comboBox_consumer.SelectedItem.ToString();
		}

        private void button1_Click_2(object sender, EventArgs e)
        {
			
			//disply message 
			lib_message.show_messagebox("hello binh");

        }

        private void button_add_company_Click_1(object sender, EventArgs e)
        {
			Add_Conpany();
		}

        private void button_add_consumer_Click_1(object sender, EventArgs e)
        {
			Add_Consumer();
		}
    }
}
