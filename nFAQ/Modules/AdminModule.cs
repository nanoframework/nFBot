using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using nFAQ.Extensions;

namespace nFAQ.Modules
{
    public class AdminModule : BaseCommandModule
    {
        private readonly Config _config;

        public AdminModule(Config config)
        {
            _config = config;
        }

        [Command("stop")]
        public async Task Stop(CommandContext ctx)
        {
            if (ctx.Member.Roles.All(r => r.Id != _config.AdminRoleId))
            {
                await ctx.ErrorNoPermission();
                return;
            }

            await ctx.RespondAsync("Stopping...");
            await ctx.Client.DisconnectAsync();
            Environment.Exit(0);
        }
    }
}
