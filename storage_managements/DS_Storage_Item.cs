﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storage_managements
{
    public class DS_Storage_Item
    {
        public string ID { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public string unit { get; set; }
        public void print_item()
        {
            Console.WriteLine("ID: " + ID);
            Console.WriteLine("Name: " + name);
            Console.WriteLine("unit: " + unit);
            Console.WriteLine("quantity: " + quantity);
        }
    }
}
