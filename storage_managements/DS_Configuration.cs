using System.Collections.Generic;

namespace storage_managements
{
    class DS_Configuration
    {
        public float page_top { get; set; }
        public float page_bottom { get; set; }
        public float page_left { get; set; }
        public float page_right { get; set; }

        public float[] pdfTableWidths { get; set; }
    //new float[] { 24.0f, 26f, 7.0f, 28f, 15f, 12.5f };

    }
}
