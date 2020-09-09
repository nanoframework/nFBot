using Microsoft.EntityFrameworkCore;
using nanoFramework.Tools.nFBot.Core.Entities;

namespace nanoFramework.Tools.nFBot.Core.Data
{
    public class FaqDbContext : DbContext
    {
        public FaqDbContext(DbContextOptions<FaqDbContext> options) : base(options) {}
        
        public DbSet<FaqEntity> Faq { get; set; }
    }
}