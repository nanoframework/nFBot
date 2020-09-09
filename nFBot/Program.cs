//
// Copyright (c) 2020 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
        private static string s_WelcomeMessage = "Welcome to nanoFramework! You can use the !help command for information on what I am able to do for you!";

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

        public static async Task Main(string[] args)
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

            var builder = new HostBuilder()
                .ConfigureAppConfiguration(b => b.AddCommandLine(args))
                .ConfigureLogging((context,
                                   b) =>
                {
                if (context.Configuration["environment"] == Environments.Development)
                    {
                        b.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
                        b.AddConsole();
                    }
                    else
                    {
                        b.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);

                        // If this key exists in any config, use it to enable App Insights
                        string appInsightsKey = context.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
                        if (!string.IsNullOrEmpty(appInsightsKey))
                        {
                            b.AddApplicationInsightsWebJobs(o => o.InstrumentationKey = appInsightsKey);
                        }
                    }
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton(_config);
                })
                .ConfigureWebJobs(webJobConfiguration =>
                {
                    webJobConfiguration.AddAzureStorageCoreServices();
                    webJobConfiguration.AddAzureStorage();
                    webJobConfiguration.AddTimers();
                })
                .UseConsoleLifetime();

            //  TODO
            // mode this to a webjob method
            _discord = new DiscordClient(new DiscordConfiguration
            {
                Token = _config.Token,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = DSharpPlus.LogLevel.Debug
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

            /////////////////////////////////////////

            var host = builder.Build();
            using (host)
            {
                await host.RunAsync();
            }
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
