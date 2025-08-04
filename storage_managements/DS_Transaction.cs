using System;
using System.Collections.Generic;

namespace storage_managements
{
    public enum direction
    {
        export,
        import,
        none
    }
    public class DS_Transaction
    {
        public string ID { get; set; }
        public string company_name { get; set; }

        public string taxID { get; set; }

        public List<DS_StorageItem> transaction_items { get; set; }

        public direction transaction_direction { get; set; }
        public DateTime transaction_time { get; set; }


        public void print_item()
        {
            Console.WriteLine("ID: " + ID);
            Console.WriteLine("Name: " + company_name);
        }
    }
}
