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
    public interface IExportPaperRepository : IRepository<ExportPaper>
    {
        ExportPaper GetSimpleById(string code);
        List<ExportPaper> GetAllExportPaper(string createDate);
        List<ExportPaper> GetAllInventory(string createDate);
        ExportPaper GetSimpleById(int id);
    }
    public class ExportPaperRepository : RepositoryBase<ExportPaper>, IExportPaperRepository
    {
        public ExportPaper GetSimpleById(string code)
        {
            return DbSet
                .Include( x => x.ExportPaperDetails)
                .SingleOrDefault(x => x.ExportPaperCode == code);
        }
        public List<ExportPaper> GetAllExportPaper(string createDate)
        {
            DateTime createDateTime;
            if (String.IsNullOrEmpty(createDate) == false)
            {
                var parseDate = DateTime.TryParseExact(createDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out createDateTime);
                if (parseDate == false)
                {
                    return new List<ExportPaper>();
                }
                return DbSet.AsQueryable()
                .Where(x => DbFunctions.TruncateTime(x.CreateDate.Value) ==
                DbFunctions.TruncateTime(createDateTime) && x.IsDeleted == false && x.Status == false)
                .ToList();
            }
            else
            {
                return DbSet.AsQueryable()
                .Where(x => x.IsDeleted == false && x.Status == false)
                .ToList();
            }
        }
        public List<ExportPaper> GetAllInventory(string createDate)
        {
            DateTime createDateTime;
            if (String.IsNullOrEmpty(createDate) == false)
            {
                var parseDate = DateTime.TryParseExact(createDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out createDateTime);
                if (parseDate == false)
                {
                    return new List<ExportPaper>();
                }
                return DbSet.AsQueryable()
                .Where(x => DbFunctions.TruncateTime(x.CreateDate.Value) ==
                DbFunctions.TruncateTime(createDateTime) && x.IsDeleted == false && x.Status == true)
                .ToList();
            }
            else
            {
                return DbSet.AsQueryable()
                .Where(x => x.IsDeleted == false && x.Status == true)
                .ToList();
            }
        }
        public ExportPaper GetSimpleById(int id)
        {
            var result = DbSet.AsQueryable()
                .Include(x=>x.ExportPaperDetails)
                .SingleOrDefault(x => x.ExportPaperId == id);
            return result;
        }
    }
}
