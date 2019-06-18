using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelo.Application.Controllers;
using Modelo.Domain.Entities;
using Modelo.Domain.Interfaces;
using Modelo.Infra.Data.Context;
using Modelo.Infra.Data.Repository;
using Modelo.UserServive.Services;
using System;
using Xunit;

namespace Modelo.Test
{
    public class UserControllerTest : IDisposable
    {
        private IRepository<UserEntity> _repository;
        private IService<UserEntity> _serviceUser;
        private UserController _controller;
        private DbContext _context;

        public UserControllerTest()
        {
            _context = new MemoryContext(new DbContextOptionsBuilder<MemoryContext>().UseInMemoryDatabase("DataUserMemoryTest").Options);
            _repository = new BaseRepository<UserEntity>(_context);
            _serviceUser = new UserService<UserEntity>(_repository);
            _controller = new UserController(_serviceUser);
            Environment.SetEnvironmentVariable("SecretKey", AuthorizationService.GenerateKey());
        }

        public void Dispose()
        {
            _serviceUser = null;
            _controller = null;
        }

        private UserEntity CreateUser(string email, string password)
        {
            return Builder<UserEntity>.CreateNew()
                .With(s => s.Email = email)
                .With(s => s.Password = password)
                .Build();
        }

        [Fact]
        public void Test_Register_Ok()
        {
            var result = _controller.Register(CreateUser("email@test.com", "test")) as OkObjectResult;
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void Test_Register_Email_Invalid_BadRequest()
        {
            var result = _controller.Register(CreateUser(string.Empty, "pass")) as BadRequestObjectResult;
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Test_Register_Null_BadRequest()
        {
            var result = _controller.Register(null) as BadRequestObjectResult;
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Test_Authenticate_Ok()
        {
            _controller.Register(CreateUser("email2@test.com", "pass"));
            var result = _controller.Authenticate(CreateUser("email2@test.com", "pass")) as OkObjectResult;
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
        }

    }
}
