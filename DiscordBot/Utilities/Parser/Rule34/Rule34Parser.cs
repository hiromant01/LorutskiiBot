using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;

namespace DiscordBot.Utilities.Parser.Rule34
{
    class Rule34Parser : IParser<string[]>
    {
        public string[] Parse(IHtmlDocument document)
        {
            var list = new List<string>();
            var items = document.QuerySelectorAll("img").Where(item => item.ClassName != null && item.ClassName.Contains("img"));

            foreach (var item in items)
            {
                list.Add(item.GetAttribute("src"));
            }

            return list.ToArray();
        }
    }
}
