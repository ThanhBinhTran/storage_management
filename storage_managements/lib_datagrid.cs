using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace storage_managements
{
    class lib_datagrid
    {
		
		public static void datagridview_prestorage_clear(DataGridView dgv, List<DS_Storage_prepare_Item> items)
        {
			items.Clear();
			datagridview_source_storage(dgv, items);
        }
		
		public static void datagridview_source_storage(DataGridView dgv, List<DS_Storage_prepare_Item> items)
        {
			BindingSource source = new BindingSource();
			source.DataSource = items;
			//dgv.DataSource = items;
			dgv.DataSource = source;
			source.ResetBindings(false);
			dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
		}
		public static void datagridview_source_item(DataGridView dgv, List<DS_Database_Item> items)
		{
			BindingSource source = new BindingSource();
			source.DataSource = items;
			//dgv.DataSource = items;
			dgv.DataSource = source;
			source.ResetBindings(false);
			dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
		}
		public static void datagridview_rename_header(DataGridView dgv, List<string> oldHeader, List<string> newHeader)
		{
			for (int i = 0; i < oldHeader.Count; i++)
			{
				dgv.Columns[oldHeader[i]].HeaderText = newHeader[i];
			}
		}
		public static void ShowTable_Item_Info(DataGridView dgv, List<DS_Database_Item> items)
		{
			datagridview_source_item(dgv:dgv, items:items);

			List<string> oldlheader = new List<string> { "ID", "name", "unit" };
			List<string> newlheader = new List<string> { "Mã Sản Phẩm", "Tên Sản Phẩm", "Đơn vị" };
			datagridview_rename_header(dgv, oldHeader: oldlheader, newHeader: newlheader);

			
		}
	}
}
