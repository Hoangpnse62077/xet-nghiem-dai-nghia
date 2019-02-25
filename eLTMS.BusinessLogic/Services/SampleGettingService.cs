using AutoMapper;
using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using eLTMS.DataAccess.Repositories;
using eLTMS.Models.Models.dto;
using eLTMS.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.BusinessLogic.Services
{
    public interface ISampleGettingService
    {
        SampleGetting GetByCodeForNurse(string code);
        bool UpdateStatusNurseDone(int id);
        SampleGetting GetSampleGetting(int id);
    }
    public class SampleGettingService : ISampleGettingService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;

        public SampleGettingService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public SampleGetting GetByCodeForNurse(string code)
        {
            var repo = this.RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var result = repo.GetByCodeForNurse(code);
            return result;
        }
        public SampleGetting GetSampleGetting(int id)
        {
            var repo = this.RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var result = repo.GetById(id);
            return result;
        }
        public bool UpdateStatusNurseDone(int id)
        {
            var sampleGettingRepo = this.RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var labTestingRepo = this.RepositoryHelper.GetRepository<ILabTestingRepository>(UnitOfWork);
            var sg = sampleGettingRepo.GetById(id);
            sg.Status = "NURSEDONE";
            var lts = labTestingRepo.GetAllBySampleGettingId(id);
            foreach(var lt in lts)
            {
                lt.Status = "Waiting";
            }
            UnitOfWork.SaveChanges();
            return true;
        }
    }
}
