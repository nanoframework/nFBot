using System.Threading.Tasks;
using nFBot.Core.Models;

namespace nFBot.Core.Providers
{
    public interface IFaqProvider
    {
        Task<Faq> GetFaqByTag(string tag);
        Task CreateFaq(Faq faq);
        Task DeleteFaq(string tag);
    }
}