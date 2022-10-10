using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Rest;
using Discord;

namespace DiscordBot.Utilities
{
    public static class TestMiniGame
    {
        public static bool Active { get; set; }
        public static RestUserMessage Message { get; set; }
        public static string FirstPlayerId = "";
        public static int FirstPlayerCounter = 0;
        public static string SecondPlayerId = "";
        public static int SecondPlayerCounter = 0;
        public async static void AddPlayer(string dsId)
        {
            if (!Active)
            {
                if (FirstPlayerId.Length == 0)
                    FirstPlayerId = dsId;
                else if (FirstPlayerId != dsId)
                {
                    SecondPlayerId = dsId;
                    Active = true;
                    ChangeMessage();
                    await Message.AddReactionAsync(Emote.Parse("<:pyro:936344630465806346>"));
                    await Message.AddReactionAsync(Emote.Parse("<:hydro:936344505358123028>"));
                }
            }
        }
        public static void GameProcess(string nameEmote, string dsId)
        {
            if (FirstPlayerId == dsId && nameEmote == "pyro")
                FirstPlayerCounter++;
            else if (SecondPlayerId == dsId && nameEmote == "hydro")
                SecondPlayerCounter++;
            ChangeMessage();
        }
        private static void ChangeMessage()
        {
            Message.ModifyAsync(x => x.Content = $"{FirstPlayerCounter}  {SecondPlayerCounter}");
        }
    }
}
//<:dendro:936344674891886603>
//<:pyro:936344630465806346>
//<:hydro:936344505358123028>
//<:geo:936344141917466714>
//<:cryo:936344243843239938>
//<:electro:936344557531041852>
//<:anemo:936343897003679887>