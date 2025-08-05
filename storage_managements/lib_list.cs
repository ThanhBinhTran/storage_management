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
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == ID)
                {
                    return i;
                }
            }
            return -1;
        }
        public static bool DoAddUpdateDatabaseItem(List<DS_StorageItem> items, string ID, string name, string unit)
        {
            int idx = Lib_List.GetIdxDatabaseItemByID(ID: ID, items: items);
            bool result = true;
            if (idx > -1) // existed one
            {
                result = Lib_Message.ShowMessagebox(mstr: "Mã đã được dùng.\nVẫn muốn ghi đè?", mbutton: MessageBoxButtons.OKCancel,
                    micon: MessageBoxIcon.Warning);
            }
            if (result)
            {
                if (idx == -1) // new one
                {
                    Lib_List.AddDatabaseItem(items: items, ID: ID, name: name, unit: unit);
                }
                else // update existed item
                {
                    Lib_List.UpdateDatabaseItemByIdx(items: items, ID: ID, name: name, unit: unit, idx: idx);
                }
                return true;
            }
            else
            {
                return false;
            }

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
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == ID)
                {
                    return i;
                }
            }
            return -1;
        }
        public static void UpdateStorageItemQuantityByIdx(List<DS_StorageItem> items, int idx, float quantity)
        {
            items[idx].quantity += quantity;
        }
        public static void DoAddUpdateStorageItem(List<DS_StorageItem> items, string ID, string name,
            string unit, float quantity, direction dir) // in = 1 ; out = -1 
        {
            int in_out = 0;
            if (dir == direction.import)
            {
                in_out = 1;
            }
            else if (dir == direction.export)
            {
                in_out = -1;
            }
            int idx = Lib_List.GetIdxStorageID(ID: ID, items: items);
            if (idx == -1) // new one
            {
                Lib_List.AddStorageItem(items: items, ID: ID, name: name, unit: unit, quantity: quantity);
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
            DS_Company item = new DS_Company
            {
                ID = ID,
                name = name
            };
            items.Add(item);

        }
        public static int GetIdxCompanyItemByID(string ID, List<DS_Company> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == ID)
                {
                    return i;
                }
            }
            return -1;
        }
        public static void UpdateCompanybyIdx(List<DS_Company> items, int idx, string name)
        {
            items[idx].name = name;
        }

        public static bool DoAddUpdateCompany(List<DS_Company> items, string ID, string name)
        {
            int idx = Lib_List.GetIdxCompanyItemByID(ID: ID, items: items);
            bool result = false;
            if (idx > -1)
            {
                result = Lib_Message.ShowMessagebox(mstr: "Trùng mã, vẫn tiếp tục?", micon: System.Windows.Forms.MessageBoxIcon.Question,
                    mbutton: System.Windows.Forms.MessageBoxButtons.OKCancel);

            }
            if (result)
            {
                return false;
            }
            if (idx == -1) // new one
            {
                Lib_List.AddCompanyItem(items: items, ID: ID, name: name);
            }
            else // update existed item
            {
                Lib_List.UpdateCompanybyIdx(items: items, idx: idx, name: name);
            }
            return true;
        }

        /*
         * do add tax ID
         */

        public static void AddTaxID(List<DS_Company> items, string ID, string name)
        {
            int idx = Lib_List.GetIdxCompanyItemByID(ID: ID, items: items);
            if (idx == -1) // new one
            {
                Lib_List.AddCompanyItem(items: items, ID: ID, name: name);
            }
        }
    }
}
