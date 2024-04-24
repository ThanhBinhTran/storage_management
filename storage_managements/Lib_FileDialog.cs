using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storage_managements
{
    class Lib_FileDialog
    {
        public static void OpenPdfFolder()
        {
            System.Diagnostics.Process.Start(Program_Parameters.pdfPath);
        }
    }
}
