using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yuuka_chan.Command
{
    public class BillCommand: ApplicationCommandModule
    {
        public BillCommand() { }

        [SlashCommand("testbill", "This is my first slash command")]
        public async Task TestBill(InteractionContext ctx)
        {
            //await ctx.Channel.SendMessageAsync("I want to check if Yuuka is working.");   // only message
            await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, 
                new DiscordInteractionResponseBuilder().WithContent("Just wanna let you know that you did not have any money left."));  // message as response
        }
    }
}
