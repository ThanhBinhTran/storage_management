using System.Collections.Generic;
using System.Windows.Forms;

namespace storage_managements
{
    class Lib_ComboBox
    {
        public static void SourceItems_name(ComboBox cb, List<DS_Company> items)
        {
            cb.Items.Clear();
            foreach (DS_Company item in items)
            {
                cb.Items.Add(item.name);
            }
        }
        public static void SourceItems_ID(ComboBox cb, List<DS_Company> items)
        {
            cb.Items.Clear();
            foreach (DS_Company item in items)
            {
                cb.Items.Add(item.ID);
            }
        }
    }
}
