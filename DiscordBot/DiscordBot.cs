using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using DiscordBot.Utilities.Parser;
using DiscordBot.Utilities.Parser.Rule34;

using Infrastructure;

namespace DiscordBot
{
    public class DiscordBot
    {
        private Timer coinTimerMessage;
        private DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider services;

        public async Task RunBotAsync()
        {
            client = new DiscordSocketClient();
            commands = new CommandService();

            services = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(commands)
                .BuildServiceProvider();
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);

            string token = "OTIyOTgzNTE1NDg4OTQ0MTM4.GHX7Mh.7hxTl-nyJ2jeUSgoZc3J1oqD4boJsvYF1cUyBI";

            client.Log += clientLog;
            client.MessageReceived += HandleCommandAsync;

            client.ReactionAdded += Client_ReactionAdded;
            client.ReactionRemoved += Client_ReactionRemoved;
            //await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
            await client.LoginAsync(TokenType.Bot, token);

            await client.StartAsync();
            await Task.Delay(-1);
        }
        private async Task Client_ReactionAdded(Cacheable<IUserMessage, ulong> arg1, Cacheable<IMessageChannel, ulong> arg2, SocketReaction arg3)
        {
            //Utilities.TestMiniGame.GameProcess(arg3.Emote.Name, arg3.UserId.ToString());

            Action<ulong> addRole = (x) => client.GetGuild(922508359368835082).GetUser(arg3.UserId).AddRoleAsync(x);
            if (arg3.MessageId == 936342181604622397)
            {
                switch (arg3.Emote.Name)
                {
                    case "pyro":
                        addRole(922996053354115082);
                        break;
                    case "hydro":
                        addRole(922996188196773938);
                        break;
                    case "anemo":
                        addRole(922996858165555220);
                        break;
                    case "electro":
                        addRole(922521769645056050);
                        break;
                    case "dendro":
                        addRole(922996968484110338);
                        break;
                    case "cryo":
                        addRole(922997148625289247);
                        break;
                    case "geo":
                        addRole(922997280930402324);
                        break;
                }
            }
        }
        private async Task Client_ReactionRemoved(Cacheable<IUserMessage, ulong> arg1, Cacheable<IMessageChannel, ulong> arg2, SocketReaction arg3)
        {
            Action<ulong> addRole = (x) => client.GetGuild(922508359368835082).GetUser(arg3.UserId).RemoveRoleAsync(x);

            if (arg3.MessageId == 936342181604622397)
            {
                switch (arg3.Emote.Name)
                {
                    case "pyro":
                        addRole(922996053354115082);
                        break;
                    case "hydro":
                        addRole(922996188196773938);
                        break;
                    case "anemo":
                        addRole(922996858165555220);
                        break;
                    case "electro":
                        addRole(922521769645056050);
                        break;
                    case "dendro":
                        addRole(922996968484110338);
                        break;
                    case "cryo":
                        addRole(922997148625289247);
                        break;
                    case "geo":
                        addRole(922997280930402324);
                        break;
                }
            }
        }
        private async Task HandleCommandAsync(SocketMessage arg)
        {
            if (!(arg is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;
            if (message.Author.IsBot) return;

            Utilities.CoinEvent.MessageEvent(client);
            var rnd = new Random();
            var messageContent = message.Content.Split(" "); 
            if (messageContent[0] == "!get" && 
                Utilities.CoinEvent.botMsg.Code == Int32.Parse(messageContent[1]) && 
                Utilities.CoinEvent.botMsg.Msg != null &&
                Utilities.CoinEvent.lastUserId != arg.Author.Id.ToString())
            {
                var userId = arg.Author.Id.ToString();
                var coin = rnd.Next(-100, 200);

                if (Utilities.CoinEvent.CountEvent == 0) return;
                if (!Logic.CheckUser(userId)) return;

                Logic.SetCoin(userId, coin);
                arg.Channel.SendMessageAsync($"{arg.Author.Mention}, тебе начислено {coin} гемов");

                Utilities.CoinEvent.botMsg.Msg.DeleteAsync();
                arg.DeleteAsync();

                Utilities.CoinEvent.lastUserId = userId;
                Utilities.CoinEvent.CountEvent--;
                Utilities.CoinEvent.botMsg.Msg = null;

                return;
            }

            var context = new SocketCommandContext(client, message);
            int argPos = 0;
            if (message.HasStringPrefix("!", ref argPos))
            {
                var result = await commands.ExecuteAsync(context, argPos, services);
                if (!result.IsSuccess)
                    Console.WriteLine(result.ErrorReason);
                if (result.Error.Equals(CommandError.UnmetPrecondition))
                    await message.Channel.SendMessageAsync(result.ErrorReason);
            }
        }

        private Task clientLog(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DiscordBot.Utilities.Parser;
//using DiscordBot.Utilities.Parser.Rule34;
//using DiscordBot.Utilities.Images;
//using Discord;
//using Discord.WebSocket;
//using Infrastructure;

//namespace DiscordBot
//{
//    public class DSBot
//    {
//        private DiscordSocketClient client;
//        private string token = "OTIyOTgzNTE1NDg4OTQ0MTM4.YcJZYg.dZnFNynjLKWrJRluVNsIsizvhkY";
//        private readonly Images image = new Images();
//        public async Task RunDSBot()
//        {
//            client = new DiscordSocketClient();
//            client.UserJoined += Client_UserJoined;
//            client.MessageReceived += Client_MessageReceived;
//            client.JoinedGuild += Client_JoinedGuild;
//            client.JoinedGuild += Client_JoinedGuild;
//            client.Log += Log;
//            client.LoginAsync(TokenType.Bot, token);
//            client.StartAsync();
//            await Task.Delay(-1);
//        }

//        private async Task Log(LogMessage msg)
//        {
//            Console.WriteLine(msg.ToString());
//            return Task.CompletedTask;
//        }
//        private async Task Client_JoinedGuild(SocketGuild user)
//        {
//            new Logic().NewUserDiscord(user.Id.ToString());
//            Console.WriteLine("хуй");
//            var channel = client.GetChannel(923671441193443348) as SocketTextChannel;
//            await channel.SendMessageAsync($"Welcome to {channel.Guild.Name}");
//            var usertest = client.GetGuild(284592161062649856).CurrentUser;
//            usertest.SendMessageAsync("хуй ты");

//        }
//        private async Task Client_UserJoined(SocketGuildUser user)
//        {
//            new Logic().NewUserDiscord(user.Id.ToString());
//            Console.WriteLine("хуй");
//            var channel = client.GetChannel(923671441193443348) as SocketTextChannel;
//            await channel.SendMessageAsync($"Welcome {user.Mention} to {channel.Guild.Name}");
//            var usertest = client.GetGuild(284592161062649856).CurrentUser;
//            usertest.SendMessageAsync("хуй ты");

//        }
//        private async Task Client_MessageReceived(SocketMessage msg)
//        {
//            var test1 = msg.Author as SocketGuildUser;
//            922514671074947072 < 3
//            922508501123727440 шелковица
//            922623657791074355 извращенец
//            msg.Channel.SendMessageAsync(msg.Content);
//            test1.RemoveRoleAsync(922520480789954560);
//            if (!msg.Author.IsBot) msg.Channel.SendMessageAsync($"{test1.Id}");
//            if (!msg.Author.IsBot) msg.Channel.SendMessageAsync($"{test1.}");
//            if (!msg.Author.IsBot)
//            {
//                switch (msg.Content)
//                {
//                    case "!1":
//                        var path = image.CreateImageAsync(msg.Author).Result;
//                        var attachment = new FileAttachment(path, path);
//                        var test = new FileAttachment[] { attachment };
//                        await msg.Channel.SendFilesAsync(test);
//                        attachment.Dispose();
//                        File.Delete(path);
//                        break;
//                    case "!2":
//                        msg.Channel.SendMessageAsync($"ты хуй {msg.Author}");
//                        break;
//                    case "!3":
//                        AnimeIMG(msg);
//                        break;
//                    case "!4":
//                        test1.SendMessageAsync("ты хуй");
//                        break;
//                }
//            }
//            msg.Channel.SendMessageAsync($"хуй ты, {msg.Author.Mention}");
//            if (msg.Content == "!1") AnimeIMG(msg);

//            return Task.CompletedTask;
//        }
//        void AnimeIMG(SocketMessage msg)
//        {
//            ParserWorker<string[]> parser = new ParserWorker<string[]>(new Rule34Parser());

//            Random rnd = new Random();
//            int value = rnd.Next(1, 10);
//            string result = "";
//            parser.Settings = new Rule34Settings(value, value);
//            parser.Start();
//            parser.OnNewData += Parser_OnNewData;
//            parser.OnCompleted += Parser_OnCompleted;
//            void Parser_OnNewData(object arg1, string[] arg2)
//            {
//                result = arg2[rnd.Next(1, arg2.Length)];
//            }
//            async void Parser_OnCompleted(object arg1)
//            {
//                msg.Channel.SendMessageAsync($"https://rule34.world/{result}");
//            }

//        }
//    }
//}
