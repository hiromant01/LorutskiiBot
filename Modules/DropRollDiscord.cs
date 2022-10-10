using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Modules
{
    public class DropRollDiscord
    {
        public static string AddDropRoll(string tableName, string url, string name)
        {
            if (Logic.AddDropRollDS(tableName, url, name)) return $"Дроп \"{name}\" добавлен в таблицу \"{tableName}\"";
            return $"Дроп с именем \"{name}\" в таблице \"{tableName}\" уже есть";
        }

        public static string RemoveDropRoll(string tableName, string name)
        {
            if (Logic.RemoveDropRollDS(tableName, name)) return $"Дроп \"{name}\" удален из таблицы \"{tableName}\"";
            return $"Дропа с именем \"{name}\" в таблице \"{tableName}\" нет";
        }
    }
}
