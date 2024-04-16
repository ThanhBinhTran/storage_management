using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storage_managements
{
    class lib_list
    {
        public static void add_item(List<DataStruct_Item> item_List, string ID, string name, string unit)
        {
            DataStruct_Item item = new DataStruct_Item { 
                ID = ID,
				name = name,
				unit = unit
            };
            item_List.Add(item);
        }

        public static void update_item_idx(List<DataStruct_Item> item_List, string ID, string name, string unit, int idx)
        {
            item_List[idx].ID = ID;
            item_List[idx].name = name;
            item_List[idx].unit = unit;
        }
        public static int is_exist_item_list_ID(string ID, List<DataStruct_Item> items)
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
        public static void print_item_list(List<DataStruct_Item> item_List)
        {
            Console.WriteLine("list size: " + item_List.Count);
            foreach (DataStruct_Item item in item_List)
            {
                item.print_item();
            }
        }
    }
}
