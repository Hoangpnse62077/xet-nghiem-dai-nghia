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
    public interface IFeedbackService
    {
        List<Feedback> GetAllFeed(string dateTime);
        Feedback getFeedbackId(int id);
        bool DeleteFeedback(int id);
        bool Update(Feedback feedbackdto);
        bool Create(Feedback feedback);
    }
    public class FeedbackService: IFeedbackService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public FeedbackService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork(); 
        }
        public List<Feedback> GetAllFeed(string dateTime)
        {
            var feedRepo = this.RepositoryHelper.GetRepository<IFeedbackRepository>(UnitOfWork);
            var feedback = feedRepo.GetAllFeed(dateTime);
            return feedback;
        }
        public Feedback getFeedbackId(int id)
        {
            var feedbackRepo = this.RepositoryHelper.GetRepository<IFeedbackRepository>(UnitOfWork);
            var feedbacks = feedbackRepo.GetSimpleById(id);
            return feedbacks;
        }
        public bool DeleteFeedback(int id)
        {
            var repo = RepositoryHelper.GetRepository<IFeedbackRepository>(UnitOfWork);
            try
            {
                var feedback = repo.GetById(id);
                feedback.IsDeleted = true;
                repo.Update(feedback);
                UnitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public bool Update (Feedback feedbackdto)
        {
            var repo = RepositoryHelper.GetRepository<IFeedbackRepository>(UnitOfWork);
            try
            {
                var feedback = repo.GetSimpleById(feedbackdto.FeedbackId);
                feedback.Status = feedbackdto.Status;
                repo.Update(feedback);
                var result = UnitOfWork.SaveChanges();
                if (result.Any()) return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool Create(Feedback feedback)
        {
            var repo = RepositoryHelper.GetRepository<IFeedbackRepository>(UnitOfWork);
            repo.Create(feedback);
            var result = UnitOfWork.SaveChanges();
            return true;
        }
    }
}
