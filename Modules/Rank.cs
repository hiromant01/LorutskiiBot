using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Modules
{
    public class Rank
    {
        private static string userNotFound = "Тебя нет.";
        public static string MyRank(string user, string nickname)
        {
            if (!Logic.CheckUser(user)) return userNotFound;
            var rank = Logic.GetRank(user);
            return $"{nickname}, сейчас у тебя - {rank.Rank} ранг и {rank.UserExp}/{rank.ExpPerRank} опыта";
        }
        public static string RangTop()
        {
            var users = Logic.GetAllUsers();
            users = users.OrderByDescending(v => v.Experience).Select(v => v).ToList();
            var result = "```Топ по рангам:\n";
            for (var i = 0; i < 4; i++)
            {
                var rank = Logic.GetRank(users[i].Nickname);
                result += $"{users[i].Nickname} - {rank.Rank} ранг, {rank.UserExp} опыта\n";
            }
            var lastRank = Logic.GetRank(users[4].Nickname);
            result += $"{users[4].Nickname} - {lastRank.Rank} ранг, {lastRank.UserExp} опыта```";
            return result;
        }
        public static string GiveExpAdmin(string user, string nickname, int exp)
        {
            if (!Logic.CheckUser(user)) return userNotFound;

            Logic.SetExp(user, exp);
            var rank = Logic.GetRank(user);
            return $"{nickname}, теперь у тебя - {rank.Rank} ранг и {rank.UserExp}/{rank.ExpPerRank} опыта";
        }
    }
}
