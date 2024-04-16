using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storage_managements
{
    public class DataStruct_Storage_Transaction
    {
        public string ID { get; set; }
        public string name { get; set; }

        public int quantity { get; set; }
        public string unit { get; set; }

        public DateTime delivery { get; set; }
        public DateTime receipt { get; set; }

        public void print_item()
        {
            Console.WriteLine("ID: " + ID);
            Console.WriteLine("Name: " + name);
            Console.WriteLine("quanlity: " + quantity);
            Console.WriteLine("unit: " + unit);
            Console.WriteLine("receipt: " + receipt);
            Console.WriteLine("delivery: " + delivery);
        }
    }
}
