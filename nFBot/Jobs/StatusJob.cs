//
// Copyright (c) 2020 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using DSharpPlus;
using DSharpPlus.Entities;
using nanoFramework.Tools.nFBot.Core.Configuration;
using System.Threading;
using System.Threading.Tasks;

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
