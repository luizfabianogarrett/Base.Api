using Microsoft.EntityFrameworkCore;
using Modelo.Domain.Entities;
using Modelo.Infra.Data.Mapping;

namespace Modelo.Infra.Data.Context
{
    public class MemoryContext : DbContext
    {
        public DbSet<UserEntity> User { get; set; }

        public MemoryContext(DbContextOptions opt) : base(opt)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>(new UserMap().Configure);
        }
    }
}
