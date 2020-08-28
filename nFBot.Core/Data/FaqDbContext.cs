using Microsoft.EntityFrameworkCore;
using nFBot.Core.Entities;

namespace nFBot.Core.Data
{
    public class FaqDbContext : DbContext
    {
        public FaqDbContext(DbContextOptions<FaqDbContext> options) : base(options) {}
        
        public DbSet<FaqEntity> Faq { get; set; }
    }
}