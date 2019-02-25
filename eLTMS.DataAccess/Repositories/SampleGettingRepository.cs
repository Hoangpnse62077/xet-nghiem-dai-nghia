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
    public interface ISampleGettingRepository : IRepository<SampleGetting>
    {
        List<SampleGetting> GetAll();
        List<SampleGetting> GetAllIncludeApp();
        List<SampleGetting> GetAll2();
        SampleGetting GetByCodeForNurse(string code); // DucBM
        SampleGetting GetFirst(int sampleId, string gettingDate, int patientId);
    }
    public class SampleGettingRepository : RepositoryBase<SampleGetting>, ISampleGettingRepository
    {
        public List<SampleGetting> GetAll()
        {
            var results = DbSet.AsQueryable()
                .Include(x => x.LabTestings)
                .ToList();
            return results;
        }
        public List<SampleGetting> GetAll2()
        {
            var results = DbSet.AsQueryable()
                .Include(p => p.Slot)
                .ToList();
            return results;
        }
        public List<SampleGetting> GetAllIncludeApp()
        {
            var results = DbSet.AsQueryable()
                .Include(p => p.Appointment)
                .ToList();
            return results;
        }

        // DucBM
        public SampleGetting GetByCodeForNurse(string code) // Stt: WAITING - chờ lấy mẫu
        {
            int id = int.Parse(code); // temp
            var result = DbSet.AsQueryable()
                .Where(x => x.Status.ToUpper().Contains("WAITING") && x.IsDeleted == false && x.SampleGettingId.Equals(id))
                .Include(x => x.Appointment.Patient)
                .Include(x => x.Sample)
                .FirstOrDefault();
            return result;
        }

        public SampleGetting GetFirst(int sampleId, string gettingDate, int patientId)
        {
            DateTime dt = DateTime.Parse(gettingDate);
            var result = DbSet.AsQueryable()
                .Include(x => x.Appointment)
                .Where(x => x.IsDeleted == false
                    && x.SampleId == sampleId
                    && x.Appointment.PatientId == patientId
                    && DbFunctions.TruncateTime(x.GettingDate) == DbFunctions.TruncateTime(dt))
                .FirstOrDefault();
            return result;
        }
    }
}
