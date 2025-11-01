using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yuuka_chan.Types.Response.Bills;
using yuuka_chan.Types.Response.Items;

namespace yuuka_chan.Command
{
    [SlashCommandGroup("bill", "For bill uses only")]
    public class BillCommand: ApplicationCommandModule
    {
        private string _billApi = Program.URL + "/api/bills";
        private string _billGetApi = Program.URL + "/api/bills/{0}";
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

        [SlashCommand("create", "Create a new bill")]
        public async Task CreateBill(InteractionContext ctx,
            [Option("description", "The description of the bill")] string description,
            [Option("date_created", "Date the bill is made. Format: DD-MM-YYYY")] string dateCreated,
            [Option("wallet_id", "The wallet ID you want to use")] long walletID)
        {
            string responseBody = string.Empty;
            BillRes createdBill = new BillRes();
            try
            {
                var payload = new
                {
                    Description = description,
                    DateCreated = DateTime.ParseExact(dateCreated, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    WalletUsedID = walletID,
                    Owner = ctx.User.Username
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await Program.Service.PostAsync(_billApi + "/create", content);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
                createdBill = JsonConvert.DeserializeObject<BillRes>(responseBody);
            }
            catch (Exception ex)
            {
                responseBody = ex.Message;
            }
            responseBody = $"## Bill #{createdBill.ID}" +
                $"\n**Description**: {createdBill.Description}\n" +
                $"**Date**: {createdBill.Date}\n" +
                $"**Owner**: {createdBill.Owner}\n" +
                $"**Wallet used**: {createdBill.WalletUsedID.Name}";
            var embed = new DiscordEmbedBuilder
            {
                Title = $"Create a new bill",
                Description = responseBody,
                Color = DiscordColor.Green,
            };
            await ctx.DeferAsync();
            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .AddEmbed(embed)
                .WithContent($"Create a new bill.")); // message as response with embed
        }

        [SlashCommand("add-item", "Add item to bill")]
        public async Task AddItemToBill(InteractionContext ctx,
            [Option("bill_id", "The bill ID you want to add item to")] long billID,
            [Option("item_name", "The name of the item")] string itemName,
            [Option("item_quantity", "The quantity of the item")] long itemQuantity,
            [Option("item_total_value", "The total value of the item")] double itemTotalValue)
        {
            string responseBody = string.Empty;
            AddNewBillItemRes res = new AddNewBillItemRes();
            try
            {
                var payload = new
                {
                    Name = itemName,
                    Quantity = itemQuantity,
                    TotalPrice = itemTotalValue
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await Program.Service.PutAsync(string.Format(_billGetApi, billID) + "/add-items", content);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
                res = JsonConvert.DeserializeObject<AddNewBillItemRes>(responseBody);
            }
            catch (Exception ex)
            {
                responseBody = ex.Message;
            }
            responseBody = $"Added \"{res.Item.Name}\" (x{res.Item.Quantity})\n" +
                $"***Balance***: {res.ExpectedBalance}\n" +
                "--------------------------";
            foreach(BillItemRes item in res.BillItems)
            {
                responseBody += $"\n - {item.Name} (x{item.Quantity}): {item.TotalPrice}";
            }

            var embed = new DiscordEmbedBuilder
            {
                Title = $"Add item to bill",
                Description = responseBody,
                Color = DiscordColor.Green,
            };
            await ctx.DeferAsync();
            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .AddEmbed(embed)
                .WithContent($"Add item to bill.")); // message as response with embed
        }
    }
}
