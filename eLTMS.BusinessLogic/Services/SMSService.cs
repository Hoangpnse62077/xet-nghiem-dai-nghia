using AutoMapper;
using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using eLTMS.DataAccess.Repositories;
using eLTMS.Models;
using eLTMS.Models.Enums;
using eLTMS.Models.Models;
using eLTMS.Models.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.BusinessLogic.Services
{
    public interface ISmsService
    {
        //Account SendSms(string phoneNumber, string password);
    }
    public class SmsService : ISmsService
    {
        /*private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public SmsService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }
        
        public Account checkLogin(string phoneNumber, string password)
        {
            var repo = RepositoryHelper.GetRepository<IAccountRepository>(this.UnitOfWork);
            Account account = repo.GetByPhoneNumberIncludeRoleEmployeePatient(phoneNumber);
            if (account!=null && account.Password.Trim().Equals(password.Trim()))
            {
                return account;
            }
            return null;
        }/**/
    }
}
