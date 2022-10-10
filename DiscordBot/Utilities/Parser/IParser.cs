using AngleSharp.Html.Dom;

namespace DiscordBot.Utilities.Parser
{
    interface IParser<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}
