﻿using System.Collections.Generic;
using System.Windows.Forms;
namespace storage_managements
{
    class lib_List
    {
        /*
         * database items
         */
        public static void add_database_item(List<DS_StorageItem> items, string ID, string name, string unit)
        {
            DS_StorageItem item = new DS_StorageItem
            {
                ID = ID,
                name = name,
                unit = unit
            };
            items.Add(item);
        }
        public static void update_database_item_by_idx(List<DS_StorageItem> items, string ID, string name, string unit, int idx)
        {
            items[idx].ID = ID;
            items[idx].name = name;
            items[idx].unit = unit;
        }
        public static int get_idx_database_item_by_ID(string ID, List<DS_StorageItem> items)
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
        public static bool do_add_update_database_item(List<DS_StorageItem> items, string ID, string name, string unit)
        {
            int idx = lib_List.get_idx_database_item_by_ID(ID: ID, items: items);
            bool result = true;
            if (idx > -1) // existed one
            {
                result = lib_Message.show_messagebox(mstr: "Mã đã được dùng.\nVẫn muốn ghi đè?", mbutton: MessageBoxButtons.OKCancel,
                    micon: MessageBoxIcon.Warning);
            }
            if (result)
            {
                if (idx == -1) // new one
                {
                    lib_List.add_database_item(items: items, ID: ID, name: name, unit: unit);
                }
                else // update existed item
                {
                    lib_List.update_database_item_by_idx(items: items, ID: ID, name: name, unit: unit, idx: idx);
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
        public static void add_storage_item(List<DS_StorageItem> items, string ID, string name, string unit, int quantity)
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
        public static int get_idx_storage_item_by_ID(string ID, List<DS_StorageItem> items)
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
        public static void update_storage_item_quantity_by_idx(List<DS_StorageItem> items, int idx, int quantity)
        {
            items[idx].quantity += quantity;
        }
        public static void do_add_update_storage_item(List<DS_StorageItem> items, string ID, string name,
            string unit, int quantity, direction dir) // in = 1 ; out = -1 
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
            int idx = lib_List.get_idx_storage_item_by_ID(ID: ID, items: items);
            if (idx == -1) // new one
            {
                lib_List.add_storage_item(items: items, ID: ID, name: name, unit: unit, quantity: quantity);
            }
            else // update existed item
            {
                lib_List.update_storage_item_quantity_by_idx(items: items, idx: idx, quantity: quantity * in_out);
            }
        }

        /*
         * conpanies
         */
        public static void add_conpany_item(List<DS_Company> items, string ID, string name)
        {
            DS_Company item = new DS_Company
            {
                ID = ID,
                name = name
            };
            items.Add(item);

        }
        public static int get_idx_conpany_item_by_ID(string ID, List<DS_Company> items)
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
        public static void update_conpany_by_idx(List<DS_Company> items, int idx, string name)
        {
            items[idx].name = name;
        }

        public static bool do_add_update_conpany(List<DS_Company> items, string ID, string name)
        {
            int idx = lib_List.get_idx_conpany_item_by_ID(ID: ID, items: items);
            bool result = false;
            if (idx > -1)
            {
                result = lib_Message.show_messagebox(mstr: "Trùng mã, vẫn tiếp tục?", micon: System.Windows.Forms.MessageBoxIcon.Question,
                    mbutton: System.Windows.Forms.MessageBoxButtons.OKCancel);

            }
            if (result)
            {
                return false;
            }
            if (idx == -1) // new one
            {
                lib_List.add_conpany_item(items: items, ID: ID, name: name);
            }
            else // update existed item
            {
                lib_List.update_conpany_by_idx(items: items, idx: idx, name: name);
            }
            return true;
        }

    }
}
