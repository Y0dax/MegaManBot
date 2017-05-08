using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MegaManDiscordBot.Services.Common;

namespace MegaManDiscordBot.Modules.Public
{
    class PublicModule : ModuleBase<SocketCommandContext>
    {

        [Command("test")]
        public async Task Test()
        {
            await ReplyAsync("Test");
        }

        [Command("echo")]
        [Remarks("Make the bot repeat a phrase")]
        //[MinPermissions(AccessLevel.ServerAdmin)]
        public async Task Echo([Remainder]string text)
        {
            await ReplyAsync(text);
        }

        [Command("info")]
        [Remarks("Get info on a user")]
        //[MinPermissions(AccessLevel.User)]
        public async Task UserInfo([Remainder]SocketGuildUser user)
        {
            //var userStrings = user.Contains("#") ? user.Split('#') : null;
            StringBuilder returnMesage = new StringBuilder();
            //ushort userid;
            //if (userStrings == null || userStrings.Length != 2 || String.IsNullOrEmpty(userStrings[0]) ||
            //    String.IsNullOrEmpty(userStrings[1]) || !ushort.TryParse(userStrings[1], out userid))
            //{
            //    await ReplyAsync("Invalid username provided. Use quotes, include the #ID, and format such as \"Mega Man#3043\" or \"Name#1234\".");
            //    return;
            //}

            //var foundUser = Context.Guild.GetUser(userStrings[0], ushort.Parse(userStrings[1]));
            //if (foundUser == null)
            //{
            //    await ReplyAsync($"{e.GetArg("User")} was not found on the server.");
            //    return;
            //}

            //if (foundUser == e.User.Server.GetUser(_client.CurrentUser.Id))
            //{
            //    await ReplyAsync("I'm Mega Man, duh.");
            //    return;
            //}

            returnMesage.Append($" {user.Mention} joined the {user.Guild.Name} on {user.JoinedAt.Value.ToLocalTime()}.");
            returnMesage.Append(user.Status != null ? $" {user.Nickname ?? user.Username} is currently {user.Status}" : "");
            returnMesage.Append(user.Game.Value.Name != null ? $" and is playing {user.Game.Value.Name}." : "");
            //returnMesage.Append(user.Status?.Value == "online" && user.JoinedAt != null ? $" They were last active at {user.LastActivityAt.Value.ToLocalTime()}." : "");

            await ReplyAsync(returnMesage.ToString());
        }

        //_client.GetService<CommandService>().CreateCommand("info")
        //    .Description("Gets general information about the bot")
        //    .Do(async e =>
        //    {
        //    await e.Channel.SendMessage($"@{e.User.Name} would like info on ");
        //});

        //    _client.GetService<CommandService>().CreateCommand("roll")
        //    .Description("Roll a die")
        //    .Parameter("dieInfo", ParameterType.Required)
        //    .Do(async e =>
        //    {
        //    try
        //    {
        //        var dieInfo = e.GetArg("dieInfo");
        //        PartialRoll roll = Dice.Roll(dieInfo);

        //        await e.Channel.SendMessage($"{e.User.Name} rolls a {dieInfo} for {roll.AsSum()} ({roll.CurrentRollExpression})");
        //    }
        //    catch
        //    {
        //        await e.Channel.SendMessage("Invalid die.");
        //    }
        //});

        //    _client.GetService<CommandService>().CreateCommand("userinfo")
        //    .Alias("Userinfo", "UserInfo")
        //    .Description("Gets general information about a user.")
        //    .Parameter("User", ParameterType.Required)
        //    .Do(async e =>
        //    {
        //    var userStrings = e.GetArg("User").Contains("#") ? e.GetArg("User").Split('#') : null;
        //    StringBuilder returnMesage = new StringBuilder();
        //    ushort userid;
        //    if (userStrings == null || userStrings.Length != 2 || String.IsNullOrEmpty(userStrings[0]) ||
        //        String.IsNullOrEmpty(userStrings[1]) || !ushort.TryParse(userStrings[1], out userid))
        //    {
        //        await e.Channel.SendMessage("Invalid username provided. Use quotes, include the #ID, and format such as \"Mega Man#3043\" or \"Name#1234\".");
        //        return;
        //    }

        //    var foundUser = e.User.Server.GetUser(userStrings[0], ushort.Parse(userStrings[1]));
        //    if (foundUser == null)
        //    {
        //        await e.Channel.SendMessage($"{e.GetArg("User")} was not found on the server.");
        //        return;
        //    }

        //    if (foundUser == e.User.Server.GetUser(_client.CurrentUser.Id))
        //    {
        //        await e.Channel.SendMessage("I'm Mega Man, duh.");
        //        return;
        //    }

        //    returnMesage.Append($" {foundUser.Mention} joined the {foundUser.Server.Name} on {foundUser.JoinedAt.ToLocalTime().ToShortDateString()}.");
        //    returnMesage.Append(foundUser.Status != null ? $" {foundUser.Nickname ?? foundUser.Name} is currently {foundUser.Status}" : "");
        //    returnMesage.Append(foundUser.CurrentGame != null ? $" and is playing {foundUser.CurrentGame.Value.Name}." : "");
        //    returnMesage.Append(foundUser.Status?.Value == "online" && foundUser.LastActivityAt != null ? $" They were last active at {foundUser.LastActivityAt.Value.ToLocalTime()}." : "");

        //    await e.Channel.SendMessage(returnMesage.ToString());
        //});
    }
}
