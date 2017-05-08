using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Services.Common
{
    /// <summary>
    /// Set the minimum permission required to use a module or command
    /// similar to how MinPermissions works in Discord.Net 0.9.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MinPermissionsAttribute : PreconditionAttribute
    {
        private AccessLevel Level;

        public MinPermissionsAttribute(AccessLevel level)
        {
            Level = level;
        }

        public override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider provider)
        {
            var access = GetPermission(context);            // Get the acccesslevel for this context

            if (access >= Level)                            // If the user's access level is greater than the required level, return success.
                return Task.FromResult(PreconditionResult.FromSuccess());
            else
                return Task.FromResult(PreconditionResult.FromError("Insufficient permissions."));
        }

        public AccessLevel GetPermission(ICommandContext c)
        {
            if (c.User.IsBot)
                return AccessLevel.Blocked;

            //if (Configuration.Load().Owners.Contains(c.User.Id))
            //    return AccessLevel.BotOwner;

            if (c.User is SocketGuildUser user)
            {
                if (c.Guild.OwnerId == user.Id)
                    return AccessLevel.ServerOwner;

                if (user.GuildPermissions.Administrator)
                    return AccessLevel.ServerAdmin;

                if (user.GuildPermissions.ManageMessages ||
                    user.GuildPermissions.BanMembers ||
                    user.GuildPermissions.KickMembers)
                    return AccessLevel.ServerMod;
            }

            return AccessLevel.User; // If nothing else, return a default permission.
        }
    }
}