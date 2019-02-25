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

    public interface IHospitalSuggestionService
    {
        List<HospitalSuggestion> GetAllHospitalSuggestions(string con);
        //bool AddPatient(Patient patient);
        ////bool Update(int id, string code, string name, string gender, string phone, string address, string companyAddress);
        //bool Update(Patient patientdto);
        //Patient GetPatientById(int id);
        //bool Delete(int id);
        //// bool UpdatePatient(Patient dto);
    }
    public class HospitalSuggestionService : IHospitalSuggestionService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public HospitalSuggestionService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }
        public List<HospitalSuggestion> GetAllHospitalSuggestions(string con)
        {
            var hospitalSuggestionRepo = this.RepositoryHelper.GetRepository<IHospitalSuggestionRepository>(UnitOfWork);
            var hospitalSuggestions = hospitalSuggestionRepo.GetAllHospitalSuggestion(con);
            return hospitalSuggestions;
        }
        //public bool Update(Patient patientdto)
        //{
        //    var repo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);

        //    try
        //    {
        //        var patient = repo.GetSimpleById(patientdto.PatientId);
        //        //var account = patient.Account;
        //        patient.PatientCode = patientdto.PatientCode;
        //        patient.FullName = patientdto.FullName;
        //        patient.Gender = patientdto.Gender;
        //        patient.PhoneNumber = patientdto.PhoneNumber;
        //        patient.HomeAddress = patientdto.HomeAddress;
        //        patient.CompanyAddress = patientdto.CompanyAddress;
        //        patient.AccountId = patientdto.AccountId;
        //        patient.AvatarUrl = patientdto.AvatarUrl;
        //        repo.Update(patient);
        //        var result = UnitOfWork.SaveChanges();
        //        if (result.Any())
        //            return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //    return true;
        //}
        //public Patient GetPatientById(int id)
        //{
        //    var patientRepo = this.RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
        //    var patient = patientRepo.GetSimpleById(id);
        //    return patient;
        //}
        //public bool AddPatient(Patient patient)
        //{
        //    var repo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
        //    try
        //    {
        //        repo.Create(patient);
        //        UnitOfWork.SaveChanges();
        //    }
        //    catch (Exception ex) { return false; }
        //    return true;
        //}


    
        ////public bool Update(int id, string code, string name, string gender, string phone, string address, string companyAddress)
        ////{
        ////    var repo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);

        ////    try
        ////    {
        ////        var patient = repo.GetById(id);
        ////        patient.PatientCode = code;
        ////        patient.FullName = name;
        ////        patient.Gender = gender;
        ////        patient.PhoneNumber = phone;
        ////        patient.HomeAddress = address;
        ////        patient.CompanyAddress = companyAddress;
        ////        repo.Update(patient);
        ////        var result = UnitOfWork.SaveChanges();
        ////        if (result.Any())
        ////            return false;
        ////    }
        ////    catch (Exception)
        ////    {
        ////        return false;
        ////    }

        ////    return true;
        ////}
      
        //// public bool UpdatePatient(Patient dto)
        ////{
        ////    var patientRepo = this.RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
        ////    var patient = patientRepo.GetById(dto.PatientId);
        ////    patient.PhoneNumber = dto.PhoneNumber;
        ////    [
        ////    if (dto.AccountId == 0 || dto.AccountId is null)
        ////    {
        ////        patient.AccountId = null;
        ////    }
        //// }
        //public bool Delete(int id)
        //{
        //    var repo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);

        //    try
        //    {
        //        var patient = repo.GetById(id);
        //        patient.IsDeleted = true;
        //        repo.Update(patient);
        //        UnitOfWork.SaveChanges();
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }

        //    return true;
        //}
    }
}
