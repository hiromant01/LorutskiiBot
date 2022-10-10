using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using System.Drawing;
using System.Drawing.Drawing2D;
using Discord;
using Discord.Rest;

namespace DiscordBot.Utilities
{
    public class CoinEvent
    {
        private static ulong idChannel = 922508359368835084;
        private static Random rnd = new Random();

        public static int CountEvent = 0;

        public static (int Code, RestMessage Msg) botMsg;
        public static string lastUserId = "";

        public static async void MessageEvent(DiscordSocketClient client)
        {
            if (botMsg.Msg != null) return;
            if (rnd.Next(1000) > 100) return;
            var channel = client.GetChannel(idChannel) as SocketTextChannel;


            var imagePath = FetchImageAsync("https://media.discordapp.net/attachments/923671441193443348/937039299642273852/72_2.png").Result;
            var attachment = new FileAttachment(imagePath, imagePath);
            var test = new FileAttachment[] { attachment };
            var code = rnd.Next(1000);
            //botMsg.Add(code, await channel.SendFilesAsync(test, $"Для сбора коинов пропиши !get {code}"););
            botMsg.Msg = await channel.SendFilesAsync(test, $"Для сбора коинов пропиши !get {code}");
            botMsg.Code = code;
            attachment.Dispose();
            File.Delete(imagePath);

            CountEvent++;
        }

        private static async Task<string> FetchImageAsync(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            var stream = await response.Content.ReadAsStreamAsync();
            string path = $"{Guid.NewGuid()}.png";
            System.Drawing.Image.FromStream(stream).Save(path);
            return await Task.FromResult(path);
        }
    }
}
