using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storage_managements
{
    public class DS_Item
    {
        public DS_Database_Item storage_item;
        public int quantity { get; set; }

        public DateTime delivery { get; set; }
        public DateTime receipt { get; set; }

        public void print_item()
        {
            storage_item.print_item();
            Console.WriteLine("quanlity: " + quantity);
            Console.WriteLine("receipt: " + receipt);
            Console.WriteLine("delivery: " + delivery);
        }
    }
}
