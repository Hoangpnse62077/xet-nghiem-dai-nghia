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
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        List<Appointment> GetAppointmentsByPatientId(int patientId); // DucBM
        List<Appointment> GetAppointmentByPhone(string phoneNumber);
        List<Appointment> GetResultByPatientId(int patientId);
        List<Appointment> GetResultDoneByPatientId(int patientId);
        Appointment GetResultDoneByAppointmentId(int patientId); // DucBM
        List<Appointment> GetAllApp();
        Appointment GetAppById(int appId);
        Appointment GetAppointmentByCode(string code);
        Appointment GetAppointmentById(int id);
        Appointment GetAppointmentByIdInclude(int id);
        List<Appointment> GetResultByAppCode(string appCode);
        int? CountByDate(string sDate);
        string GetLastCode(string sDate);
        List<Appointment> GetAppointmentsByAccountId(int accountId); // DucBM
    }
    public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
    {
        // DucBM
        public List<Appointment> GetAppointmentsByPatientId(int patientId)
        {
            //var sId = patientId.ToString();
            var result = DbSet.AsQueryable()
                .Include(x => x.Patient)
                .Where(x => x.PatientId == patientId && x.IsDeleted != true)
                .Include(x => x.SampleGettings)
                .Include(x => x.SampleGettings.Select(y => y.LabTestings.Select(z => z.LabTest)))
                .Include(x => x.SampleGettings.Select(y => y.Sample))
                .Include(x => x.SampleGettings.Select(y => y.Slot))
                .Include(x => x.Employee)
                .OrderByDescending(x => x.AppointmentId)
                .ToList();
            return result;
        }
        public List<Appointment> GetAllApp()
        {
            var result = DbSet.AsQueryable()
               .Include(x => x.Patient)
               .Include(x => x.SampleGettings.Select(y => y.Sample))
               .ToList();
            return result;
        }
        public Appointment GetAppById(int appId)
        {
            var result = DbSet.AsQueryable()
               .Include(x => x.Patient)
               .Include(x => x.SampleGettings.Select(y => y.Sample))
               .FirstOrDefault(p => p.AppointmentId == appId);
            return result;
        }

        public List<Appointment> GetResultByPatientId(int patientId)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.PatientId == patientId)
                .Include(x => x.Patient)
                .Include(x => x.Employee)

                .Include(x => x.SampleGettings.Select(y => y.Sample))
                .Include(x => x.SampleGettings.Select(y => y.LabTestings.Select(z => z.LabTest)))

                .Include(x => x.SampleGettings.Select(y => y.LabTestings.Select(z => z.LabTestingIndexes)))
                .ToList();
            return result;
        }

        // DucBM
        public Appointment GetResultDoneByAppointmentId(int apId)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.IsDeleted == false && x.AppointmentId == apId
                && (x.Status.ToUpper().Equals("DOCTORDONE") || x.Status.ToUpper().Equals("LABTESTDONE"))) // lack of status doctor done
                .Include(x => x.Patient)
                .Include(x => x.Employee)
                .Include(x => x.SampleGettings.Select(y => y.Sample))
                .Include(x => x.SampleGettings.Select(y => y.LabTestings.Select(z => z.LabTest)))

                .Include(x => x.SampleGettings.Select(y => y.LabTestings.Select(z => z.LabTestingIndexes)))
                .FirstOrDefault();

            return result;
        }

        public List<Appointment> GetResultDoneByPatientId(int patientId)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.PatientId == patientId && x.Status.Contains("DOCTORDONE"))
                .Include(x => x.Patient)
                .Include(x => x.Employee)

                .Include(x => x.SampleGettings.Select(y => y.Sample))
                .Include(x => x.SampleGettings.Select(y => y.LabTestings.Select(z => z.LabTest)))

                .Include(x => x.SampleGettings.Select(y => y.LabTestings.Select(z => z.LabTestingIndexes)))
                .ToList();
            return result;
        }

        public Appointment GetAppointmentById(int id)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.IsDeleted != true && x.AppointmentId.Equals(id))
                .FirstOrDefault();
            return result;
        }

        public Appointment GetAppointmentByIdInclude(int id)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.IsDeleted != true && x.AppointmentId.Equals(id))

                .Include(x => x.SampleGettings.Select(y => y.LabTestings))

                .Include(x => x.SampleGettings)

                .FirstOrDefault();
            return result;
        }

        public Appointment GetAppointmentByCode(string code)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.IsDeleted != true && x.AppointmentCode.Equals(code))
                .Include(x => x.Patient)
                .Include(x => x.SampleGettings.Select(y => y.LabTestings))

                .Include(x => x.SampleGettings)

                .FirstOrDefault();
            return result;
        }

        public List<Appointment> GetResultByAppCode(string appCode)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.AppointmentCode == appCode)
                .Include(x => x.Patient)
                .Include(x => x.Employee)

                .Include(x => x.SampleGettings.Select(y => y.Sample))
                .Include(x => x.SampleGettings.Select(y => y.LabTestings.Select(z => z.LabTest)))

                .Include(x => x.SampleGettings.Select(y => y.LabTestings.Select(z => z.LabTestingIndexes)))
                .ToList();
            return result;
        }

        public List<Appointment> GetAppointmentByPhone(string phoneNumber)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.Patient.PhoneNumber.Trim().Equals(phoneNumber.Trim()))
                .Include(x => x.Patient)
                .ToList();
            return result;
        }

        public int? CountByDate(string sDate)
        {
            if (sDate == null)
            {
                return null;
            }
            sDate = sDate.Trim();
            //var dateLength = "yyyy-MM-dd".Length;
            var result = DbSet.AsQueryable()
                .Count(x => x.IsDeleted == false && x.AppointmentCode.Contains(sDate));
            return result;
        }

        public string GetLastCode(string sDate)
        {
            if (sDate == null)
            {
                return null;
            }
            sDate = sDate.Trim();
            var ap = DbSet.AsQueryable()
                .Where(x => x.AppointmentCode.Contains(sDate))
                .ToList()
                .LastOrDefault();
            if (ap == null)
            {
                return null;
            }
            return ap.AppointmentCode;
        }

        public List<Appointment> GetAppointmentsByAccountId(int accountId)
        {
            var aps = DbSet.AsQueryable()
                .Include(x => x.SampleGettings)
                .Include(x => x.SampleGettings.Select(y => y.LabTestings.Select(z => z.LabTest)))
                .Include(x => x.SampleGettings.Select(y => y.Sample))
                .Include(x => x.SampleGettings.Select(y => y.Slot))
                .Include(x => x.Employee)
                .Include(x => x.Patient.PatientAccounts)
                .Include(x => x.Patient)
                .Where(x => x.IsDeleted != true)
                .OrderByDescending(x => x.AppointmentId)
                .ToList();
            var result = new List<Appointment>();
            foreach (var ap in aps)
            {
                foreach (var acc in ap.Patient.PatientAccounts)
                {
                    if (acc.AccountId == accountId)
                    {
                        result.Add(ap);
                        break;
                    }
                }
            }
            return result;
        }
    }
}
