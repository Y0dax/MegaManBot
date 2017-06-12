using Discord.Commands;
using Discord.WebSocket;
using MegaManDiscordBot.Modules.Models.DomainModels;
using Microsoft.Extensions.DependencyInjection;
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
        private string _moduleName;
        private GuildOptionsService _guildService;

        public CheckEnabledAttribute(string module)
        {
            _moduleName = module;
        }

        public async override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider provider)
        {
            _guildService = provider.GetService<GuildOptionsService>();
            var options = await _guildService.GetGuildOptions(context.Guild.Id);
            if (options != null)
            {
                var isEnabled = Extensions.GetPropValue(options.Modules, _moduleName);
                if (!(bool)isEnabled)
                {
                    return PreconditionResult.FromError("Insufficient permissions.");
                }
            }
            return PreconditionResult.FromSuccess();
        }
    }
}