using System;
using System.Collections.Generic;

namespace storage_managements
{
    class Lib_DateTime
    {
        public static DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
        public static string GetDateTime(DateTime dt)
        {
            return string.Format("{0:yyyy/MM/dd HH:mm:ss}", dt);
        }

        public static string GetDateOnly(DateTime dt)
        {
            return string.Format("{0:yyyy/MM/dd}", dt);
        }

        public static string GetYear()
        {
            return string.Format("{0:yyyy}", DateTime.Now);
        }
        public static string GetMonth()
        {
            return string.Format("{0:MM}", DateTime.Now);
        }

        public static string GetIDByDateTime()
        {
            return string.Format("{0:yy_MM_dd_HH_mm_ssf}", DateTime.Now);
        }
        public static string GetIDByTime()
        {
            return string.Format("{0:HHmm}", DateTime.Now);
        }

        public static List<DateTime> GetAllDatesBetween(DateTime startDate, DateTime endDate)
        {
            var dates = new List<DateTime>();

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                dates.Add(date);
                Console.WriteLine(date.ToString());
                Console.WriteLine(Lib_DateTime.DateToTransactionPath(dt: date));
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

        public static string GetPdfTransactionPathFromCurrentDate(string seperateby = "")
        {
            string pdf_filename = string.Format("{0}_{1}", GetIDByDateTime(), seperateby);
            return string.Format(@"{0}{1}.pdf", Program_Parameters.pdfPath, pdf_filename);
        }
        public static string GetPdfStoragePathFromCurrentDate()
        {
            string pdf_filename = string.Format("Kho_{0}", GetIDByDateTime());
            return string.Format(@"{0}{1}.pdf", Program_Parameters.pdfPath, pdf_filename);
        }
    }
}
