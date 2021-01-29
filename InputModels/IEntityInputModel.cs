using HyosungManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.InputModels
{
    public interface IEntityInputModel<TDbContext, TEntity, TKey>
        where TKey : struct
    {
        Task<TEntity> SaveAsEntityAsync(TKey? key, TDbContext context, IServiceProvider services);
    }

    public interface IRefEntityInputModel<TDbContext, TEntity, TKey>
        where TKey : class
    {
        Task<TEntity> SaveAsEntityAsync(TKey key, TDbContext context, IServiceProvider services);
    }

    public interface IAppEntityInputModel<TEntity> : IEntityInputModel<AppDbContext, TEntity, int> { }
    public interface IUserEntityInputModel<TEntity> : IRefEntityInputModel<UserDbContext, TEntity, string> { }
    public interface IUserDbEntityInputModel<TEntity> : IEntityInputModel<UserDbContext, TEntity, int> { }
    public interface IAuthEntityInputModel<TEntity> : IEntityInputModel<AuthDbContext, TEntity, int> { }
}
