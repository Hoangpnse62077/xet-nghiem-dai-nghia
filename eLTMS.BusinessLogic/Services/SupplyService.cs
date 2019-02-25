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
    public interface ISupplyService
    {
        List<Supply> GetAllSupplies(string suppliesCode);
        bool AddSupply(Supply supply);
        bool Update(int id, string code, string name, int type, string unit, string note);
        Supply GetSupplyById(int id);
        bool Delete(int id);
    }

    public class SupplyService : ISupplyService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public SupplyService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public List<Supply> GetAllSupplies(string suppliesCode)
        {
            var supplyRepo = this.RepositoryHelper.GetRepository<ISupplyRepository>(UnitOfWork);
            var supplies =  supplyRepo.GetAllSupply(suppliesCode);
            return supplies;
        }
      
        public Supply GetSupplyById(int id)
        {
            var supplyRepo = this.RepositoryHelper.GetRepository<ISupplyRepository>(UnitOfWork);
            var supplies = supplyRepo.GetSimpleById(id);
            return supplies;
        }
        public bool AddSupply(Supply supply)
        {

            var repo = RepositoryHelper.GetRepository<ISupplyRepository>(UnitOfWork);
            //repo.Create(supply);
            //var saveResult =  UnitOfWork.SaveChanges();
            try
            {
                repo.Create(supply);
                UnitOfWork.SaveChanges();
            }
            catch (Exception) { return false; }
            return true;
        }

        public bool Update(int id, string code, string name,int type, string unit, string note)
        {
            var repo = RepositoryHelper.GetRepository<ISupplyRepository>(UnitOfWork);

            try
            {
                var supply = repo.GetById(id);
                supply.SuppliesCode = code;
                supply.SuppliesName = name;
                supply.SuppliesTypeId = type;
                supply.Unit = unit;
                supply.Note =note;
                repo.Update(supply);
                UnitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public bool Delete(int id)
        {
            var repo = RepositoryHelper.GetRepository<ISupplyRepository>(UnitOfWork);

            try
            {
                var supply = repo.GetById(id);
                supply.IsDeleted = true;
                repo.Update(supply);
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
