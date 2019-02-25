
using eLTMS.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.DataAccess.Infrastructure
{
    public partial interface IRepositoryHelper
    {
 
        IUnitOfWork GetUnitOfWork();
        TRepository GetRepository<TRepository>(IUnitOfWork unitOfWork)
            where TRepository : class;
    }
    public partial class RepositoryHelper : IRepositoryHelper
    {
       

        public IUnitOfWork GetUnitOfWork()
        {
            var unitOfWork = new UnitOfWork();
            return unitOfWork;
        }

        public TRepository GetRepository<TRepository>(IUnitOfWork unitOfWork)
            where TRepository : class

        {
            if (typeof(TRepository) == typeof(IEmployeeRepository))
            {
                dynamic repo = new EmployeeRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }
            
            TRepository repository = null;
            TryGetRepositoryPartial<TRepository>(unitOfWork, ref repository);
            return repository;
        }

        partial void TryGetRepositoryPartial<TRepository>(IUnitOfWork unitOfWork, ref TRepository repository)
            where TRepository : class;
    }
}
