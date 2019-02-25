using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using eLTMS.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.BusinessLogic.Services
{
    public interface IResultService
    {
        List<LabTestType> GetLabTestTypes();
    }
    public class ResultService : IResultService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public ResultService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public List<LabTestType> GetLabTestTypes()
        {
            var repo = this.RepositoryHelper.GetRepository<ILabTestTypeRepository>(UnitOfWork);
            var result = repo.GetLabTestTypes();
            return result;
        }
    }
}
