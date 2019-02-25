using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.DataAccess.Repositories
{
    public interface ILabTestTypeRepository : IRepository<LabTestType>
    {
        List<LabTestType> GetLabTestTypes();
    }
    public class LabTestTypeRepository : RepositoryBase<LabTestType>, ILabTestTypeRepository
    {
        public List<LabTestType> GetLabTestTypes()
        {
            return DbSet.AsQueryable().Include(x => x.LabTestDetails).ToList();
        }
    }

}
