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
    public interface ISampleGroupService
    {
        List<SampleGroup> GetAll();
        //bool Create(LabTest newTesting);
        //bool CreateMany(List<LabTest> newTestings);
        //bool Update(LabTest testing);
        bool Delete(int id);
        bool AddSampleGroup(SampleGroup sample);
        SampleGroup GetSampleGroupById(int id);
    }
    public class SampleGroupService : ISampleGroupService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;

        public SampleGroupService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public List<SampleGroup> GetAll()
        {
            var repo = this.RepositoryHelper.GetRepository<ISampleGroupRepository>(UnitOfWork);
            var sample = repo.GetAllSampleGroup();
            return sample;
        }

        public SampleGroup GetSampleGroupById(int id)
        {
            var repo = this.RepositoryHelper.GetRepository<ISampleGroupRepository>(UnitOfWork);
            var sample = repo.GetSampleById(id);
            return sample;

        }
        public bool Delete(int id)
        {
            var repo = RepositoryHelper.GetRepository<ISampleGroupRepository>(UnitOfWork);

            try
            {
                var sample = repo.GetById(id);
                sample.IsDeleted = true;
                repo.Update(sample);
                UnitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public bool AddSampleGroup(SampleGroup sample)
        {

            var repo = RepositoryHelper.GetRepository<ISampleGroupRepository>(UnitOfWork);
            try
            {
                repo.Create(sample);
                UnitOfWork.SaveChanges();
            }
            catch (Exception) { return false; }
            return true;
        }
    }
}
