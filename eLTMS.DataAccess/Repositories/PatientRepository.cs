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
    public interface IPatientRepository : IRepository<Patient>
    {
        List<Patient> GetAllPatient(string phoneNumber);
        Patient GetSimpleById(int id);
        string GetLastPatientCode(); // DucBM
        Patient GetBy(int accountId, string fullName, DateTime dateOfBirth); // DucBM
    }


    public class PatientRepository : RepositoryBase<Patient>, IPatientRepository
    {
        public List<Patient> GetAllPatient(string phoneNumber)
        {
            var result = DbSet.AsQueryable()
                .Where(x => (x.PhoneNumber.Contains(phoneNumber) || x.PatientId.ToString().Contains(phoneNumber) || x.FullName.Contains(phoneNumber) || x.PatientCode.Contains(phoneNumber) || x.HomeAddress.Contains(phoneNumber)) && x.IsDeleted == false)
                .ToList();
            return result;
        }

        // DucBM
        public Patient GetBy(int accountId, string fullName, DateTime dateOfBirth)
        {
            var result = DbSet.AsQueryable()
                .FirstOrDefault(x => x.AccountId == accountId && x.FullName == fullName && x.DateOfBirth == dateOfBirth);
            return result;
        }

        // DucBM
        public string GetLastPatientCode()
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.PatientCode != null)
                .OrderBy(x => x.PatientCode)
                .ToList()
                .LastOrDefault();
            string s = null;
            if (result != null)
            {
                s = result.PatientCode;
            }
            return s;
        }

        public Patient GetSimpleById(int id)
        {
            var result = DbSet.AsQueryable()
                //.Include(x => x.Account)
                .SingleOrDefault(x => x.PatientId == id);
            return result;
        }
    }
}
