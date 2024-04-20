using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storage_managements
{
    class lib_date_time
    {
        public static DateTime get_currenttime()
        {
            return DateTime.Now;
        }
        public static string get_year()
        {
            return string.Format("{0:yyyy}", DateTime.Now);
        }
        public static string get_month()
        {
            return string.Format("{0:MM}", DateTime.Now);
        }
        public static string get_day()
        {
            return string.Format("{0:dd}", DateTime.Now);
        }

        public static string getID_byDateTime()
        {
            return string.Format("{0:yyMMddHHmmssf}", DateTime.Now);
        }

        public static string get_current_transaction_date()
        {
            string year = lib_date_time.get_year();
            string month = lib_date_time.get_month();
            string day = lib_date_time.get_day();
            string transaction_filename = year + month + day + Program_Parameters.filename_transaction;
            return string.Format("{0}//{1}//{2}//{3}",Program_Parameters.dataPath, year, month, transaction_filename);
        }
    }
}
