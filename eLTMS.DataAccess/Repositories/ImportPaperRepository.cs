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
    public interface IImportPaperRepository : IRepository<ImportPaper>
    {
        ImportPaper GetSimpleById(string code);
        List<ImportPaper> GetAllImportPaper(string createDate);
        ImportPaper GetSimpleById(int id);
    }
    public class ImportPaperRepository : RepositoryBase<ImportPaper>, IImportPaperRepository
    {
        public ImportPaper GetSimpleById(string code)
        {
            return DbSet
                .Include( x => x.ImportPaperDetails)
                .SingleOrDefault(x => x.ImportPaperCode == code);
        }
        public List<ImportPaper> GetAllImportPaper(string createDate)
        {
            DateTime createDateTime;
            if (String.IsNullOrEmpty(createDate) == false)
            {
            var parseDate = DateTime.TryParseExact(createDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out createDateTime);
                if (parseDate == false)
                {
                    return new List<ImportPaper>();
                }
                return DbSet.AsQueryable()
                .Where(x => DbFunctions.TruncateTime(x.CreateDate.Value) ==
                DbFunctions.TruncateTime(createDateTime) && x.IsDeleted == false)
                .ToList();
            }
            else
            {
                return DbSet.AsQueryable()
                .Where(x => x.IsDeleted == false)
                .ToList();
            }
            
            
        }
        public ImportPaper GetSimpleById(int id)
        {
            var result = DbSet.AsQueryable()
                .Include(x=>x.ImportPaperDetails)
                .SingleOrDefault(x => x.ImportPaperId == id);
            return result;
        }
    }
}
