using System;

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
