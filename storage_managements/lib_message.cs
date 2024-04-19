using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace storage_managements
{
    class lib_message
    {
        public static bool show_messagebox(string mstr = "message" , string mtitle = "Quản lý kho", 
            MessageBoxButtons mbutton = MessageBoxButtons.OKCancel, MessageBoxIcon micon = MessageBoxIcon.None)
        {
            // Display a message box with Yes/No buttons
            DialogResult result = MessageBox.Show(mstr, mtitle, mbutton, micon);

            if (result == DialogResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
