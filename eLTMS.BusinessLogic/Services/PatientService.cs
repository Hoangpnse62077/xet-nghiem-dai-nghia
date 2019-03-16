using eLTMS.DataAccess.Models;
using System;
using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.BusinessLogic.Services
{

    public interface IPatientService
    {
        List<Patient> GetAllPatients(string phoneNumber);
        bool AddPatient(Patient patient);
        //bool Update(int id, string code, string name, string gender, string phone, string address, string companyAddress);
        bool Update(Patient patientdto);
        Patient GetPatientById(int id);
        bool Delete(int id);
        // bool UpdatePatient(Patient dto);
        List<Patient> GetAllPatientByName(string phoneNumber);
    }
    public class PatientService : IPatientService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public PatientService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }
        public List<Patient> GetAllPatients(string phoneNumber)
        {
            var patientRepo = this.RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var patients = patientRepo.GetAllPatient(phoneNumber);
            return patients;
        }
        public bool Update(Patient patientdto)
        {
            var repo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);

            try
            {
                var patient = repo.GetSimpleById(patientdto.PatientId);
                //var account = patient.Account;
                patient.PatientCode = patientdto.PatientCode;
                patient.FullName = patientdto.FullName;
                patient.Gender = patientdto.Gender;
                patient.PhoneNumber = patientdto.PhoneNumber;
                patient.HomeAddress = patientdto.HomeAddress;
                patient.CompanyAddress = patientdto.CompanyAddress;
                patient.AccountId = patientdto.AccountId;
                patient.Age = patientdto.Age;
                repo.Update(patient);
                var result = UnitOfWork.SaveChanges();
                if (result.Any())
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        public Patient GetPatientById(int id)
        {
            var patientRepo = this.RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var patient = patientRepo.GetSimpleById(id);
            return patient;
        }
        public bool AddPatient(Patient patient)
        {
            var repo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            try
            {
                // create Patient Code in format BN-00000001
                /*int newCode = 0;
                var lastCode = repo.GetLastPatientCode();
                if (lastCode != null)
                {
                    newCode = int.Parse(lastCode.Substring(2)) + 1;
                }
                patient.PatientCode = newCode.ToString("D8"); // format as 8 digits/**/

                repo.Create(patient);
                UnitOfWork.SaveChanges();
            }
            catch (Exception ex) { return false; }
            return true;
        }


    
        //public bool Update(int id, string code, string name, string gender, string phone, string address, string companyAddress)
        //{
        //    var repo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);

        //    try
        //    {
        //        var patient = repo.GetById(id);
        //        patient.PatientCode = code;
        //        patient.FullName = name;
        //        patient.Gender = gender;
        //        patient.PhoneNumber = phone;
        //        patient.HomeAddress = address;
        //        patient.CompanyAddress = companyAddress;
        //        repo.Update(patient);
        //        var result = UnitOfWork.SaveChanges();
        //        if (result.Any())
        //            return false;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }

        //    return true;
        //}
      
        // public bool UpdatePatient(Patient dto)
        //{
        //    var patientRepo = this.RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
        //    var patient = patientRepo.GetById(dto.PatientId);
        //    patient.PhoneNumber = dto.PhoneNumber;
        //    [
        //    if (dto.AccountId == 0 || dto.AccountId is null)
        //    {
        //        patient.AccountId = null;
        //    }
        // }
        public bool Delete(int id)
        {
            var repo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);

            try
            {
                var patient = repo.GetById(id);
                patient.IsDeleted = true;
                repo.Update(patient);
                UnitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public List<Patient> GetAllPatientByName(string phoneNumber)
        {
            var allPatients = UnitOfWork.Context.Set<Patient>().AsQueryable();
            var data = allPatients.Where(x => x.FullName.Contains(phoneNumber) && x.IsDeleted == false)
                .OrderByDescending(x => x.PatientId)
                .ToList();
            return data;
        }
    }
}
