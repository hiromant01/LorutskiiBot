using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Modules
{
    public static class Talents
    {
        public static string TalentsHelp()
        {
            return "```Таланты это прокачка тебя!!!\n" +
                "Pyro - Бонус за крутки(Больше опыта)\n" +
                "Cryo - Бонус на таймлу(Больше гемов)\n" +
                "Hydro - Подкрутка на гив(Реже уходишь в минус)\n" +
                "Electro - ()\n" +
                "Anemo - ()\n" +
                "Geo - ()\n" +
                "Dendro - ()```";
        }
        public static string BoyTalents(string discordId, TalentsEnum talents)
        {
            if (!Logic.CheckUser(discordId)) return "тебя нет";
            var user = Logic.GetUser(discordId);
            var coin = user.Coin;
            switch (talents)
            {
                case TalentsEnum.Pyro:




                    break;
                case TalentsEnum.Cryo:
                    break;
                case TalentsEnum.Hydro:
                    break;
            }
            return "";
        }






    }
}
