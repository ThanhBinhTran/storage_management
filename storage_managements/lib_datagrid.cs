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
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            renameHeader(dgv: dgv, oldHeader: Program_Parameters.oldHeaderItems,
                                   newHeader: Program_Parameters.newHeaderItems);
        }



        /*
		 * companies
		 */

        public static void displayCompany(DataGridView dgv, List<DS_Company> items, int company = 0)
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
                if (company == 0) // company
                {
                    renameHeader(dgv, oldHeader: Program_Parameters.oldHeaderCompany,
                        newHeader: Program_Parameters.newHeaderCompany);
                }
                else
                {
                    renameHeader(dgv, oldHeader: Program_Parameters.oldHeaderCompany,
                        newHeader: Program_Parameters.newHeaderCompany);
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
