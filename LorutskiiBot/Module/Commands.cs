using Discord.Commands;
using Infrastructure;
using Discord;
using Discord.WebSocket;
using DiscordBot.Utilities.Parser;

namespace LorutskiiBot.Module
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        private readonly ulong channelId = 935296314051272734;

        [Command("крутка")]
        public async Task RollBaner()
        {
           // if (Context.Channel.Id != channelId) return;

            var user = Context.User;
            var image = new DiscordBot.Utilities.Images();
            var drop = Modules.RollBanner.RollDS(user.Id.ToString(), user.Mention, 0);

            if (drop.result.Length != 0) { await ReplyAsync(drop.result); return; }

            var path = image.test(Context.User, drop.drop, Modules.CardsDiscord.GetCardUrlUser(Context.User.Id.ToString())).Result;
            var attachment = new FileAttachment(path, path);
            var test = new FileAttachment[] { attachment };
            await Context.Channel.SendFilesAsync(test);
            attachment.Dispose();
            File.Delete(path);
        }
        [Command("крутка")]
        public async Task RollBaner(int count)
        {
           // if (Context.Channel.Id != channelId) return;

            var user = Context.User;
            var image = new DiscordBot.Utilities.Images();
            var drop = Modules.RollBanner.RollDS(user.Id.ToString(), user.Mention, count);

            if (drop.result.Length != 0) { await ReplyAsync(drop.result); return; }

            var path = image.test(Context.User, drop.drop, Modules.CardsDiscord.GetCardUrlUser(Context.User.Id.ToString())).Result;
            var attachment = new FileAttachment(path, path);
            var test = new FileAttachment[] { attachment };
            await Context.Channel.SendFilesAsync(test);
            attachment.Dispose();
            File.Delete(path);
        }
        [Command("timely")]
        public async Task Timely()
        {
            //if (Context.Channel.Id != channelId) return;
            var user = Context.User;
            var nickname = user.Mention;
            var discordId = user.Id.ToString();
            var result = Modules.Coins.DailyCoin(discordId, nickname);
            await Context.Channel.SendMessageAsync(result);
        }
        [Command("givecoin")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task Givecoin(string user, int coin)
        {
            var userId = user.Replace("<@!", "");
            userId = userId.Replace(">", "");

           await Context.Channel.SendMessageAsync(Modules.Coins.GiveCoinAdmin(userId, user, coin));
        }
        [Command("giveexp")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task Giveexp(string user, int exp)
        {
            var userId = user.Replace("<@!", "");
            userId = userId.Replace(">", "");

            await Context.Channel.SendMessageAsync(Modules.Rank.GiveExpAdmin(userId, user, exp));

        }
        [Command("give")]
        public async Task Give(string user, int coin)
        {
            var userId = user.Replace("<@!", "");
            userId = userId.Replace(">", "");
            await Context.Channel.SendMessageAsync(Modules.Coins.GiveCoin(Context.User.Id.ToString(), userId, user, coin));
        }
        [Command("top")]
        public async Task Top()
        {
            if (Context.Channel.Id != channelId) return;
            await Context.Channel.SendMessageAsync(Modules.Rank.RangTop());
        }
        [Command("balance")]
        public async Task Balance()
        {
            if (Context.Channel.Id != channelId) return;
            var user = Context.User;
            await Context.Channel.SendMessageAsync(Modules.Coins.Balance(user.Id.ToString(), user.Mention));

        }
        [Command("balance")]
        public async Task BalanceSomeone(string user)
        {
            var userId = user.Replace("<@!", "");
            userId = userId.Replace(">", "");
            if (Context.Channel.Id != channelId) return;
            await Context.Channel.SendMessageAsync(Modules.Coins.Balance(userId, user));

        }
        [Command("books")]
        public async Task Books()
        {

        }
        [Command("ar")]
        public async Task Ar()
        {
            if (!Logic.CheckUser(Context.User.Id.ToString()))
            {
                await ReplyAsync("Тебя нет. Пропиши !reg");
                return;
            }
            var image = new DiscordBot.Utilities.Images();
            var path = image.CreateImageAsync(Context.User, Modules.CardsDiscord.GetCardUrlUser(Context.User.Id.ToString())).Result;
            var attachment = new FileAttachment(path, path);
            var test = new FileAttachment[] { attachment };
            await Context.Channel.SendFilesAsync(test);
            attachment.Dispose();
            File.Delete(path);
            await Context.Message.DeleteAsync();
        }
        [Command("love")]
        public async Task Love(string nickname)
        {
            var rnd = new Random();
            await Context.Channel.SendMessageAsync($"Между {Context.User.Mention} и {nickname} {rnd.Next(101)}% любви :sparkling_heart:");
        }
        [Command("трусики")]
        public async Task Panties()
        {
            var nickname = Context.User.Mention;
            await Context.Channel.SendMessageAsync($"{nickname} - Главный извращенец этого дня");
        }
        [Command("bb")]
        public async Task Bb()
        {
            var nickname = Context.User.Mention;
            await Context.Channel.SendMessageAsync($"{nickname}, украл(а) мои трусики и уже уходишь? Ну иди-иди...");
        }
        [Command("qq")]
        public async Task Qq()
        {
            var nickname = Context.User.Mention;
            await Context.Channel.SendMessageAsync($"{nickname}, бери мои трусики, но только останься <3");
        }
        [Command("uid")]
        public async Task Uid()
        {
            await Context.Channel.SendMessageAsync("Привет, мой UID 705824345 ❤");
        }
        [Command("любовь")]
        public async Task Dick(string nickname)
        {
            await Context.Channel.SendMessageAsync($"Ты хуй {nickname}");
        }
        [Command("start")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task Start()
        {
            var channel = Context.Client.GetChannel(934871135097618481) as SocketTextChannel;
            await channel.SendMessageAsync($"@everyone\nLorutski начала трансляцию\nhttps://www.twitch.tv/lorutskii");

        }
        [Command("addcard")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task AddCard(string name, string url)
        {
            await Context.Message.DeleteAsync();
            await Context.Channel.SendMessageAsync(Modules.CardsDiscord.AddCard(url, name.ToLower()));
        }
        [Command("removecard")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task RemoveCard(string name)
        {
            await Context.Channel.SendMessageAsync(Modules.CardsDiscord.RemoveCard(name.ToLower()));
        }
        [Command("card")]
        public async Task SelectUserCard(string name)
        {
            await Context.Channel.SendMessageAsync(Modules.CardsDiscord.SelectUserCard(name.ToLower(), Context.User.Id.ToString()));
        }
        [Command("allcard")]
        public async Task GetAllCards()
        {
            await Context.Channel.SendMessageAsync(Modules.CardsDiscord.GetAllCard());
        }
        [Command("adddroproll")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task AddDropRoll(string tableName, string name, string url)
        {
            await Context.Message.DeleteAsync();
            await Context.Channel.SendMessageAsync(Modules.DropRollDiscord.AddDropRoll(tableName, url, name.Replace('.', ' ')));
        }
        [Command("removedroproll")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task RemoveDropRoll(string tableName, string name)
        {
            await Context.Message.DeleteAsync();
            await Context.Channel.SendMessageAsync(Modules.DropRollDiscord.RemoveDropRoll(tableName, name.Replace('.', ' ')));
        }
        [Command("freeroll")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task FreeRoll()
        {
            if (Modules.RollBanner.IsFreeRoll)
                Modules.RollBanner.IsFreeRoll = false;
            else Modules.RollBanner.IsFreeRoll = true;
            await Context.Channel.SendMessageAsync($"Бесплатные крутки - {Modules.RollBanner.IsFreeRoll}");
        }
        [Command("delete")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task DeleteMessage(int count)
        {
            var messages = await Context.Channel.GetMessagesAsync(count).FlattenAsync();
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
        }
        [Command("talentshelp")]
        public async Task TalentsHelp()
        {
            //Logic.test();
            await Context.Channel.SendMessageAsync(Modules.Talents.TalentsHelp());
        }
    }
}