﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP
{
  public static class clsDatetimeFormat
    {
      public static int GetWeekOfMonth(DateTime date)
      {
          DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);

          while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
              date = date.AddDays(1);

          return (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
      } 
    }
}
