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
    public interface ISampleService
    {
        List<Sample> GetAll();
        List<SampleDto> GetAllSampleDtos();
        bool Delete(int id);
        bool AddSample(Sample sample);
        Sample GetSampleById(int id);
    }
    public class SampleService: ISampleService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;

        public SampleService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }
       
        public List<Sample> GetAll()
        {
            var sampleRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(UnitOfWork);
            var samples = sampleRepo.GetAllSamples();
            return samples;
        }

        //DucBM
        public List<SampleDto> GetAllSampleDtos()
        {
            var sampleRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(UnitOfWork);
            var slotRepo = this.RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var samples = sampleRepo.GetAllSamples();
            foreach (var sample in samples)
            {
                var slots = slotRepo.GetBySampleGroupId((int)sample.SampleGroupId);
                sample.SampleGroup.Slots = slots;
            }
            var sampleDtos = Mapper.Map<IEnumerable<Sample>, IEnumerable<SampleDto>>(samples).ToList();
            foreach (var sampleDto in sampleDtos)
            {
                foreach (var slot in sampleDto.SlotDtos)
                {
                    slot.FmStartTime = DateTimeUtils.ConvertSecondToShortHour(slot.StartTime);
                    slot.FmFinishTime = DateTimeUtils.ConvertSecondToShortHour(slot.FinishTime);
                }
            }
            return sampleDtos;
        }

        public Sample GetSampleById(int id)
        {
            var repo = this.RepositoryHelper.GetRepository<ISampleRepository>(UnitOfWork);
            var sample = repo.GetSampleById(id);
            return sample;

        }
        public bool Delete(int id)
        {
            var repo = RepositoryHelper.GetRepository<ISampleRepository>(UnitOfWork);

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
        public bool AddSample(Sample sample)
        {

            var repo = RepositoryHelper.GetRepository<ISampleRepository>(UnitOfWork);
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
