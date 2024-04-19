using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storage_managements
{
    class lib_list
    {
        /*
         * database items
         */
        public static void add_database_item(List<DS_Database_Item> items, string ID, string name, string unit)
        {
            DS_Database_Item item = new DS_Database_Item { 
                ID = ID,
				name = name,
				unit = unit
            };
            items.Add(item);
        }
        public static void update_database_item_by_idx(List<DS_Database_Item> items, string ID, string name, string unit, int idx)
        {
            items[idx].ID = ID;
            items[idx].name = name;
            items[idx].unit = unit;
        }
        public static int get_idx_database_item_by_ID(string ID, List<DS_Database_Item> items)
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
        public static void do_add_update_database_item(List<DS_Database_Item> items, string ID, string name, string unit)
        {
            int idx = lib_list.get_idx_database_item_by_ID(ID: ID, items: items);
            if (idx == -1) // new one
            {
                lib_list.add_database_item(items: items, ID: ID, name: name, unit: unit);
            }
            else // update existed item
            {
                lib_list.update_database_item_by_idx(items: items, ID: ID, name: name, unit: unit, idx: idx);
            }
        }
        public static void print_database_items(List<DS_Database_Item> items)
        {
            Console.WriteLine("list size: " + items.Count);
            foreach (DS_Database_Item item in items)
            {
                item.print_item();
            }
        }

        /*
         * storage items
         */
        public static void add_storage_item(List<DS_Item> items, string ID, string name, string unit, int quantity)
        {
            DS_Database_Item item = new DS_Database_Item
            {
                ID = ID,
                name = name,
                unit = unit
            };
            DS_Item storage_item = new DS_Item
            {
                storage_item = item,
                quantity = quantity,
            };
            items.Add(storage_item);

        }
        public static int get_idx_storage_item_by_ID(string ID, List<DS_Item> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].storage_item.ID == ID)
                {
                    return i;
                }
            }
            return -1;
        }
        public static void update_storage_item_quantity_by_idx(List<DS_Item> items, int idx, int quantity)
        {
            items[idx].quantity += quantity;
        }
        public static void do_add_update_storage_item(List<DS_Item> items, string ID, string name, 
            string unit, int quantity, int in_out) // in = 1 ; out = -1 
        {
            int idx = lib_list.get_idx_storage_item_by_ID(ID: ID, items: items);
            if (idx == -1) // new one
            {
                lib_list.add_storage_item(items: items, ID: ID, name: name, unit: unit, quantity: quantity);
            }
            else // update existed item
            {
                lib_list.update_storage_item_quantity_by_idx(items: items, idx: idx, quantity: quantity * in_out);
            }
        }
        public static void print_storage_items(List<DS_Item> items)
        {
            Console.WriteLine("list size: " + items.Count);
            foreach (DS_Item item in items)
            {
                item.print_item();
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
            int idx = lib_list.get_idx_conpany_item_by_ID(ID: ID, items: items);
            bool result = false;
            if(idx > -1)
            {
                result = lib_message.show_messagebox(mstr: "Trùng mã, vẫn tiếp tục?", micon: System.Windows.Forms.MessageBoxIcon.Question,
                    mbutton: System.Windows.Forms.MessageBoxButtons.OKCancel);
                
            }
            if (result)
            {
                return false;
            }
            if (idx == -1) // new one
            {
                lib_list.add_conpany_item(items: items, ID: ID, name: name);
            }
            else // update existed item
            {
                lib_list.update_conpany_by_idx(items: items, idx: idx, name: name);
            }
            return true;
        }
        public static void print_conpany_items(List<DS_Company> items)
        {
            Console.WriteLine("list size: " + items.Count);
            foreach (DS_Company item in items)
            {
                item.print_item();
            }
        }
    }
}
