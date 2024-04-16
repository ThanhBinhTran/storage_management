using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace storage_managements
{
    class lib_text
    {
        public static bool is_string_empty(string instr)
        {
            if (instr.Trim().Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }    
        }

        public static string get_textbox_text(TextBox tb)
        {
            return tb.Text.Trim();
        }
        public static bool is_textbox_empty(TextBox tb)
        {
            if (tb.Text.Trim().Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void display_debug(Label label, string msg)
        {
            label.Text = msg;
        }
    }
}
