using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storage_managements
{
    class lib_list
    {
        public static void add_database_item(List<DataStruct_Database_Item> item_List, string ID, string name, string unit)
        {
            DataStruct_Database_Item item = new DataStruct_Database_Item { 
                ID = ID,
				name = name,
				unit = unit
            };
            item_List.Add(item);
        }

        public static void update_database_item_by_idx(List<DataStruct_Database_Item> item_List, string ID, string name, string unit, int idx)
        {
            item_List[idx].ID = ID;
            item_List[idx].name = name;
            item_List[idx].unit = unit;
        }
        public static int get_idx_database_item_by_ID(string ID, List<DataStruct_Database_Item> items)
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
        public static void print_database_items(List<DataStruct_Database_Item> item_List)
        {
            Console.WriteLine("list size: " + item_List.Count);
            foreach (DataStruct_Database_Item item in item_List)
            {
                item.print_item();
            }
        }


        public static void add_storage_item(List<DataStruct_Storage_Item> items, string ID, string name, string unit, int quantity)
        {
            DataStruct_Database_Item item = new DataStruct_Database_Item
            {
                ID = ID,
                name = name,
                unit = unit
            };
            DataStruct_Storage_Item storage_item = new DataStruct_Storage_Item
            {
                storage_item = item,
                quantity = quantity,
            };
            items.Add(storage_item);

        }
        public static int get_idx_storage_item_by_ID(string ID, List<DataStruct_Storage_Item> items)
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

        public static void update_storage_item_quantity_by_idx(List<DataStruct_Storage_Item> items, int idx, int quantity)
        {
            items[idx].quantity += quantity;
        }

        public static void record_storage_item_transaction(List<DataStruct_Storage_Item> items, string ID, string name, 
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
    }
}
