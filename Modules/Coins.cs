using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Modules
{
    public static class Coins
    {
        private static string userNotFound = "Тебя нет. Пропиши !reg";
        public static string Balance(string user, string nickname)
        {
            if (!Logic.CheckUser(user)) return userNotFound;
            return $"{nickname}, у тебя оказывается есть гемы - {Logic.GetCoin(user)}";
        }

        public static string GiveCoinAdmin(string user, string nickname, int coin)
        {
            if (!Logic.CheckUser(user)) return userNotFound;
            Logic.SetCoin(user, coin);
            return $"{nickname}, теперь у тебя - {Logic.GetCoin(user)} гемов";
        }
        public static string GiveCoin(string userSender, string user, string nickname, int coin)
        {
            if (!Logic.CheckUser(userSender)) return userNotFound;
            if (!Logic.CheckUser(user)) return userNotFound;

            if (Logic.GetUser(userSender).Coin < coin || coin < 0) return "Недостаточно гемов";

            Logic.SetCoin(userSender, -coin);
            Logic.SetCoin(user, coin);
            return $"{nickname} получил(а) гемы";
        }
        public static string DailyCoin(string user, string nickname)
        {
            if (!Logic.CheckUser(user)) return userNotFound;
            var userBd = Logic.GetUser(user);
            var expCoefficient = 1 + 0.25 * (Logic.GetRank(user).Rank);
            var coin = (int)(480 * expCoefficient);
            if (userBd.LastTimely.DayOfWeek != DateTime.Now.DayOfWeek)
            {
                userBd.Coin += coin;
                userBd.LastTimely = DateTime.Now;
                Logic.SaveDB();
                return $"{nickname}, ты получил(а) свои {coin} гемов";
            }
            return $"{nickname}, сегодня ты уже забирал(а) гемы";
        }
    }
}
