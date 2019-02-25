using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using eLTMS.DataAccess.Repositories;
using eLTMS.Models.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.BusinessLogic.Services
{
    public interface INurseService
    {
        List<Token> GetAllTokens();
        bool ChangeIsGot(int sampleGettingId);
        List<SampleGettingForNurseBySampleDto> GetAllBySample(string search, DateTime date, int sampleId);
    }
    class NurseService : INurseService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public NurseService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public bool ChangeIsGot(int sampleGettingId)
        {
            try
            {
                var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
                var labRepo = RepositoryHelper.GetRepository<ILabTestingRepository>(UnitOfWork);
                var sampleGetting = sgRepo.GetFirst(p => p.SampleGettingId == sampleGettingId);

                sampleGetting.IsGot = true;
                sampleGetting.Status = "NURSEDONE";
                sgRepo.Update(sampleGetting);

                var labs = labRepo.GetAll().Where(x => x.SampleGettingId == sampleGettingId);
                foreach(var lab in labs)
                {
                    lab.Status = "WAITING";
                    lab.IsDeleted = false;
                    labRepo.Update(lab);
                }

                UnitOfWork.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public List<SampleGettingForNurseBySampleDto> GetAllBySample(string search, DateTime date, int sampleId)
        {
            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var paRepo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var slotRepo = RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var spRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);
            var tableRepo = this.RepositoryHelper.GetRepository<ITableRepository>(this.UnitOfWork);

            var apps = appRepo.GetAll().Where(p => p.IsDeleted != true);
            var pas = paRepo.GetAll().Where(p => p.IsDeleted != true);
            var sgs = sgRepo.GetAll().Where(p => p.SampleId == sampleId && p.IsDeleted != true && p.GettingDate == date
            && p.IsPaid == true/* && p.Status == "WAITING" */);
            if (sampleId == 1)
            {
                sgs = sgRepo.GetAll().Where(p => (p.SampleId == 1 || p.SampleId==2) 
                && p.IsDeleted != true && p.GettingDate == date && p.IsPaid == true);
            }
            var sps = spRepo.GetAll().Where(p => p.IsDeleted != true);
            var slots = slotRepo.GetAll();

            //app + patient (1)
            var appPas = apps.Join(pas, p => p.PatientId, c => c.PatientId, (p, c) => new
            {
                app = p,
                pa = c
            });

            //sample + sampleGetting (2)
            var spSgs = sgs.Join(sps, p => p.SampleId, c => c.SampleId, (p, c) => new
            {
                sg = p,
                sp = c
            });

            //(2) + slot
            var spSgSlots = spSgs.Join(slots, p => p.sg.SlotId, c => c.SlotId, (p, c) => new
            {
                spSg = p,
                slot = c
            });
            int count = 1;
            var result = spSgSlots.Join(appPas, p => p.spSg.sg.AppointmentId,
                c => c.app.AppointmentId, (p, c) => new SampleGettingForNurseBySampleDto
                {
                    OrderNumber = count++,
                    StartTime = TimeSpan.FromSeconds(p.slot.StartTime.Value).ToString(@"hh\:mm"),
                    SampleGettingId = p.spSg.sg.SampleGettingId,
                    DateOfBirth = c.pa.DateOfBirth != null ? c.pa.DateOfBirth.Value.ToShortDateString() : "",
                    LabTestingIds = GetIdList(p.spSg.sg.LabTestings),

                    SampleName = p.spSg.sp.SampleName,
                    PatientName = c.pa.FullName,
                    Date = p.spSg.sg.GettingDate.Value.ToShortDateString(),
                    IsGot = p.spSg.sg.IsGot
                }).ToList();
                result = result.Where(p => p.PatientName.ToString().ToLower().Contains(search.ToLower())).ToList()
                .OrderBy(a => a.StartTime).ToList();
            result = result.Where(p => p.StartTime.ToString().Contains(search)
            || p.SampleGettingId.ToString().Contains(search)
            || p.Date.ToString().Contains(search)
            || p.PatientName.ToString().Contains(search)
            || p.SampleGettingId.ToString().Contains(search)
            ).ToList()
            .OrderBy(a => a.StartTime).ToList();            

            return result;
        }

        private List<int> GetIdList(ICollection<LabTesting> labTestings)
        {
            var result = new List<int>();
            foreach(var lt in labTestings)
            {
                result.Add(lt.LabTestingId);
            }
            return result;
        }

        public List<Token> GetAllTokens()
        {
            var repo = this.RepositoryHelper.GetRepository<ITokenRepository>(UnitOfWork);
            var tokens = repo.GetAll();
            return tokens;
        }
    }
}
