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
   public interface IFeedbackRepository: IRepository<Feedback>
    {
        List<Feedback> GetAllFeed(string dateTime);
        Feedback GetSimpleById(int id);
    }
    public class FeedbackRepository: RepositoryBase<Feedback>, IFeedbackRepository
    {
        public List<Feedback> GetAllFeed(string dateTime)
        {
            DateTime result;
            var validateDay= DateTime.TryParseExact(dateTime,
                                   "dd-MM-yyyy",
                                   CultureInfo.InvariantCulture,
                                   DateTimeStyles.None,
                                   out result);
            var resultRequrest = DbSet.AsQueryable()
                .Include(x=>x.Employee)
                .Include(x=>x.Patient)
                    .Where(x => ((x.Content).ToString().Contains(dateTime) || (x.FeedbackId).ToString().Contains(dateTime) || (x.Patient.FullName).ToString().Contains(dateTime) || (x.Employee.FullName).ToString().Contains(dateTime) || (x.Status).ToString().Contains(dateTime) || (validateDay == true&&DbFunctions.TruncateTime(result) == DbFunctions.TruncateTime(x.ReceivedDateTime.Value))) && x.IsDeleted == false)
                    .ToList();
            return resultRequrest;
        }
        public Feedback GetSimpleById(int id)
        {
            var result = DbSet.AsQueryable()
                .Include(x => x.Employee)
                .Include(x => x.Patient)
                .SingleOrDefault(x => x.FeedbackId == id);
            return result;
        }
    }
}
