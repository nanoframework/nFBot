using System.Threading.Tasks;
using DSharpPlus.CommandsNext;

namespace nanoFramework.Tools.nFBot.Extensions
{
    public static class CommandContextExtensions
    {
        public static async Task ErrorNoPermission(this CommandContext ctx)
        {
            await ctx.RespondAsync("You do not have permission to do this");
        }
    }
}
