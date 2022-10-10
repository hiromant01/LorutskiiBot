using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DiscordBot.Utilities.Parser.Rule34
{
    public class Rule34Settings : IParserSettings
    {
        public Rule34Settings(int start, int end)
        {
            StartPoint = start;
            EndPoint = end;
        }
        public string BaseUrl { get; set; } = "https://rule34.world/page";
        public string Prefix { get; set; } = "{CurrentId}";
        public int StartPoint { get; set; }
        public int EndPoint { get; set; }
    }
}

