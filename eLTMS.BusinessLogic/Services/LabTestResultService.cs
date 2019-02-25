using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using eLTMS.Models.Models.dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.BusinessLogic.Services
{
    public interface ILabTestResultService
    {
        List<LabTestResult> GetAllLabTestResult(LabTestResultSearchDto searchDto);
        List<LabTestType> GetLabTestTypes();
        LabTestResult AddLabTestResult(LabTestResult labTestResult);
        LabTestResult GetLabTestResultById(int id);
        bool UpdateLabTestResult(LabTestResult labTestResult);
        bool DeleteLabTestResult(int id);
    }   
    public class LabTestResultService : ILabTestResultService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;

        public LabTestResultService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public LabTestResult AddLabTestResult(LabTestResult labTestResult)
        {
            try
            {

                var unitOfWork = RepositoryHelper.GetUnitOfWork();
                var context = unitOfWork.Context;
                using (var transaction = context.Database.BeginTransaction())
                {
                    var labTestResultTmp = new LabTestResult()
                    {
                        PatientId = labTestResult.PatientId,
                        Comment = labTestResult.Comment,
                        CreatedDate = DateTime.UtcNow.AddHours(7)
                    };
                    context.Set<LabTestResult>().Add(labTestResultTmp);
                    var result = unitOfWork.SaveChanges();
                    if (result.Any())
                    {
                        transaction.Rollback();
                        return null;
                    }

                    var labTestResultDetails = labTestResult.LabTestResultDetails.Where(x => !String.IsNullOrEmpty(x.Value)).ToList();
                    labTestResultDetails.ForEach(x =>
                    {
                        x.LabTestResultId = labTestResultTmp.LabTestResultId;
                    });
                    context.Set<LabTestResultDetail>().AddRange(labTestResultDetails);
                    result = unitOfWork.SaveChanges();
                    if (result.Any())
                    {
                        transaction.Rollback();
                        return null ;
                    }
                    transaction.Commit();
                    return labTestResultTmp;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
           
            
        }

        public bool DeleteLabTestResult(int id)
        {
            var unitOfWork = RepositoryHelper.GetUnitOfWork();
            var data = unitOfWork.Context.Set<LabTestResult>()
                .Include(x => x.LabTestResultDetails)
                .SingleOrDefault(x => x.LabTestResultId == id);
            unitOfWork.Context.Set<LabTestResultDetail>().RemoveRange(data.LabTestResultDetails);
            var labTest = unitOfWork.Context.Set<LabTestResult>().Remove(data);
            try
            {
                unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<LabTestResult> GetAllLabTestResult(LabTestResultSearchDto searchDto)
        {
            var unitOfWork = RepositoryHelper.GetUnitOfWork();
            var labTest = unitOfWork.Context.Set<LabTestResult>().AsQueryable()
                .Include(x => x.Patient)
                .Where(x => (searchDto.PatientId == null || x.PatientId == searchDto.PatientId))
                .OrderByDescending(x => x.CreatedDate).ToList();
            return labTest;
        }

        public LabTestResult GetLabTestResultById(int id)
        {
            var unitOfWork = RepositoryHelper.GetUnitOfWork();
            var labTest = unitOfWork.Context.Set<LabTestResult>().AsQueryable()
                .Include(x => x.LabTestResultDetails.Select(y => y.LabTestDetail.LabTestType))
                .Include(x => x.Patient)
                .FirstOrDefault(x => x.LabTestResultId == id);
            return labTest;
        }

        public List<LabTestType> GetLabTestTypes()
        {
            var unitOfWork = RepositoryHelper.GetUnitOfWork();
            var labTest = unitOfWork.Context.Set<LabTestType>().AsQueryable()
                .Include(x => x.LabTestDetails)
                .ToList();
            return labTest;
        }

        public bool UpdateLabTestResult(LabTestResult labTestResult)
        {
            try
            {
                var unitOfWork = RepositoryHelper.GetUnitOfWork();
                var context = unitOfWork.Context;
                var labTest = context.Set<LabTestResult>()
                    .Include(x => x.LabTestResultDetails)
                    .SingleOrDefault(x => x.LabTestResultId == labTestResult.LabTestResultId);
                labTest.Patient = null;
                labTest.PatientId = labTestResult.PatientId;
                labTest.Comment = labTestResult.Comment;
                context.Set<LabTestResultDetail>().RemoveRange(labTest.LabTestResultDetails);
                var labTestResultDetails = labTestResult.LabTestResultDetails.Where(x => !String.IsNullOrEmpty(x.Value)).ToList();
                labTest.LabTestResultDetails = labTestResultDetails;
                
                var result = unitOfWork.SaveChanges();
                if (result.Any())
                {
                    
                    return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
