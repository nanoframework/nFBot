//
// Copyright (c) 2020 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using nanoFramework.Tools.nFBot.Core.Configuration;
using nanoFramework.Tools.nFBot.Core.Data;
using nanoFramework.Tools.nFBot.Core.Providers;
using nanoFramework.Tools.nFBot.Handlers;
using nanoFramework.Tools.nFBot.Jobs;
using nanoFramework.Tools.nFBot.Modules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace nanoFramework.Tools.nFBot
{
    public class Program
    {
        private static DiscordClient _discord;
        private static CommandsNextExtension _commands;
        private static LoadedConfig _loadedConfig;

        private static readonly FinalConfig _config = new FinalConfig();
        private static readonly ServiceCollection ServiceCollection = new ServiceCollection();

        public static bool IsReleaseMode()
        {
            #if DEBUG
            return false;
            #else
            return true;
            #endif
        }
        
        static void Main()
        {
            try
            {
                _loadedConfig = JsonConvert.DeserializeObject<LoadedConfig>(File.ReadAllText("./config.json"));
            }
            catch
            {
                Console.WriteLine("Unable to load config from ./config.json");
                Console.WriteLine();
                Console.WriteLine("Ensure the config exists and is valid. Use the config.example.json for reference");
                Console.WriteLine();
                Console.WriteLine("Press enter to continue");

                Console.ReadLine();

                Environment.Exit(1);
            }

            _config.Prefix = _loadedConfig.Prefix;
            _config.StatusText = _loadedConfig.StatusText;
            _config.WelcomeMessage = _loadedConfig.WelcomeMessage;
            _config.StorageMode = _loadedConfig.StorageMode;
            _config.AdminRoleId = _loadedConfig.AdminRoleId;

            if (IsReleaseMode())
            {
                _config.Token = Environment.GetEnvironmentVariable("token");
                _config.StorageConnectionString = Environment.GetEnvironmentVariable("storage_connection_string");
            }
            else
            {
                _config.Token = _loadedConfig.DebugToken;
                _config.StorageConnectionString = _loadedConfig.DebugStorageConnectionString;
            }

            switch (_config.StorageMode)
            {
                case "mysql":
                    ServiceCollection.AddDbContext<FaqDbContext>(builder =>
                    {
                        builder.UseMySql(_config.StorageConnectionString);
                    });

                    ServiceCollection.AddTransient<IFaqProvider, DatabaseFaqProvider>();
                    
                    break;
                
                case "mssql":
                    ServiceCollection.AddDbContext<FaqDbContext>(builder =>
                    {
                        builder.UseSqlServer(_config.StorageConnectionString);
                    });

                    ServiceCollection.AddTransient<IFaqProvider, DatabaseFaqProvider>();
                    
                    break;
                
                default:
                    Console.WriteLine("Invalid storage mode selected. Refer to README for valid modes");
                    Console.WriteLine();
                    Console.WriteLine("Press enter to continue");

                    Console.ReadLine();

                    Environment.Exit(1);
                    break;
            }

            MainAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            ServiceCollection.AddSingleton(_config);

            _discord = new DiscordClient(new DiscordConfiguration
            {
                Token = _config.Token,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            _commands = _discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new List<string>
                {
                    _loadedConfig.Prefix
                },
                EnableMentionPrefix = true,
                Services = ServiceCollection.BuildServiceProvider(),
                IgnoreExtraArguments = true,
                EnableDefaultHelp = false
            });

            _commands.CommandErrored += CommandErroredUsageHandler.Handle;

            _commands.RegisterCommands<AdminModule>();
            _commands.RegisterCommands<HelpModule>();
            _commands.RegisterCommands<FaqModule>();
            
            _discord.GuildMemberAdded += DiscordOnGuildMemberAdded;

            _discord.Ready += InitialStart;

            await _discord.ConnectAsync();

            await Task.Delay(-1);
        }

        private static async Task DiscordOnGuildMemberAdded(GuildMemberAddEventArgs e)
        {
            await e.Member.SendMessageAsync(_config.WelcomeMessage);
        }

        private static readonly AsyncEventHandler<ReadyEventArgs> InitialStart = async e =>
        {
            e.Client.Ready -= InitialStart;

            new Thread(async () => { await StatusJob.JobTask(_config, _discord); }).Start();
        };
    }
}
