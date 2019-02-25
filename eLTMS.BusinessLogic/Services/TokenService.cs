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
    public interface ITokenService
    {
        List<Token> GetAll();
        bool Create(string tokenString);
    }
    public class TokenService : ITokenService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public TokenService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }
        public List<Token> GetAll()
        {
            var repo = this.RepositoryHelper.GetRepository<ITokenRepository>(UnitOfWork);
            var tokens = repo.GetAll();
            return tokens;
        }

        //DucBM
        public bool Create(string tokenString)
        {
            var repo = this.RepositoryHelper.GetRepository<ITokenRepository>(UnitOfWork);
            // check if exist
            var t = repo.GetByTokenString(tokenString);
            if (t != null)
            {
                return false; // exist
            }
            // create new
            var token = new Token();
            token.TokenString = tokenString;
            token.IsDeleted = false;
            repo.Create(token);
            // save change
            var result = UnitOfWork.SaveChanges();
            if (result.Any())
            {
                return false;
            }
            return true;
        }
    }
}
