using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Modules
{
    public class CardsDiscord
    {
        public static string AddCard(string url, string name)
        {
            if (Logic.AddCardDS(url, name)) return $"Карточка \"{name}\" добавлена";
            return $"Карточка с именем \"{name}\" уже есть";
        }

        public static string RemoveCard(string name)
        {
            if (Logic.RemoveCardDS(name)) return $"Карточка \"{name}\" удалена";
            return $"Карточки с именем \"{name}\" нет";
        }

        public static string SelectUserCard(string name, string discordId)
        {
            if (Logic.SelectUserCardDS(name, discordId)) return $"Карточка профиля изменена на \"{name}\"";
            return "Тебя или карточки нет. Если тебя нет, то пропиши !reg";
        }

        public static string GetCardUrlUser(string discordId)
        {
            return Logic.GetCardUrlUserDS(discordId);
        }

        public static string GetAllCard()
        {
            return Logic.GetAllCardDS();
        }
    }
}
