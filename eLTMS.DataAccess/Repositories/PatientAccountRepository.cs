using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.DataAccess.Repositories
{
    public interface IPatientAccountRepository : IRepository<PatientAccount>
    {
    }
    public class PatientAccountRepository : RepositoryBase<PatientAccount>, IPatientAccountRepository
    {
    }
}
