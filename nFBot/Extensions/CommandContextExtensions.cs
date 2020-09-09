//
// Copyright (c) 2020 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using DSharpPlus.CommandsNext;
using System.Threading.Tasks;

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
