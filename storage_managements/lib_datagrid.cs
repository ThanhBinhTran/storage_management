using System.Collections.Generic;
using System.Windows.Forms;

namespace storage_managements
{
    class lib_datagrid
    {
        public static void DGVPreStorageClear(DataGridView dgv, List<DS_Storage_Item> items)
        {
            items.Clear();
            DGVSourceStorage(dgv, items);
        }

        public static void DGVSourceStorage(DataGridView dgv, List<DS_Storage_Item> items)
        {
            BindingSource source = new BindingSource();
            source.DataSource = items;
            //dgv.DataSource = items;
            dgv.DataSource = source;
            source.ResetBindings(false);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public static void DGVSourceItem(DataGridView dgv, List<DS_Storage_Item> items)
        {
            BindingSource source = new BindingSource();
            source.DataSource = items;
            //dgv.DataSource = items;
            dgv.DataSource = source;
            source.ResetBindings(false);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public static void DGVRenameStorageHeader(DataGridView dgv)
        {
            DGVRenameHeader(dgv: dgv, oldHeader: Program_Parameters.oldHeaderStorage, newHeader: Program_Parameters.newHeaderStorage);
        }
        public static void DGVRenameHeader(DataGridView dgv, List<string> oldHeader, List<string> newHeader)
        {
            for (int i = 0; i < oldHeader.Count; i++)
            {
                dgv.Columns[oldHeader[i]].HeaderText = newHeader[i];
            }
        }

        public static void DGVDisplayItems(DataGridView dgv, List<DS_Storage_Item> items)
        {
            DGVSourceItem(dgv: dgv, items: items);
            DGVRenameStorageHeader(dgv: dgv);
        }

        /*
		 * companies
		 */

        private static void DGVSourceCompany(DataGridView dgv, List<DS_Company> items)
        {
            BindingSource source = new BindingSource();
            source.DataSource = items;
            dgv.DataSource = source;
            source.ResetBindings(false);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        public static void DGVDisplayCompanies(DataGridView dgv, List<DS_Company> items, int company = 0)
        {
            DGVSourceCompany(dgv: dgv, items: items);
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

        private static void DGVSourceTransactions(DataGridView dgv, List<DS_Transaction_Grid> items)
        {
            BindingSource source = new BindingSource();
            source.DataSource = items;
            dgv.DataSource = source;
            source.ResetBindings(false);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        public static void DGVDisplayTransactions(DataGridView dgv, List<DS_Transaction_Grid> items, int company = 0)
        {
            DGVSourceTransactions(dgv: dgv, items: items);
            DGVRenameHeader(dgv: dgv, oldHeader: Program_Parameters.oldHeadertransaction,
                newHeader: Program_Parameters.newHeadertransaction);
        }
    }
}
