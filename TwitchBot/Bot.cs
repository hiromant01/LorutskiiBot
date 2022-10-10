using System;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchBot.Commands;
using Infrastructure;

namespace TwitchBot
{
    public class TwitchBot
    {
        private static readonly TwitchClient client = new();
        private readonly ConnectionCredentials credentials = new("lorutskiibot", "oauth:3hkknswec58apeyj2632xepra9m25d");

        private static readonly Random rnd = new();
        private static readonly string Channel = "lorutskii";
        public TwitchBot()
        {
            client.Initialize(credentials, Channel);

            client.OnLog += Client_OnLog;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnChatCommandReceived += Client_OnChatCommandReceived;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnNewSubscriber += Client_OnNewSubscriber;
            client.OnBeingHosted += Client_OnBeingHosted; //test
            client.Connect();
        }


        private void Client_OnBeingHosted(object? sender, OnBeingHostedArgs e) //test
        {
            SendMessage($"ой {e.BeingHostedNotification.Viewers}");
        }

        private static void SendMessage(string message)
        {
            client.SendMessage(Channel, message);
        }
        private void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            SendMessage($"{e.Subscriber.DisplayName}, стал таким же сладким как цветок сахарок");
        }
        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            Logic.NewUserTwitch(e.ChatMessage.Username, e.ChatMessage.UserId);

            //var nickname = e.ChatMessage.Username;
            //if (nickname == "alemuro_") SendMessage($"@{nickname}, Откуда пришел???");
        }

        private void Client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            var twitchId = e.Command.ChatMessage.UserId;
            var nickname = e.Command.ChatMessage.Username;
            var arguments = ArgumentsParse(e.Command.ArgumentsAsString);



                switch (e.Command.CommandText.ToLower())
                {
                case "timely":
                    SendMessage(Modules.Coins.DailyCoin(twitchId, nickname));
                    break;
                
                case "givecoin": //////////////
                    if (e.Command.ChatMessage.IsModerator || e.Command.ChatMessage.IsBroadcaster) //если главный или модератор
                    {
                        SendMessage(Modules.Coins.GiveCoinAdmin(arguments.str, arguments.str, arguments.number));
                    }
                    else SendMessage("Нет прав");
                    break;
                case "giveexp": //////////////
                    if (e.Command.ChatMessage.IsModerator || e.Command.ChatMessage.IsBroadcaster) //если главный или модератор
                    {
                        SendMessage(Modules.Rank.GiveExpAdmin(arguments.str, arguments.str, arguments.number));
                    }
                    else SendMessage("Нет прав");
                    break;
                case "freeroll":
                    if (e.Command.ChatMessage.IsModerator || e.Command.ChatMessage.IsBroadcaster) //если главный или модератор
                    {
                        if (Modules.RollBanner.IsFreeRoll)
                            Modules.RollBanner.IsFreeRoll = false;
                        else Modules.RollBanner.IsFreeRoll = true;
                        SendMessage($"Бесплатные крутки - {Modules.RollBanner.IsFreeRoll}");
                    }
                    break;
                case "give": ///////////////
                    SendMessage(Modules.Coins.GiveCoin(nickname, arguments.str, arguments.str, arguments.number));
                    break;
                case "top":
                    SendMessage(Modules.Rank.RangTop());
                    break;
                case "balance":
                    if (arguments.str == "")
                        SendMessage(Modules.Coins.Balance(twitchId, nickname));
                    else SendMessage(Modules.Coins.Balance(arguments.str, arguments.str));
                    break;
                case "books":
                    SendMessage(Books.GetAnswer());
                    break;
                case "крутка":
                    SendMessage(Modules.RollBanner.Roll(twitchId, nickname, arguments.number));
                    break;
                case "ar":
                    SendMessage(Modules.Rank.MyRank(twitchId, nickname));
                    break;
                case "love":
                    if (arguments.str != "") SendMessage($"Между {nickname} и {arguments.str} {rnd.Next(100)}% любви HungryPaimon");
                    break;
                case "кубик":
                    var resultKubik = rnd.Next(1, 100);
                    SendMessage($"Результат: {resultKubik}");
                    break;
                case "трусики":
                    SendMessage($"{nickname} - Главный извращенец этого дня");
                    break;
                case "bb":
                    SendMessage($"{nickname}, украл(а) мои трусики и уже уходишь? Ну иди-иди...");
                    break;
                case "qq":
                    SendMessage($"{nickname}, бери мои трусики, но только останься <3");
                    break;
                case "uid":
                    SendMessage("Привет, мой UID 705824345 ❤");
                    break;
                case "discord":
                    SendMessage("https://discord.gg/CR3CFEuMYg");
                    break;
                case "reg":
                    SendMessage(Logic.ConnectDsTwitch(twitchId, arguments.str));
                    break;
                default:
                    //SendMessage("Чего нет, того нет");
                    break;
            }
        }
        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            //client.SendMessage(e.Channel, "Привет! Бот успешно подключен к каналу.");
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine(e.Data);
        }
        private (string str, int number) ArgumentsParse(string arguments)
        {
            var number = 0;
            var str = "";

            if (arguments == "") return (str, number);
            var result = arguments.Split(' ');
            str = result[0].Trim('@');

            if (Int32.TryParse(result[0], out number)) return (str, number);
            if (result.Length == 2) Int32.TryParse(result[1], out number);
            return (str, number);

        }
    }
}
