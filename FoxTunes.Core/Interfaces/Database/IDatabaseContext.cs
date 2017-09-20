﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FoxTunes.Interfaces
{
    public interface IDatabaseContext : IDisposable
    {
        IDbConnection Connection { get; }

        IRootDataSets Sets { get; }

        IRootDataQueries Queries { get; }

        IDatabaseSet<T> GetSet<T>() where T : class;

        IDatabaseQuery<T> GetQuery<T>() where T : class;

        bool CanQuery<T>(T item) where T : class;

        IDatabaseQuery<TMember> GetMemberQuery<T, TMember>(T item, Expression<Func<T, TMember>> member)
            where T : class
            where TMember : class;

        IDatabaseQuery<TMember> GetMemberQuery<T, TMember>(T item, Expression<Func<T, ICollection<TMember>>> member)
            where T : class
            where TMember : class;

        void WithAutoDetectChanges(Action action);

        T WithAutoDetectChanges<T>(Func<T> func);

        int SaveChanges();

        Task<int> SaveChangesAsync();

        event EventHandler Disposed;
    }
}