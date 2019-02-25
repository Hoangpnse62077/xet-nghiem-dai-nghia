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
    public interface IHospitalSuggestionRepository : IRepository<HospitalSuggestion>
    {
        List<HospitalSuggestion> GetAllHospitalSuggestion(string con);
        //Patient GetSimpleById(int id);
    }


    public class HospitalSuggestionRepository : RepositoryBase<HospitalSuggestion>, IHospitalSuggestionRepository
    {
        

        public List<HospitalSuggestion> GetAllHospitalSuggestion(string con)
        {
            string[] arrListStr = con.Split(new char[] { ',' });
            List<HospitalSuggestion> add = new List< HospitalSuggestion > ();
            foreach (var item in arrListStr)
            {
                var result = DbSet.AsQueryable()

                    //.Include(x=>x.Account)
                    .Where(x => x.DiseaseName.Contains(item) && x.IsDeleted == false)

                    .ToList();
                add.AddRange(result);
            }
            return add;
        }
    }
}
