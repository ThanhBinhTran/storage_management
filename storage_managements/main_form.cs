using System;
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
        private List<DS_StorageItem> pre_transaction_items = new List<DS_StorageItem>();
        private List<DS_StorageItem> transaction_items = new List<DS_StorageItem>();
        /*
		 * Tab transactions
		 */
        private readonly List<DS_TransactionGrid> transactions_history = new List<DS_TransactionGrid>();
        private List<DS_TransactionGrid> transactions_history_show = new List<DS_TransactionGrid>();
        private List<DS_Transaction> retrieve_transactions = new List<DS_Transaction>(); // retrieve  transaction for search.
        /*
		 * tab information (items, company, consumer)
		 */
        // list of consumers and company and tax IDs
        private readonly List<DS_Company> taxIDs = new List<DS_Company>();
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

            // initial data structure, information
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

            // read all information (storage, item, companies, consumers)
            ReadAllInformation();

            SetSearchDateTimeRange();

        }
        /* GUI */
        private void InitialGUI()
        {
            InitialGUILabel();
            InitialGUITextBox();

            InitialGUIComboBox();
            InitialGUIRadioButton();
            // last step, initial GUI data grid view
            InitialGUIDataGridView();
        }

        private void InitialGUIComboBox()
        {
            Lib_ComboBox.SourceItems_ID(cb: comboBox_company, companies);
            Lib_ComboBox.SourceItems_ID(cb: comboBox_consumer, consumers);
            Lib_ComboBox.SourceItems_ID(cb: comboBox_taxID, items: taxIDs);
        }
        private void InitialGUIRadioButton()
        {
            radioButton_storage_all.Checked = true;
            radioButton_transaction_display_all.Checked = true;
            radioButton_transaction_sort_date.Checked = true;
        }
        private void InitialGUILabel()
        {
            int num = (int)numeric_threshold.Value;
            string text_lt = string.Format("ít hơn {0} món", num);
            string text_gt = string.Format("nhiều hơn {0} món", num);
            SetRadioButtonText(rb: radioButton_storage_less_than, text: text_lt);
            SetRadioButtonText(rb: radioButton_storage_greater_than, text: text_gt);
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
            Lib_FormText.ClearColorTextBox(tb: textBox_transaction_taxID);
        }

        private void InitialGUIDataGridView()
        {
            // storage tab
            DisplayStorage();

            // storage import/export tab
            DisplayDatabaseItems();

            // database information tab
            DatagridDisplayItems();
        }

        private void MessageResultTransaction(bool result)
        {
            if (result)
            {
                MessageOK(msg: "Giao dịch thành công!");
                DatagridClearTransactions();
            }
            else
            {
                MessageFail(msg: "Giao dịch KHÔNG thành công!");
            }
        }
        private void MessageResultAddition(bool result)
        {
            if (result)
            {
                MessageOK(msg: "Thêm thành công");
            }
            else
            {
                MessageFail(msg: "Thêm CHƯA thành công");
            }
        }
        private void MessageOK(string msg = "")
        {
            Lib_Message.ShowMessagebox(mstr: msg, mbutton: MessageBoxButtons.OK, micon: MessageBoxIcon.Information);
        }
        private void MessageFail(string msg = "")
        {
            Lib_Message.ShowMessagebox(mstr: msg, mbutton: MessageBoxButtons.OK, micon: MessageBoxIcon.Error);
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
            Lib_Json.ReadDatabaseItem(pre_transaction_items);

            // read storage items
            Lib_Json.ReadStorage(storages);

            // read company, consumer and tax IDs
            Lib_Json.ReadCompany(companies);
            Lib_Json.ReadConsumer(consumers);
            Lib_Json.ReadTaxID(taxIDs);
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
            RetrieveTransaction();
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
            bool result1 = IsTextboxEmpty(textbox_new_company_ID);
            bool result2 = IsTextboxEmpty(textbox_new_company_name);
            if (result1 || result2)
            {
                return MessageEmptyField();
            }

            string ID = Lib_FormText.GetTextboxText(textbox_new_company_ID);
            string name = Lib_FormText.GetTextboxText(textbox_new_company_name);
            bool result = Lib_List.DoAddUpdateCompany(items: companies, ID: ID, name: name);
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

        private bool AddTaxID(TextBox textbox_new_taxID)
        {
            bool result1 = IsTextboxEmpty(textbox_new_taxID);
            if (result1)
            {
                return MessageEmptyField();
            }

            string taxID = Lib_FormText.GetTextboxText(textbox_new_taxID);
            bool result = Lib_List.doAddTaxID(items: taxIDs, ID: taxID, name: taxID);
            if (result)
            {
                // keep 30 the most recent tax IDs
                if (taxIDs.Count > Program_Parameters.maxTaxID)
                {
                    taxIDs.RemoveRange(0, taxIDs.Count - Program_Parameters.maxTaxID);
                }
                Lib_Json.WriteTaxID(items: taxIDs);
                InitialGUIComboBox();
            }

            return true;
        }

        private bool DoTransaction(direction dir, TextBox tb_name, TextBox bt_taxID) // in = import, out = export
        {
            string pre_fix = "";
            InitialGUI();
            if (Lib_FormText.IsTextboxEmpty(tb_name))
            {
                return MessageEmptyField();
            }
            if (Lib_FormText.IsTextboxEmpty(bt_taxID))
            {
                return MessageEmptyField();
            }
            if (this.transaction_items.Count <= 0)
            {
                return MessageEmptyItems();
            }
            List<DS_Transaction> inday_transactions = new List<DS_Transaction>();
            DS_Transaction cur_transaction = new DS_Transaction();
            List<DS_StorageItem> transaction_items = new List<DS_StorageItem>();
            foreach (DS_StorageItem item in this.transaction_items)
            {
                string ID = item.ID;
                string name = item.name;
                string unit = item.unit;
                float quantity = item.quantity;
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
            cur_transaction.company_name = Lib_FormText.GetTextboxText(tb_name);
            cur_transaction.transaction_time = Lib_DateTime.GetCurrentTime();
            cur_transaction.taxID = Lib_FormText.GetTextboxText(bt_taxID);
            cur_transaction.print_item();
            // get exist transaction in the same day.
            string file_path = Lib_DateTime.GetTransactionPathFromCurrentDate();
            Lib_Json.ReadTransactions(items: inday_transactions, filepath: file_path);
            inday_transactions.Add(cur_transaction);

            Lib_Json.WriteTransaction(items: inday_transactions, filepath: file_path);
            Lib_Json.WriteStorage(items: storages);
            // update tax IDs if a new tax ID is added
            AddTaxID(textbox_new_taxID: bt_taxID);
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
                // sort by descending transaction time
                retrieve_transactions = retrieve_transactions.OrderByDescending(th => th.transaction_time).ToList();
            }
            Transactionstogrid();
        }

        private void DatagridDisplayStorage()
        {
            Lib_DataGrid.displayGrid(dgv: datagrid_storage, items: storages_display);
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

        private void DisplayStorage()
        {
            int threshold = int.MaxValue;                          // default threshold, select all items
            Display_relation relation = Display_relation.lessthan; // default lessthan relation
            if (radioButton_storage_in_storage.Checked)
            {
                threshold = 0;
                relation = Display_relation.greaterthan;
            }
            else if (radioButton_storage_out_of_storage.Checked)
            {
                threshold = 0;
            }
            else if (radioButton_storage_less_than.Checked)
            {
                threshold = (int)numeric_threshold.Value;
            }
            else if (radioButton_storage_greater_than.Checked)
            {
                threshold = (int)numeric_threshold.Value - 1;
                relation = Display_relation.greaterthan;
            }

            StorageFilterQuantity(threshold: threshold, relation: relation);
            DatagridDisplayStorage();
        }

        private void DisplayDatabaseItems()
        {
            Lib_DataGrid.displayGrid(dgv: datagrid_storage_items_info, items: database_items);
        }
        private void DisplayPreTransactionItems()
        {
            Lib_DataGrid.displayGrid(dgv: datagrid_storage_items_info, items: pre_transaction_items);
        }
        private void DatagridDisplayGoingTransaction()
        {
            Lib_DataGrid.displayGrid(dgv: datagrid_storage_transaction, items: transaction_items);
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
                        //ID = transitem.ID,
                        company_name = transitem.company_name,
                        item_ID = trans_item.ID,
                        item_name = trans_item.name,
                        item_quantity = trans_item.quantity,
                        item_unit = trans_item.unit,
                        transaction_time = transitem.transaction_time,
                        taxID = transitem.taxID
                    };
                    trans_history.transaction_direction = TransactionDirection2String(transitem.transaction_direction);
                    transactions_history.Add(trans_history);
                }
            }
        }
        private void TransactionsFilter(direction dir = direction.none, string filter_key = "",
            bool searchenable = false, bool filter_none = false)
        {
            string dir_str = TransactionDirection2String(dir);
            transactions_history_show.Clear();
            foreach (DS_TransactionGrid item in transactions_history)
            {
                bool filter_result = item.item_ID.Contains(filter_key) ||
                    item.item_name.Contains(filter_key) ||
                    (item.taxID != null && item.taxID.Contains(filter_key));
                if (item.transaction_direction == dir_str || filter_none || (searchenable && filter_result))
                {
                    transactions_history_show.Add(item);
                }
            }
            if (radioButton_transaction_sort_date.Checked)
            {
                transactions_history_show = transactions_history_show.OrderByDescending(th => th.transaction_time).ToList();
            }
            else if (radioButton_transaction_sort_company.Checked)
            {
                transactions_history_show = transactions_history_show.OrderBy(th => th.company_name).ToList();
            }
            else if (radioButton_transaction_sort_itemID.Checked)
            {
                transactions_history_show = transactions_history_show.OrderBy(th => th.item_ID).ToList();
            }
            else if (radioButton_transaction_sort_taxID.Checked)
            {
                transactions_history_show = transactions_history_show.OrderBy(th => th.taxID).ToList();
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
            Lib_DataGrid.displayGrid(dgv: datagrid_information, items: database_items);
        }
        private void DatagridDisplayCompanies()
        {
            Lib_DataGrid.displayCompany(dgv: datagrid_information, items: companies);
        }
        private void DatagridDisplayConsumers()
        {
            Lib_DataGrid.displayCompany(dgv: datagrid_information, items: consumers, company: 1);
        }
        private void DatagridDisplayTransactions()
        {
            Lib_DataGrid.displayTransactions(dgv: dataGrid_transaction, items: transactions_history_show);

        }
        private void DatagridClearTransactions()
        {
            transaction_items.Clear();
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

        private void WriteProgramConfiguration()
        {
            DS_Configuration config = new DS_Configuration();
            config.page_left = 30f;
            config.page_right = 30f;
            config.page_top = 25f;
            config.page_bottom = 25f;

            config.pdfTableWidths = new float[] { 24f, 26f, 7f, 28f, 15f, 12.5f }; // Original list
            Lib_Json.WriteProgramConfiguration(item: config);
        }
        private List<DS_StorageItem> GetTickedItems()
        {
            List < DS_StorageItem > returnItems = new List<DS_StorageItem>();
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
                    returnItems.Add(item);
                }
            }
            return returnItems;
        }
        private void DatabaseItemFilter(string filter_key, int searchmode=0)
        {
            if(filter_key.Trim().Count() == 0 )
            {
                pre_transaction_items.Clear();
                foreach (DS_StorageItem item in database_items)
                {
                    pre_transaction_items.Add(item);
                }
            }
            else
            {
                List<DS_StorageItem> tickeditem = GetTickedItems();
                foreach (DS_StorageItem item in database_items)
                {
                    bool found = (searchmode == 0 && item.ID.Contains(filter_key)) ||
                        (searchmode != 0 && item.name.Contains(filter_key));
                    if (found)
                    {
                        int idx = Lib_List.GetIdxDatabaseItemByID(ID: item.ID, items: tickeditem);
                        if (idx == -1)
                        {
                            tickeditem.Add(item);
                        }
                    }
                }
                pre_transaction_items.Clear();
                foreach (DS_StorageItem item in tickeditem)
                {
                    pre_transaction_items.Add(item);
                }
            }

        }
        /**************************************************************/

        private void tab_view_SelectedIndexChanged(object sender, EventArgs e)
        {
            int cur_tab_index = tab_view.SelectedIndex;
            if (cur_tab_index == 0)
            {
                DisplayStorage();
            }

        }


        private void button5_Click(object sender, EventArgs e)
        {
            bool result = DoTransaction(dir: direction.export, tb_name: textBox_transaction_consumer, bt_taxID: textBox_transaction_taxID);
            MessageResultTransaction(result: result);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool result = DoTransaction(dir: direction.import, tb_name: textBox_transaction_company, bt_taxID: textBox_transaction_taxID);
            MessageResultTransaction(result: result);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            transaction_items.Clear();
            DatagridDisplayGoingTransaction();
        }
        private void comboBox_company_SelectedIndexChanged(object sender, EventArgs e)
        {
            //textBox_transaction_company.Text = comboBox_company.SelectedItem.ToString();
            int selectedID = comboBox_company.SelectedIndex;
            textBox_transaction_company.Text = companies[selectedID].name;
        }

        private void comboBox_consumer_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedID = comboBox_consumer.SelectedIndex;
            textBox_transaction_consumer.Text = consumers[selectedID].name;
        }
        private void comboBox_taxID_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_transaction_taxID.Text = comboBox_taxID.SelectedItem.ToString();
        }

        private void button_add_company_Click(object sender, EventArgs e)
        {
            bool result = AddCompany();
            InitialGUIComboBox();
            MessageResultAddition(result: result);
        }

        private void button_add_consumer_Click(object sender, EventArgs e)
        {
            bool result = AddConsumer();
            InitialGUIComboBox();
            MessageResultAddition(result: result);
        }

        private void radioButton_storage_all_CheckedChanged(object sender, EventArgs e)
        {
            DisplayStorage();
        }

        private void radioButton_storage_out_of_storage_CheckedChanged(object sender, EventArgs e)
        {
            DisplayStorage();
        }

        private void radioButton_storage_less_than_CheckedChanged(object sender, EventArgs e)
        {
            DisplayStorage();
        }

        private void radioButton_storage_in_storage_CheckedChanged(object sender, EventArgs e)
        {
            DisplayStorage();
        }

        private void radioButton_storage_greater_than_CheckedChanged(object sender, EventArgs e)
        {
            DisplayStorage();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            InitialGUILabel();
            DisplayStorage();
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
   
        private void dateTimePicker_to_ValueChanged(object sender, EventArgs e)
        {
            SetSearchDateTimeRange();
        }

        private void button_add_goods_Click(object sender, EventArgs e)
        {
            bool result = AddDatabaseItem();
            MessageResultAddition(result: result);
            DatagridDisplayItems();
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            transaction_items.Clear();
            transaction_items = GetTickedItems();
            DatagridDisplayGoingTransaction();

        }

        private void button_export_pdf_Click(object sender, EventArgs e)
        {
            int separateByID = 0;
            string separateByKey = "Ngay";
            if (radioButton_transaction_sort_date.Checked)
            {
                separateByID = 0;
                separateByKey = "Ngay";
            }
            else if (radioButton_transaction_sort_company.Checked)
            {
                separateByID = 1;
                separateByKey = "Cty";
            }
            else if (radioButton_transaction_sort_itemID.Checked)
            {
                separateByID = 2;
                separateByKey = "Sp";
            }
            string filepath = Lib_DateTime.GetpdfPathFromCurrentDate(seperateby: separateByKey);
            if (transactions_history_show.Count > 0)
            {
                Lib_Pdf.CreatePdf(filepath, items: transactions_history_show, separateBy: separateByID);
                string resultmgs = string.Format("Xuất file thành công\n{0}", filepath);
                MessageOK(msg: resultmgs);
            }
            else
            {
                MessageOK(msg: "Chọn dữ liệu để xuất file!");
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
                DatabaseItemFilter(filter_key: searchkey);
                DisplayPreTransactionItems();
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
            if (tab_active == 0)    // storage tab
            {
                DisplayStorageByID(searchkey);
            }
            else if (tab_active == 1)    // import/export tab
            {
                DatabaseItemFilter(filter_key: searchkey);
                DisplayPreTransactionItems();
            }
            else if (tab_active == 2)    // transaction history tab
            {
                display_transactions();
            }
            else if (tab_active == 3)    // database info tab
            {
                DatagridDisplayItems();
            }
        }

        private void display_transactions(string filter_key = "", bool searchenable=false)
        {
            // if radioButton_storage_all.Checked  bool filter_none  = true, else false
            bool filter_none = radioButton_transaction_display_all.Checked;
            direction dir = direction.none;
            if (radioButton_transaction_display_import.Checked)
            {
                dir = direction.import;
            }
            else if (radioButton_transaction_display_export.Checked)
            {
                dir = direction.export;
            }
            TransactionsFilter(dir:dir, filter_key: filter_key, searchenable: searchenable, filter_none: filter_none);
            DatagridDisplayTransactions();
        }
        private void radioButton_transaction_display_all_CheckedChanged(object sender, EventArgs e)
        {
            display_transactions();
        }

        private void radioButton_transaction_display_import_CheckedChanged(object sender, EventArgs e)
        {
            display_transactions();
        }

        private void radioButton_transaction_display_export_CheckedChanged(object sender, EventArgs e)
        {
            display_transactions();
        }

        private void radioButton_transaction_sort_date_CheckedChanged(object sender, EventArgs e)
        {
            display_transactions();
        }

        private void radioButton_transaction_sort_company_CheckedChanged(object sender, EventArgs e)
        {
            display_transactions();
        }

        private void radioButton_transaction_sort_item_CheckedChanged(object sender, EventArgs e)
        {
            display_transactions();
        }
        private void radioButton_transaction_sort_taxID_CheckedChanged(object sender, EventArgs e)
        {
            display_transactions();
        }
        private void button_goto_pdf_folder_Click(object sender, EventArgs e)
        {
            //WriteProgramConfiguration();
            Lib_FileDialog.OpenPdfFolder();
        }
    }
}
