﻿using Modelo.Domain.Entities;
using System.Linq;

namespace Modelo.Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        void Insert(T obj);

        void Update(T obj);

        void Delete(int id);

        T Select(int id);

        IQueryable<T> Select();
    }
}

