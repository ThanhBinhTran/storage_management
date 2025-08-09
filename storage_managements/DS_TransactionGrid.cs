using System;

namespace storage_managements
{
    public class DS_TransactionGrid
    {
        //public string ID { get; set; }
        public DateTime transaction_time { get; set; }

        public string taxID { get; set; }
        public string transaction_direction { get; set; }
        
        public string company_name { get; set; }

        public string item_ID { get; set; }
        public string item_name { get; set; }

        public float item_quantity { get; set; }
        public string item_unit { get; set; }

        
    }
}
