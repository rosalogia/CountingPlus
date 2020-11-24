using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Interpreter;

namespace CountingPlus
{
    public class CommandHandler : BaseCommandModule
    {
        [Command("Hello")]
        public async Task Hello(CommandContext ctx)
        {
            await ctx.RespondAsync("Hi");
        }

        [Command("run")]
        public async Task Run(CommandContext ctx, string code)
        {
            var input = code.Trim('`', '\n');
            var output = PlusPlusSharp.execute(input, 0, 0);
            var brokenString = output.Split(":");
            
            for (var i = 0; i < brokenString.Length; i++)
            {
                if (i % 2 != 0)
                {
                    brokenString[i] = DiscordEmoji.FromName(ctx.Client, $":{brokenString[i]}:").ToString();
                }
            }
            
            var fixedOutput = string.Join("", brokenString);

            await ctx.RespondAsync(fixedOutput);
        }
    }
}