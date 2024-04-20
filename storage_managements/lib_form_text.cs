using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace storage_managements
{
    class lib_form_text
    {
        public static bool is_string_empty(string instr)
        {
            return instr.Trim().Count() == 0;
        }

        public static string get_textbox_text(TextBox tb)
        {
            return tb.Text.Trim();
        }
        public static bool is_textbox_empty(TextBox tb)
        {
            return tb.Text.Trim().Count() == 0;
        }

        public static void display_debug(Label label, string msg)
        {
            label.Text = msg;
        }

        public static void color_textbox(TextBox tb)
        {
            if (is_textbox_empty(tb))
            {
                tb.BackColor = Color.Plum;
            }
            else
            {
                tb.BackColor = Color.White;
            }
        }

    }
}
