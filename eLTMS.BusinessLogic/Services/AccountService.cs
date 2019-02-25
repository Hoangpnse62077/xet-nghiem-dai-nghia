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
    public interface IAccountService
    {
        Account checkLogin(string phoneNumber, string password);
        ResponseObjectDto CheckLoginPatient(string phoneNumber, string password);
        ResponseObjectDto CheckLoginPatientForWeb(string phoneNumber, string password);
        ResponseObjectDto RegisterPatient(RegisterDto regDto);
        Account GetById(int accountId);
    }
    public class AccountService : IAccountService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public AccountService(IRepositoryHelper repositoryHelper)
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

        // DucBM
        public ResponseObjectDto CheckLoginPatientForWeb(string phoneNumber, string password)
        {
            var repo = RepositoryHelper.GetRepository<IAccountRepository>(this.UnitOfWork);
            Account account = repo.GetAccountPatientForLogin(phoneNumber, password);
            var accountDto = Mapper.Map<Account, AccountDto>(account);

            var respObj = new ResponseObjectDto();
            if (account != null)
            {
                var patientAccount = account.PatientAccounts.FirstOrDefault();
                if (patientAccount != null)
                {
                    accountDto.PatientId = patientAccount.PatientId;
                } /**/

                respObj.Success = true;
                respObj.Message = "Đăng nhập thành công";
                respObj.Data = account;
            }
            else
            {
                respObj.Success = false;
                respObj.Message = "Sai số điện thoại hoặc mật khẩu";
                respObj.Data = null;
            }
            return respObj;
        }

        // DucBM -- for Mobile
        public ResponseObjectDto CheckLoginPatient(string phoneNumber, string password)
        {
            var repo = RepositoryHelper.GetRepository<IAccountRepository>(this.UnitOfWork);
            Account account = repo.GetAccountPatientForLogin(phoneNumber, password);
            var accountDto = Mapper.Map<Account, AccountDto>(account);

            var respObj = new ResponseObjectDto();
            if (account != null)
            {
                var patientAccount = account.PatientAccounts.FirstOrDefault();
                if (patientAccount != null)
                {
                    accountDto.PatientId = patientAccount.PatientId;
                } /**/

                respObj.Success = true;
                respObj.Message = "Đăng nhập thành công";
                respObj.Data = accountDto;
            }
            else
            {
                respObj.Success = false;
                respObj.Message = "Sai số điện thoại hoặc mật khẩu";
                respObj.Data = null;
            }
            return respObj;
        }

        // DucBM
        public ResponseObjectDto RegisterPatient(RegisterDto regDto)
        {
            var accountRepo = RepositoryHelper.GetRepository<IAccountRepository>(this.UnitOfWork);
            var patientRepo = RepositoryHelper.GetRepository<IPatientRepository>(this.UnitOfWork);
            var paRepo = RepositoryHelper.GetRepository<IPatientAccountRepository>(this.UnitOfWork);

            var respObj = new ResponseObjectDto();
            respObj.Success = true;
            respObj.Message = "Đăng ký thành công";
            respObj.Data = null;

            var existAcc = accountRepo.GetByPhoneNumber(regDto.PhoneNumber);
            if (existAcc != null)
            {
                respObj.Success = false;
                respObj.Message = "Số điện thoại này đã được đăng ký tài khoản trước đây.";
                respObj.Data = null;
            }
            else
            {
                var account = new Account();
                account.FullName = regDto.FullName;
                account.Email = regDto.Email;
                account.PhoneNumber = regDto.PhoneNumber;
                account.IdentityCardNumber = regDto.IdentityCardNumber;
                account.Password = regDto.Password;
                account.RoleId = (int)RoleEnum.Patient;
                account.IsDeleted = false;

                var patient = new Patient();
                patient.FullName = regDto.FullName;
                patient.PhoneNumber = regDto.PhoneNumber;
                patient.IdentityCardNumber = regDto.IdentityCardNumber;
                patient.IsDeleted = false;

                using (var trans = UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        accountRepo.Create(account);
                        var result = UnitOfWork.SaveChanges();
                        if (result.Any())
                        {
                            trans.Rollback();
                            respObj.Success = false;
                            respObj.Message = "Đăng ký không thành công";
                            respObj.Data = result;
                            return respObj;
                        }

                        patientRepo.Create(patient);
                        result = UnitOfWork.SaveChanges();
                        patient.PatientCode = "BN" + patient.PatientId; 
                        if (result.Any())
                        {
                            trans.Rollback();
                            respObj.Success = false;
                            respObj.Message = "Đăng ký không thành công";
                            respObj.Data = result;
                            return respObj;
                        }

                        var patientAccount = new PatientAccount();
                        patientAccount.PatientId = patient.PatientId;
                        patientAccount.AccountId = account.AccountId;
                        patientAccount.IsDeleted = false;

                        paRepo.Create(patientAccount);
                        result = UnitOfWork.SaveChanges();
                        if (result.Any())
                        {
                            trans.Rollback();
                            respObj.Success = false;
                            respObj.Message = "Đăng ký không thành công";
                            respObj.Data = result;
                            return respObj;
                        }

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        respObj.Success = false;
                        respObj.Message = "Đăng ký không thành công";
                        respObj.Data = ex;
                    }
                }
            }

            return respObj;
        }

        public Account GetById(int accountId)
        {
            var repo = RepositoryHelper.GetRepository<IAccountRepository>(this.UnitOfWork);
            Account account = repo.GetById(accountId);
            return account;
        }
    }
}
