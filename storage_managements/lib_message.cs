using System.Windows.Forms;

namespace storage_managements
{
    class Lib_Message
    {
        public static bool ShowMessagebox(string mstr = "message", string mtitle = "Quản lý kho",
            MessageBoxButtons mbutton = MessageBoxButtons.OKCancel, MessageBoxIcon micon = MessageBoxIcon.None)
        {
            // Display a message box with Yes/No buttons
            DialogResult result = MessageBox.Show(mstr, mtitle, mbutton, micon);
            return result == DialogResult.OK;
        }
    }
}
