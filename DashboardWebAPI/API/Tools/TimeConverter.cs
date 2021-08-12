using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashboardService.Tools
{
    public class TimeConverter
    {
        public static int convertMinuteToSeconds(string time)
        {
            string[] t = time.Split(':');
            return Convert.ToInt32(t[0]) * 60 + Convert.ToInt32(t[1]);
        }

        public static string convertSecondsToMinutes(int time)
        {
            int minutes = time / 60;
            int seconds = time % 60;

            return minutes.ToString() + ":" + seconds.ToString();
        }
    }
}