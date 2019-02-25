using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using eLTMS.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.BusinessLogic.Services
{
    public interface ILabTestingService
    {
        List<LabTesting> GetAll();
        List<LabTesting> GetAllLabTestingDate(string date);
        List<LabTesting> GetAllLabTesting();
        List<LabTesting> GetAllLabTestingFail();
        List<LabTesting> GetAllLabTestingResult();
        List<LabTesting> GetAllResult(string s);
        bool Update(List<LabTesting> labTesting);
        bool UpdateFail(int id);
        bool Delete(int id);
        bool UpdateStatus(List<LabTesting> labTesting);
        List<LabTesting> GetAllLabTestingHaveAppointmentCode(String code);
        LabTesting GetLabTesting(int id);
    }

    public class LabTestingService : ILabTestingService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;

        public LabTestingService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }
        public LabTesting GetLabTesting(int id)
        {
            var labTestingRepo = this.RepositoryHelper.GetRepository<ILabTestingRepository>(UnitOfWork);
            var labTesting = labTestingRepo.GetById(id);
            return labTesting;
        }
            public bool Update(List<LabTesting> labTesting)
        {
            var repo = RepositoryHelper.GetRepository<ILabTestingRepository>(UnitOfWork);

            try
            {
                var ids = labTesting.Select(x => x.LabTestingId).ToList();
                var labtest = repo.GetLabTestingByListId(ids);
                foreach (var item in labTesting)
                {
                    var curentLabTest = labtest.SingleOrDefault(x => x.LabTestingId == item.LabTestingId);
                    curentLabTest.MachineSlot = item.MachineSlot;
                    curentLabTest.Status = item.Status;
                    repo.Update(curentLabTest);
                }
                var result = UnitOfWork.SaveChanges();
                if (result.Any()) return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool UpdateStatus(List<LabTesting> labTesting)
        {
            var repo = RepositoryHelper.GetRepository<ILabTestingRepository>(UnitOfWork);

            try
            {
                var ids = labTesting.Select(x => x.LabTestingId).ToList();
                var labtest = repo.GetLabTestingByListId(ids);
                foreach (var item in labTesting)
                {
                    var curentLabTest = labtest.SingleOrDefault(x => x.LabTestingId == item.LabTestingId);
                    curentLabTest.Status = "DoctorDone";
                    repo.Update(curentLabTest);
                }
                var result = UnitOfWork.SaveChanges();
                if (result.Any()) return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public List<LabTesting> GetAll()
        {
            var repo = this.RepositoryHelper.GetRepository<ILabTestingRepository>(UnitOfWork);
            var labTesting = repo.GetAllLabTesting();
            return labTesting;
        }
        public List<LabTesting> GetAllLabTesting()
        {
            var repo = this.RepositoryHelper.GetRepository<ILabTestingRepository>(UnitOfWork);
            var labTesting = repo.GetAllLabTestings();
            return labTesting;
        }
        public List<LabTesting> GetAllLabTestingFail()
        {
            var repo = this.RepositoryHelper.GetRepository<ILabTestingRepository>(UnitOfWork);
            var labTesting = repo.GetAllLabTestingsFail();
            return labTesting;
        }
        public List<LabTesting> GetAllLabTestingDate(string date)
        {
            var repo = this.RepositoryHelper.GetRepository<ILabTestingRepository>(UnitOfWork);
            var labTesting = repo.GetAllLabTestingDate(date);
            return labTesting;
        }
        public List<LabTesting> GetAllLabTestingHaveAppointmentCode(String code)
        {
            var repo = this.RepositoryHelper.GetRepository<ILabTestingRepository>(UnitOfWork);
            var labTesting = repo.GetAllLabTestingHaveAppointmentCode(code);
            return labTesting;
        }
        public List<LabTesting> GetAllLabTestingResult()
        {
            var repo = this.RepositoryHelper.GetRepository<ILabTestingRepository>(UnitOfWork);
            var labTesting = repo.GetAllLabTestingResult();
            var dupplicatedCode = labTesting
               .GroupBy(x => x.SampleGetting.Appointment.AppointmentCode)
               .Where(x => x.Count() > 1)
               .Select(x => x.Key);

            // list labtesting id  bị xóa
            List<int> removeLabTestingIds = new List<int>();
            foreach (var item in dupplicatedCode)
            {
                var dupplicatedAppointment = labTesting.Where(x => x.SampleGetting.Appointment.AppointmentCode == item).Skip(1).Select(x => x.LabTestingId).ToList();
                removeLabTestingIds.AddRange(dupplicatedAppointment);

            }
            // insert 1 list  bị xóa, xong xóa 1 lần.
            labTesting.RemoveAll(x => removeLabTestingIds.Contains(x.LabTestingId));
            return labTesting;
        }
        public bool Delete(int id)
        {
            var repo = RepositoryHelper.GetRepository<ILabTestingRepository>(UnitOfWork);

            try
            {
                var labtesting = repo.GetLabTestingById(id);
                labtesting.Status="RETEST";
                repo.Update(labtesting);
                var result = UnitOfWork.SaveChanges();
                if (result.Any()) return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool UpdateFail(int id)
        {
            var repo = RepositoryHelper.GetRepository<ILabTestingRepository>(UnitOfWork);

            try
            {
                var labtesting = repo.GetLabTestingById(id);
                labtesting.Status = "FAIL";
                repo.Update(labtesting);
                var result = UnitOfWork.SaveChanges();
                if (result.Any()) return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public List<LabTesting> GetAllResult(string s)
        {
            var repo = this.RepositoryHelper.GetRepository<ILabTestingRepository>(UnitOfWork);
            var labTestingResult = repo.GetAllResult(s);
            var dupplicatedCode = labTestingResult
                .GroupBy(x => x.SampleGetting.Appointment.AppointmentCode)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key);

            // list labtesting id  bị xóa
            List<int> removeLabTestingIds = new List<int>();
            foreach (var item in dupplicatedCode)
            {
                var dupplicatedAppointment = labTestingResult.Where(x => x.SampleGetting.Appointment.AppointmentCode == item).Skip(1).Select(x => x.LabTestingId).ToList();
                removeLabTestingIds.AddRange(dupplicatedAppointment);
                
            }
            // insert 1 list  bị xóa, xong xóa 1 lần.
            labTestingResult.RemoveAll(x => removeLabTestingIds.Contains(x.LabTestingId));
            return labTestingResult;
        }
    }
}