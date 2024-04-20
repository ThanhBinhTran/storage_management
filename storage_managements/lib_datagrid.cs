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
		public static void datagridview_prestorage_clear(DataGridView dgv, List<DS_Storage_Item> items)
        {
			items.Clear();
			datagridview_source_storage(dgv, items);
        }
		
		public static void datagridview_source_storage(DataGridView dgv, List<DS_Storage_Item> items)
        {
			BindingSource source = new BindingSource();
			source.DataSource = items;
			//dgv.DataSource = items;
			dgv.DataSource = source;
			source.ResetBindings(false);
			dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
		}
		
		public static void datagridview_source_item(DataGridView dgv, List<DS_Storage_Item> items)
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
		
		public static void datagrid_display_items(DataGridView dgv, List<DS_Storage_Item> items)
		{
			datagridview_source_item(dgv:dgv, items:items);

			List<string> oldlheader = new List<string> { "ID", "name", "unit" };
			List<string> newlheader = new List<string> { "Mã Sản Phẩm", "Tên Sản Phẩm", "Đơn vị" };
			datagridview_rename_header(dgv, oldHeader: oldlheader, newHeader: newlheader);
		}

		/*
		 * companies
		 */

		private static void datagridview_source_company(DataGridView dgv, List<DS_Company> items)
		{
			BindingSource source = new BindingSource();
			source.DataSource = items;
			dgv.DataSource = source;
			source.ResetBindings(false);
			dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
		}
		public static void datagrid_display_companies(DataGridView dgv, List<DS_Company> items, int company = 0)
		{
			datagridview_source_company(dgv: dgv, items: items);
			List<string> oldlheader = new List<string> { "ID", "name" };
			List<string> newlheader = new List<string>();
			if (company ==0) // conpany
            {
				newlheader.Add("Mã c.ty");
				newlheader.Add("Tên c.ty");
			}
			else
            {
				newlheader.Add("Mã khách");
				newlheader.Add("Tên khách");
			}				

			datagridview_rename_header(dgv, oldHeader: oldlheader, newHeader: newlheader);
		}

		private static void datagridview_source_transactions(DataGridView dgv, List<DS_Transaction_Grid> items)
        {
			BindingSource source = new BindingSource();
			source.DataSource = items;
			dgv.DataSource = source;
			source.ResetBindings(false);
			dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
		}
		public static void datagrid_display_transactions(DataGridView dgv, List<DS_Transaction_Grid> items, int company = 0)
		{
			datagridview_source_transactions(dgv: dgv, items: items);
			//List<string> oldlheader = new List<string> { "ID", "name" };
			//List<string> newlheader = new List<string> { "ID", "name" };
			//datagridview_rename_header(dgv, oldHeader: oldlheader, newHeader: newlheader);
		}
	}
}
