using FizzWare.NBuilder;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Modelo.Application.Controllers;
using Modelo.Domain.Entities;
using Modelo.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Modelo.Test
{
    public class UserControllerTest : IDisposable
    {
        private Mock<IService<UserEntity>> _serviceUser;
        private UserController _controller;

        public UserControllerTest()
        {
            _serviceUser = new Mock<IService<UserEntity>>();
            _controller = new UserController(_serviceUser.Object);
        }

        public void Dispose()
        {
            _serviceUser = null;
            _controller = null;
        }

        [Fact]
        public void Test_Register_Ok()
        {
            var newUser = Builder<UserEntity>.CreateNew().Build();
            List<UserEntity> users = new List<UserEntity>();
            IQueryable<UserEntity> queryableUsers = users.AsQueryable();
            _serviceUser.Setup(x => x.Get()).Returns(queryableUsers);
            _serviceUser.Setup(s => s.Insert<AbstractValidator<UserEntity>>(newUser)).Returns(newUser);
            var result = _controller.Register(newUser) as OkObjectResult;
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(newUser.Id, result.Value);
        }

    }
}
