using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace storage_managements
{
    class Lib_FormText
    {
        public static bool IsStringEmpty(string instr)
        {
            return instr.Trim().Count() == 0;
        }

        public static string GetTextboxText(TextBox tb)
        {
            return tb.Text.Trim();
        }
        public static bool IsTextboxEmpty(TextBox tb)
        {
            BackColorTextBox(tb);
            return IsStringEmpty(tb.Text);
        }

        public static void DisplayNotification(Label label, string msg)
        {
            label.Text = string.Format("Thông báo: {0}", msg);
        }

        public static void BackColorTextBox(TextBox tb)
        {
            tb.BackColor = IsStringEmpty(tb.Text) ? Color.Plum : Color.White;
        }
        public static void ClearColorTextBox(TextBox tb)
        {
            tb.BackColor = Color.White;
        }
    }
}
