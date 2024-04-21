using System;
using System.Collections.Generic;

namespace storage_managements
{
    class lib_date_time
    {
        public static DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
        public static string GetTimeOnly(DateTime dt)
        {
            return string.Format("{0:yyyy/MM/dd HH:mm:ss}", dt);
        }
        public static string GetYear()
        {
            return string.Format("{0:yyyy}", DateTime.Now);
        }
        public static string GetMonth()
        {
            return string.Format("{0:MM}", DateTime.Now);
        }
        public static string GetDay()
        {
            return string.Format("{0:dd}", DateTime.Now);
        }

        public static string GetIDByDateTime()
        {
            return string.Format("{0:yyMMddHHmmssf}", DateTime.Now);
        }


        public static List<DateTime> GetAllDatesBetween(DateTime startDate, DateTime endDate)
        {
            var dates = new List<DateTime>();

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                dates.Add(date);
                Console.WriteLine(date.ToString());
                Console.WriteLine(lib_date_time.DateToTransactionPath(dt: date));
            }
            return dates;
        }

        public static string DateToTransactionPath(DateTime dt)
        {
            string year = string.Format("{0:yyyy}", dt);
            string month = string.Format("{0:MM}", dt);
            string day = string.Format("{0:dd}", dt);
            string transaction_filename = year + month + day + Program_Parameters.filename_transaction;
            return string.Format(@"{0}{1}\\{2}\\{3}", Program_Parameters.dataPath, year, month, transaction_filename);
        }
        public static string GetTransactionPathFromCurrentDate()
        {
            return DateToTransactionPath(dt: DateTime.Now);

        }
    }
}
