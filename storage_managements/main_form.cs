﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
        private readonly List<DS_StorageItem> storages = new List<DS_StorageItem>();
        private readonly List<DS_StorageItem> storages_display = new List<DS_StorageItem>();
        // list of pre-transaction items
        private readonly List<DS_StorageItem> pre_transaction_items = new List<DS_StorageItem>();
        /*
		 * Tab transactions
		 */
        private readonly List<DS_TransactionGrid> transactions_history = new List<DS_TransactionGrid>();
        private List<DS_TransactionGrid> transactions_history_show = new List<DS_TransactionGrid>();
        private readonly List<DS_Transaction> retrieve_transactions = new List<DS_Transaction>(); // retrieve  transaction for search.
        private int transactions_history_seperateby = 0;
        /*
		 * tab information (items, company, consumer)
		 */
        // list of consumers and companys
        private readonly List<DS_Company> consumers = new List<DS_Company>();
        private readonly List<DS_Company> companies = new List<DS_Company>();

        // list of items information
        private readonly List<DS_StorageItem> database_items = new List<DS_StorageItem>();

        // range of history transactions
        private DateTime datetimeFrom;
        private DateTime datetimeTo;
        public main_form()
        {
            InitializeComponent();

            // initial datastructure, information
            Initial_Program();

            // initial GUI
            InitialGUI();
        }

        /* 
		 * DEFINE FUNCTIONS
		 */
        /* initial program */
        private void Initial_Program()
        {
            // create path
            Program_Parameters.Create_paths();

            // read all infomation (storage, item, companies, consumers)
            ReadAllInformation();

            SetSearchDateTimeRange();

        }
        /* GUI */
        private void InitialGUI()
        {
            InitialGUILabel();
            InitialGUITextBox();
            InitialGUIDataGridView();
        }

        private void InitialGUIComboBox()
        {
            Lib_ComboBox.SourceItems(cb: comboBox_company, companies);
            Lib_ComboBox.SourceItems(cb: comboBox_consumer, consumers);
        }
        private void InitialGUILabel()
        {
            int num = (int)numeric_threshold.Value;
            string text_lt = string.Format("ít hơn {0} món", num);
            string text_gt = string.Format("nhiều hơn {0} món", num);
            SetRadioButtonText(rb: radioButton_less_than, text: text_lt);
            SetRadioButtonText(rb: radioButton_greater_than, text: text_gt);
        }
        private void InitialGUITextBox()
        {
            Lib_FormText.ClearColorTextBox(tb: textbox_new_company_ID);
            Lib_FormText.ClearColorTextBox(tb: textbox_new_company_name);
            Lib_FormText.ClearColorTextBox(tb: textbox_new_consumer_ID);
            Lib_FormText.ClearColorTextBox(tb: textbox_new_consumer_name);
            Lib_FormText.ClearColorTextBox(tb: textbox_new_item_ID);
            Lib_FormText.ClearColorTextBox(tb: textbox_new_item_name);
            Lib_FormText.ClearColorTextBox(tb: textbox_new_item_unit);
            Lib_FormText.ClearColorTextBox(tb: textBox_transaction_company);
            Lib_FormText.ClearColorTextBox(tb: textBox_transaction_consumer);
        }

        private void InitialGUIDataGridView()
        {
            // storage tab
            DisplayStorageByQuantity();

            // storage import/export tab
            DisplayDatabaseItems();

            // database information tab
            DatagridDisplayItems();
        }

        private void MessageOK(string msg = "")
        {
            Lib_Message.ShowMessagebox(mstr: msg, mbutton: MessageBoxButtons.OK, micon: MessageBoxIcon.Information);
        }
        private bool MessageEmptyField()
        {
            Lib_Message.ShowMessagebox(mstr: Program_Parameters.message_empty_fields, mbutton: MessageBoxButtons.OK,
                    micon: MessageBoxIcon.Error);
            return false;
        }
        private bool MessageEmptyItems()
        {
            Lib_Message.ShowMessagebox(mstr: Program_Parameters.message_empty_items, mbutton: MessageBoxButtons.OK,
                    micon: MessageBoxIcon.Error);
            return false;
        }
        private bool IsTextboxEmpty(TextBox tb)
        {
            return Lib_FormText.IsTextboxEmpty(tb);
        }
        private void ReadAllInformation()
        {
            // read item information
            Lib_Json.ReadDatabaseItem(database_items);

            // read storage items
            Lib_Json.ReadStorageItem(storages);

            // read company 
            Lib_Json.ReadCompany(companies);
            Lib_Json.ReadConsumer(consumers);
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

        private void SetRadioButtonText(RadioButton rb, string text)
        {
            rb.Text = text.Trim();
        }
        private bool AddDatabaseItem()
        {
            InitialGUITextBox();
            bool result1 = IsTextboxEmpty(textbox_new_item_ID);
            bool result2 = IsTextboxEmpty(textbox_new_item_name);
            bool result3 = IsTextboxEmpty(textbox_new_item_unit);
            if (result1 || result2 || result3)
            {
                return MessageEmptyField();
            }

            string ID = Lib_FormText.GetTextboxText(textbox_new_item_ID).ToUpper();
            string name = Lib_FormText.GetTextboxText(textbox_new_item_name);
            string unit = Lib_FormText.GetTextboxText(textbox_new_item_unit);
            bool result = Lib_List.DoAddUpdateDatabaseItem(items: database_items, ID: ID, name: name, unit: unit);
            if (result)
            {
                Lib_Json.WriteDatabaseItem(items: database_items);
                return true;
            }

            return true;
        }

        private bool AddCompany()
        {
            InitialGUITextBox();
            bool resul1 = IsTextboxEmpty(textbox_new_company_ID);
            bool resul2 = IsTextboxEmpty(textbox_new_company_name);
            bool result;
            if (resul1 || resul2)
            {
                return MessageEmptyField();
            }

            string ID = Lib_FormText.GetTextboxText(textbox_new_company_ID);
            string name = Lib_FormText.GetTextboxText(textbox_new_company_name);
            result = Lib_List.DoAddUpdateCompany(items: companies, ID: ID, name: name);
            if (result)
            {
                Lib_Json.WriteCompany(items: companies);
            }
            return true;
        }
        private bool AddConsumer()
        {
            InitialGUITextBox();
            bool result1 = IsTextboxEmpty(textbox_new_consumer_ID);
            bool result2 = IsTextboxEmpty(textbox_new_consumer_name);
            if (result1 || result2)
            {
                return MessageEmptyField();
            }

            string ID = Lib_FormText.GetTextboxText(textbox_new_consumer_ID);
            string name = Lib_FormText.GetTextboxText(textbox_new_consumer_name);
            bool result = Lib_List.DoAddUpdateCompany(items: consumers, ID: ID, name: name);
            if (result)
            {
                Lib_Json.WriteConsumer(items: consumers);
            }

            return true;
        }

        private bool DoTransaction(direction dir, TextBox tb) // in = import, out = export
        {
            string pre_fix = "";
            InitialGUI();
            if (Lib_FormText.IsTextboxEmpty(tb))
            {
                return MessageEmptyField();
            }
            if (pre_transaction_items.Count <= 0)
            {
                return MessageEmptyItems();
            }
            List<DS_Transaction> inday_transactions = new List<DS_Transaction>();
            DS_Transaction cur_transaction = new DS_Transaction();
            List<DS_StorageItem> transaction_items = new List<DS_StorageItem>();
            foreach (DS_StorageItem item in pre_transaction_items)
            {
                string ID = item.ID;
                string name = item.name;
                string unit = item.unit;
                int quantity = item.quantity;
                DS_StorageItem s_preitem = new DS_StorageItem
                {
                    ID = ID,
                    name = name,
                    unit = unit,
                    quantity = quantity
                };
                transaction_items.Add(s_preitem);

                Lib_List.DoAddUpdateStorageItem(items: storages, ID: ID, name: name,
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
            cur_transaction.ID = pre_fix + Lib_DateTime.GetIDByTime();
            cur_transaction.company_name = Lib_FormText.GetTextboxText(tb);
            cur_transaction.transaction_time = Lib_DateTime.GetCurrentTime();

            cur_transaction.print_item();
            // get exist transaction in the same day.
            string file_path = Lib_DateTime.GetTransactionPathFromCurrentDate();
            Lib_Json.ReadTransactions(items: inday_transactions, filepath: file_path);
            inday_transactions.Add(cur_transaction);

            Lib_Json.WriteTransaction(items: inday_transactions, filepath: file_path);
            Lib_Json.WriteStorageItem(items: storages);

            return true;
        }

        private void RetrieveTransaction()
        {
            retrieve_transactions.Clear();
            for (DateTime date = datetimeFrom; date <= datetimeTo; date = date.AddDays(1))
            {
                List<DS_Transaction> history_transaction = new List<DS_Transaction>();
                string file_path = Lib_DateTime.DateToTransactionPath(date);
                Lib_Json.ReadTransactions(items: history_transaction, filepath: file_path);
                foreach (DS_Transaction item in history_transaction)
                {
                    retrieve_transactions.Add(item);
                }
            }
            Transactionstogrid();
        }
        private void ShowTransaction()
        {
            transactions_history.Clear();
            foreach (DS_Transaction transitem in retrieve_transactions)
            {
                foreach (DS_StorageItem trans_item in transitem.transaction_items)
                {
                    DS_TransactionGrid trans_history = new DS_TransactionGrid
                    {
                        ID = transitem.ID,
                        company_name = transitem.company_name,
                        item_ID = trans_item.ID,
                        item_name = trans_item.name,
                        item_quantity = trans_item.quantity,
                        item_unit = trans_item.unit,
                        transaction_time = transitem.transaction_time,
                        //transaction_direction = transitem.transaction_direction
                    };
                    if (transitem.transaction_direction == direction.export)
                    {
                        trans_history.transaction_direction = "Xuất";
                    }
                    else if (transitem.transaction_direction == direction.import)
                    {
                        trans_history.transaction_direction = "Nhập";
                    }
                    transactions_history.Add(trans_history);
                }
            }
        }

        private void DatagridDisplayStorage()
        {
            Lib_DataGrid.DGVDisplayItem(dgv: datagrid_storage, items: storages_display);
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
            Display_relation relation = Display_relation.lessthan)
        {
            StorageFilterQuantity(threshold: threshold, relation: relation);
            DatagridDisplayStorage();
        }

        private void DisplayDatabaseItems()
        {
            Lib_DataGrid.DGVDisplayItem(dgv: datagrid_storage_items_info, items: database_items);
        }
        private void DatagridDisplayGoingTransaction()
        {
            Lib_DataGrid.DGVDisplayItem(dgv: datagrid_storage_transaction, items: pre_transaction_items);
        }

        private void StorageItemsFilterID(string ID)
        {
            storages_display.Clear();
            foreach (DS_StorageItem item in storages)
            {
                if (item.ID.ToLower().Contains(ID.ToLower()))
                {
                    DS_StorageItem preitem = new DS_StorageItem
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
            foreach (DS_StorageItem item in storages)
            {
                if (item.name.ToLower().Contains(name.ToLower()))
                {
                    DS_StorageItem preitem = new DS_StorageItem
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

        private void Transactionstogrid()
        {
            transactions_history.Clear();
            foreach (DS_Transaction transitem in retrieve_transactions)
            {
                foreach (DS_StorageItem trans_item in transitem.transaction_items)
                {
                    DS_TransactionGrid trans_history = new DS_TransactionGrid
                    {
                        ID = transitem.ID,
                        company_name = transitem.company_name,
                        item_ID = trans_item.ID,
                        item_name = trans_item.name,
                        item_quantity = trans_item.quantity,
                        item_unit = trans_item.unit,
                        transaction_time = transitem.transaction_time,
                    };
                    trans_history.transaction_direction = TransactionDirection2String(transitem.transaction_direction);
                    transactions_history.Add(trans_history);
                }
            }
        }
        private void TransactionsFilter(direction dir = direction.none, string filter_key = "", 
            bool searchenable=false, bool filter_none = false)
        {
            string dir_str = TransactionDirection2String(dir);
            transactions_history_show.Clear();
            foreach (DS_TransactionGrid item in transactions_history)
            {
                bool filter_result = item.item_ID.Contains(filter_key) ||
                    item.item_name.Contains(filter_key);
                if (item.transaction_direction == dir_str || filter_none || (searchenable && filter_result))
                {
                    transactions_history_show.Add(item);
                }
            }
        }

        private void StorageFilterQuantity(int threshold = int.MaxValue,
            Display_relation relation = Display_relation.lessthan)
        {
            storages_display.Clear();
            foreach (DS_StorageItem item in storages)
            {
                bool insert = false;
                if (relation == Display_relation.lessthan)
                {
                    insert = item.quantity <= threshold;
                }
                else if (relation == Display_relation.greaterthan)
                {
                    insert = item.quantity > threshold;
                }
                if (insert)
                {
                    DS_StorageItem preitem = new DS_StorageItem
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
            Lib_DataGrid.DGVDisplayItem(dgv: datagrid_information, items: database_items);
        }
        private void DatagridDisplayCompanies()
        {
            Lib_DataGrid.DGVDisplayCompany(dgv: datagrid_information, items: companies);
        }
        private void DatagridDisplayConsumers()
        {
            Lib_DataGrid.DGVDisplayCompany(dgv: datagrid_information, items: consumers, company: 1);
        }
        private void DatagridDisplayTransactions()
        {
            Lib_DataGrid.DGVDisplayTransactions(dgv: dataGrid_transaction, items: transactions_history_show);

        }
        private void DatagridClearTransactions()
        {
            pre_transaction_items.Clear();
            DatagridDisplayGoingTransaction();
        }

        private string TransactionDirection2String(direction dir)
        {
            if (dir == direction.export)
                return "Xuất";
            else if (dir == direction.import)
                return "Nhập";
            else
                return "None";
        }
        private void Notification(string msg)
        {
            Lib_FormText.DisplayNotification(label: label_message, msg: msg);
        }
        /**************************************************************/

        private void tab_view_SelectedIndexChanged(object sender, EventArgs e)
        {
            int cur_tab_index = tab_view.SelectedIndex;
            if (cur_tab_index == 0)
            {
                Lib_DataGrid.DGVDisplayItem(dgv: datagrid_storage_items_info, items: database_items);
            }

        }


        private void button5_Click(object sender, EventArgs e)
        {
            bool result = DoTransaction(dir: direction.export, tb: textBox_transaction_consumer);
            if (result)
            {
                Notification(msg: "Giao dịch thành công!");
                DatagridClearTransactions();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool result = DoTransaction(dir: direction.import, tb: textBox_transaction_company);
            if (result)
            {
                Notification(msg: "Giao dịch thành công!");
                DatagridClearTransactions();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pre_transaction_items.Clear();
            DatagridDisplayGoingTransaction();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DisplayStorageByQuantity(0);
        }




        private void comboBox_company_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_transaction_company.Text = comboBox_company.SelectedItem.ToString();
        }

        private void comboBox_consumer_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_transaction_consumer.Text = comboBox_consumer.SelectedItem.ToString();
        }


        private void button_add_company_Click_1(object sender, EventArgs e)
        {
            bool result = AddCompany();
            InitialGUIComboBox();
            if (result)
            {
                Notification(msg: "Thêm thành công");
            }
            else
            {
                Notification(msg: "Chưa hoàn thành việc thêm");
            }
        }

        private void button_add_consumer_Click_1(object sender, EventArgs e)
        {
            bool result = AddConsumer();
            InitialGUIComboBox();
            if (result)
            {
                Notification(msg: "Thêm thành công");
            }
            else
            {
                Notification(msg: "Chưa hoàn thành việc thêm");
            }
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
            DisplayStorageByQuantity(threshold: (int)numeric_threshold.Value, relation: Display_relation.lessthan);
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            DisplayStorageByQuantity(threshold: 0, relation: Display_relation.greaterthan);
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            DisplayStorageByQuantity(threshold: (int)numeric_threshold.Value - 1, relation: Display_relation.greaterthan);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            InitialGUILabel();
            if (radioButton_less_than.Checked)
            {
                DisplayStorageByQuantity(threshold: (int)numeric_threshold.Value, relation: Display_relation.lessthan);
            }
            else if (radioButton_greater_than.Checked)
            {
                DisplayStorageByQuantity(threshold: (int)numeric_threshold.Value - 1, relation: Display_relation.greaterthan);
            }
        }

        private void textbox_search_ID_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_search_name_TextChanged(object sender, EventArgs e)
        {

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

        private void dateTimePicker_from_ValueChanged(object sender, EventArgs e)
        {
            SetSearchDateTimeRange();
        }
        private void comboBox_history_transaction_sort_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
             * transactions_history_seperateby = 0 ; seperate by date 
             * transactions_history_seperateby = 1 ; seperate by companys 
             * transactions_history_seperateby = 2 ; seperate by items
             */
            transactions_history_seperateby = comboBox_history_transaction_sort.SelectedIndex;

            RetrieveTransaction();
            ShowTransaction();
            if (comboBox_history_transaction_sort.SelectedIndex == 0)
            {
                var temp_transactions_history = transactions_history_show.OrderBy(th => th.transaction_time).ToList();
                transactions_history_show = temp_transactions_history;
            }
            else if (comboBox_history_transaction_sort.SelectedIndex == 1)
            {
                var temp_transactions_history = transactions_history_show.OrderBy(th => th.company_name).ToList();
                transactions_history_show = temp_transactions_history;
            }
            else if (comboBox_history_transaction_sort.SelectedIndex == 2)
            {
                var temp_transactions_history = transactions_history_show.OrderBy(th => th.item_ID).ToList();
                transactions_history_show = temp_transactions_history;
            }
            DatagridDisplayTransactions();
        }

        private void comboBox_transaction_display_SelectedIndexChanged(object sender, EventArgs e)
        {
            RetrieveTransaction();
            if (comboBox_transaction_display.SelectedIndex == 0)
            {
                TransactionsFilter(filter_none: true);
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
            bool result = AddDatabaseItem();
            if (result)
            {
                Notification(msg: "Thêm thành công");
            }
            else
            {
                Notification(msg: "Chưa hoàn thành việc thêm");
            }
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
                    DS_StorageItem item = new DS_StorageItem
                    {
                        ID = row.Cells["ID"].Value.ToString(),
                        name = row.Cells["name"].Value.ToString(),
                        unit = row.Cells["unit"].Value.ToString(),
                        quantity = 1
                    };
                    pre_transaction_items.Add(item);
                }
            }
            DatagridDisplayGoingTransaction();

        }

        private void button_export_pdf_Click(object sender, EventArgs e)
        {
            string seperateName = "_";
            if (transactions_history_seperateby == 0)
            {
                seperateName = "N";
            }
            else if (transactions_history_seperateby == 1)
            {
                seperateName = "C";
            }
            else if (transactions_history_seperateby == 2)
            {
                seperateName = "S";
            }
            string filepath = Lib_DateTime.GetpdfPathFromCurrentDate(seperateby: seperateName);
            if (transactions_history.Count > 0)
            {
                Lib_Pdf.CreatePdf(filepath, items: transactions_history, seperateby: transactions_history_seperateby);
                string resultmgs = string.Format("Xuất file thành công\n{0}", Path.GetFullPath(filepath));
                MessageOK(msg: resultmgs);
            }
        }

        private void textBox_search_name_TextChanged_1(object sender, EventArgs e)
        {
            string searchkey = textBox_search_name.Text.Trim();
            int tab_active = tab_view.SelectedIndex;
            if (tab_active == 0)
            {
                DisplayStorageByName(searchkey);
            }
            else if (tab_active == 1)
            {
                Lib_FormText.DisplayNotification(label: label_message, msg: "tab view 1");
            }
            else if (tab_active == 2)
            {
                TransactionsFilter(filter_key: searchkey, searchenable: true);
                DatagridDisplayTransactions();
            }
            else if (tab_active == 3)
            {
                DatagridDisplayItems();
            }
        }

        private void textBox_search_ID_TextChanged_1(object sender, EventArgs e)
        {
            string searchkey = textbox_search_ID.Text.Trim();
            int tab_active = tab_view.SelectedIndex;
            if (tab_active == 0)
            {
                DisplayStorageByID(searchkey);
            }
            else if (tab_active == 1)
            {
                Lib_FormText.DisplayNotification(label: label_message, msg: "tab view 1");
            }
            else if (tab_active == 2)
            {
                TransactionsFilter(filter_key: searchkey, searchenable:true);
                DatagridDisplayTransactions();
            }
            else if (tab_active == 3)
            {
                DatagridDisplayItems();
            }
        }
    }
}
