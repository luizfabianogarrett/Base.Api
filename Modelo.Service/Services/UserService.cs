using FluentValidation;
using Modelo.Domain.Entities;
using Modelo.Domain.Interfaces;
using System;
using System.Linq;

namespace Modelo.UserServive.Services
{
    public class UserService<T> : IService<T> where T : BaseEntity
    {
        private IRepository<T> _repository;

        public UserService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public T Insert<V>(T obj) where V : AbstractValidator<T>
        {
            Validate(obj, Activator.CreateInstance<V>());
            
            _repository.Insert(obj);
            return obj;
        }

        public T Update<V>(T obj) where V : AbstractValidator<T>
        {
            Validate(obj, Activator.CreateInstance<V>());

            _repository.Update(obj);
            return obj;
        }

        public void Delete(int id)
        {
           _repository.Delete(id);
        }

        public IQueryable<T> Get() => _repository.Select();

        public T Get(int id)
        {
            return _repository.Select(id);
        }

        private void Validate(T obj, AbstractValidator<T> validator)
        {
            validator.ValidateAndThrow(obj);
        }
    }
}
