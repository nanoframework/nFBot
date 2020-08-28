using System;
using System.Linq;
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
        
        public Faq GetFaqByTag(string tag)
        {
           FaqEntity faq =  _db.Faq.FirstOrDefault(f => String.Equals(f.Tag, tag, StringComparison.CurrentCultureIgnoreCase));

           if (faq == null) return null;
           
           return new Faq
           {
               Content = faq.Content,
               CreatedDate = faq.CreatedDate,
               Creator =  faq.Creator,
               Tag = faq.Tag
           };
        }

        public void CreateFaq(Faq faq)
        {
            FaqEntity newFaq = new FaqEntity
            {
                Content = faq.Content,
                CreatedDate = DateTime.Now,
                Creator = faq.Creator,
                Tag = faq.Tag
            };

            _db.Add(newFaq);

            _db.SaveChanges();
        }
    }
}