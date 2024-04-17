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
            return DateTime.Now.Year.ToString();
        }
        public static string get_month()
        {
            return DateTime.Now.Month.ToString();
        }
        public static string get_day()
        {
            return DateTime.Now.Day.ToString();
        }

        public static string getID_byDateTime()
        {
            return string.Format("{0:yyMMddHHmmssf}", DateTime.Now);
        }
    }
}
