using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storage_managements
{
    public enum direction
    {
        export,
        import
    }
    public class DS_Transaction
    {
        public string ID { get; set; }
        public string company_name { get; set; }

        public List<DS_Storage_prepare_Item> transaction_items { get; set; }

        public DateTime transaction_time { get; set; }
        public direction transaction_direction { get; set; }

        public void print_item()
        {
            Console.WriteLine("ID: " + ID);
            Console.WriteLine("Name: " + company_name);
        }
    }
}
