using Discord;
using Discord.Commands;
using MegaManDiscordBot.Services.Polls;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MegaManDiscordBot.Modules.Models.DomainModels.PollModels;

namespace MegaManDiscordBot.Modules
{
    public class PollsModule : ModuleBase<SocketCommandContext>
    {
        private PollService _pollService;

        public PollsModule(PollService service)
        {
            _pollService = service;
        }

        [Command("poll")]
        [Summary("Create a new poll")]
        [Remarks("<\"title\"> <\"option_1\"> <\"option_2\"> ..")]
        public async Task MakePoll(string title, params string[] items)
        {
            List<PollOption> pollOptions = new List<PollOption>();
            int optionIndex = 1;
            foreach (var item in items)
            {
                pollOptions.Add(new PollOption { OptionId = optionIndex++, OptionText = item });
            }
            Poll poll = new Poll { Title = title, CreateDate = DateTime.Now, CreatorId = Context.User.Id, GuildId = Context.Guild.Id, IsOpen = true, AllowAddOptions = false, Options = pollOptions };
            await _pollService.AddPoll(poll);

            StringBuilder response = new StringBuilder();
            response.Append($"Created poll {poll.PollId}\n\n{Format.Bold(poll.Title)}\n");
            foreach (var option in poll.Options)
            {
                response.Append($"{option.OptionId}. {option.OptionText}\n");
            }

            await ReplyAsync(response.ToString());
        }

        [Command("showpoll")]
        [Summary("Show a poll's info")]
        [Remarks("<poll_id>")]
        public async Task ShowPoll(string _pollId)
        {
            ObjectId pollId;
            if (ObjectId.TryParse(_pollId, out pollId))
            {
                Poll poll = await _pollService.GetPoll(pollId);
                if (poll == null) { await ReplyAsync("I couldn't find that poll."); return; }
                if (poll.GuildId != Context.Guild.Id) { await ReplyAsync("That poll was not created by this guild."); return; }

                StringBuilder response = new StringBuilder();
                response.Append($"Poll {poll.PollId}\n\n{Format.Bold(poll.Title)}\n");
                foreach (var option in poll.Options)
                {
                    response.Append($"{option.OptionId}. {option.OptionText} | Votes: {(poll.Votes != null ? poll.Votes.Count(x => x.OptionId == option.OptionId) : 0)}\n");
                }

                await ReplyAsync(response.ToString());
            }

        }

        [Command("vote")]
        [Summary("Vote for a poll option")]
        [Remarks("<poll_id> <option_id>")]
        public async Task PickFromList(string _pollId, int optionId)
        {
            ObjectId pollId;
            if (ObjectId.TryParse(_pollId, out pollId))
            {
                Poll poll = await _pollService.GetPoll(pollId);
                if (poll == null) { await ReplyAsync("I couldn't find that poll."); return; }
                if (poll.GuildId != Context.Guild.Id) { await ReplyAsync("That poll was not created by this guild."); return; }
                var option = poll.Options.FirstOrDefault(x => x.OptionId == optionId);
                if (option == null) { await ReplyAsync("That option Id does not exist on that poll."); return; }

                //If vote exists, modify
                if (poll.Votes.Any(v => v.UserId == Context.User.Id))
                {
                    var vote = poll.Votes.FirstOrDefault(v => v.UserId == Context.User.Id);
                    if (vote.OptionId == optionId) { await ReplyAsync("You've already voted for that option."); return; }

                    var oldOption = poll.Options.FirstOrDefault(x => x.OptionId == vote.OptionId);
                    vote.OptionId = optionId;

                    var changeResponse = await _pollService.ChangeVote(poll.PollId, vote);
                    if (changeResponse != null)
                        await ReplyAsync($"Changed vote from \"{option.OptionText}\" to \"{poll.Options.FirstOrDefault(x => x.OptionId == oldOption.OptionId).OptionText}\".");
                    return;
                }

                //else create one
                var result = await _pollService.AddVote(poll.PollId, new PollVote { OptionId = optionId, UserId = Context.User.Id });
                if (result != null)
                    await ReplyAsync($"Added a vote for \"{option.OptionText}\".");
            }
        }

        //add showvotes, mypolls, guildpolls

    }
}
