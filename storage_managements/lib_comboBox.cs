using System.Collections.Generic;
using System.Windows.Forms;

namespace storage_managements
{
    class lib_ComboBox
    {
        public static void SourceItems(ComboBox cb, List<DS_Company> items)
        {
            cb.Items.Clear();
            foreach (DS_Company item in items)
            {
                cb.Items.Add(item.name);
            }
        }
    }
}
