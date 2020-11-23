using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
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
            await ctx.RespondAsync(output);
        }
    }
}