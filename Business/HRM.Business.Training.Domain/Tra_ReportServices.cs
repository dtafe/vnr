using HRM.Business.Main.Domain;
using HRM.Business.Payroll.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Data.Entity.Repositories;
using HRM.Data.Attendance.Data.Sql.Repositories;
using HRM.Business.Category.Domain;
using VnResource.Helper.Data;
using HRM.Infrastructure.Utilities.Helper;
using HRM.Business.Training.Models;
using HRM.Business.Category.Models;
using HRM.Business.Hr.Domain;
using HRM.Business.Hr.Models;
using HRM.Business.Attendance.Models;
using HRM.Business.Payroll.Domain;


namespace HRM.Business.Training.Domain
{
    public class Tra_ReportServices : BaseService
    {
        #region BC Học Viên Đủ Điều Kiện Học

        public DataTable CreateReportTraineeJoinCourseSchema()
        {
            DataTable tb = new DataTable("Tra_ReportTraineeJoinCourseEntity");
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.ID);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.TraineeName);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.IDNo);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.OrgStructureName);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.E_UNIT);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.E_DIVISION);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.E_DEPARTMENT);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.E_TEAM);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.E_SECTION);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.Seniority);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.JobTitleName);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.PositionName);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.CourseName);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.ClassName);

            return tb;
        }
        public DataTable GetReportTraineeJoinCourse(Guid[] rankIds, string[] courseIds, string orderNumber, bool IsCreateTemplate, DateTime? dateSeniory, string UserLogin)
        {

            DataTable table = CreateReportTraineeJoinCourseSchema();
            if (IsCreateTemplate)
            {
                return table;
            }
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;

                var dateCheck = DateTime.Now;
                //var dateSeniotyConfig = "31/03/N";
                // var strDateSeniorty = dateSeniotyConfig.Split('/');
                var dateCheckConfig = DateTime.Now;

                //if (dateCheckConfig.Day <= int.Parse(strDateSeniorty[0]) && dateCheckConfig.Month <= int.Parse(strDateSeniorty[1]))
                //{
                //    dateCheckConfig = new DateTime(dateCheckConfig.Year - 1, dateCheckConfig.Month, dateCheckConfig.Day);
                //}
                if (dateSeniory != null)
                {
                    dateCheckConfig = dateSeniory.Value;
                }

                var profileServices = new Hre_ProfileServices();
                var objProfile = new List<object>();
                objProfile.AddRange(new object[17]);
                objProfile[2] = orderNumber;
                objProfile[15] = 1;
                objProfile[16] = int.MaxValue - 1;
                //objProfile.Add(orderNumber);
                //objProfile.Add(null);
                //objProfile.Add(null);
                var lstProfile = profileServices.GetData<Hre_ProfileEntity>(objProfile, ConstantSql.hrm_hr_sp_get_ProfileAll, UserLogin, ref status).Where(s => s.DateQuit == null).ToList();
                //var lstProfile = profileServices.GetData<Hre_ProfileEntity>(objProfile, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();
                var lstProfileID = lstProfile.Select(s => s.ID).ToList();

                var orgsService = new Cat_OrgStructureServices();
                var objOrg = new List<object>();
                objOrg.AddRange(new object[5]);
                objOrg[3] = 1;
                objOrg[4] = int.MaxValue - 1;
                var lstallorgs = orgsService.GetData<Cat_OrgStructureEntity>(objOrg, ConstantSql.hrm_cat_sp_get_OrgStructure, UserLogin, ref status).ToList();

                var courseServices = new Tra_CourseServices();
                var objCourse = new List<object>();
                objCourse.AddRange(new object[11]);
                objCourse[9] = 1;
                objCourse[10] = int.MaxValue - 1;
                var lstCourse = courseServices.GetData<Tra_CourseEntity>(objCourse, ConstantSql.hrm_tra_sp_get_Course, UserLogin, ref status).ToList();
                if (courseIds != null)
                {
                    lstCourse = lstCourse.Where(s => courseIds.Contains(s.Code)).ToList();
                }

                var jobTitleServices = new Cat_JobTitleServices();
                var objJobtitle = new List<object>();
                objJobtitle.Add(null);
                objJobtitle.Add(null);
                objJobtitle.Add(null);
                objJobtitle.Add(1);
                objJobtitle.Add(int.MaxValue - 1);
                var lstJobtitle = jobTitleServices.GetData<Cat_JobTitleEntity>(objJobtitle, ConstantSql.hrm_cat_sp_get_JobTitle, UserLogin, ref status).ToList();

                var classServices = new Tra_ClassServices();
                var objClass = new List<object>();
                objClass.Add(null);
                objClass.Add(null);
                objClass.Add(null);
                objClass.Add(null);
                objClass.Add(null);
                objClass.Add(null);
                objClass.Add(null);
                objClass.Add(null);
                objClass.Add(1);
                objClass.Add(int.MaxValue - 1);
                var lstClass = classServices.GetData<Tra_ClassEntity>(objClass, ConstantSql.hrm_tra_sp_get_Class, UserLogin, ref status).ToList();

                var salaryClassServices = new Cat_SalaryClassServices();
                var objSalaryClass = new List<object>();
                objSalaryClass.Add(null);
                objSalaryClass.Add(1);
                objSalaryClass.Add(int.MaxValue - 1);
                var lstSalaryClass = salaryClassServices.GetData<Cat_SalaryClassEntity>(objSalaryClass, ConstantSql.hrm_cat_sp_get_SalaryClass, UserLogin, ref status).ToList();
                if (rankIds != null)
                {
                    lstSalaryClass = lstSalaryClass.Where(s => rankIds.Contains(s.ID)).ToList();
                }

                var rankServices = new Cat_SalaryRankServices();
                var objRank = new List<object>();
                objRank.Add(null);
                objRank.Add(null);
                objRank.Add(1);
                objRank.Add(int.MaxValue - 1);
                var lstRank = rankServices.GetData<Cat_SalaryRankEntity>(objRank, ConstantSql.hrm_cat_sp_get_SalaryRank, UserLogin, ref status).ToList();

                var traineeServices = new Tra_TraineeServices();
                var objTrainee = new List<object>();
                objTrainee.AddRange(new object[15]);
                objTrainee[13] = 1;
                objTrainee[14] = int.MaxValue - 1;
                var lstTrainee = rankServices.GetData<Tra_TraineeEntity>(objTrainee, ConstantSql.hrm_tra_sp_get_Trainee, UserLogin, ref status).ToList();

                var workingHistoryServices = new Hre_WorkHistoryServices();
                var objWorking = new List<object>();
                objWorking.AddRange(new object[17]);
                objWorking[15] = 1;
                objWorking[16] = int.MaxValue - 1;
                var lstWorking = workingHistoryServices.GetData<Hre_WorkHistoryEntity>(objWorking, ConstantSql.hrm_hr_sp_get_WorkHistory, UserLogin, ref status).ToList();


                var stopWorkingServices = new Hre_StopWorkingServices();
                var objStopWorking = new List<object>();
                objStopWorking.AddRange(new object[17]);
                objStopWorking[15] = 1;
                objStopWorking[16] = int.MaxValue - 1;
                var lstStopWorking = stopWorkingServices.GetData<Hre_StopWorkingEntity>(objStopWorking, ConstantSql.hrm_hr_sp_get_StopWorking, UserLogin, ref status).ToList();

                var traineeCertificateServices = new Tra_TraineeCertificateServices();
                var objTraineeCertificate = new List<object>();
                objTraineeCertificate.Add(1);
                objTraineeCertificate.Add(int.MaxValue - 1);
                var lstTraineeCertificate = traineeCertificateServices.GetData<Tra_TraineeCertificateEntity>(objTraineeCertificate, ConstantSql.hrm_tra_sp_get_TraineeCertificate, UserLogin, ref status).ToList();
                var lstTraineeIDFromTraineeCertificate = lstTraineeCertificate.Select(s => s.TraineeID).ToList();

                var requirmentDetailServices = new Tra_RequirementTrainDetailServices();
                var objRequirementDetail = new List<object>();
                objRequirementDetail.Add(null);
                objRequirementDetail.Add(1);
                objRequirementDetail.Add(int.MaxValue - 1);
                var lstRequirementDetail = requirmentDetailServices.GetData<Tra_RequirementTrainDetailEntity>(objRequirementDetail, ConstantSql.hrm_tra_sp_get_RequirementDetail, UserLogin, ref status).ToList();

                //if(lstTrainee != null)
                //{
                //    lstTrainee = lstTrainee.Where(s => lstProfileID.Contains(s.ProfileID)).ToList();
                //}

                if (lstTraineeCertificate != null)
                {
                    lstTrainee = lstTrainee.Where(s => !lstTraineeIDFromTraineeCertificate.Contains(s.ID)).ToList();
                }

                foreach (var item in lstCourse)
                {
                    string[] arrCourse = new string[1];
                    if (!string.IsNullOrEmpty(item.CourseListID))
                    {
                        arrCourse = item.CourseListID.Split(',').ToArray();
                    }

                    string[] arrJobtitle = new string[1];
                    int[] arrOrg = new int[1];
                    if (!string.IsNullOrEmpty(item.JobTitleListCode1))
                    {
                        arrJobtitle = item.JobTitleListCode1.Split(',').ToArray();
                    }
                    if (!string.IsNullOrEmpty(item.OrgListCode1))
                    {
                        arrOrg = item.OrgListCode1.Split(',').Select(s => int.Parse(s)).ToArray();
                    }

                    var lstJobtitleByJobtitleCode = lstJobtitle.Where(s => arrJobtitle.Contains(s.Code)).Select(s => s.ID).ToList();
                    var lstOrgByOrderNumber = lstallorgs.Where(s => arrOrg.Contains(s.OrderNumber)).Select(s => s.ID).ToList();
                    string[] _ranklistID = new string[1];
                    if (!string.IsNullOrEmpty(item.RankListID))
                    {
                        _ranklistID = item.RankListID.Split(',').ToArray();
                    }

                    // var lstClassByCourseID = lstClass.Where(s => s.CourseID == item.ID && arrCourse.Contains(s.CourseID)).ToList();
                    var salaryClassEntity = lstSalaryClass.Where(s => _ranklistID.Contains(s.Code)).ToList();
                    var lstSalaryClassID = salaryClassEntity.Select(s => s.ID).ToList();

                    var lstTraineePassedCourse = new List<Tra_TraineeEntity>();
                    // var lstTraineeForProfile = lstProfile.Where(s => s.CodeEmp == "M26423");
                    if (arrCourse[0] != null)
                    {
                        lstTraineePassedCourse = lstTrainee.Where(s => arrCourse.Contains(s.CourseCode) && s.Status == EnumDropDown.TraineeStatus.E_PASSED.ToString()).ToList();
                        int soLuongKhoaTienQUyet = arrCourse.Count();
                        var lstCheckTraineePassedCourse = lstTraineePassedCourse.GroupBy(m => m.ProfileID).Where(m => m.Count() >= soLuongKhoaTienQUyet).ToList();
                        var lstProfileIDPassedCourse = lstCheckTraineePassedCourse.Select(s => s.Key).Distinct().ToList();
                        lstProfile = lstProfile.Where(s => lstProfileIDPassedCourse.Contains(s.ID)).ToList();
                    }

                    foreach (var profileEntity in lstProfile)
                    {
                        var traineePassCourse = lstTrainee.Where(s => s.CourseID == item.ID && profileEntity.ID == s.ProfileID && s.Status == EnumDropDown.TraineeStatus.E_PASSED.ToString()).FirstOrDefault();
                        if (traineePassCourse != null)
                        {
                            continue;
                        }
                        Guid? orgId = profileEntity.OrgStructureID;
                        //    var org = lstOrg.FirstOrDefault(s => s.ID == profileEntity.OrgStructureID);
                        //   var orgOrg = LibraryService.GetNearestParentEntity(orgId, OrgUnit.E_DEPARTMENT, lstOrg, lstOrgType);
                        DataRow dr = table.NewRow();
                        bool flag = false;
                        var lstTraineeByProfileID = lstTrainee.Where(s => s.ProfileID == profileEntity.ID && arrCourse.Contains(s.CourseCode)).ToList();
                        dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.ID] = profileEntity.ID != null ? profileEntity.ID : Guid.Empty;
                        dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.CodeEmp] = profileEntity.CodeEmp != null ? profileEntity.CodeEmp : string.Empty;
                        dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.TraineeName] = profileEntity.ProfileName != null ? profileEntity.ProfileName : string.Empty;
                        dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.IDNo] = profileEntity.IDNo != null ? profileEntity.IDNo : string.Empty;
                        dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.OrgStructureName] = profileEntity.OrgStructureName != null ? profileEntity.OrgStructureName : string.Empty;
                        dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.E_UNIT] = profileEntity.E_UNIT != null ? profileEntity.E_UNIT : string.Empty;
                        dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.E_DIVISION] = profileEntity.E_DIVISION != null ? profileEntity.E_DIVISION : string.Empty;
                        dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.E_DEPARTMENT] = profileEntity.E_DEPARTMENT != null ? profileEntity.E_DEPARTMENT : string.Empty;
                        dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.E_SECTION] = profileEntity.E_SECTION != null ? profileEntity.E_SECTION : string.Empty;
                        dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.E_TEAM] = profileEntity.E_TEAM != null ? profileEntity.E_TEAM : string.Empty;
                        dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.JobTitleName] = profileEntity.JobTitleName != null ? profileEntity.JobTitleName : string.Empty;
                        dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.PositionName] = profileEntity.PositionName != null ? profileEntity.PositionName : string.Empty;
                        dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.CourseName] = item.CourseName != null ? item.CourseName : string.Empty;

                        //var profileEntity = lstProfile.Where(s => s.ID == trainee.ProfileID && s.SalaryClassID != null).FirstOrDefault();
                        var stopWorkingEntity = lstStopWorking.Where(s => s.ProfileID.Value == profileEntity.ID).OrderByDescending(s => s.DateUpdate).FirstOrDefault();

                        //var workingEntity = new Hre_WorkHistoryEntity();
                        var lstWorkingByProfileID = lstWorking.Where(s => s.ProfileID == profileEntity.ID && lstSalaryClassID.Contains(s.SalaryClassID == null ? Guid.Empty : s.SalaryClassID.Value)).ToList();
                        if (lstJobtitleByJobtitleCode.Count > 0)
                        {
                            lstWorkingByProfileID = lstWorkingByProfileID.Where(s => lstJobtitleByJobtitleCode.Contains(s.JobTitleID != null ? s.JobTitleID.Value : Guid.Empty)).ToList();

                        }
                        if (lstOrgByOrderNumber.Count > 0)
                        {
                            lstWorkingByProfileID = lstWorkingByProfileID.Where(s => lstOrgByOrderNumber.Contains(s.OrganizationStructureID != null ? s.OrganizationStructureID.Value : Guid.Empty)).ToList();
                        }
                        var workingEntity = lstWorkingByProfileID.OrderByDescending(s => s.DateUpdate).FirstOrDefault();

                        var dateSuspend = new TimeSpan();
                        var dateSeniority = new TimeSpan();
                        double dateResult = 0;
                        double monthSeniority = 0;
                        if (profileEntity.DateHire == null)
                        {
                            continue;
                        }
                        dateSeniority = dateCheckConfig.Subtract(profileEntity.DateHire.Value);

                        if (stopWorkingEntity != null && stopWorkingEntity.DateStop != null && stopWorkingEntity.DateComeBack != null)
                        {
                            dateSuspend = stopWorkingEntity.DateComeBack.Value.Subtract(stopWorkingEntity.DateStop.Value);
                            dateResult = dateSeniority.Subtract(dateSuspend).TotalDays / 30;
                            monthSeniority = Math.Floor(dateResult);
                        }

                        dateSeniority = dateCheckConfig.Subtract(profileEntity.DateHire.Value);
                        monthSeniority = Math.Floor(dateSeniority.TotalDays / 30);

                        dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.Seniority] = monthSeniority;


                        #region KT điều kiện seniority và maxSeniority != null
                        if (item.Seniority != null && item.MaxSeniority != null)
                        {
                            if (monthSeniority >= item.Seniority && item.MaxSeniority >= monthSeniority)
                            {

                                if (salaryClassEntity != null)
                                {
                                    if (workingEntity != null)
                                    {
                                        // if (profileEntity.SalaryClassID != workingEntity.SalaryClassID)
                                        // {
                                        var dateCheckRank = dateCheck.Subtract(workingEntity.DateEffective).TotalDays / 30;
                                        var monthRank = Math.Floor(dateCheckRank);
                                        if (item.TimeOnCurrentRank != null)
                                        {
                                            if (monthRank < item.TimeOnCurrentRank)
                                            {
                                                flag = true;
                                                // table.Rows.Add(dr);
                                            }
                                        }

                                        //}

                                    }
                                }

                            }
                            else
                            {
                                flag = true;
                            }
                        }
                        #endregion

                        #region KT điều kiện seniority != null && maxSeniority == null
                        if (item.Seniority != null && item.MaxSeniority == null)
                        {
                            if (monthSeniority >= item.Seniority)
                            {
                                if (salaryClassEntity != null)
                                {
                                    if (workingEntity != null)
                                    {
                                        //  if (profileEntity.SalaryClassID != workingEntity.SalaryClassID)
                                        // {
                                        var dateCheckRank = dateCheck.Subtract(workingEntity.DateEffective).TotalDays / 30;
                                        var monthRank = Math.Floor(dateCheckRank);
                                        if (item.TimeOnCurrentRank != null)
                                        {
                                            if (monthRank < item.TimeOnCurrentRank)
                                            {
                                                flag = true;
                                                //  table.Rows.Add(dr);
                                            }
                                        }
                                        // }
                                    }
                                }
                            }
                            else
                            {
                                flag = true;
                            }
                        }
                        #endregion

                        #region KT điều kiện seniority == && maxSeniority != null
                        if (item.MaxSeniority != null && item.Seniority == null)
                        {
                            if (monthSeniority <= item.MaxSeniority)
                            {
                                if (salaryClassEntity != null)
                                {
                                    if (workingEntity != null)
                                    {
                                        // if (profileEntity.SalaryClassID != workingEntity.SalaryClassID)
                                        // {
                                        var dateCheckRank = dateCheck.Subtract(workingEntity.DateEffective).TotalDays / 30;
                                        var monthRank = Math.Floor(dateCheckRank);
                                        if (item.TimeOnCurrentRank != null)
                                        {
                                            if (item.TimeOnCurrentRank != null)
                                            {
                                                if (monthRank < item.TimeOnCurrentRank)
                                                {
                                                    flag = true;
                                                    // table.Rows.Add(dr);
                                                }
                                            }

                                        }

                                        //}

                                    }
                                }
                            }
                            else
                            {
                                flag = true;
                            }

                        }
                        #endregion

                        //#region kiem tra dk Seniority == null và MaxSeniority == null
                        //if (item.Seniority == null && item.MaxSeniority == null)
                        //{
                        //    if (salaryClassEntity != null)
                        //    {
                        //        if (workingEntity != null)
                        //        {
                        //           // if (profileEntity.SalaryClassID != workingEntity.SalaryClassID)
                        //           // {
                        //                var dateCheckRank = dateCheck.Subtract(workingEntity.DateEffective).TotalDays / 30;
                        //                var monthRank = Math.Floor(dateCheckRank);
                        //                if (item.TimeOnCurrentRank != null)
                        //                {
                        //                    if (item.TimeOnCurrentRank != null)
                        //                    {
                        //                        if (monthRank < item.TimeOnCurrentRank)
                        //                        {
                        //                            flag = true;
                        //                          //  table.Rows.Add(dr);
                        //                        }
                        //                    }

                        //                }
                        //            //}

                        //        }
                        //    }
                        //}
                        //#endregion


                        if (lstTraineeByProfileID.Count > 0)
                        {
                            foreach (var traineeEntity in lstTraineeByProfileID)
                            {
                                // dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.Note] = traineeEntity.TeacherComment != null ? traineeEntity.TeacherComment : string.Empty;
                                double monthCourse = Math.Floor(dateCheck.Subtract(traineeEntity.EndDate.Value).TotalDays / 30);
                                var traineeInRequirementDetail = lstRequirementDetail.Where(s => s.CourseID == traineeEntity.ID && traineeEntity.ID == s.ProfileID && s.YearAnalyze != null && s.YearAnalyze.Value.Year == dateCheckConfig.Year).ToList();

                                if (traineeInRequirementDetail.Count >= 1)
                                {
                                    flag = true;
                                    continue;
                                }
                                if (traineeEntity.EndDate == null)
                                {
                                    flag = true;
                                    continue;
                                }

                                if (item.DurationMinCourse != null)
                                {
                                    if (monthCourse < item.DurationMinCourse)
                                    {
                                        flag = true;
                                        continue;
                                    }

                                }
                            }
                        }
                        else
                        {
                            var traineeInRequirementDetail = lstRequirementDetail.Where(s => s.CourseID == item.ID && profileEntity.ID == s.ProfileID && s.YearAnalyze != null && s.YearAnalyze.Value.Year == dateCheckConfig.Year).ToList();
                            if (traineeInRequirementDetail.Count >= 1)
                            {
                                flag = true;
                                continue;
                            }

                        }


                        if (flag == false)
                        {
                            if (salaryClassEntity != null)
                            {
                                if (workingEntity != null)
                                {
                                    // if (profileEntity.SalaryClassID != workingEntity.SalaryClassID)
                                    // {
                                    var dateCheckRank = dateCheck.Subtract(workingEntity.DateEffective).TotalDays / 30;
                                    var monthRank = Math.Floor(dateCheckRank);
                                    if (item.TimeOnCurrentRank != null)
                                    {
                                        if (item.TimeOnCurrentRank != null)
                                        {
                                            if (monthRank >= item.TimeOnCurrentRank)
                                            {
                                                flag = true;
                                                table.Rows.Add(dr);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        table.Rows.Add(dr);
                                    }
                                    // }
                                }
                            }
                        }
                    }
                }
            }

            return table;
        }

        public DataTable CreateReportTraineeJoinCoursePassSchema()
        {
            DataTable tb = new DataTable("Tra_ReportTraineeJoinCourseEntity");
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.ID);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.TraineeName);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.RequirementTrainName);

            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.IDNo);
            //  tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.OrgStructureName);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.E_DEPARTMENT);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.E_DIVISION);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.E_SECTION);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.E_TEAM);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.E_UNIT);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.JobTitleName);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.PositionName);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.CourseName);
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.Seniority, typeof(double));
            tb.Columns.Add(Tra_ReportTraineeJoinCourseEntity.FieldNames.Note);

            return tb;
        }
        public DataTable GetReportTraineeJoinCoursePass(Guid[] courseIds, string orderNumber, bool IsCreateTemplate, Guid[] RequirementTrainIDs, string UserLogin)
        {

            DataTable table = CreateReportTraineeJoinCoursePassSchema();
            if (IsCreateTemplate)
            {
                return table;
            }
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;

                var profileServices = new Hre_ProfileServices();
                var objProfile = new List<object>();
                objProfile.Add(orderNumber);
                objProfile.Add(null);
                objProfile.Add(null);
                //var lstProfile = profileServices.GetData<Hre_ProfileEntity>(objProfile, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();

                var lstProfile = profileServices.GetData<Hre_ProfileEntity>(objProfile, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrgN, UserLogin, ref status).ToList();
                var lstProfileID = lstProfile.Select(s => s.ID).ToList();
                //var orgsService = new Cat_OrgStructureServices();
                //var objOrg = new List<object>();
                //objOrg.AddRange(new object[5]);
                //objOrg[3] = 1;
                //objOrg[4] = int.MaxValue - 1;
                //var lstallorgs = orgsService.GetData<Cat_OrgStructureEntity>(objOrg, ConstantSql.hrm_cat_sp_get_OrgStructure, ref status).ToList();

                //var courseServices = new Tra_CourseServices();
                //var objCourse = new List<object>();
                //objCourse.AddRange(new object[11]);
                //objCourse[9] = 1;
                //objCourse[10] = int.MaxValue - 1;
                //var lstCourse = courseServices.GetData<Tra_CourseEntity>(objCourse, ConstantSql.hrm_tra_sp_get_Course, ref status).ToList();
                //if (courseIds != null)
                //{
                //    lstCourse = lstCourse.Where(s => courseIds.Contains(s.ID)).ToList();
                //}
                //if (RequirementTrainIDs != null)
                //{

                //    lstCourse = lstCourse.Where(s => s.RequirementTrainID != null && RequirementTrainIDs.Contains((Guid)s.RequirementTrainID)).ToList();
                //}

                var requirmentDetailServices = new Tra_RequirementTrainDetailServices();
                var objRequirementDetail = new List<object>();
                objRequirementDetail.Add(null);
                objRequirementDetail.Add(1);
                objRequirementDetail.Add(int.MaxValue - 1);
                var lstRequirementDetail = requirmentDetailServices.GetData<Tra_RequirementTrainDetailEntity>(objRequirementDetail, ConstantSql.hrm_tra_sp_get_RequirementDetail, UserLogin, ref status).ToList();

                if (RequirementTrainIDs != null)
                {
                    lstRequirementDetail = lstRequirementDetail.Where(s => s.RequirementTrainID != null && RequirementTrainIDs.Contains(s.RequirementTrainID.Value)).ToList();
                }
                
                if(courseIds != null)
                {
                    lstRequirementDetail = lstRequirementDetail.Where(s => s.CourseID != null && courseIds.Contains(s.CourseID.Value)).ToList();
                }

                var traineeServices = new Tra_TraineeServices();
                var objTrainee = new List<object>();
                objTrainee.AddRange(new object[15]);
                objTrainee[13] = 1;
                objTrainee[14] = int.MaxValue - 1;
                var lstTrainee = traineeServices.GetData<Tra_TraineeEntity>(objTrainee, ConstantSql.hrm_tra_sp_get_Trainee, UserLogin, ref status).ToList();

                if (courseIds != null)
                {
                    lstTrainee = lstTrainee.Where(s => courseIds.Contains(s.CourseID)).ToList();
                }

                lstTrainee = lstTrainee.Where(s => lstProfileID.Contains(s.ProfileID)).ToList();

                var lstClassIDByTrainee = lstTrainee.Select(s =>  s.ClassID).ToList();

                var lstTraineeByProfileID = lstTrainee.Select(s => s.ProfileID).ToList();

                var lstCourseId = lstTrainee.Select(s => s.CourseID).ToList();

                if (lstTraineeByProfileID.Count > 0)
                {
                    lstRequirementDetail = lstRequirementDetail.Where(s => s.CourseID != null && s.ClassID != null && s.ProfileID != null && lstTraineeByProfileID.Contains(s.ProfileID.Value) && lstClassIDByTrainee.Contains(s.ClassID.Value) && lstCourseId.Contains(s.CourseID.Value)).ToList();
                }

                foreach (var item in lstRequirementDetail)
                {
                    DataRow dr = table.NewRow();

                    dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.CodeEmp] = item.CodeEmp;
                    dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.TraineeName] = item.ProfileName;
                    dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.CourseName] = item.CourseName;
                    dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.E_DEPARTMENT] = item.E_DEPARTMENT;
                    dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.E_DIVISION] = item.E_DIVISION;
                    dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.E_SECTION] = item.E_SECTION;
                    dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.E_TEAM] = item.E_TEAM;
                    dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.E_UNIT] = item.E_UNIT;
                    dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.JobTitleName] = item.JobTitleName;
                    dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.PositionName] = item.PositionName;
                    dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.Note] = item.Comment;
                    dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.RequirementTrainName] = item.RequirementTrainName;
                    if (item.Seniority != null)
                        dr[Tra_ReportTraineeJoinCourseEntity.FieldNames.Seniority] = item.Seniority;
                    table.Rows.Add(dr);
                } 

            }

            return table;

        }
        #endregion


        #region BC học viên theo tháng

        public DataTable CreateReportTraineeByMonth()
        {
            DataTable tb = new DataTable("Tra_ReportTraineeByMonthEntity");
            tb.Columns.Add(Tra_ReportTraineeByMonthEntity.FieldNames.ID);
            tb.Columns.Add(Tra_ReportTraineeByMonthEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Tra_ReportTraineeByMonthEntity.FieldNames.ProfileName);
            tb.Columns.Add(Tra_ReportTraineeByMonthEntity.FieldNames.E_DEPARTMENT);
            tb.Columns.Add(Tra_ReportTraineeByMonthEntity.FieldNames.E_DIVISION);
            tb.Columns.Add(Tra_ReportTraineeByMonthEntity.FieldNames.E_SECTION);
            tb.Columns.Add(Tra_ReportTraineeByMonthEntity.FieldNames.E_TEAM);
            tb.Columns.Add(Tra_ReportTraineeByMonthEntity.FieldNames.E_UNIT);
            tb.Columns.Add(Tra_ReportTraineeByMonthEntity.FieldNames.ClassName);
            tb.Columns.Add(Tra_ReportTraineeByMonthEntity.FieldNames.CourseName);
            tb.Columns.Add(Tra_ReportTraineeByMonthEntity.FieldNames.StartDate, typeof(DateTime));
            tb.Columns.Add(Tra_ReportTraineeByMonthEntity.FieldNames.Month, typeof(DateTime));
            tb.Columns.Add(Tra_ReportTraineeByMonthEntity.FieldNames.WorkPlace);
            return tb;
        }

        public DataTable GetReportTraineeByMonth(DateTime Month, Guid? CourseID, bool IsCreateTemplate, string UserLogin)
        {
            DataTable table = CreateReportTraineeByMonth();
            if (IsCreateTemplate)
            {
                return table;
            }
            string status = string.Empty;

            var classServices = new Tra_ClassServices();
            var traineeServices = new Tra_TraineeServices();
            var objClass = new List<object>();
            objClass.Add(null);
            objClass.Add(null);
            objClass.Add(null);
            objClass.Add(null);
            objClass.Add(null);
            objClass.Add(null);
            objClass.Add(null);
            objClass.Add(null);
            objClass.Add(1);
            objClass.Add(int.MaxValue - 1);
            var lstClass = classServices.GetData<Tra_ClassEntity>(objClass, ConstantSql.hrm_tra_sp_get_Class, UserLogin, ref status).ToList();
            if (lstClass == null || lstClass.Count == 0)
            {
                return table;
            }
            lstClass = lstClass.Where(s => s.StartDate != null && s.StartDate.Value.Month == Month.Month && s.StartDate.Value.Year == Month.Year).ToList();
            if (CourseID != null)
            {
                lstClass = lstClass.Where(s => s.CourseID != null && s.CourseID == CourseID).ToList();
            }
            var lstClassId = lstClass.Select(s => s.ID).ToList();


            var objTrainee = new List<object>();
            objTrainee.AddRange(new object[15]);
            objTrainee[13] = 1;
            objTrainee[14] = int.MaxValue - 1;
            var lsttrainee = classServices.GetData<Tra_TraineeEntity>(objTrainee, ConstantSql.hrm_tra_sp_get_Trainee, UserLogin, ref status).ToList();

            if (lsttrainee == null || lsttrainee.Count == 0)
            {
                return table;
            }

            lsttrainee = lsttrainee.Where(s => lstClassId.Contains(s.ClassID)).ToList();

            if (lsttrainee == null || lsttrainee.Count == 0)
            {
                return table;
            }

            var courseServices = new Tra_CourseServices();
            var objCourse = new List<object>();
            objCourse.AddRange(new object[11]);
            objCourse[9] = 1;
            objCourse[10] = int.MaxValue - 1;
            var lstCourse = courseServices.GetData<Tra_CourseEntity>(objCourse, ConstantSql.hrm_tra_sp_get_Course, UserLogin, ref status).ToList();

            foreach (var trainee in lsttrainee)
            {
                DataRow dr = table.NewRow();
                var classbytrainee = lstClass.Where(s => s.ID == trainee.ClassID).FirstOrDefault();
                dr[Tra_ReportTraineeByMonthEntity.FieldNames.CodeEmp] = trainee.CodeEmp;
                dr[Tra_ReportTraineeByMonthEntity.FieldNames.ProfileName] = trainee.ProfileName;
                dr[Tra_ReportTraineeByMonthEntity.FieldNames.E_DEPARTMENT] = trainee.E_DEPARTMENT;
                dr[Tra_ReportTraineeByMonthEntity.FieldNames.E_DIVISION] = trainee.E_DIVISION;
                dr[Tra_ReportTraineeByMonthEntity.FieldNames.E_SECTION] = trainee.E_SECTION;
                dr[Tra_ReportTraineeByMonthEntity.FieldNames.E_TEAM] = trainee.E_TEAM;
                dr[Tra_ReportTraineeByMonthEntity.FieldNames.E_UNIT] = trainee.E_UNIT;
                dr[Tra_ReportTraineeByMonthEntity.FieldNames.ClassName] = trainee.ClassName;
                if (classbytrainee != null)
                {
                    var coursebyclass = lstCourse.Where(s => s.ID == classbytrainee.CourseID).FirstOrDefault();
                    dr[Tra_ReportTraineeByMonthEntity.FieldNames.CourseName] = coursebyclass != null ? coursebyclass.CourseName : null;
                }
                if (trainee.StartDate != null)
                {
                    dr[Tra_ReportTraineeByMonthEntity.FieldNames.StartDate] = trainee.StartDate;
                }

                dr[Tra_ReportTraineeByMonthEntity.FieldNames.Month] = Month;
                dr[Tra_ReportTraineeByMonthEntity.FieldNames.WorkPlace] = trainee.WorkPlaceName;
                table.Rows.Add(dr);
            }
            return table;
        }
        #endregion

        #region Tra_ReportTraineeResult
        public DataTable CreateReportTraineeResult()
        {
            DataTable tb = new DataTable("Tra_ReportTraineeResultEntity");
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.CodeEmp, typeof(string));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.ProfileName, typeof(string));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.E_DEPARTMENT);
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.E_DIVISION);
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.E_SECTION);
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.E_TEAM);
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.E_UNIT);
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.CourseCode, typeof(string));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.CourseName, typeof(string));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.ClassCode, typeof(string));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.ClassName, typeof(string));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.Score1, typeof(double));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.Score2, typeof(double));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.Score3, typeof(double));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.StatusView, typeof(string));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.StartDate, typeof(DateTime));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.EndDate, typeof(DateTime));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.Result, typeof(double));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.TrainLevelName, typeof(string));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.RequirementTrainName, typeof(string));
            

            return tb;
        }
        public DataTable GetReportTraineeResult(List<Tra_TraineeEntity> lstTrainee, bool IsCreateTemplate, string _ScoreTypeID, string UserLogin)
        {

            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var ScoreTypeServices = new Tra_ScoreTypeServices();
                Tra_ScoreTypeEntity objScoreTypeentity = new Tra_ScoreTypeEntity();

                if (_ScoreTypeID != null)
                {
                    _ScoreTypeID = Common.DotNetToOracle(_ScoreTypeID);
                    objScoreTypeentity = ScoreTypeServices.GetData<Tra_ScoreTypeEntity>(_ScoreTypeID, ConstantSql.hrm_tra_sp_get_ScoreTypeByIds, UserLogin, ref status).FirstOrDefault();
                }

                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                DataTable table = CreateReportTraineeResult();
                if (IsCreateTemplate)
                {
                    return table;
                }
                if (lstTrainee == null)
                {
                    return table;
                }
                List<Guid> lstTraineeIDs = lstTrainee.Select(s => s.ID).ToList();
                var traineeTopicServices = new Tra_TraineeTopicServices();
                var objTraineeTopic = new List<object>();
                objTraineeTopic.Add(1);
                objTraineeTopic.Add(int.MaxValue - 1);
                var lstTraineeTopic = traineeTopicServices.GetData<Tra_TraineeTopicEntity>(objTraineeTopic, ConstantSql.hrm_tra_sp_get_TraineeTopic, UserLogin, ref status).ToList();
                List<Guid> lstTraineeTopicIDs = null;
                List<Guid> lstTopicIDs = null;
                if (lstTraineeTopic.Count > 0)
                {
                    lstTraineeTopic = lstTraineeTopic.Where(s => s.TraineeID != null && lstTraineeIDs.Contains(s.TraineeID.Value)).ToList();
                    if (lstTraineeTopic.Count > 0)
                    {
                        lstTraineeTopicIDs = lstTraineeTopic.Select(s => s.ID).ToList();
                        lstTopicIDs = lstTraineeTopic.Select(s => s.TopicID.Value).Distinct().ToList();
                    }

                }
                var scoreTopicServices = new Tra_ScoreTopicServices();
                var objScoreTopic = new List<object>();
                objScoreTopic.Add(1);
                objScoreTopic.Add(int.MaxValue - 1);
                var lstScoreTopic = scoreTopicServices.GetData<Tra_ScoreTopicEntity>(objScoreTopic, ConstantSql.hrm_tra_sp_get_ScoreTopic, UserLogin, ref status).ToList();
                if (lstScoreTopic.Count > 0 && lstTopicIDs != null)
                {
                    lstScoreTopic = lstScoreTopic.Where(s => s.TopicID != null && lstTopicIDs.Contains(s.TopicID.Value)).ToList();
                }
                var traineeScoreServices = new Tra_TraineeScoreServices();
                var objTraineeScore = new List<object>();
                objTraineeScore.Add(1);
                objTraineeScore.Add(int.MaxValue - 1);
                var lstTraineeScore = traineeScoreServices.GetData<Tra_TraineeScoreEntity>(objTraineeScore, ConstantSql.hrm_tra_sp_get_TraineeScore, UserLogin, ref status).ToList();
                if (lstTraineeScore.Count > 0 && lstTraineeTopicIDs != null)
                {
                    lstTraineeScore = lstTraineeScore.Where(s => s.TraineeTopicID != null && lstTraineeTopicIDs.Contains(s.TraineeTopicID.Value)).ToList();
                }
                //var traineeScoreTypeServices = new Tra_ScoreTypeServices();
                //var objTraineeScoreType = new List<object>();
                //objTraineeScoreType.AddRange(new object[4]);
                //objTraineeScoreType[2] = 1;
                //objTraineeScoreType[3] = int.MaxValue - 1;
                //var lstTraineeScoreType = traineeScoreTypeServices.GetData<Tra_ScoreTypeEntity>(objTraineeScoreType, ConstantSql.hrm_tra_sp_get_ScoreType, ref status).ToList();

                foreach (var item in lstTrainee)
                {
                    DataRow row = table.NewRow();
                    var flag = false;
                    //diem
                    if (lstTraineeTopic.Count > 0)
                    {
                        var objTraineeTopicByTrainId = lstTraineeTopic.Where(s => s.TraineeID == item.ID).ToList();
                        foreach (var traineeTopic in objTraineeTopicByTrainId)
                        {
                            var lstScoreTopicByTopicID = lstScoreTopic.Where(s => s.TopicID == traineeTopic.TopicID).Select(s => s.ScoreTypeID).ToList();
                            var lstTraineeScoreByTypeID = lstTraineeScore.Where(s => s.ScoreTypeID != null && lstScoreTopicByTopicID.Contains(s.ScoreTypeID) && s.Score != null && s.TraineeTopicID.Value == traineeTopic.ID).ToList();

                            if (lstTraineeScoreByTypeID.Count > 0)
                            {
                                var lstTraineeScoreByScore = lstTraineeScoreByTypeID.Where(s => s.Score != null).ToList();
                                if (lstTraineeScoreByScore.Count > 0)
                                {

                                    if (objScoreTypeentity != null && objScoreTypeentity.NumOrder != null && lstTraineeScoreByTypeID.Count > 0)
                                    {
                                        var objTraineeScoreByNumOder = lstTraineeScoreByTypeID.Where(s => s.NumOrder.Value == objScoreTypeentity.NumOrder.Value + 1).FirstOrDefault();
                                        if (objTraineeScoreByNumOder != null)
                                            break;
                                    }

                                    flag = true;
                                    if (item.CodeEmp != null)
                                        row[Tra_ReportTraineeResultEntity.FieldNames.CodeEmp] = item.CodeEmp;
                                    if (item.ProfileName != null)
                                        row[Tra_ReportTraineeResultEntity.FieldNames.ProfileName] = item.ProfileName;
                                    if (item.E_DEPARTMENT != null)
                                        row[Tra_ReportTraineeResultEntity.FieldNames.E_DEPARTMENT] = item.E_DEPARTMENT;
                                    if (item.E_DIVISION != null)
                                        row[Tra_ReportTraineeResultEntity.FieldNames.E_DIVISION] = item.E_DIVISION;
                                    if (item.E_SECTION != null)
                                        row[Tra_ReportTraineeResultEntity.FieldNames.E_SECTION] = item.E_SECTION;
                                    if (item.E_TEAM != null)
                                        row[Tra_ReportTraineeResultEntity.FieldNames.E_TEAM] = item.E_TEAM;
                                    if (item.E_UNIT != null)
                                        row[Tra_ReportTraineeResultEntity.FieldNames.E_UNIT] = item.E_UNIT;
                                    if (item.CourseCode != null)
                                        row[Tra_ReportTraineeResultEntity.FieldNames.CourseCode] = item.CourseCode;
                                    if (item.CourseName != null)
                                        row[Tra_ReportTraineeResultEntity.FieldNames.CourseName] = item.CourseName;
                                    if (item.ClassCode != null)
                                        row[Tra_ReportTraineeResultEntity.FieldNames.ClassCode] = item.ClassCode;
                                    if (item.ClassName != null)
                                        row[Tra_ReportTraineeResultEntity.FieldNames.ClassName] = item.ClassName;
                                    if (item.StartDate != null)
                                        row[Tra_ReportTraineeResultEntity.FieldNames.StartDate] = item.StartDate;
                                    if (item.StatusView != null)
                                        row[Tra_ReportTraineeResultEntity.FieldNames.StatusView] = item.StatusView;
                                    if (item.EndDate != null)
                                        row[Tra_ReportTraineeResultEntity.FieldNames.EndDate] = item.EndDate;
                                    if (item.Result != null)
                                        row[Tra_ReportTraineeResultEntity.FieldNames.Result] = item.Result;
                                    if (item.TrainLevelName != null)
                                        row[Tra_ReportTraineeResultEntity.FieldNames.TrainLevelName] = item.TrainLevelName;
                                }
                                foreach (var traineeScore in lstTraineeScoreByTypeID)
                                {
                                    if (traineeScore.Score != null)
                                    {
                                        if (traineeScore.NumOrder == 1)
                                        {
                                            row[Tra_ReportTraineeResultEntity.FieldNames.Score1] = traineeScore.Score;
                                        }
                                        if (traineeScore.NumOrder == 2)
                                        {
                                            row[Tra_ReportTraineeResultEntity.FieldNames.Score2] = traineeScore.Score;
                                        }
                                        if (traineeScore.NumOrder == 3)
                                        {
                                            row[Tra_ReportTraineeResultEntity.FieldNames.Score3] = traineeScore.Score;
                                        }
                                    }

                                }

                            }
                        }

                    }
                    if (flag)
                    {
                        table.Rows.Add(row);

                    }
                }
                return table.ConfigTable(true);
            }
        }
        public DataTable GetReportTraineeResultNew(Tra_ReportTraineeResultSearchEntity model, string UserLogin, bool IsCreateTemplate)
        {
            DataTable table = CreateReportTraineeResult();
            if (IsCreateTemplate)
            {
                return table;
            }
            var baseService = new BaseService();
            var lstObject = new List<object>();
            lstObject.Add(model.ProfileName);
            lstObject.Add(model.CodeEmp);
            lstObject.Add(model.OrgStructureID);
            lstObject.Add(model.CourseID);
            lstObject.Add(model.ClassID);
            lstObject.Add(model.DateFrom);
            lstObject.Add(model.DateTo);
            lstObject.Add(model.ScoreTypeID);
            lstObject.Add(model.RequirementTrainIDs);
            lstObject.Add(1);
            lstObject.Add(int.MaxValue - 1);
            string status = "";
            var lstTra_TraineeScoreEntity = baseService.GetData<Tra_TraineeScoreEntity>(lstObject, ConstantSql.hrm_tra_sp_get_ReportTraineeResult, UserLogin, ref status);

            #region Code Old
            //List<Tra_TraineeScoreEntity> lstTra_TraineeScoreEntity = new List<Tra_TraineeScoreEntity>();
            //List<Cat_OrgStructure> orgs = new List<Cat_OrgStructure>();
            //List<Cat_OrgStructureType> orgTypes = new List<Cat_OrgStructureType>();
            //using (var context = new VnrHrmDataContext())
            //{
            //    var services = new Tra_TraineeServices();
            //    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
            //    //unitOfWork.CreateQueryable<Tra_TraineeScore>(d => d.Tra_TraineeTopic.TraineeID == Guid.Empty).ToList().Translate<Tra_TraineeScoreEntity>();
            //    List<Guid> arrOrgSt = new List<Guid>();
            //    List<Guid> arrCourseID = new List<Guid>();
            //    List<Guid> arrClassID = new List<Guid>();
            //    List<Guid> arrRequirementTrainID = new List<Guid>();

            //    if (!string.IsNullOrEmpty(model.OrgStructureID))
            //    {
            //        foreach (var item in model.OrgStructureID.Split(','))
            //        {
            //            arrOrgSt.Add(Guid.Parse(item));
            //        }
            //    }
            //    if (!string.IsNullOrEmpty(model.CourseID))
            //    {
            //        foreach (var item in model.CourseID.Split(','))
            //        {
            //            arrCourseID.Add(Guid.Parse(item));
            //        }
            //    }
            //    if (!string.IsNullOrEmpty(model.ClassID))
            //    {
            //        foreach (var item in model.ClassID.Split(','))
            //        {
            //            arrClassID.Add(Guid.Parse(item));
            //        }
            //    }
            //    if (!string.IsNullOrEmpty(model.RequirementTrainIDs))
            //    {
            //        foreach (var item in model.RequirementTrainIDs.Split(','))
            //        {
            //            arrRequirementTrainID.Add(Guid.Parse(item));
            //        }
            //    }
            //    var queryable = unitOfWork.CreateQueryable<Tra_TraineeScore>();


            //    if (!string.IsNullOrEmpty(model.ProfileName))
            //    {
            //        queryable = queryable.Where(d => d.Tra_TraineeTopic.Tra_Trainee.Hre_Profile.ProfileName.Contains(model.ProfileName));
            //    }
            //    if (!string.IsNullOrEmpty(model.CodeEmp))
            //    {
            //        queryable = queryable.Where(d => d.Tra_TraineeTopic.Tra_Trainee.Hre_Profile.CodeEmp.Contains(model.CodeEmp));
            //    }
            //    if (!string.IsNullOrEmpty(model.OrgStructureID))
            //    {
            //        queryable = queryable.Where(d => arrOrgSt.Contains(d.Tra_TraineeTopic.Tra_Trainee.Hre_Profile.Cat_OrgStructure.ID));
            //    }
            //    if (!string.IsNullOrEmpty(model.CourseID))
            //    {
            //        queryable = queryable.Where(d => arrCourseID.Contains(d.Tra_TraineeTopic.Tra_Trainee.Tra_Class1.CourseID));
            //    }
            //    if (!string.IsNullOrEmpty(model.ClassID))
            //    {
            //        queryable = queryable.Where(d => arrClassID.Contains(d.Tra_TraineeTopic.Tra_Trainee.ClassID));
            //    }
            //    //if (!string.IsNullOrEmpty(model.RequirementTrainIDs))
            //    //{
            //    //    queryable = queryable.Where(d => arrClassID.Contains(d.Tra_TraineeTopic.Tra_Trainee.ClassID));
            //    //}
            //    if (model.DateFrom.HasValue)
            //        queryable = queryable.Where(d => d.Tra_TraineeTopic.Tra_Trainee.Tra_Class1.EndDate > model.DateFrom);
            //    if (model.DateTo.HasValue)
            //        queryable = queryable.Where(d => d.Tra_TraineeTopic.Tra_Trainee.Tra_Class1.StartDate < model.DateTo);
            //    if (!string.IsNullOrEmpty(model.Status))
            //        queryable = queryable.Where(d => d.Tra_TraineeTopic.Tra_Trainee.Status == model.Status);
            //    if (!string.IsNullOrEmpty(model.ScoreTypeID))
            //    {
            //        Guid GuidScoreTypeID = Guid.Parse(model.ScoreTypeID);
            //        queryable = queryable.Where(d => d.ScoreTypeID == GuidScoreTypeID);
            //    }
                
            //    lstTra_TraineeScoreEntity = queryable
            //        .Select(d => new Tra_TraineeScoreEntity
            //    {
            //        CodeEmp = d.Tra_TraineeTopic.Tra_Trainee.Hre_Profile.CodeEmp,
            //        ProfileName = d.Tra_TraineeTopic.Tra_Trainee.Hre_Profile.ProfileName,
            //        OrgStructorId = d.Tra_TraineeTopic.Tra_Trainee.Hre_Profile.OrgStructureID,
            //        CourseCode = d.Tra_TraineeTopic.Tra_Trainee.Tra_Class1.Tra_Course.Code,
            //        CourseName = d.Tra_TraineeTopic.Tra_Trainee.Tra_Class1.Tra_Course.CourseName,
            //        ClassCode = d.Tra_TraineeTopic.Tra_Trainee.Tra_Class1.Code,
            //        ClassName = d.Tra_TraineeTopic.Tra_Trainee.Tra_Class1.ClassName,
            //        Score = d.Score,
            //        Status = d.Tra_TraineeTopic.Tra_Trainee.Status,
            //        StartDate = d.Tra_TraineeTopic.Tra_Trainee.Tra_Class1.StartDate,
            //        EndDate = d.Tra_TraineeTopic.Tra_Trainee.Tra_Class1.EndDate,
            //        Result = d.Tra_TraineeTopic.Tra_Trainee.Result,
            //        TrainLevelName = d.Tra_TraineeTopic.Tra_Trainee.Cat_TrainLevel.TrainLevelName,
            //        TraineeID = d.Tra_TraineeTopic.TraineeID,
            //        TraineeTopicID = d.TraineeTopicID,
            //        TraineeTopicName = d.Tra_TraineeTopic.Cat_Topic.TopicName,
            //        ScoreTypeID = d.ScoreTypeID,
            //        ScoreTypeName = d.Tra_ScoreType.ScoreTypeName,

            //        NumOrder = d.Tra_ScoreType.NumOrder
            //    }).ToList();
            //    var repoOrgStructure = new Cat_OrgStructureRepository(unitOfWork);
            //    var repoOrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
            //    orgs = repoOrgStructure.FindBy(s => s.Code != null).ToList();
            //    orgTypes = repoOrgStructureType.FindBy(s => s.IsDelete == null).ToList();

            //}
            #endregion
            if (lstTra_TraineeScoreEntity != null)
            {
                var lstResult = lstTra_TraineeScoreEntity.GroupBy(x => x.TraineeID).ToList();
                foreach (var item in lstResult)
                {
                    DataRow row = table.NewRow();
                    Tra_TraineeScoreEntity objResult = item.FirstOrDefault();
                    if (objResult.CodeEmp != null)
                        row[Tra_ReportTraineeResultEntity.FieldNames.CodeEmp] = objResult.CodeEmp;
                    if (objResult.ProfileName != null)
                        row[Tra_ReportTraineeResultEntity.FieldNames.ProfileName] = objResult.ProfileName;
                    if (objResult.E_DEPARTMENT != null)
                        row[Tra_ReportTraineeResultEntity.FieldNames.E_DEPARTMENT] = objResult.E_DEPARTMENT;
                    if (objResult.E_DIVISION != null)
                        row[Tra_ReportTraineeResultEntity.FieldNames.E_DIVISION] = objResult.E_DIVISION;
                    if (objResult.E_SECTION != null)
                        row[Tra_ReportTraineeResultEntity.FieldNames.E_SECTION] = objResult.E_SECTION;
                    if (objResult.E_TEAM != null)
                        row[Tra_ReportTraineeResultEntity.FieldNames.E_TEAM] = objResult.E_TEAM;
                    if (objResult.E_UNIT != null)
                        row[Tra_ReportTraineeResultEntity.FieldNames.E_UNIT] = objResult.E_UNIT;
                    if (objResult.CourseCode != null)
                        row[Tra_ReportTraineeResultEntity.FieldNames.CourseCode] = objResult.CourseCode;
                    if (objResult.CourseName != null)
                        row[Tra_ReportTraineeResultEntity.FieldNames.CourseName] = objResult.CourseName;
                    if (objResult.ClassCode != null)
                        row[Tra_ReportTraineeResultEntity.FieldNames.ClassCode] = objResult.ClassCode;
                    if (objResult.ClassName != null)
                        row[Tra_ReportTraineeResultEntity.FieldNames.ClassName] = objResult.ClassName;
                    if (objResult.StartDate != null)
                        row[Tra_ReportTraineeResultEntity.FieldNames.StartDate] = objResult.StartDate;
                    if (objResult.StatusView != null)
                        row[Tra_ReportTraineeResultEntity.FieldNames.StatusView] = objResult.StatusView;
                        //row[Tra_ReportTraineeResultEntity.FieldNames.StatusView] = EnumDropDown.GetEnumDescription<EnumDropDown.TraineeStatus>((EnumDropDown.TraineeStatus)Enum.Parse(typeof(EnumDropDown.TraineeStatus),objResult.Status.TranslateString(), true));
                    if (objResult.EndDate != null)
                        row[Tra_ReportTraineeResultEntity.FieldNames.EndDate] = objResult.EndDate;
                    if (objResult.Result != null)
                        row[Tra_ReportTraineeResultEntity.FieldNames.Result] = objResult.Result;
                    if (objResult.TrainLevelName != null)
                        row[Tra_ReportTraineeResultEntity.FieldNames.TrainLevelName] = objResult.TrainLevelName;
                    List<string> lstRequirementTrainName = new List<string>();
                    if (!string.IsNullOrEmpty(objResult.RequirementTrainName))
                    {
                        lstRequirementTrainName.Add(objResult.RequirementTrainName);
                    }
                    foreach (var traineeScore in item)
                    {
                        if (traineeScore.Score != null)
                        {
                            if (traineeScore.NumOrder == 1)
                            {
                                row[Tra_ReportTraineeResultEntity.FieldNames.Score1] = traineeScore.Score;
                            }
                            if (traineeScore.NumOrder == 2)
                            {
                                row[Tra_ReportTraineeResultEntity.FieldNames.Score2] = traineeScore.Score;
                            }
                            if (traineeScore.NumOrder == 3)
                            {
                                row[Tra_ReportTraineeResultEntity.FieldNames.Score3] = traineeScore.Score;
                            }
                        }
                        if (!string.IsNullOrEmpty(traineeScore.RequirementTrainName) && !lstRequirementTrainName.Contains(traineeScore.RequirementTrainName))
                        {
                            lstRequirementTrainName.Add(traineeScore.RequirementTrainName);
                        }
                    }
                    string strRequirementTrainName = "";
                    if (lstRequirementTrainName.Count > 0)
                    {
                        foreach (var requirementTrainName in lstRequirementTrainName)
                        {
                            strRequirementTrainName += requirementTrainName + ", ";
                        }
                    }
                    if (strRequirementTrainName != "")
                        row[Tra_ReportTraineeResultEntity.FieldNames.RequirementTrainName] = strRequirementTrainName.Substring(0, strRequirementTrainName.Length -2);
                    table.Rows.Add(row);
                }
            }
            return table;
        }
        public DataTable CreateTraineeByClassID()
        {
            DataTable tb = new DataTable("Tra_ReportTraineeEntity");
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.ID, typeof(Guid));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.ClassID, typeof(Guid));

            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.TraineeCertificateID, typeof(Guid));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.CodeEmp, typeof(string));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.ProfileName, typeof(string));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.E_UNIT, typeof(string));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.E_DIVISION, typeof(string));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.E_DEPARTMENT, typeof(string));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.E_SECTION, typeof(string));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.E_TEAM, typeof(string));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.JobTitleName, typeof(string));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.DateCertificate, typeof(DateTime));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.DateExpireCertificate, typeof(DateTime));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.Status, typeof(string));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.StatusView, typeof(string));

            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.RankingName, typeof(string));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.Score1, typeof(double));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.Score2, typeof(double));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.Score3, typeof(double));
            tb.Columns.Add(Tra_ReportTraineeResultEntity.FieldNames.Result, typeof(double));

            return tb;
        }
        public DataTable GetTraineeByClassID(List<Tra_TraineeEntity> lstTrainee, bool IsCreateTemplate)
        {
            DataTable table = CreateTraineeByClassID();
            List<Guid> lstTraineeID = lstTrainee.Select(x => x.ID).ToList();
            List<Tra_TraineeScoreEntity> lstTra_TraineeScoreEntity = new List<Tra_TraineeScoreEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var services = new Tra_TraineeServices();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                //unitOfWork.CreateQueryable<Tra_TraineeScore>(d => d.Tra_TraineeTopic.TraineeID == Guid.Empty).ToList().Translate<Tra_TraineeScoreEntity>();

                lstTra_TraineeScoreEntity = unitOfWork.CreateQueryable<Tra_TraineeScore>(d => lstTraineeID.Contains(d.Tra_TraineeTopic.TraineeID.Value)).Select(d => new Tra_TraineeScoreEntity
                {
                    TraineeID = d.Tra_TraineeTopic.TraineeID,
                    TraineeTopicID = d.TraineeTopicID,
                    TraineeTopicName = d.Tra_TraineeTopic.Cat_Topic.TopicName,
                    ScoreTypeID = d.ScoreTypeID,
                    ScoreTypeName = d.Tra_ScoreType.ScoreTypeName,
                    Score = d.Score,
                    NumOrder = d.Tra_ScoreType.NumOrder
                }).ToList();
            }
            foreach (var item in lstTrainee)
            {
                DataRow row = table.NewRow();
                row[Tra_ReportTraineeResultEntity.FieldNames.ID] = item.ID;
                row[Tra_ReportTraineeResultEntity.FieldNames.ClassID] = item.ClassID;

                if (item.TraineeCertificateID.HasValue)
                    row[Tra_ReportTraineeResultEntity.FieldNames.TraineeCertificateID] = item.TraineeCertificateID;
                row[Tra_ReportTraineeResultEntity.FieldNames.CodeEmp] = item.CodeEmp;
                row[Tra_ReportTraineeResultEntity.FieldNames.ProfileName] = item.ProfileName;
                if (!string.IsNullOrEmpty(item.E_UNIT))
                    row[Tra_ReportTraineeResultEntity.FieldNames.E_UNIT] = item.E_UNIT;
                if (!string.IsNullOrEmpty(item.E_DIVISION))
                    row[Tra_ReportTraineeResultEntity.FieldNames.E_DIVISION] = item.E_DIVISION;
                if (!string.IsNullOrEmpty(item.E_DEPARTMENT))
                    row[Tra_ReportTraineeResultEntity.FieldNames.E_DEPARTMENT] = item.E_DEPARTMENT;
                if (!string.IsNullOrEmpty(item.E_SECTION))
                    row[Tra_ReportTraineeResultEntity.FieldNames.E_SECTION] = item.E_SECTION;
                if (!string.IsNullOrEmpty(item.E_TEAM))
                    row[Tra_ReportTraineeResultEntity.FieldNames.E_TEAM] = item.E_TEAM;
                if (item.JobTitleName != null)
                    row[Tra_ReportTraineeResultEntity.FieldNames.JobTitleName] = item.JobTitleName;
                if (item.DateCertificate.HasValue)
                    row[Tra_ReportTraineeResultEntity.FieldNames.DateCertificate] = item.DateCertificate;
                if (item.DateExpireCertificate.HasValue)
                    row[Tra_ReportTraineeResultEntity.FieldNames.DateExpireCertificate] = item.DateExpireCertificate;
                if (!string.IsNullOrEmpty(item.Status))
                    row[Tra_ReportTraineeResultEntity.FieldNames.Status] = item.Status;
                if (!string.IsNullOrEmpty(item.StatusView))
                    row[Tra_ReportTraineeResultEntity.FieldNames.StatusView] = item.StatusView;
                if (!string.IsNullOrEmpty(item.RankingName))
                    row[Tra_ReportTraineeResultEntity.FieldNames.RankingName] = item.RankingName;
                List<Tra_TraineeScoreEntity> lstTraineeScore = lstTra_TraineeScoreEntity.Where(d => d.TraineeID == item.ID).ToList();
                if (lstTraineeScore.Count > 0)
                {
                    foreach (var traineeScore in lstTraineeScore)
                    {
                        if (traineeScore.Score != null)
                        {
                            if (traineeScore.NumOrder == 1)
                            {
                                row[Tra_ReportTraineeResultEntity.FieldNames.Score1] = traineeScore.Score;
                            }
                            if (traineeScore.NumOrder == 2)
                            {
                                row[Tra_ReportTraineeResultEntity.FieldNames.Score2] = traineeScore.Score;
                            }
                            if (traineeScore.NumOrder == 3)
                            {
                                row[Tra_ReportTraineeResultEntity.FieldNames.Score3] = traineeScore.Score;
                            }
                        }
                    }
                }
                if (item.Result.HasValue)
                    row[Tra_ReportTraineeResultEntity.FieldNames.Result] = item.Result;
                table.Rows.Add(row);
            }
            return table;
        }
        #endregion

        #region BC điều chỉnh lớp học cho học viên
        public DataTable CreateReportTransferTrainee()
        {
            DataTable tb = new DataTable("Tra_ReportTransferTraineeEntity");
            tb.Columns.Add(Tra_ReportTransferTraineeEntity.FieldNames.ID);
            tb.Columns.Add(Tra_ReportTransferTraineeEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Tra_ReportTransferTraineeEntity.FieldNames.ProfileName);
            tb.Columns.Add(Tra_ReportTransferTraineeEntity.FieldNames.E_DEPARTMENT);
            tb.Columns.Add(Tra_ReportTransferTraineeEntity.FieldNames.E_DIVISION);
            tb.Columns.Add(Tra_ReportTransferTraineeEntity.FieldNames.E_SECTION);
            tb.Columns.Add(Tra_ReportTransferTraineeEntity.FieldNames.E_TEAM);
            tb.Columns.Add(Tra_ReportTransferTraineeEntity.FieldNames.E_UNIT);
            tb.Columns.Add(Tra_ReportTransferTraineeEntity.FieldNames.ClassName);
            tb.Columns.Add(Tra_ReportTransferTraineeEntity.FieldNames.ClassNameOld);
            tb.Columns.Add(Tra_ReportTransferTraineeEntity.FieldNames.CourseName);
            tb.Columns.Add(Tra_ReportTransferTraineeEntity.FieldNames.StartDate, typeof(DateTime));
            tb.Columns.Add(Tra_ReportTransferTraineeEntity.FieldNames.Month, typeof(DateTime));
            tb.Columns.Add(Tra_ReportTransferTraineeEntity.FieldNames.WorkPlace);
            return tb;
        }

        public DataTable GetReportTransferTrainee(string codeEmp, string profileName, string orgStructureID, Guid? courseID, Guid? classID, Guid? classOldID, bool IsCreateTemplate, string UserLogin)
        {
            DataTable table = CreateReportTransferTrainee();
            if (IsCreateTemplate)
            {
                return table;
            }
            string status = string.Empty;

            var classServices = new Tra_ClassServices();
            var traineeServices = new Tra_TraineeServices();
            var objTrainee = new List<object>();
            objTrainee.AddRange(new object[15]);
            objTrainee[9] = profileName;
            objTrainee[1] = codeEmp;
            objTrainee[8] = orgStructureID;
            objTrainee[13] = 1;
            objTrainee[14] = int.MaxValue - 1;

            var lstTrainee = traineeServices.GetData<Tra_TraineeEntity>(objTrainee, ConstantSql.hrm_tra_sp_get_Trainee, UserLogin, ref status).Where(s => s.ClassOldID != null).ToList();

            if (courseID != null)
            {
                lstTrainee = lstTrainee.Where(s => s.CourseID != null && s.CourseID == courseID).ToList();
            }

            if (classID != null)
            {
                lstTrainee = lstTrainee.Where(s => s.ClassID != null && s.ClassID == classID).ToList();
            }

            if (classOldID != null)
            {
                lstTrainee = lstTrainee.Where(s => s.ClassOldID != null && s.ClassOldID == classOldID).ToList();
            }

            if (lstTrainee == null || lstTrainee.Count == 0)
            {
                return table;
            }

            foreach (var trainee in lstTrainee)
            {
                DataRow dr = table.NewRow();
                dr[Tra_ReportTransferTraineeEntity.FieldNames.CodeEmp] = trainee.CodeEmp;
                dr[Tra_ReportTransferTraineeEntity.FieldNames.ProfileName] = trainee.ProfileName;
                dr[Tra_ReportTransferTraineeEntity.FieldNames.E_DEPARTMENT] = trainee.E_DEPARTMENT;
                dr[Tra_ReportTransferTraineeEntity.FieldNames.E_DIVISION] = trainee.E_DIVISION;
                dr[Tra_ReportTransferTraineeEntity.FieldNames.E_SECTION] = trainee.E_SECTION;
                dr[Tra_ReportTransferTraineeEntity.FieldNames.E_TEAM] = trainee.E_TEAM;
                dr[Tra_ReportTransferTraineeEntity.FieldNames.E_UNIT] = trainee.E_UNIT;
                dr[Tra_ReportTransferTraineeEntity.FieldNames.ClassName] = trainee.ClassName;
                dr[Tra_ReportTransferTraineeEntity.FieldNames.ClassNameOld] = trainee.ClassNameOld;
                dr[Tra_ReportTransferTraineeEntity.FieldNames.CourseName] = trainee.CourseName;
                dr[Tra_ReportTraineeByMonthEntity.FieldNames.WorkPlace] = trainee.WorkPlaceName;
                table.Rows.Add(dr);
            }

            return table;
        }

        #endregion

        #region BC Học Viên Không Đủ Tiêu Chuẩn Đào Tạo
        public DataTable CreateTraineeNotInRequirementDetailSchema()
        {
            DataTable tb = new DataTable("Tra_ReportTraineeNotInRequirementDetailEntity");

            tb.Columns.Add(Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.ID);
            tb.Columns.Add(Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.STT);
            tb.Columns.Add(Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.ProfileName);
            tb.Columns.Add(Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.E_DEPARTMENT);
            tb.Columns.Add(Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.E_DIVISION);
            tb.Columns.Add(Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.E_SECTION);
            tb.Columns.Add(Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.E_TEAM);
            tb.Columns.Add(Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.E_UNIT);
            tb.Columns.Add(Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.ClassName);
            tb.Columns.Add(Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.CourseName);
            tb.Columns.Add(Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.Notes);
            return tb;

        }

        public DataTable GetReportTraineeNotInRequirementDetail(DateTime? Month, string OrderNumber, Guid[] classID, bool IsCreateTemplate, Guid[] courseID, string UserLogin)
        {
            DataTable table = CreateTraineeNotInRequirementDetailSchema();
            if (IsCreateTemplate)
            {
                return table;
            }
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var profileServices = new Hre_ProfileServices();
                //var objProfile = new List<object>();
                //objProfile.Add(OrderNumber);
                //objProfile.Add(null);
                //objProfile.Add(null);
                //var lstProfile = profileServices.GetData<Hre_ProfileEntity>(objProfile, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, ref status).ToList();

                var service = new BaseService();
                List<object> lstObj = new List<object>();
                lstObj.Add(OrderNumber);
                lstObj.Add(null);
                lstObj.Add(null);
                var lstProfileidEntity = service.GetData<Hre_ProfileIdEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrgN, UserLogin, ref status);
                List<Guid> lstProfileID = null;
                if (lstProfileidEntity != null)
                {
                    lstProfileID = lstProfileidEntity.Select(s => s.ID).ToList();
                }

                var classServices = new Tra_ClassServices();
                var objClass = new List<object>();
                objClass.Add(null);
                objClass.Add(null);
                objClass.Add(null);
                objClass.Add(null);
                objClass.Add(null);
                objClass.Add(null);
                objClass.Add(null);
                objClass.Add(null);
                objClass.Add(1);
                objClass.Add(int.MaxValue - 1);
                var lstClass = new List<Tra_ClassEntity>();


                if (Month != null)
                {
                    lstClass = classServices.GetData<Tra_ClassEntity>(objClass, ConstantSql.hrm_tra_sp_get_Class, UserLogin, ref status).Where(s => s.StartDate != null && s.StartDate.Value.Month == Month.Value.Month && s.StartDate.Value.Year == Month.Value.Year).ToList();
                }
                else
                {
                    lstClass = classServices.GetData<Tra_ClassEntity>(objClass, ConstantSql.hrm_tra_sp_get_Class, UserLogin, ref status).ToList();
                }

                if (classID != null)
                {
                    lstClass = lstClass.Where(s => classID.Contains(s.ID)).ToList();
                }
                if (courseID != null)
                {
                    lstClass = lstClass.Where(s => courseID.Contains(s.CourseID)).ToList();
                }
                //if (RequirementTrainIDs != null)
                //{
                //    lstClass = lstClass.Where(s => s.RequirementTrainID!=null && courseID.Contains((Guid)s.RequirementTrainID)).ToList();
                //}
                var lstClassIDs = lstClass.Select(s => s.ID).ToList();

                var requirementDetailServices = new Tra_RequirementTrainDetailServices();
                var objRequirementDetail = new List<object>();
                objRequirementDetail.Add(null);
                objRequirementDetail.Add(1);
                objRequirementDetail.Add(int.MaxValue - 1);
                var lstRequirementDetail = requirementDetailServices.GetData<Tra_RequirementTrainDetailEntity>(objRequirementDetail, ConstantSql.hrm_tra_sp_get_RequirementDetail, UserLogin, ref status).ToList();

                var traineeServices = new Tra_TraineeServices();
                var objTrainee = new List<object>();
                objTrainee.AddRange(new object[15]);
                objTrainee[13] = 1;
                objTrainee[14] = int.MaxValue - 1;
                var lstTraniee = traineeServices.GetData<Tra_TraineeEntity>(objTrainee, ConstantSql.hrm_tra_sp_get_Trainee, UserLogin, ref status).ToList();
                if (lstProfileID != null)
                {
                    lstTraniee = lstTraniee.Where(s => lstProfileID.Contains(s.ProfileID)).ToList();
                }
                if (lstClassIDs != null)
                {
                    lstTraniee = lstTraniee.Where(s => lstClassIDs.Contains(s.ClassID)).ToList();
                }


                var lstTraineeID = lstTraniee.Select(s => s.ProfileID).ToList();
                var stt = 1;
                #region lấy Org và OrgType

                //var orgServices = new Cat_OrgStructureServices();
                //var lstObjOrg = new List<object>();
                //lstObjOrg.Add(null);
                //lstObjOrg.Add(null);
                //lstObjOrg.Add(null);
                //lstObjOrg.Add(1);
                //lstObjOrg.Add(int.MaxValue - 1);
                //var lstOrg = orgServices.GetData<Cat_OrgStructureEntity>(lstObjOrg, ConstantSql.hrm_cat_sp_get_OrgStructure, ref status).ToList();

                //var orgTypeService = new Cat_OrgStructureTypeServices();
                //var lstObjOrgType = new List<object>();
                //lstObjOrgType.Add(null);
                //lstObjOrgType.Add(null);
                //lstObjOrgType.Add(1);
                //lstObjOrgType.Add(int.MaxValue - 1);
                //var lstOrgType = orgTypeService.GetData<Cat_OrgStructureTypeEntity>(lstObjOrgType, ConstantSql.hrm_cat_sp_get_OrgStructureType, ref status).ToList();
                #endregion

                foreach (var item in lstTraniee)
                {
                    DataRow dr = table.NewRow();
                    var traineeNotInRequirementEntity = lstRequirementDetail.Where(s => item.ProfileID == s.ProfileID.Value).FirstOrDefault();
                    //var _objProfile = lstProfile.Where(s => s.ID == item.ProfileID).FirstOrDefault();
                    //Guid? orgId = item.OrgStructureID;
                    //var org = lstOrg.FirstOrDefault(s => s.ID == item.OrgStructureID);
                    //var E_DEPARTMENT = LibraryService.GetNearestParentEntity(orgId, OrgUnit.E_DEPARTMENT, lstOrg, lstOrgType);
                    if (traineeNotInRequirementEntity == null)
                    {
                        dr[Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.STT] = stt++;
                        dr[Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.CodeEmp] = item.CodeEmp;
                        dr[Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.ProfileName] = item.ProfileName;
                        //dr[Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.SectionName] = E_DEPARTMENT != null ? E_DEPARTMENT.OrgStructureName : null;
                        dr[Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.E_DEPARTMENT] = item.E_DEPARTMENT;
                        dr[Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.E_DIVISION] = item.E_DIVISION;
                        dr[Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.E_SECTION] = item.E_SECTION;
                        dr[Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.E_TEAM] = item.E_TEAM;
                        dr[Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.E_UNIT] = item.E_UNIT;
                        dr[Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.ClassName] = item.ClassName;
                        dr[Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.CourseName] = item.CourseName;
                        dr[Tra_ReportTraineeNotInRequirementDetailEntity.FieldNames.Notes] = traineeNotInRequirementEntity == null ? string.Empty : traineeNotInRequirementEntity.Comment;
                        table.Rows.Add(dr);
                    }
                }
                return table;
            }
        }

        #endregion




        #region Tra_ReportClass
        public DataTable CreateReportClassScheme()
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable tb = new DataTable("Tra_ReportClassEntity");
                tb.Columns.Add(Tra_ReportClassEntity.FieldNames.Code, typeof(string));
                tb.Columns.Add(Tra_ReportClassEntity.FieldNames.ClassName, typeof(string));
                tb.Columns.Add(Tra_ReportClassEntity.FieldNames.StartDate, typeof(DateTime));
                tb.Columns.Add(Tra_ReportClassEntity.FieldNames.EndDate, typeof(DateTime));
                tb.Columns.Add(Tra_ReportClassEntity.FieldNames.ScheduleTimeStart, typeof(DateTime));
                tb.Columns.Add(Tra_ReportClassEntity.FieldNames.ScheduleTimeEnd, typeof(DateTime));
                tb.Columns.Add(Tra_ReportClassEntity.FieldNames.TrainingPlace, typeof(string));
                tb.Columns.Add(Tra_ReportClassEntity.FieldNames.Teacher, typeof(string));
                return tb;
            }
        }
        public DataTable GetReportClass(List<Tra_ClassEntity> lstClass, Guid? _classID, bool isCreateTemplate)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var unitOfWork = (UnitOfWork)(new UnitOfWork(context));
                DataTable table = CreateReportClassScheme();
                if (isCreateTemplate)
                    return table;
                if (lstClass == null)
                    return table;
                if (_classID != null)
                    lstClass = lstClass.Where(s => s.ID == _classID).ToList();
                if (lstClass.Count == 0)
                    return table;
                List<object> paraProfile = new List<object>();
                paraProfile.AddRange(new object[17]);
                paraProfile[15] = 1;
                paraProfile[16] = int.MaxValue - 1;
                var hreServiceProfile = new Hre_ProfileServices();
                //var lstProfile = hreServiceProfile.GetData<Hre_ProfileEntity>(paraProfile, ConstantSql.hrm_hr_sp_get_ProfileAll, ref status);
                var lstProfile = hreServiceProfile.GetProfileNameAll();
                foreach (var item in lstClass)
                {
                    string _teacher = string.Empty;
                    if (item.TrainerOtherList != null)
                    {
                        string[] arrCodeEmps = item.TrainerOtherList.ToString().Split(',');
                        for (int i = 0; i < arrCodeEmps.Length; i++)
                        {
                            var objprofile = lstProfile.Where(s => s.CodeEmp == arrCodeEmps[i]).FirstOrDefault();
                            if (objprofile != null && objprofile.ProfileName != null)
                                _teacher += objprofile.ProfileName + ", ";
                        }
                        _teacher = _teacher.Substring(0, _teacher.Length - 2);
                    }
                    DataRow row = table.NewRow();
                    if (item.Code != null)
                        row[Tra_ReportClassEntity.FieldNames.Code] = item.Code;
                    if (item.ClassName != null)
                        row[Tra_ReportClassEntity.FieldNames.ClassName] = item.ClassName;
                    if (item.StartDate != null)
                        row[Tra_ReportClassEntity.FieldNames.StartDate] = item.StartDate;
                    if (item.EndDate != null)
                        row[Tra_ReportClassEntity.FieldNames.EndDate] = item.EndDate;
                    if (item.ScheduleTimeStart != null)
                        row[Tra_ReportClassEntity.FieldNames.ScheduleTimeStart] = item.ScheduleTimeStart;
                    if (item.ScheduleTimeEnd != null)
                        row[Tra_ReportClassEntity.FieldNames.ScheduleTimeEnd] = item.ScheduleTimeEnd;
                    if (item.TrainingPlace != null)
                        row[Tra_ReportClassEntity.FieldNames.TrainingPlace] = item.TrainingPlace;
                    if (item.TrainerOtherList != null)
                        row[Tra_ReportClassEntity.FieldNames.Teacher] = _teacher;
                    table.Rows.Add(row);
                }
                return table.ConfigTable(true);
            }
        }
        #endregion

        #region BC HV Vắng Mặt
        public DataTable CreateReportTraineeUnAttendSchema()
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable tb = new DataTable("Tra_ReportTraineeUnAttendEntity");
                tb.Columns.Add(Tra_ReportTraineeUnAttendEntity.FieldNames.ID);
                tb.Columns.Add(Tra_ReportTraineeUnAttendEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Tra_ReportTraineeUnAttendEntity.FieldNames.ProfileName);
                tb.Columns.Add(Tra_ReportTraineeUnAttendEntity.FieldNames.E_DEPARTMENT);
                tb.Columns.Add(Tra_ReportTraineeUnAttendEntity.FieldNames.E_DIVISION);
                tb.Columns.Add(Tra_ReportTraineeUnAttendEntity.FieldNames.E_SECTION);
                tb.Columns.Add(Tra_ReportTraineeUnAttendEntity.FieldNames.E_TEAM);
                tb.Columns.Add(Tra_ReportTraineeUnAttendEntity.FieldNames.E_UNIT);
                tb.Columns.Add(Tra_ReportTraineeUnAttendEntity.FieldNames.ClassName);
                tb.Columns.Add(Tra_ReportTraineeUnAttendEntity.FieldNames.CourseName);
                tb.Columns.Add(Tra_ReportTraineeUnAttendEntity.FieldNames.StartDate, typeof(DateTime));
                tb.Columns.Add(Tra_ReportTraineeUnAttendEntity.FieldNames.Month, typeof(DateTime));
                tb.Columns.Add(Tra_ReportTraineeUnAttendEntity.FieldNames.WorkPlace);
                tb.Columns.Add(Tra_ReportTraineeUnAttendEntity.FieldNames.RequirementTrainName);
                return tb;
            }
        }

        public DataTable GetReportTraineeUnAttend(string codeEmp, string profileName, string orgStructureID, Guid[] courseID, Guid[] classID, DateTime? _StartDate, DateTime? _EndDate, bool IsCreateTemplate, Guid[] RequirementTrainIDs, string UserLogin)
        {
            string status = string.Empty;

            DataTable table = CreateReportTraineeUnAttendSchema();
            if (IsCreateTemplate)
            {
                return table;
            }

            var traineeServices = new Tra_TraineeServices();
            var objTrainee = new List<object>();
            objTrainee.AddRange(new object[15]);
            objTrainee[9] = profileName;
            objTrainee[1] = codeEmp;
            objTrainee[8] = orgStructureID;
            objTrainee[3] = DateTime.Now;
            objTrainee[4] = DateTime.MaxValue;
            objTrainee[13] = 1;
            objTrainee[14] = int.MaxValue - 1;

            var lstTrainee = traineeServices.GetData<Tra_TraineeEntity>(objTrainee, ConstantSql.hrm_tra_sp_get_Trainee, UserLogin, ref status).ToList();
            if (lstTrainee.Count > 0)
                lstTrainee = lstTrainee.Where(s => s.EndDate != null && s.EndDate.Value > DateTime.Now).ToList();
            if (_StartDate != null)
                lstTrainee = lstTrainee.Where(s => s.StartDate != null && s.StartDate.Value <= _StartDate.Value).ToList();
            if (_EndDate != null)
                lstTrainee = lstTrainee.Where(s => s.EndDate != null && s.EndDate.Value >= _EndDate.Value).ToList();
            if (courseID != null)
            {
                lstTrainee = lstTrainee.Where(s => courseID.Contains(s.CourseID)).ToList();
            }

            if (classID != null)
            {
                lstTrainee = lstTrainee.Where(s => classID.Contains(s.ClassID)).ToList();
            }

            var lisRequirementDetailByprofileID = new List<Guid>();

            var requirmentDetailServices = new Tra_RequirementTrainDetailServices();
            var objRequirementDetail = new List<object>();
            objRequirementDetail.Add(null);
            objRequirementDetail.Add(1);
            objRequirementDetail.Add(int.MaxValue - 1);
            var lstRequirementDetail = requirmentDetailServices.GetData<Tra_RequirementTrainDetailEntity>(objRequirementDetail, ConstantSql.hrm_tra_sp_get_RequirementDetail, UserLogin, ref status).ToList();

            var lstClassIDByTrainee = lstTrainee.Select(s => s.ClassID).ToList();
            var lstCourseId = lstTrainee.Select(s => s.CourseID).ToList();
            if (RequirementTrainIDs != null)
            {
                lstRequirementDetail = lstRequirementDetail.Where(s => s.RequirementTrainID != null && RequirementTrainIDs.Contains(s.RequirementTrainID.Value)).ToList();
               // lstRequirementDetail = lstRequirementDetail.Where(s => s.CourseID != null && s.ClassID != null && s.ProfileID != null && lisRequirementDetailByprofileID.Contains(s.ProfileID.Value) && lstClassIDByTrainee.Contains(s.ClassID.Value) && lstCourseId.Contains(s.CourseID.Value)).ToList();
                if (lstRequirementDetail.Count > 0)
                {
                    lisRequirementDetailByprofileID = lstRequirementDetail.Where(s => s.ProfileID != null).Select(s => (Guid)s.ProfileID).ToList();                   
                }
            }         

            if(lisRequirementDetailByprofileID.Count > 0)
            {
               // lstTrainee = lstTrainee.Where(s => lisRequirementDetailByprofileID.Contains(s.ProfileID)).ToList();

                lisRequirementDetailByprofileID = lstRequirementDetail.Where(s => s.CourseID != null && s.ClassID != null && s.ProfileID != null && lisRequirementDetailByprofileID.Contains(s.ProfileID.Value) && lstClassIDByTrainee.Contains(s.ClassID.Value) && lstCourseId.Contains(s.CourseID.Value)).Select(s => (Guid)s.ProfileID).ToList();
            }
            var lstTraineeID = lstTrainee.Select(s => s.ID).ToList();


            var traineeTopicServices = new Tra_TraineeTopicServices();
            var objTraineeTopic = new List<object>();
            objTraineeTopic.Add(1);
            objTraineeTopic.Add(int.MaxValue - 1);
            var lstTraineeTopic = traineeTopicServices.GetData<Tra_TraineeTopicEntity>(objTraineeTopic, ConstantSql.hrm_tra_sp_get_TraineeTopic, UserLogin, ref status).ToList();

            lstTraineeTopic = lstTraineeTopic.Where(s => s.TraineeID != null && lstTraineeID.Contains(s.TraineeID.Value)).ToList();
            var lstTraineeTopicID = lstTraineeTopic.Select(s => s.ID).ToList();

            var traineeScoreService = new Tra_TraineeScoreServices();
            var objTraineeScore = new List<object>();
            objTraineeScore.Add(1);
            objTraineeScore.Add(int.MaxValue);
            var lstTraineeScore = traineeScoreService.GetData<Tra_TraineeScoreEntity>(objTraineeScore, ConstantSql.hrm_tra_sp_get_TraineeScore, UserLogin, ref status).ToList();

            
           
            
            lstTraineeScore = lstTraineeScore.Where(s => s.TraineeTopicID != null && lstTraineeTopicID.Contains(s.TraineeTopicID.Value) && s.TraineeID != null && s.Score != null).ToList();

            foreach (var item in lstTrainee)
            {
                DataRow dr = table.NewRow();
                var lstTraineeScoreByTraineeID = lstTraineeScore.Where(s => item.ID == s.TraineeID.Value).ToList();
                if (lstTraineeScoreByTraineeID.Count == 0)
                {
                    dr[Tra_ReportTraineeUnAttendEntity.FieldNames.CodeEmp] = item.CodeEmp;
                    dr[Tra_ReportTraineeUnAttendEntity.FieldNames.ProfileName] = item.ProfileName;
                    dr[Tra_ReportTraineeUnAttendEntity.FieldNames.E_DEPARTMENT] = item.E_DEPARTMENT;
                    dr[Tra_ReportTraineeUnAttendEntity.FieldNames.E_DIVISION] = item.E_DIVISION;
                    dr[Tra_ReportTraineeUnAttendEntity.FieldNames.E_SECTION] = item.E_SECTION;
                    dr[Tra_ReportTraineeUnAttendEntity.FieldNames.E_TEAM] = item.E_TEAM;
                    dr[Tra_ReportTraineeUnAttendEntity.FieldNames.E_UNIT] = item.E_UNIT;
                    dr[Tra_ReportTraineeUnAttendEntity.FieldNames.ClassName] = item.ClassName;
                    dr[Tra_ReportTraineeUnAttendEntity.FieldNames.CourseName] = item.CourseName;
                    dr[Tra_ReportTraineeUnAttendEntity.FieldNames.WorkPlace] = item.WorkPlaceName;
                    dr[Tra_ReportTraineeUnAttendEntity.FieldNames.RequirementTrainName] = item.RequirementTrainName;
                    table.Rows.Add(dr);
                }
            }

            return table;
        }
        #endregion
    }
}
