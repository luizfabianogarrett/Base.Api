using Microsoft.EntityFrameworkCore;
using Modelo.Domain.Entities;
using Modelo.Domain.Interfaces;
using System.Linq;

namespace Modelo.Infra.Data.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private DbContext context;

        public BaseRepository(DbContext dtc)
        {
            context = dtc;
        }

        public void Insert(T obj)
        {
            obj.Id = 0;
            context.Set<T>().Add(obj);
            context.SaveChanges();
        }

        public void Update(T obj)
        {
            context.Entry(obj).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            context.Set<T>().Remove(Select(id));
            context.SaveChanges();
        }

        public IQueryable<T> Select()
        {
            return context.Set<T>().AsQueryable();
        }

        public T Select(int id)
        {
            return context.Set<T>().Find(id);
        }
    }
}
