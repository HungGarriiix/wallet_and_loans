using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using yuuka_chan.Types.Request.Auth;
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
            await ctx.DeferAsync();

            string responseBody = string.Empty;
            IEnumerable<WalletRes> res = new List<WalletRes>();

            try
            {
                HttpResponseMessage response = await Program.GetAuthorizedAsync(_walletlApi, ctx.User.Id);
                response.EnsureSuccessStatusCode();

                responseBody = await response.Content.ReadAsStringAsync();
                res = JsonConvert.DeserializeObject<IEnumerable<WalletRes>>(responseBody) ?? new List<WalletRes>();
                responseBody = string.Empty;
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

            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .AddEmbed(embed)
                .WithContent($"Get wallet list."));
        }

        [SlashCommand("get", "Get wallet from your expense")]
        public async Task GetWallet(
            InteractionContext ctx,
            [Option("id", "ID of wallet")] long id
        )
        {
            await ctx.DeferAsync();

            string responseBody = string.Empty;
            WalletRes res = new();

            try
            {
                HttpResponseMessage response = await Program.GetAuthorizedAsync(string.Format(_walletGetApi, id), ctx.User.Id);
                response.EnsureSuccessStatusCode();

                responseBody = await response.Content.ReadAsStringAsync();
                res = JsonConvert.DeserializeObject<WalletRes>(responseBody)!;

                responseBody = $"**Name:** {res.Name}\n" +
                   $"**Balance:** {res.Balance}\n";
            }
            catch (Exception ex)
            {
                responseBody = ex.Message;
            }

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Title = $"Wallet #{res?.ID} information",
                Description = responseBody,
                Color = DiscordColor.Green
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .AddEmbed(embed)
                .WithContent($"Get wallet by ID."));
        }

        [SlashCommand("create", "Create new wallet for user")]
        public async Task CreateWallet(
            InteractionContext ctx, 
            [Option("name", "Wallet name")] string name,
            [Option("balance", "Wallet initial balance")] double balance
        )
        {
            await ctx.DeferAsync();

            string responseBody = string.Empty;
            CreateWalletReq req = new CreateWalletReq(name, (float)balance);
            WalletRes res = new WalletRes();

            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await Program.PostAuthorizedAsync(_createWalletApi, ctx.User.Id, jsonContent);
                response.EnsureSuccessStatusCode();

                responseBody = await response.Content.ReadAsStringAsync();
                res = JsonConvert.DeserializeObject<WalletRes>(responseBody)!;
                responseBody = $"**Name:** {res.Name}\n" +
                   $"**Balance:** {res.Balance}\n";
            }
            catch (Exception ex)
            {
                responseBody = ex.Message;
            }

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Title = $"Wallet #{res?.ID} created",
                Description = responseBody,
                Color = DiscordColor.Green
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .AddEmbed(embed)
                .WithContent($"Wallet created."));
        }

        [SlashCommand("get_id", "Get user ID")]
        public async Task TestGetId(InteractionContext ctx)
        {
            await ctx.DeferAsync();
            Console.WriteLine($"[get_id] called by {ctx.User.Id}, URL={Program.URL}");

            string url = Program.URL + "/api/wallets/test";
            string result = string.Empty;
            try
            {
                // Step 1: login
                Console.WriteLine($"[get_id] calling login...");
                var token = await Program.GetTokenAsync(ctx.User.Id);
                Console.WriteLine($"[get_id] token received: {token[..20]}...");

                // Step 2: call test endpoint
                HttpResponseMessage response = await Program.GetAuthorizedAsync(url, ctx.User.Id);
                Console.WriteLine($"[get_id] API status: {response.StatusCode}");
                result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[get_id] API response: {result}");
            }
            catch (Exception ex)
            {
                result = $"ERROR: {ex.GetType().Name}: {ex.Message}";
                Console.WriteLine($"[get_id] EXCEPTION: {ex}");
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .WithContent($"Discord ID: `{ctx.User.Id}`\nAPI: {result}"));
        }
    }
}
