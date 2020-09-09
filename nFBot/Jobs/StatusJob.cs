using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using nanoFramework.Tools.nFBot.Core.Configuration;

namespace nanoFramework.Tools.nFBot.Jobs
{
    public static class StatusJob
    {
        public static async Task JobTask(FinalConfig config, DiscordClient discord)
        {
            while (true)
            {
                await discord.UpdateStatusAsync(new DiscordActivity(config.StatusText, ActivityType.ListeningTo));

                Thread.Sleep(30000);
            }
        }
    }
}
