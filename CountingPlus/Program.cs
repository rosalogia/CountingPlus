using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CountingPlus
{
    public struct ConfigJson
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
        
        [JsonProperty("prefix")]
        public string CommandPrefix { get; private set; }
    }
    
    class Program
    {
        public DiscordClient Client { get; set; }
        public static CommandsNextExtension Commands;
        
        static void Main(string[] args)
        {
            var program = new Program();
            program.MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task MainAsync(string[] args)
        {
            var json = "";

            try
            {
                using (var fs = File.OpenRead("config.json"))
                using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                    json = await sr.ReadToEndAsync();
            }
            catch (FileNotFoundException _)
            {
                Console.WriteLine("Could not find a configuration file. Are you missing one?");
                return;
            }

            var cfgjson = JsonConvert.DeserializeObject<ConfigJson>(json);
            var cfg = new DiscordConfiguration
            {
                Token = cfgjson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };
            
            Client = new DiscordClient(cfg);

            Commands = Client.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] { cfgjson.CommandPrefix }
            });
            
            Commands.RegisterCommands<CommandHandler>();
            
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}