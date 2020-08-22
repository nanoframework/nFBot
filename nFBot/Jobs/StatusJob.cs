using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace nFBot.Jobs
{
    public static class StatusJob
    {
        public static async Task JobTask(Config config, DiscordClient discord)
        {
            while (true)
            {
                await discord.UpdateStatusAsync(new DiscordActivity(config.StatusText, ActivityType.ListeningTo));

                Thread.Sleep(30000);
            }
        }
    }
}
