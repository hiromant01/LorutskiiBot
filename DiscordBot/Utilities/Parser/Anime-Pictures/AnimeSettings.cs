using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Utilities.Parser.Anime_Pictures
{
    class AnimeSettings : IParserSettings
    {
        public AnimeSettings(int start, int end)
        {
            StartPoint = start;
            EndPoint = end;
        }

        public string BaseUrl { get; set; } = "https://anime-pictures.net/pictures/view_posts";

        public string Prefix { get; set; } = "{CurrentId}";

        public int StartPoint { get; set; }

        public int EndPoint { get; set; }
    }
}
