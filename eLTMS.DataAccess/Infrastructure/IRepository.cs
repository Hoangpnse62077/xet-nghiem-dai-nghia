using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

/********************************************************************/
/*  Reference: https://github.com/Hoangpnse62077/CasptoneProject    */
/*  Gmail: hoangpnse62077@fpt.edu.vn                                */
/********************************************************************/

namespace eLTMS.DataAccess.Infrastructure
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; set; }
    }
    public interface IRepository<TEntity> where TEntity : class/*, ISoftDeletable*/
    {
        IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null);
        Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null);
        TEntity GetOne(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "");
        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null);
        TEntity GetFirst(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null);
        TEntity GetById(object id);
        Task<TEntity> GetByIdAsync(object id);
        int GetCount(Expression<Func<TEntity, bool>> filter = null);
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null);
        bool GetExists(Expression<Func<TEntity, bool>> filter = null);
        Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter = null);
        //IQueryable<TEntity> GetActive();

        void Create(TEntity entity, string createdBy = null);
        void Delete(object id, string modifiedBy = null);
        void Delete(TEntity entity, string modifiedBy = null);
        void Delete(Expression<Func<TEntity, bool>> filter, string modifiedBy = null);
        void Update(TEntity entity, string modifiedBy = null);
        void Update(TEntity entity, string[] fieldsToUpdate, string modifiedBy = null);
    }
}