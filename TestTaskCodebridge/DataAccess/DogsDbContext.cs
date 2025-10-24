using Microsoft.EntityFrameworkCore;
using TestTaskCodebridge.DataAccess.Entitites;

namespace TestTaskCodebridge.DataAccess
{
    public class DogsDbContext : DbContext
    {
        public DogsDbContext(DbContextOptions<DogsDbContext> options) : base(options) { }

        public DbSet<DogEntity> Dogs { get; set; }

        protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase("DogsDbTests");
        }

    }
}
