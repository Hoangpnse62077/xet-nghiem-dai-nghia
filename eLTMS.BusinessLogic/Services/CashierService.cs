using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using eLTMS.DataAccess.Repositories;
using eLTMS.Models.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.BusinessLogic.Services
{
    public interface ICashiertService
    {
        bool Add(AppointmentAddDto data, List<LabTestDto> labTests);
        bool ChangeIsPaid(int sampleGettingId);
        //List<Appointment> GetAllAppointment();
        List<AppointmentGetBySampleDto> GetAllBySample(string search, DateTime date, int sampleId);
        int CheckAndDeleteBlood(DateTime dateTime);
        int CheckAndDeleteUrine(DateTime dateTime);
        int CheckAndDeleteCell(DateTime dateTime);
        int CheckAndDeleteMucus(DateTime dateTime);
        int CheckAndDeletePhan(DateTime dateTime);
        PriceListDto GetPrice(int sampleGettingId);
        List<Token> GetAllTokens();

    }

    class CashierService : ICashiertService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public CashierService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        //ten ten
        public string CreateAppReturnCode(Appointment appointment)
        {
            var appointmentRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            try
            {
                // Convert AppointmentDto to Appointment
                var now = DateTime.Now;
                var sDate = now.ToString("yyyy-MM-dd");
                var count = appointmentRepo.CountByDate(sDate);
                var code = sDate + "-" + count;
                appointment.AppointmentCode = code;
                appointment.Status = "NEW";
                // Create
                appointmentRepo.Create(appointment);
                var result = this.UnitOfWork.SaveChanges();
                if (result.Any())
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return appointment.AppointmentCode;
        }

        //ten ten
        public bool ChangeIsPaid(int sampleGettingId)
        {
            try
            {
                var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
                var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);

                var sampleGetting = sgRepo.GetFirst(p => p.SampleGettingId == sampleGettingId);
                sampleGetting.IsGot = false;
                sampleGetting.IsPaid = true;
                sampleGetting.Status = "WAITING";
                sgRepo.Update(sampleGetting);

                var app = appRepo.GetById(sampleGetting.AppointmentId);
                app.Status = "RECEPTDONE";
                appRepo.Update(app);

                UnitOfWork.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        //ten ten
        public bool Add(AppointmentAddDto data, List<LabTestDto> labTests)
        {
            var rs = false;
            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var accRepo = RepositoryHelper.GetRepository<IAccountRepository>(UnitOfWork);
            var paRepo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var slotRepo = RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            try
            {
                //tao account 
                var account = accRepo.GetAll().FirstOrDefault(p => p.PhoneNumber == data.Phone);
                if (account == null)
                {
                    accRepo.Create(new Account
                    {
                        PhoneNumber = data.Phone,
                        FullName = data.Name,
                        IsDeleted = false,
                    });
                }
                UnitOfWork.SaveChanges();
                //tao patient
                var accId = accRepo.GetAll().FirstOrDefault(p => p.PhoneNumber == data.Phone).AccountId;
                Patient patient = null;
                patient = new Patient();

                patient.AccountId = accId;
                patient.Gender = data.Gender;
                //IdentityCardNumber = data.IdentityCardNumber,
                patient.DateOfBirth = data.DateOfBirth;
                patient.HomeAddress = data.Address;
                patient.FullName = data.Name;
                patient.PhoneNumber = data.Phone;
                patient.IsDeleted = false;
                paRepo.Create(patient);

                UnitOfWork.SaveChanges();
                patient.PatientCode = "BN" + patient.PatientId;

                //tao cuoc hen
                //var paId = paRepo.GetFirst(p => p.PhoneNumber == data.Phone).PatientId;//lấy Id bệnh nhân  dựa vào phone
                var appCode = CreateAppReturnCode(new Appointment //tạo app
                {
                    PatientId = patient.PatientId,
                    Status = "NEW",
                    IsOnline = false,
                    //Date = DateTime.Now.Date,
                    IsDeleted = false
                });
                UnitOfWork.SaveChanges();

                //xep lich hen cho tung loai xet nghiem
                var appId = appRepo.GetFirst(p => p.AppointmentCode == appCode).AppointmentId;//
                if (data.Mau == true)
                {
                    //tim slot va ban trong
                    var slotAndTable = GetEmptyTableAndSlot(1);
                    //neu con ban va slot trong thi moi tao lich hen
                    if (slotAndTable != null)
                    {
                        var sg = new SampleGetting //xong tạo sg ở đây
                        {
                            GettingDate = DateTime.Now.Date,
                            AppointmentId = appId,
                            SampleId = 1,
                            SlotId = slotAndTable.slotId,
                            TableId = slotAndTable.tableId,
                            IsDeleted = false
                        };
                        sg.LabTestings = new List<LabTesting>();
                        foreach(var lt in labTests)
                        {
                            if (lt.SampleId==1) // 1: Mau
                            {
                                var labTesting = new LabTesting();
                                labTesting.LabTestId = lt.LabTestId;
                                sg.LabTestings.Add(labTesting);
                            }
                        }
                        //tao lich hen loai xet nghiem mau 
                        sgRepo.Create(sg);
                        //var ID = sgRepo.GetFirst(p => p.SampleGettingId == sgId).SampleGettingId;
                        rs = true;
                    }
                }
                if (data.NuocTieu == true)
                {
                    //tim slot va ban trong
                    var slotAndTable = GetEmptyTableAndSlot(1);
                    //neu con ban va slot trong thi moi tao lich hen
                    if (slotAndTable != null)
                    {
                        var sg = new SampleGetting //xong tạo sg ở đây
                        {
                            GettingDate = DateTime.Now.Date,
                            AppointmentId = appId,
                            SampleId = 2,
                            SlotId = slotAndTable.slotId,
                            TableId = slotAndTable.tableId,
                            IsDeleted = false
                        };
                        sg.LabTestings = new List<LabTesting>();
                        foreach (var lt in labTests)
                        {
                            if (lt.SampleId == 2) 
                            {
                                var labTesting = new LabTesting();
                                labTesting.LabTestId = lt.LabTestId;
                                sg.LabTestings.Add(labTesting);
                            }
                        }
                        //tao lich hen loai xet nghiem mau 
                        sgRepo.Create(sg);
                        //var ID = sgRepo.GetFirst(p => p.SampleGettingId == sgId).SampleGettingId;
                        rs = true;
                    }
                }
                if (data.Phan == true)
                {
                    var slotAndTable = GetEmptyTableAndSlot(2);
                    if (slotAndTable != null)
                    {
                        var sg = new SampleGetting //xong tạo sg ở đây
                        {
                            GettingDate = DateTime.Now.Date,
                            AppointmentId = appId,
                            SampleId = 4,
                            SlotId = slotAndTable.slotId,
                            TableId = slotAndTable.tableId,
                            IsDeleted = false
                        };
                        sg.LabTestings = new List<LabTesting>();
                        foreach (var lt in labTests)
                        {
                            if (lt.SampleId == 4)
                            {
                                var labTesting = new LabTesting();
                                labTesting.LabTestId = lt.LabTestId;
                                sg.LabTestings.Add(labTesting);
                            }
                        }
                        //tao lich hen loai xet nghiem phan 
                        sgRepo.Create(sg);
                        rs = true;
                    }
                }
                if (data.TeBaoHoc == true)
                {
                    //tim slot va ban trong
                    var slotAndTable = GetEmptyTableAndSlot(3);
                    //neu con ban va slot trong thi moi tao lich hen
                    if (slotAndTable != null)
                    {
                        var sg = new SampleGetting //xong tạo sg ở đây
                        {
                            GettingDate = DateTime.Now.Date,
                            AppointmentId = appId,
                            SampleId = 3,
                            SlotId = slotAndTable.slotId,
                            TableId = slotAndTable.tableId,
                            IsDeleted = false
                        };
                        sg.LabTestings = new List<LabTesting>();
                        foreach (var lt in labTests)
                        {
                            if (lt.SampleId == 3) // 1: Mau
                            {
                                var labTesting = new LabTesting();
                                labTesting.LabTestId = lt.LabTestId;
                                sg.LabTestings.Add(labTesting);
                            }
                        }
                        //tao lich hen loai xet nghiem mau 
                        sgRepo.Create(sg);
                        //var ID = sgRepo.GetFirst(p => p.SampleGettingId == sgId).SampleGettingId;
                        rs = true;
                    }
                }
                if (data.Dich == true)
                {
                    var slotAndTable = GetEmptyTableAndSlot(4);
                    if (slotAndTable != null)
                    {
                        var sg = new SampleGetting //xong tạo sg ở đây
                        {
                            GettingDate = DateTime.Now.Date,
                            AppointmentId = appId,
                            SampleId = 5,
                            SlotId = slotAndTable.slotId,
                            TableId = slotAndTable.tableId,
                            IsDeleted = false
                        };
                        sg.LabTestings = new List<LabTesting>();
                        foreach (var lt in labTests)
                        {
                            if (lt.SampleId == 5) 
                            {
                                var labTesting = new LabTesting();
                                labTesting.LabTestId = lt.LabTestId;
                                sg.LabTestings.Add(labTesting);
                            }
                        }
                        //tao lich hen loai xet nghiem dich 
                        sgRepo.Create(sg);
                        rs = true;
                    }
                }

                UnitOfWork.SaveChanges();
            }
            catch (Exception ex) { return false; }
            return rs;
        }

        public class TableAndSlotId
        {
            public int tableId { get; set; }
            public int slotId { get; set; }
        }

        public TableAndSlotId GetEmptyTableAndSlot(int sampleGroupId)
        {
            var slotRepo = RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var tabRepo = this.RepositoryHelper.GetRepository<ITableRepository>(this.UnitOfWork);
            var sgRepo = this.RepositoryHelper.GetRepository<ISampleGettingRepository>(this.UnitOfWork);

            //lay gio hien tai
            var nowTime = DateTime.Now;
            var nowTimeToSecond = nowTime.Hour * 3600 + nowTime.Minute * 60 + nowTime.Second;
            //lay tat ca slot
            var slots = slotRepo.GetAll().Where(p => p.StartTime >= nowTimeToSecond
                            && p.SampleGroupId==sampleGroupId)
                            .OrderBy(p => p.StartTime).ToList();
            var tables = tabRepo.GetAll()
                            .Where(p => p.SampleGroupId == sampleGroupId)
                            .OrderBy(p => p.TableId).ToList();
            foreach (var slot in slots)
            {
                var sgs = sgRepo.GetAll().Where(p => p.SlotId == slot.SlotId && p.GettingDate == DateTime.Now.Date).ToList();
                //mau va nuoc tieu
                if (sampleGroupId == 1)
                {
                    if (sgs.Count < tables.Count)
                    {
                            return new TableAndSlotId
                            {
                                //neu co 9 cuoc hen ma co 10 cai ban thi lay cai ban stt la 9(tuc la cai ban thu 10) 
                                tableId = tables[sgs.Count].TableId,
                                slotId = slot.SlotId
                            };
                    }
                }
                //Phân
                else if (sampleGroupId == 2)
                {
                    if (sgs.Count < tables.Count) 
                    {
                            return new TableAndSlotId
                            {
                                tableId = tables[sgs.Count].TableId,
                                slotId = slot.SlotId
                            };
                    }
                }
                //Tế Bào
                else if (sampleGroupId == 3)
                {
                    if (sgs.Count < tables.Count)
                    {
                            return new TableAndSlotId
                            {
                                tableId = tables[sgs.Count].TableId,
                                slotId = slot.SlotId
                            };
                    }
                }
                //Dịch
                else if (sampleGroupId == 4)
                {
                    if (sgs.Count < tables.Count)
                    {
                            return new TableAndSlotId
                            {
                                tableId = tables[sgs.Count].TableId,
                                slotId = slot.SlotId
                            };
                    }
                }
            }
            //return null co nghia la het slot het ban
            return null;
        }

        public List<AppointmentGetBySampleDto> GetAllBySample(string search, DateTime date, int sampleId)
        {
            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var paRepo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var slotRepo = RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var spRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);
            var tableRepo = this.RepositoryHelper.GetRepository<ITableRepository>(this.UnitOfWork);

            var apps = appRepo.GetAll().Where(p => p.IsDeleted != true);
            var pas = paRepo.GetAll().Where(p => p.IsDeleted != true);
            var sgs = sgRepo.GetAll().Where(p => p.SampleId == sampleId && p.IsDeleted != true
            && p.GettingDate == date /*&& p.Status != "NURSEDONE"*/);

            var sps = spRepo.GetAll().Where(p => p.IsDeleted != true);
            var slots = slotRepo.GetAll();
            var tables = tableRepo.GetAll();
            var appPas = apps.Join(pas, p => p.PatientId, c => c.PatientId, (p, c) => new
            {
                app = p,
                pa = c
            });
            var spSgs = sgs.Join(sps, p => p.SampleId, c => c.SampleId, (p, c) => new
            {
                sg = p,
                sp = c
            });
            var spSgSlots = spSgs.Join(slots, p => p.sg.SlotId, c => c.SlotId, (p, c) => new
            {
                spSg = p,
                slot = c
            });
            var count = 1;
            var result = spSgSlots.Join(appPas, p => p.spSg.sg.AppointmentId,
                c => c.app.AppointmentId, (p, c) => new AppointmentGetBySampleDto
                {
                    StartTime = TimeSpan.FromSeconds(p.slot.StartTime.Value).ToString(@"hh\:mm"),//xuất ra string giờ : phút
                    SampleName = p.spSg.sp.SampleName,
                    DateOfBirth = c.pa.DateOfBirth != null ? c.pa.DateOfBirth.Value.ToShortDateString() : "",
                    OrderNumber = count++,
                    Phone = c.pa.PhoneNumber,
                    Address = c.pa.HomeAddress,
                    PatientName = c.pa.FullName,
                    Date = p.spSg.sg.GettingDate.Value.ToShortDateString(),
                    //Table = p.table.TableName,
                    SampleGettingId = p.spSg.sg.SampleGettingId,
                    SampleId = p.spSg.sg.SampleId,
                    IsPaid = p.spSg.sg.IsPaid
                }).GroupBy(a => a.PatientName).Select(g => g.First()).ToList();
            result = result.Where(p => p.PatientName.ToString().Contains(search)
            || p.Phone.ToString().Contains(search)).ToList()
            .OrderBy(a => a.StartTime).ToList();
            return result;
        }

        public int CheckAndDeleteBlood(DateTime dateTime)
        {
            var dateNow = dateTime.Date;
            //lay so giay trong slot
            var hours = dateTime.Hour;
            var minutes = dateTime.Minute;
            var seconds = dateTime.Second;
            var time = hours * 3600 + minutes * 60 + seconds;

            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var paRepo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var slotRepo = RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var spRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);

            try
            {
                var pas = paRepo.GetAll().ToList();
                var apps = appRepo.GetAll().Where(p => p.IsOnline == true).ToList();
                var slots = slotRepo.GetAll().Where(p => p.StartTime == time).ToList();
                var sgs = sgRepo.GetAll().Where(p => p.GettingDate == dateNow && p.IsPaid != true && (p.SampleId == 1)).ToList();
                var appPas = apps.Join(pas, p => p.PatientId, c => c.PatientId, (p, c) => new
                {
                    app = p,
                    pa = c
                }).ToList();
                var sgSlots = sgs.Join(slots, p => p.SlotId, c => c.SlotId, (p, c) => new
                {
                    sg = p,
                    slot = c
                }).ToList();
                var result = sgSlots.Join(appPas, p => p.sg.AppointmentId,
                c => c.app.AppointmentId, (p, c) => new
                {
                    sgId = p.sg.SampleGettingId
                }).ToList();
                //count de đếm số cuộc hẹn được xóa
                var count = 0;
                if (result.Count > 0)
                {

                    foreach (var item in result)
                    {
                        //lay sg ra dua vao id
                        var sg = sgRepo.GetById(item.sgId);
                        //doi thuoc tinh isdelete
                        sg.IsDeleted = true;
                        count++;
                    }
                    //save xuong db
                    UnitOfWork.SaveChanges();
                }
                return count;

            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int CheckAndDeleteUrine(DateTime dateTime)
        {
            var dateNow = dateTime.Date;
            //lay so giay trong slot
            var hours = dateTime.Hour;
            var minutes = dateTime.Minute;
            var seconds = dateTime.Second;
            var time = hours * 3600 + minutes * 60 + seconds /*- 3 * 60*/;

            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var paRepo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var slotRepo = RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var spRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);

            try
            {
                var pas = paRepo.GetAll().ToList();
                var apps = appRepo.GetAll().Where(p => p.IsOnline == true).ToList();
                var slots = slotRepo.GetAll().Where(p => p.StartTime == time).ToList();
                var sgs = sgRepo.GetAll().Where(p => p.GettingDate == dateNow && p.IsPaid != true && (p.SampleId == 2)).ToList();
                var appPas = apps.Join(pas, p => p.PatientId, c => c.PatientId, (p, c) => new
                {
                    app = p,
                    pa = c
                }).ToList();
                var sgSlots = sgs.Join(slots, p => p.SlotId, c => c.SlotId, (p, c) => new
                {
                    sg = p,
                    slot = c
                }).ToList();
                var result = sgSlots.Join(appPas, p => p.sg.AppointmentId,
                c => c.app.AppointmentId, (p, c) => new
                {
                    sgId = p.sg.SampleGettingId
                }).ToList();
                //count de đếm số cuộc hẹn được xóa
                var count = 0;
                if (result.Count > 0)
                {

                    foreach (var item in result)
                    {
                        //lay sg ra dua vao id
                        var sg = sgRepo.GetById(item.sgId);
                        //doi thuoc tinh isdelete
                        sg.IsDeleted = true;
                        count++;
                    }
                    //save xuong db
                    UnitOfWork.SaveChanges();
                }
                return count;

            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int CheckAndDeleteCell(DateTime dateTime)
        {
            var dateNow = dateTime.Date;
            var hours = dateTime.Hour;
            var minutes = dateTime.Minute;
            var seconds = dateTime.Second;
            var time = hours * 3600 + minutes * 60 + seconds;

            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var paRepo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var slotRepo = RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var spRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);

            try
            {
                var pas = paRepo.GetAll().ToList();
                var apps = appRepo.GetAll().Where(p => p.IsOnline == true).ToList();
                var slots = slotRepo.GetAll().Where(p => p.StartTime == time).ToList();
                var sgs = sgRepo.GetAll().Where(p => p.GettingDate == dateNow && p.IsPaid != true && (p.SampleId == 3)).ToList();
                var appPas = apps.Join(pas, p => p.PatientId, c => c.PatientId, (p, c) => new
                {
                    app = p,
                    pa = c
                }).ToList();
                var sgSlots = sgs.Join(slots, p => p.SlotId, c => c.SlotId, (p, c) => new
                {
                    sg = p,
                    slot = c
                }).ToList();
                var result = sgSlots.Join(appPas, p => p.sg.AppointmentId,
                c => c.app.AppointmentId, (p, c) => new
                {
                    sgId = p.sg.SampleGettingId
                }).ToList();
                var count = 0;
                if (result.Count > 0)
                {

                    foreach (var item in result)
                    {
                        var sg = sgRepo.GetById(item.sgId);
                        sg.IsDeleted = true;
                        count++;
                    }
                    UnitOfWork.SaveChanges();
                }
                return count;

            }
            catch (Exception e)
            {
                return 0;
            }

        }

        public int CheckAndDeleteMucus(DateTime dateTime)
        {
            var dateNow = dateTime.Date;
            var hours = dateTime.Hour;
            var minutes = dateTime.Minute;
            var seconds = dateTime.Second;
            var time = hours * 3600 + minutes * 60 + seconds;

            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var paRepo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var slotRepo = RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var spRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);

            try
            {
                var pas = paRepo.GetAll().ToList();
                var apps = appRepo.GetAll().Where(p => p.IsOnline == true).ToList();
                var slots = slotRepo.GetAll().Where(p => p.StartTime == time).ToList();
                var sgs = sgRepo.GetAll().Where(p => p.GettingDate == dateNow && p.IsPaid != true && (p.SampleId == 5)).ToList();
                var appPas = apps.Join(pas, p => p.PatientId, c => c.PatientId, (p, c) => new
                {
                    app = p,
                    pa = c
                }).ToList();
                var sgSlots = sgs.Join(slots, p => p.SlotId, c => c.SlotId, (p, c) => new
                {
                    sg = p,
                    slot = c
                }).ToList();
                var result = sgSlots.Join(appPas, p => p.sg.AppointmentId,
                c => c.app.AppointmentId, (p, c) => new
                {
                    sgId = p.sg.SampleGettingId
                }).ToList();
                var count = 0;
                if (result.Count > 0)
                {

                    foreach (var item in result)
                    {
                        var sg = sgRepo.GetById(item.sgId);
                        sg.IsDeleted = true;
                        count++;
                    }
                    UnitOfWork.SaveChanges();
                }
                return count;

            }
            catch (Exception e)
            {
                return 0;
            }

        }

        public int CheckAndDeletePhan(DateTime dateTime)
        {
            var dateNow = dateTime.Date;
            var hours = dateTime.Hour;
            var minutes = dateTime.Minute;
            var seconds = dateTime.Second;
            var time = hours * 3600 + minutes * 60 + seconds;

            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var paRepo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var slotRepo = RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var spRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);

            try
            {
                var pas = paRepo.GetAll().ToList();
                var apps = appRepo.GetAll().Where(p => p.IsOnline == true).ToList();
                var slots = slotRepo.GetAll().Where(p => p.StartTime == time).ToList();
                var sgs = sgRepo.GetAll().Where(p => p.GettingDate == dateNow && p.IsPaid != true && (p.SampleId == 4)).ToList();
                var appPas = apps.Join(pas, p => p.PatientId, c => c.PatientId, (p, c) => new
                {
                    app = p,
                    pa = c
                }).ToList();
                var sgSlots = sgs.Join(slots, p => p.SlotId, c => c.SlotId, (p, c) => new
                {
                    sg = p,
                    slot = c
                }).ToList();
                var result = sgSlots.Join(appPas, p => p.sg.AppointmentId,
                c => c.app.AppointmentId, (p, c) => new
                {
                    sgId = p.sg.SampleGettingId
                }).ToList();
                var count = 0;
                if (result.Count > 0)
                {

                    foreach (var item in result)
                    {
                        var sg = sgRepo.GetById(item.sgId);
                        sg.IsDeleted = true;
                        count++;
                    }
                    UnitOfWork.SaveChanges();
                }
                return count;

            }
            catch (Exception e)
            {
                return 0;
            }

        }

        // Author: DucBM
        public List<Token> GetAllTokens()
        {
            var repo = this.RepositoryHelper.GetRepository<ITokenRepository>(UnitOfWork);
            var tokens = repo.GetAll();
            return tokens;
        }

        public PriceListDto GetPrice(int sampleGettingId)
        {
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var labTestRepo = RepositoryHelper.GetRepository<ILabTestRepository>(UnitOfWork);
            var labTestingRepo = this.RepositoryHelper.GetRepository<ILabTestingRepository>(this.UnitOfWork);

            var sgs = sgRepo.GetAll().Where(p => p.SampleGettingId == sampleGettingId && p.IsPaid != true);
            var lts = labTestingRepo.GetAll().Where(p => p.IsDeleted != true);
            var labs = labTestRepo.GetAll().Where(p => p.IsDeleted != true);

            var ltsSgs = sgs.Join(lts, p => p.SampleGettingId, c => c.SampleGettingId, (p, c) => new
            {
                sg = p,
                lt = c
            });
            var count = 1;
            var result = ltsSgs.Join(labs, p => p.lt.LabTestId,
                c => c.LabTestId, (p, c) => new PriceListItemDto
                {
                    OrderNumber = count++,
                    LabtestName = c.LabTestName,
                    Price = c.Price,
                }).ToList();
            int? total = 0;
            foreach (var i in result)
            {
                total += i.Price;
            }
            var rs = new PriceListDto
            {
                PriceListItemDto = result,
                TotalPrice = total, 
            };
            return rs;
        }
    }
}
    