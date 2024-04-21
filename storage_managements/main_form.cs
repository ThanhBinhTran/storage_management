using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;


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
		 * Tab transactions
		 */
        private List<DS_Transaction_Grid> transactions_history = new List<DS_Transaction_Grid>();
        private List<DS_Transaction> retrieve_transactions = new List<DS_Transaction>(); // retrieve  transaction for search.

        /*
		 * tab information (items, company, consumer)
		 */
        // list of consumers
        private List<DS_Company> consumers = new List<DS_Company>();
        // list of companys
        private List<DS_Company> companies = new List<DS_Company>();
        // list of items information
        private List<DS_Storage_Item> database_items = new List<DS_Storage_Item>();

        // range of history transactions
        private DateTime datetimeFrom;
        private DateTime datetimeTo;
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
            lib_json.ReadDatabaseItem(database_items);

            // read storage items
            lib_json.ReadStorageItem(storages);

            // read company 
            lib_json.ReadCompany(companies);
            lib_json.ReadConsumer(consumers);
        }

        /* get start and end dates for retrieval transactions */
        private void SetSearchDateTimeRange()
        {
            datetimeFrom = dateTimePicker_from.Value;
            datetimeTo = dateTimePicker_to.Value;
            if (dateTimePicker_from.Value > dateTimePicker_to.Value)
            {
                datetimeTo = dateTimePicker_from.Value;
                datetimeFrom = dateTimePicker_to.Value;
            }
        }
        /** read goods information from json file **/


        private bool AddDatabaseItem()
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
            lib_json.WriteDatabaseItem(items: database_items);
            return true;
        }

        private bool AddCompany()
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
        private bool AddConsumer()
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
                lib_json.WriteConsumer(items: consumers);
            }

            return true;
        }

        private bool DoTransaction(direction dir, string company_name) // in = import, out = export
        {
            string pre_fix = "";
            if (lib_form_text.is_string_empty(company_name))
            {
                lib_message.show_messagebox(mstr: Program_Parameters.message_company_empty, mbutton: MessageBoxButtons.OK,
                    micon: MessageBoxIcon.Error);
                return false;
            }
            List<DS_Transaction> inday_transactions = new List<DS_Transaction>();
            DS_Transaction cur_transaction = new DS_Transaction();
            List<DS_Storage_Item> transaction_items = new List<DS_Storage_Item>();
            foreach (DS_Storage_Item item in pre_transaction_items)
            {
                string ID = item.ID;
                string name = item.name;
                string unit = item.unit;
                int quantity = item.quantity;
                DS_Storage_Item s_preitem = new DS_Storage_Item
                {
                    ID = ID,
                    name = name,
                    unit = unit,
                    quantity = quantity
                };
                transaction_items.Add(s_preitem);

                lib_list.do_add_update_storage_item(items: storages, ID: ID, name: name,
                        unit: unit, quantity: quantity, dir: dir);
            }
            cur_transaction.transaction_direction = dir;
            if (dir == direction.import)
            {
                pre_fix = "N";

            }
            else if (dir == direction.export)
            {
                pre_fix = "X";
            }
            cur_transaction.transaction_items = transaction_items;
            cur_transaction.ID = pre_fix + lib_date_time.GetIDByDateTime();
            cur_transaction.company_name = company_name;
            cur_transaction.transaction_time = lib_date_time.GetCurrentTime();

            cur_transaction.print_item();
            // get exist transaction in the same day.
            string file_path = lib_date_time.GetTransactionPathFromCurrentDate();
            lib_json.ReadTransactions(items: inday_transactions, filepath: file_path);
            inday_transactions.Add(cur_transaction);

            lib_json.WriteTransaction(items: inday_transactions, filepath: file_path);
            lib_json.WriteStorageItem(items: storages);

            return true;
        }

        private void RetrieveTransaction()
        {
            retrieve_transactions.Clear();
            for (DateTime date = datetimeFrom; date <= datetimeTo; date = date.AddDays(1))
            {
                List<DS_Transaction> history_transaction = new List<DS_Transaction>();
                string file_path = lib_date_time.DateToTransactionPath(date);
                lib_json.ReadTransactions(items: history_transaction, filepath: file_path);
                foreach( DS_Transaction item in history_transaction)
                {
                    retrieve_transactions.Add(item);
                }
            }
        }
        private void ShowTransaction()
        {
            transactions_history.Clear();
            foreach (DS_Transaction transitem in retrieve_transactions)
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
        }

        /* table initial */
        private void TranslateVietnameseHeaderTable()
        {
            // storage datagrid
            List<string> oldHeaderStorage = new List<string> { "ID", "name", "unit" };
            List<string> newHeaderStorage = new List<string> { "Mã Sản Phẩm", "Tên Sản Phẩm", "Đơn vị" };
            lib_datagrid.datagridview_rename_header(dgv:datagrid_storage, oldHeader: oldHeaderStorage, newHeader: newHeaderStorage);
        }
        private void DatagridDisplayStorage()
        {
            lib_datagrid.datagridview_source_storage(dgv: datagrid_storage, items: storages_display);
        }

        private void DisplayStorageByID(string ID)
        {
            StorageItemsFilterID(ID: ID);
            DatagridDisplayStorage();
        }
        private void DisplayStorageByName(string name)
        {
            StorageItemsFilterName(name: name);
            DatagridDisplayStorage();
        }
        private void DisplayStorageByQuantity(int threshold = int.MaxValue,
            display_relation relation = display_relation.lessthan)
        {
            StorageFilterQuantity(threshold: threshold, relation: relation);
            DatagridDisplayStorage();
        }


        private void StorageItemsFilterID(string ID)
        {
            storages_display.Clear();
            foreach (DS_Storage_Item item in storages)
            {
                if (item.ID.ToLower().Contains(ID.ToLower()))
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

        private void StorageItemsFilterName(string name)
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

        private void TransactionsFilter(direction dir = direction.import, bool all_transaction = false)
        {
            transactions_history.Clear();
            foreach (DS_Transaction transitem in retrieve_transactions)
            {
                if (transitem.transaction_direction == dir || all_transaction)
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
            }
        }
        private void transaction_filter_by_date()
        {

        }
        private void StorageFilterQuantity(int threshold = int.MaxValue,
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
        
        private void DatagridDisplayItems()
        {
            lib_datagrid.datagrid_display_items(dgv: datagrid_information, items: database_items);
        }
        private void DatagridDisplayCompanies()
        {
            lib_datagrid.datagrid_display_companies(dgv: datagrid_information, items: companies);
        }
        private void DatagridDisplayConsumers()
        {
            lib_datagrid.datagrid_display_companies(dgv: datagrid_information, items: consumers, company: 1);
        }
        private void DatagridDisplayTransactions()
        {
            lib_datagrid.datagrid_display_transactions(dgv: dataGrid_transaction, items: transactions_history);
        }
        /**************************************************************/
        private void button1_Click(object sender, EventArgs e)
        {
            lib_list.add_database_item(database_items, ID: "123", name: "Đường", unit: "Bao 50Kg");
            lib_list.add_database_item(database_items, ID: "456", name: "Bột", unit: "Bao 25Kg");
            lib_list.add_database_item(database_items, ID: "789", name: "Dầu", unit: "Canh");
            lib_json.WriteDatabaseItem(database_items);
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
        }

        private void tab_view_SelectedIndexChanged(object sender, EventArgs e)
        {
            lib_datagrid.datagrid_display_items(dgv: datagrid_storage_items_info, items: database_items);
        }

        private void button3_Click(object sender, EventArgs e)
        {


        }

        private void button5_Click(object sender, EventArgs e)
        {
            DoTransaction(dir: direction.export, company_name: textBox_transaction_consumer.Text);
            label_message.Text = lib_date_time.GetIDByDateTime();
            lib_form_text.color_textbox(textBox_transaction_consumer);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DoTransaction(dir: direction.import, company_name: textBox_transaction_company.Text);
            label_message.Text = lib_date_time.GetIDByDateTime();
            lib_form_text.color_textbox(textBox_transaction_company);
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
            DisplayStorageByQuantity(0);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DisplayStorageByQuantity((int)numeric_threshold.Value);
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
            lib_datagrid.datagrid_display_companies(dgv: datagrid_information, items: consumers, company: 1);
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
#if false
            DateTime time_from = dateTimePicker_from.Value;
            DateTime time_to = dateTimePicker_to.Value;
            if (time_from > time_to)
            {
                lib_message.show_messagebox("lớn hơn");
            }
            {
                lib_message.show_messagebox("nhỏ hơn");
            }
            lib_date_time.GetAllDatesBetween(time_from, time_to); 
#endif
            string filepath = string.Format("test{0}.pdf", lib_date_time.GetIDByDateTime());
            if (transactions_history.Count > 0)
            {
                lib_pdf.CreatePdf(filepath, items: transactions_history);
            }


        }

        private void button_add_company_Click_1(object sender, EventArgs e)
        {
            AddCompany();
            lib_form_text.color_textbox(textbox_new_company_ID);
            lib_form_text.color_textbox(textbox_new_company_name);
        }

        private void button_add_consumer_Click_1(object sender, EventArgs e)
        {
            bool result = AddConsumer();
            lib_form_text.color_textbox(textbox_new_consumer_ID);
            lib_form_text.color_textbox(textbox_new_consumer_name);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            DisplayStorageByQuantity();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            DisplayStorageByQuantity(threshold: 0);
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            DisplayStorageByQuantity(threshold: (int)numeric_threshold.Value, relation: display_relation.lessthan);
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            DisplayStorageByQuantity(threshold: 0, relation: display_relation.greaterthan);
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            DisplayStorageByQuantity(threshold: (int)numeric_threshold.Value - 1, relation: display_relation.greaterthan);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (radioButton_less_than.Checked)
            {
                DisplayStorageByQuantity(threshold: (int)numeric_threshold.Value, relation: display_relation.lessthan);
            }
            else if (radioButton_greater_than.Checked)
            {
                DisplayStorageByQuantity(threshold: (int)numeric_threshold.Value - 1, relation: display_relation.greaterthan);
            }
        }

        private void textbox_search_ID_TextChanged(object sender, EventArgs e)
        {
            DisplayStorageByID(textbox_search_ID.Text.Trim());
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //display_storage_items_by_name();
        }

        private void textBox_search_name_TextChanged(object sender, EventArgs e)
        {
            DisplayStorageByName(textBox_search_name.Text.Trim());
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            RetrieveTransaction();
            ShowTransaction();
            DatagridDisplayTransactions();
        }

        private void radioButton_transaction_export_CheckedChanged(object sender, EventArgs e)
        {
            RetrieveTransaction();
            TransactionsFilter(dir: direction.export);
            DatagridDisplayTransactions();
        }

        private void radioButton_transaction_import_CheckedChanged(object sender, EventArgs e)
        {
            RetrieveTransaction();
            TransactionsFilter(dir: direction.import);

            DatagridDisplayTransactions();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            DatagridDisplayCompanies();
        }

        private void radioButton_consumer_CheckedChanged(object sender, EventArgs e)
        {
            DatagridDisplayConsumers();
        }

        private void radioButton_items_CheckedChanged(object sender, EventArgs e)
        {
            DatagridDisplayItems();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            RetrieveTransaction();
            TransactionsFilter(all_transaction: true);

            DatagridDisplayTransactions();
        }

        private void dateTimePicker_from_ValueChanged(object sender, EventArgs e)
        {
            SetSearchDateTimeRange();
        }

        private void radioButton_transaction_by_items_CheckedChanged(object sender, EventArgs e)
        {
            RetrieveTransaction();
            ShowTransaction();
            // Use OrderBy to sort the item ID
            var temp_transactions_history = transactions_history.OrderBy(th => th.item_ID).ToList();
            transactions_history = temp_transactions_history;
            DatagridDisplayTransactions();
        }

        private void radioButton_transaction_by_companies_CheckedChanged(object sender, EventArgs e)
        {
            RetrieveTransaction();
            ShowTransaction();
            // Use OrderBy to sort the company
            var temp_transactions_history = transactions_history.OrderBy(th => th.company_name).ToList();
            transactions_history = temp_transactions_history;
            DatagridDisplayTransactions();
        }

        private void comboBox_history_transaction_sort_SelectedIndexChanged(object sender, EventArgs e)
        {
            RetrieveTransaction();
            ShowTransaction();
            if (comboBox_history_transaction_sort.SelectedIndex == 0)
            {
                var temp_transactions_history = transactions_history.OrderBy(th => th.item_ID).ToList();
                transactions_history = temp_transactions_history;
            }
            else if (comboBox_history_transaction_sort.SelectedIndex == 1)
            {
                var temp_transactions_history = transactions_history.OrderBy(th => th.company_name).ToList();
                transactions_history = temp_transactions_history;
            }
            DatagridDisplayTransactions();
        }

        private void comboBox_transaction_display_SelectedIndexChanged(object sender, EventArgs e)
        {
            RetrieveTransaction();
            if (comboBox_transaction_display.SelectedIndex == 0)
            {
                TransactionsFilter(all_transaction: true);
            }
            else if (comboBox_transaction_display.SelectedIndex == 1)
            {
                TransactionsFilter(dir: direction.import);
            }
            else if (comboBox_transaction_display.SelectedIndex == 2)
            {
                TransactionsFilter(dir: direction.export);
            }
            DatagridDisplayTransactions();
        }

        private void dateTimePicker_to_ValueChanged(object sender, EventArgs e)
        {
            SetSearchDateTimeRange();
        }

        private void button_add_goods_Click(object sender, EventArgs e)
        {
            AddDatabaseItem();
            lib_form_text.color_textbox(textbox_new_item_ID);
            lib_form_text.color_textbox(textbox_new_item_name);
            lib_form_text.color_textbox(textbox_new_item_unit);
            DatagridDisplayItems();
        }

        private void button_add_Click(object sender, EventArgs e)
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
    }
}
