using System.Windows.Forms;

namespace storage_managements
{
    class Lib_Message
    {
        public static bool ShowMessageBox(string mStr = "message", string mtitle = "Quản lý kho",
            MessageBoxButtons mbutton = MessageBoxButtons.OKCancel, MessageBoxIcon mIcon = MessageBoxIcon.None)
        {
            // Display a message box with Yes/No buttons
            DialogResult result = MessageBox.Show(mStr, mtitle, mbutton, mIcon);
            return result == DialogResult.OK;
        }
    }
}
