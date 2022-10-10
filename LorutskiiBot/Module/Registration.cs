using Discord.Commands;
using Infrastructure;
using Discord;

namespace DiscordBot.Modules
{
    public class Registration : ModuleBase<SocketCommandContext>
    {
        [Command("reg")]
        public async Task RegistrationAsync()
        {
            var user = Context.User;
            var discordId = user.Id.ToString();
            if (Logic.CheckUser(discordId))
            {
                await Context.Channel.SendMessageAsync("Ты уже связан");
                return;
            }
            if (Logic.LinkRequestsDsTwitch.ContainsValue(discordId))
            {
                await Context.Channel.SendMessageAsync("Ты уже хочешь связаться");
                return;
            }
            var identifier = Guid.NewGuid().ToString();
            Logic.LinkRequestsDsTwitch[identifier] = discordId;
            await user.SendMessageAsync($"Для возможности пользоваться ботом в дискорде тебе нужно \nВ ЧАТЕ ТВИЧА \"https://www.twitch.tv/lorutskii\" прописать\n!reg {identifier}");
        }
    }
}