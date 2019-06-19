using FluentValidation;
using Modelo.Domain.Entities;
using Modelo.Domain.Interfaces;
using Modelo.UserServive.Validators;
using System;
using System.Linq;

namespace Modelo.UserServive.Services
{
    public class UserService<T> : IUserService<T> where T : BaseEntity
    {

        IService<UserEntity> _base;

        public UserService(IService<UserEntity> baseService)
        {
            _base = baseService;
        }

        public void Delete(int id)
        {
            _base.Delete(id);
        }

        public UserEntity Get(int id)
        {
            return _base.Get(id);
        }

        public IQueryable<UserEntity> Get()
        {
            return _base.Get();
        }

        public UserEntity Insert<V>(UserEntity obj) where V : AbstractValidator<T>
        {
            return _base.Insert<UserValidator>(obj);
        }

        public UserEntity Register(UserEntity user)
        {
            var userDb = _base.Get().FirstOrDefault(s => s.Email == user.Email);

            if (userDb != null)
                throw new Exception("Not found");

            user.Password = AuthorizationService.GenerateHashMd5(user.Password, Environment.GetEnvironmentVariable("SecretKey"));

            _base.Insert<UserValidator>(user);

            return user;
        }

        public UserEntity Update<V>(UserEntity obj) where V : AbstractValidator<T>
        {
            return _base.Update<UserValidator>(obj);
        }
    }
}
