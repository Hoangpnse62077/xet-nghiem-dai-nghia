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
    public interface ISampleRepository : IRepository<Sample>
    {
        List<Sample> GetAllSamples();
        Sample GetSampleById(int id);
    }
    public class SampleRepository : RepositoryBase<Sample>, ISampleRepository
    {
        public List<Sample> GetAllSamples()
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.IsDeleted == false)
                .Include(x => x.LabTests)
                .Include(x => x.SampleGroup);
            foreach(var rs in result)
            {
                rs.LabTests = rs.LabTests.Where(x => x.IsDeleted == false).ToList();
            }
            return result.ToList();

        }

        public Sample GetSampleById(int id)
        {
            var result = DbSet.Where(s => s.SampleId == id).ToList().FirstOrDefault();
            return result;
        }
    }
}
