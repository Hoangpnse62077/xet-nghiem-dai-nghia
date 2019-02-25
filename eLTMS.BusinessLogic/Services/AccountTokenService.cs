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
    public interface IAccountTokenService
    {
        List<AccountToken> GetBy(params int[] roleIds);
    }
    public class AccountTokenService : IAccountTokenService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public AccountTokenService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public List<AccountToken> GetBy(params int[] roleIds)
        {
            var repo = this.RepositoryHelper.GetRepository<IAccountTokenRepository>(UnitOfWork);
            var roleList = roleIds.ToList();
            var results = repo.GetBy(roleList);
            return results;
        }
    }
}
