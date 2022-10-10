using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.Commands
{
    public static class Books
    {
        public static string GetAnswer()
        {
            var dayofweek = (int)DateTime.Now.AddHours(-6).DayOfWeek;
            string result = "";
            if (dayofweek == 0) result = "Сегодня фарми все!"; //вс
            if (dayofweek == 1 || dayofweek == 4) result = "свобода, процветание, бренность"; //пн, чт
            if (dayofweek == 2 || dayofweek == 5) result = "борьба, усердие, изящество"; //вт, пт
            if (dayofweek == 3 || dayofweek == 6) result = "поэзия, золото, свет"; //ср, сб
            return result;
        }
    }
}
