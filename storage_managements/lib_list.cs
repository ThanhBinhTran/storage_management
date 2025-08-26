using System.Collections.Generic;
using System.Windows.Forms;
namespace storage_managements
{
    class Lib_List
    {
        /*
         * database items
         */
        public static void AddDatabaseItem(List<DS_StorageItem> items, string ID, string name, string unit)
        {
            DS_StorageItem item = new DS_StorageItem
            {
                ID = ID,
                name = name,
                unit = unit
            };
            items.Add(item);
        }
        public static void UpdateDatabaseItemByIdx(List<DS_StorageItem> items, string ID, string name, string unit, int idx)
        {
            items[idx].ID = ID;
            items[idx].name = name;
            items[idx].unit = unit;
        }
        public static int GetIdxDatabaseItemByID(string ID, List<DS_StorageItem> items)
        {
            return items.FindIndex(item => item.ID == ID);
        }
        public static bool DoAddUpdateDatabaseItem(List<DS_StorageItem> items, string ID, string name, string unit)
        {
            int idx = Lib_List.GetIdxDatabaseItemByID(ID, items);

            if (idx > -1) // existed one
            {
                bool confirm = Lib_Message.ShowMessageBox(
                    mStr: "Mã đã được dùng.\nVẫn muốn ghi đè?",
                    mbutton: MessageBoxButtons.OKCancel,
                    mIcon: MessageBoxIcon.Warning
                );

                if (!confirm)
                    return false;
            }

            if (idx == -1) // new one
            {
                Lib_List.AddDatabaseItem(items, ID, name, unit);
            }
            else // update existed item
            {
                Lib_List.UpdateDatabaseItemByIdx(items, ID, name, unit, idx);
            }

            return true;

        }

        /*
         * storage items
         */
        public static void AddStorageItem(List<DS_StorageItem> items, string ID, string name, string unit, float quantity)
        {
            DS_StorageItem item = new DS_StorageItem
            {
                ID = ID,
                name = name,
                unit = unit,
                quantity = quantity
            };
            items.Add(item);

        }
        public static int GetIdxStorageID(string ID, List<DS_StorageItem> items)
        {
            return items.FindIndex(item => item.ID == ID);
        }
        public static void UpdateStorageItemQuantityByIdx(List<DS_StorageItem> items, int idx, float quantity)
        {
            items[idx].quantity += quantity;
        }
        public static void DoAddUpdateStorageItem(List<DS_StorageItem> items, string ID, string name,
            string unit, float quantity, direction dir) // in = 1 ; out = -1 
        {
            int in_out = dir == direction.import ? 1 : dir == direction.export ? -1 : 0;
            int idx = Lib_List.GetIdxStorageID(ID: ID, items: items);
            if (idx == -1) // new one
            {
                Lib_List.AddStorageItem(items: items, ID: ID, name: name, unit: unit, quantity: quantity * in_out);
            }
            else // update existed item
            {
                Lib_List.UpdateStorageItemQuantityByIdx(items: items, idx: idx, quantity: quantity * in_out);
            }
        }

        /*
         * company
         */
        public static void AddCompanyItem(List<DS_Company> items, string ID, string name)
        {
            items.Add(new DS_Company { ID = ID, name = name });
        }
        public static int GetIdxCompanyItemByID(string ID, List<DS_Company> items)
        {
            return items.FindIndex(item => item.ID == ID);
        }

        public static void UpdateCompanyByIdx(List<DS_Company> items, int idx, string name)
        {
            items[idx].name = name;
        }

        public static bool DoAddUpdateCompany(List<DS_Company> items, string ID, string name)
        {
            int idx = Lib_List.GetIdxCompanyItemByID(ID, items);

            if (idx > -1)
            {
                bool confirm = Lib_Message.ShowMessageBox(
                    mStr: "Trùng mã, vẫn tiếp tục?",
                    mIcon: MessageBoxIcon.Question,
                    mbutton: MessageBoxButtons.OKCancel
                );

                if (!confirm)
                    return false;

                Lib_List.UpdateCompanyByIdx(items, idx, name);
            }
            else
            {
                Lib_List.AddCompanyItem(items, ID, name);
            }

            return true;
        }

        /*
         * do add tax ID
         */

        public static bool doAddTaxID(List<DS_Company> items, string ID, string name)
        {
            int idx = Lib_List.GetIdxCompanyItemByID(ID: ID, items: items);
            if (idx == -1) // new one
            {
                Lib_List.AddCompanyItem(items: items, ID: ID, name: name);
                return true;
            }
            return false;
        }
    }
}
