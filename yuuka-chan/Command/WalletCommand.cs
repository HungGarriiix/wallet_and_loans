using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using yuuka_chan.Types.Request.Wallets;
using yuuka_chan.Types.Response.Bills;
using yuuka_chan.Types.Response.Wallets;

namespace yuuka_chan.Command
{
    [SlashCommandGroup("wallet", "For wallet uses only")]
    public class WalletCommand : ApplicationCommandModule
    {
        private string _walletlApi = Program.URL + "/api/wallets";
        private string _createWalletApi = Program.URL + "/api/wallets/add";
        private string _walletGetApi = Program.URL + "/api/wallets/{0}";
        public WalletCommand() { }

        [SlashCommand("get_all", "Get all wallets owned by you")]
        public async Task GetWallets(InteractionContext ctx)
        {
            string responseBody = string.Empty;
            IEnumerable<WalletRes> res = new List<WalletRes>();

            try
            {
                HttpResponseMessage response = await Program.Service.GetAsync(_walletlApi);
                response.EnsureSuccessStatusCode();

                responseBody = await response.Content.ReadAsStringAsync();
                res = JsonConvert.DeserializeObject<IEnumerable<WalletRes>>(responseBody);
                responseBody = string.Empty; // reset result
                foreach (WalletRes wallet in res)
                {
                    responseBody += $"### Wallet #{wallet.ID}\n" +
                        $"**Name:** {wallet.Name}\n" +
                        $"**Balance:** {wallet.Balance}\n" +
                    "------------------------------------\n";
                }
                
            }
            catch (Exception ex)
            {
                responseBody = ex.Message;
            }

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Title = $"Wallet list",
                Description = responseBody,
                Color = DiscordColor.Green
            };

            await ctx.DeferAsync();
            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .AddEmbed(embed)
                .WithContent($"Get wallet list.")); // message as response with embed
        }

        [SlashCommand("get", "Get wallet from your expense")]
        public async Task GetWallet(
            InteractionContext ctx,
            [Option("id", "ID of wallet")] long id
        )
        {
            string responseBody = string.Empty;
            WalletRes res = new();

            try
            {
                HttpResponseMessage response = await Program.Service.GetAsync(string.Format(_walletGetApi, id));
                
                response.EnsureSuccessStatusCode();

                responseBody = await response.Content.ReadAsStringAsync();
                res = JsonConvert.DeserializeObject<WalletRes>(responseBody);

                responseBody = $"**Name:** {res.Name}\n" +
                   $"**Balance:** {res.Balance}\n";
            }
            catch (Exception ex)
            {
                responseBody = ex.Message;
            }

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Title = $"Wallet #{res.ID} information",
                Description = responseBody,
                Color = DiscordColor.Green
            };

            await ctx.DeferAsync();
            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .AddEmbed(embed)
                .WithContent($"Get wallet by ID.")); // message as response with embed
        }

        [SlashCommand("create", "Create new wallet for user")]
        public async Task CreateWallet(
            InteractionContext ctx, 
            [Option("name", "Wallet name")] string name,
            [Option("balance", "Wallet initial balance")] double balance
        )
        {
            string responseBody = string.Empty;
            CreateWalletReq req = new CreateWalletReq(name, (float)balance);
            WalletRes res = new WalletRes();

            try
            {
                HttpResponseMessage response = await Program.Service.PostAsJsonAsync(_createWalletApi, req);
                response.EnsureSuccessStatusCode();

                responseBody = await response.Content.ReadAsStringAsync();
                res = JsonConvert.DeserializeObject<WalletRes>(responseBody);
                responseBody = $"**Name:** {res.Name}\n" +
                   $"**Balance:** {res.Balance}\n";
            }
            catch (Exception ex)
            {
                responseBody = ex.Message;
            }

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Title = $"Wallet #{res.ID} created",
                Description = responseBody,
                Color = DiscordColor.Green
            };

            await ctx.DeferAsync();
            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .AddEmbed(embed)
                .WithContent($"Wallet created.")); // message as response with embed
        }
        
    }
}
