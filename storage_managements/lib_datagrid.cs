using System.Collections.Generic;
using System.Windows.Forms;

namespace storage_managements
{
    class lib_DataGrid
    {
        public static void DGVRenameHeader(DataGridView dgv, List<string> oldHeader, List<string> newHeader)
        {
            for (int i = 0; i < oldHeader.Count; i++)
            {
                dgv.Columns[oldHeader[i]].HeaderText = newHeader[i];
            }
        }
        public static void DGVDisplayItem(DataGridView dgv, List<DS_StorageItem> items, int item_type = 0)
        {
            BindingSource source = new BindingSource();
            source.DataSource = items;
            dgv.DataSource = source;
            source.ResetBindings(false);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DGVRenameHeader(dgv: dgv, oldHeader: Program_Parameters.oldHeaderitems,
                                   newHeader: Program_Parameters.newHeaderitems);
        }



        /*
		 * companies
		 */

        public static void DGVDisplayCompany(DataGridView dgv, List<DS_Company> items, int company = 0)
        {
            if (items.Count > 0)
            {
                BindingSource source = new BindingSource();
                source.DataSource = items;
                dgv.DataSource = source;
                source.ResetBindings(false);
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                if (company == 0) // conpany
                {
                    DGVRenameHeader(dgv, oldHeader: Program_Parameters.oldHeadercompany,
                        newHeader: Program_Parameters.newHeadercompany);
                }
                else
                {
                    DGVRenameHeader(dgv, oldHeader: Program_Parameters.oldHeadercompany,
                        newHeader: Program_Parameters.newHeadercompany);
                }
            }
        }

        public static void DGVDisplayTransactions(DataGridView dgv, List<DS_TransactionGrid> items)
        {
            BindingSource source = new BindingSource();
            source.DataSource = items;
            dgv.DataSource = source;
            source.ResetBindings(false);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DGVRenameHeader(dgv: dgv, oldHeader: Program_Parameters.oldHeadertransaction,
                                        newHeader: Program_Parameters.newHeadertransaction);
        }
    }
}
