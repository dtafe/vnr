using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Presentation.HrmSystem.Models;
using HRM.Presentation.Service;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;
using HRM.Business.Main.Domain;
using HRM.Presentation.Hr.Models;
using HRM.Presentation.Category.Models;
using HRM.Business.Hr.Domain;
using HRM.Business.Hr.Models;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class DashboardController : ApiController
    {
        #region MyRegion
        private string userLogin = string.Empty;
        public string UserLogin
        {
            get
            {
                if (Request.Headers != null)
                {
                    var headerValues = Request.Headers.GetValues(HeaderObject.UserLogin);
                    userLogin = headerValues.FirstOrDefault();
                }
                return userLogin;
            }
        }
        #endregion
        string status = string.Empty;
        public DashboardInformationModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new DashboardInformationModel();
            ActionService service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<DashboardInformationEntity>(id, ConstantSql.hrm_sys_sp_get_DashBoardById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<DashboardInformationModel>();
            }
            //model.ActionStatus = status;
            return model;
        }


        public DashboardAlertsModel Get() 
        {
            var profileServices = new Hre_ProfileServices();
            Sys_AttOvertimePermitConfigServices sysServices = new Sys_AttOvertimePermitConfigServices();
            DashboardAlertsModel dashboardAlertModel = new DashboardAlertsModel();
            ActionService service = new ActionService(UserLogin);
            BaseService baseServices = new BaseService();
            string status = string.Empty;


            Boolean ischeck = profileServices.IsShowExConByConResult();
               
           #region lay cau hinh hien thi hop dong sap den han
                //lay cau hinh hien thi hop dong sap den han
                dashboardAlertModel.ShowValue1 = sysServices.GetConfigValue<bool>(AppConfig.HRM_HRE_CONTRACT_ALERT_EXPRIDAY);
                if (dashboardAlertModel.ShowValue1 != null && dashboardAlertModel.ShowValue1 == true)
                {
                    var ShowAfterDate1 = service.GetData<Sys_AllSettingEntity>("HRM_HRE_CONTRACT_ALERT_EXPRIDAY_VALUEAFTE", ConstantSql.hrm_sys_sp_get_AllSettingByKey, ref status).FirstOrDefault();
                    var ShowBeforDate1 = service.GetData<Sys_AllSettingEntity>("HRM_HRE_CONTRACT_ALERT_EXPRIDAY_VALUEBEFOR", ConstantSql.hrm_sys_sp_get_AllSettingByKey, ref status).FirstOrDefault();
                    DateTime? dateFrom = null;
                    DateTime? dateTo = null;
                    var count = 0;
                    string _status = EnumDropDown.Status.E_APPROVED.ToString();

                    if (ischeck == false)
                    {
                        dateTo = ShowAfterDate1.Value1 != null ? DateTime.Now.AddDays(Convert.ToDouble(ShowAfterDate1.Value1)) : DateTime.Now.AddDays(0);
                        dateFrom = ShowBeforDate1.Value1 != null ? DateTime.Now.AddDays(-Convert.ToDouble(ShowBeforDate1.Value1)) : DateTime.Now.AddDays(0);
                    }

                    #region lstObjExpiryContract
                    List<object> lstObjExpiryContract = new List<object>();
                    lstObjExpiryContract.Add(null);
                    lstObjExpiryContract.Add(_status);
                    lstObjExpiryContract.Add(dateFrom);
                    lstObjExpiryContract.Add(dateTo);
                    lstObjExpiryContract.Add(null);
                    lstObjExpiryContract.Add(null);
                    lstObjExpiryContract.Add(null);
                    lstObjExpiryContract.Add(null);
                    lstObjExpiryContract.Add(null);
                    lstObjExpiryContract.Add(null);
                    lstObjExpiryContract.Add(null);
                    lstObjExpiryContract.Add(1);
                    lstObjExpiryContract.Add(int.MaxValue - 1);
                    #endregion

                    #region lstObjContractType
                    var lstObjContractType = new List<object>();
                    lstObjContractType.Add(null);
                    lstObjContractType.Add(null);
                    lstObjContractType.Add(null);
                    lstObjContractType.Add(null);
                    lstObjContractType.Add(1);
                    lstObjContractType.Add(int.MaxValue - 1);
                    #endregion

                    var lstContractType = baseServices.GetData<CatContractTypeModel>(lstObjContractType, ConstantSql.hrm_cat_sp_get_ContractType, UserLogin, ref status);

                    var lstExpiryContract = baseServices.GetData<Hre_ContractModel>(lstObjExpiryContract, ConstantSql.hrm_hr_sp_get_RptExpireContract, UserLogin, ref status).Where(s => s.StatusEvaluation != WorkdayStatus.E_APPROVED.ToString()).ToList();


                    foreach (var item in lstExpiryContract)
                    {
                        var dateCheck = DateTime.Now;
                        var contractTypeEntity = lstContractType.Where(s => item.ContractTypeID == s.ID).FirstOrDefault();
                        if (ischeck == true)
                        {
                        
                                if (contractTypeEntity != null && contractTypeEntity.ExpiryContractLoop != null)
                                {
                                    var dateExpiry = dateCheck.AddDays(contractTypeEntity.ExpiryContractLoop.Value);

                                    if (contractTypeEntity != null && contractTypeEntity.ExpiryContractLoop != null)
                                    {
                                        if (item.DateExtend != null && item.DateExtend <= dateExpiry)
                                        {
                                            count++;
                                        }
                                        if (item.DateExtend == null && item.DateEnd != null && item.DateEnd.Value <= dateExpiry)
                                        {
                                            count++;
                                        }
                                    }
                                }
                            
                        }
                        else
                        {
                                count++;
                        }
                    }
                    dashboardAlertModel.sumContract = count;
                }
                
                #endregion

           #region lay cau hinh hien thi sinh nhat trong thang

                dashboardAlertModel.ShowValue5 = sysServices.GetConfigValue<bool>(AppConfig.HRM_HRE_PROFILE_ALERT_BIRTHDAY_NOWDAY);
                if (dashboardAlertModel.ShowValue5 != null && dashboardAlertModel.ShowValue5 == true)
                {
                    var Birthfrom = sysServices.GetConfigValue<double>(AppConfig.HRM_HRE_PROFILE_ALERT_BIRTHDAY_NOWDAY_VALUEBEFOR);
                    var Birthto = sysServices.GetConfigValue<double>(AppConfig.HRM_HRE_PROFILE_ALERT_BIRTHDAY_NOWDAY_VALUEAFTER);
                    var _DateFrom = DateTime.Now.AddDays(-Birthfrom); 
                    var _DateTo = DateTime.Now.AddDays(+Birthto);
                    List<object> lstObjBirthDay = new List<object>();
                    lstObjBirthDay.Add(new DateTime(DateTime.Now.Year,1,1));
                    lstObjBirthDay.Add(new DateTime(DateTime.Now.Year,12,31));
                    lstObjBirthDay.Add(null);
                    lstObjBirthDay.Add(null);
                    lstObjBirthDay.Add(null);
                    lstObjBirthDay.Add(null);
                    var _lstBirthday = baseServices.GetData<DashboardAlertsModel>(lstObjBirthDay, ConstantSql.hrm_hr_sp_get_RptBirthday, UserLogin, ref status);
                    if(_DateFrom !=_DateTo)
                    { 
                        var _kc = Convert.ToInt16((_DateTo - _DateFrom).ToString().Substring(0,(_DateTo - _DateFrom).ToString().IndexOf('.')));
                        if (_kc >= 365)
                        {
                            dashboardAlertModel.sumBirthDay = _lstBirthday.Count();
                        }
                        else
                        {
                            if (_DateFrom.Year < _DateTo.Year)
                            {
                                DateTime _dtf = (new DateTime(_DateTo.Year, 1, 1).AddMilliseconds(-1));
                                DateTime _dtt = (new DateTime(_DateTo.Year, 1, 1));
                                dashboardAlertModel.sumBirthDay = _lstBirthday.Where(s => (s.DateOfBirth >= _DateFrom && s.DateOfBirth <= _dtf) || (s.DateOfBirth >= _dtt && s.DateOfBirth <= _DateTo)).ToList().Count();
                          
                            }
                            else
                            {
                                dashboardAlertModel.sumBirthDay = _lstBirthday.Where(s => (s.DateOfBirth >= _DateFrom && s.DateOfBirth <= _DateTo)).ToList().Count();
                            }
                        }
                    }
                }
                #endregion

           #region lay cau hinh hien thi phu luc hop dong den han
                bool? _IsShowContractAppendixExpriday = sysServices.GetConfigValue<bool>(AppConfig.HRM_HRE_CONTRACT_ALERT_APPENDDIXEXPRIDAY);

            if (_IsShowContractAppendixExpriday != null && _IsShowContractAppendixExpriday == true)
            {
                //lay cau hinh ngay hien thi
                int? _DayShowContractAppendixExpridayFrom = sysServices.GetConfigValue<int?>(AppConfig.HRM_HRE_CONTRACT_ALERT_DAYAPPENDDIXEXPRIDAYFROM);
                int? _DayShowContractAppendixExpridayTo = sysServices.GetConfigValue<int?>(AppConfig.HRM_HRE_CONTRACT_ALERT_DAYAPPENDDIXEXPRIDAYTO);
                if (_DayShowContractAppendixExpridayFrom != null && _DayShowContractAppendixExpridayTo != null)
                {
                    dashboardAlertModel.IsShowContractAppendixExpriday = true;
                    DateTime _dateFrom = DateTime.Now.AddDays(-_DayShowContractAppendixExpridayTo.Value);
                    DateTime _dateTo = DateTime.Now.AddDays(_DayShowContractAppendixExpridayFrom.Value);
                    var ObjAppendixContract = new List<object>();
                    ObjAppendixContract.AddRange(new object[7]);
                    ObjAppendixContract[3] = _dateFrom;
                    ObjAppendixContract[4] = _dateTo;
                    ObjAppendixContract[5] = 1;
                    ObjAppendixContract[6] = int.MaxValue - 1;
                    var sumAppendixContract = baseServices.GetData<Hre_AppendixContractModel>(ObjAppendixContract, ConstantSql.hrm_hr_sp_get_AppendixExpiredContract, UserLogin, ref status);
                    dashboardAlertModel.sumAppendixExpiryContract = sumAppendixContract != null ? sumAppendixContract.Count : 0;
                }
            }
            #endregion



           
            //lay cau hinh hien thi sinh nhat trong thang
            //dashboardAlertModel.ShowValue5 = sysServices.GetConfigValue<bool>(AppConfig.HRM_HRE_PROFILE_ALERT_BIRTHDAY_NOWDAY);
            //if (dashboardAlertModel.ShowValue5 != null && dashboardAlertModel.ShowValue5 == true)
            //{
            //    dashboardAlertModel.sumBirthDay = baseServices.GetData<DashboardAlertsModel>(lstObjBirthDay, ConstantSql.hrm_hr_sp_get_RptBirthday, UserLogin, ref status).Select(s => s.ID).ToList().Count;
            //}
           
            List<object> lstObjProfileProbation = new List<object>();
            lstObjProfileProbation.AddRange(new object[22]);
            lstObjProfileProbation[20] = 1;
            lstObjProfileProbation[21] = int.MaxValue - 1;
            //Hre_ProfileProbationSearchModel modelsearch = new Hre_ProfileProbationSearchModel();
            //modelsearch.DateFrom = DateTime.Now;
            //modelsearch.DateTo = dateTo;
           

            //lay cau hinh hien canh bao trang chu Hết Hạn Thử Việc Trong Tháng hay khong
            dashboardAlertModel.ShowValue2 = sysServices.GetConfigValue<bool>(AppConfig.HRM_HRE_PROFILE_ALERT_PROBATION_EXPRIDAY);
            if(dashboardAlertModel.ShowValue2!=null && dashboardAlertModel.ShowValue2==true)
            {
                int ProbationValueBefore = sysServices.GetConfigValue<int>(AppConfig.HRM_HRE_PROFILE_ALERT_PROBATION_EXPRIDAY_VALUEBEFOR);
                int ProbationValueAfter = sysServices.GetConfigValue<int>(AppConfig.HRM_HRE_PROFILE_ALERT_PROBATION_EXPRIDAY_VALUEAFTER);
                //modelsearch.DateHireFrom = DateTime.Now.Date.AddMonths(-1);
                //modelsearch.DateHireTo = DateTime.Now.Date.AddMonths(1);
                lstObjProfileProbation[10] = DateTime.Now.Date.AddDays(-ProbationValueBefore);
                lstObjProfileProbation[11] = DateTime.Now.Date.AddDays(ProbationValueAfter);
                var sum = baseServices.GetData<DashboardAlertsModel>(lstObjProfileProbation, ConstantSql.hrm_hr_sp_get_ProbationProfile,UserLogin, ref status);
                dashboardAlertModel.sumProbation = sum != null ? sum.Count : 0;
            }
         

            //lay cau hinh hien thi luong co ban cho duyet
            dashboardAlertModel.IsShowBasicSalaryWaitingAprove = sysServices.GetConfigValue<bool>(AppConfig.HRM_SAL_BASICSALARY_ALERT_WAITINGAPROVE);

           
            //lay cau hinh hien thi hop dong cho duyet
            dashboardAlertModel.IsShowContractWaitingAprove = sysServices.GetConfigValue<bool>(AppConfig.HRM_HRE_CONTRACT_ALERT_WAITINGAPROVE);
            if(dashboardAlertModel.IsShowContractWaitingAprove !=null && dashboardAlertModel.IsShowContractWaitingAprove==true)
            {
                //sum dh cho duyet
                List<object> paraContractWaiting = new List<object>();
                paraContractWaiting.AddRange(new object[20]);
                paraContractWaiting[18] = 1;
                paraContractWaiting[19] = int.MaxValue - 1;
                var lstContractWaiting = baseServices.GetData<Hre_ContractModel>(paraContractWaiting, ConstantSql.hrm_hr_sp_get_ContractWaiting,UserLogin, ref status);
                if (lstContractWaiting != null)
                    dashboardAlertModel.sumContractWaiting = lstContractWaiting.Count;
            }
            //canh bao so ngay tam hoan den han
            //get so ngay cau hinh
            //string keyDaySuspenseExpiry= AppConfig.HRM_HRE_CONFIG_DAYSUSPENSEEXPIRY.ToString();
            //var objDaySuspenseExpiry=baseServices.GetData<Sys_AllSettingEntity>(keyDaySuspenseExpiry,ConstantSql.hrm_sys_sp_get_AllSettingByKey,ref status).FirstOrDefault();
          
            #region lay cau hinh hien thi nv sap den ngay tam hoan

            bool? _IsShowDaySuspenseExpiry=sysServices.GetConfigValue<bool?>(AppConfig.HRM_HRE_PROFILE_ALERT_DAYSUSPENSEEXPIRY);

            if (_IsShowDaySuspenseExpiry != null && _IsShowDaySuspenseExpiry == true)
            {
            

                //lay cau hinh ngay hien thi
                int? _DayShowDaySuspenseExpiryFrom = sysServices.GetConfigValue<int?>(AppConfig.HRM_HRE_PROFILE_ALERT_DAYSUSPENSEEXPIRYFROM);
                int? _DayShowDaySuspenseExpiryTo = sysServices.GetConfigValue<int?>(AppConfig.HRM_HRE_PROFILE_ALERT_DAYSUSPENSEEXPIRYTO);
                if (_DayShowDaySuspenseExpiryFrom != null && _DayShowDaySuspenseExpiryTo!=null)
                {
                    //var valueDaySuspenseExpiry = sysServices.GetConfigValue<int>(AppConfig.HRM_HRE_CONFIG_DAYSUSPENSEEXPIRY);
                    //if (valueDaySuspenseExpiry >= 0)
                    //{
                    //DateTime _dateFromTemp = DateTime.Now.AddDays(-valueDaySuspenseExpiry);
                    //DateTime _dateFrom = new DateTime(_dateFromTemp.Year, _dateFromTemp.Month, _dateFromTemp.Day, 0, 0, 0);
                    //DateTime _dateTo = DateTime.Now;
                    dashboardAlertModel.IsShowDaySuspenseExpiry = true;
                    string _status = EnumDropDown.StopWorkStatus.E_APPROVED.ToString();
                    DateTime _dateFrom = DateTime.Now.AddDays(-_DayShowDaySuspenseExpiryTo.Value);
                    DateTime _dateTo = DateTime.Now.AddDays(_DayShowDaySuspenseExpiryFrom.Value);
                    List<object> paraSuspense = new List<object>();
                    paraSuspense.AddRange(new object[13]);
                    paraSuspense[5] = _dateFrom;
                    paraSuspense[6] = _dateTo;
                    paraSuspense[8] = _status;
                    paraSuspense[11] = 1;
                    paraSuspense[12] = int.MaxValue - 1;
                    var lstSuspense = baseServices.GetData<Hre_StopWorkingModel>(paraSuspense, ConstantSql.hrm_hr_sp_get_Suspense, UserLogin, ref status);
                    if (lstSuspense != null)
                        dashboardAlertModel.sumDaySuspenseExpiry = lstSuspense.Count;

                    // }
                }
            }
            #endregion
      
            //canh bao nv sap den ngay nghi viec

            #region lay cau hinh hien thi nv sap den ngay nghi viec
            bool? _IsShowDayStopWorking=sysServices.GetConfigValue<bool>(AppConfig.HRM_HRE_PROFILE_ALERT_DAYSTOPWORKING);
            if (_IsShowDayStopWorking != null && _IsShowDayStopWorking == true)
            {
                int? _DayShowDayStopWorkingFrom=sysServices.GetConfigValue<int?>(AppConfig.HRM_HRE_PROFILE_ALERT_DAYSTOPWORKINGFROM);
                int? _DayShowDayStopWorkingTo=sysServices.GetConfigValue<int?>(AppConfig.HRM_HRE_PROFILE_ALERT_DAYSTOPWORKINGTO);
                if( _DayShowDayStopWorkingFrom!= null && _DayShowDayStopWorkingTo != null)
                {
                    dashboardAlertModel.IsShowDayStopWorking = true;
                    //get cau hinh
                    //var valueDaytopworkingExpiry = sysServices.GetConfigValue<int>(AppConfig.HRM_HRE_CONFIG_DAYSTOPWORKINGEXPIRY);
                    //if (valueDaytopworkingExpiry > 0)
                    //{
                  
                        //DateTime _dateFromTemp = DateTime.Now.AddDays(-valueDaytopworkingExpiry);
                        //DateTime _dateFrom = new DateTime(_dateFromTemp.Year, _dateFromTemp.Month, _dateFromTemp.Day, 0, 0, 0);
                        //DateTime _dateTo = DateTime.Now;
                    DateTime _dateFrom = DateTime.Now.AddDays(-_DayShowDayStopWorkingTo.Value);
                    DateTime _dateTo = DateTime.Now.AddDays(_DayShowDayStopWorkingFrom.Value);
                    string _status = EnumDropDown.StopWorkStatus.E_APPROVED.ToString();
                    List<object> paraStopWorking = new List<object>();
                    paraStopWorking.AddRange(new object[17]);
                    paraStopWorking[0] = _status;
                    paraStopWorking[6] = _dateFrom;
                    paraStopWorking[7] = _dateTo;
                    paraStopWorking[15] = 1;
                    paraStopWorking[16] = int.MaxValue - 1;
                    var lstStopWorking = baseServices.GetData<Hre_StopWorkingModel>(paraStopWorking, ConstantSql.hrm_hr_sp_get_StopWorking, UserLogin, ref status);
                    if (lstStopWorking != null)
                        dashboardAlertModel.sumDayStopWorking = lstStopWorking.Count;
                //}
                }
            }
            #endregion

            #region lay cau hinh hien thi nv sap vao lam lai
            bool? _IsShowDayComeBackExpiry=sysServices.GetConfigValue<bool>(AppConfig.HRM_HRE_PROFILE_ALERT_DAYCOMEBACKEXPIRY);
            if (_IsShowDayComeBackExpiry != null && _IsShowDayComeBackExpiry == true)
            {
                //lay cau hinh ngay hien thi
                int? _DayShowDayComeBackExpiryFrom = sysServices.GetConfigValue<int?>(AppConfig.HRM_HRE_PROFILE_ALERT_DAYCOMEBACKEXPIRYFROM);
                int? _DayShowDayComeBackExpiryTo = sysServices.GetConfigValue<int?>(AppConfig.HRM_HRE_PROFILE_ALERT_DAYCOMEBACKEXPIRYTO);
                if (_DayShowDayComeBackExpiryFrom != null && _DayShowDayComeBackExpiryTo!=null)
                {
                    dashboardAlertModel.IsShowDayComeBackExpiry = true;

                    //var valueDayComebackExpiry = sysServices.GetConfigValue<int>(AppConfig.HRM_HRE_CONFIG_DAYCOMEBACKEXPIRY);
                    //if (valueDayComebackExpiry > 0)
                    //{
                        //DateTime _dateFromTemp = DateTime.Now.AddDays(-valueDayComebackExpiry);
                        //DateTime _dateFrom = new DateTime(_dateFromTemp.Year, _dateFromTemp.Month, _dateFromTemp.Day, 0, 0, 0);
                        //DateTime _dateTo = DateTime.Now;

                    string _status = EnumDropDown.StopWorkStatus.E_APPROVED.ToString();
                    DateTime _dateFrom=DateTime.Now.AddDays(-_DayShowDayComeBackExpiryTo.Value);
                    DateTime _dateTo=DateTime.Now.AddDays(_DayShowDayComeBackExpiryFrom.Value);
                    List<object> paraComback = new List<object>();
                    paraComback.AddRange(new object[14]);
                    paraComback[0] = _status;
                    paraComback[6] = _dateFrom;
                    paraComback[7] = _dateTo;
                    paraComback[12] = 1;
                    paraComback[13] = int.MaxValue - 1;
                    var lstStopWorking = baseServices.GetData<Hre_StopWorkingModel>(paraComback, ConstantSql.hrm_hr_sp_get_RegisterComback,UserLogin, ref status).ToList();
                    if (lstStopWorking != null)
                        dashboardAlertModel.sumDayComeBackExpiry = lstStopWorking.Count;
               // }
                }
            }
            #endregion

            //lay cau hinh hien thi nv la, HDT cho duyet
            dashboardAlertModel.IsShowHDTJobWaitingApproved = sysServices.GetConfigValue<bool>(AppConfig.HRM_HRE_PROFILE_ALERT_HDTJOBWAINTINGAPPROVED);
            if(dashboardAlertModel.IsShowHDTJobWaitingApproved!=null&&dashboardAlertModel.IsShowHDTJobWaitingApproved==true)
            {
                //sum nv vao hdt cho duyet
                List<object> paraHDTJobWaitingApproved = new List<object>();
                paraHDTJobWaitingApproved.AddRange(new object[14]);
                paraHDTJobWaitingApproved[12] = 1;
                paraHDTJobWaitingApproved[13] = int.MaxValue - 1;
                var lstHDTJobWaitingApproved = baseServices.GetData<Hre_ContractModel>(paraHDTJobWaitingApproved, ConstantSql.hrm_hr_sp_get_HDTJobWaiting,UserLogin, ref status).ToList();
                if (lstHDTJobWaitingApproved != null)
                    dashboardAlertModel.sumHDTJobWaitingApproved = lstHDTJobWaitingApproved.Count;
            }
            //lay cau hinh hien thi nguoi cho danh gia
            dashboardAlertModel.IsShowEvalutionWaiting = sysServices.GetConfigValue<bool>(AppConfig.HRM_EVA_PROFILE_ALERT_EVALUTIONWAITING);
            // Giấy Phép LĐ Đến Hạn
            dashboardAlertModel.IsShowExpiryWorkPermit = sysServices.GetConfigValue<bool>(AppConfig.PAPRlABORPHEPSLED);
            if (dashboardAlertModel.IsShowExpiryWorkPermit == true)
            {
                int ValueBefore = sysServices.GetConfigValue<int>(AppConfig.PAPRlABORPHEPSLED_VALUEBEFOR);
                int ValueAfter = sysServices.GetConfigValue<int>(AppConfig.PAPRlABORPHEPSLED_VALUEAFTER);

                List<object> listObj = new List<object>();
                listObj.Add(DateTime.Now.AddDays(-ValueBefore));
                listObj.Add(DateTime.Now.AddDays(ValueAfter));
                listObj.Add(1);
                listObj.Add(int.MaxValue - 1);

                status = string.Empty;
                var result = baseServices.GetData<Hre_ProfileWorkPermitExpireModel>(listObj, ConstantSql.hrm_hr_sp_get_ProfileByWorkPermitExpiredDate,UserLogin, ref status);
                dashboardAlertModel.sumExpiryWorkPermit = result != null ? result.Count : 0;
            }

            //lấy số nhân viên nghỉ việc mà chưa được quyết toán theo cấu hình
            Sys_AttOvertimePermitConfigServices Sys_Services = new Sys_AttOvertimePermitConfigServices();
            bool IsShowSETTLEMENT = Sys_Services.GetConfigValue<bool>(AppConfig.SAL_SETTLEMENT);
            if (IsShowSETTLEMENT)
            {
                dashboardAlertModel.IsShowSumProfileQuitNotSettlement = true;
                double ValueBeforeSettlement = sysServices.GetConfigValue<double>(AppConfig.SAL_SETTLEMENT_VALUEBEFOR);
                double ValueAfterSettlement = sysServices.GetConfigValue<double>(AppConfig.SAL_SETTLEMENT_VALUEAFTER);
                var DateSettlementFrom = DateTime.Now.AddDays(-ValueBeforeSettlement);
                var DateSettlementTo = DateTime.Now.AddDays(+ValueAfterSettlement);
                List<object> ListMode = new List<object>();
                ListMode.AddRange(new object[19]);
                ListMode[8] = DateSettlementFrom;
                ListMode[9] = DateSettlementTo;
                ListMode[17] = 1;
                ListMode[18] = Int32.MaxValue - 1;
                List<Hre_ProfileEntity> ListProfileQuit = baseServices.GetData<Hre_ProfileEntity>(ListMode, ConstantSql.hrm_hr_sp_get_ProfileQuit,UserLogin, ref status)
                    .Where(m => m.IsSettlement==null ||  m.IsSettlement == false).ToList();
                //int SumProfileQuitResult = 0;
                //foreach (var i in ListProfileQuit)
                //{
                //    if (i.DateQuit != null && DateTime.Now.Date.Subtract((DateTime)i.DateQuit.Value.Date).TotalDays >= ValueBeforeSettlement)
                //    {
                //        SumProfileQuitResult++;
                //    }
                //}
                //dashboardAlertModel.SumProfileQuitNotSettlement = SumProfileQuitResult;
                dashboardAlertModel.SumProfileQuitNotSettlement = ListProfileQuit.Count();
            }
            
            return dashboardAlertModel;
        }
        /// <summary>
        /// Lưu dữ liệu xuống database, với tham số đầu vào là Model tương ứng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public EmployeeModel Put(EmployeeModel model)
        //{
        //    //Chuyển model thành Model tương dứng dưới database
        //    var modelEmp = new Sys_UserInfoEntity()
        //    {
        //        UserLogin = model.LoginName,
        //        Code = model.Code,
        //        IsCheckLDAP = true,
        //        LDAPDatasource="1"
        //    };
        //    var service = new EmployeeServices();
        //    if (model.ID!= Guid.Empty)
        //    {
        //        modelEmp.ID = model.ID;
        //        service.EditEmployees(modelEmp);
        //    }
        //    else
        //    {
        //       service.Add<Sys_UserInfoEntity>(modelEmp);
        //    }
        //    return model;
        //}

    }
}
