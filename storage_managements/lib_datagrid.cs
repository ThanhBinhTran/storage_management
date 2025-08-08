using System.Collections.Generic;
using System.Windows.Forms;

namespace storage_managements
{
    class Lib_DataGrid
    {
        public static void renameHeader(DataGridView dgv, List<string> oldHeader, List<string> newHeader)
        {
            for (int i = 0; i < oldHeader.Count; i++)
            {
                dgv.Columns[oldHeader[i]].HeaderText = newHeader[i];
            }
        }
        public static void displayGrid(DataGridView dgv, List<DS_StorageItem> items)
        {
            BindingSource source = new BindingSource
            {
                DataSource = items
            };
            dgv.DataSource = source;
            source.ResetBindings(false);
            //color even rows
            dgv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // if item number is less than threshold color red that row
            /*
            dgv.CellFormatting += (sender, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == dgv.Columns["quantity"].Index)
                {
                    if (int.TryParse(e.Value?.ToString(), out int quantity) && quantity < Program_Parameters.maxTaxID)
                    {
                        e.CellStyle.BackColor = System.Drawing.Color.Red;
                    }
                }
            };
            */
            renameHeader(dgv: dgv, oldHeader: Program_Parameters.oldHeaderItems,
                                   newHeader: Program_Parameters.newHeaderItems);
        }



        /*
		 * companies
         * mode = 0 display company
         * mode = 1 display consumer
		 */

        public static void displayCompany(DataGridView dgv, List<DS_Company> items, int displayMode = 0)
        {
            if (items.Count > 0)
            {
                BindingSource source = new BindingSource
                {
                    DataSource = items
                };
                dgv.DataSource = source;
                source.ResetBindings(false);
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                if (displayMode == 0) // company
                {
                    renameHeader(dgv, oldHeader: Program_Parameters.oldHeaderCompany,
                        newHeader: Program_Parameters.newHeaderCompany);
                }
                else    // consumer
                {
                    renameHeader(dgv, oldHeader: Program_Parameters.oldHeaderCompany,
                        newHeader: Program_Parameters.newHeaderConsumer);
                }
            }
        }

        public static void displayTransactions(DataGridView dgv, List<DS_TransactionGrid> items)
        {
            BindingSource source = new BindingSource
            {
                DataSource = items
            };
            dgv.DataSource = source;
            source.ResetBindings(false);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            renameHeader(dgv: dgv, oldHeader: Program_Parameters.oldHeaderTransaction,
                                        newHeader: Program_Parameters.newHeaderTransaction);
        }
    }
}
