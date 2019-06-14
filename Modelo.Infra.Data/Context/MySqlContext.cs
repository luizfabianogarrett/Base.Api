using Microsoft.EntityFrameworkCore;
using Modelo.Domain.Entities;
using Modelo.Infra.Data.Mapping;

namespace Modelo.Infra.Data.Context
{
    public class MySqlContext : DbContext
    {
        public DbSet<UserEntity> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySql("Server=[SERVIDOR];Port=[PORTA];Database=modelo;Uid=[USUARIO];Pwd=[SENHA]");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>(new UserMap().Configure);
        }
    }
}
