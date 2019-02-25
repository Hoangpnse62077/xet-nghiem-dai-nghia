
using eLTMS.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/********************************************************************/
/*  Reference: https://github.com/Hoangpnse62077/CasptoneProject    */
/*  Gmail: hoangpnse62077@fpt.edu.vn                                */
/********************************************************************/

namespace eLTMS.DataAccess.Infrastructure
{
    public partial interface IRepositoryHelper
    {

        IUnitOfWork GetUnitOfWork();
        TRepository GetRepository<TRepository>(IUnitOfWork unitOfWork)
            where TRepository : class;
    }
    public partial class RepositoryHelper : IRepositoryHelper
    {


        public IUnitOfWork GetUnitOfWork()
        {
            var unitOfWork = new UnitOfWork();
            return unitOfWork;
        }

        public TRepository GetRepository<TRepository>(IUnitOfWork unitOfWork)
            where TRepository : class

        {
            //if (typeof(TRepository) == typeof(IResultPaperRepository))
            //{
            //    dynamic repo = new ResultPaperRepository();
            //    repo.UnitOfWork = unitOfWork;
            //    return (TRepository)repo;
            //}

            if (typeof(TRepository) == typeof(IHospitalSuggestionRepository))
            {
                dynamic repo = new HospitalSuggestionRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }
            if (typeof(TRepository) == typeof(ISupplyRepository))
            {
                dynamic repo = new SupplyRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }
            if (typeof(TRepository) == typeof(IAccountRepository))
            {
                dynamic repo = new AccountRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }
            if (typeof(TRepository) == typeof(ILabTestRepository))
            {
                dynamic repo = new LabTestRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }
            if (typeof(TRepository) == typeof(ISampleRepository))
            {
                dynamic repo = new SampleRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }
            if (typeof(TRepository) == typeof(ISampleGroupRepository))
            {
                dynamic repo = new SampleGroupRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }
            if (typeof(TRepository) == typeof(IAppointmentRepository))
            {
                dynamic repo = new AppointmentRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }

            if (typeof(TRepository) == typeof(ISampleGettingRepository))
            {
                dynamic repo = new SampleGettingRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }
            if (typeof(TRepository) == typeof(IEmployeeRepository))
            {
                dynamic repo = new EmployeeRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }

            if (typeof(TRepository) == typeof(ILabTestingIndexRepository))
            {
                dynamic repo = new LabTestingIndexRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }

            if (typeof(TRepository) == typeof(IImportPaperDetailRepository))
            {
                dynamic repo = new ImportPaperDetailRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }
            if (typeof(TRepository) == typeof(IImportPaperRepository))
            {
                dynamic repo = new ImportPaperRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }
            if (typeof(TRepository) == typeof(IExportPaperDetailRepository))
            {
                dynamic repo = new ExportPaperDetailRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }
            if (typeof(TRepository) == typeof(IExportPaperRepository))
            {
                dynamic repo = new ExportPaperRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }
            if (typeof(TRepository) == typeof(IPatientRepository))
            {
                dynamic repo = new PatientRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }
            if (typeof(TRepository) == typeof(ISlotRepository))
            {
                dynamic repo = new SlotRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }
            if (typeof(TRepository) == typeof(ITableRepository))
            {
                dynamic repo = new TableRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }

            if (typeof(TRepository) == typeof(IFeedbackRepository))
            {
                dynamic repo = new FeedbackRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }

            if (typeof(TRepository) == typeof(ILabTestingRepository))
            {
                dynamic repo = new LabTestingRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }

            if (typeof(TRepository) == typeof(ITokenRepository))
            {
                dynamic repo = new TokenRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }
            if (typeof(TRepository) == typeof(ITableRepository))
            {
                dynamic repo = new TableRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }
            if (typeof(TRepository) == typeof(IPatientAccountRepository))
            {
                dynamic repo = new PatientAccountRepository();
                repo.UnitOfWork = unitOfWork;
                return (TRepository)repo;
            }

            TRepository repository = null;
            TryGetRepositoryPartial<TRepository>(unitOfWork, ref repository);
            return repository;
        }

        partial void TryGetRepositoryPartial<TRepository>(IUnitOfWork unitOfWork, ref TRepository repository)
            where TRepository : class;
    }
}
