using DavidBekeris.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DavidBekeris.Data
{
    public class DavidBekerisContext : DbContext
    {
        public DbSet<IndexModel> IndexModels { get; set; }
        public DbSet<ProjektModel> ProjektModels { get; private set; }
        public DavidBekerisContext(DbContextOptions<DavidBekerisContext> options) : base(options) { }
    }
}
