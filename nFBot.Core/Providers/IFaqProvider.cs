using nFBot.Core.Models;

namespace nFBot.Core.Providers
{
    public interface IFaqProvider
    {
        Faq GetFaqByTag(string tag);
        void CreateFaq(Faq faq);
    }
}