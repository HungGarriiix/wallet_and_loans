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
    [SlashCommandGroup("auth", "Authentication and User Management")]
    public class AuthCommand: ApplicationCommandModule
    {
        private string _authApi => Program.URL + "/api/auth";

        [SlashCommand("register", "Register new user to the system")]
        public async Task RegisterNewUser(InteractionContext ctx,
            [Option("display_name", "Display name (Optional)")] string? displayName = null)
        {
            await ctx.DeferAsync();

            string responseBody = string.Empty;
            try
            {
                var payload = new RegisterUserReq
                {
                    UserId = ctx.User.Id.ToString(),
                    PlatformId = int.Parse(YuukaConstants.PLATFORM_ID),
                    DisplayName = displayName == null ? ctx.User.Username : displayName,
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
                Title = "Registration Status",
                Description = responseBody,
                Color = responseBody.Contains("created") ? DiscordColor.Green : DiscordColor.Yellow,
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .AddEmbed(embed)
                .WithContent("Register status."));
        }
    }
}
