using Microsoft.EntityFrameworkCore;
using Modelo.Domain.Entities;

namespace Modelo.Infra.Data.Context
{
    public class MemoryContext : DbContext
    {
        public DbSet<UserEntity> User { get; set; }

        public MemoryContext(DbContextOptions opt) : base(opt)
        {

        }
    }
}
