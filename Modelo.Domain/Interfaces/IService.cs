using FluentValidation;
using Modelo.Domain.Entities;
using System.Linq;

namespace Modelo.Domain.Interfaces
{
    public interface IService<T> where T : BaseEntity
    {
        T Insert<V>(T obj) where V : AbstractValidator<T>;

        T Update<V>(T obj) where V : AbstractValidator<T>;

        void Delete(int id);

        T Get(int id);

        IQueryable<T> Get();
    }
}
