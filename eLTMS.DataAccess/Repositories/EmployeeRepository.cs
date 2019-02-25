using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.DataAccess.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        List<Employee> GetAllEmployee(string fullName);
        Employee GetSimpleById(int id);
    }
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public List<Employee> GetAllEmployee(string fullName)
        {
            DateTime result;
            var validateDay = DateTime.TryParseExact(fullName,
                                   "dd-MM-yyyy",
                                   CultureInfo.InvariantCulture,
                                   DateTimeStyles.None,
                                   out result);
            var resultRequrest = DbSet.AsQueryable()
                .Include(x => x.Account)
                .Include(x => x.Account.Role)
                .Where(x => (((validateDay == true && DbFunctions.TruncateTime(result) == DbFunctions.TruncateTime(x.StartDate.Value)) || (validateDay==true&&DbFunctions.TruncateTime(result)==DbFunctions.TruncateTime(x.DateOfBirth.Value))||x.FullName.Contains(fullName)||x.PhoneNumber.Contains(fullName)||x.HomeAddress.Contains(fullName)||x.Account.Role.RoleName.Contains(fullName))||x.Account.Email.Contains(fullName)) && x.IsDeleted == false)
                .ToList();
            return resultRequrest;
        }
        public Employee GetSimpleById(int id)
        {
            var result = DbSet.AsQueryable()
                .Include(x => x.Account)
                .SingleOrDefault(x => x.EmployeeId == id);
            return result;
        }
    }
}
