using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storage_managements
{
    public class DS_Transaction_Grid
    {
        public string ID { get; set; }
        public string company_name { get; set; }

        public string item_ID { get; set; }
        public string item_name { get; set; }

        public int item_quantity { get; set; }
        public string item_unit { get; set; }
        

        public DateTime transaction_time { get; set; }
        public direction transaction_direction { get; set; }

        public void print_item()
        {
            Console.WriteLine("ID: " + ID);
            Console.WriteLine("Name: " + company_name);
        }
    }
}
