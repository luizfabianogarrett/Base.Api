using FluentValidation;
using Modelo.Domain.Entities;
using System.Linq;

namespace Modelo.Domain.Interfaces
{
    public interface IUserService<T> where T : BaseEntity
    {
        UserEntity Insert<V>(UserEntity obj) where V : AbstractValidator<T>;

        UserEntity Update<V>(UserEntity obj) where V : AbstractValidator<T>;

        void Delete(int id);

        UserEntity Get(int id);

        IQueryable<UserEntity> Get();
        UserEntity Register(UserEntity obj);
    }
}
