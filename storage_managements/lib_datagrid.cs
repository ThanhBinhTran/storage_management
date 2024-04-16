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

		public static void datagridview_source_storage(DataGridView dgv, List<DataStruct_Storage_prepare_Item> itemlist)
        {
			dgv.DataSource = itemlist;
			dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
		}
		public static void datagridview_source_item(DataGridView dgv, List<DataStruct_Database_Item> itemlist)
		{
			dgv.DataSource = itemlist;
			dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
		}
		public static void datagridview_rename_header(DataGridView dgv, List<string> oldHeader, List<string> newHeader)
		{
			for (int i = 0; i < oldHeader.Count; i++)
			{
				dgv.Columns[oldHeader[i]].HeaderText = newHeader[i];
			}
		}
		public static void ShowTable_Item_Info(DataGridView dgv, List<DataStruct_Database_Item> itemlist)
		{
			datagridview_source_item(dgv:dgv, itemlist:itemlist);

			List<string> oldlheader = new List<string> { "ID", "name", "unit" };
			List<string> newlheader = new List<string> { "Mã Sản Phẩm", "Tên Sản Phẩm", "Đơn vị" };
			datagridview_rename_header(dgv, oldHeader: oldlheader, newHeader: newlheader);

			
		}
	}
}
