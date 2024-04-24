using System;

namespace storage_managements
{
    public class DS_StorageItem
    {
        public string ID { get; set; }
        public string name { get; set; }
        public float quantity { get; set; }
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
