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
    public interface ISampleGroupRepository : IRepository<SampleGroup>
    {
       
        List<SampleGroup> GetAllSampleGroup();
        SampleGroup GetSampleById(int id);
    }
    public class SampleGroupRepository : RepositoryBase<SampleGroup>, ISampleGroupRepository
    {
        
        public List<SampleGroup> GetAllSampleGroup()
        {
           
            var result = DbSet.AsQueryable()
             .Where(x =>  x.IsDeleted == false)
             .ToList();
            return result;

        }

        public SampleGroup GetSampleById(int id)
        {
            var result = DbSet.Where(s => s.SampleGroupId == id).ToList().FirstOrDefault();
            return result;
        }
    }
}
