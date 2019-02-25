using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

/********************************************************************/
/*  Reference: https://github.com/Hoangpnse62077/CasptoneProject    */
/*  Gmail: hoangpnse62077@fpt.edu.vn                                */
/********************************************************************/

namespace eLTMS.DataAccess.Infrastructure
{
    public abstract class RepositoryBase<TEntity> : RepositoryBase, IRepository<TEntity> where TEntity : class/*, ISoftDeletable*/
    {
        private DbSet<TEntity> _dbset;
        protected DbSet<TEntity> DbSet
        {
            get
            {
                if (_dbset == null)
                {
                    _dbset = UnitOfWork.Context.Set<TEntity>();
                }
                return _dbset;
            }
        }

        protected virtual IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            includeProperties = includeProperties ?? string.Empty;
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        public virtual IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return GetQueryable(null, orderBy, includeProperties, skip, take).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return await GetQueryable(null, orderBy, includeProperties, skip, take).ToListAsync();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return GetQueryable(filter, orderBy, includeProperties, skip, take).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return await GetQueryable(filter, orderBy, includeProperties, skip, take).ToListAsync();
        }

        public virtual TEntity GetOne(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            return GetQueryable(filter, null, includeProperties).SingleOrDefault();
        }

        public virtual async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null)
        {
            return await GetQueryable(filter, null, includeProperties).SingleOrDefaultAsync();
        }

        public virtual TEntity GetFirst(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            return GetQueryable(filter, orderBy, includeProperties).FirstOrDefault();
        }

        public virtual async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null)
        {
            return await GetQueryable(filter, orderBy, includeProperties).FirstOrDefaultAsync();
        }

        public virtual TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }

        public virtual Task<TEntity> GetByIdAsync(object id)
        {
            return DbSet.FindAsync(id);
        }

        public virtual int GetCount(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQueryable(filter).Count();
        }

        public virtual Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQueryable(filter).CountAsync();
        }

        public virtual bool GetExists(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQueryable(filter).Any();
        }

        public virtual Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQueryable(filter).AnyAsync();
        }

        public void Create(TEntity entity, string createdBy = null)
        {
            DbSet.Add(entity);
        }

        public virtual void Delete(object id, string modifiedBy = null)
        {
            var entity = DbSet.Find(id);
            Delete(entity, modifiedBy);
        }

        public virtual void Delete(TEntity entity, string modifiedBy = null)
        {
            if (UnitOfWork.Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> filter, string modifiedBy = null)
        {
            var objects = GetQueryable(filter);
            foreach (var obj in objects)
            {
                Delete(obj, modifiedBy);
            }
        }

        public virtual void Update(TEntity entity, string modifiedBy = null)
        {
            if (UnitOfWork.Context.Entry(entity).State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }
            UnitOfWork.Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Update(TEntity entity, string[] fieldsToUpdate, string modifiedBy = null)
        {
            this.DbSet.Attach(entity);
            var dbEntityEntry = UnitOfWork.Context.Entry(entity);
            var dbProperties = dbEntityEntry.GetDatabaseValues();
            foreach (var property in dbEntityEntry.OriginalValues.PropertyNames.Where(p => !p.Equals("RecordVersion", StringComparison.InvariantCultureIgnoreCase)))
            {
                var original = dbProperties.GetValue<object>(property);
                var current = dbEntityEntry.CurrentValues.GetValue<object>(property);
                if (fieldsToUpdate.Contains(property) && (original == null && current != null || original != null && current == null || original != null && current != null) && ((original != null && !original.Equals(current)) || (current != null && !current.Equals(original))))
                {
                    dbEntityEntry.Property(property).IsModified = true;
                }
                else
                {
                    dbEntityEntry.Property(property).OriginalValue = dbEntityEntry.Property(property).CurrentValue = original;
                    dbEntityEntry.Property(property).IsModified = false;
                }
            }
        }

        //public IQueryable<TEntity> GetActive()
        //{
        //    return UnitOfWork.Context.Set<TEntity>().Where(x => x.IsDeleted == false);
        //}
    }
    public abstract class RepositoryBase : IRepository
    {
        public IUnitOfWork UnitOfWork { get; set; }
    }
}