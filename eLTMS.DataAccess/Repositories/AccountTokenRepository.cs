using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace eLTMS.DataAccess.Repositories
{

    public interface IAccountTokenRepository : IRepository<AccountToken>
    {
        List<AccountToken> GetBy(List<int> roleIds);
    }
    public class AccountTokenRepository : RepositoryBase<AccountToken>, IAccountTokenRepository
    {
        public List<AccountToken> GetBy(List<int> roleIds)
        {
            List<AccountToken> result = DbSet.AsQueryable()
                .Include(x => x.Account)
                .Include(x => x.Token)
                .Where(x => x.IsDeleted == false && x.Account.RoleId != null && roleIds.Contains((int)x.Account.RoleId))
                .ToList();
            return result;
        }
    }
}