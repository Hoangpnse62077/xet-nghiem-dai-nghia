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
    public interface IExportPaperService
    {
        bool AddExportPaper(ExportPaper exportPaper);
        List<ExportPaper> GetAllExportPapers(string createDate);
        List<ExportPaper> GetAllInventorys(string createDate);
        ExportPaper GetExportPaperById(int id);
        ExportPaper GetByexportPaperCode(string code);
        bool Delete(int id);
    }
    public class ExportPaperService : IExportPaperService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public ExportPaperService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public bool AddExportPaper(ExportPaper exportPaper)
        {
            var exportPaperRepo = RepositoryHelper.GetRepository<IExportPaperRepository>(UnitOfWork);
            var exportPaperDetailRepo = RepositoryHelper.GetRepository<IExportPaperDetailRepository>(UnitOfWork);
            var supplyRepo = RepositoryHelper.GetRepository<ISupplyRepository>(UnitOfWork);
            var allSupply = supplyRepo.GetAll();
            using (var transaction = UnitOfWork.BeginTransaction())
            {

                try
                {
                    var exportPerDto = new ExportPaper();
                    exportPerDto.CreateDate = DateTime.Now;
                    exportPerDto.ExportPaperCode = exportPaper.ExportPaperCode;
                    exportPerDto.Note = exportPaper.Note;
                    exportPerDto.IsDeleted = false;
                    exportPerDto.Status = exportPaper.Status;
                    exportPaperRepo.Create(exportPerDto);
                    var dbValresults = UnitOfWork.SaveChanges();
                    if (dbValresults.Any())
                    {
                        transaction.Rollback();
                        return false;
                    }
                    foreach (var item in exportPaper.ExportPaperDetails)
                    {
                        var currentSupply = allSupply.SingleOrDefault(x => x.SuppliesId == item.SuppliesId);
                        currentSupply.Quantity -= item.Quantity;
                        var exportPaperDetail = new ExportPaperDetail()
                        {
                            ExportPaperId = exportPerDto.ExportPaperId,
                            Quantity = item.Quantity,
                            Unit = item.Unit,
                            Note = item.Note,
                            SuppliesId = item.SuppliesId
                        };
                        exportPaperDetailRepo.Create(exportPaperDetail);
                    }

                    dbValresults = UnitOfWork.SaveChanges();
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

        public ExportPaper GetByexportPaperCode(string code)
        {
            var exportPaperRepo = RepositoryHelper.GetRepository<IExportPaperRepository>(UnitOfWork);
            var data = exportPaperRepo.GetSimpleById(code);
            return data;
        }
        public List<ExportPaper> GetAllExportPapers(string createDate)
        {
            var exportPaperRepo = this.RepositoryHelper.GetRepository<IExportPaperRepository>(UnitOfWork);
            var exportPapers = exportPaperRepo.GetAllExportPaper(createDate);
            return exportPapers;
        }
        public List<ExportPaper> GetAllInventorys(string createDate)
        {
            var exportPaperRepo = this.RepositoryHelper.GetRepository<IExportPaperRepository>(UnitOfWork);
            var exportPapers = exportPaperRepo.GetAllInventory(createDate);
            return exportPapers;
        }
        public ExportPaper GetExportPaperById(int id)
        {
            var exportPaperRepo = this.RepositoryHelper.GetRepository<IExportPaperRepository>(UnitOfWork);
            var exportPapers = exportPaperRepo.GetSimpleById(id);
            return exportPapers;
        }
        public bool Delete(int id)
        {
            var repo = RepositoryHelper.GetRepository<IExportPaperRepository>(UnitOfWork);

            try
            {
                var export = repo.GetById(id);
                export.IsDeleted = true;
                repo.Update(export);
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
