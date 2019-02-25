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
        List<Supply> GetAllSupply(string suppliesCode);
        Supply GetSimpleById(int id);
    }
    public class SupplyRepository : RepositoryBase<Supply>, ISupplyRepository
    {
        public List<Supply> GetAllSupply(string suppliesCode)
        {
            var result = DbSet.AsQueryable()
                .Include(x => x.SupplyType)
                .Where(x => (x.SuppliesCode.Contains(suppliesCode) || x.SuppliesName.Contains(suppliesCode)) && x.IsDeleted == false)           
                .ToList();
            return result;
        }

        public Supply GetSimpleById(int id)
        {
            var result = DbSet.AsQueryable()               
                .Include(x => x.SupplyType)
                .SingleOrDefault(x => x.SuppliesId == id);
            return result;
        }
    }
}
