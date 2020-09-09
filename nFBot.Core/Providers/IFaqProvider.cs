//
// Copyright (c) 2020 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Tools.nFBot.Core.Models;
using System.Threading.Tasks;

namespace nanoFramework.Tools.nFBot.Core.Providers
{
    public interface IFaqProvider
    {
        Task<Faq> GetFaqByTag(string tag);
        Task CreateFaq(Faq faq);
        Task DeleteFaq(string tag);
    }
}