using System.Threading.Tasks;
using nanoFramework.Tools.nFBot.Core.Models;

namespace nanoFramework.Tools.nFBot.Core.Providers
{
    public interface IFaqProvider
    {
        Task<Faq> GetFaqByTag(string tag);
        Task CreateFaq(Faq faq);
        Task DeleteFaq(string tag);
    }
}