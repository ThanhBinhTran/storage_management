using System.Collections.Generic;
using System.Linq;
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
        public static void displayStorage(DataGridView dgv, List<DS_StorageItem> items)
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

        public static void displayTransactions(DataGridView dgv, List<DS_TransactionGrid> items, List<DS_Company> taxIDs)
        {
            BindingSource source = new BindingSource
            {
                DataSource = items
            };
            dgv.DataSource = source;
            source.ResetBindings(false);

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            // highlight cell if taxID is inside taxIDs.ID
            // light yellow if taxIDs.name is '+'
            // light green if taxIDs.name is '-'
            dgv.CellFormatting += (sender, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == dgv.Columns["taxID"].Index)
                {
                    if (e.Value != null && taxIDs != null)
                    {
                        var taxID = taxIDs.Where(x => x.ID == e.Value.ToString()).FirstOrDefault();
                        if (taxID != null)
                        {
                            if (taxID.name == "+")
                            {
                                e.CellStyle.BackColor = System.Drawing.Color.LightCoral;
                            }
                            else if (taxID.name == "-")
                            {
                                e.CellStyle.BackColor = System.Drawing.Color.LightYellow;
                            }
                        }
                    }

                }
            };
            renameHeader(dgv: dgv, oldHeader: Program_Parameters.oldHeaderTransaction,
                                        newHeader: Program_Parameters.newHeaderTransaction);
        }
    }
}
