using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storage_managements
{
    public class DS_Company
    {
        public string ID { get; set; }
        public string name { get; set; }

        public void print_item()
        {
            Console.WriteLine("ID: " + ID);
            Console.WriteLine("Name: " + name);
        }
    }
}
