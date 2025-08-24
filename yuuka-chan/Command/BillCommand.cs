using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yuuka_chan.Types.Response.Bills;

namespace yuuka_chan.Command
{
    [SlashCommandGroup("bill", "For bill uses only")]
    public class BillCommand: ApplicationCommandModule
    {
        private string _billApi = Program.URL + "/api/bills";
        public BillCommand() { }

        [SlashCommand("get_all", "Get your bills")]
        public async Task GetBills(InteractionContext ctx)
        {
            string responseBody = string.Empty;
            BillRes[] bills = [] ;
            try
            {
                HttpResponseMessage response = await Program.Service.GetAsync(_billApi);

                responseBody = await response.Content.ReadAsStringAsync();
                bills = JsonConvert.DeserializeObject<BillRes[]>(responseBody);
                if (bills == null) throw new Exception();
            } 
            catch (Exception ex)
            {
                responseBody = ex.Message;
            }

            responseBody = $"**Description**: {bills[0].Description}\n" +
                $"**Date**: {bills[0].Date}\n" +
                $"**Owner**: {bills[0].Owner}\n" +
                $"**Wallet used**: {bills[0].WalletUsedID.Name} (ID: {bills[0].WalletUsedID.ID})";

            var embed = new DiscordEmbedBuilder
            {
                Title = $"Bill: {bills[0].ID}",
                Description = responseBody,
                Color = DiscordColor.Green,
            };

            await ctx.DeferAsync();
            
            //await ctx.Channel.SendMessageAsync("I want to check if Yuuka is working.");   // only message
            //await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, 
            //    new DiscordInteractionResponseBuilder().WithContent("Just wanna let you know that you did not have any money left."));  // message as response
            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .AddEmbed(embed)
                .WithContent($"Just wanna let you know that you did not have any money left: {_billApi}")); // message as response with embed
        }
    }
}
