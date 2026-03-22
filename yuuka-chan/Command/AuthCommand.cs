using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using yuuka_chan.Common;
using yuuka_chan.Types.Request.Auth;
using yuuka_chan.Types.Response.Bills;

namespace yuuka_chan.Command
{
    [SlashCommandGroup("auth", "For bill uses only")]
    public class AuthCommand: ApplicationCommandModule
    {
    }

    public class NonAuthCommand: ApplicationCommandModule
    {
        private string _authApi = Program.URL + "/api/auth";
        public NonAuthCommand() { }

        [SlashCommand("register", "Register new user to the system")]
        public async Task RegisterNewUser(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            string responseBody = string.Empty;
            try
            {
                var payload = new RegisterUserReq
                {
                    UserId = ctx.User.Id.ToString(),
                    PlatformId = int.Parse(YuukaConstants.PLATFORM_ID)
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await Program.PostAuthorizedAsync(_authApi + "/register/third", ctx.User.Id, content);
                responseBody = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                responseBody = ex.Message;
            }

            var embed = new DiscordEmbedBuilder
            {
                Title = "You've created",
                Description = responseBody,
                Color = DiscordColor.Green,
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .AddEmbed(embed)
                .WithContent("Register status."));
        }
    }
}
