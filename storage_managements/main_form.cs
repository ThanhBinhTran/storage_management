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
		 * Tab storage
		 */
		// list of storage items
		private List<DS_Storage_Item> storages = new List<DS_Storage_Item>();
		private List<DS_Storage_Item> storages_display = new List<DS_Storage_Item>();
		// list of pre-transaction items
		private List<DS_Storage_Item> pre_transaction_items = new List<DS_Storage_Item>();


		/*
		 * tab information (items, company, consumer)
		 */
		// list of consumers
		public List<DS_Company> consumers = new List<DS_Company>();
		// list of companys
		public List<DS_Company> companies = new List<DS_Company>();
		// list of items information
		public List<DS_Storage_Item> database_items = new List<DS_Storage_Item>();

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
			lib_datagrid.datagrid_display_items(dgv: datagrid_information, items: database_items);
			lib_datagrid.datagrid_display_items(dgv: datagrid_storage_items_info, items: database_items);
			
		}
		
		private void read_all_information()
        {
			// read item information
			lib_json.Read_Database_Item(database_items);

			// read storage items
			lib_json.Read_Storage_Item(storages);

			// read company 
			lib_json.Read_Company(companies);
			lib_json.Read_Consumer(consumers);
		}


		/** read goods information from json file **/


		private bool Add_Database_Item()
        {
			if (lib_form_text.is_textbox_empty(textbox_new_item_ID) || 
				lib_form_text.is_textbox_empty(textbox_new_item_name) ||
				lib_form_text.is_textbox_empty(textbox_new_item_unit))
            {
				lib_message.show_messagebox(mstr: "Tên hoặc mã hoặc đơn vị đang trống", 
					micon: MessageBoxIcon.Error, mbutton: MessageBoxButtons.OK);
				return false;
            }

			string ID = lib_form_text.get_textbox_text(textbox_new_item_ID);
			string name = lib_form_text.get_textbox_text(textbox_new_item_name);
			string unit = lib_form_text.get_textbox_text(textbox_new_item_unit);
			lib_list.do_add_update_database_item(items: database_items, ID: ID, name: name, unit: unit);
			lib_json.Write_Database_Item(items: database_items);
			return true;
        }

		private bool Add_Conpany()
		{
			bool result;
			if (lib_form_text.is_textbox_empty(textbox_new_company_ID) ||
				lib_form_text.is_textbox_empty(textbox_new_company_name))
			{
				lib_message.show_messagebox(mstr: "Mã, tên công ty chưa có", mbutton: MessageBoxButtons.OK, micon: MessageBoxIcon.Error);
				return false;
			}

			string ID = lib_form_text.get_textbox_text(textbox_new_company_ID);
			string name = lib_form_text.get_textbox_text(textbox_new_company_name);
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
			if (lib_form_text.is_textbox_empty(textbox_new_consumer_ID) ||
				lib_form_text.is_textbox_empty(textbox_new_consumer_name))
			{
				lib_message.show_messagebox(mstr: "Mã, tên khách chưa có", mbutton: MessageBoxButtons.OK, micon: MessageBoxIcon.Error);
				return false;
			}

			string ID = lib_form_text.get_textbox_text(textbox_new_consumer_ID);
			string name = lib_form_text.get_textbox_text(textbox_new_consumer_name);
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
			if(lib_form_text.is_string_empty(company_name))
            {
				lib_message.show_messagebox(mstr: Program_Parameters.message_company_empty,mbutton:MessageBoxButtons.OK,
					micon:MessageBoxIcon.Error);
				return false;
            }
			List < DS_Transaction> transactions = new List<DS_Transaction>();
			DS_Transaction transaction = new DS_Transaction();
			List<DS_Storage_Item> transaction_items = new List<DS_Storage_Item>();
			foreach (DS_Storage_Item item in pre_transaction_items)
            {
				string ID = item.ID;
				string name = item.name;
				string unit = item.unit;
				int quantity = item.quantity;
				DS_Storage_Item s_preitem = new DS_Storage_Item
				{ ID = ID, 
					name = name, unit = unit, quantity = quantity };
				transaction_items.Add(s_preitem);

				lib_list.do_add_update_storage_item(items: storages, ID: ID, name: name,
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
			lib_json.Write_Storage_Item(items: storages);

			return true;
		}

		/*
		 * relation = 0, less thanh low threshold
		 * relation = 1, greater than up threshold
		 */


		private void display_storage_items_by_ID(string ID)
		{
			storage_items_filter_by_ID(ID: ID);
			datagrid_display_storage();
		}
		private void display_storage_items_by_name(string name)
		{
			storage_items_filter_by_name(name:name);
			datagrid_display_storage();
		}
		private void display_storage_items_by_quantity(int threshold = int.MaxValue,
			display_relation relation = display_relation.lessthan)
		{
			storage_items_filter_by_quantity(threshold: threshold, relation: relation);
			datagrid_display_storage();
		}


		private void storage_items_filter_by_ID(string ID)
		{
			storages_display.Clear();
			foreach (DS_Storage_Item item in storages)
			{
				if(item.ID.ToLower().Contains(ID.ToLower()))
                {
					DS_Storage_Item preitem = new DS_Storage_Item
					{
						ID = item.ID,
						name = item.name,
						unit = item.unit,
						quantity = item.quantity
					};
					storages_display.Add(preitem);
				}					
			}
		}

		private void storage_items_filter_by_name(string name)
		{
			storages_display.Clear();
			foreach (DS_Storage_Item item in storages)
			{
				if (item.name.ToLower().Contains(name.ToLower()))
				{
					DS_Storage_Item preitem = new DS_Storage_Item
					{
						ID = item.ID,
						name = item.name,
						unit = item.unit,
						quantity = item.quantity
					};
					storages_display.Add(preitem);
				}
			}
		}

		private void storage_items_filter_by_quantity(int threshold = int.MaxValue,
			display_relation relation = display_relation.lessthan)
        {
			storages_display.Clear();
			bool insert = false;
			foreach (DS_Storage_Item item in storages)
			{
				insert = false;
				if (relation == display_relation.lessthan)
				{
					insert = item.quantity <= threshold;
				}
				else if (relation == display_relation.greaterthan)
				{
					insert = item.quantity > threshold;
				}
				if (insert)
				{
					DS_Storage_Item preitem = new DS_Storage_Item
					{
						ID = item.ID,
						name = item.name,
						unit = item.unit,
						quantity = item.quantity
					};
					storages_display.Add(preitem);
				}
			}
		}
		private void datagrid_display_storage()
        {
			lib_datagrid.datagridview_source_storage(dgv: datagrid_storage, items: storages_display);
		}
		private void datagrid_display_items()
		{
			lib_datagrid.datagrid_display_items(dgv: datagrid_information, items: database_items);
		}
		private void datagrid_display_companies()
		{
			lib_datagrid.datagrid_display_items(dgv: datagrid_information, items: database_items);
		}
		private void datagrid_display_consumers()
		{
			lib_datagrid.datagrid_display_items(dgv: datagrid_information, items: database_items);
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
			lib_datagrid.datagrid_display_items(dgv: datagrid_information, items: database_items);
			//ShowTable_Item_Info(table_item, itemlist:item_list);

		}


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
			Add_Database_Item();
			lib_form_text.color_textbox(textbox_new_item_ID);
			lib_form_text.color_textbox(textbox_new_item_name);
			lib_form_text.color_textbox(textbox_new_item_unit);
			datagrid_display_items();
			
		}

        private void tab_view_SelectedIndexChanged(object sender, EventArgs e)
        {
			lib_datagrid.datagrid_display_items(dgv: datagrid_storage_items_info, items: database_items);
		}

        private void button3_Click(object sender, EventArgs e)
        {
			pre_transaction_items.Clear();
			foreach (DataGridViewRow row in datagrid_storage_items_info.Rows)
			{

				bool isChecked = Convert.ToBoolean(row.Cells[0].Value);

				if (isChecked)
				{
					DS_Storage_Item item = new DS_Storage_Item();
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
			
        }

        private void button7_Click(object sender, EventArgs e)
        {
			pre_transaction_items.Clear();
			lib_datagrid.datagridview_source_storage(dgv: datagrid_storage_transaction, items: pre_transaction_items);
        }

        private void button8_Click(object sender, EventArgs e)
        {
			display_storage_items_by_quantity(0);
		}

        private void button9_Click(object sender, EventArgs e)
        {
			display_storage_items_by_quantity((int)numeric_threshold.Value);
		}

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button_add_company_Click(object sender, EventArgs e)
        {
			lib_datagrid.datagrid_display_companies(dgv: datagrid_information, items: companies);
		}

        private void button_add_consumer_Click(object sender, EventArgs e)
        {
			lib_datagrid.datagrid_display_companies(dgv: datagrid_information, items: consumers, company:1);
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
			lib_form_text.color_textbox(textbox_new_company_ID);
			lib_form_text.color_textbox(textbox_new_company_name);
		}

        private void button_add_consumer_Click_1(object sender, EventArgs e)
        {
			bool result = Add_Consumer();
			lib_form_text.color_textbox(textbox_new_consumer_ID);
			lib_form_text.color_textbox(textbox_new_consumer_name);
		}

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
			display_storage_items_by_quantity();
		}

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
			display_storage_items_by_quantity(threshold:0);
		}

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
			display_storage_items_by_quantity(threshold: (int)numeric_threshold.Value, relation: display_relation.lessthan);
		}

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
			display_storage_items_by_quantity(threshold: 0, relation: display_relation.greaterthan);
		}

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
			display_storage_items_by_quantity(threshold: (int)numeric_threshold.Value-1, relation: display_relation.greaterthan);
		}

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
			if(radioButton_less_than.Checked)
            {
				display_storage_items_by_quantity(threshold: (int)numeric_threshold.Value, relation: display_relation.lessthan);
			}				
			else if(radioButton_greater_than.Checked)
			{ 
				display_storage_items_by_quantity(threshold: (int)numeric_threshold.Value -1, relation: display_relation.greaterthan);
			}
        }

        private void textbox_search_ID_TextChanged(object sender, EventArgs e)
        {
			display_storage_items_by_ID(textbox_search_ID.Text.Trim());
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
			//display_storage_items_by_name();
		}

        private void textBox_search_name_TextChanged(object sender, EventArgs e)
        {
			display_storage_items_by_name(textBox_search_name.Text.Trim());
		}

        private void button2_Click_1(object sender, EventArgs e)
        {
			List<DS_Transaction> transactions = new List<DS_Transaction>();
			List<DS_Transaction_Grid> transactions_history = new List<DS_Transaction_Grid>();
			lib_json.Read_Transactions(items: transactions);
			foreach(DS_Transaction transitem in transactions)
            {
				foreach (DS_Storage_Item trans_item in transitem.transaction_items)
                {
					DS_Transaction_Grid trans_history = new DS_Transaction_Grid
					{
						ID = transitem.ID,
						company_name = transitem.company_name,
						item_ID = trans_item.ID,
						item_name = trans_item.name,
						item_quantity = trans_item.quantity,
						item_unit = trans_item.unit,
						transaction_time = transitem.transaction_time,
						transaction_direction = transitem.transaction_direction
					};
					transactions_history.Add(trans_history);
				}					
            }
			dataGrid_transaction.DataSource = transactions_history;
		}
    }
}
