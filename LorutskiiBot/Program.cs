using TwitchBot;
using DiscordBot.Modules;

namespace Template
{
    class Program
    {
        static void Main(string[] args)
        {
            var twitchBot = new TwitchBot.TwitchBot();
            var discordBot = new DiscordBot.DiscordBot().RunBotAsync().GetAwaiter().GetResult;
            Console.ReadLine();
        }
    }
}