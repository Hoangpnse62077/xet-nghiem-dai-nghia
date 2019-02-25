using AutoMapper;
using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using eLTMS.DataAccess.Repositories;
using eLTMS.Models.Models.dto;
using eLTMS.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLTMS.Models;
using eLTMS.Models.Enums;
//using eLTMS.Models.;

namespace eLTMS.BusinessLogic.Services
{
    public interface IAppointmentService
    {
        ResponseObjectDto Create(AppointmentDto appointment);
        Appointment GetSingleByCode(string code);
        Appointment GetSingleById(int appointmentId); // Author: DucBM
        Appointment GetResultDoneByAppointmentId(int appointmentId); // Author: DucBM
        List<AppointmentDto> GetAppointmentsByPatientId(int patientId); // Author: DucBM
        List<AppointmentDto> GetAppointmentsByAccountId(int accountId); // Author: DucBM
        List<Appointment> GetResult(int patientId);
        List<Appointment> GetResultDone(int patientId);
        List<Appointment> GetAppByPhone(string phone);
        List<Appointment> GetResultByAppCode(string appCode);
        ResponseObjectDto UpdateAppointment(int appointmentId, List<SampleGettingDto> sgDtos);
        bool Update(string code, string con, string cmt);
        ResponseObjectDto DeleteAppointment(int appointmentId);
        List<Token> GetAllTokens();
    }
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public AppointmentService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public List<Appointment> GetAppByPhone(string phone)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var sampleRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);
            var apps = appRepo.GetAppointmentByPhone(phone);
            return apps;
        }


        public Appointment GetSingleById(int appointmentId)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var result = appRepo.GetAppointmentByIdInclude(appointmentId);
            return result;
        }
        public Appointment GetSingleByCode(string code)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var result = appRepo.GetAppointmentByCode(code);
            return result;
        }

        public Appointment GetResultDoneByAppointmentId(int appointmentId)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var result = appRepo.GetResultDoneByAppointmentId(appointmentId);
            return result;
        }


        public ResponseObjectDto Create(AppointmentDto appointmentDto)
        {
            var responseObject = new ResponseObjectDto();
            responseObject.Success = true;
            responseObject.Message = "Đặt lịch thành công";

            try
            {
                var appointmentRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
                var sgRepo = this.RepositoryHelper.GetRepository<ISampleGettingRepository>(this.UnitOfWork);
                var sampleRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);
                var tableRepo = this.RepositoryHelper.GetRepository<ITableRepository>(this.UnitOfWork);
                var patientRepo = this.RepositoryHelper.GetRepository<IPatientRepository>(this.UnitOfWork);
                var patientAccountRepo = this.RepositoryHelper.GetRepository<IPatientAccountRepository>(this.UnitOfWork);
                var accRepo = this.RepositoryHelper.GetRepository<IAccountRepository>(this.UnitOfWork);

                Patient patient = null;
                if (appointmentDto.PatientDto != null) // Make Ap. without Login
                {
                    //var idcNumber = appointmentDto.PatientDto.IdentityCardNumber;
                    //patient = patientRepo.GetByIDCNumber(idcNumber);
                    var phone = appointmentDto.PatientDto.PhoneNumber;
                    var account = accRepo.GetByPhoneNumber(phone);

                    if (account == null)
                    {
                        account = new Account();
                        account.FullName = appointmentDto.PatientDto.FullName;
                        account.PhoneNumber = phone;
                        account.Password = ConstantManager.DEFAULT_PASSWORD;
                        account.RoleId = (int)RoleEnum.Patient;
                        account.IsDeleted = false;
                        accRepo.Create(account);
                        try
                        {
                            var result = this.UnitOfWork.SaveChanges();
                            if (result.Any())
                            {
                                responseObject.Success = false;
                                responseObject.Message = "Có lỗi xảy ra";
                                responseObject.Data = result;
                                return responseObject;
                            }
                        }
                        catch (Exception ex)
                        {
                            responseObject.Success = false;
                            responseObject.Message = "Có lỗi xảy ra";
                            responseObject.Data = ex;
                            return responseObject;
                        }
                        responseObject.Data = new
                        {
                            appointmentDto.PatientDto.PhoneNumber,
                            DefaultPassword = account.Password
                        };
                    }
                    int accountId = account.AccountId;

                    var dateOfBirth = DateTime.Parse(appointmentDto.PatientDto.DateOfBirth);
                    patient = patientRepo.GetBy(accountId, appointmentDto.PatientDto.FullName, dateOfBirth);
                    if (patient == null)
                    {
                        // Create a new Patient
                        patient = new Patient();
                        patient.AccountId = accountId;
                        patient.FullName = appointmentDto.PatientDto.FullName;
                        patient.IdentityCardNumber = appointmentDto.PatientDto.IdentityCardNumber;
                        patient.PhoneNumber = appointmentDto.PatientDto.PhoneNumber;
                        patient.DateOfBirth = dateOfBirth;
                        patient.Gender = appointmentDto.PatientDto.Gender;
                        patient.HomeAddress = appointmentDto.PatientDto.HomeAddress;
                        patient.IsDeleted = false;

                        patientRepo.Create(patient);
                        try
                        {
                            var result = this.UnitOfWork.SaveChanges();
                            if (result.Any())
                            {
                                responseObject.Success = false;
                                responseObject.Message = "Có lỗi xảy ra";
                                responseObject.Data = result;
                                return responseObject;
                            }
                        }
                        catch (Exception ex)
                        {
                            responseObject.Success = false;
                            responseObject.Message = "Có lỗi xảy ra";
                            responseObject.Data = ex;
                            return responseObject;
                        }
                        patient.PatientCode = "BN" + patient.PatientId;
                    }

                    if (account != null && patient != null)
                    {
                        var patientAccount = new PatientAccount();
                        patientAccount.AccountId = account.AccountId;
                        patientAccount.PatientId = patient.PatientId;
                        patientAccount.IsDeleted = false;
                        patientAccountRepo.Create(patientAccount);
                        var result = this.UnitOfWork.SaveChanges();
                        if (result.Any())
                        {
                            responseObject.Success = false;
                            responseObject.Message = "Có lỗi xảy ra";
                            responseObject.Data = result;
                            return responseObject;
                        }
                    }

                    appointmentDto.PatientId = patient.PatientId;
                }
                else if (appointmentDto.PatientId != null) // Make Ap. with Login
                {
                    patient = patientRepo.GetById((int)appointmentDto.PatientId);
                    if (patient == null)
                    {
                        responseObject.Success = false;
                        responseObject.Message = "Có lỗi xảy ra";
                        responseObject.Data = new
                        {
                            MessageForDev = "Không tồn tại PatientId này"
                        };
                        return responseObject;
                    }
                }
                else
                {
                    responseObject.Success = false;
                    responseObject.Message = "Có lỗi xảy ra";
                    responseObject.Data = new
                    {
                        MessageForDev = "PatientId truyền vào là Null"
                    };
                    return responseObject;
                }

                var appointment = new Appointment();
                // Convert AppointmentDto to Appointment
                var now = DateTime.Now;
                var sDate = now.ToString("yyyy-MM-dd");

                // Generate code
                var lastcode = appointmentRepo.GetLastCode(sDate);
                var count = 0;
                if (lastcode != null)
                {
                    count = int.Parse(lastcode.Substring("yyyy-MM-dd-".Length));
                }
                var code = sDate + "-" + (count + 1);


                appointment.PatientId = patient.PatientId;
                appointment.AppointmentCode = code;
                appointment.Status = "NEW";
                appointment.PatientId = appointmentDto.PatientId;
                appointment.EnterTime = now;
                appointment.IsOnline = true;
                appointment.IsDeleted = false;

                appointment.SampleGettings = new List<SampleGetting>();

                var sampleGettingDtos = appointmentDto.SampleGettingDtos;
                foreach (var sgDto in appointmentDto.SampleGettingDtos)
                {
                    var duplicatedSG = sgRepo.GetFirst(sgDto.SampleId, sgDto.GettingDate, (int)appointmentDto.PatientId);
                    if (duplicatedSG != null)
                    {
                        var sampleName = sampleRepo.GetById(sgDto.SampleId).SampleName;
                        var date = DateTime.Parse(sgDto.GettingDate).ToString("dd-MM-yyyy");
                        responseObject.Success = false;
                        responseObject.Message = string.Format("Bạn đã từng đăng ký lấy mẫu {0} vào ngày {1}.\n Bạn không thể lấy mẫu {0} 2 lần 1 ngày.", sampleName, date);
                        return responseObject;
                    }

                    var sg = Mapper.Map<SampleGettingDto, SampleGetting>(sgDto);
                    var avaiTable = tableRepo.GetFirstAvailableTable((int)sg.SlotId, (DateTime)sg.GettingDate);
                    if (avaiTable == null)
                    {
                        responseObject.Success = false;
                        responseObject.Message = "Có ca xét nghiệm đã hết chỗ";
                        return responseObject;
                    }
                    sg.TableId = avaiTable.TableId;
                    sg.Status = "NEW";
                    sg.LabTestings = new List<LabTesting>();
                    sg.IsGot = false;
                    sg.IsPaid = false;
                    sg.IsDeleted = false;
                    foreach (var id in sgDto.LabTestIds)
                    {
                        var labTesting = new LabTesting();
                        labTesting.LabTestId = id;
                        labTesting.SampleGettingId = sg.SampleGettingId;
                        labTesting.Status = "NEW";
                        labTesting.IsDeleted = false;
                        sg.LabTestings.Add(labTesting);
                    }
                    appointment.SampleGettings.Add(sg);
                }
                // Create
                appointmentRepo.Create(appointment);
                try
                {
                    var result = this.UnitOfWork.SaveChanges();
                    if (result.Any())
                    {
                        responseObject.Success = false;
                        responseObject.Message = "Có lỗi xảy ra";
                        responseObject.Data = result;
                        return responseObject;
                    }
                }
                catch (Exception ex)
                {
                    responseObject.Success = false;
                    responseObject.Message = "Có lỗi xảy ra";
                    responseObject.Data = ex;
                    return responseObject;
                }
            }
            catch (Exception ex)
            {
                responseObject.Success = false;
                responseObject.Message = "Có lỗi xảy ra";
                responseObject.Data = ex;
                return responseObject;
            }

            return responseObject;
        }

        public List<AppointmentDto> GetAppointmentsByAccountId(int accountId)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var apps = appRepo.GetAppointmentsByAccountId(accountId);
            var appDtos = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDto>>(apps).ToList();
            return appDtos;
        }

        public List<AppointmentDto> GetAppointmentsByPatientId(int patientId)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var apps = appRepo.GetAppointmentsByPatientId(patientId);
            var appDtos = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDto>>(apps).ToList();
            return appDtos;
        }

        public List<Appointment> GetResult(int patientId)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var sampleRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);
            var apps = appRepo.GetResultByPatientId(patientId);
            return apps;
        }
        public List<Appointment> GetResultDone(int patientId)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var sampleRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);
            var apps = appRepo.GetResultDoneByPatientId(patientId);
            return apps;
        }
        public List<Appointment> GetResultByAppCode(string appCode)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var sampleRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);
            var apps = appRepo.GetResultByAppCode(appCode);
            return apps;
        }

        // Author: DucBM
        public ResponseObjectDto UpdateAppointment(int appointmentId, List<SampleGettingDto> sampleGettingDtos)
        {
            var responseObject = new ResponseObjectDto();
            responseObject.Success = true;
            responseObject.Message = "Chỉnh sửa lịch thành công";

            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            // get existing appointment by AppointmentCode

            var appointment = appRepo.GetAppointmentByIdInclude(appointmentId);
            // delete old records
            // TEMPORARY !! -> too waist memory !!
            foreach (var sg in appointment.SampleGettings)
            {
                sg.IsDeleted = true;
                foreach (var lt in sg.LabTestings)
                {
                    lt.IsDeleted = true;
                }
            }

            // modify SampleGettings property
            appointment.SampleGettings = new List<SampleGetting>();
            foreach (var sgDto in sampleGettingDtos)
            {
                var sg = Mapper.Map<SampleGettingDto, SampleGetting>(sgDto);
                sg.LabTestings = new List<LabTesting>();
                sg.Status = "NEW";
                sg.TableId = 1;
                sg.IsDeleted = false;
                foreach (var id in sgDto.LabTestIds)
                {
                    var lt = new LabTesting();
                    lt.LabTestId = id;
                    sg.LabTestings.Add(lt);
                }
                appointment.SampleGettings.Add(sg);
            }
            try
            {
                // update entity
                appRepo.Update(appointment);
                // save to DB
                var result = this.UnitOfWork.SaveChanges();
                if (result.Any())
                {
                    responseObject.Success = false;
                    responseObject.Message = "Có lỗi xảy ra";
                    responseObject.Data = result;
                }
            }
            catch (Exception ex)
            {
                responseObject.Success = false;
                responseObject.Message = "Có lỗi xảy ra";
                responseObject.Data = new
                {
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }

            return responseObject;
        }

        public bool Update(string code, string con, string cmt)
        {
            try
            {
                var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
                // get existing appointment by AppointmentCode
                var appointment = appRepo.GetAppointmentByCode(code);
                // modify SampleGettings property               
                appointment.Conclusion = con;
                appointment.DoctorComment = cmt;
                appointment.Status = "DOCTORDONE";
                appointment.ReturnTime = DateTime.Now;

                foreach (var sg in appointment.SampleGettings)
                {
                    foreach (var lt in sg.LabTestings)
                    {
                        lt.Status = "DOCTORDONE";
                    }
                }

                // update entity
                appRepo.Update(appointment);
                // save to DB
                this.UnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        // Author: DucBM
        public ResponseObjectDto DeleteAppointment(int appointmentId)
        {
            var responseObject = new ResponseObjectDto();
            responseObject.Success = true;

            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            // get existing appointment by AppointmentCode
            var appointment = appRepo.GetAppointmentById(appointmentId);
            if (appointment == null)
            {
                responseObject.Success = false;
                responseObject.Message = "Có lỗi xảy ra";
                return responseObject;
            }
            else
            {
                // assign IsDeleted = true
                appointment.IsDeleted = true;
                // update entity
                appRepo.Update(appointment);
                try
                {
                    // save to DB
                    var result = this.UnitOfWork.SaveChanges();
                    if (result.Any())
                    {
                        responseObject.Success = false;
                        responseObject.Message = "Có lỗi xảy ra";
                        responseObject.Data = result;
                    }
                }
                catch (Exception ex)
                {
                    responseObject.Success = false;
                    responseObject.Message = "Có lỗi xảy ra";
                    responseObject.Data = ex;
                }
            }
            return responseObject;
        }


        public List<Token> GetAllTokens()
        {
            var repo = this.RepositoryHelper.GetRepository<ITokenRepository>(UnitOfWork);
            var tokens = repo.GetAll();
            return tokens;
        }
    }
}
