using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using eLTMS.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.BusinessLogic.Services
{
    public interface IImportPaperService
    {
        bool AddImportPaper(ImportPaper importPaper);
        List<ImportPaper> GetAllImportPapers(string createDate);
        ImportPaper GetImportPaperById(int id);
        ImportPaper GetByimportPaperCode(string code);
        bool Delete(int id);
    }
    public class ImportPaperService : IImportPaperService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public ImportPaperService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public bool AddImportPaper(ImportPaper importPaper)
        {
            var importPaperRepo = RepositoryHelper.GetRepository<IImportPaperRepository>(UnitOfWork);
            var importPaperDetailRepo = RepositoryHelper.GetRepository<IImportPaperDetailRepository>(UnitOfWork);
            var supplyRepo = RepositoryHelper.GetRepository<ISupplyRepository>(UnitOfWork);
            var allSupply = supplyRepo.GetAll();
            using (var transaction = UnitOfWork.BeginTransaction())
            {

                try
                {
                    var importPerDto = new ImportPaper();
                    importPerDto.CreateDate = DateTime.Now;
                    importPerDto.ImportPaperCode = importPaper.ImportPaperCode;
                    importPerDto.Note = importPaper.Note;
                    importPerDto.IsDeleted =false;
                    importPaperRepo.Create(importPerDto);
                    var dbValresults =  UnitOfWork.SaveChanges();
                    if (dbValresults.Any())
                    {
                        transaction.Rollback();
                        return false;
                    }
                    foreach (var item in importPaper.ImportPaperDetails)
                    {
                        var currentSupply = allSupply.SingleOrDefault(x => x.SuppliesId == item.SuppliesId);
                        currentSupply.Quantity += item.Quantity;
                        var importPaperDetail = new ImportPaperDetail()
                        {
                            ImportPaperId = importPerDto.ImportPaperId,
                            Quantity = item.Quantity,
                            Unit = item.Unit,
                            Note = item.Note,
                            SuppliesId = item.SuppliesId
                        };
                        importPaperDetailRepo.Create(importPaperDetail);
                    }

                    dbValresults =  UnitOfWork.SaveChanges();
                    if (dbValresults.Any())
                    {
                        transaction.Rollback();
                        return false;
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return true;
                }
            }
        }

        public ImportPaper GetByimportPaperCode(string code)
        {
            var importPaperRepo = RepositoryHelper.GetRepository<IImportPaperRepository>(UnitOfWork);
            var data = importPaperRepo.GetSimpleById(code);
            return data;
        }
        public List<ImportPaper> GetAllImportPapers(string createDate)
        {
            var importPaperRepo = this.RepositoryHelper.GetRepository<IImportPaperRepository>(UnitOfWork);
            var importPapers = importPaperRepo.GetAllImportPaper(createDate);
            return importPapers;
        }
        public ImportPaper GetImportPaperById(int id)
        {
            var importPaperRepo = this.RepositoryHelper.GetRepository<IImportPaperRepository>(UnitOfWork);
            var importPapers = importPaperRepo.GetSimpleById(id);
            return importPapers;
        }

        public bool Delete(int id)
        {
            var repo = RepositoryHelper.GetRepository<IImportPaperRepository>(UnitOfWork);

            try
            {
                var import = repo.GetById(id);
                import.IsDeleted = true;
                repo.Update(import);
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
