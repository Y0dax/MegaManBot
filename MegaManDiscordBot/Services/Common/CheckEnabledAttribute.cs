using Discord.Commands;
using Discord.WebSocket;
using MegaManDiscordBot.Modules.Models.DomainModels;
using MegaManDiscordBot.Services.Moderator;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Services.Common
{
    /// <summary>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CheckEnabledAttribute : PreconditionAttribute
    {
        private string _module;
        private GuildOptionsService _guildService;//Need to set somehow

        public CheckEnabledAttribute(string module)
        {
            _module = module;
        }

        public async override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider provider)
        {
            var options = await _guildService.GetGuildOptions(context.Guild.Id);
            if (options != null)
            {
                var isEnabled = options.GetType().GetProperties().Where(x => x.Name == _module).FirstOrDefault().GetValue(options);
                if (!(bool)isEnabled)
                {
                    return PreconditionResult.FromError("Insufficient permissions.");
                }
            }
            return PreconditionResult.FromSuccess();
        }

        public async Task<GuildOptions> GetOptions(ICommandContext context)
        {
            return await _guildService.GetGuildOptions(context.Guild.Id);
        }
    }
}