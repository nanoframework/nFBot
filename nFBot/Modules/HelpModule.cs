﻿//
// Copyright (c) 2020 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using nanoFramework.Tools.nFBot.Core.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace nanoFramework.Tools.nFBot.Modules
{
    public class HelpModule : BaseCommandModule
    {
        private readonly FinalConfig _config;

        public HelpModule(FinalConfig config)
        {
            _config = config;
        }
        
        [Command("help")]
        public async Task Help(CommandContext ctx)
        {
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Color = new Optional<DiscordColor>(new DiscordColor("#02ABF0")),
                Title = "Help",
            };

            string faqSection = "faq - View an FAQ tag";

            if (ctx.Member.Roles.Any(r => r.Id == _config.AdminRoleId))
            {
                faqSection += "\ncreatefaq - Create a new FAQ tag";
                faqSection += "\ndeletefaq - Delete and existing FAQ tag";
            }
            
            embed.AddField("FAQ", faqSection);

            await ctx.RespondAsync(embed: embed);
        }
    }
}
