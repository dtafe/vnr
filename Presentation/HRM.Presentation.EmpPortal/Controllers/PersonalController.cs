using HRM.Business.Hr.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.EmpPortal.Models;
using HRM.Presentation.Payroll.Models;
using HRM.Presentation.Evaluation.Models;
using HRM.Presentation.Service;
using HRM.Business.Evaluation.Models;
using VnResource.Helper.Data;
using HRM.Business.Main.Domain;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.Hr.Models;
namespace HRM.Presentation.EmpPortal.Controllers
{
    public class PersonalController : BasePortalController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Paysips(string userLogin)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");
            var id = Session[SessionObjects.ProfileID];
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            var listModel = service.GetData<Hre_ProfileModelPortal>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();
            if (listModel != null)
            {
                Session["ProfileName"] = listModel.ProfileName;
                listModel.ActionStatus = status;
            }
            return GetOnlyView(listModel);

           
        }
        public ActionResult GetDataTemp([DataSourceRequest] DataSourceRequest request)
        {
            request.PageSize = 20;
            List<Sal_PayrollTableItemTempModel> lst = new List<Sal_PayrollTableItemTempModel>();
            Sal_PayrollTableItemTempModel model = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = "Số phép năm còn lại - tháng tính lương/Balance AL",
                Value = "6.67",
                Description1 = ""
            };
            lst.Add(model);
            Sal_PayrollTableItemTempModel model1 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = "Số ngày công - tháng tính lương/No of working days",
                Value = "26.00",
                Description1 = ""
            };
            lst.Add(model1);
            Sal_PayrollTableItemTempModel model2 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = "Số ngày phép còn lại đến cuối năm / Remain AL",
                Value = "9.00",
                Description1 = ""
            };
            lst.Add(model2);
            Sal_PayrollTableItemTempModel model3 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = "Công chuẩn / Standard Working Days",
                Value = "26.00",
                Description1 = ""
            };
            lst.Add(model3);
            Sal_PayrollTableItemTempModel model4 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = "Số ngày phép năm đã nghỉ từ đầu năm/ Used AL",
                Value = "5.00",
                Description1 = ""
            };
            lst.Add(model4);
            Sal_PayrollTableItemTempModel model5 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = "ALLOWANCE_COMMENT_ALL",
                Value = "",
                Description1 = ""
            };
            lst.Add(model5);
            Sal_PayrollTableItemTempModel model6 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = "Tổng lương/ Gross salary",
                Value = "4,979,102.16",
                Description1 = ""
            };
            lst.Add(model6);
            Sal_PayrollTableItemTempModel model7 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = " Lương cơ bản (Trước đc)/ Basic salary(before adj)",
                Value = "2,496,100.00",
                Description1 = ""
            };
            lst.Add(model7);

            Sal_PayrollTableItemTempModel model8 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = " Tổng lương thời gian / Working Salary",
                Value = "2,496,100.00",
                Description1 = ""
            };
            lst.Add(model8);

            Sal_PayrollTableItemTempModel model9 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = " Ngày công thực tế(Trước Đc)/Actual working(before)",
                Value = "26.00",
                Description1 = ""
            };
            lst.Add(model9);

            Sal_PayrollTableItemTempModel model10 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = "Tổng lương tăng ca/ Overtime amount",
                Value = "54,002.16",
                Description1 = ""
            };
            lst.Add(model10);
            Sal_PayrollTableItemTempModel model11 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = " Tăng ca ngày thường (Trước đc)/OT normal(before)",
                Value = "3.00",
                Description1 = ""
            };
            lst.Add(model11);

            Sal_PayrollTableItemTempModel model12 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = "Tổng khoản cộng/ Total",
                Value = "2,429,000.00",
                Description1 = ""
            };
            lst.Add(model12);

            Sal_PayrollTableItemTempModel model13 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = " Phụ cấp thâm niên/ Seniority allowance",
                Value = "100,000.00",
                Description1 = ""
            };
            lst.Add(model13);

            Sal_PayrollTableItemTempModel model14 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = " Phụ cấp hiện diện/ Attendance allowance",
                Value = "200,000.00",
                Description1 = ""
            };
            lst.Add(model14);

            Sal_PayrollTableItemTempModel model15 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = " Phụ cấp tay nghề/ Skill incentive",
                Value = "80,000.00",
                Description1 = ""
            };
            lst.Add(model15);
            Sal_PayrollTableItemTempModel model16 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = " Phụ cấp đi lại/ Transportation allowance",
                Value = "220,000.00",
                Description1 = ""
            };
            lst.Add(model16);

            Sal_PayrollTableItemTempModel model17 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = " Phụ cấp cơm/ Meal allowance",
                Value = "390,000.00",
                Description1 = ""
            };
            lst.Add(model17);

            Sal_PayrollTableItemTempModel model18 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = " Phụ cấp con nhỏ/ Child allowance",
                Value = "70,000.00",
                Description1 = ""
            };
            lst.Add(model18);

            Sal_PayrollTableItemTempModel model19 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = " Phụ cấp hiệu suất/ Efficiency incentive",
                Value = "1,369,000.00",
                Description1 = ""
            };
            lst.Add(model19);

            Sal_PayrollTableItemTempModel model20 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = "Tổng khoản trừ/ Total deduction",
                Value = "274,090.50",
                Description1 = ""
            };
            lst.Add(model20);

            Sal_PayrollTableItemTempModel model21 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = " Trừ phí công đoàn/ Trade Union",
                Value = "12,000.00",
                Description1 = ""
            };
            lst.Add(model21);

            Sal_PayrollTableItemTempModel model22 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = " BHXH_EMP (8%) / Social Ins",
                Value = "199,688.00",
                Description1 = ""
            };
            lst.Add(model22);

            Sal_PayrollTableItemTempModel model23 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = " BHYT_EMP (1.5%) / Health Ins",
                Value = "37,441.50",
                Description1 = ""
            };
            lst.Add(model23);

            Sal_PayrollTableItemTempModel model24 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = "  BHTN_EMP (1%) / Unemp Ins",
                Value = "24,961.00",
                Description1 = ""
            };
            lst.Add(model24);

            Sal_PayrollTableItemTempModel model25 = new Sal_PayrollTableItemTempModel()
            {
                ProfileName = "Thực nhận / NET INCOME",
                Value = "4,705,011.66",
                Description1 = ""
            };
            lst.Add(model25);
            for (int i = 0; i < lst.Count; i++)
            {
                lst[i].ID = i+1;
                
            }
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GeneralProfileDetail(string userLogin)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");

            var id = Session[SessionObjects.ProfileID];
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            var model = service.GetData<Hre_ProfileModelPortal>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();
            if (model != null)
            {
                Session["ProfileName"] = model.ProfileName;
                model.ActionStatus = status;
            }
            return GetOnlyView(model);
        }
        public ActionResult BasicInfo(string userLogin)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");

             var id = Session[SessionObjects.ProfileID];
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            var listModel = service.GetData<Hre_ProfileModelPortal>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();
            if (listModel != null)
            {
                Session["ProfileName"] = listModel.ProfileName;
                listModel.ActionStatus = status;
            }
            return GetOnlyView(listModel);
        }

        [HttpPost]
        public JsonResult CreateBasicInfo(Hre_ProfileModel model, string userLogin)
        {
            var id = Session[SessionObjects.ProfileID];
            var serv = new Hre_ProfileServices();
            string status = string.Empty;
            var profile = serv.GetData<Hre_ProfileModel>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();

            bool change = false;
            #region check change
            if (profile.ProfileName != model.ProfileName)
                change = true;
            if (profile.CodeAttendance != model.CodeAttendance)
                change = true;
            if (profile.CodeEmp != model.CodeEmp)
                change = true;
            if (profile.DateApplyAttendanceCode != model.DateApplyAttendanceCode)
                change = true;
            if (profile.WorkPlaceName != model.WorkPlaceName)
                change = true;
            if (profile.CodeTax != model.CodeTax)
                change = true;
            if (profile.DateOfIssuedTaxCode != model.DateOfIssuedTaxCode)
                change = true;
            if (profile.DateHire != model.DateHire)
                change = true;
            if (profile.DateEndProbation != model.DateEndProbation)
                change = true;
            if (profile.JobTitleID != model.JobTitleID)
                change = true;
            if (profile.PositionID != model.PositionID)
                change = true;
            if (profile.EmpTypeID != model.EmpTypeID)
                change = true;
            if (profile.PayrollGroupID != model.PayrollGroupID)
                change = true;
            if (profile.JobTitleID != model.JobTitleID)
                change = true;
            if (profile.PositionID != model.PositionID)
                change = true;
            if (profile.EmpTypeID != model.EmpTypeID)
                change = true;
            if (profile.PayrollGroupID != model.PayrollGroupID)
                change = true;
            if (profile.DateOfEffect != model.DateOfEffect)
                change = true;
            if (profile.LaborType != model.LaborType)
                change = true;
            if (profile.WorkPlaceID != model.WorkPlaceID)
                change = true;
            if (profile.SupervisorID != model.SupervisorID)
                change = true;
            if (profile.HighSupervisorID != model.HighSupervisorID)
                change = true;
            #endregion
            if (!change)
            {
                profile.ActionStatus = "Success";
                return Json(profile);
            }
            if (profile.ID == model.ID)
            {
                profile.ActionStatus = model.ActionStatus;
                profile.ProfileName = model.ProfileName;
                profile.CodeAttendance = model.CodeAttendance;
                profile.CodeEmp = model.CodeEmp;
                profile.DateApplyAttendanceCode = model.DateApplyAttendanceCode;
                profile.LocationCode = model.LocationCode;
                profile.WorkPlaceName = model.WorkPlaceName;
                profile.CodeTax = model.CodeTax;
                profile.DateOfIssuedTaxCode = model.DateOfIssuedTaxCode;
                profile.DateHire = model.DateHire;
                //profile.PromotionCondition = model.PromotionCondition;
                profile.DateEndProbation = model.DateEndProbation;
                profile.JobTitleID = model.JobTitleID;
                profile.PositionID = model.PositionID;
                profile.EmpTypeID = model.EmpTypeID;
                profile.PayrollGroupID = model.PayrollGroupID;
                profile.DateOfEffect = model.DateOfEffect;
                profile.LaborType = model.LaborType;
                profile.WorkPlaceID = model.WorkPlaceID;
                profile.SupervisorID = model.SupervisorID;
                profile.HighSupervisorID = model.HighSupervisorID;
            }

            var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Post(_Hrm_Hre_Service, "api/Hre_Profile/", profile);
            
            return Json(result);
        }
        [HttpPost]
        public JsonResult CreatePersonalInfo(Hre_ProfileModel model, string userLogin)
        {
            var id = Session[SessionObjects.ProfileID];
            var serv = new Hre_ProfileServices();
            string status = string.Empty;
            var profile = serv.GetData<Hre_ProfileModel>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();

            bool change = false;
            #region check change
            if (profile.NameEnglish != model.NameEnglish)
                change = true;
            if (profile.Gender != model.Gender)
                change = true;
            if (profile.DayOfBirth != model.DayOfBirth)
                change = true;
            if (profile.MonthOfBirth != model.MonthOfBirth)
                change = true;
            if (profile.YearOfBirth != model.YearOfBirth)
                change = true;
            if (profile.NationalityID != model.NationalityID)
                change = true;
            if (profile.PlaceOfBirth != model.PlaceOfBirth)
                change = true;
            if (profile.MarriageStatus != model.MarriageStatus)
                change = true;
            if (profile.IDNo != model.IDNo)
                change = true;
            if (profile.IDDateOfIssue != model.IDDateOfIssue)
                change = true;
            if (profile.IDPlaceOfIssue != model.IDPlaceOfIssue)
                change = true;
            if (profile.Origin != model.Origin)
                change = true;
            //if (profile.TypeOfVehicle != model.TypeOfVehicle)
            //    change = true;
            //if (profile.LockerName != model.LockerName)
            //    change = true;
            if (profile.EducationLevelID != model.EducationLevelID)
                change = true;
            if (profile.EthnicID != model.EthnicID)
                change = true;
            if (profile.Notes != model.Notes)
                change = true;
            if (profile.BloodType != model.BloodType)
                change = true;
            if (profile.Height != model.Height)
                change = true;
            if (profile.Weight != model.Weight)
                change = true;
            if (profile.PassportNo != model.PassportNo)
                change = true;
            if (profile.PassportPlaceOfIssue != model.PassportPlaceOfIssue)
                change = true;
            if (profile.PassportDateOfIssue != model.PassportDateOfIssue)
                change = true;
            if (profile.PassportDateOfExpiry != model.PassportDateOfExpiry)
                change = true;

           
            
            #endregion
            if (!change)
            {
                profile.ActionStatus = "Success";
                return Json(profile);
            }
            if (profile.ID == model.ID)
            {

                profile.ActionStatus = model.ActionStatus;
                profile.NameEnglish = model.NameEnglish;
                profile.Gender = model.Gender;
                profile.DayOfBirth = model.DayOfBirth;
                profile.MonthOfBirth = model.MonthOfBirth;
                profile.YearOfBirth = model.YearOfBirth;
                profile.NationalityID = model.NationalityID;
                profile.PlaceOfBirth = model.PlaceOfBirth;
                profile.MarriageStatus = model.MarriageStatus;
                profile.IDNo = model.IDNo;
                profile.IDDateOfIssue = model.IDDateOfIssue;
                profile.IDPlaceOfIssue = model.IDPlaceOfIssue;
                profile.Origin = model.Origin;
                profile.EducationLevelID = model.EducationLevelID;
                profile.EthnicID = model.EthnicID;
                profile.Notes = model.Notes;
                profile.BloodType = model.BloodType;
                profile.Height = model.Height;
                profile.Weight = model.Weight;
                profile.PassportNo = model.PassportNo;
                profile.PassportPlaceOfIssue = model.PassportPlaceOfIssue;
                profile.PassportDateOfIssue = model.PassportDateOfIssue;
                profile.PassportDateOfExpiry = model.PassportDateOfExpiry;

            
            }

            var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Post(_Hrm_Hre_Service, "api/Hre_Profile/", profile);
            return Json(result);
        }
        [HttpPost]
        public JsonResult CreateContactInfo(Hre_ProfileModel model, string userLogin)
        {
            var id = Session[SessionObjects.ProfileID];
            var serv = new Hre_ProfileServices();
            string status = string.Empty;
            var profile = serv.GetData<Hre_ProfileModel>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();

            bool change = false;
            #region check change
            if (profile.Email != model.Email)
                change = true;
            if (profile.Email2 != model.Email2)
                change = true;
            if (profile.Cellphone != model.Cellphone)
                change = true;
            if (profile.HomePhone != model.HomePhone)
                change = true;

            if (profile.TCountryID != model.TCountryID)
                change = true;
            if (profile.TDistrictID != model.TDistrictID)
                change = true;

            if (profile.TProvinceID != model.TProvinceID)
                change = true;

            if (profile.TAVillageID != model.TAVillageID)
                change = true;

            if (profile.TAddress != model.TAddress)
                change = true;



            if (profile.PCountryID != model.PCountryID)
                change = true;
            if (profile.PDistrictID != model.PDistrictID)
                change = true;

            if (profile.PProvinceID != model.PProvinceID)
                change = true;

            if (profile.VillageID != model.VillageID)
                change = true;

            if (profile.PAddress != model.PAddress)
                change = true;

            if (profile.AddressEmergency != model.AddressEmergency)
                change = true;
            
            #endregion

            if (!change)
            {
                profile.ActionStatus = "Success";
                return Json(profile);
            }
            if (profile.ID == model.ID)
            {
                profile.ActionStatus = model.ActionStatus;
                profile.Email = model.Email;
                profile.Email2 = model.Email2;
                profile.Cellphone = model.Cellphone;
                profile.HomePhone = model.HomePhone;
                profile.TCountryID = model.TCountryID;
                profile.TDistrictID = model.TDistrictID;
                profile.TProvinceID = model.TProvinceID;
                profile.TAVillageID = model.TAVillageID;
                profile.TAddress = model.TAddress;
                profile.PCountryID = model.PCountryID;
                profile.PDistrictID = model.PDistrictID;
                profile.PProvinceID = model.PProvinceID;
                profile.VillageID = model.VillageID;
                profile.PAddress = model.PAddress;
                profile.AddressEmergency = model.AddressEmergency;
            }

            var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Post(_Hrm_Hre_Service, "api/Hre_Profile/", profile);
            return Json(result);
        }
        public ActionResult ContactInfo(string userLogin)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");
             var id = Session[SessionObjects.ProfileID];
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            var listModel = service.GetData<Hre_ProfileModelPortal>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();
            if (listModel != null)
            {
                Session["ProfileName"] = listModel.ProfileName;
                listModel.ActionStatus = status;
            }
            return GetOnlyView(listModel);
        }
        public ActionResult PersonalInfo(string userLogin)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");
             var id = Session[SessionObjects.ProfileID];
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            var listModel = service.GetData<Hre_ProfileModelPortal>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();
            if (listModel != null)
            {
                Session["ProfileName"] = listModel.ProfileName;
                listModel.ActionStatus = status;
            }
            return GetOnlyView(listModel);
        }
        public ActionResult RelativeInfo(string userLogin)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");
             var id = Session[SessionObjects.ProfileID];            
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            var listModel = service.GetData<Hre_ProfileModelPortal>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();
            if (listModel != null)
            {
                Session["ProfileName"] = listModel.ProfileName;
                listModel.ActionStatus = status;
            }
            return GetOnlyView(listModel);
        }
        public ActionResult InsuranceInfo(string userLogin)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");
             var id = Session[SessionObjects.ProfileID];
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            var listModel = service.GetData<Hre_ProfileModelPortal>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin,ref status).FirstOrDefault();
            if (listModel != null)
            {
                Session["ProfileName"] = listModel.ProfileName;
                listModel.ActionStatus = status;
            }
            return GetOnlyView(listModel);
        }

        public ActionResult Contact(string userLogin)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");
             var id = Session[SessionObjects.ProfileID];
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            var listModel = service.GetData<Hre_ProfileModelPortal>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin,ref status).FirstOrDefault();
            if (listModel != null)
            {
                Session["ProfileName"] = listModel.ProfileName;
                listModel.ActionStatus = status;
            }
            return GetOnlyView(listModel);
        }

        public ActionResult WorkHistory(string userLogin)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");
             var id = Session[SessionObjects.ProfileID];
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            var listModel = service.GetData<Hre_ProfileModelPortal>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin,ref status).FirstOrDefault();
            if (listModel != null)
            {
                Session["ProfileName"] = listModel.ProfileName;
                listModel.ActionStatus = status;
            }
            return GetOnlyView(listModel);
        }
        public ActionResult HDTJob(string userLogin)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");
             var id = Session[SessionObjects.ProfileID];
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            var listModel = service.GetData<Hre_ProfileModelPortal>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin,ref status).FirstOrDefault();
            if (listModel != null)
            {
                Session["ProfileName"] = listModel.ProfileName;
                listModel.ActionStatus = status;
            }
            return GetOnlyView(listModel);
        }

        public ActionResult TAMScanLog(string userLogin)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");
             var id = Session[SessionObjects.ProfileID];  
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            var listModel = service.GetData<Hre_ProfileModelPortal>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin,ref status).FirstOrDefault();
            if (listModel != null)
            {
                Session["ProfileName"] = listModel.ProfileName;
                listModel.ActionStatus = status;
            }
            return GetOnlyView(listModel);
        }
        public ActionResult WorkDay(string userLogin)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal"); 
             var id = Session[SessionObjects.ProfileID];
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            var listModel = service.GetData<Hre_ProfileModelPortal>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin,ref status).FirstOrDefault();
            if (listModel != null)
            {
                Session["ProfileName"] = listModel.ProfileName;
                listModel.ActionStatus = status;
            }
            return GetOnlyView(listModel);
        }
        public ActionResult Roster(string userLogin)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");
             var id = Session[SessionObjects.ProfileID];
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            var listModel = service.GetData<Hre_ProfileModelPortal>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin,ref status).FirstOrDefault();
            if (listModel != null)
            {
                Session["ProfileName"] = listModel.ProfileName;
                listModel.ActionStatus = status;
            }
            return GetOnlyView(listModel);
        }
        public ActionResult LeaveDay(string userLogin)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");
             var id = Session[SessionObjects.ProfileID];            
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            var listModel = service.GetData<Hre_ProfileModelPortal>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin,ref status).FirstOrDefault();
            if (listModel != null)
            {
                Session["ProfileName"] = listModel.ProfileName;
                listModel.ActionStatus = status;
            }
            return GetOnlyView(listModel);
        }
        public ActionResult Overtime(string userLogin)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");
             var id = Session[SessionObjects.ProfileID];
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            var listModel = service.GetData<Hre_ProfileModelPortal>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin,ref status).FirstOrDefault();
            if (listModel != null)
            {
                Session["ProfileName"] = listModel.ProfileName;
                listModel.ActionStatus = status;
            }
            return GetOnlyView(listModel);
        }
        public ActionResult ProfileAttendanceDetail(string userLogin)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");
             var id = Session[SessionObjects.ProfileID];
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            var model = service.GetData<Hre_ProfileModelPortal>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin,ref status).FirstOrDefault();
            if (model != null)
            {
                Session["ProfileName"] = model.ProfileName;
                model.ActionStatus = status;
            }
            return GetOnlyView(model);
        }
        public ActionResult SaleEvaluation(string userLogin)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");
             var id = Session[SessionObjects.ProfileID];
           
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            List<object> lstObjSaleEvaluation = new List<object>();
            lstObjSaleEvaluation.Add(id);
       //     lstObjSaleEvaluation.Add(1);
     //       lstObjSaleEvaluation.Add(1000000000);

            var listModel = service.GetData<Eva_SaleEvaluationModel>(lstObjSaleEvaluation, ConstantSql.hrm_eva_sp_get_SaleEvaluationByProId, userLogin,ref status);
            if (listModel != null && listModel.Count > 0)
            {
                var model = listModel.FirstOrDefault();
                model.ActionStatus = status;
                return GetOnlyView(model);
            }
            return View();
        }

        public ActionResult CreateSaleEvaluation()
        {

            return GetView();
        }

        public ActionResult EditSaleEvaluation(Guid id)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");
            string status = string.Empty;
            var model = new Eva_SaleEvaluationModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Eva_SaleEvaluationEntity>(id, ConstantSql.hrm_eva_sp_get_SaleEvaluationByID, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Eva_SaleEvaluationModel>();
            }
            model.ActionStatus = status;
            return GetOnlyView(model);
        }

        public ActionResult SalaryInfo(string userLogin)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");
             var id = Session[SessionObjects.ProfileID];
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            Sal_SalaryInformationModel listModel1 = null;
            var listModel = service.GetData<Sal_SalaryInformationModel>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_sal_sp_get_Sal_SalaryInfomationByProfileId, userLogin,ref status);
            if (listModel != null && listModel.Count > 0)
            {
                listModel1 = listModel.FirstOrDefault();
                listModel1.ActionStatus = status;
            }
            return GetOnlyView(listModel1);


        }
        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            if (selectedIds.Count >= 0)
            {
                BaseService service = new BaseService();
                var status = string.Empty;
                for (int i = 0; i < selectedIds.Count; i++)
                {
                    status = service.Remove<Eva_SaleEvaluationEntity>(selectedIds[i]);
                    if (status.StartsWith("Error"))
                    {
                        break;
                    }
                }
                return Json(status, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }
        public ActionResult BasicSalary(string userLogin)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");
             var id = Session[SessionObjects.ProfileID];
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            var listModel = service.GetData<Hre_ProfileModelPortal>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin,ref status).FirstOrDefault();
            if (listModel != null)
            {
                Session["ProfileName"] = listModel.ProfileName;
                listModel.ActionStatus = status;
            }
            return GetOnlyView(listModel);
        }
        //public ActionResult Paysips()
        //{
        //    if (!CheckedLogin)
        //    {
        //        return CheckLogin();
        //    }
        //     var id = Session[SessionObjects.ProfileID];
        //    var service = new Hre_ProfileServices();
        //    string status = string.Empty;
        //    var listModel = service.GetData<Hre_ProfileModelPortal>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, ref status).FirstOrDefault();
        //    if (listModel != null)
        //    {
        //        listModel.ActionStatus = status;
        //    }
        //    return View(listModel);
        //}


        public string SaveSaleEvalution(Eva_SaleEvaluationModel model)
        {

            #region Tính tỉ lệ = thực đạt / chỉ tiêu

            if (model != null)
            {
                model.ResultNumber = model.ResultNumber.HasValue ? model.ResultNumber.Value : 0;
                model.TagetNumber = model.TagetNumber.HasValue ? model.TagetNumber.Value : 0;
                if (model.TagetNumber > 0)
                {
                    model.ResultPercent = (model.ResultNumber / model.TagetNumber) * 100;
                }
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
             service.UpdateOrCreate<Eva_SaleEvaluationEntity, Eva_SaleEvaluationModel>(model);
            //IdLeaveday = model.ID;
             if (model.ID != Guid.Empty)
             {
                 return model.ID.ToString();
             }
             return model.ActionStatus;
        }




	}
}