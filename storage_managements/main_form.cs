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
        private List<DS_StorageItem> storages_display = new List<DS_StorageItem>();
        // list of pre-transaction items
        private List<DS_StorageItem> database_items_display = new List<DS_StorageItem>();
        private List<DS_StorageItem> transaction_items = new List<DS_StorageItem>();
        /*
		 * Tab transactions
		 */
        private readonly List<DS_TransactionGrid> transactions_history = new List<DS_TransactionGrid>();
        private List<DS_TransactionGrid> transactions_history_display = new List<DS_TransactionGrid>();
        private List<DS_Transaction> retrieve_transactions = new List<DS_Transaction>(); // retrieve  transaction for search.
        /*
		 * tab information (items, company, consumer)
		 */
        // list of database of items, consumers and company and tax IDs
        private List<DS_StorageItem> databaseItems = new List<DS_StorageItem>();
        
        private List<DS_Company> databaseConsumers = new List<DS_Company>();
        private List<DS_Company> databaseCompanies = new List<DS_Company>();
        private List<DS_Company> taxIDs = new List<DS_Company>();

        /*
         * list  taxIDs  if transaction of export is not equal to import
         * attribute name = '+' if import  is greater than export
         * attribute name = '-' if export is greater than import
         * reuse DS_Company to implement this feature
        */
        private List<DS_Company> taxIDs_Follow = new List<DS_Company>();

        // list of items information
        

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
            Lib_ComboBox.SourceItems_ID(cb: comboBox_company, databaseCompanies);
            Lib_ComboBox.SourceItems_ID(cb: comboBox_consumer, databaseConsumers);
            Lib_ComboBox.SourceItems_ID(cb: comboBox_taxID, items: taxIDs);
        }
        private void InitialGUIRadioButton()
        {
            radioButton_storage_all.Checked = true;
            radioButton_transaction_display_all.Checked = true;
            radioButton_transaction_sort_date.Checked = true;
            radioButton_database_items.Checked = true;
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
            DisplayDatabaseItem();
        }

        private void RetrieveTaxIDs()
        {
            // get all taxIDs in retrieve_transactions list ignore null values and storage in list of string
            taxIDs_Follow.Clear();
            List<String> tempTaxIDs = new List<String>();
            foreach (var item in retrieve_transactions)
            {
                if (item.taxID != null && item.taxID != "")
                {
                    tempTaxIDs.Add(item.taxID);
                }
            }
            // remove duplicated tempTaxIDs then store back
            tempTaxIDs = tempTaxIDs.Distinct().ToList();
            // for each tax ID in tempTaxIDs, calculate all quantities of export and import of each items
            // if got negative result , show message and stop
            foreach (var taxID in tempTaxIDs)
            {
                var exportQuantity = retrieve_transactions.Where(x => x.taxID == taxID && x.transaction_direction == direction.export).Sum(x => x.transaction_items.Sum(y => y.quantity));
                var importQuantity = retrieve_transactions.Where(x => x.taxID == taxID && x.transaction_direction == direction.import).Sum(x => x.transaction_items.Sum(y => y.quantity));
                // sign is - if export > import
                // sign is + if export < import
                // sign is '' if export = import
                String sign = exportQuantity == importQuantity ? "" : exportQuantity < importQuantity ? "+" : "-";
                taxIDs_Follow.Add(new DS_Company
                {
                    ID = taxID,
                    name = sign,
                });
            }
        }

        private void MessageResultTransaction(bool result)
        {
            if (result)
            {
                MessageOK(msg: Program_Parameters.message_successful_transaction);
                DatagridClearTransactions();
            }
            else
            {
                MessageFail(msg: Program_Parameters.message_failed_transaction);
            }
        }
        private void MessageResultAddition(bool result)
        {
            if (result)
            {
                MessageOK(msg: Program_Parameters.message_successful_add);
            }
            else
            {
                MessageFail(msg: Program_Parameters.message_failed_add);
            }
        }
        private void MessageOK(string msg = "")
        {
            Lib_Message.ShowMessageBox(mStr: msg, mbutton: MessageBoxButtons.OK, mIcon: MessageBoxIcon.Information);
        }
        private void MessageFail(string msg = "")
        {
            Lib_Message.ShowMessageBox(mStr: msg, mbutton: MessageBoxButtons.OK, mIcon: MessageBoxIcon.Error);
        }
        private bool MessageEmptyField()
        {
            Lib_Message.ShowMessageBox(mStr: Program_Parameters.message_empty_fields, mbutton: MessageBoxButtons.OK,
                    mIcon: MessageBoxIcon.Error);
            return false;
        }
        private bool MessageEmptyItems()
        {
            Lib_Message.ShowMessageBox(mStr: Program_Parameters.message_empty_items, mbutton: MessageBoxButtons.OK,
                    mIcon: MessageBoxIcon.Error);
            return false;
        }
        private bool IsTextboxEmpty(TextBox tb)
        {
            return Lib_FormText.IsTextboxEmpty(tb);
        }
        private void ReadAllInformation()
        {
            // read item information
            Lib_Json.ReadDatabaseItem(databaseItems);
            Lib_Json.ReadDatabaseItem(database_items_display);

            // read storage items
            Lib_Json.ReadStorage(storages);

            // read database for company, consumer and tax IDs
            Lib_Json.ReadCompany(databaseCompanies);
            Lib_Json.ReadConsumer(databaseConsumers);
            Lib_Json.ReadTaxID(taxIDs);
            // read transactions
            SetSearchDateTimeRange();
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
        private bool AddItem()
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
            bool result = Lib_List.DoAddUpdateDatabaseItem(items: databaseItems, ID: ID, name: name, unit: unit);
            if (result)
            {
                databaseItems = databaseItems.OrderBy(c => c.ID).ToList();
                Lib_Json.WriteDatabaseItem(items: databaseItems);
                // update database_items_display
                database_items_display.Clear();
                database_items_display = new List<DS_StorageItem>(databaseItems);
                DisplayDatabase();
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
            bool result = Lib_List.DoAddUpdateCompany(items: databaseCompanies, ID: ID, name: name);
            if (result)
            {
                databaseCompanies = databaseCompanies.OrderBy(c => c.ID).ToList();
                Lib_Json.WriteCompany(items: databaseCompanies);
                DisplayDatabase();
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
            bool result = Lib_List.DoAddUpdateCompany(items: databaseConsumers, ID: ID, name: name);
            if (result)
            {
                databaseConsumers = databaseConsumers.OrderBy(c => c.ID).ToList();
                Lib_Json.WriteConsumer(items: databaseConsumers);
                DisplayDatabase();
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
            if (this.transaction_items.Count <= 0)
            {
                return MessageEmptyItems();
            }
            List<DS_Transaction> todayTransactions = new List<DS_Transaction>();
            DS_Transaction currentTransaction = new DS_Transaction();
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
            currentTransaction.transaction_direction = dir;
            if (dir == direction.import)
            {
                pre_fix = "N";

            }
            else if (dir == direction.export)
            {
                pre_fix = "X";
            }
            currentTransaction.transaction_items = transaction_items;
            currentTransaction.ID = pre_fix + Lib_DateTime.GetIDByTime();
            currentTransaction.company_name = Lib_FormText.GetTextboxText(tb_name);
            currentTransaction.transaction_time = Lib_DateTime.GetCurrentTime();
            if (!Lib_FormText.IsTextboxEmpty(bt_taxID))
            {
                currentTransaction.taxID = Lib_FormText.GetTextboxText(bt_taxID);
                // update tax IDs if a new tax ID is added
                AddTaxID(textbox_new_taxID: bt_taxID);
            }
            currentTransaction.print_item();
            // get exist transaction in the same day.
            string file_path = Lib_DateTime.GetTransactionPathFromCurrentDate();
            Lib_Json.ReadTransactions(items: todayTransactions, filepath: file_path);
            todayTransactions.Add(currentTransaction);

            Lib_Json.WriteTransaction(items: todayTransactions, filepath: file_path);
            Lib_Json.WriteStorage(items: storages);
            // update tab transaction
            RetrieveTransaction();
            // DisplayPreTransactionItems
            DisplayPreTransactionItems();
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
            }
            retrieve_transactions = retrieve_transactions.OrderByDescending(th => th.transaction_time).ToList();
            RetrieveTaxIDs();
            TransactionsToGrid();
            DisplayTransactions();
        }

        private void DatagridDisplayStorage()
        {
            storages_display = storages_display.OrderBy(x => x.ID).ToList();
            Lib_DataGrid.displayStorage(dgv: datagrid_storage, items: storages_display);
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
            Display_relation relation = Display_relation.lesThan; // default lessthan relation
            if (radioButton_storage_in_storage.Checked)
            {
                threshold = 0;
                relation = Display_relation.greaterThan;
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
                relation = Display_relation.greaterThan;
            }

            StorageFilterQuantity(threshold: threshold, relation: relation);
            DatagridDisplayStorage();
        }

        private void DisplayDatabase()
        {
            if (radioButton_database_items.Checked)
            {
                DisplayDatabaseItem();
            }
            else if (radioButton_database_company.Checked)
            {
                DisplayDatabaseCompany();
            }
            else if (radioButton_database_consumer.Checked)
            {
                DisplayDatabaseConsumer();
            }
        }

        private void DisplayDatabaseItems()
        {
            Lib_DataGrid.displayStorage(dgv: datagrid_storage_items_info, items: databaseItems);
        }
        private void DisplayPreTransactionItems()
        {
            // update quantity for easy viewing
            // if database_items_display have id in storage, update quantity
            foreach (DS_StorageItem item in database_items_display)
            {
                DS_StorageItem storageItem = storages.FirstOrDefault(x => x.ID == item.ID);
                if (storageItem != null)
                {
                    item.quantity = storageItem.quantity;
                }
            }
            Lib_DataGrid.displayStorage(dgv: datagrid_storage_items_info, items: database_items_display);
        }
        private void DatagridDisplayGoingTransaction()
        {
            Lib_DataGrid.displayStorage(dgv: datagrid_storage_transaction, items: transaction_items);
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

        private void TransactionsToGrid()
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
        /*
        * search mode:
        * 0 : no search
        * 1 : search by ID
        * 2 : search by Name
        */
        private void TransactionsFilter(direction dir = direction.none, string filter_key = "",
            int searchMode = 0, bool filter_none = false)
        {
            string dir_str = TransactionDirection2String(dir);
            transactions_history_display.Clear();
            foreach (DS_TransactionGrid item in transactions_history)
            {
                bool searchResult = (searchMode == 1 && item.item_ID.ToLower().Contains(filter_key.ToLower())) ||
                                    (searchMode == 2 && item.company_name.ToLower().Contains(filter_key.ToLower())) ||
                                    (searchMode == 1 && item.taxID != null && item.taxID.ToLower().Contains(filter_key.ToLower()));
                if (item.transaction_direction == dir_str || filter_none || searchResult)
                {
                    transactions_history_display.Add(item);
                }
            }
            if (radioButton_transaction_sort_date.Checked)
            {
                transactions_history_display = transactions_history_display.OrderByDescending(th => th.transaction_time).ToList();
            }
            else if (radioButton_transaction_sort_company.Checked)
            {
                transactions_history_display = transactions_history_display.OrderBy(th => th.company_name).ToList();
            }
            else if (radioButton_transaction_sort_itemID.Checked)
            {
                transactions_history_display = transactions_history_display.OrderBy(th => th.item_ID).ToList();
            }
            else if (radioButton_transaction_sort_taxID.Checked)
            {
                transactions_history_display = transactions_history_display.OrderBy(th => th.taxID).ToList();
            }
        }

        private void StorageFilterQuantity(int threshold = int.MaxValue,
            Display_relation relation = Display_relation.lesThan)
        {
            storages_display.Clear();
            foreach (DS_StorageItem item in storages)
            {
                bool insert = false;
                if (relation == Display_relation.lesThan)
                {
                    insert = item.quantity <= threshold;
                }
                else if (relation == Display_relation.greaterThan)
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

        private void DisplayDatabaseItem()
        {
            Lib_DataGrid.displayStorage(dgv: datagrid_information, items: databaseItems);
        }
        private void DisplayDatabaseCompany()
        {
            Lib_DataGrid.displayCompany(dgv: datagrid_information, items: databaseCompanies, displayMode: 0);
        }
        private void DisplayDatabaseConsumer()
        {
            Lib_DataGrid.displayCompany(dgv: datagrid_information, items: databaseConsumers, displayMode: 1);
        }
        private void DatagridDisplayTransactions()
        {
            Lib_DataGrid.displayTransactions(dgv: dataGrid_transaction, items: transactions_history_display, taxIDs: taxIDs_Follow);

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
        /*
        * search mode:
        * 0 : no search
        * 1 : search by ID
        * 2 : search by Name
        */
        private void DatabaseItemSearch(string searchKey, int searchMode = 0)
        {
            if (searchKey.Trim().Count() == 0)
            {
                database_items_display.Clear();
                // deep copy from database_items to pre_transaction_items
                database_items_display = new List<DS_StorageItem>(databaseItems);
            }
            else
            {
                List<DS_StorageItem> tickedItems = GetTickedItems();
                foreach (DS_StorageItem item in databaseItems)
                {
                    bool found = (searchMode == 1 && item.ID.ToLower().Contains(searchKey.ToLower())) ||
                        (searchMode == 2 && item.name.ToLower().Contains(searchKey.ToLower()));
                    if (found)
                    {
                        int idx = Lib_List.GetIdxDatabaseItemByID(ID: item.ID, items: tickedItems);
                        if (idx == -1)
                        {
                            tickedItems.Add(item);
                        }
                    }
                }
                database_items_display.Clear();
                // deep copy from database_items to tickeditem
                database_items_display = new List<DS_StorageItem>(tickedItems);
            }

        }
        /**************************************************************/

        private void tab_view_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tabActive = tab_view.SelectedIndex;
            if (tabActive == 0) // storage tab
            {
                DisplayStorage();
            }
            else if (tabActive == 1) // transaction tab
            {
                DisplayPreTransactionItems();
            }
            else if (tabActive == 2) // transaction history tab
            {
                DisplayTransactions();
            }
            else if (tabActive == 3) // information tab
            {
                DisplayDatabaseItem();
            }

        }

        private void button_storage_pdf_Click(object sender, EventArgs e)
        {
            string filepath = Lib_DateTime.GetPdfStoragePathFromCurrentDate();
            if (transactions_history_display.Count > 0)
            {
                Lib_Pdf.CreateStoragePdf(filepath, items: storages);
                string resultmgs = string.Format("Xuất file thành công\n{0}", filepath);
                MessageOK(msg: resultmgs);
            }
            else
            {
                MessageOK(msg: "Chọn dữ liệu để xuất file!");
            }
        }
        private void button_export_Click(object sender, EventArgs e)
        {
            bool result = DoTransaction(dir: direction.export, tb_name: textBox_transaction_consumer, bt_taxID: textBox_transaction_taxID);
            MessageResultTransaction(result: result);
        }

        private void button_import_Click(object sender, EventArgs e)
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
            textBox_transaction_company.Text = databaseCompanies[selectedID].name;
        }

        private void comboBox_consumer_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedID = comboBox_consumer.SelectedIndex;
            textBox_transaction_consumer.Text = databaseConsumers[selectedID].name;
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

        private void radioButton_database_company_CheckedChanged(object sender, EventArgs e)
        {
            DisplayDatabase();
        }

        private void radioButton_database_consumer_CheckedChanged(object sender, EventArgs e)
        {
            DisplayDatabase();
        }

        private void radioButton_database_items_CheckedChanged(object sender, EventArgs e)
        {
            DisplayDatabase();
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
            bool result = AddItem();
            MessageResultAddition(result: result);
            DisplayDatabaseItem();
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
            if (radioButton_transaction_sort_date.Checked)          // transaction data time
            {
                separateByID = 0;
                separateByKey = "Ngay";
            }
            else if (radioButton_transaction_sort_company.Checked)  // company name
            {
                separateByID = 1;
                separateByKey = "CongTy";
            }
            else if (radioButton_transaction_sort_itemID.Checked)   // sort by items ID
            {
                separateByID = 2;
                separateByKey = "SanPham";
            }
            else if (radioButton_transaction_sort_taxID.Checked)    // sort by tax ID
            {
                separateByID = 3;
                separateByKey = "MaHoaDon";
            }
            string filepath = Lib_DateTime.GetPdfTransactionPathFromCurrentDate(seperateby: separateByKey);
            if (transactions_history_display.Count > 0)
            {
                Lib_Pdf.CreateTransactionPdf(filepath, items: transactions_history_display, taxIDs: taxIDs_Follow, separateBy: separateByID);
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
            string searchKey = textBox_search_name.Text.Trim();
            int tab_active = tab_view.SelectedIndex;
            if (tab_active == 0)  // storage tab
            {
                DisplayStorageByName(searchKey);
            }
            else if (tab_active == 1) // import/export tab
            {
                DatabaseItemSearch(searchKey: searchKey, searchMode: 2);
                DisplayPreTransactionItems();
            }
            else if (tab_active == 2) // transaction history tab
            {
                TransactionsFilter(filter_key: searchKey, searchMode: 2);
                DatagridDisplayTransactions();
            }
        }

        private void textBox_search_ID_TextChanged_1(object sender, EventArgs e)
        {
            string searchKey = textbox_search_ID.Text.Trim();
            int tabActive = tab_view.SelectedIndex;
            if (tabActive == 0)    // storage tab
            {
                DisplayStorageByID(searchKey);
            }
            else if (tabActive == 1)    // import/export tab
            {
                DatabaseItemSearch(searchKey: searchKey, searchMode: 1);
                DisplayPreTransactionItems();
            }
            else if (tabActive == 2)    // transaction history tab
            {
                TransactionsFilter(filter_key: searchKey, searchMode:1);
                DatagridDisplayTransactions();
            }
        }

        private void DisplayTransactions()
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
            TransactionsFilter(filter_none: filter_none, dir:dir);
            DatagridDisplayTransactions();
        }
        private void radioButton_transaction_display_all_CheckedChanged(object sender, EventArgs e)
        {
            DisplayTransactions();
        }

        private void radioButton_transaction_display_import_CheckedChanged(object sender, EventArgs e)
        {
            DisplayTransactions();
        }

        private void radioButton_transaction_display_export_CheckedChanged(object sender, EventArgs e)
        {
            DisplayTransactions();
        }

        private void radioButton_transaction_sort_date_CheckedChanged(object sender, EventArgs e)
        {
            DisplayTransactions();
        }

        private void radioButton_transaction_sort_company_CheckedChanged(object sender, EventArgs e)
        {
            DisplayTransactions();
        }

        private void radioButton_transaction_sort_item_CheckedChanged(object sender, EventArgs e)
        {
            DisplayTransactions();
        }
        private void radioButton_transaction_sort_taxID_CheckedChanged(object sender, EventArgs e)
        {
            DisplayTransactions();
        }
        private void button_goto_pdf_folder_Click(object sender, EventArgs e)
        {
            //WriteProgramConfiguration();
            Lib_FileDialog.OpenPdfFolder();
        }
    }
}
