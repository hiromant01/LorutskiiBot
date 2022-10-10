using AngleSharp.Html.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscordBot.Utilities.Parser.Anime_Pictures
{
    class AnimeParser : IParser<string[]>
    {
        public string[] Parse(IHtmlDocument document)
        {
            var list = new List<string>();
            var items = document.QuerySelectorAll("img").Where(item => item.ClassName != null && item.ClassName.Contains("img_cp"));

            foreach (var item in items)
            {
                list.Add(item.GetAttribute("src"));
            }

            return list.ToArray();
        }
    }
}
