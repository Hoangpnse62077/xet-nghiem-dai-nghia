using AutoMapper;
using eLTMS.Models.Models.dto;
using eLTMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eLTMS.DataAccess.Models;
using eLTMS.Models.Utils;

namespace eLTMS.Models.Mapping
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Account, AccountDto>();

                cfg.CreateMap<Supply, SupplyDto>()
                .ForMember(dst => dst.SuppliesId, src => src.MapFrom(x => x.SuppliesId))
                .ForMember(dst => dst.SuppliesName, src => src.MapFrom(x => x.SuppliesName))
                .ForMember(dst => dst.SuppliesTypeName, src => src.MapFrom(x => x.SupplyType.SuppliesTypeName))
                .ForMember(dst => dst.SuppliesCode, src => src.MapFrom(x => x.SuppliesCode))
                .ForMember(dst => dst.Quantity, src => src.MapFrom(x => x.Quantity))
                .ForMember(dst => dst.Unit, src => src.MapFrom(x => x.Unit))
                .ForMember(dst => dst.SuppliesTypeId, src => src.MapFrom(x => x.SuppliesTypeId))
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
                .ForMember(dst => dst.Note, src => src.MapFrom(x => x.Note));

                cfg.CreateMap<Patient, PatientDto>()
                .ForMember(dst => dst.PatientId, src => src.MapFrom(x => x.PatientId))
                .ForMember(dst => dst.FullName, src => src.MapFrom(x => x.FullName))
                .ForMember(dst => dst.Gender, src => src.MapFrom(x => x.Gender))
                .ForMember(dst => dst.PatientCode, src => src.MapFrom(x => x.PatientCode))
                .ForMember(dst => dst.PhoneNumber, src => src.MapFrom(x => x.PhoneNumber))
                .ForMember(dst => dst.IdentityCardNumber, src => src.MapFrom(x => x.IdentityCardNumber))
                .ForMember(dst => dst.HomeAddress, src => src.MapFrom(x => x.HomeAddress))
                .ForMember(dst => dst.AccountId, src => src.MapFrom(x => x.AccountId))
                .ForMember(dst => dst.DateOfBirth, src => src.MapFrom(x => (x.DateOfBirth != null) ? x.DateOfBirth.Value.ToString("dd-MM-yyyy") : ""))
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
                .ForMember(dst => dst.Age, src => src.MapFrom(x => x.Age))
                .ForMember(dst => dst.CompanyAddress, src => src.MapFrom(x => x.CompanyAddress));

                cfg.CreateMap<LabTest, LabTestDto>()
                .ForMember(dst => dst.LabTestId, src => src.MapFrom(x => x.LabTestId))
                .ForMember(dst => dst.LabTestName, src => src.MapFrom(x => x.LabTestName))
                .ForMember(dst => dst.LabTestCode, src => src.MapFrom(x => x.LabTestCode))
                .ForMember(dst => dst.Description, src => src.MapFrom(x => x.Description))
                .ForMember(dst => dst.Price, src => src.MapFrom(x => x.Price))
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
                .ForMember(dst => dst.SampleName, src => src.MapFrom(x => x.Sample.SampleName))
                .ForMember(dst => dst.SampleId, src => src.MapFrom(x => x.SampleId));


                cfg.CreateMap<Sample, SampleDto>()
                .ForMember(dst => dst.SampleId, src => src.MapFrom(x => x.SampleId))
                .ForMember(dst => dst.SampleName, src => src.MapFrom(x => x.SampleName))
                .ForMember(dst => dst.SampleGroupId, src => src.MapFrom(x => x.SampleGroupId))
                .ForMember(dst => dst.Description, src => src.MapFrom(x => x.Description))
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
                .ForMember(dst => dst.SampleGroupName, src => src.MapFrom(x => x.SampleGroup.GroupName))
                .ForMember(dst => dst.SampleDuration, src => src.MapFrom(x => x.SampleGroup.GettingDuration))
                .ForMember(dst => dst.OpenTime, src => src.MapFrom(x => x.SampleGroup.OpenTime))
                .ForMember(dst => dst.CloseTime, src => src.MapFrom(x => x.SampleGroup.CloseTime))
                .ForMember(dst => dst.LabTests, src => src.MapFrom(x => x.LabTests))
                .ForMember(dst => dst.SlotDtos, src => src.MapFrom(x => x.SampleGroup.Slots));


                cfg.CreateMap<SampleGroup, SampleGroupDto>()
                .ForMember(dst => dst.SampleGroupId, src => src.MapFrom(x => x.SampleGroupId))
                .ForMember(dst => dst.GroupName, src => src.MapFrom(x => x.GroupName))
                .ForMember(dst => dst.GettingDuration, src => src.MapFrom(x => x.GettingDuration))
                .ForMember(dst => dst.OpenTime, src => src.MapFrom(x => x.OpenTime))
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
                .ForMember(dst => dst.CloseTime, src => src.MapFrom(x => x.CloseTime));

                cfg.CreateMap<ImportPaper, ImportPaperDto>()
                .ForMember(dst => dst.ImportPaperId, src => src.MapFrom(x => x.ImportPaperId))
                .ForMember(dst => dst.ImportPaperCode, src => src.MapFrom(x => x.ImportPaperCode))
                .ForMember(dst => dst.CreateDate, src => src.MapFrom(x => x.CreateDate))
                .ForMember(dst => dst.AccountId, src => src.MapFrom(x => x.AccountId))
                .ForMember(dst => dst.Note, src => src.MapFrom(x => x.Note))
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
                .ForMember(dst => dst.ImportPaperDetailDtos, src => src.MapFrom(x => x.ImportPaperDetails));

                cfg.CreateMap<ImportPaperDetail, ImportPaperDetailDto>()
                .ForMember(dst => dst.ImportPaperDetailId, src => src.MapFrom(x => x.ImportPaperDetailId))
                .ForMember(dst => dst.ImportPaperId, src => src.MapFrom(x => x.ImportPaperId))
                .ForMember(dst => dst.SuppliesId, src => src.MapFrom(x => x.SuppliesId))
                .ForMember(dst => dst.Unit, src => src.MapFrom(x => x.Unit))
                .ForMember(dst => dst.Quantity, src => src.MapFrom(x => x.Quantity))
                .ForMember(dst => dst.Note, src => src.MapFrom(x => x.Note))
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted));

                cfg.CreateMap<SampleGettingDto, SampleGetting>();

                cfg.CreateMap<SampleGetting, SampleGettingDto>()
                .ForMember(dst => dst.SampleId, src => src.MapFrom(x => x.SampleId))
                .ForMember(dst => dst.SampleName, src => src.MapFrom(x => x.Sample.SampleName))
                .ForMember(dst => dst.GettingDate, src => src.MapFrom(x => (x.GettingDate != null) ? x.GettingDate.Value.ToString("yyyy-MM-dd") : ""))
                .ForMember(dst => dst.FmStartTime, src => src.MapFrom(x => DateTimeUtils.ConvertSecondToShortHour((int)x.Slot.StartTime)))
                .ForMember(dst => dst.FmFinishTime, src => src.MapFrom(x => DateTimeUtils.ConvertSecondToShortHour((int)x.Slot.FinishTime)))
                .ForMember(dst => dst.LabTestIds, src => src.MapFrom(x => x.LabTestings.Select(y => y.LabTestId)))
                .ForMember(dst => dst.LabTests, src => src.MapFrom(x => x.LabTestings.Select(y => y.LabTest)));

                cfg.CreateMap<SampleGetting, SampleGettingForNurseDto>()
                .ForMember(dst => dst.SampleGettingCode, src => src.MapFrom(x => x.SampleGettingId.ToString()))
                .ForMember(dst => dst.SampleName, src => src.MapFrom(x => x.Sample.SampleName))
                .ForMember(dst => dst.PatientName, src => src.MapFrom(x => x.Appointment.Patient.FullName));

                //cfg.CreateMap<Appointment, SampleGettingForReceptionistDto>()
                //.ForMember(dst => dst.AppointmentCode, src => src.MapFrom(x => x.Appointment.AppointmentCode))
                //.ForMember(dst => dst.PatientName, src => src.MapFrom(x => x.Appointment.Patient.FullName))
                //.ForMember(dst => dst.PatientAddress, src => src.MapFrom(x => x.Appointment.Patient.HomeAddress))
                //.ForMember(dst => dst.PatientPhone, src => src.MapFrom(x => x.Appointment.Patient.PhoneNumber))
                //.ForMember(dst => dst.TableName, src => src.MapFrom(x => x.Table.TableName))
                //.ForMember(dst => dst.EnterTime, src => src.MapFrom(x => (x.EnterTime != null) ? x.EnterTime.Value.ToString("dd-MM-yyyy HH:mm") : ""));
                //.ForMember(dst => dst.FmFinishTime, src => src.MapFrom(x => DateTimeUtils.ConvertSecondToShortHour((int)x.Slot.FinishTime)));

                cfg.CreateMap<ExportPaper, ExportPaperDto>()
                .ForMember(dst => dst.ExportPaperId, src => src.MapFrom(x => x.ExportPaperId))
                .ForMember(dst => dst.ExportPaperCode, src => src.MapFrom(x => x.ExportPaperCode))
                .ForMember(dst => dst.CreateDate, src => src.MapFrom(x => x.CreateDate))
                .ForMember(dst => dst.AccountId, src => src.MapFrom(x => x.AccountId))
                .ForMember(dst => dst.Note, src => src.MapFrom(x => x.Note))
                 .ForMember(dst => dst.Status, src => src.MapFrom(x => x.Status))
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
                .ForMember(dst => dst.ExportPaperDetailDtos, src => src.MapFrom(x => x.ExportPaperDetails));

                cfg.CreateMap<Appointment, AppointmentGetByPhoneDto>()
                .ForMember(dst => dst.PatientName, src => src.MapFrom(x => x.Patient.FullName))
                .ForMember(dst => dst.PhoneNumber, src => src.MapFrom(x => x.Patient.PhoneNumber))
                .ForMember(dst => dst.Address, src => src.MapFrom(x => x.Patient.HomeAddress));

                cfg.CreateMap<Appointment, AppointmentGetResultDto>()
                .ForMember(dst => dst.AppCode, src => src.MapFrom(x => x.AppointmentCode))
                .ForMember(dst => dst.PatientName, src => src.MapFrom(x => x.Patient.FullName))
                .ForMember(dst => dst.Age, src => src.MapFrom(x => x.Patient.DateOfBirth))
                .ForMember(dst => dst.Gender, src => src.MapFrom(x => x.Patient.Gender))
                .ForMember(dst => dst.DoctorName, src => src.MapFrom(x => x.Employee.FullName))
                .ForMember(dst => dst.Gender, src => src.MapFrom(x => x.Patient.Gender))
                .ForMember(dst => dst.TestPurpose, src => src.MapFrom(x => x.TestPurpose))
                .ForMember(dst => dst.EnterTime, src => src.MapFrom(x => x.EnterTime))
                .ForMember(dst => dst.ReturnTime, src => src.MapFrom(x => x.ReturnTime))
                .ForMember(dst => dst.Conclusion, src => src.MapFrom(x => x.Conclusion))
                .ForMember(dst => dst.ResultApproved, src => src.MapFrom(x => x.ResultApproved));

                //.ForMember(dst => dst.LabTestingDtos, src => src.MapFrom(x => x.LabTestings));

                cfg.CreateMap<Appointment, ResultOfAppointmentDto>() // Author: DucBM
                .ForMember(dst => dst.DoctorName, src => src.MapFrom(x => (x.Employee != null) ? x.Employee.FullName : ""))
                .ForMember(dst => dst.PatientName, src => src.MapFrom(x => (x.Patient != null) ? x.Patient.FullName : ""))
                .ForMember(dst => dst.PatientBirthYear, src => src.MapFrom(x => (x.Patient != null && x.Patient.DateOfBirth != null) ? x.Patient.DateOfBirth.Value.Year.ToString() : ""))
                .ForMember(dst => dst.PatientGender, src => src.MapFrom(x => (x.Patient != null) ? x.Patient.Gender : ""))
                .ForMember(dst => dst.PatientAddress, src => src.MapFrom(x => (x.Patient != null) ? x.Patient.HomeAddress : ""))
                .ForMember(dst => dst.SampleGettings, src => src.MapFrom(x => x.SampleGettings));

                cfg.CreateMap<SampleGetting, ResultOfSampleGettingDto>() // Author: DucBM
                .ForMember(dst => dst.SampleName, src => src.MapFrom(x => (x.Sample != null) ? x.Sample.SampleName : ""))
                .ForMember(dst => dst.LabTestings, src => src.MapFrom(x => x.LabTestings));

                cfg.CreateMap<LabTesting, ResultOfLabTestingDto>() // Author: DucBM
                .ForMember(dst => dst.LabTestName, src => src.MapFrom(x => (x.LabTest != null) ? x.LabTest.LabTestName : ""))
                .ForMember(dst => dst.LabTestingIndexes, src => src.MapFrom(x => x.LabTestingIndexes));

                cfg.CreateMap<LabTestingIndex, ResultOfLabTestingIndexDto>(); // Author: DucBM


                cfg.CreateMap<Appointment, AppointmentDto>()
                .ForMember(dst => dst.AppointmentCode, src => src.MapFrom(x => x.AppointmentCode))
                .ForMember(dst => dst.AppointmentId, src => src.MapFrom(x => x.AppointmentId))
                .ForMember(dst => dst.Conclusion, src => src.MapFrom(x => x.Conclusion))
                .ForMember(dst => dst.DoctorComment, src => src.MapFrom(x => x.DoctorComment))
                .ForMember(dst => dst.DateResult, src => src.MapFrom(x => (x.ReturnTime != null) ? x.ReturnTime.Value.ToString("dd-MM-yyyy") : ""))
                .ForMember(dst => dst.Status, src => src.MapFrom(x => x.Status))
                .ForMember(dst => dst.DoctorName, src => src.MapFrom(x => (x.Employee != null) ? x.Employee.FullName : ""))
                .ForMember(dst => dst.SampleGettingDtos, src => src.MapFrom(x => x.SampleGettings))
                .ForMember(dst => dst.PatientName, src => src.MapFrom(x => (x.Patient != null) ? x.Patient.FullName : ""))
                .ForMember(dst => dst.PatientDto, src => src.MapFrom(x => x.Patient));


                cfg.CreateMap<AppointmentDto, Appointment>()
                .ForMember(dst => dst.SampleGettings, src => src.MapFrom(x => x.SampleGettingDtos));

                cfg.CreateMap<HospitalSuggestion, HospitalSuggestionDto>()
                .ForMember(dst => dst.DiseaseName, src => src.MapFrom(x => x.DiseaseName))
                .ForMember(dst => dst.HospitalList, src => src.MapFrom(x => x.HospitalList))
                .ForMember(dst => dst.HospitalAdd, src => src.MapFrom(x => x.HospitalAdd))
                .ForMember(dst => dst.HospitalPhone, src => src.MapFrom(x => x.HospitalPhone))
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted));

                cfg.CreateMap<Appointment, AppointmentGetAllDto>()
                .ForMember(dst => dst.AppointmentCode, src => src.MapFrom(x => x.AppointmentCode))
                .ForMember(dst => dst.PatientName, src => src.MapFrom(x => x.Patient.FullName))
                .ForMember(dst => dst.Phone, src => src.MapFrom(x => x.Patient.PhoneNumber))
                .ForMember(dst => dst.Address, src => src.MapFrom(x => x.Patient.HomeAddress))
                .ForMember(dst => dst.Date, src => src.MapFrom(x => x.ReturnTime))
                 .ForMember(dst => dst.DoctorComment, src => src.MapFrom(x => x.DoctorComment))
                 .ForMember(dst => dst.DateOB, src => src.MapFrom(x => x.Patient.DateOfBirth))
                 .ForMember(dst => dst.Gender, src => src.MapFrom(x => x.Patient.Gender))
                .ForMember(dst => dst.Conclusion, src => src.MapFrom(x => x.Conclusion))
                .ForMember(dst => dst.SampleGettingDtos, src => src.MapFrom(x => x.SampleGettings));

                cfg.CreateMap<Appointment, AppointmentUpdateDto>()
                .ForMember(dst => dst.AppCode, src => src.MapFrom(x => x.AppointmentCode))
                .ForMember(dst => dst.PatientId, src => src.MapFrom(x => x.PatientId))
                .ForMember(dst => dst.DoctorId, src => src.MapFrom(x => x.DoctorId))
                .ForMember(dst => dst.IsEmergency, src => src.MapFrom(x => x.IsEmergency))
                .ForMember(dst => dst.TestPurpose, src => src.MapFrom(x => x.TestPurpose))
                .ForMember(dst => dst.EnterTime, src => src.MapFrom(x => x.EnterTime))
                .ForMember(dst => dst.ReturnTime, src => src.MapFrom(x => x.ReturnTime))
                .ForMember(dst => dst.Conlusion, src => src.MapFrom(x => x.Conclusion))
                .ForMember(dst => dst.ResultApproved, src => src.MapFrom(x => x.ResultApproved))
                .ForMember(dst => dst.Status, src => src.MapFrom(x => x.Status));

                cfg.CreateMap<LabTestingIndex, LabTestingIndexDto>()
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
                .ForMember(dst => dst.LabTestingId, src => src.MapFrom(x => x.LabTestingId))
                .ForMember(dst => dst.LabtTestingIndexId, src => src.MapFrom(x => x.LabtTestingIndexId))
                .ForMember(dst => dst.IndexName, src => src.MapFrom(x => x.IndexName))
                .ForMember(dst => dst.IndexValue, src => src.MapFrom(x => x.IndexValue))
                .ForMember(dst => dst.LowNormalHigh, src => src.MapFrom(x => x.LowNormalHigh))
                .ForMember(dst => dst.NormalRange, src => src.MapFrom(x => x.NormalRange))
                .ForMember(dst => dst.Unit, src => src.MapFrom(x => x.Unit));

                cfg.CreateMap<LabTestingIndexDto, LabTestingIndex>();

                cfg.CreateMap<Employee, EmployeeDto>()
                .ForMember(dst => dst.EmployeeID, src => src.MapFrom(x => x.EmployeeId))
                .ForMember(dst => dst.FullName, src => src.MapFrom(x => x.FullName))
                .ForMember(dst => dst.PhoneNumber, src => src.MapFrom(x => x.PhoneNumber))
                .ForMember(dst => dst.RoleDisplay, src => src.MapFrom(x => x.Account.Role.RoleName))
                .ForMember(dst => dst.Role, src => src.MapFrom(x => x.Account.Role.RoleName))
                .ForMember(dst => dst.AccountId, src => src.MapFrom(x => x.AccountId))
                .ForMember(dst => dst.Status, src => src.MapFrom(x => x.Status))
                .ForMember(dst => dst.Gender, src => src.MapFrom(x => x.Gender))
                .ForMember(dst => dst.HomeAddress, src => src.MapFrom(x => x.HomeAddress))
                .ForMember(dst => dst.DateOfStart, src => src.MapFrom(x => x.StartDate.HasValue ? x.StartDate.Value.ToString("dd-MM-yyyy") : ""))
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
                .ForMember(dst => dst.DateOfBirth, src => src.MapFrom(x => x.DateOfBirth.HasValue ? x.DateOfBirth.Value.ToString("dd-MM-yyyy") : ""))
                .ForMember(dst => dst.Avatar, src => src.MapFrom(x => x.Account.AvatarUrl))
                .ForMember(dst => dst.Email, src => src.MapFrom(x => x.Account.Email));
                cfg.CreateMap<LabTesting, LabTestingDto>()
                .ForMember(dst => dst.LabTestName, src => src.MapFrom(x => x.LabTest.LabTestName))
                .ForMember(dst => dst.Price, src => src.MapFrom(x => x.LabTest.Price))
                .ForMember(dst => dst.LabTestingId, src => src.MapFrom(x => x.LabTestingId))
                .ForMember(dst => dst.LabTestId, src => src.MapFrom(x => x.LabTestId))
                .ForMember(dst => dst.AppointmentCode, src => src.MapFrom(x => x.SampleGetting.Appointment.AppointmentCode))
                .ForMember(dst => dst.SampleId, src => src.MapFrom(x => x.SampleGetting.SampleId))
                .ForMember(dst => dst.SampleGettingId, src => src.MapFrom(x => x.SampleGetting.SampleGettingId))
                .ForMember(dst => dst.SampleName, src => src.MapFrom(x => x.SampleGetting.Sample.SampleName))
                .ForMember(dst => dst.Status, src => src.MapFrom(x => x.Status))
                .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
                .ForMember(dst => dst.MachineSlot, src => src.MapFrom(x => x.MachineSlot))
                 .ForMember(dst => dst.PatientPhone, src => src.MapFrom(x => x.SampleGetting.Appointment.Patient.PhoneNumber))
                .ForMember(dst => dst.PatientName, src => src.MapFrom(x => x.SampleGetting.Appointment.Patient.FullName))
                 .ForMember(dst => dst.PDOB, src => src.MapFrom(x => x.SampleGetting.Appointment.Patient.DateOfBirth.HasValue ? x.SampleGetting.Appointment.Patient.DateOfBirth.Value.ToString("dd-MM-yyyy") : ""))
                .ForMember(dst => dst.GetApp, src => src.MapFrom(x => x.SampleGetting.Appointment.EnterTime))
                .ForMember(dst => dst.ReturnRe, src => src.MapFrom(x => x.SampleGetting.Appointment.ReturnTime))
               .ForMember(dst => dst.LabTestingIndexDtos, src => src.MapFrom(x => x.LabTestingIndexes));

                cfg.CreateMap<Slot, SlotDto>();

                cfg.CreateMap<Feedback, FeedbackDto>()
               .ForMember(dst => dst.FeedbackId, src => src.MapFrom(x => x.FeedbackId))
               .ForMember(dst => dst.EmployeeName, src => src.MapFrom(x => x.Employee.FullName))
               .ForMember(dst => dst.PatientName, src => src.MapFrom(x => x.Patient.FullName))
               .ForMember(dst => dst.Content, src => src.MapFrom(x => x.Content))
               .ForMember(dst => dst.ReceivedDateTime, src => src.MapFrom(x => x.ReceivedDateTime.HasValue ? (x.ReceivedDateTime.Value).ToString("dd-MM-yyyy") : ""))
               .ForMember(dst => dst.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
               .ForMember(dst => dst.Status, src => src.MapFrom(x => x.Status));

                cfg.CreateMap<FeedbackDto, Feedback>();

            });

        }

        /*
        private static string GetRoleName(string name)
        {
            if (name == "Y tá")
            {
                return "Y tá";
            }
            else if (name == "Bác sĩ")
            {
                return "Bác sĩ";
            }
            else if (name == "Bác sĩ")
            {
                return "Bác sĩ";
            }
            else if (name == "Quản kho")
            {
                return "Quản kho";
            }
            else if (name == "Quản lý")
            {
                return "Quản lý";
            }
            else if (name == "Kỹ thuật viên")
            {
                return "Kỹ thuật viên";
            }
            else if (name == "Tiếp tân")
            {
                return "Tiếp tân";
            }
            else
            {
                return "";
            }

        }/**/
    }
}