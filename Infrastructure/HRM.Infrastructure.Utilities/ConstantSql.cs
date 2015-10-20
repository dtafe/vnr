namespace HRM.Infrastructure.Utilities
{
    /// <summary>
    /// Quy tắc đặt tên store và tên hàm
    /// TenProject_TenModule_Loai(sp hoặc fn)_Action(get, add, edit, delete...)_Tên
    /// vd: hrm_hr_sp_get_Profile
    /// </summary>
    public class ConstantSql
    {

        #region Stored Procedures

        #region Dashboard
        /// <summary> [Hieu.Van] My Information </summary> 
        public const string hrm_dashboard_sp_get_InformationByUserID = "exec hrm_dashboard_sp_get_InformationByUserID @userId";

        /// <summary> [Hieu.Van] Tồng Số hợp đồng đến hạn </summary>
        public const string hrm_dashboard_sp_get_AlertsForContract = "exec hrm_dashboard_sp_get_AlertsForContract";

        /// <summary> [Hieu.Van] Tồng Số hợp đồng thừ việc đến hạn </summary>
        public const string hrm_dashboard_sp_get_AlertsForProbation = "exec hrm_dashboard_sp_get_AlertsForProbation";

        /// <summary> [Hieu.Van] Tồng Số sinh nhật gần đến </summary>
        public const string hrm_dashboard_sp_get_AlertsForBirthDay = "exec hrm_dashboard_sp_get_AlertsForBirthDay";
        public static string hrm_att_sp_get_DashboardLeaves
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_ATT_SP_GET_DASHBOARDLEAVES";
                }
                else
                {
                    return "exec hrm_att_sp_get_DashboardLeaves";
                }
            }
        }
        public static string hrm_att_getdata_AnnualDetail
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_GETDATA_ANNUALDETAIL";
                }
                else
                {
                    return "exec hrm_att_getdata_AnnualDetail";
                }
            }
        }
        #endregion

        public static string hrm_rec_sp_get_TagByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "";
                }
                else
                {
                    return "exec hrm_rec_sp_get_TagByIds";
                }
            }
        }
        #region Category
        public static string hrm_cat_getdata_DayOff
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_GETDATA_DAYOFF";
                }
                else
                {
                    return "exec hrm_cat_getdata_DayOff";
                }
            }
        }
        public static string hrm_cat_sp_get_Region
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_REGION";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Region";
                }
            }
        }

        public static string hrm_cat_sp_get_RegionById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_REGIONBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_RegionById @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_WorkPlace
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_WORKPLACE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_workplace";
                }
            }
        }
        public static string hrm_cat_sp_get_WorkPlaceById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_WORKPLACEBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_WorkPlaceById @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_WorkPlace_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_WORKPLACEMULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_workplace_multi";
                }
            }
        }
        public static string hrm_cat_sp_get_Supplier_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SUPPLIERMULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Supplier_Multi";
                }
            }
        }

        public static string hrm_cat_sp_get_PruchaseItems_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PURCHASEITEMMULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_PurchaseItems_Multi";
                }
            }
        }

        public static string hrm_cat_sp_get_SalaryClassType_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SALARYTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SalaryClassType_Multi";
                }
            }
        }

        public static string hrm_cat_sp_get_Qualification_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_QUALIFICATION_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Qualification_Multi";
                }
            }
        }
        public static string hrm_cat_sp_get_QualificationLevel_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_QUALIFILEVEL_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_QualificationLevel_Multi";
                }
            }
        }
        public static string hrm_cat_sp_get_Religion
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_RELIGION";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Religion";
                }
            }
        }
        public static string hrm_cat_sp_get_ReligionById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_RELIGIONBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ReligionById @Keyword";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách dữ liệu theo country id
        /// </summary>       
        public static string hrm_cat_sp_get_ProvinceByCountryId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PROVINCEBYCOUNTRYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ProvinceByCountryId @Id";
                }
            }
        }
        public static string hrm_tra_sp_get_CertificateByCourseID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_CERTIFIBYCOURSEID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_CertificateByCourseID @Id";
                }
            }

        }
        public static string hrm_cat_sp_get_VillageByDistrictId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_VILLAGEBYDISTRICTID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_VillageByDistrictId @Id";
                }
            }
        }


        public static string hrm_tra_sp_get_TopicByCourseID_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_TOPICBYCOURSEID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TopicByCourseID_Multi @Id";
                }
            }
        }
        public static string hrm_cat_sp_get_BudgetByFunctionId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_BUDGETBYFUNCTIONID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_BudgetByFunctionId @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_ChannelByBudgetOwnerId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_CHANNELBYBUDGETID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ChannelByBudgetOwnerId @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_JobTypeByRoleId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_JOBTYPEBYROLEID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_JobTypeByRoleId @Id";
                }
            }
        }

        /// <summary>
        /// Lấy danh sách dữ liệu theo province id
        /// </summary>        
        public static string hrm_cat_sp_get_DisctrictByProvinceId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_DISCBYPROVINCEID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_DisctrictByProvinceId @Id";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_InOut
        /// </summary>
        //public const string hrm_cat_sp_get_Province = "exec hrm_cat_sp_get_Province";
        public static string hrm_cat_sp_get_Province
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_PROVINCE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Province";
                }
            }
        }

        public static string hrm_cat_sp_get_ProvinceById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PROVINCEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ProvinceById @Id";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách dữ liệu Cat_ConditionalColor
        /// </summary>
        //public const string hrm_cat_sp_get_conditioncolor = "exec hrm_cat_sp_get_conditioncolor";
        public static string hrm_cat_sp_get_conditioncolor
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_CONDITIONALCOLOR";
                }
                else
                {
                    return "exec hrm_cat_sp_get_conditioncolor";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách dữ liệu CatProduct
        /// </summary>
        //public const string hrm_cat_sp_get_Product = "exec hrm_cat_sp_get_Product";
        public static string hrm_cat_sp_get_Product
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_PRODUCT";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Product";
                }
            }
        }

        public static string hrm_cat_sp_get_ProductItem_All
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_PRODUCTITEM_ALL";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ProductItem_All";
                }
            }
        }


        public static string hrm_cat_sp_get_ProductItem
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_PRODUCTITEM";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ProductItem";
                }
            }
        }
        public static string hrm_cat_sp_get_ProductItemByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PRODUCTITEMBYIDS(:id,:R_Output);end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ProductItemByIds @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_ProductItem_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PRODUCTITEMMULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_Cat_sp_get_ProductItem_multi @Keyword";
                }
            }
        }


        public static string hrm_cat_sp_get_ProductItemById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PRODUCTITEMBYID(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ProductItemById @ID";
                }
            }
        }

        public static string hrm_hr_sp_set_UpdateNoPrint
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_SET_UPDATENOPRINT(:Ids); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_set_UpdateNoPrint @ID";
                }
            }
        }

        /// <summary>
        /// Lấy danh sách chọn dữ liệu CatProduct
        /// </summary>
        //public const string hrm_cat_sp_get_ProductByIds = "exec hrm_cat_sp_get_ProductByIds";
        public static string hrm_cat_sp_get_ProductByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PRODUCTBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ProductByIds @Ids";
                }
            }
        }
        /// <summary>
        /// Lấy 1 đối tượng dữ liệu  Cat_ConditionalColor
        /// </summary>
        //public const string hrm_cat_sp_get_Cat_ConditionalColorById = "exec hrm_cat_sp_get_Cat_ConditionalColorById";
        public static string hrm_cat_sp_get_Cat_ConditionalColorById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_CONDITIONCOLORBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Cat_ConditionalColorById @Id";
                }
            }
        }
        /// <summary>
        /// Lấy 1 đối tượng dữ liệu Cat_AppendixContractType
        /// </summary>
        //public const string hrm_cat_sp_get_AppendixContractTypeById = "exec hrm_cat_sp_get_AppendixContractTypeById";
        public static string hrm_cat_sp_get_AppendixContractTypeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ACONTRACTTYPEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AppendixContractTypeById @Id";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách dữ liệu Cat_AppendixContractType
        /// </summary>
        //public const string hrm_cat_sp_get_AppendixContractType = "exec hrm_cat_sp_get_AppendixContractType";
        public static string hrm_cat_sp_get_AppendixContractType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ACONTRACTTYPE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AppendixContractType";
                }
            }
        }
        public static string hrm_cat_sp_get_AppendixContractTypeByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ACONTRACTTYPEBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AppendixContractTypeByIds @Ids";
                }
            }
        }

        /// <summary>
        /// Lấy 1 đối tượng dữ liệu CatRewardedType
        /// </summary>
        //public const string hrm_cat_sp_get_RewardedTypeById = "exec hrm_cat_sp_get_RewardedTypeById";
        public static string hrm_cat_sp_get_RewardedTypeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_REWAREDTYPEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_RewardedTypeById @Id";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách dữ liệu CatRewardedType
        /// </summary>
        //public const string hrm_cat_sp_get_RewardedType = "exec hrm_cat_sp_get_RewardedType";
        public static string hrm_cat_sp_get_RewardedType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_REWARDEDTYPE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_RewardedType";
                }
            }
        }
        public static string hrm_cat_sp_get_RewardedTypeByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_REWARDEDTYPEBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_RewardedTypeByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_RewardType_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_REWARDEDTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_RewardType_multi @Keyword";
                }
            }
        }
        /// <summary>
        /// Lấy 1 đối tượng dữ liệu CatQualification
        /// </summary>
        //public const string hrm_cat_sp_get_QualificationById = "exec hrm_cat_sp_get_QualificationById";
        public static string hrm_cat_sp_get_QualificationById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_QUALIFICATIONBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_QualificationById @Id";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách dữ liệu CatQualification
        /// </summary>
        //public const string hrm_cat_sp_get_Qualification = "exec hrm_cat_sp_get_Qualification";
        public static string hrm_cat_sp_get_Qualification
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_QUALIFICATION";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Qualification";
                }
            }
        }
        public static string hrm_cat_sp_get_QualificationByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_QUALIFICATIONBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_QualificationByIds @Ids";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách dữ liệu  CatNameEntity loại trình độ học vấn
        /// </summary>
        //public const string hrm_cat_sp_get_EducationLevel = "exec hrm_cat_sp_get_EducationLevel";
        public static string hrm_cat_sp_get_EducationLevel
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_EDUCATIONLEVEL";
                }
                else
                {
                    return "exec hrm_cat_sp_get_EducationLevel";
                }
            }
        }

        public static string hrm_cat_sp_get_TypeOfTransfer
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_TYPEOFTRANSFER";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TypeOfTransfer";
                }
            }
        }

        public static string hrm_cat_sp_get_DisciplineReason
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_DISCIPLINEREASON";
                }
                else
                {
                    return "exec hrm_cat_sp_get_DisciplineReason";
                }
            }
        }

        public static string hrm_cat_sp_get_EducationLevelByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_EDUCATIONLEVELBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_EducationLevelByIds @Ids";
                }
            }
        }
        #region Cat_ReasonDeny
        public static string hrm_cat_sp_get_ResonDeny
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_RESONDENY";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ResonDeny";
                }
            }
        }
        public static string hrm_cat_sp_get_ResonDenyByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_RESONDENYBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ResonDenyByIds @Ids";
                }
            }
        }
        #endregion


        #region Trình độ văn hóa

        public static string hrm_cat_sp_get_GraduatedLevel
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_GRADUATEDLEVEL";
                }
                else
                {
                    return "exec hrm_cat_sp_get_GraduatedLevel";
                }
            }
        }
        public static string hrm_cat_sp_get_GraduatedLevelByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_GRADUATEDLEVELBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_GraduatedLevelByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_conditioncolorByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_CONDCOLORBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_conditioncolorByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_GraduatedLevel_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_GRADUATED_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_GraduatedLevel_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_LockObject_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_LOCKOBJECT_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_LockObject_Multi @Keyword";
                }
            }
        }
        #endregion

        /// <summary>
        /// Lấy danh sách dữ liệu  CatNameEntity theo loại trình độ 
        /// </summary>
        //public const string hrm_cat_sp_get_EducationLevel = "exec hrm_cat_sp_get_EducationLevel";
        public static string hrm_cat_sp_get_LevelGeneral
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_LEVELGENERAL";
                }
                else
                {
                    return "exec hrm_cat_sp_get_LevelGeneral";
                }
            }

        }
        
        public static string hrm_cat_sp_get_LockObjectByIDs
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    
                    return "begin CAT_SP_GET_LOCKOBJECTBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_CAT_SP_GET_LOCKOBJECTBYIDS @Ids";
                }
            }
        }
        public static string hrm_cat_sp_get_LockObject
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_LOCKOBJECT";
                }
                else
                {
                    return "exec hrm_CAT_SP_GET_LOCKOBJECT";
                }
            }

        }

        public static string hrm_cat_sp_get_CutOffDurationType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_CUTOFFDURATIONTYPE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CutOffDurationType";
                }
            }
        }

        public static string hrm_cat_sp_get_CutOffDurationTypeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_CUTOFFTYPEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CutOffDurationTypeById @Id";
                }
            }
        }
        public static string hrm_cat_sp_get_LevelGeneralByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_LEVELGENERALBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_LevelGeneralByIds @Ids";
                }
            }
        }
        /// <summary>
        /// Lấy 1 đối tượng dữ liệu CatSalaryClassType
        /// </summary>
        //public const string hrm_cat_sp_get_SalaryClassTypeById = "exec hrm_cat_sp_get_SalaryClassTypeById";
        public static string hrm_cat_sp_get_SalaryClassTypeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SALARYCLASSTYPEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SalaryClassTypeById @Id";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách dữ liệu CatSalaryClassType
        /// </summary>
        //public const string hrm_cat_sp_get_SalaryClassType = "exec hrm_cat_sp_get_SalaryClassType";
        public static string hrm_cat_sp_get_SalaryClassType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_SALARYCLASSTYPE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SalaryClassType";
                }
            }
        }
        public static string hrm_cat_sp_get_SalaryClassTypeByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SALARYCLTYPEBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SalaryClassTypeByIds @Ids";
                }
            }
        }
        /// <summary>
        /// Lấy 1 đối tượng dữ liệu Cat_PerformanceType
        /// </summary>
        //public const string hrm_cat_sp_get_PerformanceTypeById = "exec hrm_cat_sp_get_PerformanceTypeById";
        public static string hrm_cat_sp_get_PerformanceTypeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PERFORMANCETYPEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_PerformanceTypeById @Id";
                }
            }
        }
        //Tho.Bui: PerformanceType_multi
        public static string hrm_cat_sp_get_PerformanceType_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PERFORTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_performancetype_multi @Keyword";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách dữ liệu Cat_PerformanceType
        /// </summary>
        //public const string hrm_cat_sp_get_PerformanceType = "exec hrm_cat_sp_get_PerformanceType";
        public static string hrm_cat_sp_get_PerformanceType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_PERFORMANCETYPE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_PerformanceType";
                }
            }
        }
        public static string hrm_cat_sp_get_PerformanceTypeByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PERFORMTYPEBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_PerformanceTypeByIds @Ids";
                }
            }
        }

        /// <summary>
        /// Lấy 1 đối tượng dữ liệu CatSalaryClass
        /// </summary>
        //public const string hrm_cat_sp_get_SalaryClassById = "exec hrm_cat_sp_get_SalaryClassById";
        public static string hrm_cat_sp_get_SalaryClassById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SALARYCLASSBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SalaryClassById @Id";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách dữ liệu CatSalaryClass
        /// </summary>
        //public const string hrm_cat_sp_get_SalaryClass = "exec hrm_cat_sp_get_SalaryClass";
        public static string hrm_cat_sp_get_SalaryClass
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_SALARYCLASS";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SalaryClass";
                }
            }
        }
        public static string hrm_cat_sp_get_SalaryClassByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SALARYCLASSBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SalaryClassByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_SalaryClass_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SALCLASS_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SalaryClass_multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_SalaryJobGroup_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SALJOBGROUP_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SalaryJobGroup_multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_SalaryAdjCampaign_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SALADJCAM_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SalaryAdjCampaign_multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_SalaryClassType_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SALCLASSTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SalaryClassType_multi @Keyword";
                }
            }
        }
        /// <summary>
        /// Lấy ds đối tượng dữ liệu UsualAllowanceLevel theo AllowanceID
        /// </summary>
        //public const string hrm_cat_sp_get_UsualAllowanceLevelByAllowanceID = "exec hrm_cat_sp_get_UsualAllowanceLevelByAllowanceID";
        public static string hrm_cat_sp_get_UsualAllowanceLevelByAllowanceID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_UALEVELBYPALLOID";
                }
                else
                {
                    return "exec hrm_cat_sp_get_UsualAllowanceLevelByAllowanceID";
                }
            }
        }
        /// <summary>
        /// Lấy 1 đối tượng dữ liệu UsualAllowanceLevel
        /// </summary>
        //public const string hrm_cat_sp_get_UsualAllowanceLevelById = "exec hrm_cat_sp_get_UsualAllowanceLevelById";
        public static string hrm_cat_sp_get_UsualAllowanceLevelById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_UALEVELBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_UsualAllowanceLevelById @Id";
                }
            }
        }
        /// <summary>
        /// Lấy 1 đối tượng dữ liệu UsualAllowance
        /// </summary>
        //public const string hrm_cat_sp_get_UsualAllowanceById = "exec hrm_cat_sp_get_UsualAllowanceById";
        public static string hrm_cat_sp_get_UsualAllowanceById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_USUALALLOWANCEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_UsualAllowanceById @Id";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách dữ liệu UsualAllowance
        /// </summary>
        //public const string hrm_cat_sp_get_UsualAllowance = "exec hrm_cat_sp_get_UsualAllowance";
        public static string hrm_cat_sp_get_UsualAllowance
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_USUALALLOWANCE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_UsualAllowance";
                }
            }
        }
        public static string hrm_cat_sp_get_UsualAllowanceByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_USUALALLOWANCEBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_UsualAllowanceByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_UsualAllowance_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_USUALALLOW_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_UsualAllowance_Multi @Keyword";
                }
            }
        }


        /// <summary>
        /// [Tho.Bui]:Lấy danh sách dữ liệu CatSalaryrank
        /// </summary>
        public static string hrm_cat_sp_get_SalaryRank
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_SALARYRANK";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SalaryRank";
                }
            }
        }

        public static string hrm_cat_sp_get_SalaryRank_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SALARYRANK_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SalaryRank_Multi @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_SalaryRankClass_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_RYRANKCLASS_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SalaryRankClass_Multi @Keyword";
                }
            }
        }

        /// <summary>
        ///[Tho.Bui] Lấy 1 đối tượng dữ liệu CatSalaryRank by Id
        /// </summary>
        public static string hrm_cat_sp_get_SalaryRankById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SALARYRANKBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SalaryRankById @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_SalaryRankBySalaryClassId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SALRANKBYSALCLASSID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SalaryRankBySalaryClassId @Id";
                }
            }
        }
        /// <summary>
        /// [Tho.Bui]: Export select row
        /// </summary>
        public static string hrm_cat_sp_get_SalaryRankByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SALARYRANKBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SalaryRankByIds @Ids";
                }
            }
        }
        /// <summary>
        /// Lấy 1 đối tượng dữ liệu CatProduct
        /// </summary>
        //public const string hrm_cat_sp_get_ProductById = "exec hrm_cat_sp_get_ProductById";
        public static string hrm_cat_sp_get_ProductById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PRODUCTBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ProductById @Id";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách dữ liệu CatBank
        /// </summary>
        //public const string hrm_cat_sp_get_Bank = "exec hrm_cat_sp_get_Bank";
        public static string hrm_cat_sp_get_Bank
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_BANK";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Bank";
                }
            }
        }
        public static string hrm_cat_sp_get_ProductTypeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PRODUCTTYPEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ProductTypeById @Id";
                }
            }
        }
        public static string hrm_cat_sp_get_ProductTypeByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PRODUCTTYPEBYIDS(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ProductTypeByIds @Ids";
                }
            }
        }
        public static string hrm_Cat_sp_get_Bank_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_BANK_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_Cat_sp_get_Bank_multi @Keyword";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách dữ liệu CatProductType
        /// </summary>
        //public const string hrm_cat_sp_get_ProductType = "exec hrm_cat_sp_get_ProductType";
        public static string hrm_cat_sp_get_ProductType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_PRODUCTTYPE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ProductType";
                }
            }
        }

        public static string hrm_cat_sp_get_Country
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_COUNTRY";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Country";
                }
            }
        }
        public static string hrm_cat_sp_get_Supplier
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_SUPPLIER";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Supplier";
                }
            }
        }

        public static string hrm_cat_sp_get_SupplierByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SUPPLIERBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SupplierByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_PurchaseItems
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_PURCHASEITEMS";
                }
                else
                {
                    return "exec hrm_cat_sp_get_PurchaseItems";
                }
            }
        }

        public static string hrm_cat_sp_get_PurchaseItemsByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PURCHASEITEMSBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_PurchaseItemsByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_CountryById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_COUNTRYBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CountryById @Id";
                }
            }
        }
        public static string hrm_cat_sp_get_BankById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_BANKBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_BankById @Id";
                }
            }
        }

        /// <summary>
        /// Lấy danh sách dữ liệu CatCategory
        /// </summary>
        //public const string hrm_cat_sp_get_Bank = "exec hrm_cat_sp_get_Bank";
        public static string hrm_cat_sp_get_Category
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_CATEGORY";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Category";
                }
            }
        }

        public static string hrm_cat_sp_get_CategoryById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_CATEGORYBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CategoryById @Id";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách dữ liệu Currency
        /// </summary>
        public static string hrm_cat_sp_get_Currency
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_CURRENCY";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Currency";
                }
            }
        }

        public static string hrm_cat_sp_get_CurrencyById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_CURRENCYBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CurrencyById @Id";
                }
            }
        }



        public static string hrm_cat_sp_get_Currency_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_CURRENCY_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Currency_Multi @Keyword";
                }
            }

        }

        public static string hrm_Cat_sp_get_Product_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PRODUCT_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_Cat_sp_get_Product_multi @Keyword";
                }
            }

        }
        public static string hrm_sal_sp_get_SalDepartment_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_SALDEPARTMENT_MT";
                }
                else
                {
                    return "exec hrm_sal_sp_get_SalDepartment_Multi @Keyword";
                }
            }
        }

        /// <summary> Lấy đối tượng Cat_DayOff theo  DayOffyId </summary> 
        // public const string hrm_cat_sp_get_DayOffById = "exec hrm_cat_sp_get_DayOffById @DayOffyId";
        public static string hrm_cat_sp_get_DayOffById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_DAYOFFBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_DayOffById @Id";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Cat_DayOff</summary>
        //public const string hrm_cat_sp_get_DayOff = "exec hrm_cat_sp_get_DayOff";
        public static string hrm_cat_sp_get_DayOff
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_DAYOFF";
                }
                else
                {
                    return "exec hrm_cat_sp_get_DayOff";
                }
            }
        }

        public static string Hrm_SYS_SP_GET_LOCKOBJECTITEM
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_LOCKOBJECTITEM";
                }
                else
                {
                    return "exec Hrm_SYS_SP_GET_LOCKOBJECTITEM";
                }
            }
        }
        /// <summary> Lấy danh sách đối tượng Cat_OrgStructure</summary>
        public static string hrm_cat_sp_get_OrgStructure
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ORGSTRUCTURE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OrgStructure";
                }
            }
        }


        /// <summary> Lấy danh sách đối tượng Cat_OrgStructure</summary>
        public static string hrm_cat_sp_get_OrgStructureType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ORGSTRUCTURETYPE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OrgStructureType";
                }
            }
        }
        public static string hrm_cat_sp_get_OrgStructureTypeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ORGTYPEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OrgStructureTypeById @Id";
                }
            }
        }
        // public const string hrm_cat_sp_get_OrgStructure = "exec hrm_cat_sp_get_OrgStructure";

        /// <summary> [Chuc.Nguyen] - Lấy danh sách phòng ban theo ParentId</summary>
        //  public const string hrm_cat_sp_get_OrgStructureByParentId = "exec hrm_cat_sp_get_OrgStructureByParentId @parentId";

        public static string hrm_cat_sp_get_OrgStructureByParentId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ORGSTRUCTBYPARENTID";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OrgStructureByParentId @parentId";
                }
            }
        }

        public static string hrm_hr_sp_get_OrgChart
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_ORGCHART";
                }
                else
                {
                    return "exec hrm_hr_sp_get_OrgChart";
                }
            }
        }
        ///<summary> Lấy danh sách đối tượng Cat_OrgStructure để show grid</summary>>
        // public const string hrm_cat_sp_get_AllOrgStructure = "exec hrm_cat_sp_get_AllOrgStructure";
        public static string hrm_cat_sp_get_AllOrgStructure
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ALLORGSTRUCTURE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AllOrgStructure";
                }
            }
        }
        //public static string hrm_cat_sp_get_OrgStructureType
        //{
        //    get
        //    {
        //        if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //        {
        //            return "CAT_SP_GET_ORGSTRUCTURETYPE";
        //        }
        //        else
        //        {
        //            return "exec hrm_cat_sp_get_OrgStructureType";
        //        }
        //    }
        //}

        /// <summary> Lấy danh sách đối tượng Cat_District</summary>
        //public const string hrm_cat_sp_get_District = "exec hrm_cat_sp_get_District";
        public static string hrm_cat_sp_get_District
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_DISTRICT";
                }
                else
                {
                    return "exec hrm_cat_sp_get_District";
                }
            }
        }
        public static string hrm_cat_sp_get_DistrictById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_DISTRICTBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_DistrictById @Id";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Cat_Export</summary>
        public static string hrm_cat_sp_get_Export
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_EXPORT";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Export";
                }
            }

        }

        public static string hrm_cat_sp_get_Pivot
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_PIVOT";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Pivot";
                }
            }
        }

        public static string hrm_cat_sp_get_ExportWord
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_EXPORTWORD";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ExportWord";
                }
            }
        }

        public static string hrm_cat_sp_get_ExportById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_EXPORTBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ExportById @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_PivotById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PIVOTBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_PivotById @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_OrgMoreInforContractById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ORGINFOCONTRACTBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OrgMoreInforContractById @Id";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Cat_ExportItem</summary>
        // public const string hrm_cat_sp_get_ExportItem = "exec hrm_cat_sp_get_ExportItem";
        public static string hrm_cat_sp_get_ExportItem
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_EXPORTITEM";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ExportItem";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Cat_ExportItem By ExportID</summary>
        // public const string hrm_cat_sp_get_ExportItemByExportID = "exec hrm_cat_sp_get_ExportItemByExportID @ExportID";

        //public const string hrm_cat_sp_get_ExportItemByExportID = "exec hrm_cat_sp_get_ExportItemByExportID @ExportID";
        public static string hrm_cat_sp_get_ExportItemByExportID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_EXPORTITEMBTEXID";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ExportItemByExportID";
                }
            }
        }

        public static string hrm_cat_sp_get_PivotItemByPivotID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_PIVOTITEMBPIVOTID";
                }
                else
                {
                    return "exec hrm_cat_sp_get_PivotItemByPivotID";
                }
            }
        }
        /// <summary> Lấy danh sách đối tượng Cat_ImportItem/summary>
        // public const string hrm_cat_sp_get_ImportItem = "exec hrm_cat_sp_get_ImportItem";
        public static string hrm_cat_sp_get_ImportItem
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_IMPORTITEM";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ImportItem";
                }
            }
        }

        public static string hrm_cat_sp_get_ImportItemById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_IMPORTITEMBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ImportItemById @Id";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Cat_Position</summary>
        //public const string hrm_cat_sp_get_Position = "exec hrm_cat_sp_get_Position";
        public static string hrm_cat_sp_get_Position
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_POSITION";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Position";
                }
            }
        }

        public static string hrm_cat_sp_get_PositionById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_POSITONBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_PositionById @Id";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Cat_OvertimeType</summary>
        //public const string hrm_cat_sp_get_OvertimeType = "exec hrm_cat_sp_get_OvertimeType";

        public static string hrm_cat_sp_get_OvertimeType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_OVERTIMETYPE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OvertimeType";
                }
            }
        }
        public static string hrm_cat_sp_get_OvertimeTypeByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_OVERTIMETYPEBYIDS(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OvertimeTypeByIds @Id";
                }
            }
        }
        public static string hrm_cat_sp_get_OvertimeTypeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_OVERTIMETYPEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OvertimeTypeById @Id";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Cat_Element</summary>
        // public const string hrm_cat_sp_get_Element = "exec hrm_cat_sp_get_Element";
        public static string hrm_cat_sp_get_Element
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ELEMENT";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Element_Search";
                }
            }
        }

        public static string hrm_cat_sp_get_ElementByMethod
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ELEMENTBYMETHOD";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Element_By_Method";
                }
            }
        }

        public static string hrm_cat_sp_get_FormulaTemplate
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_FORMULATEMPLATE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_FormulaTemplate";
                }
            }
        }

        public static string hrm_cat_sp_get_ElementByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ELEMENTBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ElementByIds @ID";
                }
            }
        }

        public static string hrm_cat_sp_get_Element_All
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ELEMENT";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Element";
                }
            }
        }


        public static string hrm_cat_sp_get_AdvancePayment
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ADVANCEPAYMENT";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AdvancePayment";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Cat_ElementById</summary>
        // public const string hrm_cat_sp_get_ElementById = "exec hrm_cat_sp_get_ElementById";

        public static string hrm_cat_sp_get_ElementById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ELEMENTBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ElementById @Id";
                }
            }
        }
        public static string hrm_cat_sp_get_AdvancePaymentById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "chua viet";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AdvancePaymentById @Id";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Cat_Element</summary>
        // public const string hrm_cat_sp_get_JobTitle = "exec hrm_cat_sp_get_JobTitle";
        public static string hrm_cat_sp_get_JobTitle
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_JOBTITLE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_JobTitle";
                }
            }
        }

        public static string hrm_cat_sp_get_JobTitleById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_JOBTITLEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_JobTitleById @Id";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Cat_ContractType</summary>
        //  public const string hrm_cat_sp_get_ContractType = "exec hrm_cat_sp_get_ContractType";

        public static string hrm_cat_sp_get_ContractType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_CONTRACTTYPE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ContractType";
                }
            }
        }

        public static string hrm_cat_sp_get_InsuranceConfig
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_INSURANCECONFIG";
                }
                else
                {
                    return "exec hrm_cat_sp_get_InsuranceConfig";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Cat_ExportItem By ExportID</summary>
        //public const string hrm_cat_sp_get_ContractTypeById = "exec hrm_cat_sp_get_ContractTypeById @ContractTypeID";
        public static string hrm_cat_sp_get_ContractTypeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_CONTRACTTYPEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ContractTypeById @Id";
                }
            }
        }


        /// <summary> Lấy danh sách đối tượng Cat_Grade</summary>
        // public const string hrm_cat_sp_get_Cat_Grade = "exec hrm_cat_sp_get_Cat_Grade";
        public static string hrm_cat_sp_get_Cat_Grade
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_GRADE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Cat_Grade";
                }
            }
        }


        /// <summary>
        /// Lấy danh sách đối tượng Cat_Laundry
        /// </summary>
        public static string hrm_cat_sp_get_Cat_Laundry
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_LAUNDRY";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Cat_Laundry";
                }
            }
        }
        /// <summary> Lấy danh sách đối tượng Cat_ExportItem By ExportID</summary>
        //public const string hrm_cat_sp_get_Cat_GradeById = "exec hrm_cat_sp_get_Cat_GradeById @Id";
        public static string hrm_cat_sp_get_Cat_GradeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_GRADEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Cat_GradeById @Id";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Cat_GradeAttendance</summary>
        //  public const string hrm_cat_sp_get_Cat_GradeAttendance = "exec hrm_cat_sp_get_Cat_GradeAttendance";


        public static string hrm_cat_sp_get_Cat_GradeAttendance
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_GRADEATTENDANCE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Cat_GradeAttendance";
                }
            }
        }
        public static string hrm_cat_sp_get_Cat_GradeAttendanceByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_GRADEATTBYIDS(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Cat_GradeAttendanceByIds @Id";
                }
            }
        }
        /// <summary> Lấy danh sách đối tượng Cat_GradeAttendance By Id</summary>
        public static string hrm_cat_sp_get_Cat_GradeAttendanceById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_GRADEATTENDANCEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Cat_GradeAttendanceById @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_Cat_GradePayrollById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_GRADEPAYROLLBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Cat_GradePayrollById @Id";
                }
            }
        }

        //public const string hrm_cat_sp_get_Cat_GradeAttendanceById = "exec hrm_cat_sp_get_Cat_GradeAttendanceById @Id";

        /// <summary> Lấy danh sách đối tượng Cat_GradeAttendance By Id</summary>
        //public const string hrm_cat_sp_get_GradeAttendance_Multi = "exec hrm_cat_sp_get_GradeAttendance_Multi";
        public static string hrm_cat_sp_get_GradeAttendance_Multi
        {

            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_GRADEATT_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_GradeAttendance_Multi @Keyword";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Att_Grade</summary>
        public static string hrm_att_sp_get_Att_Grade
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_GRADE";
                }
                else
                {
                    return "exec hrm_att_sp_get_Att_Grade";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Att_Grade By ID</summary>
        public static string hrm_att_sp_get_Att_GradeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_GRADEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_Att_GradeById @Id";
                }
            }
        }


        /// <summary> Lấy danh sách dữ liệu Laundry </summary>
        public const string hrm_cat_sp_get_Laundry = "exec hrm_cat_sp_get_Laundry";

        /// <summary>  Lấy danh sách dữ liệu costcentre       </summary>
        //public const string hrm_cat_sp_get_CostCentre = "exec hrm_cat_sp_get_CostCentre";
        public static string hrm_cat_sp_get_CostCentre
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_COSTCENTRE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CostCentre";
                }
            }
        }

        public static string hrm_sal_sp_get_CostCentre
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_COSTCENTRE";
                }
                else
                {
                    return "exec hrm_sal_sp_get_Costcentre";
                }
            }
        }
        public static string hrm_sal_sp_getdata_CostCentreSal
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GETDATA_COSTCENTRESAL";
                }
                else
                {
                    return "exec hrm_sal_sp_getdata_CostCentreSal";
                }
            }
        }
        public static string hrm_sal_sp_get_PayrollTableItemByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_PAYROLLTBITBYPROID";
                }
                else
                {
                    return "exec hrm_sal_sp_get_PayrollTableItemByProfileId";
                }
            }
        }

        #region Sal_HealthInsuranceAndOrther
        public static string hrm_sal_sp_get_HealthInsuranceAndOrther
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_HEATHINSANDORTHER";
                }
                else
                {
                    return "exec hrm_sal_sp_get_HealthInsuranceAndOrther";
                }
            }
        }
        #endregion

        #region Sal_UnusualAllowance


        public static string hrm_sal_sp_get_UnusualAllowanceByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_UNUSUAALLOWANBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_sal_sp_get_UnusualAllowanceByIds @ID";
                }
            }
        }
        public static string hrm_sal_sp_get_UnusualAllowanceFilialWedding
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_UNUSUAALLOWANCE";
                }
                else
                {
                    return "exec hrm_sal_sp_get_UnusualAllowance";
                }
            }
        }

        public static string hrm_cat_sp_get_RelativeByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_RELATIVEBYPROID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_RelativeByProfileId @Id";
                }
            }
        }
        public static string hrm_cat_sp_get_RelativeByProfileIfForChildcare
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_RELATIVEBYPROCHILD(:Id,:R_Output); end;";
                }
                else
                {
                    return "Chưa viết";
                }
            }
        }
        public static string hrm_cat_sp_get_YearOfBirthByRelativeId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_YOFBIRBYRELATIVEID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_YearOfBirthByRelativeId @Id";
                }
            }
        }
        public static string hrm_cat_sp_get_RelativeTypeByRelativeId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_RELATIVETYPEBYRLID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_RelativeTypeByRelativeId @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_RelativeTypeByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_RELATIVETYPEBYPROID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_RelativeTypeByProfileId @Id";
                }
            }
        }
        public static string hrm_cat_sp_get_UnusualEDType_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_UNUSUALEDTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_UnusualEDType_multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_UnusualEDTypeByCode
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_UNUSUALEDTYPEBYCODE(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "Chưa viết";
                }
            }
        }

        public static string hrm_cat_sp_get_AmountByUSCfg_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_AMOUNTBYUSCFG_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AmountByUSCfg_multi @Keyword";
                }
            }
        }
        public static string hrm_hre_getdata_Supervisor
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRE_GETDATA_SUPERVISOR";
                }
                else
                {
                    return "exec hrm_hre_getdata_Supervisor";
                }
            }
        }

        public static string hrm_cat_sp_get_GetAmountByChildCare
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_AMOUNTBYCHILDCARE(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_GetAmountByChildCare @Keyword";
                }
            }
        }
        public static string hrm_sal_sp_get_UnusualAllowanceByProfileid
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_UNUSUALALWCBYPROID";
                }
                else
                {
                    return "exec hrm_sal_sp_get_UnusualAllowanceByProfileid";
                }
            }
        }
        public static string hrm_sal_sp_get_UnusualEDChildCareByProfileid
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_UNUSLEDCHILBYPROID";
                }
                else
                {
                    return "exec hrm_sal_sp_get_UnusualEDChildCareByProfileid";
                }
            }
        }


        #endregion
        public static string hrm_cat_sp_get_CostCentreById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_COSTCENTREBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CostCentreById @Id";
                }
            }
        }

        public static string hrm_sal_sp_get_CostCentreById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_COSTCENTREBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_CostcentreByID @Id";
                }
            }
        }

        public static string hrm_sal_sp_get_CostCentreByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_COSTCENTREBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_CostcentreByIDs @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_CostCentreByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_COSTCENTREBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CostCentreByIds @Ids";
                }
            }
        }


        public static string hrm_sal_sp_get_CostCentre_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_COSTCENTRE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CostCentre_multi @Keyword";
                }
            }
        }



        public static string hrm_sal_sp_get_GradeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_GRADEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_GradeByID @Id";
                }
            }
        }



        /// <summary> Lấy danh sách đối tượng Cat_ImportItem By ImportID</summary>
        public static string hrm_cat_sp_get_ImportItemByImportID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_IMPORTBYIMPORTID";
                }
                else
                {
                    // return "exec hrm_cat_sp_get_ImportItemByImportID @ImportID";
                    return "exec hrm_cat_sp_get_ImportItemByImportID";
                }
            }
        }


        /// <summary> Lấy danh sách đối tượng hrm_cat_sp_get_SyncItemBySyncID </summary>
        public static string hrm_cat_sp_get_SyncItemBySyncID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_SYNCITEMBYSYNCID";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SyncItemBySyncID";
                }
            }
        }


        /// <summary> Lấy danh sách đối tượng Cat_ShiftItem By ShiftID</summary>
        public static string hrm_cat_sp_get_ShiftItemByShiftID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_SHIFTBYSHIFTID";
                }
                else
                {
                    // return "exec hrm_cat_sp_get_ImportItemByImportID @ImportID";
                    return "exec hrm_cat_sp_get_ShiftItemByShiftID";
                }
            }
        }

        public static string hrm_cat_sp_get_ShiftItemOvertimeByShiftID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_SHIFTOTBYSHIFTID";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ShiftItemOvertimeByShiftID";
                }
            }
        }

        public static string hrm_cat_sp_get_ShiftItemById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_SHIFTITEMBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ShiftItemById @Id";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng ContractType đế export selected(store : hrm_hr_sp_get_ContractTypeByIds) </summary>
        public static string hrm_cat_sp_get_ContractTypeByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_CONTRACTTYPEBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ContractTypeByIds @selectedIds";
                }
            }
        }

        /// <summary>  Lấy danh sách dữ liệu EmployeeType       </summary>
        // public const string hrm_cat_sp_get_EmployeeType = "exec hrm_cat_sp_get_EmployeeType";

        public static string hrm_cat_sp_get_EmployeeType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_EMPLOYEETYPE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_EmployeeType";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng EmployeeType By ID</summary>
        //public const string hrm_cat_sp_get_EmployeeTypeById = "exec hrm_cat_sp_get_EmployeeTypeById @EmployeeTypeID";

        public static string hrm_cat_sp_get_EmployeeTypeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_EMPLOYEETYPEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_EmployeeTypeById @Id";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng EmployeeType đế export selected(store : hrm_cat_sp_get_EmployeeTypeIds) </summary>
        public const string hrm_cat_sp_get_EmployeeTypeIds = "exec hrm_cat_sp_get_EmployeeTypeIds @selectedIds";

        /// <summary>  Lấy danh sách dữ liệu RelativeType       </summary>
        ///  public const string hrm_cat_sp_get_RelativesType = "exec hrm_cat_sp_get_RelativesType";
        public static string hrm_cat_sp_get_RelativesType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_RELATIVETYPE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_RelativesType";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng RelativeType By ID</summary>
        //  public const string hrm_cat_sp_get_RelativesTypeById = "exec hrm_cat_sp_get_RelativesTypeById @RelativeTypeID";
        public static string hrm_cat_sp_get_RelativesTypeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_RELATIVETYPEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_RelativesTypeById @Id";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng DisciplinedTypes By ID</summary>
        public static string hrm_cat_sp_get_DisciplinedTypesById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_DISCIPTYPESBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_DisciplinedTypesById @Id";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng DisciplinedTypes </summary>
        public static string hrm_cat_sp_get_DisciplinedTypes
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_DISCIPLINEDTYPES";
                }
                else
                {
                    return "exec hrm_cat_sp_get_DisciplinedTypes";
                }
            }
        }

        public static string hrm_cat_sp_get_DisciplinedTypes_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_DISCIPTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_DisciplinedTypes_multi @Keyword";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng RelativeType đế export selected(store : hrm_cat_sp_get_RelativeTypeIds) </summary>
        public const string hrm_cat_sp_get_RelativesTypeIds = "exec hrm_cat_sp_get_RelativesTypeIds @selectedIds";

        /// <summary> Lấy danh sách đối tượng Cat_LeaveDayType</summary>
        // public const string hrm_cat_sp_get_LeaveDayType = "exec hrm_cat_sp_get_LeaveDayType";
        public static string hrm_cat_sp_get_LeaveDayType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_LEAVEDAYTYPE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_LeaveDayType";
                }
            }
        }
        public static string hrm_cat_sp_get_LeaveDayTypeByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_LEAVEDAYTYPEBYIDS(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_LeaveDayTypeByIds @Id";
                }
            }
        }
        public static string hrm_cat_sp_get_LeaveDayTypeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_LEAVEDAYTYPEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_LeaveDayTypeById @Id";
                }
            }
        }

        /// <summary>
        /// [Hien.Nguyen] - 2014/10/26 Get laeveDayType By Code
        /// </summary>
        public static string hrm_cat_sp_get_LeaveDayTypeByCode
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_LEAVEDAYTYPEBYCODE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_LeaveDayTypeByCode";
                }
            }
        }


        /// <summary> Lấy danh sách đối tượng Cat_DayOff</summary>
        public const string hrm_cat_sp_get_DayOff_Search = "exec hrm_cat_sp_get_DayOff_Search";
        /// <summary> Lấy danh sách đối tượng Cat_Element</summary>
        public static string hrm_cat_sp_get_Element_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ELEMENT_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Element_Multi @Keyword";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng DayOff đế export selected(store : hrm_cat_sp_get_DayOffByIds) </summary>
        // public const string hrm_cat_sp_get_DayOffByIds = "exec hrm_cat_sp_get_DayOffByIds @selectedIds";

        public static string hrm_cat_sp_get_DayOffByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_DAYOFFBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_DayOffByIds @Ids";
                }
            }
        }

        /// <summary> Lấy danh sách dữ liệu Import </summary>
        // public const string hrm_cat_sp_get_Import = "exec hrm_cat_sp_get_Import";
        public static string hrm_cat_sp_get_Import
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_IMPORT";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Import";
                }
            }
        }

        public static string hrm_cat_sp_get_ImportById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_IMPORTBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ImportById @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_SyncById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SYNCBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SyncById @Id";
                }
            }
        }


        /// <summary> Lấy danh sách dữ liệu LateEarlyRule </summary>
        //public const string hrm_cat_sp_get_LateEarlyRule = "exec hrm_cat_sp_get_LateEarlyRule";
        public static string hrm_cat_sp_get_LateEarlyRule
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_LATEEARLYRULE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_LateEarlyRule";
                }
            }
        }
        /// <summary>
        /// [Hien.Nguyen] 22/10/2014 - Lấy danh sách dữ liệu LateEarlyRule by gradecfg id
        /// </summary>
        public static string hrm_cat_sp_get_LateEarlyRuleByAttGradeId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_LATEEARLYRULEBYATT";
                }
                else
                {
                    return "exec hrm_cat_sp_get_LateEarlyRuleByAtt";
                }
            }
        }




        public static string hrm_cat_sp_get_LateEarlyRuleById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_LATEEARLYRULEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_LateEarlyRuleById @Id";
                }
            }
        }

        /// <summary> Lấy danh sách dữ liệu ResignReason </summary>
        // public const string hrm_cat_sp_get_ResignReason = "exec hrm_cat_sp_get_ResignReason";
        public static string hrm_cat_sp_get_ResignReason
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_RESIGNREASON";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ResignReason";
                }
            }
        }
        public static string hrm_cat_sp_get_ResignReasonById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_RESIGNREASONBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ResignReasonById @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_OvertimeReason
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_OVERTIMEREASON";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OvertimeReason";
                }
            }
        }
        public static string hrm_cat_sp_get_OvertimeReasonByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_OVERTIMEREASONBYIDS(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OvertimeReasonByIds @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_OvertimeReasonById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_OVERTIMEREASONBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OvertimeReasonById @Keyword";
                }
            }
        }

        /// <summary> Lấy danh sách dữ liệu TAMScanReasonMiss </summary>
        // public const string hrm_cat_sp_get_ResignReason = "exec hrm_cat_sp_get_ResignReason";
        public static string hrm_cat_sp_get_TAMScanReasonMiss
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_TAMSCANREASONNMISS";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TAMScanReasonMiss";
                }
            }
        }

        public static string hrm_cat_sp_get_TAMScanReasonMissByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_TAMSCANREASONMBYIDS(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TAMScanReasonMissByIds @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_GradePayroll
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_GRADEPAYROLL";
                }
                else
                {
                    return "exec hrm_cat_sp_get_GradePayroll";
                }
            }
        }
        public static string hrm_cat_sp_get_GradePayroll_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_GRADEPAYROLL_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_GradePayroll_multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_CatElement_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ELEMENT_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Element_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_TamScanReasonMiss_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_TAMREASONMISS_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TamScanReasonMiss_multi @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_PayrollGroup_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PAYROLLGROUP_MUTIL(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_PayrollGroup_multi @Keyword";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng "Cat_TAMScanReasonMiss" By ID</summary>
        public static string hrm_cat_sp_get_TAMScanReasonMiss_ById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    //return "begin CAT_SP_GET_TAMSCANMISSBYID(:Id,:R_Output); end;";
                    return "begin CAT_SP_GET_TAMScanReaMiss_BYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TAMScanReasonMiss_ById @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_HDTJobTypeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_HDTJOBTYPEBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_HDTJobTypeById @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_HDTJobType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_HDTJOBTYPE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_HDTJobType";
                }
            }
        }

        public static string hrm_cat_sp_set_ApprovedAllHDTJobType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_SET_APPROVEALLHDTTYPE";
                }
                else
                {
                    return "exec hrm_cat_sp_set_ApprovedAllHDTJobType";
                }
            }
        }

        public static string hrm_cat_sp_get_HDTJobType_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_HDTJOBTYPEMULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_HDTJobType_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_PayrollGroupById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PAYROLLGROUPBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_PayrollGroupById @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_PayrollGroupByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PAYROLLGROUPBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_PayrollGroupByIds @Ids";
                }
            }
        }



        public static string hrm_cat_sp_get_UnusualAllowanceCfg
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_UNUSUALALLOWCFG";
                }
                else
                {
                    return "exec hrm_cat_sp_get_UnusualAllowanceCfg";
                }
            }
        }

        // Lấy theo loại PC + loại
        public static string hrm_cat_sp_get_UnusualAllowanceEDCfg
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_UNUSUALALLOWCFG";
                }
                else
                {
                    return "exec hrm_cat_sp_get_UnusualAllowanceEDCfg";
                }
            }
        }

        public static string hrm_cat_sp_get_UnuCfgbyChild
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_MULTIUNUCFGBYCHILD(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_multiunucfgbychild";
                }
            }
        }

        public static string hrm_cat_sp_get_UnusualAllowanceCfg_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_UNUSUALALCFG_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_UnusualAllowanceCfg_multi";
                }
            }
        }


        public static string hrm_cat_sp_get_UnusualAllowanceCfgId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_UNUSUALALLOWCFGID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_UnusualAllowanceCfgId @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_UnusualAllowanceCfgIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_UNUSUALALLOWCFGIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_UnusualAllowanceCfgIds @Ids";
                }
            }
        }

        #endregion

        #region Cat_Sync
        public static string hrm_cat_sp_get_Sync
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_SYNC";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Sync";
                }
            }
        }
        #endregion

        #region Canteen
        /// <summary>
        /// Lấy danh sách dữ liệu Canteen theo store tìm kiếm
        /// </summary>

        public static string hrm_can_sp_get_Canteen
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAN_SP_GET_CANTEEN";
                }
                else
                {
                    return "exec hrm_can_sp_get_Canteen";
                }
            }
        }

        /// <summary>
        /// Lấy danh sách dữ liệu theo country id
        /// </summary>       
        public static string hrm_can_sp_get_Canteen_ByCateringId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_CANTEEN_BYCATERINGID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_sp_get_Canteen_ByCateringId @Id";
                }
            }
        }

        public static string hrm_can_sp_get_Canteen_ByTAMScanMissingById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_TAMSCANMISSBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_sp_get_Canteen_TAMSCANMISSBYID @Id";
                }
            }
        }

        /// <summary>
        /// Lấy danh sách dữ liệu máy quẹt thẻ theo store tìm kiếm
        /// </summary>
        public static string hrm_can_get_MachineOfLine
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAN_SP_GET_MACHINEOFLINE";
                }
                else
                {
                    return "exec hrm_can_get_MachineOfLine";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách dữ liệu Catering theo store tìm kiếm
        /// </summary>
        //public const string hrm_can_sp_get_Catering = "exec hrm_can_sp_get_Catering";
        public static string hrm_can_sp_get_Catering
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAN_SP_GET_CATERING";
                }
                else
                {
                    return "exec hrm_can_sp_get_Catering";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách dữ liệu Line theo store tìm kiếm
        /// </summary>
        //public const string hrm_can_sp_get_Line = "exec hrm_can_sp_get_Line";
        public static string hrm_can_sp_get_Line
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAN_SP_GET_LINE";
                }
                else
                {
                    return "exec hrm_can_sp_get_Line";
                }
            }
        }

        // Lấy DS Dữ Liệu Quên quẹt Thẻ - không phải màn hình tổng hợp dữ liệu quên quẹt thẻ.
        public static string hrm_can_sp_get_MealRecordMissing
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAN_SP_GET_MEALRECORDMISSING";
                }
                else
                {
                    return "exec hrm_can_sp_get_MealRecordMissing";
                }
            }
        }


        public static string hrm_can_sp_get_RecordMissing
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAN_SP_GET_RECORDMISSING";
                }
                else
                {
                    return "exec hrm_can_sp_get_RecordMissing";
                }
            }
        }

        public static string hrm_can_sp_get_MealRecordMissingId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_MEALRECORDMISSINGID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_sp_get_MealRecordMissingId  @Id";
                }
            }
        }




        /// <summary>
        /// Lấy danh sách dữ liệu MEALALLOWANCETYPE theo store tìm kiếm
        /// </summary>        
        public static string hrm_can_sp_get_MealAllowanceType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAN_SP_GET_MEALALLOWANCETYPE ";
                }
                else
                {
                    return "exec hrm_can_sp_get_MealAllowanceType";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách dữ liệu Location theo store tìm kiếm
        /// </summary>        
        public static string hrm_can_sp_get_Location
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAN_SP_GET_LOCATION";
                }
                else
                {
                    return "exec hrm_can_sp_get_Location";
                }
            }
        }

        /// <summary>
        /// Lấy danh sách dữ liệu Location theo store tìm kiếm
        /// </summary>        
        public static string hrm_lau_sp_get_Location
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "LAU_SP_GET_LOCATION";
                }
                else
                {
                    return "exec hrm_lau_sp_get_Location";
                }
            }
        }

        /// <summary>
        /// Lấy danh sách dữ liệu BackPay theo store tìm kiếm
        /// </summary>        
        public static string hrm_can_sp_get_BackPay
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAN_SP_GET_BACKPAY";
                }
                else
                {
                    return "exec hrm_can_sp_get_BackPay";
                }
            }
        }

        public static string hrm_can_sp_get_ComputeBackPay
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAN_SP_GET_COMPUTEBACKPAY";
                }
                else
                {
                    return "exec hrm_can_sp_get_ComputeBackPay";
                }
            }
        }



        /// <summary>
        /// Lấy danh sách dữ liệu TamScanLog theo store tìm kiếm
        /// </summary>
        public static string hrm_can_sp_get_TamScanLog
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAN_SP_GET_TAMSCANLOG";
                }
                else
                {
                    return "exec hrm_can_sp_get_TamScanLog";
                }
            }
        }
        /// <summary> Lấy danh sách đối tượng Can_TamScanLog (store : hrm_can_sp_get_TamScanLogIds) </summary>
        public static string hrm_can_sp_get_TamScanLogIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_TAMSCANLOGBYIDS(:selectedIds,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_sp_get_TamScanLogIds @selectedIds";
                }
            }
        }

        public static string hrm_can_sp_delete_TAMScanLog
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_DELETE_TAMSCANLOG(:CardCode,:DateFrom,:DateTo); end;";
                }
                else
                {
                    return "exec hrm_can_sp_delete_TAMScanLog @CardCode,@DateFrom,@DateTo";
                }
            }
        }

        public static string hrm_can_get_MealAllowanceToMoney
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAN_SP_GET_MEALALLOWANCE";
                }
                else
                {
                    return "exec hrm_can_get_MealAllowanceToMoney";
                }
            }
        }

        public static string hrm_can_get_MealAllowanceToMoneyById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_MEALALLOWANCEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_get_MealAllowanceToMoneyById @Id";
                }
            }
        }

        /// <summary>
        /// [Hieu.Van] store lấy danh sách dữ liệu MealRecord
        /// </summary>
        public static string hrm_can_get_MealRecord
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAN_SP_GET_MEALRECORD";
                }
                else
                {
                    return "exec hrm_can_get_MealRecord";
                }
            }
        }

        public static string hrm_can_sp_get_MealRecord_ByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_MEALRECORD_BYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_sp_get_MealRecord_ByIds @Ids";
                }
            }
        }
        /// <summary>
        /// [Hieu.Van] store lấy danh sách dữ liệu MealRecord theo ID
        /// </summary>
        public static string hrm_can_get_MealRecordById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_MEALRECORDBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_get_MealRecordById @Id";
                }
            }
        }

        public static string hrm_can_get_MealAllowanceType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAN_SP_GET_MEALALLOWANCETYPE";
                }
                else
                {
                    return "exec hrm_can_get_MeaAllowanceType";
                }
            }
        }
        public static string hrm_can_get_MealPriceType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAN_SP_GET_MEALPRICETYPE";
                }
                else
                {
                    return "exec hrm_can_get_MealPriceType";
                }
            }
        }

        public static string hrm_can_get_MeaPriceType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAN_SP_GET_MEALPRICETYPE";
                }
                else
                {
                    return "exec hrm_can_get_MeaPriceType";
                }
            }
        }
        public static string hrm_can_sp_get_backpaybyid
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_BACKPAYBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec can_sp_get_backpaybyid @Id";
                }
            }
        }

        public static string hrm_can_sp_get_canteenbyid
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_CANTEENBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_sp_get_canteenbyid @Id";
                }
            }
        }

        public static string hrm_can_sp_get_cateringbyid
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_CATERINGBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_sp_get_cateringbyid @Id";
                }
            }
        }

        public static string hrm_can_sp_get_locationbyid
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_LOCATIONBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_sp_get_locationbyid @Id";
                }
            }
        }

        public static string hrm_lau_sp_get_locationbyid
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin LAU_SP_GET_LOCATIONBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_lau_sp_get_locationbyid @Id";
                }
            }
        }

        public static string hrm_can_sp_get_machineoflinebyid
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_MACHINEOFLINEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec can_sp_get_machineoflinebyid @Id";
                }
            }
        }

        public static string hrm_can_sp_get_mealallowtypebyid
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_MEALALLOWTYPEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_sp_get_mealallowtypebyid @Id";
                }
            }
        }

        public static string hrm_can_sp_get_tamscanlogbyid
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_TAMSCANLOGBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec can_sp_get_tamscanlogbyid @Id";
                }
            }
        }

        #endregion

        #region Hre

        public static string hrm_hr_sp_get_KeepingSalary
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_KEEPINGSALARY";
                }
                else
                {
                    return "exec hrm_hr_sp_get_KeepingSalary";
                }
            }
        }

        #region Hre_ReportProfileIsWaitingStopWorking
        public static string hrm_hr_sp_get_ProfileWaitingStopWoking
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILEWAITSTOP";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileWaitingStopWoking";
                }
            }
        }
        #endregion


        /// <summary>
        /// Lấy danh sách tất cả dữ liệu hre_profile
        /// </summary>
        //public const string hrm_hr_sp_get_Profile = "exec hrm_hr_sp_get_Profile";
        public static string hrm_hr_sp_get_Profile
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILE";
                }
                else
                {
                    return "exec hrm_hr_sp_get_Profile";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileOrgStructureDetail
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILEORGDETAIL";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileOrgStructureDetails";
                }
            }
        }


        public static string hrm_hr_sp_get_ProfileActive
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILEACTIVE";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileActive";
                }
            }
        }

        public static string hrm_rec_sp_get_ProfileWorking_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PRO_WORKING_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_ProfileWorking_Multi @Keyword";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileByCodeAtt
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_HR_SP_GET_PROBYCODEATT";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileByCodeAtt";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileByCodeAttID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_HR_SP_GET_PROBYCODEATTID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileByCodeAttID";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileByCodeEmp
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_HR_SP_GET_PROFILEBYCODEEMP";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileByCodeEmp";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileByProfileName
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_HR_SP_GET_PROFILEBYPRONAME";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileByProfileName";
                }
            }
        }
        public static string hrm_hr_sp_get_RelativeByProfileName
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_HR_SP_GET_RELATVBYPRONAME";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RelativeByProfileName";
                }
            }
        }
        public static string hrm_hr_sp_get_ProfileByProfileNameAndBirthDay
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PROBYNAMEANDBIRTH";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileByProfileNameAndBirthDay";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileByIDNo
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_HR_SP_GET_PROFILEBYIDNO";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileByIDNo";
                }
            }
        }

        /// <summary>
        /// [Tho.Bui]Lấy danh sách tất cả dữ liệu hre_profile theo is back list
        /// </summary>
        //public const string hrm_hr_sp_get_Profile = "exec hrm_hr_sp_get_Profile";
        public static string hrm_hr_sp_get_ProfileIsBackList
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTISBACKLISTPROFILE";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileIsBackList";
                }
            }
        }
        public static string hrm_hr_sp_get_DateEndAccidentTypeList
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTACCIDENT";
                }
                else
                {
                    return "exec hrm_hr_sp_get_DateEndAccidentTypeList";
                }
            }
        }

        public static string hrm_rec_sp_get_ReportJobVacancy
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_REPORTJOBVACANCY";
                }
                else
                {
                    return "exec hrm_rec_sp_get_ReportJobVacancy";
                }
            }
        }


        // chua viet store SQL
        public static string hrm_rec_sp_get_ReportListCandidate
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_REPORTLISTCANDIDATE";
                }
                else
                {
                    return "exec hrm_rec_sp_get_ReportListCandidate";
                }
            }
        }

        /// <summary>
        /// [Tho.Bui] Att_Pregnancy Report
        /// </summary>
        public static string hrm_att_sp_get_ReportPregnancy
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_RPTPREGNANCY";
                }
                else
                {
                    return "exec hrm_att_sp_get_ReportPregnancy";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách tất cả dữ liệu hre_profile thử việc
        /// </summary>
        //public const string hrm_hr_sp_get_ProbationProfile = "exec [hrm_hr_sp_get_ProbationProfile]";
        public static string hrm_hr_sp_get_ProbationProfile
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILEPROBATION";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProbationProfile";
                }
            }
        }
        public static string hrm_hr_sp_get_PlanHeadCount
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PLANHEADCOUNT";
                }
                else
                {
                    return "exec hrm_hr_sp_get_PlanHeadCount";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách tất cả dữ liệu hre_profile 
        /// </summary>
        //public const string hrm_hr_sp_get_ProfileAll = "exec hrm_hr_sp_get_ProfileAll";
        public static string hrm_hr_sp_get_ProfileAll
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILEALL";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileAll";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileDataAll
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILEDATAALL";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileDataAll";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách tất cả dữ liệu hre_profile nghỉ việc
        /// </summary>
        //public const string hrm_hr_sp_get_ProfileQuit = "exec hrm_hr_sp_get_ProfileQuit";
        public static string hrm_hr_sp_get_ProfileQuit
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILEQUIT";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileQuit";
                }
            }
        }

        public static string hrm_hr_sp_get_StopWorking
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILESTOPWORKING";
                }
                else
                {
                    return "exec hrm_hr_sp_get_StopWorking";
                }
            }
        }


        public static string hrm_hr_sp_get_RptProfileComBackByID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_RPTPROFILECOMBACKID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RptProfileComBackByID @Ids";
                }
            }
        }
        public static string hrm_hr_sp_get_RptProfileComBack
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTPROFILECOMBACK";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RptProfileComBack";
                }
            }
        }

        public static string hrm_tra_sp_get_RptTraineeByMonth
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_RPTTRAINEEBYMONTH";
                }
                else
                {
                    return "exec hrm_tra_sp_get_RptTraineeByMonth";
                }
            }
        }

        public static string hrm_hr_sp_get_Suspense
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_HR_SP_GET_SUSPENSE";
                }
                else
                {
                    return "exec hrm_hr_sp_get_Suspense";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileSuspense
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILESUSPENSE";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileSuspense";
                }
            }
        }

        public static string hrm_hr_sp_getdata_SuspenseOrQuit
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GETDATA_SUSPENSEORQUIT";
                }
                else
                {
                    return "exec hrm_hr_sp_getdata_SuspenseOrQuit";
                }
            }
        }

        public static string hrm_hr_sp_get_RegisterComback
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_HR_SP_GET_REGISTERCOMBACK";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RegisterComback";
                }
            }
        }

        public static string hrm_hr_sp_get_DateStopByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_DATESTOPBYPROD(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_DateStopByProfileId @Id";
                }
            }
        }
        public static string hrm_hr_sp_get_ResonRegisterByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_RSREGISTERBYPROID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ResonRegisterByProfileId @Id";
                }
            }
        }

        public static string hrm_hr_sp_get_StopWorkingById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_STOPWORKINGBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_StopWorkingById @Id";
                }
            }
        }

        public static string hrm_hr_sp_get_StopWorkingByProId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_STOPWORKINGBYPROID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_StopWorkingByProId @Id";
                }
            }
        }

        /// <summary>
        /// [Tho.Bui] Lấy danh sách tất cả dữ liệu hre_profile nghỉ hưu
        /// </summary>
        //public const string hrm_hr_sp_get_ProfileRetirement = "exec hrm_hr_sp_get_ProfileQuit";
        public static string hrm_hr_sp_get_ProfileRetirement
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILERETIREMENT";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileRetirement";
                }
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả dữ liệu hre_profile chờ nhận việc
        /// </summary>
        //public const string hrm_hr_sp_get_ProfileWaitingHire = "exec hrm_hr_sp_get_ProfileWaitingHire";
        public static string hrm_hr_sp_get_ProfileWaitingHire
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILEWAITINGHIRE";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileWaitingHire";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách tất cả dữ liệu hre_profile chờ nhận việc
        /// </summary>
        //public const string hrm_hr_sp_get_ProfileWaitingHire = "exec hrm_hr_sp_get_ProfileWaitingHire";

        public static string hrm_hr_sp_get_ProfileUnHire
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILEUNHIREHIRE";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileUnHire";
                }
            }
        }
        /// <summary>
        /// Lấy danh nhân viên thiếu dữ liệu
        /// </summary>
        public static string hrm_hr_sp_get_ProfileNotFullData
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILENOTFULLDATA";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileNotFullData";
                }
            }
        }

        /// <summary>
        /// Lấy đối tượng profile bởi ID
        /// </summary>      
        public static string hrm_hr_sp_get_ProfileById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PROFILEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileById @Id";
                }
            }
        }
        /// <summary>
        ///Lấy đối tượng profile bởi ID
        ///lấy tracttype then bang Hre_Contract
        /// </summary>
        public static string hrm_hr_sp_get_ProfileByIdv2
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PROFILEBYIDv2(:Id,:R_Output); end;";

                }
                else
                {
                    return "exec hrm_HR_SP_GET_PROFILEBYIDv2 @Id";
                }
            }
        }
        public static string hrm_Cat_SP_GET_ModeCode_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin Cat_SP_GET_ModeCode_Multi(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_Cat_SP_GET_ModeCode_Multi @Keyword";
                }
            }
        }

        public static string hrm_Cat_SP_GET_ReceivePlace_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin Cat_SP_GET_PurRepl_Multi(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_Cat_SP_GET_PurRepl_Multi @Keyword";
                }
            }
        }
        public static string hrm_Cat_SP_GET_PaymentMethod_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin Cat_SP_GET_PaymentMethod_Multi(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_Cat_SP_GET_PaymentMethod_Multi @Keyword";
                }
            }
        }
        public static string hrm_Cat_SP_GET_ColorCode_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin Cat_SP_GET_ColorCode_Multi(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_Cat_SP_GET_ColorCode_Multi @Keyword";
                }
            }
        }
        /// <summary>
        /// Lấy đối tượng nhiều profile bởi ID
        /// </summary>
        /// hrm_hr_sp_get_Profile_multi
        public static string hrm_hr_sp_get_Profile_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PROFILE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_Profile_multi @Keyword";
                }
            }
        }

        /// <summary>
        /// Get Dữ liệu nhân viên nghỉ việc
        /// </summary>
        public static string hrm_hr_sp_get_ProfileQuit_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PROFILEQUIT_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileQuit_multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_MasterDataGroup_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_MASTDATAGROUP_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_MasterDataGroup_multi @Keyword";
                }
            }
        }


        public static string hrm_hr_sp_get_ProfileByCodeEmps
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PROFILEBYCODEEMPS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileByCodeEmps @ID";
                }
            }
        }
        public static string hrm_hr_sp_get_ProfileAllByCodeEmps
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PROFILEALLBYCODEEMP(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileAllByCodeEmps @ID";
                }
            }
        }

        public static string hrm_cat_sp_get_StoreDocumentByCode
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_STOREDOCUMENTBYCODE(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_StoreDocumentByCode @ID";
                }
            }
        }


        public static string hrm_hr_sp_get_ProfileSuspense_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PROSUSPENSE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileSuspense_multi @Keyword";
                }
            }
        }


        #region get_att_profilenotgrade
        public static string hrm_hr_sp_get_ProfileNotGrade
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PROFILENOTGRADE(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileNotGrade";
                }
            }
        }
        public static string hrm_hr_sp_get_ProfileNotGrade_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILENOTGRADEMUlTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "hrm_hr_sp_get_ProfileNotGradeMulti @Keyword";
                }
            }
        }
        public static string hrm_hr_sp_get_ProfileNotGrade_multids
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PRFINOTGRADEMUlTIds(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_PRFINOTGRADEMUlTIds @Keyword";
                }
            }
        }
        #endregion

        #region get_sal_profilenotgrade
        /// <summary>
        /// Load danh sach nhan vien chua co che do luong
        /// </summary>
        public static string hrm_hr_sp_get_PFNG_sal
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_PFNG";
                }
                else
                {
                    return "exec hrm_sal_sp_get_PFNG";
                }
            }
        }
        /// <summary>
        /// load nhan vien chua co che do luong len multiselect
        /// </summary>
        public static string hrm_hr_sp_get_PFNGMUlTIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_PFNGMUlTIds(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_PFNGMUlTIds @Keyword";
                }
            }
        }

        #endregion






        public static string hrm_hr_sp_get_AllProfile_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_HR_SP_GET_ALLPROFILE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_AllProfile_multi @Keyword";
                }
            }
        }

        /// <summary>  multi load profiles là nguoi phe duyet </summary>
        public static string hrm_hr_sp_get_ApproveProfile_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_APPROVEPROFILE_MUL(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ApproveProfile_multi @Keyword";
                }
            }
        }

        /// <summary>  multi load profiles là cap tren danh gia </summary>
        public static string hrm_hr_sp_get_HighProfile_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_HIGHPROFILE_MUL(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_HighProfile_multi @Keyword";
                }
            }
        }


        public static string hrm_hr_sp_get_Profile_multi_ProfileNotContract
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PRONOTCONTACT_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_Profile_multi_ProfileNotContract @Keyword";
                }
            }
        }

        /// <summary>
        /// Lấy đối tượng profile bởi Danh sách ID
        /// </summary>
        //public const string hrm_hr_sp_get_ProfileByIds = "exec hrm_hr_sp_get_ProfileByIds @selectedIds";
        public static string hrm_hr_sp_get_ProfileByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PROFILEBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileByIds @Ids";
                }
            }
        }
        /// <summary>
        /// Export selected Hre_Profile not full data
        /// </summary>
        //public const string hrm_hr_sp_get_ProfileByIds = "exec hrm_hr_sp_get_ProfileByIds @selectedIds";
        public static string hrm_hr_sp_get_ProfileNotFullDataByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PROFILENOTFULLDATABYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileNotFullDataByIds @Ids";
                }
            }
        }

        /// <summary>
        /// [Hien.Pham] - 2014/05/31 
        /// Store lấy danh sách id của nhân viên bởi 1 chuổi phòng ban id
        /// </summary>
        //public const string hrm_hr_sp_get_ProfileIdsByOrg = "exec hrm_hr_sp_get_ProfileIdsByOrg @strOrgIds,@profileName,@codeEmp";
        public static string hrm_hr_sp_get_ProfileIdsByOrg
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILEIDSBYORG";
                    //return "begin HR_SP_GET_PROFILEIDSBYORG(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileIdsByOrg";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileIdsByOrgN
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILEIDSBYORGN";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileIdsByOrgN";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileForRptContractExpired
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFORRPTCONTRACTEXP";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileForRptContractExpired";
                }
            }
        }
        public static string hrm_att_sp_getdata_reportWorDay
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_GETDATA_REPORTWORKDAY";
                    //return "begin HR_SP_GET_PROFILEIDSBYORG(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_getdata_reportworday";
                }
            }
        }


        public static string hrm_att_sp_get_attdancetable
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_ATTTABLE";
                    //return "begin HR_SP_GET_PROFILEIDSBYORG(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_AttendanceTable";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileIdsByOrgStructure
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILEIDSBYORG";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileIdsByOrg";
                }
            }
        }
        public static string hrm_hr_sp_get_ProfileIdsByCodeEmp
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILEIDSBCODEEMPS";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileIdsByCodeEmp";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileIdsByOrgStructureID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROBYORGID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileIdsByOrgStructureID";
                }
            }
        }

        /// <summary>
        /// [Chuc.Nguen] - 2014/05/31 
        /// Store xóa danh sách nhân viên theo list id
        /// </summary>
        public const string hrm_hr_sp_remove_ProfileByIds = "exec hrm_hr_sp_remove_ProfileByIds @strIds";
        /// <summary>
        /// Lấy danh sách tất cả dữ liệu hre_Contract
        /// </summary>
        //public const string hrm_hr_sp_get_Contract = "exec hrm_hr_sp_get_Contract";
        public static string hrm_hr_sp_get_Contract
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_CONTRACT";
                }
                else
                {
                    return "exec hrm_hr_sp_get_Contract";
                }
            }
        }

        // Lấy dữ liệu tất cả ds hđ - không có khóa ngoại nào hết.
        public static string hrm_hr_sp_get_ContractData
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_CONTRACTDATA";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ContractData";
                }
            }
        }


        // Lấy dữ liệu ở màn hình ds HĐ
        public static string hrm_hr_sp_get_ContractList
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_CONTRACTLIST";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ContractList";
                }
            }
        }

        #region Hre_ContractWaiting
        public static string hrm_hr_sp_get_ContractWaiting
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_CONTRACTWAITING";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ContractWaiting";
                }
            }
        }

        public static string hrm_hr_sp_get_ContractEnd
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_CONTRACTEND";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ContractEnd";
                }
            }
        }
        #endregion

        /////[Tho.Bui] Lấy danh sách tất cả đoàn đảng của tất cả nhân viên
        ///// </summary>
        ////public const string hrm_hr_sp_get_Contract = "exec hrm_hr_sp_get_Contract";
        //public static string hrm_hr_sp_get_PartyAndUnion
        //{
        //    get
        //    {
        //        if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //        {
        //            return "HR_SP_GET_PROPARTYUINION";
        //        }
        //        else
        //        {
        //            return "exec hrm_hr_sp_get_Contract";
        //        }
        //    }
        //}
        /// <summary>
        /// Lấy đối tượng Contract bởi ID
        /// </summary>
        //public const string hrm_hr_sp_get_ContractById = "exec hrm_hr_sp_get_ContractById @contractId";
        public static string hrm_hr_sp_get_ContractById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_CONTRACTBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ContractById @Id";
                }
            }
        }

        public static string hrm_Sal_sp_get_SumAmountPayrollTableItem
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_SUMAMOUNTSALARY";
                }
                else
                {
                    return "Chưa viết";
                }
            }
        }

        public static string hrm_Sal_sp_get_BasicSalaryById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_BASICSALARYBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_BasicSalaryByID @Id";
                }
            }
        }
        public static string hrm_cat_sp_get_BasicSalaryById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_CONTRACTBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_BasicSalaryById @Id";
                }
            }
        }


        /// <summary>
        /// Lấy đối tượng Contract bởi ProfileID
        /// </summary>
        //public const string hrm_hr_sp_get_ContractsByProfileId = "exec hrm_hr_sp_get_ContractsByProfileId @ProfileId";
        public static string hrm_hr_sp_get_ContractsByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_CONTRACTBYPROID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ContractsByProfileId @Id";
                }
            }
        }

        public static string hrm_hr_sp_get_ContractsByListId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_CONTRACTBYLSTIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ContractsByListId @Ids";
                }
            }
        }
        public static string hrm_eva_sp_get_PerformanceGeneralListID
        {

            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_PERFORMANCEBYID";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceGeneralListID @Id";
                }
            }
        }

        public static string hrm_eva_sp_Set_Performance_Status
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_SET_PERFORMANCE_STATUS";
                }
                else
                {
                    return "exec hrm_eva_sp_Set_Performance_Status";
                }
            }
        }

        public static string hrm_hr_sp_get_StopWorkingByListId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_STOPWORKINGBYLSTIDS";
                }
                else
                {
                    return "exec hrm_hr_sp_get_StopWorkingByListId";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileQuitByListId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILEBYLSTID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileQuitByListId";
                }
            }
        }

        public static string hrm_cat_sp_get_LeaveDayTypeByListId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_LEAVEDAYTYPEBYLSTID";
                }
                else
                {
                    return "exec hrm_cat_sp_get_LeaveDayTypeByListId";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileByListId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILEBYLISTID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileByListId";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileWorkingByListId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_TPROFILEBYIDS";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileByIds";
                }
            }
        }

        public static string hrm_hr_sp_get_AppendixContractByIdContractID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    // chua viet.
                    return "hrm_hr_sp_get_ACByIdContractID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_AppendixContractByIdContractID";
                }
            }
        }
        public static string hrm_hr_sp_get_AppendixContractByListId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_APPCONTRACTBYIDS";
                }
                else
                {
                    return "exec hrm_hr_sp_get_AppendixContractByIds";
                }
            }
        }
        public static string hrm_hr_sp_get_ProfileProbationByListId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROBATIONLISTIDS";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileProbationByListId";
                }
            }
        }
        public static string hrm_hr_sp_get_ProfileWaitingHireByListId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_WAITINGHIREBYLISTID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileWaitingHireByListId";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách tất cả dữ liệu hre_Dependance
        /// </summary>
        //public const string hrm_hr_sp_get_Dependant = "exec hrm_hr_sp_get_Dependant";
        public static string hrm_hr_sp_get_Dependant
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_DEPENDANT";
                }
                else
                {
                    return "exec hrm_hr_sp_get_Dependant";
                }
            }
        }
        /// <summary>
        /// Lấy đối tượng Dependant bởi ID
        /// </summary>
        //public const string hrm_hr_sp_get_DependantById = "exec hrm_hr_sp_get_DependantById @dependantId";
        public static string hrm_hr_sp_get_DependantById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_DEPENDANTBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_DependantById @Id";
                }
            }
        }
        /// <summary>
        /// Lấy ds đối tượng VisaInfo theo ProfileID
        /// </summary>
        //public const string hrm_hr_sp_get_VisaInfoByProfileId = "exec hrm_hr_sp_get_VisaInfoByProfileId ";
        public static string hrm_hr_sp_get_VisaInfoByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_VISAINFOBYPROID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_VisaInfoByProfileId @Id";
                }
            }
        }
        /// <summary>
        /// Lấy đối tượng VisaInfo bởi ID
        /// </summary>
        //public const string hrm_hr_sp_get_VisaInfoById = "exec hrm_hr_sp_get_VisaInfoById @VisaInfoId";
        public static string hrm_hr_sp_get_VisaInfoById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_VISAINFOBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_VisaInfoById @Id";
                }
            }
        }
        public static string hrm_hr_sp_get_VisaInfoDateEndList
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTVISAINFO";
                }
                else
                {
                    return "exec hrm_hr_sp_get_VisaInfoDateEndList";
                }
            }
        }
        public static string hrm_hr_sp_get_ProfileByWorkPermitExpiredDate
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILEBYWPEXPIREDDATE";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileByWorkPermitExpiredDate";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách tất cả dữ liệu hre_WorkHistory
        /// </summary>
        //public const string hrm_hr_sp_get_WorkHistory = "exec hrm_hr_sp_get_WorkHistory";
        public static string hrm_hr_sp_get_WorkHistory
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_WORKHISTORY";
                }
                else
                {
                    return "exec hrm_hr_sp_get_WorkHistory";
                }
            }
        }

        public static string hrm_hr_sp_get_WorkHistoryWaitingApprove
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_WORKHISWAITAPPROVE";
                }
                else
                {
                    return "exec hrm_hr_sp_get_WorkHistoryWaitingApprove";
                }
            }
        }
        /// <summary>
        /// Lấy đối tượng WorkHistory bởi ID
        /// </summary>
        //public const string hrm_hr_sp_get_WorkHistoryById = "exec hrm_hr_sp_get_WorkHistoryById @workHistoryId";
        public static string hrm_hr_sp_get_WorkHistoryById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_WORKHISTORYBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_WorkHistoryById @Id";
                }
            }
        }
        /// <summary>
        /// Lấy đối tượng WorkHistory bởi ID
        /// </summary>
        //public const string hrm_hr_sp_get_WorkHistoryById = "exec hrm_hr_sp_get_WorkHistoryById @workHistoryId";
        public static string hrm_hr_sp_get_WorkHistoryByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_HISTORYBYPROID(:ID,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_WorkHistoryByProfileId @Id";
                }
            }
        }
        public static string hrm_hr_sp_get_CandidateHistoryByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_CANDIDATEBYPROID(:ID,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_CandidateHistoryByProfileId @Id";
                }
            }
        }

        public static string hrm_hr_sp_get_CandidateHistoryByCandidateId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_CANHISBYCANDIDATEID(:ID,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_CandidateHistoryByCandidateId @Id";
                }
            }
        }

        /// <summary>
        /// [Quoc.Do] Lấy đối tượng  CandidateHistory bởi ID
        /// </summary>
        //public const string hrm_hr_sp_get_CandidateHistoryById = "exec hrm_hr_sp_get_CandidateHistoryById @CandidateHistorId";
        public static string hrm_hr_sp_get_CandidateHistoryById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_CANDIDATEHISTORYBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_CandidateHistoryById @Id";
                }
            }
        }
        /// <summary> Lấy danh sách tất cả dữ liệu hre_CardHistory </summary>
        //public const string hrm_hr_sp_get_CardHistory = "exec hrm_hr_sp_get_CardHistory";
        public static string hrm_hr_sp_get_CardHistory
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_CARDHISTORY";
                }
                else
                {
                    return "exec hrm_hr_sp_get_CardHistory";
                }
            }
        }

        public static string hrm_hr_sp_get_CardHistorysByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_CARDHISBYPROID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_CardHistorysByProfileId";
                }
            }
        }
        public static string hrm_att_sp_get_GradeAttendanceByProIdCutID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_GRADEATTBYPROCUT";
                }
                else
                {
                    return "exec hrm_att_sp_get_GradeAttendanceByProIdCutID";
                }
            }
        }
        /// <summary> Lấy danh sách tất cả dữ liệu khen thưởng hrm_hr_sp_get_Reward </summary>
        //public const string hrm_hr_sp_get_Reward = "exec hrm_hr_sp_get_Reward";
        public static string hrm_hr_sp_get_Reward
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_REWARD";
                }
                else
                {
                    return "exec hrm_hr_sp_get_Reward";
                }
            }
        }
        /// <summary> Lấy đối tượng khen thưởng bởi rewardId (store :hrm_hr_sp_get_RewardById) </summary>
        //public const string hrm_hr_sp_get_RewardById = "exec hrm_hr_sp_get_RewardById @rewardId";
        public static string hrm_hr_sp_get_RewardById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_REWARDBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RewardById @Id";
                }
            }
        }

        //[Tho.Bui]Lấy đối tượng ProfilePartyUnion by ID
        public static string hrm_hr_sp_get_ProfilePartyUnionId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PARTYUNIONBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfilePartyUnionId @Id";
                }
            }
        }

        public static string hrm_hr_sp_get_PartyUnionList
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PARTYUNION";
                }
                else
                {
                    return "exec hrm_hr_sp_get_PartyUnionList";
                }
            }
        }
        /// <summary> Lấy danh sách kỷ luật (store: hrm_hr_sp_get_Discipline) </summary>
        //public const string hrm_hr_sp_get_Discipline = "exec hrm_hr_sp_get_Discipline";
        public static string hrm_hr_sp_get_Discipline
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_DISCIPLINE";
                }
                else
                {
                    return "exec hrm_hr_sp_get_Discipline";
                }
            }
        }
        /// <summary> Lấy đối tượng kỷ luật bởi disciplineId (store : hrm_hr_sp_get_DisciplineById) </summary>
        //public const string hrm_hr_sp_get_DisciplineById = "exec hrm_hr_sp_get_DisciplineById @disciplineId";
        public static string hrm_hr_sp_get_DisciplineById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_DISCIPLINEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_DisciplineById @Id";
                }
            }
        }
        /// <summary> Lấy đối tượng cardHistory bởi cardHistoryId (store : hrm_hr_sp_get_CardHistoryById) </summary>
        //public const string hrm_hr_sp_get_CardHistoryById = "exec hrm_hr_sp_get_CardHistoryById @CardHistoryId";
        public static string hrm_hr_sp_get_CardHistoryById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_CARDHISTORYBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_CardHistoryById @Id";
                }
            }
        }
        /// <summary> Lấy đối tượng cardHistory bởi profileId (store : hrm_hr_sp_get_CardHistoryByProfileId) </summary>
        public static string hrm_hr_sp_get_CardHistoryByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_CARDHISBYPROID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_CardHistoryByProfileId @Id";
                }
            }
        }

        /// <summary> Lấy danh sách Relative (store : hrm_hr_sp_get_Relatives) </summary>
        //public const string hrm_hr_sp_get_Relatives = "exec hrm_hr_sp_get_Relatives";
        public static string hrm_hr_sp_get_Relatives
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RELATIVES";
                }
                else
                {
                    return "exec hrm_hr_sp_get_Relatives";
                }
            }
        }

        /// <summary> Lấy đối tượng Relative theo  RelaivesId </summary>
        //public const string hrm_hr_sp_get_RelativesById = "exec hrm_hr_sp_get_RelativesById @relativesId";
        public static string hrm_hr_sp_get_RelativesById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_RELATIVESBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RelativesById @Id";
                }
            }
        }
        // Bắt trùng CMND của người phụ thuộc
        public static string hrm_hr_sp_get_DependantByIdNo
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_DEPENDANTBYIDNO(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_DependantByIdNo @Id";
                }
            }
        }
        // Bắt trùng CMND của người thân
        public static string hrm_hr_sp_get_RelativesByIdNo
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_RELATIVESBYIDNO(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RelativesByIdNo @Id";
                }
            }
        }
        // lấy Relative theo nhân viên
        public static string hrm_hr_sp_get_RelativeByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_RELATIVEBYPROID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RelativeByProfileId @Id";
                }
            }
        }
        //lay Relative theo nhan vien
        public static string hrm_hr_sp_get_RelativeByProfileIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_RELATIVEBYPROIDS(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RelativeByProfileIds @Id";
                }
            }
        }
        public static string hrm_hr_sp_get_CodeAlocalByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_HR_SP_GET_CODEALOCALBYPRO";
                }
                else
                {
                    return "exec hrm_hr_sp_get_CodeAlocalByProfileId";
                }
            }
        }
        public static string hrm_hr_sp_get_Sal_CostCentreSalByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_HR_SP_GET_SALCTCTSBYPROID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_Sal_CostCentreSalByProfileId";
                }
            }
        }
        public static string hrm_hr_sp_get_Sal_CostCentreSalById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_CAT_SP_GET_SALCTCTSALBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CostCentreSalById @Id";
                }
            }
        }

        public static string hrm_sal_sp_get_CodeAlocalById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_SAL_SP_GET_CODEALOCALBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_CodeAlocalById @Id";
                }
            }
        }

        // lấy Relative theo nhân viên
        public static string hrm_hr_sp_get_DependantByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_DEPENDANTBYPROID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_DependantByProfileId";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Relatives (store : hrm_hr_sp_get_RelativesByIds) </summary>
        //public const string hrm_hr_sp_get_RelativesByIds = "exec hrm_hr_sp_get_RelativesByIds @selectedIds";
        public static string hrm_hr_sp_get_RelativesByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_RELATIVESBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RelativesByIds @Ids";
                }
            }
        }


        public static string hrm_hr_sp_get_Relativebyname
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_HR_SP_GET_RELATIVEBYNAME";
                }
                else
                {
                    return "exec hrm_hr_sp_get_Relativebyname";
                }
            }
        }

        public static string hrm_hr_sp_get_profilebyname
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_HR_SP_GET_PROFILEBYNAME";
                }
                else
                {
                    return "exec hrm_hr_sp_get_profilebyname";
                }
            }
        }


        /// <summary> Lấy danh sách HDTJob (store : hrm_hr_sp_get_HDTJob)  </summary>
        //public const string hrm_hr_sp_get_HDTJob = "exec hrm_hr_sp_get_HDTJob";
        public static string hrm_hr_sp_get_HDTJob
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_HDTJOB";
                }
                else
                {
                    return "exec hrm_hr_sp_get_HDTJob";
                }
            }
        }



        public static string hrm_hr_sp_get_ReportSumaryHDTProfile
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTSUMHDTPRO";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ReportSumaryHDTProfile";
                }
            }
        }

        public static string hrm_hr_sp_get_HDTJobWaiting
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_HR_SP_GET_HDTJOBWAITING";
                }
                else
                {
                    return "exec hrm_hr_sp_get_HDTJobWaiting";
                }
            }
        }

        public static string hrm_hr_sp_get_HDTJobOut
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_HDTJOBOUT";
                }
                else
                {
                    return "exec hrm_hr_sp_get_HDTJobOut";
                }
            }
        }
        /// <summary> Lấy đối tượng HDTJob (store : hrm_hr_sp_get_HDTJobById)  </summary>
        //public const string hrm_hr_sp_get_HDTJobById = "exec hrm_hr_sp_get_HDTJobById @hdtjobId";
        public static string hrm_hr_sp_get_HDTJobById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_HDTJOBBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_HDTJobById @Id";
                }
            }
        }

        public static string hrm_hr_sp_set_ApprovedAllHDTJob
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_SET_APPROVEDALLHDTJOB";
                }
                else
                {
                    return "exec hrm_hr_sp_set_ApprovedAllHDTJob";
                }
            }
        }

        /// <summary>
        /// Lấy đối tượng WorkHistory bởi ID
        /// </summary>
        //public const string hrm_hr_sp_get_WorkHistoryById = "exec hrm_hr_sp_get_WorkHistoryById @workHistoryId";
        public static string hrm_hr_sp_get_HDTJobsByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_HDTJOBSBYPROID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_HDTJobsByProfileId @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_ElementByPayrollID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ELEMENTBYGRADEID";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ElementByPayrollID";
                }
            }
        }

        public static string hrm_cat_sp_get_AdvancePaymentByPayrollID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ADVPAYMENTBYPAYROLL";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AdvancePaymentByPayrollID";
                }
            }
        }


        /// <summary>
        /// [Tho.Bui]: lấy đối tượng Reward bời ProfileID
        /// </summary>
        //public const string hrm_hr_sp_get_WorkHistoryById = "exec hrm_hr_sp_get_WorkHistoryById @workHistoryId";
        public static string hrm_hr_sp_get_ReWardByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_REWARDBYPROID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RewardByProfileId";
                }
            }
        }

        /// <summary>
        /// [Tin.Nguyen]: lấy đối tượng Qualification bời ProfileID
        public static string hrm_hr_sp_get_QualificationByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_QUALIFICAITONBYPROID(:profileId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_QualificationByProfileId @profileId";
                }
            }
        }

        /// <summary>
        /// [Tho.Bui]: lấy đối tượng discipline bời ProfileID
        /// </summary>
        public static string hrm_hr_sp_get_DisciplineprofileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_DISCIPLINEPROID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_DisciplineprofileId @Id";
                }
            }
        }

        /// <summary>
        /// [Tho.Bui]: Lấy đối tượng ProfilePartyUnion bởi profileID
        /// </summary>
        public static string hrm_hr_sp_get_ProfilePartyUnionprofileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PARTYUNIONPROID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfilePartyUnionprofileId";
                }
            }
        }
        public static string hrm_hr_sp_get_ComputingprofileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_COMPUTINGBYPROID(:profileId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ComputingprofileId @profileId";
                }
            }
        }

        /// <summary>
        /// [Tho.Bui]: lấy đối tượng Accident bời ProfileId
        /// </summary>
        public static string hrm_hr_sp_get_AccidentprofileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_ACCIDENTPROID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_AccidentprofileId";
                }
            }
        }

        public static string hrm_hr_sp_get_LanguageprofileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_LANGUAGEBYPROID(:profileId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_LanguageprofileId @profileId";
                }
            }
        }
        /// <summary> Lấy danh sách đối tượng Reward (store : hrm_hr_sp_get_RewardByProfileId) </summary>
        public const string hrm_hr_sp_get_RewardByProfileId = "exec hrm_hr_sp_get_RewardByProfileId @profileId";

        /// <summary> Lấy danh sách đối tượng Discipline (store : hrm_hr_sp_get_DisciplineByProfileId) </summary>
        public const string hrm_hr_sp_get_DisciplineByProfileId = "exec hrm_hr_sp_get_DisciplineByProfileId @profileId";

        /// <summary> Lấy danh sách đối tượng Relative (store : hrm_hr_sp_get_RelativesByProfileId) </summary>
        public const string hrm_hr_sp_get_RelativesByProfileId = "exec hrm_hr_sp_get_RelativesByProfileId @profileId";

        /// <summary> Lấy danh sách đối tượng HDTJob (store : hrm_hr_sp_get_HDTJobByProfileId) </summary>
        public const string hrm_hr_sp_get_HDTJobByProfileId = "exec hrm_hr_sp_get_HDTJobByProfileId @profileId";


        /// <summary> Lấy danh sách đối tượng Accident (store : hrm_hr_sp_get_AccidentByIds) </summary>
        //public const string hrm_hr_sp_get_AccidentByIds = "exec hrm_hr_sp_get_AccidentByIds @selectedIds";
        public static string hrm_hr_sp_get_AccidentByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_ACCIDENTBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_AccidentByIds @Ids";
                }
            }
        }
        /// <summary> Lấy danh sách đối tượng Address (store : hrm_hr_sp_get_AddressByIds) </summary>
        //public const string hrm_hr_sp_get_AddressByIds = "exec hrm_hr_sp_get_AddressByIds @selectedIds";
        public static string hrm_hr_sp_get_AddressByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_ADDRESSBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_AddressByIds @Ids";
                }
            }
        }
        /// <summary> Lấy danh sách đối tượng CardHistory (store : hrm_hr_sp_get_CardHistoryByIds) </summary>
        //public const string hrm_hr_sp_get_CardHistoryByIds = "exec hrm_hr_sp_get_CardHistoryByIds @selectedIds";
        public static string hrm_hr_sp_get_CardHistoryByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_CARDHISTORYBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_CardHistoryByIds @Ids";
                }
            }
        }
        /// <summary> Lấy danh sách đối tượng Contract (store : hrm_hr_sp_get_ContractByIds) </summary>
        //public const string hrm_hr_sp_get_ContractByIds = "exec hrm_hr_sp_get_ContractByIds @selectedIds";
        public static string hrm_hr_sp_get_ContractByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_CONTRACTBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ContractByIds @Ids";
                }
            }
        }
        /// <summary> Lấy danh sách đối tượng Dependant (store : hrm_hr_sp_get_DependantByIds) </summary>
        //public const string hrm_hr_sp_get_DependantByIds = "exec hrm_hr_sp_get_DependantByIds @selectedIds";
        public static string hrm_hr_sp_get_DependantByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_DEPENDANTBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_DependantByIds @Ids";
                }
            }
        }
        /// <summary> Lấy danh sách đối tượng Discipline (store : hrm_hr_sp_get_DisciplineByIds) </summary>
        //public const string hrm_hr_sp_get_DisciplineByIds = "exec hrm_hr_sp_get_DisciplineByIds @selectedIds";
        public static string hrm_hr_sp_get_DisciplineByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_DISCIPLINEBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_DisciplineByIds @Ids";
                }
            }
        }
        //[Tho.Bui]: Export Word
        public static string hrm_hr_sp_get_TempleteDisciplineByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_DISCIPLINEBYIDS";
                }
                else
                {
                    return "exec hrm_hr_sp_get_TempleteDisciplineByIds";
                }
            }
        }
        /// <summary> Lấy danh sách đối tượng HDTJob (store : hrm_hr_sp_get_HDTJobByIds) </summary>
        //public const string hrm_hr_sp_get_HDTJobByIds = "exec hrm_hr_sp_get_HDTJobByIds @selectedIds";
        public static string hrm_hr_sp_get_HDTJobByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_HDTJOBBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_HDTJobByIds @Ids";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Reward (store : hrm_hr_sp_get_RewardByIds) </summary>
        //public const string hrm_hr_sp_get_RewardByIds = "exec hrm_hr_sp_get_RewardByIds @selectedIds";
        public static string hrm_hr_sp_get_RewardByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_REWARDBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RewardByIds @Ids";
                }
            }
        }
        public static string hrm_hr_sp_get_TempleteRewardByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_REWARDBYIDS";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RewardByIds";
                }
            }
        }
        //[Tho.Bui]: Export select profile party and union on grid
        public static string hrm_hr_sp_get_ProfilePartyUnionByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PROPARUNIBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfilePartyUnionByIds @Ids";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng SoftSkill (store : hrm_hr_sp_get_SoftSkillByIds) </summary>
        //public const string hrm_hr_sp_get_SoftSkillByIds = "exec hrm_hr_sp_get_SoftSkillByIds @selectedIds";
        public static string hrm_hr_sp_get_SoftSkillByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_SOFTSKILLBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_SoftSkillByIds @Ids";
                }
            }
        }
        /// <summary> Lấy danh sách đối tượng Uniform (store : hrm_hr_sp_get_UniformByIds) </summary>
        //public const string hrm_hr_sp_get_UniformByIds = "exec hrm_hr_sp_get_UniformByIds @selectedIds";
        public static string hrm_hr_sp_get_UniformByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_UNIFORMBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_UniformByIds @Ids";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Uniform (store : hrm_hr_sp_get_UniformById) </summary>
        //public const string hrm_hr_sp_get_UniformById = "exec hrm_hr_sp_get_UniformById @uniformId";
        public static string hrm_hr_sp_get_UniformById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_UNIFORMBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_UniformById @Id";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng WorkHistory (store : hrm_hr_sp_get_WorkHistoryByIds) </summary>
        //public const string hrm_hr_sp_get_WorkHistoryByIds = "exec hrm_hr_sp_get_WorkHistoryByIds @selectedIds";
        public static string hrm_hr_sp_get_WorkHistoryByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_WORKHISTORYBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_WorkHistoryByIds @Ids";
                }
            }
        }
        /// <summary>
        /// lấy danh sách Contract theo chuổi ProfileID.
        /// </summary>
        public static string hrm_hr_sp_get_ContractBYProIDs
        {
            get 
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_CONTRACTBYPROIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ContractBYProIDs @Ids";
                }
            }
        }
        public static string hrm_hr_sp_get_ExportWorkHistoryByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_WORKHISTORYBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_WorkHistoryByIds @Ids";
                }
            }
        }
        #region Hre_SoftSkill
        /// <summary> Lấy danh sách SoftSkill (store : hrm_hr_sp_get_SoftSkill) </summary>
        //public const string hrm_hr_sp_get_SoftSkill = "exec hrm_hr_sp_get_SoftSkill";
        public static string hrm_hr_sp_get_SoftSkill
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_SOFTSKILL";
                }
                else
                {
                    return "exec hrm_hr_sp_get_SoftSkill";
                }
            }
        }
        /// <summary> Lấy đối tượng SoftSkill theo  SoftSkillId </summary>
        //public const string hrm_hr_sp_get_SoftSkillById = "exec hrm_hr_sp_get_SoftSkillById @softSkillId";
        public static string hrm_hr_sp_get_SoftSkillById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_SOFTSKILLBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_SoftSkillById @Id";
                }
            }
        }
        /// <summary> Lấy đối tượng SoftSkill theo  profileId </summary>
        //public const string hrm_hr_sp_get_SoftSkillByprofileId = "exec hrm_hr_sp_get_SoftSkillByprofileId @profileId";
        public static string hrm_hr_sp_get_SoftSkillByprofileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_SOFTSKILLBYPROID(:profileId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_SoftSkillByprofileId @profileId";
                }
            }
        }
        #endregion

        #region Hre_Accident

        /// <summary> Lấy danh sách Accident (store : hrm_hr_sp_get_Accident) </summary>
        //public const string hrm_hr_sp_get_Accident = "exec hrm_hr_sp_get_Accident";
        public static string hrm_hr_sp_get_Accident
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_ACCIDENT";
                }
                else
                {
                    return "exec hrm_hr_sp_get_Accident";
                }
            }
        }
        /// <summary> Lấy đối tượng Accident theo AccidentId </summary>
        //public const string hrm_hr_sp_get_AccidentById = "exec hrm_hr_sp_get_AccidentById @accidentId";
        public static string hrm_hr_sp_get_AccidentById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_ACCIDENTBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_AccidentById @Id";
                }
            }
        }


        public static string hrm_hr_sp_get_PurchaseRequestById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "chua viet";
                }
                else
                {
                    return "exec hrm_hr_sp_get_PurchaseRequestById @Id";
                }
            }
        }
        public static string hrm_hr_sp_get_PurchaseRequestItemById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "chua viet";
                }
                else
                {
                    return "exec hrm_hr_sp_get_PurchaseRequestItemById @Id";
                }
            }
        }

        public static string hrm_hr_sp_get_PlanHeadCountById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PLANHEADCOUNTBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_PlanHeadCountById @Id";
                }
            }
        }

        /// <summary> Lấy đối tượng Accident theo  profileId </summary>
        //public const string hrm_hr_sp_get_AccidentByprofileId = "exec hrm_hr_sp_get_AccidentByprofileId @profileId";
        public static string hrm_hr_sp_get_AccidentByprofileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_ACCIDENTBYPROFILEID(:profileId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_AccidentByprofileId @profileId";
                }
            }
        }
        #endregion

        #region Hre_AppendixContract
        /// <summary>
        /// Lấy danh sách tất cả dữ liệu hre_AppendixContract
        /// </summary>
        //public const string hrm_hr_sp_get_AppendixContract = "exec hrm_hr_sp_get_AppendixContract";
        public static string hrm_hr_sp_get_AppendixContract
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_APPENDIXCONTRACT";
                }
                else
                {
                    return "exec hrm_hr_sp_get_AppendixContract";
                }
            }
        }


        //  chưa viết sql
        public static string hrm_hr_sp_get_AppendixExpiredContract
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_APPENDEXCONTRACT";
                }
                else
                {
                    return "exec hrm_hr_sp_get_AppendixExpiredContract";
                }
            }
        }

        /// <summary>
        /// Lấy đối tượng Contract bởi ID
        /// </summary>
        //public const string hrm_hr_sp_get_AppendixContractById = "exec hrm_hr_sp_get_AppendixContractById @contractId";
        public static string hrm_hr_sp_get_AppendixContractById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_APPENDCONTRACTBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_AppendixContractById @Id";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng AppendixContract (store : hrm_hr_sp_get_AppendixContractByIds) </summary>
        //public const string hrm_hr_sp_get_AppendixContractByIds = "exec hrm_hr_sp_get_AppendixContractByIds @selectedIds";
        public static string hrm_hr_sp_get_AppendixContractByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_APPCONTRACTBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_AppendixContractByIds @Ids";
                }
            }
        }


        public static string hrm_cat_sp_get_AppendixContractByContractID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_APENDIXBYCONTRACTID";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AppendixContractByContractID";
                }
            }
        }

        public static string hrm_cat_sp_get_PRItemByPRID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_PRItemBYPRID";
                }
                else
                {
                    return "exec hrm_cat_sp_get_PRItemByPRID";
                }
            }
        }
        public static string hrm_cat_sp_get_DepartmentIteByDepartmentID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "Chua vik";
                }
                else
                {
                    return "exec hrm_cat_sp_get_DepartmentIteByDepartmentID";
                }
            }
        }

        public static string hrm_cat_sp_get_AppendixContractType_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_APPBYCONTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AppendixContractType_multi @Keyword";
                }
            }
        }


        #endregion


        #region Hre_Report

        public static string hrm_hr_sp_get_RptProbationProfile
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTPROBATIONPROFILE";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RptProbationProfile";
                }
            }
        }

        public static string hrm_hr_sp_get_RptPrenancy
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTPREGNANCY";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RptPrenancy";
                }
            }
        }

        public static string hrm_hr_sp_get_RptQuitProfile
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTQUITPROFILE";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RptQuitProfile";
                }
            }
        }

        public static string hrm_hr_sp_get_RptWorkingProfile
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTWORKINGPROFILE";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RptWorkingProfile";
                }
            }
        }



        public static string hrm_hr_sp_get_RptHDTJob
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTHDTJOB";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RptHDTJob";
                }
            }
        }
        public static string hrm_hr_sp_get_RptCodeNotInSystem
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTCODENOTINSYSTEM";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RptCodeNotInSystem";
                }
            }
        }

        public static string hrm_hr_sp_get_RptBirthday
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPBIRTHDAY";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RptBirthday";
                }
            }
        }
        public static string hrm_hr_sp_get_RptDiscripline
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTDISCRIPLINE";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RptDiscripline";
                }
            }
        }
        public static string hrm_hr_sp_get_RptReward
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTREWARD";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RptReward";
                }
            }
        }
        public static string hrm_hr_sp_get_RptHistoryProfile
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTHISTORYPROFILE";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RptHistoryProfile";
                }
            }
        }
        public static string hrm_hr_sp_get_RptNotContract
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTNOTCONTRACT";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RptNotContract";
                }
            }
        }

        public static string hrm_hr_sp_get_RptOrgProfile
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_REPORTORPRO";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RptOrgProfile";
                }
            }
        }
        public static string hrm_hr_sp_get_RptWorkHistoryDept
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTWORKHISTORYDEPT";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RptWorkHistoryDept";
                }
            }
        }
        public static string hrm_hr_sp_get_RptWorkHistoryDeptByids
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTWHISTORYDEPTBYIDS";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RptWorkHistoryDeptByids";
                }
            }
        }

        public static string hrm_hr_sp_get_RptProfileNew
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTPROFILENEW";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RptProfileNew";
                }
            }
        }

        public static string hrm_hr_sp_get_RptExpireContract
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTEXPIRECONTRACT";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RptExpireContract";
                }
            }
        }

        public static string hrm_hr_sp_get_ExpireContract
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_EXPIRECONTRACT";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ExpireContract";
                }
            }
        }
        #endregion

        #endregion

        #region lấy data
        public static string hrm_att_getdata_Workday
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_GETDATA_WORKDAY";
                }
                else
                {
                    return "exec hrm_att_getdata_Workday";
                }
            }
        }
        public static string hrm_att_getdata_Overtime
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_GETDATA_OVERTIME";
                }
                else
                {
                    return "exec hrm_att_getdata_Overtime";
                }
            }
        }
        public static string hrm_hre_getdata_CardHistory
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRE_GETDATA_CARDHISTORY";
                }
                else
                {
                    return "exec hrm_hre_getdata_CardHistory";
                }
            }
        }
        public static string hrm_hre_getdata_WorkHistory
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRE_GETDATA_WORKHISTORY";
                }
                else
                {
                    return "exec hrm_hre_getdata_WorkHistory";
                }
            }
        }

        //bang.nguyen cap nhat status hre_profile .... va cac bang lien quan
        public static string hrm_hre_sp_Set_ApproveProfile_Status
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRE_SET_APPROVEPROSTATUS";
                }
                else
                {
                    return "exec hrm_hre_sp_Set_ApproveProfile_Status";
                }
            }
        }
        public static string hrm_att_getdata_Grade
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_GETDATA_GRADE";
                }
                else
                {
                    return "hrm_att_getdata_Grade";
                }
            }
        }
        public static string hrm_att_getdata_LeaveDay
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_GETDATA_LEAVEDAY";
                }
                else
                {
                    return "hrm_att_getdata_LeaveDay";
                }
            }
        }
        public static string hrm_att_getdata_LeaveDay_Inner
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_GETDATA_LEAVEDAY_INNER";
                }
                else
                {
                    return "hrm_att_getdata_LeaveDay_Inner";
                }
            }
        }
        public static string hrm_att_getdata_Roster
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_GETDATA_ROSTER";
                }
                else
                {
                    return "hrm_att_getdata_Roster";
                }
            }
        }
        public static string hrm_att_getdata_Roster_Inner
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_GETDATA_ROSTER_INNER";
                }
                else
                {
                    return "hrm_att_getdata_Roster_Inner";
                }
            }
        }
        public static string hrm_att_getdata_TamScanLog
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_GETDATA_TAMSCANLOG";
                }
                else
                {
                    return "hrm_att_getdata_TamScanLog";
                }
            }
        }
        #endregion

        #region Attendance

        /// <summary>
        /// Thiet lap trang thai cho roster
        /// </summary>
        public static string hrm_att_sp_Set_Roster_Status
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_SET_ROSTER_STATUS";
                }
                else
                {
                    return "hrm_att_sp_Set_Roster_Status";
                }
            }
        }

        /// <summary>
        /// Thiet lap trang thai cho overtime
        /// </summary>
        public static string hrm_att_sp_Set_Overtime_Status
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_SET_OVERTIME_STATUS";
                }
                else
                {
                    return "exec hrm_att_sp_Set_Overtime_Status";
                }
            }
        }

        public static string hrm_att_sp_Set_OvertimePortal_Status
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_SET_OTPORTAL_STATUS";
                }
                else
                {
                    return "exec hrm_att_sp_Set_OvertimePortal_Status";
                }
            }
        }

        /// <summary>
        /// Thiet lap phuong thuc thanh toan cho overtime
        /// </summary>
        public static string hrm_att_sp_Set_Overtime_Payment
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_SET_OVERTIME_PAYMENT";
                }
                else
                {
                    return "exec hrm_att_sp_Set_Overtime_Payment";
                }
            }
        }

        /// <summary>
        /// kiet.chung - Thiet lap AllowOvertime cho overtime
        /// </summary>
        public static string Att_sp_Set_Overtime_AllowOvertime
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_SET_OVERTIME_ALLOW";
                }
                else
                {
                    return "exec hrm_Att_sp_Set_Overtime_AllowOvertime";
                }
            }
        }

        /// <summary>
        /// Thiet lap trang thai cho roster group
        /// </summary>
        public static string hrm_att_sp_Set_RosterGroup_Status
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_SET_ROSTERGROUP_STATUS";
                }
                else
                {
                    return "hrm_att_sp_Set_RosterGroup_Status";
                }
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả dữ liệu nhân viên chưa có ca làm việc
        /// </summary>
        public static string hrm_att_sp_get_Roster_ProfileNotRoster
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_PROFILENOTROSTER";
                }
                else
                {
                    return "exec hrm_att_sp_get_Roster_ProfileNotRoster";
                }
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_AnnualLeaveDetail
        /// </summary>
        public static string hrm_att_sp_get_AnnualLeaveDetail
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_ANNUALLEAVEDETAIL";
                }
                else
                {
                    return "exec hrm_att_sp_get_AnnualLeaveDetail";
                }
            }
        }


        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_AllowLimitOvertime
        /// </summary>
        public static string hrm_att_sp_get_AllowLimitOvertime
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_ALLOWLIMITOVERTIME";
                }
                else
                {
                    return "exec hrm_att_sp_get_AllowLimitOvertime";
                }
            }
        }
        public static string hrm_att_getdata_AllowLimitOvertime
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_GETDATA_ALLOWLIMITOVERTIME";
                }
                else
                {
                    return "exec hrm_att_getdata_AllowLimitOvertime";
                }
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_AllowLimitOvertimebyid
        /// </summary>
        public static string hrm_att_sp_get_AllowLimitOvertimebyid
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_ALLOWLIMITOTBYID(:AllowLimitOvertimebyid,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_AllowLimitOvertimebyid @allowLimitOvertimebyid";
                }
            }
        }


        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_InOut
        /// </summary>
        public const string hrm_att_sp_get_InOut = "exec hrm_att_sp_get_InOut";

        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_Overtime
        /// </summary>
        //public const string hrm_att_sp_get_Overtime = "exec hrm_att_sp_get_Overtime @ProfileName, @RegisterHours, @PageIndex, @PageSize";
        public static string hrm_att_sp_get_Overtime
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_OVERTIME";
                }
                else
                {
                    return "exec hrm_att_sp_get_Overtime";
                }
            }
        }


        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_Roster
        /// </summary>
        public static string hrm_att_sp_get_Roster
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_ROSTER";
                }
                else
                {
                    return "exec hrm_att_sp_get_Roster";
                }
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_RosterGroup
        /// </summary>
        public static string hrm_att_sp_get_RosterGroup
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_ROSTERGROUP";
                }
                else
                {
                    return "exec hrm_att_sp_get_RosterGroup";
                }
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_TAMScanLog
        /// </summary>
        public static string hrm_att_sp_get_TAMScanLog
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_TAMSCANLOG";
                }
                else
                {
                    return "exec hrm_att_sp_get_TAMScanLog";
                }
            }
        }


        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_Pregnancy
        /// </summary>
        public static string hrm_att_sp_get_Pregnancy
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_PREGNANCY";
                }
                else
                {
                    return "exec hrm_att_sp_get_Pregnancy";
                }
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_Leaveday
        /// </summary>
        public static string hrm_att_sp_get_Leaveday
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_LEAVEDAY";
                }
                else
                {
                    return "exec hrm_att_sp_get_Leaveday";
                }
            }

        }

        public static string hrm_att_sp_get_LeavedayPortal
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_LEAVEDAYPORTAL";
                }
                else
                {
                    return "exec hrm_att_sp_get_LeavedayPortal";
                }
            }
        }


        /// <summary> Lấy danh sách máy chấm công bởi TamID </summary>
        public static string hrm_att_sp_get_TAMScanLogById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_TAMSCANLOGBYID(:TamID,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_TAMScanLogById @Id";
                }
            }
        }

        /// <summary> Lấy danh sách máy chấm công bởi profileId và CutOffID </summary>
        public static string hrm_att_sp_get_TAMScanLogByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_TAMSCANLOGBYPROID";
                }
                else
                {
                    return "exec hrm_att_sp_get_TAMScanLogByProfileId";
                }
            }
        }



        /// <summary> Lấy danh sách lịch làm việc bởi profileId và CutOffID </summary>
        public static string hrm_att_sp_get_RosterByProIDandCutOID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_ROSTERBYPROANDCUT";
                }
                else
                {
                    return "exec hrm_att_sp_get_RosterByProIDandCutOID";
                }
            }
        }


        /// <summary> Lấy danh sách ngày nghì bởi profileId và CutOffID </summary>
        //public const string hrm_att_sp_get_LeavedayByProIDandCutOID = "exec hrm_att_sp_get_LeavedayByProIDandCutOID @ProfileID, @CutOffID";
        public static string hrm_att_sp_get_LeavedayByProIDandCutOID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_LEAVEDAYBYPROCUT";
                }
                else
                {
                    return "exec hrm_att_sp_get_LeavedayByProIDandCutOID";
                }
            }
        }
        /// <summary> Lấy danh sách ngày công bởi profileId và CutOffID </summary>
        public static string hrm_att_sp_get_WorkDayByProIDandCutOID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_WDBYPROIDANDCUTOID";
                }
                else
                {
                    return "exec hrm_att_sp_get_WorkDayByProIDandCutOID";
                }
            }
        }



        /// <summary> Lấy danh sách tăng ca bởi profileId và CutOffID </summary>
        public static string hrm_att_sp_get_OvertimeByProIDandCutOID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_OVERTIMEBYPROCUT";
                }
                else
                {
                    return "exec hrm_att_sp_get_OvertimeByProIDandCutOID";
                }
            }
        }

        /// <summary> Lấy danh sách chi tiết chấm công bởi profileId và CutOffID </summary>
        public static string hrm_att_sp_get_AttendanceTableItemByProIDandCutOID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_ATTTABLEITEMPROCUT";
                }
                else
                {
                    return "exec hrm_att_sp_get_AttendanceTableItemByProIDandCutOID";
                }
            }
        }

        /// <summary> Lấy danh sách attendancetable bởi profileId và CutOffID </summary>
        public static string hrm_att_sp_get_AttendanceTableByProIDandCutOID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_ATTTABLEBYPROCUT";
                }
                else
                {
                    return "exec hrm_att_sp_get_AttendanceTableByProIDandCutOID";
                }
            }
        }


        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_WorkDay
        /// </summary>
        public static string hrm_att_sp_get_WorkDay
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_WORKDAY";
                }
                else
                {
                    return "exec hrm_att_sp_get_WorkDay";
                }
            }
        }


        /// <summary> Lấy danh sách overtime bởi overtimeId </summary>
        public static string hrm_att_sp_get_OvertimeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_OVERTIMEBYID(:OvertimeId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_OvertimeById @OvertimeId";
                }
            }

        }

        public static string hrm_hr_sp_get_Notification
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "";
                }
                else
                {
                    return "exec hrm_hr_sp_get_Notification @userLogin";
                }
            }
        }



        /// <summary> Lấy danh sách overtime bởi profileId </summary>
        //public static string hrm_att_sp_get_OvertimeByProfileId
        //{
        //    get
        //    {
        //        if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //        {
        //            return "begin ATT_SP_GET_OVERTIMEBYPROID(:profileId,:R_Output); end;";
        //        }
        //        else
        //        {
        //            return "exec hrm_att_sp_get_OvertimeByProfileId @profileId";
        //        }
        //    }
        //}

        public static string hrm_att_sp_get_OvertimeByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_OVERTIMEBYPROID";
                }
                else
                {
                    return "exec hrm_att_sp_get_OvertimeByProfileId";
                }
            }
        }

        /// <summary> Lấy danh sách Roster bởi RosterId </summary>
        public static string hrm_att_sp_get_RosterById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_ROSTERBYID(:RosterId,:R_Output); end;";
                    //return "ATT_SP_GET_ROSTERBYID";
                }
                else
                {
                    return "exec hrm_att_sp_get_RosterById @RosterId";
                }
            }
        }


        /// <summary> Lấy danh sách Roster bởi RosterId </summary>
        public static string hrm_att_sp_get_RosterByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_ROSTERBYPROFILEID(:ProfileId,:R_Output); end;";
                    // return "ATT_SP_GET_ROSTERBYPROFILEID";
                }
                else
                {
                    return "exec hrm_att_sp_get_RosterByProfileId @ProfileId";
                }
            }
        }


        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_Address
        /// </summary>        
        public static string hrm_hr_sp_get_Address
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_ADDRESS";
                }
                else
                {
                    return "exec hrm_hr_sp_get_Address";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_Address by ID
        /// </summary>        
        public static string hrm_hr_sp_get_AddressById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_ADDRESSBYID(:addressId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_AddressById @Id";
                }
            }
        }
        /// <summary>
        /// Lấy danh sách tas61t cả dữ liệu AnnualLeave theo Id
        /// </summary>
        public static string hrm_att_sp_get_AnnualLeaveById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_ANNUALLEAVEBYID(:annualleaveId,:R_Output); end;";
                    // return "ATT_SP_GET_ANNUALLEAVEBYID";
                }
                else
                {
                    return "exec hrm_att_sp_get_AnnualLeaveById @annualleaveId";
                }
            }
        }


        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_AnnualLeave
        /// </summary>
        public static string hrm_att_sp_get_AnnualLeave
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_ANNUALLEAVE";
                }
                else
                {
                    return "exec hrm_att_sp_get_AnnualLeave";
                }
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_AnnualLeave
        /// </summary>
        public static string hrm_att_sp_get_AnnualDetail
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_ANNUALDETAIL";
                }
                else
                {
                    return "exec hrm_att_sp_get_AnnualDetail";
                }
            }
        }

        #region Att_CompensationDetail
        public static string hrm_att_sp_get_CompensationDetail
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_COMPENSATIONDETAIL";
                }
                else
                {
                    return "exec hrm_att_sp_get_CompensationDetail";
                }
            }
        }
        #endregion


        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_AttendanceTable
        /// </summary>
        public static string hrm_att_sp_get_AttendanceTable
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_ATTTABLE";
                }
                else
                {
                    return "exec hrm_att_sp_get_AttendanceTable";
                }
            }
        }


        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_AttendanceTable by id
        /// </summary>
        public static string hrm_att_sp_get_AttendanceTableById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_ATTTABLEBYID(:attendanceTableId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_AttendanceTableById @attendanceTableId";
                }
            }
        }


        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_AttendanceTableByAttendanceTableId by AttendanceTableId
        /// </summary>
        public const string hrm_att_sp_get_AttendanceTableByAttendanceTableId = "exec hrm_att_sp_get_AttendanceTableByAttendanceTableId @AttendanceTableId";

        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_AttendanceTableByProfileIdId by ProfileId
        /// </summary>
        public static string hrm_att_sp_get_AttendanceTableByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_ATTTABLEBYPROFILEID(:ProfileId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_AttendanceTableByProfileId @ProfileId";
                }
            }
        }



        public static string att_get_AttTable_CfId_PfId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_GET_ATTTABLE_CFID_PFID(:profileId,:cutOffId,:R_Output); end;";
                    // return "ATT_SP_GET_ANNUALLEAVEBYID";
                }
                else
                {
                    return "exec att_get_AttTable_CfId_PfId @profileId, @cutOffId";
                }
            }
        }




        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_AttendanceTableItem
        /// </summary>
        public static string hrm_att_sp_get_AttendanceTableItem
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_ATTTABLEITEM";
                }
                else
                {
                    return "exec hrm_att_sp_get_AttendanceTableItem";
                }
            }
        }
        public static string hrm_att_sp_getdata_AttendanceTableItem
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GETDATA_ATTTABLEITEM";
                }
                else
                {
                    return "exec hrm_att_sp_getdata_AttendanceTableItem";
                }
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_AttendanceTableItem By AttendanceTableId
        /// </summary>
        public static string hrm_att_sp_get_AttendanceTableItemByAttendanceTableId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_TABLEITEMBYTABLEID(:AttendanceTableId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_AttendanceTableItemByAttendanceTableId @AttendanceTableId";
                }
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Cat_Shift
        /// </summary>

        // public const string hrm_cat_sp_get_Shift = "exec hrm_cat_sp_get_Shift";
        public static string hrm_cat_sp_get_Shift
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_SHIFT";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Shift";
                }
            }
        }

        public static string hrm_cat_sp_get_ShiftById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SHIFTBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ShiftById @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_ShiftByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SHIFTBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ShiftByIds @Ids";
                }
            }
        }




        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Cat_ShiftItem
        /// </summary>
        public const string hrm_cat_sp_get_ShiftItem = "exec hrm_cat_sp_get_ShiftItem";

        /// <summary>
        /// Lấy danh sách tất cả dữ liệu WorkDayByInOut
        /// </summary>
        public static string hrm_att_sp_get_WorkDayByInOut
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_WORKDAYBYINOUT(:inTime,:outTime,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_WorkDayByInOut @inTime, @outTime";
                }
            }
        }


        /// <summary>
        /// Lấy danh sách tất cả dữ liệu InOut by ProfileId
        /// </summary>
        public const string hrm_att_sp_get_InOutByProfileId = "exec hrm_att_sp_get_InOutByProfileId  @profileId";
        /// <summary>
        /// Lấy danh sách tất cả dữ liệu WorkDay by ProfileId
        /// </summary>
        public static string hrm_att_sp_get_WorkDayByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_WORKDAYBYPROID(:profileId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_WorkDayByProfileId @profileId";
                }
            }
        }

        public static string hrm_att_sp_get_WorkDayByProfileIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_WORKDAYBYPROFILEIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_WorkDayByProfileIds @Ids";
                }
            }
        }

        //public static string hrm_att_sp_get_LeaveDayByProfileIds
        //{
        //    get
        //    {
        //        if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //        {
        //            return "begin ATT_SP_GET_LEAVEBYPROFILEIDS(:Ids,:R_Output); end;";
        //        }
        //        else
        //        {
        //            return "exec hrm_att_sp_get_LeaveDayByProfileIds @Ids";
        //        }
        //    }
        //}
        public static string hrm_att_sp_get_LeaveDayByProfileIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_LEAVEBYPROFILEIDS";
                }
                else
                {
                    return "exec hrm_att_sp_get_LeaveDayByProfileIds";
                }
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả dữ liệu WorkDay by Id
        /// </summary>
        public static string hrm_att_sp_get_WorkDayById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_WORKDAYBYID(:workdayID,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_WorkDayById @workdayID";
                }
            }
        }

        public static string hrm_att_sp_get_LeaveDayById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_LEAVEDAYBYID(:Leavedaybyid,:R_Output); end;";
                }
                else
                {

                    return "exec hrm_att_sp_get_LeaveDayById @Id";
                }
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Hre_Uniform
        /// </summary>
        //public const string hrm_hr_sp_get_Uniform = "exec hrm_hr_sp_get_Uniform";
        public static string hrm_hr_sp_get_Uniform
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_UNIFORM";
                }
                else
                {
                    return "exec hrm_hr_sp_get_Uniform";
                }
            }
        }


        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Hre_Certificate
        /// </summary>
        public const string hrm_hr_sp_get_Certificate = "exec hrm_hr_sp_get_Certificate";

        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Hre_Certificate by ID
        /// </summary>
        public const string hrm_hr_sp_get_CertificateById = "exec hrm_hr_sp_get_CertificateById @certificateId";

        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Hre_Qualification
        /// </summary>
        //public const string hrm_hr_sp_get_Qualification = "exec hrm_hr_sp_get_Qualification";
        public static string hrm_hr_sp_get_Qualification
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_QUALIFICATION";
                }
                else
                {
                    return "exec hrm_hr_sp_get_Qualification";
                }
            }
        }

        public static string hrm_hr_sp_get_PurchaseRequest
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PURCHASEREQUEST";
                }
                else
                {
                    return "exec hrm_hr_sp_get_PurchaseRequest";
                }
            }
        }
        public static string hrm_hr_sp_get_PurchaseRequestByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PURCHASEREQUESTBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_hr_sp_get_PurchaseRequestByIds @ID";
                }
            }
        }
        public static string hrm_hr_sp_get_ProfileQualification
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROQUALIFICATION";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileQualification";
                }
            }
        }

        /// <summary> Lấy danh sách CardCode bởi profileId </summary>
        public static string hrm_hre_sp_get_Profile_CardCodeByProfileID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PRO_CARDCODEBYPROID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hre_sp_get_Profile_CardCodeByProfileID @Id";
                }
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Hre_Qualification by ID
        /// </summary>
        //public const string hrm_hr_sp_get_QualificationById = "exec hrm_hr_sp_get_QualificationById @qualificationId";

        public static string hrm_hr_sp_get_QualificationById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_QUALIFICATIONBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_QualificationById @Id";
                }
            }
        }
        public static string hrm_hr_sp_get_ProfileQualificationById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_QUALIFIBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileQualificationById @Id";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileLanguageById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_LANGUAGEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileLanguageById @Id";
                }
            }
        }
        public static string hrm_hr_sp_get_ProfileComputingById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_COMPUTINGBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileComputingById @Id";
                }
            }
        }


        /// <summary> Lấy danh sách đối tượng Qualification (store : hrm_hr_sp_get_QualificationByIds) </summary>
        //public const string hrm_hr_sp_get_QualificationByIds = "exec hrm_hr_sp_get_QualificationByIds @selectedIds";
        public static string hrm_hr_sp_get_QualificationByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_QUALIFICATIONBYIDS(:Ids,:R_Output); end;";
                    //  return "ATT_SP_GET_ANNUALLEAVEBYIDS";
                }
                else
                {
                    return "exec hrm_hr_sp_get_QualificationByIds @Ids";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileQualificationByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PROQUALIFIBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileQualificationByIds @Ids";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng AllowLimitOvertime (store : hrm_att_sp_get_AllowLimitOvertimeByIds) </summary>
        public static string hrm_att_sp_get_AllowLimitOvertimeByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_ALLOWLIMITOTBYIDS(:selectedIds,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_AllowLimitOvertimeByIds @selectedIds";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Att_AnnualLeave (store : hrm_att_sp_get_AnnualLeaveByIds) </summary>
        public static string hrm_att_sp_get_AnnualLeaveByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_ANNUALLEAVEBYIDS(:selectedIds,:R_Output); end;";
                    //  return "ATT_SP_GET_ANNUALLEAVEBYIDS";
                }
                else
                {
                    return "exec hrm_att_sp_get_AnnualLeaveByIds @selectedIds";
                }
            }
        }



        /// <summary> Lấy danh sách đối tượng Att_AttendanceTable (store : hrm_att_sp_get_AttendanceTableByIds) </summary>
        public const string hrm_att_sp_get_AttendanceTableByIds = "exec hrm_att_sp_get_AttendanceTableByIds @selectedIds";
        /// <summary> Lấy danh sách đối tượng Att_AttendanceTableEntity bởi store [hrm_att_sp_get_AttendanceTable_ByFilter] </summary>
        public static string hrm_att_sp_get_AttendanceTable_ByFilter
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_ATTTABLEBYFILTER(:MonthYear,:CutOffDurationID,:strProfileID,:R_Output); end;";
                    // return "ATT_SP_GET_ANNUALLEAVEBYID";
                }
                else
                {
                    return "exec hrm_att_sp_get_AttendanceTable_ByFilter @MonthYear,@CutOffDurationID,@strProfileID";
                }
            }
        }


        /// <summary> Lấy danh sách đối tượng Att_AttendanceTableItem bởi store [hrm_att_sp_get_AttendanceTableItem_ByAttendanceTable_StrId] </summary>
        public static string hrm_att_sp_get_AttendanceTableItem_ByAttendanceTable_StrId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_TABLEITEMBYTABLEITEMID(:strAttendanceTableIds,:R_Output); end;";
                }
                else
                {
                    return "exec [hrm_att_sp_get_AttendanceTableItem_ByAttendanceTable_StrId] @strAttendanceTableIds";
                }
            }
        }

        public static string hrm_att_sp_get_AttendanceTableItemById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_TABLEITEMBYID(:AttendanceTableItemById,:R_Output); end;";
                }
                else
                {
                    return "exec [hrm_att_sp_get_AttendanceTableItemById] @AttendanceTableItemById ";
                }
            }
        }


        /// <summary>
        /// Lấy danh sách tất cả dữ liệu Att_CutOffDuration
        /// </summary>      
        public static string hrm_att_sp_get_CutOffDurations
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_CUTOFFDURATIONS";
                }
                else
                {
                    return "exec hrm_att_sp_get_CutOffDurations";
                }
            }
        }



        /// <summary> Lấy danh sách đối tượng Att_CutOffDuration (store : hrm_hr_sp_get_CutOffDurationByIds) </summary>
        public static string hrm_att_sp_get_CutOffDurationByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_CUTOFFDURATIONBYIDS(:selectedIds,:R_Output); end;";
                    //  return "ATT_SP_GET_CUTOFFDURATIONBYIDS";
                }
                else
                {
                    return "exec hrm_att_sp_get_CutOffDurationByIds @selectedIds";
                }
            }
        }


        /// <summary> Lấy danh sách đối tượng Att_WorkDay (store : hrm_hr_sp_get_WorkDayByIds) </summary>
        public static string hrm_att_sp_get_WorkDayByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_WORKDAYBYIDS(:selectedIds,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_WorkDayByIds @selectedIds";
                }
            }
        }


        /// <summary> Update status Att_WorkDay (store : hrm_att_sp_get_WorkDay_UpdateStatus) </summary>
        public static string hrm_att_sp_get_WorkDay_UpdateStatus
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_WD_UPDATESTATUS";
                }
                else
                {
                    return "exec hrm_att_sp_get_WorkDay_UpdateStatus";
                }
            }
        }

        /// <summary> Update status Att_WorkDay (store : hrm_att_sp_set_WorkDay_lateearlyDuration) </summary>
        public static string hrm_att_sp_set_WorkDay_LateEarlyDuration
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_SET_WORKDAY_LATEEARLY";
                }
                else
                {
                    return "exec hrm_att_sp_set_WorkDay_LateEarlyDuration";
                }
            }
        }

        /// <summary> lấy intime outtime của cat_shift update cho att_workday </summary>
        public static string hrm_att_sp_set_WorkDay_addInOut
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_SET_WORKDAY_ADDINOUT";
                }
                else
                {
                    return "exec hrm_att_sp_set_WorkDay_addInOut";
                }
            }
        }

        /// <summary> thay đổi vị trí in và out </summary>
        public static string hrm_att_sp_set_WorkDay_ChangeInOut
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_SET_WORKDAY_CHANGEINOUT";
                }
                else
                {
                    return "exec hrm_att_sp_set_WorkDay_ChangeInOut";
                }
            }
        }


        /// <summary> Lấy danh sách đối tượng Att_WorkDay (store : hrm_att_sp_get_TAMScanLogByIds) </summary>
        public static string hrm_att_sp_get_TAMScanLogByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_TAMSCANLOGBYIDS(:selectedIds,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_TAMScanLogByIds @selectedIds";
                }
            }
        }


        /// <summary> Lưu xác nhận tăng ca (store : hrm_att_sp_save_OvertimeConfirm) </summary>
        public static string hrm_att_sp_save_OvertimeConfirm
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_SAVE_OVERTIMECOMFIRM(:OvertimeId,:ConfirmHours); end;";
                }
                else
                {
                    return "exec hrm_att_sp_save_OvertimeConfirm @OvertimeId,@ConfirmHours";
                }
            }
        }



        /// <summary> Lấy danh sách đối tượng Att_Grade (store : hrm_att_sp_get_GradeByIds) </summary>
        public static string hrm_att_sp_get_GradeByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_GRADEBYIDS(:selectedIds,:R_Output); end;";
                    //  return "ATT_SP_GET_GRADEBYIDS";
                }
                else
                {
                    return "exec hrm_att_sp_get_GradeByIds @selectedIds";
                }
            }
        }


        /// <summary> Lấy danh sách đối tượng Att_Roster (store : hrm_att_sp_get_RosterByIds) </summary>
        public static string hrm_att_sp_get_RosterByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_ROSTERBYIDS(:selectedIds,:R_Output); end;";
                    //  return "ATT_SP_GET_ROSTERBYIDS";
                }
                else
                {
                    return "exec hrm_att_sp_get_RosterByIds @selectedIds";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Att_ProfileNotRoster (store : hrm_att_sp_get_Roster_ProfileNotRosterByIds) </summary>
        public static string hrm_att_sp_get_Roster_ProfileNotRosterByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_PROFILENOTROSTERIDS(:selectedIds,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_Roster_ProfileNotRosterByIds @selectedIds";
                }
            }
        }

        /// <summary> Lấy danh sách pregnancy bởi Id </summary>
        public static string hrm_att_sp_get_PregnancyById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_PREGNANCYBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_PregnancyById @Id";
                }
            }
        }

        public static string hrm_att_sp_get_RosterGroupById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_ROSTERGROUPBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_RosterGroupById @Id";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Att_RosterGroup (store : hrm_att_sp_get_RosterGroupByIds) </summary>
        public static string hrm_att_sp_get_RosterGroupByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_ROSTERGROUPBYIDS(:selectedIds,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_RosterGroupByIds @selectedIds";
                }
            }
        }


        /// <summary> Lấy danh sách đối tượng Att_Overtime (store : hrm_att_sp_get_OvertimeByIds) </summary>
        public static string hrm_att_sp_get_OvertimeByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_OVERTIMEBYIDS(:selectedIds,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_OvertimeByIds @selectedIds";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng Att_Overtime (store : hrm_att_sp_get_OT) </summary>
        public static string hrm_att_sp_get_OT
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_OT";
                }
                else
                {
                    return "exec hrm_att_sp_get_OT";
                }
            }
        }


        /// <summary> Lấy danh sách đối tượng Att_Leaveday (store : hrm_att_sp_get_LeavedayByIds) </summary>
        public static string hrm_att_sp_get_LeavedayByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_LEAVEDAYBYIDS(:selectedIds,:R_Output); end;";
                    // return "ATT_SP_GET_LEAVEDAYBYIDS";
                }
                else
                {
                    return "exec hrm_att_sp_get_LeavedayByIds @selectedIds";
                }
            }
        }


        /// <summary> Update status Att_Leaveday (store : hrm_att_sp_get_Leaveday_UpdateStatus) </summary>
        public static string hrm_att_sp_get_Leaveday_UpdateStatus
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_UPDATE_LEAVEDAY_STATUS";
                }
                else
                {
                    return "exec hrm_att_sp_get_Leaveday_UpdateStatus";
                }
            }
        }
        /// <summary> Update status Att_Leaveday (store : hrm_att_sp_get_Leaveday_UpdateStatus) </summary>
        public static string hrm_att_sp_get_LeavedayPortal_UpdateStatus
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_UPDATE_LDPORTAL_STATUS";
                }
                else
                {
                    return "exec hrm_att_sp_get_LeavedayPortal_UpdateStatus";
                }
            }
        }


        /// <summary> Lấy danh sách đối tượng Att_Pregnancy (store : hrm_att_sp_get_PregnancyByIds) </summary>
        public static string hrm_att_sp_get_PregnancyByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_PREGNANCYBYIDS(:selectedIds,:R_Output); end;";
                    //return "ATT_SP_GET_PREGNANCYBYIDS";
                }
                else
                {
                    return "exec hrm_att_sp_get_PregnancyByIds @selectedIds";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng quẹt thẻ để load dữ liệu từ server về (store : hrm_att_sp_delete_TAMScanLog) </summary>
        public static string hrm_att_sp_delete_TAMScanLog
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_DELETE_TAMSCANLOG";
                }
                else
                {
                    return "exec hrm_att_sp_delete_TAMScanLog @CardCode,@DateFrom,@DateTo";
                }
            }
        }

        // Xoá dữ liệu bảng Can_TAMScanlog by store
        public static string hrm_Can_sp_delete_TAMScanLog
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAN_SP_DELETE_TAMSCANLOG";
                }
                else
                {
                    return "exec hrm_can_sp_delete_TAMScanLog @Id, @DateFrom, @DateTo";
                }
            }
        }

        // Xoá dữ liệu bảng Can_RecordMissing by store
        public static string hrm_Can_sp_delete_RecordMissing
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAN_SP_DELETE_RECORDMISSING";
                }
                else
                {
                    return "exec hrm_Can_sp_delete_RecordMissing @Id";
                }
            }
        }

        public static string hrm_lau_sp_get_LineById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin LAU_SP_GET_LINEBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_lau_sp_get_LineById @Keyword";
                }
            }
        }
        public static string hrm_lau_sp_get_MarkerById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin LAU_SP_GET_MARKERBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_lau_sp_get_MarkerById @Keyword";
                }
            }
        }
        public static string hrm_lau_sp_get_LockerById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin LAU_SP_GET_LOCKERBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_lau_sp_get_LockerById @Keyword";
                }
            }
        }


        /// <summary>
        /// [Hieu.Van] 
        /// Store lấy danh sách Lau_LaundryRecord theo dateFrom dateTo
        /// </summary>
        public static string hrm_lau_sp_get_LaundryRecord_ByDateFromDateTo
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "LAU_SP_GET_LAURECORD_BYFROMTO";
                }
                else
                {
                    return "exec hrm_lau_sp_get_LaundryRecord_ByDateFromDateTo";
                }
            }
        }


        /// <summary>
        /// [Hieu.Van] 
        /// Store lấy danh sách Can_MealRecord theo dateFrom dateTo
        /// </summary>
        public static string hrm_can_sp_get_MealRecord_ByDateFromDateTo
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAN_SP_GET_MEALRECORD_BYFROMTO";
                }
                else
                {
                    return "exec hrm_can_sp_get_MealRecord_ByDateFromDateTo";
                }
            }
        }

        // Xoá dữ liệu bảng lau_TAMScanlog by store
        public static string hrm_Lau_sp_delete_TAMScanLog
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "LAU_SP_DELETE_TAMSCANLOG";
                }
                else
                {
                    return "exec hrm_lau_sp_delete_TAMScanLog @CardCode,@DateFrom,@DateTo";
                }
            }
        }



        /// <summary> Danh sách ngày nghỉ hàng năm (sau khi phân tích) </summary>
        public const string hrm_att_sp_get_AnnualLeaveDetailAnalysis = "exec hrm_att_sp_get_AnnualLeaveDetailAnalysis";

        /// <summary> Lấy danh sách đối tượng Att_AnnualLeaveDetail (store : hrm_att_sp_get_AnnualLeaveDetailByIds) </summary>
        public static string hrm_att_sp_get_AnnualLeaveDetailByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_ANNLEAVEDETAILBYIDS ATT_SP_GET_ANNUALDETAILBYIDS(:selectedIds,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_AnnualLeaveDetailByIds @selectedIds";
                }
            }
        }

        //[Hien.Nguyen]
        public static string hrm_att_sp_get_AnnLeaveDetailById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_ANNLEAVEDETAILBYID(:Annualleavedetailbyid,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_AnnLeaveDetailById @Annualleavedetailbyid";
                }
            }
        }

        //[Hien.Nguyen]
        public static string hrm_att_sp_get_AttendanceItemById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_ATTENDANCEITEMBYID(:AttendanceTableItembyid,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_AttendanceItemById @AttendanceTableItembyid";
                }
            }
        }

        //[Hien.Nguyen]
        public static string hrm_att_sp_get_CutOffDurationById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_CUTOFFDURATIONBYID(:Att_CutOffDurationbyid,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_CutOffDurationById @Att_CutOffDurationbyid";
                }
            }
        }


        /// <summary> Danh sách ngày nghỉ chi tiết phép ốm hàng năm (sau khi phân tích) </summary>
        public const string hrm_att_sp_get_AnnualSickLeaveDetailAnalysis = "exec hrm_att_sp_get_AnnualSickLeaveDetailAnalysis";

        /// <summary> Danh sách  chi tiết BHXH (sau khi phân tích) </summary>
        public static string hrm_att_sp_get_AnnualInsuranceLeaveDetailAnalysis
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_INSLEAVEDETAIL";
                }
                else
                {
                    return "exec hrm_att_sp_get_AnnualInsuranceLeaveDetailAnalysis";
                }
            }
        }



        #endregion

        #region HRMSystem
        /// <summary> Lấy danh sách tất cả dữ liệu Sys_dynamicColumn </summary>
        public static string hrm_sys_sp_get_DynamicColumn
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_DYNAMICCOLUMN";
                }
                else
                {
                    return "exec [hrm_sys_sp_get_DynamicColumn]";
                }
            }
        }


        /// <summary> Lấy danh sách GroupPermission (store : hrm_hr_sp_get_GroupPermission)  </summary>
        public static string hrm_sys_sp_get_GroupPermission
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_GROUPPERMISSION";
                }
                else
                {
                    return "exec hrm_sys_sp_get_GroupPermission";
                }
            }
        }


        /// <summary> Lấy đối tượng GroupPermission (store : hrm_hr_sp_get_GroupPermissionById)  </summary>
        //     public const string SYS_SP_GET_USERINFOBYID = "exec hrm_sys_sp_get_GroupPermissionById @grouppermissionId";//không sử dụng store này

        /// <summary> Lấy đối tượng GroupPermission theo Group (store : hrm_sys_sp_get_GroupPermissionByGroupId)  </summary>
        public static string hrm_sys_sp_get_GroupPermissionByGroupId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_GroupPermissionGID(:GroupId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_GroupPermissionByGroupId @groupId";
                }
            }
        }



        /// <summary> Lấy đối tượng GroupPermission theo GroupId và ResourceId (store : hrm_sys_sp_get_GroupPermissionByGroupAndResource)  </summary>
        public static string hrm_sys_sp_get_GroupPermissionByGroupAndResource
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_GroupPermissGrpRes";
                }
                else
                {
                    return "exec hrm_sys_sp_get_GroupPermissionByGroupAndResource";
                }
            }
        }



        /// <summary> Lấy danh sách DataPermission (store : hrm_system_sp_get_DataPermission)  </summary>
        public static string hrm_system_sp_get_DataPermission
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_DATAPERMISSION";
                }
                else
                {
                    return "exec hrm_system_sp_get_DataPermission";
                }
            }
        }


        /// <summary> Lấy danh sách DataPermission by id (store : hrm_system_sp_get_DataPermissioId)  </summary>
        public static string hrm_system_sp_get_DataPermissioId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_DATAPERMISSIONID(:DataPermissioId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_system_sp_get_DataPermissioId @DataPermissioId";
                }
            }
        }


        /// <summary> Lấy add column (store : hrm_sys_sp_altertable_AddColumn)  </summary>
        public const string hrm_sys_sp_altertable_AddColumn = "exec hrm_sys_sp_altertable_AddColumn @tablename,@columnname,@datatype";

        /// <summary> Lấy remove column (store : [hrm_sys_sp_altertable_DeleteColumn])  </summary>
        public const string hrm_sys_sp_altertable_DeleteColumn = "exec [hrm_sys_sp_altertable_DeleteColumn] @tablename,@columnname";

        /// <summary> Lấy danh sách users (store : hrm_hr_sp_get_User)  </summary>
        public const string hrm_hr_sp_get_User = "exec hrm_hr_sp_get_User";

        /// <summary> Lấy đối tượng user bởi Id  (store : hrm_hr_sp_get_UserById)  </summary>
        public const string hrm_hr_sp_get_UserById = "exec hrm_hr_sp_get_UserById @userId";

        /// <summary> Lấy danh sách GroupUsers (store : hrm_hr_sp_get_GroupUser)  </summary>
        public static string hrm_sys_sp_get_GroupUser
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_GROUPUSER";
                }
                else
                {
                    return "exec hrm_sys_sp_get_GroupUser";
                }
            }
        }


        /// <summary> Lấy đối tượng GroupUsers bởi Id  (store : hrm_hr_sp_get_GroupUserById)  </summary>
        public static string hrm_sys_sp_get_GroupUserById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_GROUPUSERID(:groupuserId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_GroupUserById @groupuserId";
                }
            }
        }

        public static string hrm_sys_sp_get_DelegateApprove
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_DELEGATEAPPROVE";
                }
                else
                {
                    return "exec hrm_sys_sp_get_DelegateApprove";
                }
            }
        }

        public static string hrm_sys_sp_get_DelegateApproveById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_DELEGATEAPPROVEBYID(:groupuserId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_DelegateApproveById @groupuserId";
                }
            }
        }

        /// <summary> Lấy đối tượng GroupUsers bởi Group Id  (store : hrm_sys_sp_get_GroupUserByGroupId)  </summary>
        public static string hrm_sys_sp_get_GroupUserByGroupId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_GROUPUSERGROUPID(:groupId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_GroupUserByGroupId @groupId";
                }
            }
        }


        /// <summary> Lấy đối tượng GroupUsers bởi Id  (store : hrm_hr_sp_get_GroupUserById)  </summary>
        public static string hrm_sys_sp_get_GroupUserByUserId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_GROUPUSERUID(:userId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_GroupUserByUserId @userId";
                }
            }
        }


        /// <summary> Lấy đối tượng DataPermission bởi Id  (store : hrm_sys_sp_get_DataPermissionByUserId)  </summary>
        public static string hrm_sys_sp_get_DataPermissionByUserId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_DATAPERMISSIONUID(:userId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_DataPermissionByUserId @userId";
                }
            }
        }
        /// <summary> Lấy đối tượng Sys_AllSetting bởi key  </summary>
        public static string hrm_sys_sp_get_AllSettingByKey
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_ALL_SETTING_BYKEY(:Key,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_AllSettingByKey @Key";
                }
            }
        }

        /// <summary> Lấy đối tượng submenu value2 trong AllSetting theo   </summary>
        public const string hrm_sys_sp_get_UserSetting_Submenu = "exec hrm_sys_sp_get_UserSetting_Submenu @Name,@Keyword,@UserID";

        /// <summary> Lấy danh sách dữ liệu ReleaseNote </summary>
        public const string hrm_sys_sp_get_ReleaseNote = "exec hrm_sys_sp_get_ReleaseNote";

        /// <summary> Lấy danh sách dữ liệu Bookmark </summary>
        public const string hrm_sys_sp_get_Bookmark = "exec hrm_sys_sp_get_Bookmark";

        /// <summary> Lấy danh sách dữ liệu Bookmark bởi UserID </summary>
        //public const string hrm_sys_sp_get_BookmarkByUserID = "exec hrm_sys_sp_get_BookmarkByUserID";

        public static string hrm_sys_sp_get_BookmarkByUserID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_BOOKMARKBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_BookmarkByUserID @userId";
                }
            }
        }

        public static string hrm_sys_sp_get_BookmarkByHotKey
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_BOOKMARKBYHOTKEY";
                }
                else
                {
                    return "exec hrm_sys_sp_get_BookmarkByHotKey @bookmarkId ,@hotKey";
                }
            }
        }



        /// <summary> Lấy danh sách users </summary>
        public static string hrm_sys_sp_get_users
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_USERS";
                }
                else
                {
                    return "exec hrm_sys_sp_get_users";
                }
            }
        }
        public static string hrm_sys_sp_get_code_object
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_CODE_OBJECT";
                }
                else
                {
                    return "exec hrm_sys_sp_get_code_object";
                }
            }
        }

        public static string hrm_sys_sp_get_code_config
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_CODE_CONFIG";
                }
                else
                {
                    return "exec hrm_sys_sp_get_code_config";
                }
            }
        }


        /// <summary> Lấy đối tượng DataPermission bởi Id  (store : hrm_sys_sp_get_DataPermissionByUserId)  </summary>
        public static string hrm_cat_sp_get_GetAllSettings
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_GETALLSETTINGS(:ValueSelected,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_GetAllSettings @ValueSelected";
                }
            }
        }

        /// <summary> Cập nhật bảng All Setting dựa vào Name  </summary>
        //public const string hrm_cat_sp_get_UpdateAllSettingByName = "exec hrm_cat_sp_get_UpdateAllSettingByName @strName, @value";
        public static string hrm_cat_sp_get_UpdateAllSettingByName
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_UPDATEALLSETTINGBYNAME";
                }
                else
                {
                    return "hrm_cat_sp_get_UpdateAllSettingByName";
                }
            }
        }
        /// <summary> Lấy danh sách groups </summary>
        public static string hrm_sys_sp_get_groups
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_GROUPS";
                }
                else
                {
                    return "exec hrm_sys_sp_get_groups";
                }
            }
        }

        public static string hrm_sys_sp_get_groupById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_GROUPBYID(:groupId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_groupById @groupId";
                }
            }
        }
        public static string Hrm_CAT_SP_GET_PURMCAMBYID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PURMCAMBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_CAT_SP_GET_PURMCAMBYID  @Id";
                }
            }
        }
        public static string Hrm_CAT_SP_GET_PURMCAM
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_PURMCAM";
                }
                else
                {
                    return "exec hrm_CAT_SP_GET_PURMCAM";
                }
            }
        }
        /// <summary> Lấy danh sách groups </summary>
        public static string hrm_cat_sp_get_tables
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_TABLES";
                }
                else
                {
                    return "exec hrm_cat_sp_get_tables";
                }
            }
        }

        public static string hrm_cat_sp_get_FieldNameByTableName
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_FNBYTABLENAME";
                }
                else
                {
                    return "exec hrm_cat_sp_get_FieldNameByTableName";
                }
            }
        }
        /// <summary> Lấy danh sách groups </summary>
        public static string hrm_cat_sp_get_tablesByTableName
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_TABLESBYTABLENAME";
                }
                else
                {
                    return "exec hrm_cat_sp_get_tablesByTableName";
                }
            }
        }

        /// <summary> Lấy danh sách groups </summary>
        public static string hrm_sys_sp_get_checkDuplicate
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_CHECKEXIST";
                }
                else
                {
                    return "hrm_sys_sp_get_CheckExist";
                }
            }

        }

        /// <summary> Delete boi store </summary>
        public static string hrm_sys_sp_delete_byid
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_DELETE_BYID";
                }
                else
                {
                    return "hrm_sys_sp_delete_byid";
                }
            }
        }

        /// <summary>[Hieu.Van] kiểm tra 2 điều kiện có tồn tại bản ghi nào hay chưa? VD: profile: 2354 với TimeLog: 2014-01-02 có bản ghi nào chưa</summary>
        public static string hrm_sys_sp_get_checkDuplicate_2Condition
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_CHECKEXIST_2CONDIT";
                }
                else
                {
                    return "chua viet";
                }
            }
        }

        public static string hrm_sys_sp_get_checkDuplicate_BookMark
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_CHECKBOOKMARK";
                }
                else
                {
                    return "chua viet";
                }
            }
        }
        public static string hrm_sys_sp_get_checkDuplicate_4Condition
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_CHECKEXIST_4CONDIT";
                }
                else
                {
                    return "chua viet";
                }
            }
        }

        public static string hrm_sys_sp_get_LockObject
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_LOCKOBJECT";
                }
                else
                {
                    return "exec hrm_sys_sp_get_LockObject";
                }
            }

        }

        public static string hrm_sys_sp_get_LockObjectItemByLockObjectID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_LOCKOBJITEMBYLOID";
                }
                else
                {
                    return "exec hrm_sys_sp_get_LockObjectItemByLockObjectID";
                }
            }

        }

        public static string hrm_sys_sp_get_AllSetting
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_ALLSETTING";
                }
                else
                {
                    return "exec hrm_sys_sp_get_AllSetting";
                }
            }

        }
        public static string hrm_sys_sp_get_AsynTask
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_ASYNTASK";
                }
                else
                {
                    return "exec hrm_sys_sp_get_AsynTask";
                }
            }

        }

        public static string hrm_sys_sp_get_AsynTask_ById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_ASYNTASKBYID(:ain_Sys_AsynTaskId,:aoc_output); end;";

                }
                else
                {
                    return "exec hrm_sys_sp_get_AsynTask_ById @Id";
                }
            }
        }

        public static string hrm_sys_sp_get_UserbyId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_USERBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_UserbyId @Keyword";
                }
            }
        }

        /// <summary> Xoá GroupPermission2 </summary>
        public static string hrm_sys_sp_delete_GroupPermission2
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_DELETE_GROUPPERMISSION2";
                }
                else
                {
                    return "exec hrm_sys_sp_delete_GroupPermission2";
                }
            }
        }

        /// <summary> Lấy Dashboard  </summary>
        public static string hrm_sys_sp_get_DashBoardById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_DASHBOARDBYID(:GroupId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_DashBoardById @groupId";
                }
            }
        }
        public static string hrm_sys_sp_get_DashboardApproveById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_DASHBODAPPROVEBYID(:UserId,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_DashboardApproveById @UserId";
                }
            }
        }
        #endregion

        #region Laundry

        /// <summary> Lấy danh sách dữ liệu Provider </summary>
        // public const string hrm_lau_sp_get_Provider = "exec hrm_lau_sp_get_Provider";
        public static string hrm_lau_sp_get_Provider
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "LAU_SP_GET_PROVIDER";
                }
                else
                {
                    return "exec hrm_lau_sp_get_Provider";
                }
            }
        }

        /// <summary> Lấy danh sách dữ liệu Locker </summary>
        //public const string hrm_lau_sp_get_Locker = "exec hrm_lau_sp_get_Locker";
        public static string hrm_lau_sp_get_Locker
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "LAU_SP_GET_LOCKER";
                }
                else
                {
                    return "exec hrm_lau_sp_get_Locker";
                }
            }
        }

        public static string hrm_lau_sp_get_Locker_Mutil
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin LAU_SP_GET_LOCKER_MUTIL(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_lau_sp_get_Locker_Mutil @Keyword";
                }
            }
        }

        /// <summary> Lấy danh sách dữ liệu Reader </summary>
        // public const string hrm_lau_sp_get_Reader = "exec hrm_lau_sp_get_Reader";
        public static string hrm_lau_sp_get_Reader
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "LAU_SP_GET_READER";
                }
                else
                {
                    return "exec hrm_lau_sp_get_Reader";
                }
            }
        }

        // public const string hrm_lau_sp_get_Reader = "exec hrm_lau_sp_get_Reader";
        public static string hrm_lau_sp_get_MachineOfLine
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "LAU_SP_GET_MACHINEOFLINE";
                }
                else
                {
                    return "exec hrm_lau_sp_get_MachineOfLine";
                }
            }
        }

        public static string hrm_lau_sp_get_MachineOfLine_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin LAU_SP_GET_MACHINEOFLINE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_lau_sp_get_MachineOfLine_multi @Keyword";
                }
            }
        }

        public static string hrm_lau_sp_get_TamScanLog
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "LAU_SP_GET_TAMSCANLOG";
                }
                else
                {
                    return "exec hrm_lau_sp_get_TamScanLog";
                }
            }
        }
        /// <summary>
        /// [Tam.Le] - 8.4.2014 - Lấy dữ liệu Marker theo Line Id
        /// </summary>       
        public static string hrm_lau_sp_get_MarkerByLineId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin LAU_SP_GET_MARKERBYLINEID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_lau_sp_get_MarkerByLineId @Id";
                }
            }
        }
        /// <summary>
        /// [Tam.Le] - 8.4.2014 - Lấy dữ liệu Locker theo Line Id
        /// </summary>       
        public static string hrm_lau_sp_get_LockerByLineId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin LAU_SP_GET_LOCKERBYLINEID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_lau_sp_get_LockerByLineId @Id";
                }
            }
        }
        public static string hrm_lau_sp_get_LaundryRecord
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "LAU_SP_GET_LAUNDRYRECORD";
                }
                else
                {
                    return "exec hrm_lau_sp_get_LaundryRecord";
                }
            }
        }
        public static string hrm_lau_sp_get_LaundryRecord_byId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin LAU_SP_GET_LAUNDRYRECORD_BYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_lau_sp_get_LaundryRecord_byId @Id";
                }
            }
        }

        public static string hrm_att_sp_get_LaundryRecord_UpdateStatus
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "LAU_SP_GET_LAURD_UPDATESTATUS";
                }
                else
                {
                    return "exec hrm_att_sp_get_LaundryRecord_UpdateStatus";
                }
            }
        }
        public static string hrm_lau_sp_get_LaundryRecord_ByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin LAU_SP_GET_LAUNDRYRECORD_BYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_lau_sp_get_LaundryRecord_ByIds @Ids";
                }
            }
        }

        public static string hrm_lau_sp_get_Marker
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "LAU_SP_GET_MARKER";
                }
                else
                {
                    return "exec hrm_lau_sp_get_Marker";
                }
            }
        }

        public static string hrm_lau_sp_get_Marker_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin LAU_SP_GET_MARKER_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_lau_sp_get_Marker_multi @Keyword";
                }
            }
        }
        public static string hrm_lau_sp_get_Line
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "LAU_SP_GET_LINE";
                }
                else
                {
                    return "exec hrm_lau_sp_get_Line";
                }
            }
        }
        public static string hrm_lau_sp_get_Line_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin LAU_SP_GET_LINE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_lau_sp_get_Line_multi @Keyword";
                }
            }
        }


        public static string hrm_lau_sp_get_SummaryExceptClothing
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "LAU_SP_GET_SUMEXCEPTCLOTHING";
                }
                else
                {
                    return "exec hrm_lau_sp_get_SummaryExceptClothing";
                }
            }
        }
        #endregion

        #region StoreMulti

        //public const string hrm_cat_sp_get_JobTitle_Multi = "exec hrm_cat_sp_get_JobTitle_Multi @Keyword";
        public static string hrm_can_sp_get_MealAllowanceTypeSetting_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_MALLOWTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_sp_get_MealAllowanceTypeSetting_Multi @Keyword";
                }
            }
        }
        //public const string hrm_cat_sp_get_ProductType_Multi = "exec hrm_cat_sp_get_ProductType_Multi @Keyword";
        public static string hrm_cat_sp_get_ProductType_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PRODUCTTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ProductType_Multi @Keyword";
                }
            }
        }

        //public const string hrm_cat_sp_get_JobTitle_Multi = "exec hrm_cat_sp_get_JobTitle_Multi @Keyword";
        public static string hrm_cat_sp_get_JobTitle_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_JOBTITLE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_JobTitle_Multi @Keyword";
                }
            }
        }
        // public const string hrm_cat_sp_get_EducationLevel_Multi = "exec hrm_cat_sp_get_EducationLevel_Multi @Keyword";
        public static string hrm_cat_sp_get_EducationLevel_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_EDUCATIONL_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_EducationLevel_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_Discipline_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_DISCIPLINE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Discipline_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_Dise_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_CAT_SP_GET_DISE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Dise_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_Rank_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_CAT_SP_GET_RANK_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Rank_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_TypeOfTransfer_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_TYPETRANSFER_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TypeOfTransfer_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_NameEntityType_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_NAMEENTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_NameEntityType_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_JobVancancyReson_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_JOBVANCANCYRESON_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_JobVancancyReson_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_CategoryKPI_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_CATKPI_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CategoryKPI_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_TimeAnalyze
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_TIMEANALYZE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TimeAnalyze_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_Email_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_EMAIL_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Email_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_ReasonDeny_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_REASONDENY_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ReasonDeny_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_BlackListReason_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_BLACKLREASON_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_BlackListReason_Multi @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_TimeEvalutionData_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_TIMEEVALDATA_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TimeEvalutionData_Multi @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_TypeOfStop_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_TYPEOFSTOP_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TypeOfStop_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_AreaPostJob_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_AREAPOSTJOB_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AreaPostJob_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_Pivot_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PIVOT_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Pivot_Multi @Keyword";
                }
            }
        }



        // public const string hrm_cat_sp_get_Position_Multi = "exec hrm_cat_sp_get_Position_Multi @Keyword";
        public static string hrm_cat_sp_get_Position_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_POSITION_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Position_Multi @Keyword";
                }
            }
        }
        //public const string hrm_cat_sp_get_EmployeeType_Multi = "exec hrm_cat_sp_get_EmployeeType_Multi @Keyword";
        public static string hrm_cat_sp_get_EmployeeType_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_EMPLOYEETYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_EmployeeType_Multi @Keyword";
                }
            }
        }
        public static string hrm_hr_sp_get_Relatives_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_RELATIVES_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_Relative_Multi @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_AccidentType_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ACCIDENTTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AccidentType_Multi @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_AccidentType_MultiNew
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ADENTTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AccidentType_MultiNew @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_ComputingType_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_COMPUTINGTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ComputingType_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_ComputingLevel_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_COMPUTINGTLV_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ComputingLevel_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_LanguageType_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_LANGUAGETYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_LanguageType_Multi @Keyword";
                }
            }
        }


        public static string hrm_cat_sp_get_LanguageLevel_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_LANGUAGELEVEL_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_LanguageLevel_Multi @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_LanguageSkill_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_LANGUAGESKILL_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_LanguageSkill_Multi @Keyword";
                }
            }
        }
        //  public const string hrm_cat_sp_get_CostCentre_Multi = "exec hrm_cat_sp_get_CostCentre_Multi @Keyword";
        public static string hrm_cat_sp_get_CostCentre_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_COSTCENTRE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CostCentre_Multi @Keyword";
                }
            }
        }
        ////public const string hrm_hr_sp_get_Profile_Multi = "exec hrm_hr_sp_get_Profile_Multi @Keyword";
        //public static string hrm_hr_sp_get_Profile_Multi
        //{
        //    get
        //    {
        //        if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //        {
        //            return "begin HR_SP_GET_PROFILE_MULTI(:Keyword,:R_Output); end;";
        //        }
        //        else
        //        {
        //            return "exec hrm_hr_sp_get_Profile_Multi @Keyword";
        //        }
        //    }
        //}
        //public const string hrm_hr_sp_get_ProfileFemale_Multi = "exec hrm_hr_sp_get_ProfileFemale_Multi @Keyword";
        public static string hrm_hr_sp_get_ProfileFemale_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PROFILEFEMALE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileFemale_Multi @Keyword";
                }
            }
        }
        //  public const string hrm_cat_sp_get_ContractType_multi = "exec hrm_cat_sp_get_ContractType_multi @Keyword";
        public static string hrm_cat_sp_get_ContractType_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_CONTRACTTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ContractType_multi @Keyword";
                }
            }
        }




        //public const string hrm_cat_sp_get_Country_multi = "exec hrm_cat_sp_get_Country_multi @Keyword";
        public static string hrm_cat_sp_get_Country_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_COUNTRY_MUTIL(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Country_multi @Keyword";
                }
            }
        }


        //public const string hrm_cat_sp_get_EthnicGroup_Multi = "exec hrm_cat_sp_get_EthnicGroup_Multi @Keyword";
        public static string hrm_cat_sp_get_EthnicGroup_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ETHNICGROUP_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_EthnicGroup_Multi @Keyword";
                }
            }
        }

        //public const string hrm_cat_sp_get_EthnicGroup_Multi = "exec hrm_cat_sp_get_EthnicGroup_Multi @Keyword";
        public static string hrm_cat_sp_get_religion_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_RELIGION_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_religion_Multi @Keyword";
                }
            }
        }


        //public const string hrm_cat_sp_get_CountrybyId = "exec hrm_cat_sp_get_CountrybyId @Keyword";
        public static string hrm_cat_sp_get_CountrybyId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_COUNTRYBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CountrybyId @Keyword";
                }
            }
        }

        //public const string hrm_cat_sp_get_Region_multi = "exec hrm_cat_sp_get_Region_multi @Keyword";
        public static string hrm_cat_sp_get_Region_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_REGION_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Region_multi @Keyword";
                }
            }
        }
        // public const string hrm_cat_sp_get_Province_multi = "exec hrm_cat_sp_get_Province_multi @Keyword";
        public static string hrm_cat_sp_get_Province_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PROVINCE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Province_multi @Keyword";
                }
            }
        }

        //Tho.Bui: GetMultiCommunistPartyPosition
        public static string hrm_cat_sp_get_CommunistPartyPosition_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_COMMUNI_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_communni_multi @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_District_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_DISTRICT_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_District_multi @Keyword";
                }
            }
        }
        public static string hrm_sys_sp_get_user_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_USER_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_user_multi @Keyword";
                }
            }
        }


        public static string hrm_sys_sp_get_group_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_GROUP_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_group_multi @Keyword";
                }
            }
        }

        public static string hrm_sys_sp_get_ForeignKey_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_FOREIGNKEY_MULTI";
                }
                else
                {
                    return "hrm_sys_sp_get_ForeignKey_multi";
                }
            }
        }

        public static string hrm_sys_sp_get_userApproved_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_USERAPPROVED_MULTI";
                }
                else
                {
                    return "hrm_sys_sp_get_userApproved_multi";
                }
            }
        }


        //public const string hrm_cat_sp_get_LeaveDayType_multi = "exec hrm_cat_sp_get_LeaveDayType_multi @Keyword";
        public static string hrm_cat_sp_get_LeaveDayType_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_LEAVEDAYTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_LeaveDayType_multi @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_ResignReason_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_RESIGNREASON_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ResignReason_multi @Keyword";
                }
            }
        }


        //public const string hrm_cat_sp_get_OvertimeType_multi = "exec hrm_cat_sp_get_OvertimeType_multi @Keyword";
        public static string hrm_cat_sp_get_OvertimeType_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_OVERTIMETYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OvertimeType_multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_OrgStructure_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ORGSTRUCTURE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OrgStructure_multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_OrgStructure_Cascading
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ORGSTRUCTURE_CASCADING";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OrgStructure_Cascading";
                }
            }
        }

        public static string hrm_cat_sp_get_ShopGroup_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SHOPGROUP_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ShopGroup_multi @Keyword";
                }
            }
        }

        /// <summary>
        /// [Chuc.Nguyen] - Lấy tất cả dữ liệu cho phòng ban
        /// </summary>
        public static string hrm_cat_sp_get_OrgStructure_Data
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ORGSTRUCTURE_DATA";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OrgStructure_Data";
                }
            }
        }

        /// <summary>
        /// Store lấy quyền cho cây phòng ban
        /// </summary>
        public static string hrm_cat_sp_get_PermissionByOrg
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_PERMISSIONBYORG";
                }
                else
                {
                    return "Chưa viết";
                }
            }
        }

        public static string hrm_cat_sp_get_OrgStructureByOrderNumber
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ORGBYORDERNUMBER";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OrgStructureByOrderNumber";
                }
            }
        }

        /// <summary>
        /// [Hien.Nguyen] - Lấy tất cả dữ liệu cho phòng ban có tổng hợp nhân viên
        /// </summary>
        public static string hrm_cat_sp_get_OrgStructure_Data_SumProfile
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ORGSTRUCTURE_SUMPRO";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OrgStructure_SumProFile";
                }
            }
        }


        public static string hrm_cat_sp_get_OrgStructureById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ORGSTRUCTUREBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OrgStructureById @Id";
                }
            }
        }
        // public const string hrm_cat_sp_get_OrgStructureType_multi = "exec hrm_cat_sp_get_OrgStructureType_multi @Keyword";
        public static string hrm_cat_sp_get_OrgStructureType_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ORGSTRUCTTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OrgStructureType_multi @Keyword";
                }
            }
        }

        // public const string hrm_cat_sp_get_RelativeType_multi = "exec hrm_cat_sp_get_RelativeType_multi @Keyword";
        public static string hrm_cat_sp_get_RelativeType_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_RELATIVETYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_RelativeType_multi @Keyword";
                }
            }
        }
        // public const string hrm_cat_sp_get_Shift_multi = "exec hrm_cat_sp_get_Shift_multi @Keyword";
        public static string hrm_cat_sp_get_Shift_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SHIFT_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Shift_multi @Keyword";
                }
            }
        }
        //public const string hrm_cat_sp_get_Grade_multi = "exec hrm_cat_sp_get_Grade_multi @Keyword";
        public static string hrm_cat_sp_get_Grade_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_GRADE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Grade_multi @Keyword";
                }
            }
        }
        //public const string hrm_cat_sp_get_Export_multi = "exec hrm_cat_sp_get_Export_multi @Keyword";
        public static string hrm_cat_sp_get_Export_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_EXPORT_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Export_multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_ExportWord_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_EXPORTWORD_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ExportWord_multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_reportMapping_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_RPTMAPPING_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_reportMapping_multi @Keyword";
                }
            }
        }


        //public const string hrm_hre_sp_get_Dependant_multi = "exec hrm_hre_sp_get_Dependant_multi @Keyword";
        public static string hrm_hre_sp_get_Dependant_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_DEPENDANT_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hre_sp_get_Dependant_multi @Keyword";
                }
            }
        }
        public static string hrm_hr_sp_get_Profile_GeneralGrid
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILEGENERALGRID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_Profile_GeneralGrid";
                }
            }
        }
        public static string hrm_att_sp_get_CutOffDuration_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_CUTOFF_MULTI(:Keyword,:R_Output); end;";
                    //  return "ATT_SP_GET_CUTOFF_MULTI";
                }
                else
                {
                    return "exec hrm_att_sp_get_CutOffDuration_multi @Keyword";
                }
            }
        }

        public static string hrm_can_sp_get_Location_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_LOCATION_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_sp_get_Location_multi @Keyword";
                }
            }
        }

        public static string hrm_can_sp_get_TamScanLog_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_TAMSCANLOG_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_sp_get_TamScanLog_multi @Keyword";
                }
            }
        }

        public static string hrm_can_sp_get_Catering_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_CATERING_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_sp_get_Catering_multi @Keyword";
                }
            }
        }

        public static string hrm_can_sp_get_Canteen_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_CANTEEN_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_sp_get_Canteen_multi @Keyword";
                }
            }
        }

        public static string hrm_can_sp_get_Reader_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_READER_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_sp_get_Reader_multi @Keyword";
                }
            }
        }

        public static string hrm_lau_sp_get_Locker_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin LAU_SP_GET_LOCKER_MUTIL(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_lau_sp_get_Locker_multi @Keyword";
                }
            }
        }

        public static string hrm_can_sp_get_Line_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_LINE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_sp_get_Line_multi @Keyword";
                }
            }
        }

        public static string hrm_can_sp_get_LineById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_LINEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_sp_get_LineById @Id";
                }
            }
        }
        /// <summary>
        /// [Tam.Le] lấy id và name
        /// </summary>
        public static string hrm_can_sp_get_MealAllowanceTypeSetting_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_MEAL_ALL_TYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_sp_get_MealAllowanceTypeSetting_multi @Keyword";
                }
            }
        }
        /// <summary>
        /// [Hieu.Van] lấy id và code Machine Of Line
        /// </summary>
        public static string hrm_can_sp_get_MachineOfLine_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_SP_GET_MACHINEOFLINE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_sp_get_MachineOfLine_multi @Keyword";
                }
            }
        }
        /// <summary> Lấy danh sách đối tượng sys.tables</summary>
        // public const string hrm_cat_sp_get_Import_multi = "exec hrm_cat_sp_get_Import_multi @Keyword";
        public static string hrm_cat_sp_get_Import_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_IMPORT_MULTI(:Keyword,:R_Output); end;";
                    //  return "ATT_SP_GET_CUTOFF_MULTI";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Import_multi @Keyword";
                }
            }
        }

        //Sử dụng cho màn hình Import khi chọn ObjectName thì tự động lọc ImportName
        public static string hrm_cat_sp_get_Import_By_Obj
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_IMPORT_BY_OBJ(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Import_By_Obj @Keyword";
                }
            }
        }
        /// <summary> Lấy danh sách đối tượng sys.tables</summary>
        public const string hrm_cat_sp_get_SysTables_multi = "exec hrm_cat_sp_get_SysTables_multi @Keyword";

        /// <summary>  Lấy danh sách dữ liệu RelativeType       </summary>
        //public const string hrm_cat_sp_get_EthnicGroup = "exec hrm_cat_sp_get_EthnicGroup";
        public static string hrm_cat_sp_get_EthnicGroup
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ETHRNICGROUP";
                }
                else
                {
                    return "exec hrm_cat_sp_get_EthnicGroup";
                }
            }
        }


        /// <summary> Lấy danh sách đối tượng RelativeType By ID</summary>
        //   public const string hrm_cat_sp_get_EthnicGroupById = "exec hrm_cat_sp_get_EthnicGroupById @EthnicGroupID";
        public static string hrm_cat_sp_get_EthnicGroupById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ETHRNICGROUPBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_EthnicGroupById @Id";
                }
            }
        }


        /// <summary> Lấy danh sách đối tượng RelativeType đế export selected(store : hrm_cat_sp_get_RelativeTypeIds) </summary>
        public const string hrm_cat_sp_get_EthnicGroupByIds = "exec hrm_cat_sp_get_EthnicGroupByIds @selectedIds";

        /// <summary> Lấy danh sách childfield của một đối tượng</summary>
        public static string hrm_cat_sp_get_Import_childfield_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_IMPORT_FIELD_MULTI";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Import_childfield_multi";
                }
            }
        }



        #endregion

        #region Other

        /// <summary>Lấy ngày hiện tại </summary>
        public const string hrm_sp_get_datetime = "exec hrm_sp_get_datetime";

        #endregion

        #region [Payroll]

        /// <summary>
        /// Lấy dữ liệu bảng Sal_SalaryInformation
        /// </summary>
        /// 
        /// <summary> [Quoc.Do]Lấy đối tượng Sal_SalaryInformation By ID</summary>
        //public const string  hrm_sal_sp_get_Sal_SalaryInformationById = "exec  hrm_sal_sp_get_Sal_SalaryInformationById @EmployeeTypeID";
        public static string hrm_sal_sp_get_Sal_SalaryInformationById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SP_GET_SALARYINFORMATIONBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_Sal_SalaryInformationById @Keyword";
                }
            }
        }

        /// <summary>
        /// Lấy đối tượng profille bời ProfileID trong bảng Sal_SalaryInfomation
        /// </summary>      
        public static string hrm_sal_sp_get_Sal_SalaryInfomationByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_SAINFOMATIONBYPROID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_Sal_SalaryInfomationByProfileId @Id";
                }
            }
        }
        /// <summary> [Quoc.Do]Lấy danh sách đối tượng Sal_SalaryInformation By ID</summary>
        //public const string  hrm_sal_sp_get_Sal_SalaryInformation = "exec  hrm_sal_sp_get_Sal_SalaryInformation";

        public static string hrm_sal_sp_get_Sal_SalaryInformation
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_SALARYINFORMATION";
                }
                else
                {
                    return "exec hrm_sal_sp_get_Sal_SalaryInformation";
                }
            }
        }



        public static string hrm_sal_sp_get_Sal_Grade
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_GRADE";
                }
                else
                {
                    return "exec hrm_sal_sp_get_Grade";
                }
            }
        }

        public static string hrm_sal_sp_get_Sal_Producttive
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_PRODUCTTIVE";
                }
                else
                {
                    return "exec hrm_sal_sp_get_Sal_Producttive";
                }
            }
        }


        public static string hrm_sal_sp_get_Sal_ProductSalary
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_PRODUCTSALARY";
                }
                else
                {
                    return "exec hrm_sal_sp_get_Sal_ProductSalary";
                }
            }
        }


        public static string hrm_sal_sp_get_ProductSalaryByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_PRODUCTSALARYBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_sal_sp_get_ProductSalaryByIds @ID";
                }
            }
        }

        public static string hrm_sal_sp_get_Sal_ProducttiveByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "Chưa viết";
                }
                else
                {
                    return "Chưa viết";
                }
            }
        }

        public static string hrm_sal_sp_get_Sal_ProductiveByID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_PRODUCTTIVEBYID (:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_Sal_ProducttiveByID @Id";
                }
            }
        }

        public static string hrm_sal_sp_get_HoldSalary
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_HOLDSALARY";
                }
                else
                {
                    return "exec hrm_sal_sp_get_HOLDSALARY";
                }
            }
        }

        public static string hrm_sal_sp_get_HoldSalaryByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_HOLDSALARYBYIDS(:Id,:R_Output);end; ";
                }
                else
                {
                    return "exec hrm_sal_sp_get_HoldSalaryByIds @ID";
                }
            }
        }

        /// Lấy danh sách chọn dữ liệu Sal_SalaryInformation
        /// </summary>
        //public const string hrm_sal_sp_get_SalaryInformationIds = "exec hrm_sal_sp_get_SalaryInformationIds";
        public static string hrm_sal_sp_get_SalaryInformationIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_SALARYINFOMATIONIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_SalaryInformationIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_getSal_GradeByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_GRADEBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_GradeByIDs @Ids";
                }
            }
        }

        /// <summary>
        /// Lấy dữ liệu bảng BasicPayroll
        /// </summary>
        public static string hrm_sal_sp_get_BasicPayroll
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_BASICSALARY";
                }
                else
                {
                    return "exec hrm_sal_sp_get_BasicSalary";
                }
            }

        }

        public static string hrm_sal_sp_get_BasicSalaryIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_BASICSALARYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_BasicSalaryIds @Ids";
                }
            }

        }

        public static string hrm_sal_sp_get_InsuranceSalary
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_INSURANCESALARY";
                }
                else
                {
                    return "exec hrm_sal_sp_get_InsuranceSalary";
                }
            }

        }

        public static string hrm_sal_sp_get_InsuranceSalaryById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_INSURANCESALARYBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_InsuranceSalaryByID @Id";
                }
            }
        }

        public static string hrm_sal_sp_get_InsuranceSalaryIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_SAL_SP_GET_INSSALARYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_InsuranceSalaryIds @Ids";
                }
            }

        }

        /// <summary>
        /// Lấy dữ liệu bảng BasicPayroll theo ID
        /// </summary>
        public static string hrm_sal_sp_get_BasicPayrollById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_BASICSALARYBYIDPRO";
                }
                else
                {
                    return "exec hrm_sal_sp_get_BasicSalaryByProfile";
                }
            }
        }

        // Load PC thường xuyên
        public static string hrm_sal_sp_get_UnusualEDByProId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_UNUSUALBYPROID";
                }
                else
                {
                    return "exec hrm_sal_sp_get_UnusualEDByProId";
                }
            }
        }

        // Load PC bất thường
        public static string hrm_sal_sp_get_UnuEDByProId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_UNUEDBYPROID";
                }
                else
                {
                    return "exec hrm_sal_sp_get_UnuEDByProId";
                }
            }
        }

        public static string hrm_sal_sp_get_GradeAndAllownaceByProId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_GRADEALLOWBYPROID";
                }
                else
                {
                    return "exec hrm_sal_sp_get_GradeAllowByProfileID";
                }
            }
        }
        /// <summary>
        /// Lấy dữ liệu bảng PayrollTable theo ID profile và Cutoff
        /// </summary>
        public static string hrm_sal_sp_get_PayrollTable
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_PAYROLLTABLE";
                }
                else
                {
                    return "exec hrm_sal_sp_get_PayrollTable";
                }
            }
        }

        /// <summary>
        /// Lấy dữ liệu bảng PayrollTable_BK theo ID profile và Cutoff
        /// </summary>
        public static string hrm_sal_sp_get_PayrollTableBK
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_PAYROLLTABLEBK";
                }
                else
                {
                    return "exec hrm_sal_sp_get_PayrollTableBK";
                }
            }
        }

        /// <summary>
        /// Lấy dữ liệu bảng Payrolltableitem theo ID profile và Cutoff
        /// </summary>
        public static string hrm_sal_sp_get_PayrollTableItemByProfile
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_TABLEITEMBYPROFILE";
                }
                else
                {
                    return "exec hrm_sal_sp_get_PayrollTableItemByProfile";
                }
            }
        }

        public static string hrm_sal_sp_get_PayCommissionItem
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_PAYCOMMISSIONITEM";
                }
                else
                {
                    return "exec hrm_sal_sp_get_PayCommissionItem";
                }
            }
        }

        public static string hrm_sal_sp_get_UnusualPayItemByProfile
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_UNPAYITEMBYPROFILE";
                }
                else
                {
                    return "exec hrm_sal_sp_get_UnusualPayItemByProfile";
                }
            }
        }
        public static string hrm_sal_sp_get_UnusualPay
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_UNUSUALPAY";
                }
                else
                {
                    return "exec hrm_sal_sp_get_UnusualPay";
                }
            }
        }





        /// <summary>
        /// Lấy dữ liệu bảng Payrolltableitem theo ID profile và Cutoff
        /// </summary>
        public static string hrm_sal_sp_get_ExportPayrollTableItemByProfile
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_EXTBLITEMBYPROFILE";
                }
                else
                {
                    return "exec hrm_sal_sp_get_ExportPayrollTableItemByProfile";
                }
            }
        }

        public static string hrm_sal_sp_get_PayrollTableItem
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_PAYROLLTABLEITEM";
                }
                else
                {
                    return "exec hrm_sal_sp_get_PayrollTableItem";
                }
            }
        }

        public static string hrm_sal_sp_get_KaiReportPaymentout
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_KAIREPORTPAYMENTOUT";
                }
                else
                {
                    return "exec hrm_sal_sp_get_KaiReportPaymentout";
                }
            }
        }

        public static string hrm_sal_sp_get_PayrollTableItemBK
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_PAYROLLTABLEITEMBK";
                }
                else
                {
                    return "exec hrm_sal_sp_get_PayrollTableItemBK";
                }
            }
        }

        public static string hrm_sal_sp_get_BasicPayrollGetAll
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_BASICSALARYGETALL";
                }
                else
                {
                    return "exec hrm_sal_sp_get_BasicSalaryGetAll";
                }
            }
        }
        /// <summary>
        /// Get Salaryjobgroup multi
        /// </summary>
        public static string hrm_cat_sp_get_SalaryJobGroup_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_SALARYJOBGROUP_MUL";
                }
                else
                {
                    return "exec hrm_cat_sp_get_salaryjobgroup_mul";
                }
            }
        }

        /// <summary> Nhóm Lương </summary>
        public static string hrm_cat_sp_get_payrollGroup
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_PAYROLLGROUP";
                }
                else
                {
                    return "exec hrm_cat_sp_get_payrollGroup";
                }
            }

        }
        /// <summary> Nhóm Tài khoản </summary>
        public static string hrm_cat_sp_get_AccountType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ACCOUNTTYPE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AccountType";
                }
            }

        }
        public static string hrm_cat_sp_get_AccountTypeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ACCOUNTTYPEBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AccountTypeById @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_AccountTypeByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ACCOUNTTYPEBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AccountTypeByIds @Ids";
                }
            }
        }
        public static string hrm_cat_sp_get_AccountType_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ACCOUNTTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AccountType_multi @Keyword";
                }
            }
        }

        /// <summary> Loại Tai Nai Lap Động</summary>
        public static string hrm_cat_sp_get_AccidentType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ACCIDENTTYPE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AccidentType";
                }
            }

        }
        public static string hrm_cat_sp_get_AccidentTypeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ACCIDENTTYPEBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AccidentTypeById @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_AccidentTypeByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return " begin CAT_SP_GET_ACCIDENTTYPEBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AccidentTypeByIds @Ids";
                }
            }
        }

        public static string hrm_sal_sp_get_UnusualEDByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return " begin SAL_SP_GET_UNUSUALEDBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_UnusualEDByIds @Ids";
                }
            }
        }

        public static string hrm_sal_sp_get_UnusualEDById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_UNUSUALEDBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_UnusualEDById @Keyword";
                }
            }
        }
        public static string hrm_sal_sp_get_UnusualED
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_UNUSUALED";
                }
                else
                {
                    return "exec hrm_sal_sp_get_UnusualED";
                }
            }
        }

        public static string hrm_sal_sp_get_UnusualEDChildCare
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_UNUSUALEDBYCHILD";
                }
                else
                {
                    return "exec hrm_sal_sp_get_UnusualEDByChild";
                }
            }
        }
        public static string hrm_sal_sp_get_AllowanceEvaluationYear
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_UNUSUALEDBYEVA";
                }
                else
                {
                    return "exec hrm_sal_sp_get_AllowanceEvaluationYear";
                }
            }
        }

        public static string hrm_sal_sp_get_UnusualEDChildCareById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_UNUSUALEDCHILDBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_UnusualEDChildById @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_UnusualEDChildCareByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return " begin SAL_SP_GET_UNUSUALEDCHILDBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_UnusualEDChildCareByIds @Ids";
                }
            }
        }

        public static string hrm_sal_sp_Set_UnusualEDChildApproved_Status
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_SET_APPROVEUSUSTATUS";
                }
                else
                {
                    return "exec hrm_sal_sp_Set_UnusualEDChildApproved_Status";
                }
            }
        }
        public static string hrm_sal_sp_set_PayrollTable_IsPaid
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_SET_PAYROLLTABLE_ISPAID";
                }
                else
                {
                    return "exec hrm_sal_sp_set_PayrollTable_IsPaid";
                }
            }
        }

        #endregion

        #region Tho.Bui CostCenTre
        public static string hrm_Sal_sp_get_Sal_CostCentreSal
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_COSTCENTRE";
                }
                else
                {
                    return "exec hrm_Sal_sp_get_Sal_CostCentreSal";
                }
            }
        }

        //public static string hrm_sal_sp_get_CostCentreById
        //{
        //    get
        //    {
        //        if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
        //        {
        //            return "begin SAL_SP_GET_COSTCENTREBYID(:Keyword,:R_Output); end;";
        //        }
        //        else
        //        {
        //            return "exec hrm_sal_sp_get_CostCentreById @Keyword";
        //        }
        //    }
        //}

        #endregion

        #region Function
        /// <summary>
        /// Tên các Function
        /// vd: hrm_hr_fn_get_Profile
        /// </summary>

        #endregion
        #endregion

        #region Cat_Budget
        /// <summary>
        /// Lấy danh sách dữ liệu CatProductType
        /// </summary>
        public static string hrm_cat_sp_get_Budget
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_BUDGET";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Budget";
                }
            }
        }

        public static string hrm_cat_sp_get_BudgetById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_BUDGETBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_BudgetById @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_BudgetByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_BUDGETBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_BudgetByIds @Ids";
                }
            }

        }





        public static string hrm_cat_sp_get_Budget_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_BUDGET_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Budget_multi @Keyword";
                }
            }
        }
        #endregion

        #region Cat_LineItem

        public static string hrm_cat_sp_get_LineItem
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_LINEITEM";
                }
                else
                {
                    return "exec hrm_cat_sp_get_LineItem";
                }
            }
        }
        public static string hrm_cat_sp_get_LineItemById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_LINEITEMBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_LineItemById @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_LineItemByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_LINEITEMBYIDS(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_LineItemByIds @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_LineItem_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_LINEITEM_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_LineItem_Multi @Keyword";
                }
            }
        }
        #endregion

        #region Cat_Item

        public static string hrm_cat_sp_get_Item
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ITEM";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Item";
                }
            }
        }
        public static string hrm_cat_sp_get_ItemById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ITEMBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ItemById @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_ItemByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ITEMBYIDS(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ItemByIds @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_Item_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ITEM_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Item_Multi @Keyword";
                }
            }
        }
        #endregion

        #region Cat_Brand


        public static string hrm_cat_sp_get_Brand
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_BRAND";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Brand";
                }
            }
        }
        public static string hrm_cat_sp_get_Brand_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_BRAND_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Brand_Multi @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_BrandByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_BRANDBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_BrandByIds @ID";
                }
            }
        }


        #endregion

        #region Cat_Unit

        public static string hrm_cat_sp_get_Unit
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_UNIT";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Unit";
                }
            }
        }
        public static string hrm_cat_sp_get_Unit_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_UNIT_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Unit_Multi @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_UnitById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_UNITBYID(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_UnitById @ID";
                }
            }
        }
        public static string hrm_cat_sp_get_UnitByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_UNITBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_UnitByIds @ID";
                }
            }
        }
        public static string hrm_cat_sp_get_SupplierById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SUPPLIERBYID(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SupplierById @ID";
                }
            }
        }

        public static string hrm_cat_sp_get_PurchaseItemsById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PRUCHASEITEMBYID(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_PurchaseItemsById @ID";
                }
            }
        }

        #endregion

        #region Cat_KBIBonus

        public static string hrm_cat_sp_get_KPIBonus
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_KPIBONUS";
                }
                else
                {
                    return "exec hrm_cat_sp_get_KPIBonus";
                }
            }
        }
        public static string hrm_cat_sp_get_KPIBonus_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_KPIBONUS_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_Cat_sp_get_KPIBonus_multi @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_KPIBonusIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_KPIBONUSBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_KPIBonusByIds @ID";
                }
            }
        }
        #endregion

        #region Cat_KPIBonusItem

        public static string hrm_cat_sp_get_KPIBonusItem
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_KPIBONUSITEM";
                }
                else
                {
                    return "exec hrm_cat_sp_get_KPIBonusItem";
                }
            }
        }
        public static string hrm_cat_sp_get_KPIBonusItemByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_KPIBONUSITEMBYIDS";
                }
                else
                {
                    return "exec hrm_cat_sp_get_KPIBonusItemByIds";
                }
            }
        }

        public static string hrm_cat_sp_get_KPIBonusItemByID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_KPIBONUSITEMBYID";
                }
                else
                {
                    return "exec hrm_cat_sp_get_KPIBonusItemByID @ID";
                }
            }
        }



        #endregion

        #region Cat_YouthUnionPosition
        /// <summary>
        /// Lấy danh sách dữ liệu Cat_YouthUnionPosition
        /// </summary>
        public static string hrm_cat_sp_get_YouthUnionPosition
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_YOUTHUNIONPOSITION";
                }
                else
                {
                    return "exec hrm_cat_sp_get_YouthUnionPosition";
                }
            }
        }
        public static string hrm_cat_sp_get_YouthUnionPositionById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_YOUTHUNIONBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_YouthUnionPositionById @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_YouthUnionPosition_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_YOUTHUNION_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_YouthUnionPosition_multi @Keyword";
                }
            }
        }
        #endregion

        #region Cat_CommunistPartyPosition
        /// <summary>
        /// Lấy danh sách dữ liệu Cat_CommunistPartyPosition
        /// </summary>
        public static string hrm_cat_sp_get_CommunistPartyPosition
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_COMMUNISTPARTY";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CommunistPartyPosition";
                }
            }
        }
        public static string hrm_cat_sp_get_CommunistPartyPositionById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_COMMUNISTPARTYBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CommunistPartyPositionById @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_CommunistPartyPosition_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_COMMUNIST_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CommunistPartyPosition_multi @Keyword";
                }
            }
        }
        #endregion

        #region Cat_WoundedSoldierTypes
        /// <summary>
        /// Lấy danh sách dữ liệu Cat_WoundedSoldierTypes
        /// </summary>
        public static string hrm_cat_sp_get_WoundedSoldierTypes
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_WOUNDEDSOLDIERTYPES";
                }
                else
                {
                    return "exec hrm_cat_sp_get_WoundedSoldierTypes";
                }
            }
        }
        public static string hrm_cat_sp_get_WoundedSoldierTypesById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_WSTYPESBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_WoundedSoldierTypesById @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_WoundedSoldierTypes_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_WSTYPES_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_WoundedSoldierTypes_multi @Keyword";
                }
            }
        }
        #endregion

        #region Cat_ArmedForceTypes
        /// <summary>
        /// Lấy danh sách dữ liệu Cat_ArmedForceTypes
        /// </summary>
        public static string hrm_cat_sp_get_ArmedForceTypes
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ArmedForceTypes";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ArmedForceTypes";
                }
            }
        }
        public static string hrm_cat_sp_get_ArmedForceTypesById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ArmedForceTypesBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ArmedForceTypesById @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_ArmedForceTypes_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ARFORCETYPES_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ArmedForceTypes_multi @Keyword";
                }
            }
        }
        #endregion

        #region Cat_PoliticalLevel
        /// <summary>
        /// Lấy danh sách dữ liệu Cat_PoliticalLevel
        /// </summary>
        public static string hrm_cat_sp_get_PoliticalLevel
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_POLITICALLEVEL";
                }
                else
                {
                    return "exec hrm_cat_sp_get_PoliticalLevel";
                }
            }
        }
        public static string hrm_cat_sp_get_PoliticalLevelById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_POLITICALLEVELBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_PoliticalLevelById @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_PoliticalLevel_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_POLILEVEL_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_PoliticalLevel_multi @Keyword";
                }
            }
        }
        #endregion

        #region Cat_TradeUnionistPosition
        /// <summary>
        /// Lấy danh sách dữ liệu Cat_TradeUnionistPosition
        /// </summary>
        public static string hrm_cat_sp_get_TradeUnionistPosition
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_TUPOSITION";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TradeUnionistPosition";
                }
            }
        }
        public static string hrm_cat_sp_get_TradeUnionistPositionById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_TUPOSITIONBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TradeUnionistPositionById @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_TradeUnionistPosition_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_TUPOSITION_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TradeUnionistPosition_multi @Keyword";
                }
            }
        }
        #endregion

        #region Cat_SelfDefenceMilitiaPosition
        /// <summary>
        /// Lấy danh sách dữ liệu Cat_SelfDefenceMilitiaPosition
        /// </summary>
        public static string hrm_cat_sp_get_SelfDefenceMilitiaPosition
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_SDMPOSITION";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SelfDefenceMilitiaPosition";
                }
            }
        }
        public static string hrm_cat_sp_get_SelfDefenceMilitiaPositionById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SDMPOSITIONBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SelfDefenceMilitiaPositionById @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_SelfDefenceMilitiaPosition_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SDMPOSITION_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SelfDefenceMilitiaPosition_multi @Keyword";
                }
            }
        }
        #endregion

        #region Cat_VeteranUnionPosition
        /// <summary>
        /// Lấy danh sách dữ liệu Cat_VeteranUnionPosition
        /// </summary>
        public static string hrm_cat_sp_get_VeteranUnionPosition
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_VUPOSITION";
                }
                else
                {
                    return "exec hrm_cat_sp_get_VeteranUnionPosition";
                }
            }
        }
        public static string hrm_cat_sp_get_VeteranUnionPositionById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_VUPOSITIONBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_VeteranUnionPositionById @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_VeteranUnionPosition_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_VUPOSITION_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_VeteranUnionPosition_multi @Keyword";
                }
            }
        }
        #endregion

        #region Lấy dữ liệu để xuất BC

        public static string hrm_cat_sp_get_AllOrg
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ALLORG";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AllOrg";
                }
            }
        }

        public static string hrm_cat_sp_get_AllEmpType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ALLEMPTYPE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AllEmpType";
                }
            }
        }

        public static string hrm_cat_sp_get_AllPosition
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ALLPOSITION";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AllPosition";
                }
            }
        }

        public static string hrm_cat_sp_get_AllOrgType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ALLORGTYPE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AllOrgType";
                }
            }
        }
        #endregion

        #region Evaluation

        #region performanceTemplate


        public static string hrm_eva_sp_get_performanceTemplate
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_PERFORMANCETEMP";
                }
                else
                {
                    return "exec hrm_eva_sp_get_performanceTemplate";
                }
            }
        }
        public static string hrm_hr_sp_get_PerformanceTemplateByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_PERTEMPBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_PerformanceTemplateByIds @Ids";
                }
            }
        }
        public static string hrm_cat_sp_get_TemplateById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_TEMPLATEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TemplateById @Id";
                }
            }
        }

        public static string hrm_eva_sp_get_KpiByTemplateID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_KPIBYTEMPLATEID";
                }
                else
                {
                    return "exec hrm_eva_sp_get_KpiByTemplateID";
                }
            }
        }

        public static string hrm_eva_sp_get_KpiByPerformanceID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_KPIBYPERFORMANCEID";
                }
                else
                {
                    return "exec hrm_eva_sp_get_KpiByPerformanceID";
                }
            }
        }

        public static string hrm_eva_sp_get_Template_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_TEMPLATE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_Template_Multi @Keyword";
                }
            }
        }

        public static string hrm_eva_sp_get_SaleTypebyID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_SALESTYPEID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_SalesTypeByID @Id";
                }
            }
        }


        #endregion
        #region [Tho.Bui] Eva_Level
        public static string hrm_eva_sp_get_EvaLevelById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_EVALEVELBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_EvaLevelById @Keyword";
                }
            }
        }

        public static string hrm_eva_sp_get_EvaLevelList
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_EVALEVELLIST";
                }
                else
                {
                    return "exec hrm_eva_sp_get_EvaLevelList";
                }
            }
        }
        public static string hrm_eva_sp_get_EvaLevelByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_EVALEVELBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_EvaLevelByIds @Ids";
                }
            }
        }
        #endregion
        #region [Quoc.Do] Eva_PerformanceEva
        public static string hrm_eva_sp_get_PerformanceEvaByPerformance
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_PEREVABYPERFORMANCE";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceEvaByPerformance";
                }
            }
        }
        public static string hrm_eva_sp_get_PerformanceEvaByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_PERFORMANCEEVABYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceEvaByIds @Ids";
                }
            }
        }
        public static string hrm_eva_sp_get_PerformanceEvaByProfile
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_PERFORMANCEEVABYPRO";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceEvaByProfile";
                }
            }
        }
        public static string hrm_eva_sp_get_PerformanceEvaWaitingEva
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_PERFORMANCEEVAWAIT";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceEvaWaitingEva";
                }
            }
        }
        public static string hrm_eva_sp_get_PerformanceEvaActive
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_PEREVAACTIVE";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceEvaActive";
                }
            }
        }
        public static string hrm_eva_sp_get_PerformanceEvaById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_PERFORMANCEEVABYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceEvaById @Keyword";
                }
            }
        }

        #endregion
        #region [Quoc.Do] Eva_Performance
        public static string hrm_eva_sp_get_PerformanceById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_PERFORMANCEBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceById @Keyword";
                }
            }
        }

        public static string hrm_eva_sp_get_Performance
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_PERFORMANCE";
                }
                else
                {
                    return "exec hrm_eva_sp_get_Performance";
                }
            }
        }

        // Chưa viết SQL
        public static string hrm_eva_sp_get_PerformanceResultByProfileID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_PERFORMANCEBYPROID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceResultByProfileID @Keyword";
                }
            }
        }

        // Chưa viết SQL
        public static string hrm_tra_sp_get_TrainningResultByProfileID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_TRAINRESULTBYPROID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TrainningResultByProfileID @Keyword";
                }
            }
        }

        public static string hrm_hr_sp_get_AppendixContractByProfileID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_APPENDIXBYPROID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_AppendixContractByProfileID @Keyword";
                }
            }
        }

        public static string hrm_eva_sp_get_PerformanceGeneral
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_PERFORMANCEGENERAL";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceGeneral";
                }
            }        
        }

        public static string hrm_eva_sp_get_PerformanceGeneralByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_PERGENERALBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceGeneralByIds @Ids";
                }
            }
        }

        public static string hrm_eva_sp_get_PerformanceEva
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_PERFORMACEEVA";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceEva";
                }
            }
        }



        public static string hrm_eva_sp_get_PerformanceByProfile
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_PERFORMANCEBYPRO";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceByProfile";
                }
            }
        }
        public static string hrm_eva_sp_get_PerformanceByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_PERFORMANCEBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceByIds @Ids";
                }
            }
        }
        #endregion
        #region [Quoc.Do] Eva_PerformancePlan
        public static string hrm_eva_sp_get_PerformancePlanById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_PERFORMANCEPLANBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformancePlanById @Keyword";
                }
            }
        }

        public static string hrm_eva_sp_get_PerformancePlan
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_PERFORMANCEPLAN";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformancePlan";
                }
            }
        }
        public static string hrm_eva_sp_get_PerformancePlanByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_PERFORPLANBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformancePlanByIds @Ids";
                }
            }
        }
        public static string hrm_eva_sp_get_PerformancePlan_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_PERPLAN_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformancePlan_Multi @Keyword";
                }
            }
        }
        #endregion
        #region [Quoc.Do] Eva_SaleBonus
        public static string hrm_eva_sp_get_SaleBonusById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_SALEBONUSBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_SaleBonusById @Keyword";
                }
            }
        }

        public static string hrm_eva_sp_get_SaleBonus
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_SALEBONUS";
                }
                else
                {
                    return "exec hrm_eva_sp_get_SaleBonus";
                }
            }
        }
        public static string hrm_eva_sp_get_BonusSalaryByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_BONUSSALBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_BonusSalaryByIds @Ids";
                }
            }
        }

        public static string hrm_eva_sp_get_SaleBonusByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_SALEBONUSBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_SaleBonusByIds @Ids";
                }
            }
        }

        public static string hrm_eva_getdata_SaleBonus
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_GETDATA_SALEBONUS";
                }
                else
                {
                    return "hrm_eva_getdata_SaleBonus";
                }
            }
        }
        #endregion

        #region [Quoc.Do] Eva_KPI
        public static string hrm_eva_sp_get_KPIById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_KPIBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_KPIById @Keyword";
                }
            }
        }

        public static string hrm_eva_sp_get_KPI
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_KPI";
                }
                else
                {
                    return "exec hrm_eva_sp_get_KPI";
                }
            }
        }
        public static string hrm_eva_sp_get_KPIByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_KPIBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_KPIByIds @Ids";
                }
            }
        }
        public static string hrm_eva_sp_get_KPI_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVAL_SP_GET_KPIMULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_KPI_Multi";
                }
            }
        }
        public static string hrm_cat_sp_get_objectKPI_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_OBJECTKPIMULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_objectKPI_Multi";
                }
            }
        }
        public static string hrm_eva_sp_get_KPIListByNameEntityID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_KPIBYNAMEENTITYID";
                }
                else
                {
                    return "exec hrm_eva_sp_get_KPIListByNameEntityID";
                }
            }
        }
        public static string hrm_eva_sp_get_KPIMutilByNameEntityID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_KPIMUTILBYNAMEENTITYID";
                }
                else
                {
                    return "exec hrm_eva_sp_get_KPIMutilByNameEntityID";
                }
            }
        }
        public static string hrm_eva_sp_get_KPIByEntityId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_KPIBYENTITYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_KPIByEntityId @Keyword";
                }
            }
        }
        #endregion

        #region Eva_SalesType

        public static string hrm_eva_sp_get_SalesType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_SALESTYPE";
                }
                else
                {
                    return "exec hrm_eva_sp_get_SalesType";
                }
            }
        }

        /// <summary> Lấy đối tượng SalesType bởi Danh sách ID </summary>
        public static string hrm_eva_sp_get_SalesTypeByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_SALESTYPEBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_SalesTypeByIds @Ids";
                }
            }
        }


        #endregion
        #region Eva_TagEva - Loại Đánh Giá
        public static string hrm_eva_sp_get_TagEva_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_TAGEVA_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_TagEva_multi @Keyword";
                }
            }
        }
        #endregion
        #region Eva_Level - Cấp Bậc
        public static string hrm_eva_sp_get_Level_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_LEVEL_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_Level_multi @Keyword";
                }
            }
        }
        #endregion
        #region Eva_PerformanceEvaDetail - Chi tiết kết quả đánh giá
        public static string hrm_eva_sp_get_PerformanceEvaDetailByPerformanceEvaID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_PEVADETAILBYPEVAID";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceEvaDetailByPerformanceEvaID";
                }
            }
        }
        public static string hrm_eva_sp_get_PerformanceEvaDetailAllByPerformanceEvaID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_PEVADEALLBYPEVAID";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceEvaDetailAllByPerformanceEvaID";
                }
            }
        }
        public static string hrm_eva_sp_get_PerformanceEvaDetailAllSelfByPerformanceEvaID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_PEVADEASELFBYPEVAID";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceEvaDetailAllSelfByPerformanceEvaID";
                }
            }
        }
        public static string hrm_eva_sp_get_PerformanceEvaDetailById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_PERFORMANCEEVADETAILBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceEvaDetailById";
                }
            }
        }
        #endregion

        #region Eva_SaleEvaluation - Doanh Số Cá Nhân

        public static string hrm_eva_sp_get_SaleEvaluation
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_SALEEVALUATION";
                }
                else
                {
                    return "exec hrm_eva_sp_get_SaleEvaluation";
                }
            }
        }
        public static string hrm_eva_sp_get_SaleEvaluationByProId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_SALEEVALUATIONBYPRO";
                }
                else
                {
                    return "exec hrm_eva_sp_get_SaleEvaluationByProId";
                }
            }
        }
        public static string hrm_eva_sp_get_SaleEvaluationByProfile
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_SALEEVALUATIONBYPRO";
                }
                else
                {
                    return "exec hrm_eva_sp_get_SaleEvaluationByProfile";
                }
            }
        }
        public static string hrm_hr_sp_get_SaleEvaluationByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_SALEEVALUATIONBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_SaleEvaluationByIds @Ids";
                }
            }
        }

        public static string hrm_eva_sp_get_SalesType_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_SALESTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_SalesType_multi @Keyword";
                }
            }
        }

        public static string hrm_eva_sp_get_SaleEvaluationByID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_SALEEVALUATIONID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_SaleEvaluationByID @Id";
                }
            }
        }

        #endregion

        #region Eva_Evaluator - Người đánh giá
        public static string hrm_eva_sp_get_Evaluator
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_EVALUATOR";
                }
                else
                {
                    return "exec hrm_eva_sp_get_Evaluator";
                }
            }
        }
        public static string hrm_hr_sp_get_EvaluatorByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_EVALUATORBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_EvaluatorByIds @Ids";

                }
            }
        }

        public static string hrm_eva_sp_get_EvaluatorById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_EVALUATORBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_EvaluatorById @Keyword";
                }
            }
        }

        public static string hrm_eva_sp_get_EvaluatorByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_EVALUATORBYPROFID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_EvaluatorByProfileId @Keyword";
                }
            }
        }

        /// <summary>  multi performanceTemplate </summary>
        public static string hrm_eva_sp_get_PerformanceTemplate_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_PERTEMPLATE_MULTI";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceTemplate_multi";
                }
            }
        }


        /// <summary>  multi load profiles nguoi danh gia </summary>
        public static string hrm_eva_sp_get_Evaluator_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_EVALUATOR_MULTI";
                }
                else
                {
                    return "exec hrm_eva_sp_get_Evaluator_multi";
                }
            }
        }

        #endregion

        #region Người Duyệt
        public static string hrm_fin_sp_get_ApproverECLAIM
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "FIN_SP_GET_APPROVERECLAIM";
                }
                else
                {
                    return "exec hrm_fin_sp_get_ApproverECLAIM";
                }
            }
        }

        public static string hrm_eva_sp_get_ApproverECLAIMById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin FIN_SP_GET_APPROVERECLAIMBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_ApproverECLAIMById @Keyword";
                }
            }
        }

        #endregion

        #region Eva_BonusSalary

        public static string hrm_eva_sp_get_BonusSalary
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_BONUSSALARY";
                }
                else
                {
                    return "exec hrm_eva_sp_get_BonusSalary";
                }
            }
        }


        #endregion
        #region Eva_PerformanceExtend

        public static string hrm_eva_sp_get_PerformanceExtendById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_PEREXTENDBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceExtendById @Keyword";
                }
            }
        }


        #endregion
        #region Eva_PerformanceForDetail

        public static string hrm_eva_sp_get_PerformanceForDetailByPerformanceId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin EVA_SP_GET_PERFORBYPERID";
                }
                else
                {
                    return "exec hrm_eva_sp_get_PerformanceForDetailByPerformanceId";
                }
            }
        }


        #endregion



        #endregion

        #region Cat_NameEntity
        /// <summary>
        /// Lấy danh sách dữ liệu Cat_NameEntity
        /// </summary>
        public static string hrm_cat_sp_get_NameEntity
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_NAMEENTITY";
                }
                else
                {
                    return "exec hrm_cat_sp_get_NameEntity";
                }
            }
        }
        public static string hrm_cat_sp_get_NameEntityByKPI
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_NAMEENTITYBYKPI";
                }
                else
                {
                    return "exec hrm_cat_sp_get_NameEntityByKPI";
                }
            }
        }
        public static string hrm_cat_sp_get_NameEntityById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_NAMEENTITYBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_NameEntityById @Keyword";
                }
            }
        }
        #endregion

        #region Insurance module

        #region [Quoc.Do] Ins_InsuranceRecord
        public static string hrm_ins_sp_get_InsuranceRecordById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin INS_SP_GET_INSURANCERECORDBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_ins_sp_get_InsuranceRecordById @Keyword";
                }
            }
        }

        public static string hrm_ins_sp_get_InsuranceRecord
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "INS_SP_GET_INSURANCERECORD";
                }
                else
                {
                    return "exec hrm_ins_sp_get_InsuranceRecord";
                }
            }
        }
        public static string hrm_ins_sp_get_InsuranceRecordByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin INS_SP_GET_INSRECORDBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_ins_sp_get_InsuranceRecordByIds @Ids";
                }
            }
        }

        public static string hrm_ins_sp_get_InsuranceSettlement
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "INS_SP_GET_INSURANCESETTLEMENT";
                }
                else
                {
                    return "exec hrm_ins_sp_get_InsuranceSettlement";
                }
            }
        }

        public static string hrm_ins_sp_get_InsuranceSettlementByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin INS_SP_GET_INSSETTLEMENTBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_ins_sp_get_InsuranceSettlementByIds @Ids";
                }
            }
        }
        #endregion

        #region [Quoc.Do] Ins_ChildSick
        public static string hrm_ins_sp_get_ChildSickById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin INS_SP_GET_CHILDSICKBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_ins_sp_get_ChildSickById @Keyword";
                }
            }
        }

        public static string hrm_ins_sp_get_ChildSick
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "INS_SP_GET_CHILDSICK";
                }
                else
                {
                    return "exec hrm_ins_sp_get_ChildSick";
                }
            }
        }
        public static string hrm_ins_sp_get_ChildSickByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin INS_SP_GET_CHILDSICKBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_ins_sp_get_ChildSickByIds @Ids";
                }
            }
        }
        public static string hrm_ins_sp_get_ChildSick_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "INS_SP_GET_CHILDSICK_MULTI";
                }
                else
                {
                    return "exec hrm_ins_sp_get_ChildSick_Multi";
                }
            }
        }
        #endregion

        #region Phan tich bao hiem

        public static string hrm_ins_sp_get_ProfileInsMonthly
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "INS_SP_GET_PROFINSMONTHLY";
                }
                else
                {
                    return "exec hrm_ins_sp_get_ProfileInsMonthly";
                }
            }
        }

        public static string hrm_ins_sp_get_ProfileInsMonthlyByProfileIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin INS_SP_GET_PROINSMONTHBYPROIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_ins_sp_get_ProfileInsMonthlyByProfileIds @Ids";
                }
            }
        }

        public static string hrm_ins_sp_get_ProfileInsMonthlyFromTo
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "INS_SP_GET_PROFINSMONTHLYFRMTO";
                }
                else
                {
                    return "exec hrm_ins_sp_get_ProfileInsMonthlyFromTo";
                }
            }
        }

        public static string hrm_ins_sp_get_D02
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "INS_SP_GET_D02";
                }
                else
                {
                    return "exec hrm_ins_sp_get_D02";
                }
            }
        }

        public static string hrm_ins_sp_get_D02ItemByD02ID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "INS_SP_GET_D02ITEMBYD02ID";
                }
                else
                {
                    return "exec hrm_ins_sp_get_D02ItemByD02ID";
                }
            }
        }


        public static string hrm_ins_sp_get_ProfInsMonthlyByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin INS_SP_GET_PROFINSMONTHLYBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_ins_sp_get_ProfInsMonthlyByIds @Ids";
                }
            }
        }

        #endregion

        #region Truy Lĩnh Bảo Hiểm

        public static string hrm_ins_sp_get_SalPayBack
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "INS_SP_GET_SALPAYBACK";
                }
                else
                {
                    return "exec hrm_ins_sp_get_SalPayBack";
                }
            }
        }

        public static string hrm_ins_sp_get_SalPayBackByID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin INS_SP_GET_SALPAYBACKBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_ins_sp_get_SalPayBackByID @Keyword";
                }
            }
        }
        #endregion

        #endregion

        #region Cat_ValueEntity
        public static string hrm_cat_sp_get_ValueEntityById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_VALUEENTITYBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ValueEntityById @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_ValueEntity
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_VALUEENTITY";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ValueEntity";
                }
            }
        }
        public static string hrm_cat_sp_get_ValueEntityByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_VALUEENTITYBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ValueEntityByIds @Ids";
                }
            }
        }
        #endregion

        #region Cat_RateInsurance
        public static string hrm_cat_sp_get_RateInsuranceById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_RATEINSURANCEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_RateInsuranceById @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_RateInsurance
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_RATEINSURANCE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_RateInsurance";
                }
            }
        }
        public static string hrm_cat_sp_get_RateInsuranceByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_RATEINSURANCEBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_RateInsuranceByIds @Ids";
                }
            }
        }
        #endregion
        #region Sys_ColumnMode
        public static string hrm_sys_sp_get_ColumnMode
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_COLUMNMODE";
                }
                else
                {
                    return "hrm_sys_sp_get_ColumnMode";
                }
            }
        }
        #endregion

        #region Sys_Version
        public static string hrm_sys_sp_get_Version
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_VERSION";
                }
                else
                {
                    return "hrm_sys_sp_get_Version";
                }
            }
        }
        #endregion

        #region Sys_ProcessApproved
        public static string hrm_sys_sp_get_ProcessApproved_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_PROCESSAPPR_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_ProcessApproved_multi @Keyword";
                }
            }
        }

        public static string hrm_sys_sp_get_ProcessApprovedById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_PROCESSAPPROVEDBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_ProcessApprovedById @Id";
                }
            }
        }

        public static string hrm_sys_sp_get_ProcessApproved
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_PROCESSAPPROVED";
                }
                else
                {
                    return "exec hrm_sys_sp_get_ProcessApproved";
                }
            }
        }
        public static string hrm_sys_sp_get_ProcessApprovedByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_PROCESSAPPROVEBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_ProcessApprovedByIds @Ids";
                }
            }
        }
        #endregion

        #region Sys_ConditionApproved
        public static string hrm_sys_sp_get_ConditionApprovedById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_CONDITIONAPPROVBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_ConditionApprovedById @Id";
                }
            }
        }

        public static string hrm_sys_sp_get_ConditionApproved
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_CONDITIONAPPROVED";
                }
                else
                {
                    return "exec hrm_sys_sp_get_ConditionApproved";
                }
            }
        }
        public static string hrm_sys_sp_get_ConditionApprovedByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_CONDITIONAPPROBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_ConditionApprovedByIds @Ids";
                }
            }
        }
        #endregion

        #region Sys_UserApprove
        public static string hrm_sys_sp_get_UserApproveById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_USERAPPROVEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_UserApproveById @Id";
                }
            }
        }

        public static string hrm_sys_sp_get_UserApprove
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_USERAPPROVE";
                }
                else
                {
                    return "exec hrm_sys_sp_get_UserApprove";
                }
            }
        }
        public static string hrm_sys_sp_get_UserApproveByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_USERAPPROVEBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_UserApproveByIds @Ids";
                }
            }
        }
        #endregion

        #region Sys_TemplateSendMail
        public static string hrm_sys_sp_get_TemplateSendMailById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_TEMPLATEMAILBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_TemplateSendMailById @Id";
                }
            }
        }

        public static string hrm_sys_sp_get_TemplateSendMail
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_TEMPLATESENDMAIL";
                }
                else
                {
                    return "exec hrm_sys_sp_get_TemplateSendMail";
                }
            }
        }
        public static string hrm_sys_sp_get_TemplateSendMailByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_TEMPLATEMAILBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_TemplateSendMailByIds @Ids";
                }
            }
        }
        #endregion

        #region Sys_ConfigProcessApprove
        public static string hrm_sys_sp_get_ConfigProcessApproveById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_CONFIGPROAPPROVEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_ConfigProcessApproveById @Id";
                }
            }
        }

        public static string hrm_sys_sp_get_CodeObjectById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_CODEOBJECTBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_CodeObjectById @Id";
                }
            }
        }

        public static string hrm_sys_sp_get_CodeConfigById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_CODECONFIGBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_CodeConfigById @Id";
                }
            }
        }

        public static string hrm_sys_sp_get_ConfigProcessApprove
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_CONFIGPROCESSAPP";
                }
                else
                {
                    return "exec hrm_sys_sp_get_ConfigProcessApprove";
                }
            }
        }
        public static string hrm_sys_sp_get_ConfigProcessApproveByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_ConfigPAPPROCEBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_ConfigProcessApproveByIds @Ids";
                }
            }
        }
        #endregion

        #region Cat_Shop

        public static string hrm_cat_sp_get_Shop
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_CAT_SP_GET_SHOP";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Shop";
                }
            }
        }

        public static string hrm_cat_sp_get_ShopGroup
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_SHOPGROUP";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ShopGroup";
                }
            }
        }

        public static string hrm_cat_sp_get_ShopById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_CAT_SP_GET_SHOPBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ShopById @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_ShopGroupById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SHOPGROUPBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ShopGroupById @Id";
                }
            }
        }

        public static string hrm_Cat_sp_get_Shop_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_CAT_SP_GET_SHOP_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_Cat_sp_get_Shop_multi @Keyword";
                }
            }
        }


        public static string hrm_Cat_sp_get_ShopIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_CAT_SP_GET_SHOPIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_Cat_sp_get_ShopIds @Ids";
                }
            }
        }
        public static string hrm_Cat_sp_get_ShopGroupIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_SHOPGROUPIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_Cat_sp_get_ShopGroupIds @Ids";
                }
            }
        }



        public static string hrm_cat_sp_get_LevelGeneralid
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_SHOPGROUPIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_LevelGeneralid @Ids";
                }
            }
        }
        public static string hrm_Cat_sp_get_ShopbyOrgID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_CAT_SP_GET_SHOPBYORGID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_Cat_sp_get_ShopbyOrgID @Id";
                }
            }
        }

        public static string hrm_Cat_sp_get_HDTJobTypeByGroupID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_HDTJOBTYPEBYGROUPID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_Cat_sp_get_HDTJobTypeByGroupID @Id";
                }
            }
        }

        public static string hrm_Cat_sp_get_HDTJobTypeCodeByGroupID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_HDTJOBTCBYGROUPID";
                }
                else
                {
                    return "exec hrm_Cat_sp_get_HDTJobTypeCodeByGroupID";
                }
            }
        }

        #endregion

        #region Sal_Estimate
        public static string hrm_sal_sp_get_Estimate
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_ESTIMATE";
                }
                else
                {
                    return "exec hrm_sal_sp_get_Estimate";
                }
            }
        }

        public static string hrm_sal_sp_get_EstimateDetail
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_ESTIMATEDETAIL";
                }
                else
                {
                    return "exec hrm_sal_sp_get_EstimateDetail";
                }
            }
        }

        #endregion

        #region Sal_RevenueForShop

        public static string hrm_sal_sp_get_RevenueForShop
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_SAL_SP_GET_REVENUE";
                }
                else
                {
                    return "exec hrm_sal_sp_get_RevenueForShop";
                }
            }
        }

        public static string hrm_sal_sp_get_RevenueForShopById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_REVENUEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_RevenueForShopById @Id";
                }
            }
        }
        #endregion

        #region Sal_LineItemForShop

        public static string hrm_sal_sp_get_LineItemForShop
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_SAL_SP_GET_LINEIFSHOP";
                }
                else
                {
                    return "exec hrm_sal_sp_get_LineItemForShop";
                }
            }
        }

        public static string hrm_sal_sp_get_LineItemForShopById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_LINEFORSHOPBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_LineItemForShopById @Id";
                }
            }
        }
        #endregion

        #region Sal_RevenueRecord
        public static string hrm_sal_sp_get_RevenueRecord
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_SAL_SP_GET_REVENUERECORD";
                }
                else
                {
                    return "exec hrm_sal_sp_get_RevenueRecord";
                }
            }
        }

        public static string hrm_sal_sp_get_RevenueRecordByShopID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_RVNRECORDBySHOPID";
                }
                else
                {
                    return "exec hrm_sal_sp_get_RevenueRecordByShopID";
                }
            }
        }
        public static string hrm_sal_sp_get_RevenueRecordForChildGrid
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_RVNRCODFORCHILDGRID";
                }
                else
                {
                    return "exec hrm_sal_sp_get_RevenueRecordForChildGrid";
                }
            }
        }
        public static string hrm_sal_sp_get_LineItemForChildGrid
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_LNITEMFORCHILDGRID";
                }
                else
                {
                    return "exec hrm_sal_sp_get_LineItemForChildGrid";
                }
            }
        }

        public static string hrm_sal_sp_get_ItemForChildGrid
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_ITEMFORCHILDGRID";
                }
                else
                {
                    return "exec hrm_sal_sp_get_ItemForChildGrid";
                }
            }
        }

        public static string hrm_sal_sp_get_RevenueRecordIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_SAL_SP_GET_REVENUERECORDIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_RevenueRecordIds @Ids";
                }
            }
        }
        #endregion

        #region Sal_ItemForShop

        public static string hrm_sal_sp_get_ItemForShop
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_SAL_SP_GET_ITEMFORSHOP";
                }
                else
                {
                    return "exec hrm_sal_sp_get_ItemForShop";
                }
            }
        }

        public static string hrm_sal_sp_get_ItemForShopById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_IFORSHOPBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_ItemForShopById @Id";
                }
            }
        }

        public static string hrm_sal_sp_get_SalDepartmentItemById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "Chua Vik";
                }
                else
                {
                    return "exec hrm_sal_sp_get_SalDepartmentItemById @Id";
                }
            }
        }

        public static string hrm_Sal_sp_get_SalaryDepartmentItem
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_SAL_SP_GET_DEPARTMENTITEM";
                }
                else
                {
                    return "exec hrm_Sal_sp_get_SalaryDepartmentItem";
                }
            }
        }

        #endregion

        #region Sal_RevenueForProfile
        public static string hrm_sal_sp_get_RevenueForProfile
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_REVENFORPROFILE";
                }
                else
                {
                    return "exec hrm_sal_sp_get_RevenueForProfile";
                }
            }
        }

        public static string hrm_sal_sp_get_RevenueForProfileById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_REVENFORPROBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_RevenueForProfileById @Id";
                }
            }
        }
        #endregion

        #region SAL_Deparment

        public static string hrm_sal_sp_get_SalaryDeparment
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_SARARYDEPARTMENT";
                }
                else
                {
                    return "exec hrm_sal_sp_get_SalaryDeparment";
                }
            }
        }
        public static string hrm_sal_sp_get_SalDepartmentById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "Chua Vik";
                }
                else
                {
                    return "exec hrm_sal_sp_get_SalDepartmentById @Id";
                }
            }
        }
        public static string hrm_sal_sp_get_SalDepartmentByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_SARARYDEPARTMBYIDS (:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_sal_sp_get_SalDepartmentByIds @ID";
                }
            }

        }
        #endregion

        #region Att_TimeSheet

        public static string hrm_att_sp_get_TimeSheet
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_ATT_SP_GET_TIMESHEET";
                }
                else
                {
                    return "exec hrm_att_sp_get_TimeSheet";
                }
            }
        }

        public static string hrm_att_sp_get_TimeSheetById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_ATT_SP_GET_TIMESBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_TimeSheetById @Id";
                }
            }
        }
        #endregion

        #region Cat_UnitPrice

        public static string hrm_cat_sp_get_UnitPrice
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_CAT_SP_GET_UNITPRICE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_UnitPrice";
                }
            }
        }
        public static string hrm_cat_sp_get_UnitPriceByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_CAT_SP_GET_UNITPBYIDS(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_UnitPriceByIds @Id";
                }
            }
        }
        public static string hrm_cat_sp_get_UnitPriceById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_CAT_SP_GET_UNITPBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_UnitPriceById @Id";
                }
            }
        }

        #endregion

        #region Cat_JobType

        public static string hrm_cat_sp_get_JobType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_CAT_SP_GET_JOBTYPE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_JobType";
                }
            }
        }
        public static string hrm_cat_sp_get_JobTypeByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_CAT_SP_GET_JOBTYPEBYIDS(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_JobTypeByIds @Id";
                }
            }
        }
        public static string hrm_cat_sp_get_JobTypeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_CAT_SP_GET_JOBTYPEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_JobTypeById @Id";
                }
            }
        }

        public static string hrm_sys_sp_get_LockObjectByID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_LOCKOBJECTBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_LockObjectByID @Id";
                }
            }
        }

        public static string hrm_sys_sp_get_LockObjectItemByID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SYS_SP_GET_LOCKOBJECTITEMBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sys_sp_get_LockObjectItemByID @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_JobType_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_CAT_SP_GET_JOBTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_JobType_multi @Keyword";
                }
            }
        }
        #endregion

        #region Cat_Role

        public static string hrm_cat_sp_get_Role
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_CAT_SP_GET_ROLE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Role";
                }
            }
        }

        public static string hrm_cat_sp_get_MasterDataGroup
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_MASTERDATAGROUP";
                }
                else
                {
                    return "exec hrm_cat_sp_get_MasterDataGroup";
                }
            }
        }

        public static string hrm_cat_sp_get_MasterDataGroupid
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_MASTERDATAGROUPBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_MasterDataGroupid @Id";
                }
            }
        }

        /// <summary> Lấy danh sách đối tượng MasterDataGroupItemBy By MasterDataGroupID</summary>
        public static string hrm_cat_sp_get_MasterDataGroupItemByMasterDataGroupID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_MSTBYDATAGROUPID";
                }
                else
                {
                    return "exec hrm_cat_sp_get_MasterDataGroupItemByMasterDataGroupID";
                }
            }
        }


        public static string hrm_cat_sp_get_RoleByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_CAT_SP_GET_ROLEBYIDS(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_RoleByIds @Id";
                }
            }
        }
        public static string hrm_cat_sp_get_RoleById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_CAT_SP_GET_ROLEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_RoleById @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_Role_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_CAT_SP_GET_ROLE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Role_multi @Keyword";
                }
            }
        }
        #endregion

        #region Cat_KPIBonus
        public static string hrm_Cat_sp_get_KPIBonus_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_CAT_SP_GET_KPIBONUS_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_Cat_sp_get_KPIBonus_multi @Keyword";
                }
            }
        }
        #endregion

        #region Cat_Owner
        public static string hrm_cat_sp_get_Owner
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_OWNER";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Owner";
                }
            }
        }


        public static string hrm_cat_sp_get_OwnerByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_OWNERBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OwnerByIds @ID";
                }
            }
        }
        public static string hrm_cat_sp_get_Owner_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_OWNER_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Owner_Multi @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_Function_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_FUNCTION_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Function_Multi @Keyword";
                }
            }
        }

        #endregion

        #region Cat_TrainCategory

        public static string hrm_cat_sp_get_TrainCategory
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_TRAINCATEGORY";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TrainCategory";
                }
            }
        }
        public static string hrm_cat_sp_get_TrainCategoryByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_TRAINCATEGORYBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TrainCategoryByIds @ID";
                }
            }
        }
        public static string hrm_cat_sp_get_TrainCategory_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_TRAINCATEGORY_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TrainCategory_Multi @Keyword";
                }
            }

        }

        #endregion


        #region Cat_TrainLevel
        public static string hrm_cat_sp_get_TrainLevel
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_TRAINLEVEL";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TrainLevel";
                }
            }
        }

        public static string hrm_cat_sp_get_TrainLevelByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_TRAINLEVELBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TrainLevelByIds @ID";
                }
            }
        }

        public static string hrm_cat_sp_get_TrainLevel_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_TRAINLEVEL_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TrainLevel_Multi @Keyword";
                }
            }

        }
        #endregion

        #region Cat_DataGroupDetail
        public static string hrm_cat_sp_get_DataGroupDetailByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_DTGROUPDETAILLBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_DataGroupDetailByIds @ID";
                }
            }
        }

        public static string hrm_cat_sp_get_DataGroupDetailByDTGroupID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_DTGROUPDTBYDTGID";
                }
                else
                {
                    return "exec hrm_cat_sp_get_DataGroupDetailByDTGroupID";
                }
            }
        }


        #endregion
        #region Cat_ReceivePlace
        public static string hrm_Cat_SP_GET_RECEIVEPLACE
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "Cat_SP_GET_RECEIVEPLACE";
                }
                else
                {
                    return "exec hrm_Cat_SP_GET_RECEIVEPLACE";
                }
            }
        }
        public static string hrm_Cat_SP_GET_RECEIVEPLACEBYID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin Cat_SP_GET_RECEIVEPLACEBYID (:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_Cat_SP_GET_RECEIVEPLACEBYID @Id";
                }
            }
        }
        #endregion
        #region Cat_Model
        public static string hrm_Cat_sp_get_CatModel
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "Cat_sp_get_CatModel";
                }
                else
                {
                    return "exec hrm_Cat_sp_get_CatModel";
                }
            }
        }
        public static string hrm_Cat_SP_GET_ModelByModelID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin Cat_SP_GET_ModelByModelID (:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_Cat_SP_GET_ModelByModelID @Id";
                }
            }
        }
        public static string hrm_Cat_SP_GET_PurColorByID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin Cat_SP_GET_PurColorByID (:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_Cat_SP_GET_PurColorByID @Id";
                }
            }
        }
        #endregion
        #region Cat_DataGroup
        public static string hrm_cat_sp_get_DataGroupByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_DATAGROUPDTBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_DataGroupByIds @ID";
                }
            }
        }
        public static string hrm_cat_sp_get_DataGroup
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_DATAGROUP";
                }
                else
                {
                    return "exec hrm_cat_sp_get_DataGroup";
                }
            }
        }

        #endregion
        #region Cat_SourceAds
        public static string hrm_cat_sp_get_SourceAds_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SOURCEADS_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SourceAds_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_SourceAdsById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SOURCEADSBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SourceAdsById @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_SourceAdsByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SOURCEASDBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SourceAdsByIds @ID";
                }
            }
        }
        public static string hrm_cat_sp_get_SourceAds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_SOURCEADS";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SourceAds";
                }
            }
        }


        #endregion
        #region MyRegion
        public static string hrm_att_sp_get_CompensationDetailByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_COMPENSTDTBYPROID";
                }
                else
                {
                    return "exec hrm_att_sp_get_CompensationDetailByProfileId";
                }
            }
        }
        #endregion
        #region Hre_AnnualDetailView
        public static string hrm_att_sp_get_AnnualDetailByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_ANNUALDETAILBYPROID";
                }
                else
                {
                    return "exec hrm_att_sp_get_AnnualDetailByProfileId";
                }
            }
        }
        public static string hrm_att_sp_get_AnnualLeaveByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_ANNUALLEAVEBYPROID";
                }
                else
                {
                    return "exec hrm_att_sp_get_AnnualLeaveByProfileId";
                }
            }
        }
        #endregion
        #region Kai_RankMark
        public static string hrm_kai_sp_get_RankMarkById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin KAI_SP_GET_RANKMARKBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_kai_sp_get_RankMarkById @Id";
                }
            }
        }

        public static string hrm_kai_sp_get_RankMarkByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin KAI_SP_GET_RANKMARKBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_kai_sp_get_RankMarkByIds @ID";
                }
            }
        }
        public static string hrm_kai_sp_get_RankMark
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "KAI_SP_GET_RANKMARK";
                }
                else
                {
                    return "exec hrm_kai_sp_get_RankMark";
                }
            }
        }


        #endregion

        #region Sal_ProductCapacity
        public static string hrm_sal_sp_get_ProductCapacityById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_PRODUCTCAPACITYBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_ProductCapacityById @Id";
                }
            }
        }

        public static string hrm_sal_sp_get_ProductCapacityByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_PRODUCTCAPCTBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_sal_sp_get_ProductCapacityByIds @ID";
                }
            }
        }
        public static string hrm_sal_sp_get_ProductCapacity
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_PRODUCTCAPACITY";
                }
                else
                {
                    return "exec hrm_sal_sp_get_ProductCapacity";
                }
            }
        }
        #endregion

        #region Cat_ExchangeRate
        public static string hrm_cat_sp_get_ExchangeRateByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_EXCHANGERATEBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ExchangeRateByIds @ID";
                }
            }
        }
        public static string hrm_cat_sp_get_ExchangeRate
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_EXCHANGERATE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ExchangeRate";
                }
            }
        }
        #endregion

        #region Cat_Village
        public static string hrm_cat_sp_get_VillageByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_VILLAGEBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_VillageByIds @ID";
                }
            }
        }
        public static string hrm_cat_sp_get_Village
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_VILLAGE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Village";
                }
            }
        }
        #endregion



        #region Cat_UnAllowCfgAmount
        public static string hrm_cat_sp_get_UnAllowCfgAmountByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_UNALCFGAMOUNTBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_UnAllowCfgAmountByIds @ID";
                }
            }
        }
        public static string hrm_cat_sp_get_Cat_UnAllowCfgAmount
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_UNALCFGAMOUNT";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Cat_UnAllowCfgAmount";
                }
            }
        }

        #endregion
        #region Cat_CateCode
        public static string hrm_cat_sp_get_CateCodeByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_CATECODEBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CateCodeByIds @ID";
                }
            }
        }
        public static string hrm_cat_sp_get_CateCode
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_CATECODE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CateCode";
                }
            }
        }
        #endregion
        #region Cat_MasterProject
        public static string hrm_cat_sp_get_MasterProjectByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_MASTERPROJECTBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_MasterProjectByIds @ID";
                }
            }
        }
        public static string hrm_cat_sp_get_MasterProject
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_MASTERPROJECT";
                }
                else
                {
                    return "exec hrm_cat_sp_get_MasterProject";
                }
            }
        }
        #endregion

        public static string hrm_hr_sp_Profile_MultiByPara
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_PROFILE_MULTIBYPARA";
                }
                else
                {
                    return "exec hrm_hr_sp_Profile_MultiByPara";
                }
            }
        }

        public static string hrm_sal_sp_get_RevenueForShopIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_SAL_SP_GET_REVEFORSHOPIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_RevenueForShopIds @Ids";
                }
            }
        }

        public static string hrm_sal_sp_get_ItemForShopIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_SAL_SP_GET_ITEMFORSHOPIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_ItemForShopIds @Ids";
                }
            }
        }

        public static string hrm_sal_sp_get_LineItemForShopIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_SAL_SP_GET_LITEMFORSHOPIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_LineItemForShopIds @Ids";
                }
            }
        }

        public static string hrm_sal_sp_get_RevenueForProfileIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_SAL_SP_GET_REFORPROIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_RevenueForProfileIds @Ids";
                }
            }
        }

        public static string hrm_sys_sp_get_AutoBackup
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SYS_SP_GET_AUTOBACKUP";
                }
                else
                {
                    return "exec hrm_sys_sp_get_AutoBackup";
                }
            }
        }

        #region Report
        public static string hrm_rep_sp_get_Master
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REP_SP_GET_MASTER";
                }
                else
                {
                    return "exec hrm_rep_sp_get_Master";
                }
            }
        }
        public static string hrm_rep_sp_get_MasterByID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REP_SP_GET_MASTERBYID(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rep_sp_get_MasterByID @Id";
                }
            }
        }

        public static string hrm_rep_sp_get_ConditionItemByID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REP_SP_GET_CONDITIONITEMBYID(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rep_sp_get_ConditionItemByID @Id";
                }
            }
        }
        public static string hrm_rep_sp_get_ConditionByMasterID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REP_SP_GET_CONDITIONBYMASTERID";
                }
                else
                {
                    return "exec hrm_rep_sp_get_ConditionByMasterID";
                }
            }
        }
        public static string hrm_rep_sp_get_ConditionItemByMasterID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REP_SP_GET_CONDITIONITEM";
                }
                else
                {
                    return "exec hrm_rep_sp_get_ConditionItem";
                }
            }
        }
        public static string hrm_rep_sp_get_ColumnByMasterID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REP_SP_GET_COLUMNBYMASTERID";
                }
                else
                {
                    return "exec hrm_rep_sp_get_ColumnByMasterID";
                }
            }
        }
        public static string hrm_rep_sp_get_ConditionItem
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REP_SP_GET_CONDITIONITEM";
                }
                else
                {
                    return "exec hrm_rep_sp_get_ConditionItem";
                }
            }
        }
        public static string hrm_rep_sp_get_ExcuteQuery
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "Chưa viết";
                }
                else
                {
                    return "exec hrm_rep_sp_get_ExcuteQuery @Id";
                }
            }
        }

        #endregion



        #region Hre_Recruiment

        #region Rec_QuestionType

        public static string hrm_rec_sp_get_QuestionType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_QUESTIONTYPE";
                }
                else
                {
                    return "exec hrm_rec_sp_get_QuestionType";
                }
            }
        }

        public static string hrm_rec_sp_get_QuestionTypeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_QUTYPEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_QuestionTypeById @Id";
                }
            }
        }

        public static string hrm_rec_sp_get_QuestionType_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_QUESTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_QuestionType_multi @Keyword";
                }
            }
        }
        #endregion
        #region Rec_RecruitmentCampaignItem
        public static string hrm_rec_sp_get_RecruitmentCampaignItem
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_RECCAMPAIGNITEM";
                }
                else
                {
                    return "exec hrm_rec_sp_get_RecruitmentCampaignItem";
                }
            }
        }

        public static string hrm_rec_sp_get_RecruitmentCampaignItemById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_RECAMPAIGNITEMBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_RecruitmentCampaignItemById @Id";
                }
            }
        }
        public static string hrm_rec_sp_get_RecruitmentCampaignItemByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_RECAMPAIGNITEMBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_RecruitmentCampaignItemByIds @Ids";
                }
            }
        }
        #endregion
        #region Rec_RecruitmentCampaign
        public static string hrm_rec_sp_get_RecruitmentCampaign_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_RECAMPAIGN_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_RecruitmentCampaign_multi @Keyword";
                }
            }
        }
        public static string hrm_rec_sp_get_RecruitmentCampaign
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_RECRUITMENTCAMPAIGN";
                }
                else
                {
                    return "exec hrm_rec_sp_get_RecruitmentCampaign";
                }
            }
        }

        public static string hrm_rec_sp_get_RecCosDetail
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_RECCOSDETAIL";
                }
                else
                {
                    return "exec hrm_rec_sp_get_RecCosDetail";
                }
            }
        }

        public static string hrm_rec_sp_get_RecruitmentCampaignById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_RECAMPAIGNBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_RecruitmentCampaignById @Id";
                }
            }
        }
        public static string hrm_rec_sp_get_RecruitmentCampaignByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_RECAMPAIGNBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_RecruitmentCampaignByIds @Ids";
                }
            }
        }

        public static string hrm_rec_sp_get_CostDetailByRecruitmentCampaignId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_COSTDETAILBYCAMID(:ID,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_CostDetailByRecruitmentCampaignId @ID";
                }
            }
        }

        public static string hrm_rec_sp_get_RecCostDetailById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_COSTDETAILBYID(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_rec_sp_get_RecCostDetailById @ID";
                }
            }
        }
        #endregion
        #region Rec_QuestionBank
        public static string hrm_rec_sp_get_QuestionBank
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_QUESTIONBANK";
                }
                else
                {
                    return "exec hrm_rec_sp_get_QuestionBank";
                }
            }
        }

        public static string hrm_rec_sp_get_QuestionBankById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_QUESTIONBANKBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_QuestionBankById @Id";
                }
            }
        }
        public static string hrm_rec_sp_get_QuestionBankByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_QUESBANKBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_QuestionBankByIds @Ids";
                }
            }
        }
        public static string hrm_rec_sp_get_QuestionBank_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_QUESTIONBANK_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_QuestionBank_multi @Keyword";
                }
            }
        }
        #endregion
        #region Rec_Questionaire
        public static string hrm_rec_sp_get_Questionaire_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_QUESTIONAIRE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_Questionaire_Multi @Keyword";
                }
            }
        }
        public static string hrm_rec_sp_get_Questionaire
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_QUESTIONAIRE";
                }
                else
                {
                    return "exec hrm_rec_sp_get_Questionaire";
                }
            }
        }

        public static string hrm_rec_sp_get_QuestionaireById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_QUESTIONAIREBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_QuestionaireById @Id";
                }
            }
        }
        public static string hrm_rec_sp_get_QuestionaireByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_QUESTIONAIREBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_QuestionaireByIds @Ids";
                }
            }
        }

        #endregion
        #region Rec_Question
        public static string hrm_rec_sp_get_QuestionByQuestionaireId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_QUESTIONBYQAIREID ";
                }
                else
                {
                    return "exec hrm_rec_sp_get_QuestionByQuestionaireId";
                }
            }
        }

        public static string hrm_rec_sp_get_QuestionById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_QUESTIONBYID (:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_QuestionById @Id";
                }
            }
        }


        #endregion
        #region Rec_JobVacancy
        public static string hrm_rec_sp_get_JobVacancy_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_JOBVACANCY_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_JobVacancy_multi @Keyword";
                }
            }
        }

        public static string hrm_rec_sp_get_JobVacancy
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_JOBVACANCY";
                }
                else
                {
                    return "exec hrm_rec_sp_get_JobVacancy";
                }
            }
        }

        public static string hrm_rec_sp_get_JobVacancyById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_JOBVACANCYBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_JobVacancyById @Id";
                }
            }
        }
        public static string hrm_rec_sp_get_JobVacancyByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_JOBVACANCYBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_JobVacancyByIds @Ids";
                }
            }
        }
        #endregion
        #region [Tho.Bui]:Rec_Interview
        /// <summary>
        /// get all list ineterview
        /// </summary>
        public static string hrm_rec_sp_get_Interview
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_REC_SP_GET_INTERVIEW";
                }
                else
                {
                    return "exec hrm_rec_sp_get_Interview";
                }
            }
        }
        /// <summary>
        /// get levelinterview and jobvancyID from Rec_Interview by candidateID have paramater
        /// </summary>
        public static string hrm_rec_sp_get_CandidateId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_CANDIDATEID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_CandidateId @Id";
                }
            }
        }
        /// <summary>
        /// get list interview from Rec_Interview by ID have paramater
        /// </summary>
        public static string hrm_rec_sp_get_InterviewById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_REC_SP_GET_INTERVIEWBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_InterviewById @Id";
                }
            }
        }
        /// <summary>
        /// get list interview from Rec_Interview by ID haven't paramater
        /// </summary>
        public static string hrm_rec_sp_get_InterviewById1
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_REC_SP_GET_INTERVIEWBYID(:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_InterviewById";
                }
            }
        }
        public static string hrm_rec_sp_get_InterviewByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_REC_SP_GET_INTERVIEWBYIDS(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_InterviewByIds @Id";
                }
            }
        }
        //Load JobVancyName
        public static string hrm_rec_sp_get_JobVacancyId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_JOBVANCANCYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_JobVacancyId @Id";
                }
            }
        }
        public static string hrm_rec_sp_get_JobVacancyId1
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_JOBVANCANCYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_JobVacancyId @Id";
                }
            }
        }
        /// <summary>
        /// get list interview from Rec_Interview by candidateID
        /// </summary>
        /// 
        public static string hrm_rec_sp_get_InterviewByCandidateID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_INTERVIEWBYCANID";
                }
                else
                {
                    return "exec hrm_rec_sp_get_InterviewByCandidateID";
                }
            }
        }
        #endregion
        #region [Tho.Bui] Rec_InterviewCampaignDetail
        public static string hrm_rec_sp_get_InterviewCampaignDetail
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_INTERVIEWCPDETAIL";
                }
                else
                {
                    return "exec hrm_rec_sp_get_InterviewCampaignDetail";
                }
            }
        }
        public static string hrm_rec_sp_get_CandidateByCdCpDetailId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_CDDBYCDCPDETAILID";
                }
                else
                {
                    return "hrm_rec_sp_get_CandidateByCdCpDetailId";
                }
            }
        }
        public static string hrm_rec_sp_get_InCamDetailByCddId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_IVCPDETAILBYCAID";
                }
                else
                {
                    return "hrm_rec_sp_get_InCamDetailByCddId";
                }
            }
        }

        public static string hrm_rec_sp_get_InCamDetailByLstCandidateIDs
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_INCAMDEBYLSTCAIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_InCamDetailByLstCandidateIDs @Ids";
                }
            }
        }

        // lấy chi tiết ke hoach phong vang bang recruimenthistoryids
        public static string hrm_rec_sp_get_InCamDetailByLstRechisIDs
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_INCAMDEBYRECHISIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_InCamDetailByLstRechisIDs @Ids";
                }
            }
        }

        public static string hrm_rec_sp_get_RecHisByListCandidateID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_RECHISBYLSTCAID(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_RecHisByListCandidateID @Ids";
                }
            }
        }

        public static string hrm_rec_sp_get_InterviewCampaignDetailByCddId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_INTERCAMDECANID(:ID,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_InterviewCampaignDetailByCddId @ID";
                }
            }
        }
        #endregion
        #region Rec_CandidateHistory
        public static string hrm_rec_sp_get_CandidateHistoryByCandidateID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_REC_SP_GET_CANDIDATEHISTORYBYCANDIDATEID";
                }
                else
                {
                    return "exec hrm_rec_sp_get_CandidateHistoryByCandidateID";
                }
            }
        }
        #endregion

        #region Rec_CandidateBusiness
        public static string hrm_rec_sp_get_CandidateBusinessByCandidateId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_CABUBYCANDIDATEID";
                }
                else
                {
                    return "exec hrm_rec_sp_get_CandidateBusinessByCandidateId";
                }
            }
        }
        public static string hrm_rec_sp_get_CandidateBusiness
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_CANDIDATEBUSINESS";
                }
                else
                {
                    return "exec hrm_rec_sp_get_CandidateBusiness";
                }
            }
        }

        public static string hrm_rec_sp_get_CandidateBusinessById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_CANBUSINESSBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_CandidateBusinessById @Id";
                }
            }
        }
        public static string hrm_rec_sp_get_CandidateBusinessByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_CANBUSINESSBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_CandidateBusinessByIds @Ids";
                }
            }
        }

        #endregion

        #region Rec_Relative
        public static string hrm_rec_sp_get_RelativeByCandidateId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_RELATIVEBYCANID";
                }
                else
                {
                    return "exec hrm_rec_sp_get_RelativeByCandidateId";
                }
            }
        }
        public static string hrm_rec_sp_get_Relative
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_RELATIVE";
                }
                else
                {
                    return "exec hrm_rec_sp_get_Relative";
                }
            }
        }

        public static string hrm_rec_sp_get_RelativeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_RELATIVEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_RelativeById @Id";
                }
            }
        }
        public static string hrm_rec_sp_get_RelativeByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_RELATIVEBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_RelativeByIds @Ids";
                }
            }
        }

        #endregion
        #endregion

        #region Rec_Tag

        public static string hrm_rec_sp_get_Tag
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_TAG";
                }
                else
                {
                    return "exec hrm_rec_sp_get_Tag";
                }
            }
        }
        public static string hrm_rec_sp_get_Tag_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_TAG_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_Tag_Multi @Keyword";
                }
            }
        }

        public static string hrm_rec_sp_get_TagById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_TAGBYID(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_rec_sp_get_TagById @ID";
                }
            }
        }
        #endregion
        #region Rec_SourceAds

        public static string hrm_rec_sp_get_SourceAdsByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_SOURCEASDBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_rec_sp_get_SourceAdsByIds @ID";
                }
            }
        }
        public static string hrm_rec_sp_get_SourceAds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_SOURCEADS";
                }
                else
                {
                    return "exec hrm_rec_sp_get_SourceAds";
                }
            }
        }
        public static string hrm_rec_sp_get_SourceAds_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_SOURCEADS_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_SourceAds_Multi @Keyword";
                }
            }
        }

        #endregion

        #region Rec_RecruitmentHistory
        public static string hrm_rec_sp_get_RecruitmentHistory
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_RECRUIMENTHISTORY";
                }
                else
                {
                    return "exec hrm_rec_sp_get_RecruitmentHistory";
                }
            }
        }
        public static string hrm_rec_sp_get_RecruitmentHistoryById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_RECRUIMENTHTRBYID(:ID, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_rec_sp_get_RecruitmentHistoryById @ID";
                }
            }
        }

        #endregion
        #region Rec_Candidate
        public static string hrm_rec_sp_getdata_FilterCandidate
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GETDATA_FILTERCANDIDATE";
                }
                else
                {
                    return "exec hrm_rec_sp_getdata_FilterCandidate";
                }
            }
        }

        public static string hrm_rec_sp_get_Candidate
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_CANDIDATE";
                }
                else
                {
                    return "exec hrm_rec_sp_get_Candidate";
                }
            }
        }

        public static string hrm_rec_sp_get_CandidateBlack
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_CANDIDATEBLACK";
                }
                else
                {
                    return "exec hrm_rec_sp_get_CandidateBlack";
                }
            }
        }

        public static string hrm_rec_sp_get_Candidate_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_CANDIDATE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_Candidate_Multi @Keyword";
                }
            }
        }
        public static string hrm_rec_sp_get_CandidateById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_CANDIDATEBYID(:ID, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_rec_sp_get_CandidateById @ID";
                }
            }
        }
        public static string hrm_rec_sp_get_CandidateById1
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_CANDIDATEBYID";
                }
                else
                {
                    return "hrm_rec_sp_get_CandidateById";
                }
            }
        }


        public static string hrm_rec_sp_get_GroupConditionByID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_GROUPCONDITIONBYID";
                }
                else
                {
                    return "hrm_rec_sp_get_GroupConditionByID";
                }
            }
        }
        public static string hrm_rec_sp_get_CandidateByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_CANDIDATEBYIDS(:Ids, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_rec_sp_get_CandidateByIds @ID";
                }
            }
        }
        public static string hrm_rec_sp_Set_Candidate_Status
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_SET_CANDIDATE_STATUS";
                }
                else
                {
                    return "hrm_rec_sp_Set_Candidate_Status";
                }
            }
        }

        public static string hrm_hr_sp_get_Candidate_GeneralGrid
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_CANDIDATEGENERALGRID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_Candidate_GeneralGrid";
                }
            }
        }
        public static string hrm_hr_sp_get_CandidateByMoreIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_CANDIDATEBYMOREIDS";
                }
                else
                {
                    return "hrm_hr_sp_get_CandidateByMoreIds";
                }
            }
        }
        public static string hrm_hr_sp_get_RecHistoryByMoreIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_RECHISTORYBYMOREIDS";
                }
                else
                {
                    return "hrm_hr_sp_get_RecHistoryByMoreIds";
                }
            }
        }
        public static string hrm_rec_sp_get_InterviewDetailByCandidateID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_INTERDETAILBYCANID(:ID, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_rec_sp_get_InterviewDetailByCandidateID @ID";
                }
            }
        }
        #endregion
        #region Rec_recruimentHistory
        public static string hrm_rec_sp_get_RecruimentHistoryByCandidateID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_RCMHISTORBYCANID";
                }
                else
                {
                    return "exec hrm_rec_sp_get_RecruimentHistoryByCandidateID";
                }
            }
        }

        public static string hrm_rec_sp_get_RecHisByCandidateID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_RECHISBYCANDIDATEID(:ID,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_RecHisByCandidateID @ID";
                }
            }
        }

        public static string hrm_rec_sp_get_RecHistoryByCandidateID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_RECHISTORYBYCANID(:ID,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_RecHistoryByCandidateID @ID";
                }
            }
        }

        #endregion
        #region Rec_CandidateQualification
        public static string hrm_rec_sp_get_CandidateQualificationByCandidateID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_CANDDQUALIFIBYCANID";
                }
                else
                {
                    return "exec hrm_rec_sp_get_CandidateQualificationByCandidateID";
                }
            }
        }

        public static string hrm_rec_sp_get_Rec_CandidateQualificationById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_CANDDQUALIFIBYID(:Ids, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_rec_sp_get_Rec_CandidateQualificationById @Id";
                }
            }
        }

        #endregion
        #region Rec_CandidateComputingLevel
        public static string hrm_rec_sp_get_CandidateComputingLevelByCandidateID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_CANDDCOMLVBYCANID";
                }
                else
                {
                    return "exec hrm_rec_sp_get_CandidateComputingLevelByCandidateID";
                }
            }
        }
        public static string hrm_rec_sp_get_Rec_CandidateComputingLevelById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_CANDDCOMLVBYID(:Ids, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_rec_sp_get_Rec_CandidateComputingLevelById @Id";
                }
            }
        }

        #endregion
        #region Rec_CandidateLanguageLevel
        public static string hrm_rec_sp_get_CandidateLanguageLevelByCandidateID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_CANDDLANLVBYCANID";
                }
                else
                {
                    return "exec hrm_rec_sp_get_CandidateLanguageLevelByCandidateID";
                }
            }
        }
        public static string hrm_rec_sp_get_Rec_CandidateLanguageLevelById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_CANDDLANLVBYID(:Ids, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_rec_sp_get_Rec_CandidateLanguageLevelById @Id";
                }
            }
        }

        #endregion

        #region Rec_WaitingInterviewList
        public static string hrm_rec_sp_get_WaitingInterviewList
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_WAITTINGINTERVIEW";
                }
                else
                {
                    return "exec hrm_rec_sp_get_WaitingInterviewList";
                }
            }
        }
        //[Tho.Bui]-Load InterviewList
        public static string hrm_rec_sp_get_WaitingInterList
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_WAITTINGINTERLIST";
                }
                else
                {
                    return "exec hrm_rec_sp_get_WaitingInterList";
                }
            }
        }
        #endregion



        #region Rec_CandidatesFailToGetTheJob
        public static string hrm_rec_sp_get_CandidatesFailToGetTheJob
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_CANDIDATESFAILJOB";
                }
                else
                {
                    return "exec hrm_rec_sp_get_CandidatesFailToGetTheJob";
                }
            }
        }
        public static string hrm_rec_sp_get_CanFailJobIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_CANFAILJOBIDS(:Ids, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_rec_sp_get_CanFailJobIds @Ids";
                }
            }
        }
        #endregion
        #region Rec_CandidateHistory
        public static string hrm_rec_sp_get_CandidateHistory
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_CANDIDATEHISTORY";
                }
                else
                {
                    return "exec hrm_rec_sp_get_CandidateHistory";
                }
            }
        }
        public static string hrm_rec_sp_get_CandidateHistory_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_CANDIDATEHISTORY_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_CandidateHistory_Multi @Keyword";
                }
            }
        }
        public static string hrm_rec_sp_get_CandidateHistoryByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_CANDIDATEHISTORYBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_rec_sp_get_CandidateHistoryByIds @ID";
                }
            }
        }
        public static string hrm_rec_sp_get_CandidateHistoryById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_REC_SP_GET_CANHISTORYBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_CandidateHistoryById @Id";
                }
            }
        }
        #endregion
        public static string hrm_cat_sp_get_NameEntityByType_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_NAMEENBYTYPE_MULTI(:Type,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_NameEntityByType_multi @Type";
                }
            }
        }

        public static string hrm_cat_sp_get_CutOffDurationType_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_CUTOFFTYPE_MULTI(:Type,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CutOffDurationType_Multi @Type";
                }
            }
        }
        /// <summary>
        ///[Tho.Bui] get candidatehistoryid by candidateid from Rec_Candidatehistory
        /// </summary>
        /// 
        public static string hrm_rec_sp_get_CandidateHistoryIdByCandidateId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_CANDIDATEHISTORYID(:Type,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_CandidateHistoryIdByCandidateId @Type";
                }
            }
        }
        /// <summary>
        /// [Tho.Bui] lay candidateid trong table Rec_RecruitmentHistoryEntity
        /// </summary>
        /// 
        public static string hrm_rec_sp_get_RecruitmentHistoryIdByCandidateId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_RECRUIHISTORYCANID(:Type,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_RecruitmentHistoryIdByCandidateId @Type";
                }
            }
        }

        #region Rec_JobCondition
        public static string hrm_rec_sp_get_JobConditionByVacancyId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_JOBCONDITIONBYVAID";
                }
                else
                {
                    return "exec hrm_rec_sp_get_JobConditionByVacancyId";
                }
            }
        }
        public static string hrm_rec_sp_get_JobCondition
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_JOBCONDITION";
                }
                else
                {
                    return "exec hrm_rec_sp_get_JobCondition";
                }
            }
        }

        public static string hrm_rec_sp_get_JobConditionById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_JOBCONDITIONBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_JobConditionById @Id";
                }
            }
        }
        public static string hrm_rec_sp_get_JobConditionById1
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_JOBCONDITIONBYID";
                }
                else
                {
                    return "hrm_rec_sp_get_JobConditionById";
                }
            }
        }
        public static string hrm_rec_sp_get_JobConditionByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_JOBCONDITIONBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_JobConditionByIds @Ids";
                }
            }
        }

        public static string hrm_rec_sp_get_JobConditionForInterView
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_JOBCONINTERVIEW";
                }
                else
                {
                    return "hrm_rec_sp_get_JobConditionForInterView";
                }
            }
        }
        #endregion

        #region InterviewCampaign

        public static string hrm_rec_sp_get_InterviewCampaign
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_INTCAMPAIGN";
                }
                else
                {
                    return "exec hrm_rec_sp_get_InterviewCampaign";
                }
            }
        }

        public static string hrm_rec_sp_get_InterviewCampaignById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_INTCAMPAIGNBYID(:ID, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_rec_sp_get_InterviewCampaignById @ID";
                }
            }
        }

        public static string hrm_rec_sp_get_InterviewCampaignByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_INTCAMPAIGNBYIDS(:Ids, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_rec_sp_get_InterviewCampaignByIds @Ids";
                }
            }
        }

        public static string hrm_rec_sp_get_InterviewCampaign_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_INTCAMPAIGNMULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_rec_sp_get_InterviewCampaign_multi @Keyword";
                }
            }
        }
        #endregion

        #region Rec_EnrolledCandidates
        public static string hrm_rec_sp_get_EnrolledCandidates
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_ENROLLEDCANDIDATES";
                }
                else
                {
                    return "exec hrm_rec_sp_get_EnrolledCandidates";
                }
            }
        }
        public static string hrm_rec_sp_get_EnrolledCanIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_ENROLLEDCANIDS(:Ids, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_rec_sp_get_EnrolledCanIds @Ids";
                }
            }
        }
        #endregion

        #region Rec_FailCandidate
        public static string hrm_rec_sp_get_FailCandidates
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_FAILCANDIDATES";
                }
                else
                {
                    return "exec hrm_rec_sp_get_FailCandidates";
                }
            }
        }
        #endregion
        #region Cat_HDTJobGroup

        public static string hrm_cat_sp_get_HDTJobGroup
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_HDTJOBGROUP";
                }
                else
                {
                    return "exec hrm_cat_sp_get_HDTJobGroup";
                }
            }
        }
        public static string hrm_cat_sp_get_HDTJobGroupByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_HDTJOBGROUPBYIDS(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_HDTJobGroupByIds @Id";
                }
            }
        }
        public static string hrm_cat_sp_get_HDTJobGroupById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_HDTJOBGROUPBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_HDTJobGroupById @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_HDTJobGroup_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_HDTJOBGROUP_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_HDTJobGroup_multi @Keyword";
                }
            }
        }
        public static string hrm_cat_sp_get_HDTJobGroupCode_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_HDTJOBGROUPC_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_HDTJobGroupCode_multi @Keyword";
                }
            }
        }
        #endregion

        #region Cat_HDTJobTypePrice
        public static string hrm_cat_sp_get_HDTJobTypePrice
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_HDTJOBTYPEPRICE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_HDTJobTypePrice";
                }
            }
        }

        public static string hrm_cat_sp_get_HDTJobTypePriceById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_HDTTYPEPRICEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_HDTJobTypePriceById @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_HDTJobTypePriceByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_HDTTYPEPRICEBYIDS(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_HDTJobTypePriceByIds @Ids";
                }
            }
        }
        #endregion

        public static string hrm_hr_sp_get_ApprovedPurchaseRequest
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_APRPURCHASEREQUEST";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ApprovedPurchaseRequest";
                }
            }
        }

        public static string hrm_hr_sp_get_TravelRequest_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_TRAVELREQUEST_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_TravelRequest_Multi @Keyword";
                }
            }
        }

        public static string hrm_hr_sp_get_CashAdvance_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_CASHADVANCE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_CashAdvance_Multi @Keyword";
                }
            }
        }

        public static string hrm_hr_sp_get_TravelRequest
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_TRAVELREQUEST";
                }
                else
                {
                    return "exec hrm_hr_sp_get_TravelRequest";
                }
            }
        }

        public static string hrm_hr_sp_get_HistoryApprovedClaim
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_HISTORYAPPORVED";
                }
                else
                {
                    return "exec hrm_hr_sp_get_HistoryApprovedClaim";
                }
            }
        }

        public static string hrm_hr_sp_get_TravelRequestById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_TRAVELREQUESTBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_TravelRequestById @Id";
                }
            }
        }

        public static string hrm_hr_sp_get_TravelRequestItemById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_TRAVELITEMBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_TravelRequestItemById @Id";
                }
            }
        }

        public static string hrm_hr_sp_get_TravelRequestItemByTravelRequestID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_TRAITEMBYTRAID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_TravelRequestItemByTravelRequestID";
                }
            }
        }

        public static string hrm_hr_sp_get_CashAdvanceItemByCashAdvanceID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_CASHITEMBYCASHID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_CashAdvanceItemByCashAdvanceID";
                }
            }
        }

        public static string hrm_hr_sp_get_OrgMoreInfoByOrgID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_ORGINFOBYODGID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_OrgMoreInfoByOrgID";
                }
            }
        }


        public static string hrm_hr_sp_get_ClaimByTravelRequestID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_HR_SP_GET_CLAIMBYTRAVELREQUESTID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ClaimByTravelRequestID";
                }
            }
        }
        public static string hrm_hr_sp_get_Claim
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_HR_SP_GET_CLAIM";
                }
                else
                {
                    return "exec hrm_hr_sp_get_Claim";
                }
            }
        }
        public static string hrm_hr_sp_get_CashAdvance
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_HR_SP_GET_CASHADVANCE";
                }
                else
                {
                    return "exec hrm_hr_sp_get_CashAdvance";
                }
            }
        }

        public static string hrm_hr_sp_get_ClaimItemByClaimID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_HR_SP_GET_CLAIMITEMBYCLAIMID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ClaimItemByClaimID";
                }
            }
        }





        public static string hrm_hr_sp_get_ClaimById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_HR_SP_GET_CLAIMBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ClaimById @Id";
                }
            }
        }

        public static string hrm_hr_sp_get_CashAdvanceById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_HR_SP_GET_CASHADVANCEBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_CashAdvanceById @Id";
                }
            }
        }

        public static string hrm_hr_sp_get_CashAdvanceItemById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_HR_SP_GET_CASHADVITEMBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_CashAdvanceItemById @Id";
                }
            }
        }

        public static string hrm_hr_sp_get_ClaimItemById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_HR_SP_GET_CLAIMITEMBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ClaimItemById @Id";
                }
            }
        }

        public static string hrm_hr_sp_get_ApprovedFIN_TravelRequest
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_APRTRAVELREQ";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ApprovedFIN_TravelRequest";
                }
            }
        }

        public static string hrm_hr_sp_get_ApprovedClaim
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_APRCLAIM";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ApprovedClaim";
                }
            }
        }

        public static string hrm_rec_sp_checkduplidatecandidate
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_CHECKDUPLIDATECANDIDATE";
                }
                else
                {
                    return "exec hrm_rec_sp_checkduplidatecandidate";
                }
            }
        }

        public static string hrm_rec_sp_checkduplidatecandidateHistory
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_CHECKDUPCANHISTORY";
                }
                else
                {
                    return "exec hrm_rec_sp_checkduplidatecandidateHistory";
                }
            }
        }

        public static string hrm_rec_sp_checkduplidaterecruimentHistory
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_CHECKDUPRECHISTORY";
                }
                else
                {
                    return "exec hrm_rec_sp_checkduplidaterecruimentHistory";
                }
            }
        }
        #region Cat_Skill
        public static string hrm_cat_sp_get_Skill_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SKILL_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Skill_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_SkillById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SKILLBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SkillById @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_SkillByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SKILLBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SkillByIds @ID";
                }
            }
        }
        public static string hrm_cat_sp_get_Skill
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_SKILL";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Skill";
                }
            }
        }
        #endregion
        #region Cat_SkillTopic
        public static string hrm_cat_sp_get_SkillTopicBySkillId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_SKILLTOPICBYSKILLID";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SkillTopicBySkillId";
                }
            }
        }
        public static string hrm_cat_sp_get_SkillTopic_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SKILLTOPIC_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SkillTopic_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_SkillTopicById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SKILLTOPICBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SkillTopicById @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_SkillTopicByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SKILLTOPICBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SkillTopicByIds @ID";
                }
            }
        }
        public static string hrm_cat_sp_get_SkillTopic
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_SKILLTOPIC";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SkillTopic";
                }
            }
        }
        #endregion


        #region Cat_Topic
        public static string hrm_cat_sp_get_TopicById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_TOPICBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TopicById @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_Topic
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_CAT_SP_GET_TOPIC";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Topic";
                }
            }
        }

        public static string hrm_cat_sp_get_Topic_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_CAT_SP_GET_TOPIC_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Topic_Multi";
                }
            }
        }
        #endregion
        #region PlanName
        public static string hrm_tra_sp_get_PlanName_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_PLAN_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_PlanName_Multi";
                }
            }
        }

        #endregion

        #region Tra_RequirementTrain
        public static string hrm_tra_sp_get_RequirementTrain
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_REQUIREMENTTRAIN";
                }
                else
                {
                    return "exec hrm_tra_sp_get_RequirementTrain";
                }
            }
        }

        public static string hrm_tra_sp_get_RequirementTrain_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_ReQuReMnTrain_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_TRA_SP_GET_ReQuReMnTrain_MULTI @Keyword";
                }
            }
        }

        public static string hrm_tra_sp_get_RequirementDetail
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_REQUIREMENDETAIL";
                }
                else
                {
                    return "exec hrm_tra_sp_get_RequirementDetail";
                }
            }
        }

        public static string hrm_tra_sp_get_RequirementTrainIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_REQMENTRAINBYIDS(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_RequirementTrainIds @Keyword";
                }
            }
        }

        public static string hrm_tra_sp_get_RequirementTrainInSide
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_REQUITRAININSIDE";
                }
                else
                {
                    return "exec hrm_tra_sp_get_RequirementTrainInSide";
                }
            }
        }
        #endregion


        #region Tra_RequirementTrainDetail
        public static string hrm_tra_sp_get_RequirementTrainDetailIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_REQMENTRAINDTBYIDS(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_RequirementTrainDetailIds @Keyword";
                }
            }
        }
        public static string hrm_tra_sp_get_RequirementTrainDetailByRMTDTID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_RQTRAINDTBYRMDTID";
                }
                else
                {
                    return "exec hrm_tra_sp_get_RequirementTrainDetailByRMTDTID";
                }
            }
        }

        public static string hrm_tra_sp_get_TopicByCourseID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_TOPICBYCOURSEID";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TopicByCourseID";
                }
            }
        }

        #endregion

        #region Tra_PlanInside
        public static string hrm_tra_sp_get_PlanInside
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_PLANINSIDE";
                }
                else
                {
                    return "exec hrm_tra_sp_get_PlanInside";
                }
            }
        }


        #endregion
        #region Tra_PlanOutside
        public static string hrm_tra_sp_get_PlanOutside
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_PLANOUTSIDE";
                }
                else
                {
                    return "exec hrm_tra_sp_get_PlanOutside";
                }
            }
        }
        #endregion

        #region Tra_Plan

        public static string hrm_tra_sp_get_Plan
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_PLAN";
                }
                else
                {
                    return "exec hrm_tra_sp_get_Plan";
                }
            }
        }


        public static string hrm_tra_sp_get_PlanIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_PLANBYIDS(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_PlanIds @Keyword";
                }
            }
        }
        public static string hrm_Cat_SP_GET_ColorByModelID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "Cat_SP_GET_ColorByModelID";
                }
                else
                {
                    return "exec hrm_Cat_SP_GET_ColorByModelID";
                }
            }
        }
        public static string hrm_tra_sp_get_ClassByPlanId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_CLASSBYPLANID";
                }
                else
                {
                    return "exec hrm_tra_sp_get_ClassByPlanId";
                }
            }
        }
        public static string hrm_tra_sp_get_PlanDetailByPlanId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_PLANDETAILYPLANID";
                }
                else
                {
                    return "exec hrm_tra_sp_get_PlanDetailByPlanId";
                }
            }
        }
        public static string hrm_tra_sp_get_RmTrainByPlanId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_RMTRAINBYPLANID";
                }
                else
                {
                    return "exec hrm_tra_sp_get_RmTrainByPlanId";
                }
            }
        }
        #endregion



        #region Tra_PlanDetail
        public static string hrm_tra_sp_get_PlanDetail
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_PLANDETAIL";
                }
                else
                {
                    return "exec hrm_tra_sp_get_PlanDetail";
                }
            }
        }

        public static string hrm_tra_sp_get_PlanDetailById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_PLANDETAILBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_PlanDetailById @Keyword";
                }
            }
        }
        #endregion

        #region Tra_Certificate
        public static string hrm_tra_sp_get_CertificateById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_CERTIFICATEBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_CertificateById @Keyword";
                }
            }
        }
        public static string hrm_tra_sp_get_TraineeCertificateById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_TRAINEECERTIFIBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineeCertificateById @Keyword";
                }
            }
        }

        public static string hrm_tra_sp_get_Certificate
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_TRA_SP_GET_CERTIFICATE";
                }
                else
                {
                    return "exec hrm_tra_sp_get_Certificate";
                }
            }
        }

        public static string hrm_tra_sp_get_Certificate_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_CERTIFICATE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_Certificate_Multi";
                }
            }
        }
        #endregion

        public static string hrm_tra_sp_get_CostCenter_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_COSTCENTER_MUTIL(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_CostCenter_Multi @Keyword";
                }
            }

        }

        #region Tra_Course
        public static string hrm_tra_sp_get_Course_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_COURSE_MUTIL(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_Course_Multi @Keyword";
                }
            }

        }

        public static string hrm_tra_sp_get_CourseByCode_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_COURSEBYCODE_MUTIL(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_tra_sp_get_CourseByCode_Multi @ID";
                }
            }
        }


        public static string hrm_tra_sp_get_Course
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_COURSE";
                }
                else
                {
                    return "exec hrm_tra_sp_get_Course";
                }
            }
        }
        public static string hrm_tra_sp_get_ProfileByClassId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_PROFILEBYCLASSID";
                }
                else
                {
                    return "exec hrm_tra_sp_get_ProfileByClassId";
                }
            }
        }
        public static string hrm_tra_sp_get_ClassByClassId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_CLASSBYCLASSID";
                }
                else
                {
                    return "exec hrm_tra_sp_get_ClassByClassId";
                }
            }
        }
        public static string hrm_tra_sp_get_CourseById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_COURSEBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_CourseById @Keyword";
                }
            }
        }
        public static string hrm_tra_sp_get_CourseByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_COURSEBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_tra_sp_get_CourseByIds @ID";
                }
            }
        }
        public static string hrm_cat_sp_get_OrgByOrderNumbers
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ORGBYORDERNUMBERS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OrgByOrderNumbers @ID";
                }
            }
        }

        #endregion

        #region Tra_RankingGroup
        public static string hrm_tra_sp_get_RankingGroup_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_RANKINGGROUP_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_RankingGroup_Multi @Keyword";
                }
            }

        }
        #endregion

        #region Tra_ScoreType
        public static string hrm_tra_sp_get_ScoreType_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_SCORETYPE_MUTIL(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_ScoreType_Multi @Keyword";
                }
            }

        }

        public static string hrm_tra_sp_get_ScoreType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_SCORETYPE";
                }
                else
                {
                    return "exec hrm_tra_sp_get_ScoreType";
                }
            }
        }

        public static string hrm_tra_sp_get_ScoreTypeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_SCORETYPEBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_ScoreTypeById @Keyword";
                }
            }
        }

        public static string hrm_tra_sp_get_ScoreTypeByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_SCORETYPEBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_tra_sp_get_ScoreTypeByIds @ID";
                }
            }
        }
        #endregion

        #region Tra_TrainerMailReminder
        public static string hrm_tra_sp_get_TrainerMailReminderById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_TRAMAILREBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TrainerMailReminderById @Keyword";
                }
            }
        }

        public static string hrm_tra_sp_get_TrainerMailReminder
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_TRAINERMAILREMINDER";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TrainerMailReminder";
                }
            }
        }
        #endregion

        #region Tra_Trainee
        public static string hrm_tra_sp_get_TraineeById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_TRAINEEBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineeById @Keyword";
                }
            }
        }

        public static string hrm_tra_sp_get_TraineeByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_TRAINEEBYIDS(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineeByIds @Keyword";
                }
            }
        }

        public static string hrm_tra_sp_get_Trainee
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_TRAINEE";
                }
                else
                {
                    return "exec hrm_tra_sp_get_Trainee";
                }
            }
        }

        public static string hrm_tra_sp_get_TraineeRegister
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_TRAINEEREGISTER";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineeRegister";
                }
            }
        }

        public static string hrm_tra_sp_get_TraineeForAdd
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_TRAINEEFORADD";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineeForAdd";
                }
            }
        }

        public static string hrm_tra_sp_get_TraineeFinish
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_TRAINEEFINISH";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineeFinish";
                }
            }
        }

        #region Tra_TraineeFinishInside
        public static string hrm_tra_sp_get_TraineeFinishInside
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_TRAINEEFINISHINSIDE";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineeFinishInside";
                }
            }
        }
        public static string hrm_tra_sp_get_TraineeFinishInsideIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_TRAINEEFININBYIDS(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineeFinishInsideIds @Keyword";
                }
            }
        }


        #endregion

        public static string hrm_tra_sp_get_TraineeProgress
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_TRAINEEPROGRESS";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineeProgress";
                }
            }
        }

        public static string hrm_tra_sp_get_TraineePass
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_TRAINEEPASS";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineePass";
                }
            }
        }

        public static string hrm_tra_sp_get_TraineeFail
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_TRAINEEFAIL";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineeFail";
                }
            }
        }

        public static string hrm_tra_sp_get_TraineeWithOutProcess
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_TRAWITHOUTPROCESS";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineeWithOutProcess";
                }
            }
        }

        public static string hrm_tra_sp_get_TraineeExpiredCertificate
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_TRAEXPIREDCER";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineeExpiredCertificate";
                }
            }
        }
        public static string hrm_tra_sp_get_TraineeCertificate
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_TRAINEECERTIFICATE";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineeCertificate";
                }
            }
        }
        #endregion

        #region Tra_Class

        public static string hrm_tra_sp_get_ClassByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_CLASSBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_tra_sp_get_ClassByIds @ID";
                }
            }
        }
        public static string hrm_tra_sp_get_Class_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_CLASS_MUTIL(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_Class_Multi @Keyword";
                }
            }

        }

        public static string hrm_tra_sp_get_Class
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_CLASS";
                }
                else
                {
                    return "exec hrm_tra_sp_get_Class";
                }
            }
        }
        public static string hrm_tra_sp_get_CourseTopic
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_COURSETOPIC";
                }
                else
                {
                    return "exec hrm_tra_sp_get_CourseTopic";
                }
            }
        }
        public static string hrm_tra_sp_get_ScoreTopic
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_SCORETOPIC";
                }
                else
                {
                    return "exec hrm_tra_sp_get_ScoreTopic";
                }
            }
        }
        public static string hrm_tra_sp_get_ReportTraineeResult
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_REPORTTRAINEERESULT";
                }
                else
                {
                    return "exec hrm_tra_sp_get_ReportTraineeResult";
                }
            }
        }


        public static string hrm_tra_sp_get_TraineeTopic
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_TRAINEETOPIC";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineeTopic";
                }
            }
        }

        public static string hrm_tra_sp_get_ClassInSide
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_CLASSINSIDE";
                }
                else
                {
                    return "exec hrm_tra_sp_get_ClassInSide";
                }
            }
        }
        public static string hrm_tra_sp_get_ClassInSideByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_CLASSINSIDEBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_tra_sp_get_ClassInSideByIds @ID";
                }
            }
        }

        public static string hrm_tra_sp_get_ClassFinish
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_CLASSFINISH";
                }
                else
                {
                    return "exec hrm_tra_sp_get_ClassFinish";
                }
            }
        }
        public static string hrm_tra_sp_get_ClassFinishByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_CLASSFINISHBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_tra_sp_get_ClassFinishByIds @ID";
                }
            }
        }

        public static string hrm_tra_sp_get_ClassProcess
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_CLASSPROCESS";
                }
                else
                {
                    return "exec hrm_tra_sp_get_ClassProcess";
                }
            }
        }
        public static string hrm_tra_sp_get_ClassProcessByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_CLASSPROCESSBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_tra_sp_get_ClassProcessByIds @ID";
                }
            }
        }


        public static string hrm_tra_sp_get_ClassProcessInSide
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_CLASSPROCESSINSIDE";
                }
                else
                {
                    return "exec hrm_tra_sp_get_ClassProcessInSide";
                }
            }
        }
        public static string hrm_tra_sp_get_ClassProcessInSideByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_CLASSPRINSIDEBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_tra_sp_get_ClassProcessInSideByIds @ID";
                }
            }
        }

        public static string hrm_tra_sp_get_ClassById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_CLASSBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_ClassById @Keyword";
                }
            }

        }
        public static string hrm_cat_sp_get_TraineeByClassID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_TRAINEEBYCLASSID";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TraineeByClassID";
                }
            }
        }

        #endregion

        #region Tra_CourseTopic
        public static string hrm_cat_sp_get_CourseTopicByCourseID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_TOPICBYCOURCEID";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CourseTopicByCourseID";
                }
            }
        }
        #endregion

        #region Rec_GroupCondition

        public static string hrm_tra_sp_get_GroupCondition
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_GROUPCONDITION";
                }
                else
                {
                    return "exec hrm_tra_sp_get_GroupCondition";
                }
            }
        }

        public static string hrm_tra_sp_get_GroupConditionById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_GROUPCONDITIONBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_GroupConditionById @Keyword";
                }
            }
        }

        public static string hrm_rec_sp_get_JobConditionByGroupConditionId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_JOBCONBYGROUID";
                }
                else
                {
                    return "exec hrm_rec_sp_get_JobConditionByGroupConditionId";
                }
            }
        }

        public static string hrm_cat_sp_get_GroupCondition_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_GROUPCON_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_GroupCondition_Multi";
                }
            }
        }
        #endregion


        public static string hrm_hr_sp_get_CandidateGeneral
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_HR_SP_GET_CANDIDATEGENERAL";
                }
                else
                {
                    return "exec hrm_hr_sp_get_CandidateGeneral";
                }
            }
        }

        public static string hrm_hr_sp_get_GeneralCandidateByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_GENERALCANBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_GeneralCandidateByIds @Ids";
                }
            }
        }

        public static string hrm_rec_sp_get_GroupConditionByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin REC_SP_GET_GROUPCONDITIONBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_rec_sp_get_GroupConditionByIds @ID";
                }
            }
        }

        public static string hrm_rec_sp_get_GroupConditions
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_GROUPCONDITIONS";
                }
                else
                {
                    return "exec hrm_rec_sp_get_GroupCondition";
                }
            }
        }

        public static string hrm_kai_sp_get_CategoryById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin KAI_SP_GET_CATEGORYBYID (:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_kai_sp_get_CategoryById @Id";
                }
            }
        }

        public static string hrm_Kai_sp_get_Category
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "KAI_SP_GET_CATEGORY";
                }
                else
                {
                    return "exec hrm_kai_sp_get_Category";
                }
            }
        }
        public static string hrm_Kai_sp_get_CategoryByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin KAI_SP_GET_CATEGORYBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_Kai_sp_get_CategoryByIds @ID";
                }
            }
        }

        public static string hrm_kai_sp_get_KaiZenDataById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin KAI_SP_GET_KAIZENDATABYID (:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_kai_sp_get_KaiZenDataById @Id";
                }
            }
        }

        public static string hrm_cat_sp_get_Category_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin KAI_SP_GET_CATEGORY_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Category_multi @Keyword";
                }
            }
        }

        public static string hrm_Kai_sp_get_KaiZenData
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "KAI_SP_GET_KAIZENDATA";
                }
                else
                {
                    return "exec hrm_kai_sp_get_KaiZenData";
                }
            }
        }

        public static string hrm_Kai_sp_get_KaiZenDataByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin KAI_SP_GET_KAIZENDATABYIDS(:Id, :R_Output); end;  ";
                }
                else
                {
                    return "Chưa Viết";
                }
            }
        }

        public static string hrm_Cat_sp_get_Bank
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRM_CAT_SP_GET_BANK";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Bank";
                }
            }
        }

        public static string hrm_Cat_sp_get_BankByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_BANKBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_BankByIds @ID";
                }
            }
        }

        public static string hrm_cat_sp_CatagoryByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_CATEGORYBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CategoryByIds @Ids";
                }
            }
        }

        public static string hrm_hr_sp_get_RecHisByCandidateIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_RECHISBYCANIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RecHisByCandidateIds @Ids";
                }
            }
        }

        public static string hrm_hr_sp_get_InterViewByCandidateIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_INTERVIEWBYCANIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_InterViewByCandidateIds @Ids";
                }
            }
        }

        public static string hrm_tra_sp_get_TraineeScoreById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_TRAINEESCOREBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineeScoreById @Id";
                }
            }
        }

        public static string hrm_tra_sp_get_TraineeRegisterById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_TRAINEEREGISTERBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineeRegisterById @Id";
                }
            }
        }

        // tính điểm học viên
        public static string hrm_tra_sp_get_TraineeScore
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_TRAINEESCORE";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineeScore";
                }
            }
        }

        // Lấy dữ liệu ds điểm học viên
        public static string hrm_tra_sp_get_TraineeScoreData
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_TRAINEESCOREDATA";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineeScoreData";
                }
            }
        }
        #region Eva_EvalutionData
        public static string hrm_att_getdata_AttendanceTable
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_GETDATA_ATTTABLE";
                }
                else
                {
                    return "exec hrm_att_getdata_AttendanceTable";
                }
            }
        }
        public static string hrm_cat_getdata_LeaveDayType
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_GETDATA_LEADAYTYPE";
                }
                else
                {
                    return "exec hrm_cat_getdata_LeaveDayType";
                }
            }
        }
        public static string hrm_hre_getdata_discipline
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HRE_GETDATA_DISCIPLINE";
                }
                else
                {
                    return "exec hrm_hre_getdata_discipline";
                }
            }
        }
        public static string hrm_kai_getdata_kaizendata
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "KAI_GETDATA_KAIZENDATA";
                }
                else
                {
                    return "exec hrm_kai_getdata_kaizendata";
                }
            }
        }
        public static string hrm_eva_getdata_EvalutionData
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_GETDATA_EVALUTIONDATA";
                }
                else
                {
                    return "exec hrm_eva_getdata_EvalutionData";
                }
            }
        }
        public static string hrm_eva_sp_get_EvalutionData
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GET_EVALUTIONDATA";
                }
                else
                {
                    return "exec hrm_eva_sp_get_EvalutionData";
                }
            }
        }
        public static string hrm_vnr_DeleteEvalutionData
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "VNR_DELETEEVALUTIONDATA";
                }
                else
                {
                    return "exec hrm_vnr_DeleteEvalutionData";
                }
            }
        }

        public static string hrm_vnr_RemoveEvalutionData
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "VNR_REMOVEVALUTIONDATA";
                }
                else
                {
                    return "exec hrm_vnr_RemoveEvalutionData";
                }
            }
        }
        public static string hrm_vnr_RemoveDataIsDelete
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "VNR_REMOVEDATAISDELETE";
                }
                else
                {
                    return "exec VNR_REMOVEDATAISDELETE";
                }
            }
        }


        public static string hrm_eva_sp_getdata_ProfileByOrg
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "EVA_SP_GETDATA_PROFILEBYORG";
                }
                else
                {
                    return "exec hrm_eva_sp_getdata_ProfileByOrg";
                }
            }
        }

        #endregion

        #region Tra_TraineeCertificate
        public static string hrm_tra_sp_get_TraineeCertificateList
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "TRA_SP_GET_TRAINEECERTIFICATEL";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineeCertificateList";
                }
            }
        }
        #endregion

        public static string hrm_cat_sp_YouthUnionPositionByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_YUPOSITIONBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_YouthUnionPositionByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_CommunistPartyPositionByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_CPPOSITIONBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CommunistPartyPositionByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_WoundedSoldierTypesByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_WSTYPEBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_WoundedSoldierTypesByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_PoliticalLevelByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_POLITICALLEVELBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_PoliticalLevelByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_ArmedForceTypesByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_AFTYPESBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ArmedForceTypesByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_TradeUnionistPositionByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_TUPOSITIONBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TradeUnionistPositionByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_SelfDefenceMilitiaPositionByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SDMPOSITIONBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SelfDefenceMilitiaPositionByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_VeteranUnionPositionByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_VUPOSITIONBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_VeteranUnionPositionByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_RegionByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_REGIONBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_RegionByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_CountryByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_COUNTRYBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CountryByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_ProvinceByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_PROVINCEBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ProvinceByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_DistrictByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_DISTRICBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_DistrictByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_ReligionByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_RELIGIONBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ReligionByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_CurrencyByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_CURRENCYBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CurrencyByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_EthnicGroupsByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ETHRNICGROUPBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_EthnicGroupByIds @Ids";
                }
            }
        }

        public static string hrm_hr_sp_get_TypeOfTransferByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_TYPEOFTRANFERBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_TypeOfTransferByIds @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_ScoreTopicByTopicID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_SCORETOPICBYTOID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ScoreTopicByTopicID @Id";
                }
            }
        }
        public static string hrm_hr_sp_get_RptRelatives
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTRELATIVES";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RptRelatives";
                }
            }
        }

        public static string hrm_sal_sp_get_RptKaizenDataDetail
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_RPTKAIZENDATADETAIL";
                }
                else
                {
                    return "exec hrm_sal_sp_get_RptKaizenDataDetail";
                }
            }
        }

        public static string hrm_sal_sp_get_RptKaizenDataAccumulate
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GET_RPTKAIZENACCUMULATE";
                }
                else
                {
                    return "exec hrm_sal_sp_get_RptKaizenDataAccumulate";
                }
            }
        }

        public static string hrm_kai_sp_get_KaiZenDataByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin KAI_GET_KAIZENDATABYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_kai_sp_get_KaiZenDataByIds @Ids";
                }
            }
        }

        public static string hrm_Cat_sp_get_TraineeByClassIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_TRAINEEBYCLASSIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_Cat_sp_get_TraineeByClassIds @ID";
                }
            }

        }

        public static string hrm_sal_sp_get_BasicSalary_UpdateStatus
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_UPDATE_BASICSALARY_STT";
                }
                else
                {
                    return "exec hrm_sal_sp_get_BasicSalary_UpdateStatus";
                }
            }
        }


        public static string hrm_cat_sp_get_TraineeScoreByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_TRAINEESCOREBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_TraineeScoreByIds @ID";
                }
            }
        }

        public static string hrm_tra_sp_get_TraineeCertificateByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin TRA_SP_GET_TRAINEECTFCBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_tra_sp_get_TraineeCertificateByIds @ID";
                }
            }
        }

        public static string hrm_cat_sp_get_InsuranceConfigById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_INSURANCECONFIGBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_InsuranceConfigById @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_ReqDocument
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_REQDOCUMENT";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ReqDocument";
                }
            }
        }


        public static string hrm_cat_sp_get_ReqDocumentByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_REPDOCUMENTBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ReqDocumentByIds @ID";
                }
            }
        }

        public static string hrm_cat_sp_get_ReqDocumentById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_REPDOCUMENTBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_ReqDocumentById @Keyword";
                }
            }
        }

        public static string hrm_hr_sp_get_ReqDocument_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_REPDOCUMENT_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ReqDocument_multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_DataGroup_multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_DATAGROUP_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_DataGroup_multi @Keyword";
                }
            }
        }

        public static string hrm_can_sp_get_SumMealRecord
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAN_SP_GET_SUMRYMEALRECORD";
                }
                else
                {
                    return "exec hrm_cat_sp_get_SumryMealRecord";
                }
            }
        }

        #region Sal_ReportProfile
        public static string hrm_sal_sp_getdata_Profile
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GETDATA_PROFILE";
                }
                else
                {
                    return "exec hrm_sal_sp_getdata_Profile";
                }
            }
        }
        public static string hrm_sal_sp_getdata_BasicSalaryByCutOff
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "SAL_SP_GETDATA_BSSALARYBYCUTF";
                }
                else
                {
                    return "exec hrm_sal_sp_getdata_BasicSalaryByCutOff";
                }
            }
        }



        #endregion

        public static string hrm_cat_sp_get_GetMultiCategoryType_munti
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin KAI_SP_GET_CATEGORYTYPE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_GetMultiCategoryType_munti @Keyword";
                }
            }
        }

        #region Rec_ReportInterview
        public static string hrm_rec_sp_get_ReportInterview
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_RPTINTERVIEW";
                }
                else
                {
                    return "exec hrm_rec_sp_get_ReportInterview";
                }
            }
        }
        #endregion

        // Chưa viết SQL
        public static string hrm_att_sp_get_PersonalSubmitOvertime
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_PERSONALOVERTIME";
                }
                else
                {
                    return "exec hrm_att_sp_get_PersonalSubmitOvertime";
                }
            }
        }

        // Chưa viết SQL
        public static string hrm_att_sp_get_PersonalSubmitLeaveDay
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_PERSONALLEAVEDAY";
                }
                else
                {
                    return "exec hrm_att_sp_get_PersonalSubmitLeaveDay";
                }
            }
        }

        public static string hrm_can_get_MealRecordByProfileIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAN_GET_MEALRECORDBYPROFILEIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_can_get_MealRecordByProfileIds @Ids";
                }
            }
        }


        public static string hrm_hr_sp_get_RptRecieveObjectByTime
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTOBJECTBYTIME";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RptRecieveObjectByTime";
                }
            }
        }

        public static string hrm_sal_sp_get_BasicSalaryByProfileIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_BASICSALBYPROIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_BasicSalaryByProfileIds @Ids";
                }
            }
        }


        public static string hrm_sal_sp_get_InsuranceSalaryByProfileIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_INSSALBYPROIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_InsuranceSalaryByProfileIds @Ids";
                }
            }
        }

        public static string hrm_hr_sp_get_WorkHistoryByProfileIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_WORKHISBYPROIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_WorkHistoryByProfileIds @Ids";
                }
            }
        }

        public static string hrm_sal_sp_get_Sal_GradeByProfileIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_SAL_GRADEBYPROIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_Sal_GradeByProfileIds @Ids";
                }
            }
        }

        public static string hrm_sal_sp_get_Att_GradeByProfileIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin SAL_SP_GET_ATT_GRADEBYPROIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_sal_sp_get_Att_GradeByProfileIds @Ids";
                }
            }
        }

        public static string hrm_hr_sp_get_CandidateGeneralByProfileIDs
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_CANGENERBYPROIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_CandidateGeneralByProfileIDs @Ids";
                }
            }
        }

        public static string hrm_hr_sp_get_CandidateGeneralByProfileID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_CANGENERBYPROID(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_CandidateGeneralByProfileID @Ids";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfilebyOrgStructureID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PROFILEBYORGID(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfilebyOrgStructureID @Ids";
                }
            }
        }

        public static string hrm_hr_sp_get_PlanHeadCountbyOrgStructureID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PLANHEADCOUNTBYORGID(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_PlanHeadCountbyOrgStructureID @Ids";
                }
            }
        }

        public static string hrm_cat_sp_get_OrgMoreInforContractByOrgID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ORGMORECONBYORGID(:ID,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_OrgMoreInforContractByOrgID @ID";
                }
            }
        }

        public static string hrm_hr_sp_set_ApprovedAllContract
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_SET_APPROVEDALLCONTRACT";
                }
                else
                {
                    return "exec hrm_hr_sp_set_ApprovedAllContract";
                }
            }
        }

        public static string hrm_cat_sp_get_CostSource_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_COSTSOURCE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_CostSource_Multi @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_Vehicle_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_VEHICLE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_Vehicle_Multi @Keyword";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileByCandidateID
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PROFILEBYCANDIDATEID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileByCandidateID @Id";
                }
            }
        }
        #region Att_CompensationConfig
        public static string hrm_att_sp_get_CompensationConfigById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_COMPENSATIONCFBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_att_sp_get_CompensationConfigById @Id";
                }
            }
        }

        public static string hrm_att_sp_get_CompensationConfigByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin ATT_SP_GET_COMPENSATIONCFBYIDS(:Id, :R_Output); end; ";
                }
                else
                {
                    return "exec hrm_att_sp_get_CompensationConfigByIds @ID";
                }
            }
        }
        public static string hrm_att_sp_get_CompensationConfig
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "ATT_SP_GET_COMPENSATIONCF";
                }
                else
                {
                    return "exec hrm_att_sp_get_CompensationConfig";
                }
            }
        }
        #endregion


        public static string hrm_hr_sp_get_ReportDependantProfileQuit
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RPTDEPENDANTPROQUIT";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ReportDependantProfileQuit";
                }
            }
        }

        public static string hrm_hr_sp_get_MPById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_HR_SP_GET_MPBYID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_MPById @Id";
                }
            }
        }

        public static string hrm_hr_sp_get_MPByProfileId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HRM_HR_SP_GET_MPBYPROFILEID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_MPByProfileId @Id";
                }
            }
        }

        public static string hrm_hr_sp_get_EvaluationContractWaitingApprove
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_EVACONTRACTWAIAPP";
                }
                else
                {
                    return "exec hrm_hr_sp_get_EvaluationContractWaitingApprove";
                }
            }
        }

        public static string hrm_rec_sp_get_InterviewCampaignDetailByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_INCAMDETAILBYIDS";
                }
                else
                {
                    return "exec hrm_rec_sp_get_InterviewCampaignDetailByIds";
                }
            }
        }

        #region Hre_ContractExtend
        public static string hrm_hr_sp_get_ContractExtendList
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_CONTRACTEXTENDLIST";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ContractExtendList";
                }
            }
        }

        public static string hrm_hr_sp_get_ContractExtendByListId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_CONEXTENDBYLISTID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ContractExtendByListId";
                }
            }
        }
        #endregion
        public static string hrm_hr_sp_get_RecCandidateHistory
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_RECCANDIDATEHISTORY";
                }
                else
                {
                    return "exec hrm_hr_sp_get_RecCandidateHistory";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileCandidateHistory
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILECANHISTORY";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileCandidateHistory";
                }
            }
        }

        public static string hrm_hr_sp_get_CandidateHistoryByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_CANHISTORYBYIDS";
                }
                else
                {
                    return "exec hrm_hr_sp_get_CandidateHistoryByIds";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileReportHDTByIds
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin HR_SP_GET_PROREPORTHDTBYIDS(:Ids,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileReportHDTByIds @Ids";
                }
            }
        }

        public static string hrm_hr_sp_get_HDTJobReport
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_HDTJOBREPORT";
                }
                else
                {
                    return "exec hrm_hr_sp_get_HDTJobReport";
                }
            }
        }

        #region Cat_AbilityTitle
        public static string hrm_cat_sp_get_AbilityTileById
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ABILITYTILEBYID(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AbilityTileById @Keyword";
                }
            }
        }

        public static string hrm_cat_sp_get_AbilityTile
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "CAT_SP_GET_ABILITYTILE";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AbilityTile";
                }
            }
        }

        public static string hrm_cat_sp_get_AbilityTile_Multi
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ABILITYTILE_MULTI(:Keyword,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AbilityTile_Multi @Keyword";
                }
            }
        }
        #endregion

        public static string hrm_cat_sp_get_AbilityTileBySalaryClassId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "begin CAT_SP_GET_ABITILEBYSALCLASID(:Id,:R_Output); end;";
                }
                else
                {
                    return "exec hrm_cat_sp_get_AbilityTileBySalaryClassId @Id";
                }
            }
        }

        public static string hrm_hr_sp_get_ProfileRetirementByListId
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "HR_SP_GET_PROFILERETIMBYLISTID";
                }
                else
                {
                    return "exec hrm_hr_sp_get_ProfileRetirementByListId";
                }
            }
        }

        public static string hrm_rec_sp_get_InterviewDataReport
        {
            get
            {
                if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
                {
                    return "REC_SP_GET_INTERVIEWDATAREPORT";
                }
                else
                {
                    return "exec hrm_rec_sp_get_InterviewDataReport";
                }
            }
        }
    }
}

