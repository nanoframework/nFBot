using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using nFBot.Core.Data;
using nFBot.Core.Entities;
using nFBot.Core.Models;

namespace nFBot.Core.Providers
{
    public class DatabaseFaqProvider : IFaqProvider
    {
        private readonly FaqDbContext _db;

        public DatabaseFaqProvider(FaqDbContext db)
        {
            _db = db;
        }
        
        public async Task<Faq> GetFaqByTag(string tag)
        {
           FaqEntity faq =  await _db.Faq.FirstOrDefaultAsync(f => String.Equals(f.Tag, tag, StringComparison.CurrentCultureIgnoreCase));

           if (faq == null) return null;
           
           return new Faq
           {
               Content = faq.Content,
               CreatedDate = faq.CreatedDate,
               Creator =  faq.Creator,
               Tag = faq.Tag
           };
        }

        public async Task CreateFaq(Faq faq)
        {
            FaqEntity newFaq = new FaqEntity
            {
                Content = faq.Content,
                CreatedDate = DateTime.Now,
                Creator = faq.Creator,
                Tag = faq.Tag
            };

            await _db.AddAsync(newFaq);

            await _db.SaveChangesAsync();
        }

        public async Task DeleteFaq(string tag)
        {
            FaqEntity faq =  await _db.Faq.FirstOrDefaultAsync(f => String.Equals(f.Tag, tag, StringComparison.CurrentCultureIgnoreCase));

            if (faq == null) return;

            _db.Faq.Remove(faq);

            await _db.SaveChangesAsync();
        }
    }
}