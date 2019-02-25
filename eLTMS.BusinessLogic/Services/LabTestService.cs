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
    public interface ILabTestService
    {
        List<LabTest> GetAllLabTests(string code);
        bool AddLabTest(LabTest labTest);
        bool Update(LabTest labTest);
        LabTest GetLabTestById(int id);
        bool Delete(int id);
    }
    public class  LabTestService : ILabTestService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public LabTestService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }
        public List<LabTest> GetAllLabTests(string code)
        {
            var labTestRepo = this.RepositoryHelper.GetRepository<ILabTestRepository>(UnitOfWork);
            var labTests = labTestRepo.GetAllLabTest(code);
            return labTests;
        }

        public LabTest GetLabTestById(int id)
        {
            var labTestRepo = this.RepositoryHelper.GetRepository<ILabTestRepository>(UnitOfWork);
            var labTests = labTestRepo.GetSimpleById(id);
            return labTests;
        }
        public bool AddLabTest(LabTest labTest)
        {

            var repo = RepositoryHelper.GetRepository<ILabTestRepository>(UnitOfWork);
            try
            {
                repo.Create(labTest);
                UnitOfWork.SaveChanges();
            }
            catch (Exception) { return false; }
            return true;
        }

        //public bool Update(int id, string code, string name, int type, string unit, string note)
        //{
        //    var repo = RepositoryHelper.GetRepository<ISupplyRepository>(UnitOfWork);

        //    try
        //    {
        //        var supply = repo.GetById(id);
        //        supply.SuppliesCode = code;
        //        supply.SuppliesName = name;
        //        supply.SuppliesTypeId = type;
        //        supply.Unit = unit;
        //        supply.Note = note;
        //        repo.Update(supply);
        //        UnitOfWork.SaveChanges();
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        public bool Update(LabTest labTest)
        {
            var repo = RepositoryHelper.GetRepository<ILabTestRepository>(UnitOfWork);

            try
            {
                var labtest = repo.GetSimpleById(labTest.LabTestId);
                labtest.LabTestName = labTest.LabTestName;
                labtest.Description = labTest.Description;
                labtest.Price = labTest.Price;
                labtest.SampleId = labTest.SampleId;
                labtest.LabTestCode = labTest.LabTestCode;
                repo.Update(labtest);
                var result = UnitOfWork.SaveChanges();
                if (result.Any()) return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool Delete(int id)
        {
            var repo = RepositoryHelper.GetRepository<ILabTestRepository>(UnitOfWork);

            try
            {
                var labTest = repo.GetById(id);
                labTest.IsDeleted = true;
                repo.Update(labTest);
                UnitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
