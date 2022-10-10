using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class Logic
    {
        private readonly static Context db = Context.Initialization();
        public static Dictionary<string, string> LinkRequestsDsTwitch = new Dictionary<string, string>();

        public static void SaveDB()
        {
            db.SaveChanges();
        }
        public static bool CheckUser(string user)
        {
            if (user  == "") return false;
            bool result;
            if (Int64.TryParse(user, out var number))
            {
                result = (db.Users.Where(v => v.DSId == user).FirstOrDefault() != null);
                if (result == false) result = (db.Users.Where(v => v.TwitchId == user).FirstOrDefault() != null);
            } else
                result = (db.Users.Where(v => v.Nickname == user).FirstOrDefault() != null);
            return result;
        }

        public static User GetUser(string user)
        {
            User result;
            if (Int64.TryParse(user, out var number))
            {
                result = db.Users.Where(v => v.DSId == user).FirstOrDefault();
                if (result == null) result = db.Users.Where(v => v.TwitchId == user).FirstOrDefault();
            } else
                result  = db.Users.Where(v => v.Nickname == user).FirstOrDefault();
            return result;
        }


        public static List<User> GetAllUsers()
        {
            var result = new List<User>();
            foreach (var user in db.Users)
                result.Add(user);
            return result;
        }
        public static bool CheckCoins(string user, int coin) => GetUser(user).Coin >= coin; //хватает ли "nickname" "coin" на покупку
        public static (int Rank, int UserExp, int ExpPerRank) GetRank(string user)
        {
            var userExp = GetUser(user).Experience;
            var rank = 0;
            var exp = 100;
            while (true)
            {
                userExp -= exp;
                if (userExp < 0) break;
                rank++;
                exp += 150;
            }
            var result = (rank, userExp + exp, exp);
            return result;
        }
        public static int GetCoin(string user) => GetUser(user).Coin;
        public static void SetCoin(string user, int coin)
        {
            GetUser(user).Coin += coin;
            db.SaveChanges();
        }
        public static void SetExp(string user, int exp)
        {
            GetUser(user).Experience += exp;
            db.SaveChanges();
        }

        //только твич
        public static void NewUserTwitch(string nickname, string twitchId)
        {
            if (!CheckUser(twitchId))
            {
                var user = new User { Nickname = nickname, TwitchId = twitchId, Identifier = "", Coin = 320, LastCommand = DateTime.MinValue, DSId = "" };
                db.Users.Add(user);
                db.UserTalents.Add(new UserTalent() { UserId = db.Users.Last().Id });
                db.SaveChanges();
            }
        }
        public static string ConnectDsTwitch(string twitchId, string identifier)
        {
            var user = GetUser(twitchId);
            if (user.DSId != "") return "Ты уже связан";
            if (!LinkRequestsDsTwitch.ContainsKey(identifier)) return "Не знаю";
            user.DSId = LinkRequestsDsTwitch[identifier];
            user.Identifier = identifier;
            db.SaveChanges();
            return "Ты связан";
        }

        public static void RollCountAdd(string user)
        {
            GetUser(user).RollCount++;
            db.SaveChanges();
        }
        public static void RollCountReset(string user)
        {
            GetUser(user).RollCount = 0;
            db.SaveChanges();
        }
        public static int GetRollCount(string user)
        {
            return GetUser(user).RollCount;
        }

        //только дискорд команды дальше
        public static bool AddCardDS(string url, string name)
        {
            if (db.Cards.Where(x => x.Name == name).ToList().Count() != 0) return false;
            db.Cards.Add(new Card() { Url = url, Name = name });
            db.SaveChanges();
            return true;
        }
        public static bool RemoveCardDS(string name)
        {
            if (db.Cards.Where(x => x.Name == name).ToList().Count() == 0) return false;
            var card = db.Cards.Where(x => x.Name == name).First();
            foreach (var user in db.Users)
            {
                if (user.CardId == card.Id) user.CardId = 0;
            }
            db.Remove(card);
            db.SaveChanges();
            return true;
        }
        public static bool SelectUserCardDS(string name, string discordId)
        {
            if (!CheckUser(discordId)) return false;
            if (db.Cards.Where(x => x.Name == name).ToList().Count() == 0) return false;
            var user = GetUser(discordId);
            var card = db.Cards.Where(x => x.Name == name).First();
            user.CardId = card.Id;
            db.SaveChanges();
            return true;
        }
        public static string GetCardUrlUserDS(string discordId)
        {
            var user = GetUser(discordId);
            if (user.CardId == 0) return "https://media.discordapp.net/attachments/923671441193443348/936047609582350406/660c3df0dcc25569.png";
            var card = db.Cards.Where(x => x.Id == user.CardId).First();
            return card.Url;
        }
        public static string GetAllCardDS()
        {
            var result = "```";
            var i = 1;
            foreach (var card in db.Cards)
            {
                var text = $"{i} {card.Name}\t";
                for (var j = text.Length; j < 15; j++) text += " ";
                if (i % 3 != 0) result += $"{text}\t";
                if (i % 3 == 0) result += $"{text}\n";
                i++;
            }
            result += "```";
            return result;
        }
        public static bool AddDropRollDS(string tableName, string url, string name)
        {
            switch (tableName)
            {
                case "ThreeStarWeapons":
                    if (db.ThreeStarWeapons.Where(x => x.Name == name).ToList().Count() != 0) return false;
                    db.ThreeStarWeapons.Add(new ThreeStarWeapon() { Url = url, Name = name });
                    break;
                case "FourStarWeapons":
                    if (db.FourStarWeapons.Where(x => x.Name == name).ToList().Count() != 0) return false;
                    db.FourStarWeapons.Add(new FourStarWeapon() { Url = url, Name = name });
                    break;
                case "FiveStarWeapons":
                    if (db.FiveStarWeapons.Where(x => x.Name == name).ToList().Count() != 0) return false;
                    db.FiveStarWeapons.Add(new FiveStarWeapon() { Url = url, Name = name });
                    break;
                case "FourStarCharacters":
                    if (db.FourStarCharacters.Where(x => x.Name == name).ToList().Count() != 0) return false;
                    db.FourStarCharacters.Add(new FourStarCharacter() { Url = url, Name = name });
                    break;
                case "FiveStarCharacters":
                    if (db.FiveStarCharacters.Where(x => x.Name == name).ToList().Count() != 0) return false;
                    db.FiveStarCharacters.Add(new FiveStarCharacter() { Url = url, Name = name });
                    break;
                default: return false;
            }
            db.SaveChanges();
            return true;
        }
        public static bool RemoveDropRollDS(string tableName, string name)
        {
            switch (tableName)
            {
                case "ThreeStarWeapons":
                    if (db.ThreeStarWeapons.Where(x => x.Name == name).ToList().Count() == 0) return false;
                    db.Remove(db.ThreeStarWeapons.Where(x => x.Name == name).First());
                    break;
                case "FourStarWeapons":
                    if (db.FourStarWeapons.Where(x => x.Name == name).ToList().Count() == 0) return false;
                    db.Remove(db.FourStarWeapons.Where(x => x.Name == name).First());
                    break;
                case "FiveStarWeapons":
                    if (db.FiveStarWeapons.Where(x => x.Name == name).ToList().Count() == 0) return false;
                    db.Remove(db.FiveStarWeapons.Where(x => x.Name == name).First());
                    break;
                case "FourStarCharacters":
                    if (db.FourStarCharacters.Where(x => x.Name == name).ToList().Count() == 0) return false;
                    db.Remove(db.FourStarCharacters.Where(x => x.Name == name).First());
                    break;
                case "FiveStarCharacters":
                    if (db.FiveStarCharacters.Where(x => x.Name == name).ToList().Count() == 0) return false;
                    db.Remove(db.FiveStarCharacters.Where(x => x.Name == name).First());
                    break;
                default: return false;
            }
            db.SaveChanges();
            return true;
        }
        public static (string name, string url) GetRandomDropRollDS(string tableName)
        {
            var rnd = new Random();
            var rndNumber = 0;
            switch (tableName)
            {
                case "ThreeStarWeapons":
                    var threeStarWeaponsList = db.ThreeStarWeapons.ToList();
                    rndNumber = rnd.Next(threeStarWeaponsList.Count);
                    return (threeStarWeaponsList[rndNumber].Name,
                        threeStarWeaponsList[rndNumber].Url);
                case "FourStarWeapons":
                    var fourStarWeaponsList = db.FourStarWeapons.ToList();
                    rndNumber = rnd.Next(fourStarWeaponsList.Count);
                    return (fourStarWeaponsList[rndNumber].Name,
                        fourStarWeaponsList[rndNumber].Url);
                case "FiveStarWeapons":
                    var fiveStarWeaponsList = db.FiveStarWeapons.ToList();
                    rndNumber = rnd.Next(fiveStarWeaponsList.Count);
                    return (fiveStarWeaponsList[rndNumber].Name,
                        fiveStarWeaponsList[rndNumber].Url);
                case "FourStarCharacters":
                    var fourStarCharactersList = db.FourStarCharacters.ToList();
                    rndNumber = rnd.Next(fourStarCharactersList.Count);
                    return (fourStarCharactersList[rndNumber].Name,
                        fourStarCharactersList[rndNumber].Url);
                case "FiveStarCharacters":
                    var fiveStarCharactersList = db.FiveStarCharacters.ToList();
                    rndNumber = rnd.Next(fiveStarCharactersList.Count);
                    return (fiveStarCharactersList[rndNumber].Name,
                        fiveStarCharactersList[rndNumber].Url);
                default: return ("", "");
            }
        }
    }
}
