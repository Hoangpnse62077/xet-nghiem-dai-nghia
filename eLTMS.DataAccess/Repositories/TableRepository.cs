using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.DataAccess.Repositories
{
    public interface ITableRepository : IRepository<Table>
    {
        List<Table> GetAllTable();
        int GetTableCountBySampleGroupId(int sampleGroupId);
        Table GetFirstAvailableTable(int slotId, DateTime gettingDate);
    }
    public class TableRepository : RepositoryBase<Table>, ITableRepository
    {
        public List<Table> GetAllTable()
        {
            var result = DbSet.AsQueryable()
                .ToList();
            return result;
        }

        public int GetTableCountBySampleGroupId(int sampleGroupId)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.SampleGroupId == sampleGroupId)
                .Count();
            return result;
        }

        public Table GetFirstAvailableTable(int slotId, DateTime gettingDate)
        {
            var result = UnitOfWork.Context.Database.SqlQuery<Table>
                ("EXEC [dbo].[GetFistAvailableTable] @SlotId, @GettingDate",
                new SqlParameter("@SlotId", slotId),
                new SqlParameter("@GettingDate", gettingDate))
                .FirstOrDefault();
            return result;
        }
    }
}
