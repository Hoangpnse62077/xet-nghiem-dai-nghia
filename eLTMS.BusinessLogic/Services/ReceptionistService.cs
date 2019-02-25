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
    public interface IReceptionistService
    {
        List<PatientGetByDateTestingDto> GetAllPatientByDateTesting(string search, DateTime date);
        List<SampleGettingForReceptionistDto> GetAppByPatient(int patientId, DateTime date);
        List<Token> GetAllTokens();
        bool ChangeIsPaid(int patientId, DateTime date);
        PriceListDto GetPrice(int patientId, DateTime date);
        bool DeleteSG(int sgId);
    }
    class ReceptionistService : IReceptionistService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public ReceptionistService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        //get patient
        public List<PatientGetByDateTestingDto> GetAllPatientByDateTesting(string search, DateTime date)
        {
            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var paRepo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var spRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);

            var apps = appRepo.GetAll().Where(p => p.IsDeleted != true && p.IsOnline == true);
            var pas = paRepo.GetAll().Where(p => p.IsDeleted != true );
            var sgs = sgRepo.GetAll().Where(p => p.GettingDate == date && p.IsDeleted !=true 
            && p.Status != "NURSEDONE" && p.Status != "DOCTORDONE");
            
            var sps = spRepo.GetAll().Where(p => p.IsDeleted != true);
            
            //app + patient (1)
            var appPas = pas.Join(apps, p => p.PatientId, c => c.PatientId, (p, c) => new
            {
                pa = p,
                app = c
            });

            //sample + sampleGetting (2)
            var spSgs = sgs.Join(sps, p => p.SampleId, c => c.SampleId, (p, c) => new
            {
                sg = p,
                sp = c
            });
            var count = 1;
            var result = appPas.Join(spSgs, p => p.app.AppointmentId,
                c => c.sg.AppointmentId, (p, c) => new PatientGetByDateTestingDto
                {
                    OrderNumber = count++,
                    PatientName = p.pa.FullName,
                    PatientID = p.pa.PatientId,
                    Phone = p.pa.PhoneNumber,
                    DateOfBirth = p.pa.DateOfBirth != null ? p.pa.DateOfBirth.Value.ToShortDateString() : "",
                    Address = p.pa.HomeAddress,
                    //IdentityCardNumber = p.pa.IdentityCardNumber
                    IsPaid = c.sg.IsPaid

                }).GroupBy(a => a.PatientID).Select(g => g.First()).ToList();
            result = result.Where(p => p.PatientName.ToString().ToLower().Contains(search.ToLower())
            || p.Phone.ToString().Contains(search)).ToList();               
            return result;
        }


        //get app
        public List<SampleGettingForReceptionistDto> GetAppByPatient(int patientId, DateTime date)
        {
            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var paRepo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var slotRepo = RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var spRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);
            var tableRepo = this.RepositoryHelper.GetRepository<ITableRepository>(this.UnitOfWork);

            var apps = appRepo.GetAll().Where(p => p.IsDeleted != true);
            var pas = paRepo.GetAll().Where(p => p.IsDeleted != true && p.PatientId == patientId);
            var sgs = sgRepo.GetAll().Where(p => p.IsDeleted != true && p.GettingDate == date);
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
            var count = 1;
            var result = spSgSlots.Join(appPas, p => p.spSg.sg.AppointmentId,
                c => c.app.AppointmentId, (p, c) => new SampleGettingForReceptionistDto
                {
                    OrderNumber = count++,
                    SampleGettingId = p.spSg.sg.SampleGettingId,
                    SampleName = p.spSg.sp.SampleName,
                    Status = p.spSg.sg.Status,
                    //LabTesting = p.spSg.sp.LabTests,
                    StartTime = TimeSpan.FromSeconds(p.slot.StartTime.Value).ToString(@"hh\:mm"),
                    EnterDate = c.app.EnterTime.Value.ToString("dd/MM/yyyy"),
                    EnterTime = c.app.EnterTime.Value.ToString("HH:mm:ss"),
                }).ToList();
            
            return result;
        }

        public List<Token> GetAllTokens()
        {
            var repo = this.RepositoryHelper.GetRepository<ITokenRepository>(UnitOfWork);
            var tokens = repo.GetAll();
            return tokens;
        }

        public bool ChangeIsPaid(int patientId,DateTime date)
        {
            try
            {
                var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
                var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
                var paRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);

                var apps = appRepo.GetAll();
                var pas = paRepo.GetAll().Where(p => p.IsDeleted != true && p.PatientId == patientId);
                var sgs = sgRepo.GetAll().Where(p => p.IsDeleted != true && p.GettingDate == date);


                //app + patient (1)
                var appPas = apps.Join(pas, p => p.PatientId, c => c.PatientId, (p, c) => new
                {
                    app = p,
                    pa = c
                });

                //(1) + sampleGetting 
                var appPasSg = appPas.Join(sgs, p => p.app.AppointmentId, c => c.AppointmentId, (p, c) => new
                {
                    appPa = p,
                    sg = c
                });
                var listSg = new List<SampleGetting>();
                foreach(var item in appPasSg)
                {
                    listSg.Add(sgRepo.GetById(item.sg.SampleGettingId));
                }
                foreach (var sg in listSg) {
                    sg.IsPaid = true;
                    sg.Status = "WAITING";
                    sgRepo.Update(sg);

                    var app = appRepo.GetById(sg.AppointmentId);
                    app.Status = "RECEPTDONE";
                    appRepo.Update(app);
                    UnitOfWork.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public PriceListDto GetPrice(int patientId,DateTime date)
        {
            var paRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var labTestRepo = RepositoryHelper.GetRepository<ILabTestRepository>(UnitOfWork);
            var labTestingRepo = this.RepositoryHelper.GetRepository<ILabTestingRepository>(this.UnitOfWork);
            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);

            var apps = appRepo.GetAll().Where(p => p.IsDeleted != true);
            var pas = paRepo.GetAll().Where(p => p.IsDeleted != true && p.PatientId == patientId);
            var sgs = sgRepo.GetAll().Where(p => p.IsDeleted != true && p.GettingDate == date);
            var lts = labTestingRepo.GetAll().Where(p => p.IsDeleted != true);
            var labs = labTestRepo.GetAll().Where(p => p.IsDeleted != true);

            var appPas = apps.Join(pas, p => p.PatientId, c => c.PatientId, (p, c) => new
            {
                app = p,
                pa = c
            });
            var appPasSgs = appPas.Join(sgs, p => p.app.AppointmentId, c => c.AppointmentId, (p, c) => new
            {
                appPa = p,
                sg = c
            });
            var appPasSgsLts = appPasSgs.Join(lts, p => p.sg.SampleGettingId, c => c.SampleGettingId, (p, c) => new
            {
                appPaSg = p,
                lt = c
            });
            var count = 1;
            var result = appPasSgsLts.Join(labs, p => p.lt.LabTestId,
                c => c.LabTestId, (p, c) => new PriceListItemDto
                {
                    OrderNumber = count++,
                    LabtestName = c.LabTestName,
                    Price = c.Price,
                }).ToList();
            int? total = 0;
            foreach (var i in result)
            {
                total += i.Price;
            }
            var rs = new PriceListDto
            {
                PriceListItemDto = result,
                TotalPrice = total,
            };
            return rs;
        }

        public bool DeleteSG(int sgId)
        {
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);

            try
            {
                var sg = sgRepo.GetById(sgId);
                sg.IsDeleted = true;
                sgRepo.Update(sg);
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
