using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using nFBot.Attributes;


namespace nFBot.Modules
{
    public class FaqModule : BaseCommandModule
    {
        [Command("faq")]
        [Usage("faq <tag>")]
        public async Task Faq(CommandContext ctx, string tag)
        {
            
        }
    }
}
