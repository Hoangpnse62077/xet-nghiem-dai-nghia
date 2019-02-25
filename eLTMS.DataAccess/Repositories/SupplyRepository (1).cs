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
    
    public interface ISupplyRepository : IRepository<Supply>
    {
        List<Supply> GetAllSupply();
    }
    public class SupplyRepository : RepositoryBase<Supply>, ISupplyRepository
    {
        public List<Supply> GetAllSupply()
        {
            var result = DbSet.AsQueryable()
                .Include( x => x.SupplyType)
                .ToList();
            return result;
        }
    }
}
