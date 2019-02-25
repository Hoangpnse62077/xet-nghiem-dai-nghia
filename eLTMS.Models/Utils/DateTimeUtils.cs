using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.Models.Utils
{
    public class DateTimeUtils
    {
        // DucBM
        public static string ConvertDateToString(DateTime? dateTime)
        {
            if (dateTime==null)
            {
                return null;
            }
            var dt = (DateTime)dateTime;
            var s = dt.ToString("yyyy-MM-dd");
            return s;
        }
        // DucBM
        public static DateTime? ConvertStringToDate(string s)
        {
            try
            {
                DateTime dt = DateTime.Parse(s);
                return (DateTime?)dt;
            } catch (Exception ex)
            {
                // 
            }
            return null;
        }
        public static string ConvertTimeSpanToShortHour(TimeSpan? ts)
        {
            string s = null;
            if (ts==null)
            {
                return null;
            }
            try
            {
                var ts2 = (TimeSpan)ts;
                s = string.Format("{0}:{1:D2}", ts2.Hours, ts2.Minutes);
            } catch(Exception ex)
            {
                return null;
            }
            return s;
        }
        public static string ConvertSecondToShortHour(int second)
        {
            int hour = second / 60 / 60;
            int min = second / 60 % 60;
            string s = string.Format("{0}:{1:D2}", hour, min);
            return s;
        }
    }
}
