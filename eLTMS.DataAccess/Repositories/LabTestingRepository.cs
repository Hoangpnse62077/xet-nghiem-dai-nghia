using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace eLTMS.DataAccess.Repositories
{
    public interface ILabTestingRepository : IRepository<LabTesting>
    {
        List<LabTesting> GetAllLabTesting();
        List<LabTesting> GetAllResult(string s);
        List<LabTesting> GetAllLabTestingResult();
        List<LabTesting> GetAllLabTestings();
        List<LabTesting> GetAllLabTestingsFail();
        List<LabTesting> GetAllLabTestingDate(string date);
        LabTesting GetLabTestingById(int id);
        List<LabTesting> GetLabTestingByListId(List<int> ids);
        List<LabTesting> GetAllLabTestingHaveAppointmentCode(String code);
        List<LabTesting> GetAllBySampleGettingId(int sampleGettingId);
    }
    public class LabTestingRepository : RepositoryBase<LabTesting>, ILabTestingRepository
    {
        public List<LabTesting> GetAllLabTesting()
        {
            var result = DbSet.AsQueryable()
                .Include(x => x.LabTest)
                .Include(x => x.LabTestingIndexes)
                .ToList();
            return result;
        }

        public List<LabTesting> GetAllLabTestings()
        {

            var result = DbSet.AsQueryable()
             .Where(x => x.Status.Contains("Waiting") && x.IsDeleted == false)
             .Include(x => x.LabTest)
             .Include(x => x.SampleGetting.Appointment)
             .Include(x => x.SampleGetting.Sample)
             .ToList();
            return result;

        }
        public List<LabTesting> GetAllLabTestingsFail()
        {

            var result = DbSet.AsQueryable()
             .Where(x => x.Status.Contains("Fail") && x.IsDeleted == false)
             .Include(x => x.LabTest)
             .Include(x => x.SampleGetting.Appointment.Patient)
             .Include(x => x.SampleGetting.Appointment)
             .Include(x => x.SampleGetting.Sample)
             .ToList();
            return result;

        }
        public List<LabTesting> GetAllLabTestingDate(string date)
        {

            var result = DbSet.AsQueryable()
             .Where(x => x.Status.Contains("Waiting")&&x.SampleGetting.Appointment.AppointmentCode.Contains(date) && x.IsDeleted == false)
             .Include(x => x.LabTest)
             .Include(x => x.SampleGetting.Appointment)
             .Include(x => x.SampleGetting.Sample)
             .ToList();
            return result;

        }
        public List<LabTesting> GetAllLabTestingHaveAppointmentCode(String code)
        {

            var result = DbSet.AsQueryable()
             .Where(x => x.SampleGetting.Appointment.AppointmentCode.Contains(code) && x.IsDeleted == false)
             .Include(x => x.LabTest)
             .Include(x => x.SampleGetting.Appointment)
             .Include(x => x.SampleGetting.Sample)
             .ToList();
            return result;

        }
        public List<LabTesting> GetAllLabTestingResult()
        {

            var result = DbSet.AsQueryable()
             .Where(x => x.Status.Contains("LabtestDone") && x.IsDeleted == false)
             .Include(x => x.LabTest)
             .Include(x => x.SampleGetting.Appointment)
             .Include(x => x.SampleGetting.Sample)
             .Include(x => x.SampleGetting.Appointment.Patient)
             .ToList();
            return result;

        }
        public List<LabTesting> GetAllResult(string s)
        {

            var result = DbSet.AsQueryable()
             .Where(x => x.Status.ToUpper().Contains("DOCTORDONE") && x.IsDeleted == false && x.SampleGetting.Appointment.AppointmentCode.Contains(s))
             .Include(x => x.LabTest)
             .Include(x => x.SampleGetting.Appointment)
             .Include(x => x.SampleGetting.Sample)
            .Include(x => x.SampleGetting.Appointment.Patient)
             .ToList();
            return result;

        }
        public LabTesting GetLabTestingById(int id)
        {
            var result = DbSet.Where(s => s.LabTestingId == id).ToList().FirstOrDefault();
            return result;
        }

        public List<LabTesting> GetLabTestingByListId(List<int> ids)
        {
            var result = DbSet.Where(s => ids.Contains(s.LabTestingId)).ToList();

            return result;
        }

        public List<LabTesting> GetAllBySampleGettingId(int sampleGettingId)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.SampleGettingId == sampleGettingId && x.IsDeleted == false)
                .ToList();
            return result;
        }
    }
}
