using HRM.Business.Hr.Domain;
using HRM.Business.Hr.Models;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System;
using System.Collections;
using System.Linq;
using HRM.Business.Category.Domain;
using HRM.Business.Category.Models;
using HRM.Business.Insurance.Models;


namespace HRM.Business.Insurance.Domain
{
    public class Ins_InsuranceRecordServices : BaseService
    {

        public DataTable GetReportNotHaveSocialSchema() {
            DataTable tb = new DataTable("Ins_ReportNotHaveSocialModel");
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.ID,typeof(Guid));
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.ProfileName);
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.E_DEPARTMENT);
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.E_DIVISION);
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.E_SECTION);
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.E_TEAM);
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.E_UNIT);
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.PositionName);
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.JobTitleName);
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.EthnicGroupName);
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.DateHire,typeof(DateTime));
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.DateQuit, typeof(DateTime));
       
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.HealthTreatmentPlace);
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.HealthTreatmentPlaceCode);
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.PlaceOfTreatmentProvince);
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.Origin);
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.IDNo);
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.DateOfBirth, typeof(DateTime));
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.IDPlaceOfIssue);
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.IDDateOfIssue);
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.PAddress);
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.TAddress);
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.IDPOI_Code);
            tb.Columns.Add(Ins_ReportNotHaveSocialEntity.FieldNames.IDPOI);
            
            return tb;
        }
        public DataTable GetReportNotHaveSocial(DateTime? dateFrom, DateTime? dateTo, string OrgStructureID,string userLogin) 
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;

                //string type_COLLECT_SOCIAL_INSURANCE = AppConfig.E_COLLECT_SOCIAL_INSURANCE.ToString();
                //bool _e_PROBATION_NoPay_INSURANCE = false;


                var profileServices = new Hre_ProfileServices();
                DataTable table = GetReportNotHaveSocialSchema();
                var lstObjProfile = new List<object>();
                lstObjProfile.Add(OrgStructureID);
                lstObjProfile.Add(null);
                lstObjProfile.Add(null);
                var lstProfile = profileServices.GetData<Hre_ProfileEntity>(lstObjProfile, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, userLogin, ref status).ToList();

                var gradePayrollService = new Cat_GradePayrollServices();
                var lstObjGradePayroll = new List<object>();
                lstObjGradePayroll.Add(null);
                lstObjGradePayroll.Add(null);
                lstObjGradePayroll.Add(1);
                lstObjGradePayroll.Add(int.MaxValue -1);

                var contractServices = new Hre_ContractServices();
                var lstObjContract = new List<object>();
                lstObjContract.AddRange(new object[15]);
                lstObjContract[13] = 1;
                lstObjContract[14] = int.MaxValue -1;


                var provinceServices = new Cat_ProvinceServices();
                var lstObjProvince = new List<object>();
                lstObjProvince.Add(null);
                lstObjProvince.Add(null);
                lstObjProvince.Add(null);
                lstObjProvince.Add(null);
                lstObjProvince.Add(null);
                lstObjProvince.Add(null);
                var lstProvince = provinceServices.GetData<Cat_ProvinceEntity>(lstObjProvince, ConstantSql.hrm_cat_sp_get_Province, userLogin, ref status).ToList();

                foreach (var item in lstProfile)
                {
                    DataRow dr = table.NewRow();
                    dr[Ins_ReportNotHaveSocialEntity.FieldNames.ID] = item.ID;
                    dr[Ins_ReportNotHaveSocialEntity.FieldNames.CodeEmp] = item.CodeEmp == null ? string.Empty : item.CodeEmp;
                    dr[Ins_ReportNotHaveSocialEntity.FieldNames.ProfileName] = item.ProfileName == null ? string.Empty : item.ProfileName;
                    dr[Ins_ReportNotHaveSocialEntity.FieldNames.E_DEPARTMENT] = item.OrgStructureName == null ? string.Empty : item.E_DEPARTMENT;
                    dr[Ins_ReportNotHaveSocialEntity.FieldNames.E_DIVISION] = item.OrgStructureName == null ? string.Empty : item.E_DIVISION;
                    dr[Ins_ReportNotHaveSocialEntity.FieldNames.E_SECTION] = item.OrgStructureName == null ? string.Empty : item.E_SECTION;
                    dr[Ins_ReportNotHaveSocialEntity.FieldNames.E_TEAM] = item.OrgStructureName == null ? string.Empty : item.E_TEAM;
                    dr[Ins_ReportNotHaveSocialEntity.FieldNames.E_UNIT] = item.OrgStructureName == null ? string.Empty : item.E_UNIT;
                    dr[Ins_ReportNotHaveSocialEntity.FieldNames.PositionName] = item.PositionName == null ? string.Empty : item.PositionName;
                    dr[Ins_ReportNotHaveSocialEntity.FieldNames.JobTitleName] = item.JobTitleName == null ? string.Empty : item.JobTitleName;
                    dr[Ins_ReportNotHaveSocialEntity.FieldNames.EthnicGroupName] = item.EthnicGroupName == null ? string.Empty : item.EthnicGroupName;
                    
                    dr[Ins_ReportNotHaveSocialEntity.FieldNames.HealthTreatmentPlace] = item.HealthTreatmentPlace == null ? string.Empty : item.HealthTreatmentPlace;
                    dr[Ins_ReportNotHaveSocialEntity.FieldNames.HealthTreatmentPlaceCode] = item.HealthTreatmentPlaceCode == null ? string.Empty : item.HealthTreatmentPlaceCode;

                    
                    var province = lstProvince.Where(s => s.ID == item.PlaceOfIssueID).FirstOrDefault();
                    if(province != null)
                    {
                        dr[Ins_ReportNotHaveSocialEntity.FieldNames.IDPOI_Code] = province.Code == null ? string.Empty : province.Code;
                        dr[Ins_ReportNotHaveSocialEntity.FieldNames.IDPOI] = province.ProvinceName == null ? string.Empty : province.ProvinceName;
                    }
                    dr[Ins_ReportNotHaveSocialEntity.FieldNames.Origin] = item.Origin == null ? string.Empty : item.Origin;
                    dr[Ins_ReportNotHaveSocialEntity.FieldNames.PAddress] = item.PAddress == null ? string.Empty : item.PAddress;
                    dr[Ins_ReportNotHaveSocialEntity.FieldNames.TAddress] = item.TAddress == null ? string.Empty : item.TAddress;
                    
                    if (item.DateHire != null)
                    {
                        dr[Ins_ReportNotHaveSocialEntity.FieldNames.DateHire] = item.DateHire.Value;
                    }
                    if (item.DateQuit != null)
                    {
                        dr[Ins_ReportNotHaveSocialEntity.FieldNames.DateHire] = item.DateQuit.Value;
                    }
                    if (item.DateOfBirth != null)
                    {
                        dr[Ins_ReportNotHaveSocialEntity.FieldNames.DateOfBirth] = item.DateOfBirth.Value;
                    }

                    var provinceTreatment = lstProvince.Where(s => s.ID == item.ProvinceInsuranceID).FirstOrDefault();
                    if (provinceTreatment != null)
                    {
                        dr[Ins_ReportNotHaveSocialEntity.FieldNames.PlaceOfTreatmentProvince] = provinceTreatment.Code == null ? string.Empty : provinceTreatment.Code;
                    }
                    table.Rows.Add(dr);

                }
                return table;
            }
            
        }
    }
}
