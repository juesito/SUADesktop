using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    class DatesHelper
    {
        public static long DateDiffInYears(DateTime dt1, DateTime dt2)
        {
            return DateAndTime.DateDiff(DateInterval.Year,
                dt1, dt2, FirstDayOfWeek.System, FirstWeekOfYear.System);
        }

        public static long DateDiffInMonths(DateTime dt1, DateTime dt2)
        {
            return DateAndTime.DateDiff(DateInterval.Month,
                dt1, dt2, FirstDayOfWeek.System, FirstWeekOfYear.System);
        }
    }
}
