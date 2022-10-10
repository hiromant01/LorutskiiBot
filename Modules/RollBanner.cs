using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Modules
{
    public static class RollBanner
    {
        private class RollObject
        {
            public string Name { get; set; }
            public string Url { get; set; }
            public int Exp { get; set; }
            public int Star { get; set; }
            public string Emoji { get; set; }
            public RollObject((string name, string url) dbRoll, int star, int exp, string emoji)
            {
                Name = dbRoll.name;
                Url = dbRoll.url;
                Exp = exp;
                Star = star;
                Emoji = emoji;
            }

        }
        private static Dictionary<int, int> expRoll = new Dictionary<int, int>()
        {
            {6, 1000},
            {5, 100},
            {4, 50},
            {3, 5},
        };
        private static readonly List<string> phrases = new List<string> { "Вот мы и вновь встретились - ", "Твое упорство восхищает - ", "Хорошая попытка - ",
                                                        "А ты умеешь приятно удивлять - ", "Это же твой путь к успеху - ", "Это же - ", "Смотри - ",
                                                        "Ну как обычно - ", "Не верю глазам, это - ", "Ух ты, это - ", "Повезло-повезло, это - ",
                                                        "Просто - ", "Ну вот, опять - " };
        private static Random rnd = new Random();

        public static bool IsFreeRoll = false;
        public static string Roll(string user, string nickname, int count)
        {
            if (count == 0) count++;
            if (!IsFreeRoll)
            {
                if (!Logic.CheckUser(user)) return "Тебя нет";
                if (count > 10 || count < 0) return "Все хорошо?";
                if (!Logic.CheckCoins(user, 160 * count)) return $"{nickname}, недостаточно гемов";
            }

            var result = phrases[rnd.Next(0, phrases.Count - 1)] + "\n";
            for (var i = 0; i < count; i++)
            {
                var roll = RollGeneral(user);
                result += $"{roll.Emoji}{roll.Name}{roll.Emoji} / \n";
            }
            result = result.Remove(result.Length - 3, 2);
            return result;
        }
        public static (List<(string url, int star)> drop, string result) RollDS(string user, string nickname, int count)
        {
            if (count == 0) count++;
            var result = new List<(string url, int star)>();

            if (!IsFreeRoll)
            {
                if (!Logic.CheckUser(user)) return (result, "Тебя нет. Пропиши !reg");
                if (count > 10 || count < 0) return (result, "Все хорошо?");
                if (!Logic.CheckCoins(user, 160 * count)) return (result, $"{nickname}, недостаточно гемов");
            }

            for (var i = 0; i < count; i++)
            {
                var roll = RollGeneral(user);
                result.Add((roll.Url, roll.Star));
            }
            return (result, "");
        }
        private static RollObject RollGeneral(string user)
        {
            var rollCount = Logic.GetRollCount(user);
            var roll = (rollCount > 75 && rollCount < 90) ? Roll(1000) :
                (rollCount >= 89) ? Roll(100) :
                (rollCount  % 10 == 0) ? Roll(1200) :
                Roll();

            if (!IsFreeRoll)
            {
                Logic.RollCountAdd(user);
                if (roll.Star == 5)
                    Logic.RollCountReset(user);

                Logic.SetCoin(user, -160);
                Logic.SetExp(user, roll.Exp);
            }
            return roll;
        }

        private static RollObject Roll(int rndRange = 10000)
        {
            var resultKrutka = rnd.Next(1, rndRange);
            return
                resultKrutka == 6666 ? new RollObject(("https://cdn.discordapp.com/attachments/923671441193443348/947426501291307038/pngwing.com.png", "Даша❤❤❤❤❤"),
                    6, expRoll[6], " lorutsEZ lorutsEZ lorutsEZ ") :
                resultKrutka == 5555 ? new RollObject(("https://cdn.discordapp.com/attachments/923671441193443348/947423174281273364/9b66247308932efd.png", "Трусики лучшей вайфу"),
                    6, expRoll[6], " lorutsEZ lorutsEZ lorutsEZ ") :
                resultKrutka < 50 ? new RollObject(Logic.GetRandomDropRollDS("FiveStarCharacters"), 5, expRoll[5], " lorutsEZ ") :
                resultKrutka < 100 ? new RollObject(Logic.GetRandomDropRollDS("FiveStarWeapons"), 5, expRoll[5], " lorutsEZ ") :
                resultKrutka < 600 ? new RollObject(Logic.GetRandomDropRollDS("FourStarCharacters"), 4, expRoll[4], " lorutsPog ") :
                resultKrutka < 1200 ? new RollObject(Logic.GetRandomDropRollDS("FourStarWeapons"), 4, expRoll[4], " lorutsPog ") :
                new RollObject(Logic.GetRandomDropRollDS("ThreeStarWeapons"), 3, expRoll[3], "");
        }
    }
}
