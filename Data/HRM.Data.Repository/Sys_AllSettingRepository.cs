using System.Linq;
using Devart.Data.Oracle;
using HRM.Business.HrmSystem.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System.Data.SqlClient;
using System.Collections.Generic;
namespace HRM.Data.Entity.Repositories
{
    public class Sys_AllSettingRepository : CustomBaseRepository<Sys_AllSetting>
    {
        public Sys_AllSettingRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        //public IQueryable<Sys_AllSetting> GetAll()
        //{
        //    IQueryable<Sys_AllSetting> result = this.DbSet.ToList().AsQueryable();
        //    return result;
        //}

        /// <summary>
        /// Lấy đối tượng overtime bởi store hrm_att_sp_get_OvertimeByProfileId [Tam.Le - 2014/05/19]
        /// </summary>
        /// <returns></returns>
        public string GetSubMenu(Sys_AllSettingEntity model)
        {
            var result = this.UnitOfWork.Context.Database.SqlQuery<Sys_AllSettingEntity>(ConstantSql.hrm_sys_sp_get_UserSetting_Submenu,
                    new SqlParameter
                    {
                        ParameterName = "Name",
                        Value = model.Name
                    },
                    new SqlParameter
                    {
                        ParameterName = "Keyword",
                        Value = model.Value1
                    },
                    new SqlParameter
                    {
                        ParameterName = "UserID",
                        Value = model.UserID
                    }).FirstOrDefault();
            if (result == null)
            {
                return "";
            }
            else
            {
                return result.Value2;
            }

        }

        public List<Sys_AllSettingEntity> GetAllSettings(string ValueSelected)
        {
            if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
            {
                return this.UnitOfWork.Context.Database.SqlQuery<Sys_AllSettingEntity>(ConstantSql.hrm_cat_sp_get_GetAllSettings,
                    new OracleParameter()
                    {
                        ParameterName = "ValueSelected",
                        Value = ValueSelected,
                        OracleDbType = OracleDbType.NVarChar,
                        DbType = System.Data.DbType.String
                    },
                    new OracleParameter()
                    {
                        ParameterName = "R_Output",
                        OracleDbType = OracleDbType.Cursor,
                        Direction = System.Data.ParameterDirection.Output
                    }
                    ).ToList<Sys_AllSettingEntity>();
            }
            else
            {
                return this.UnitOfWork.Context.Database.SqlQuery<Sys_AllSettingEntity>(ConstantSql.hrm_cat_sp_get_GetAllSettings, new SqlParameter
                {
                    ParameterName = "ValueSelected",
                    Value = ValueSelected
                }).ToList<Sys_AllSettingEntity>();
            }
        }

        public void UpdateSettingByName(string strName, string value)
        {
            if (Common.UseDataBaseName == DATABASETYPE.ORACLE.ToString())
            {
                this.UnitOfWork.Context.Database.SqlQuery<Sys_AllSettingEntity>(ConstantSql.hrm_cat_sp_get_UpdateAllSettingByName,
                    new OracleParameter()
                    {
                        ParameterName = "strName",
                        Value = strName,
                        OracleDbType = OracleDbType.NVarChar,
                        DbType = System.Data.DbType.String
                    },
                    new OracleParameter()
                    {
                        ParameterName = "value",
                        Value = value,
                        OracleDbType = OracleDbType.NVarChar,
                        DbType = System.Data.DbType.String
                    }
                    )
                    .ToList<Sys_AllSettingEntity>().AsQueryable();
            }
            else
            {
                if (value != null)
                {
                    this.UnitOfWork.Context.Database.SqlQuery<Sys_AllSettingEntity>(ConstantSql.hrm_cat_sp_get_UpdateAllSettingByName, new SqlParameter
                    {
                        ParameterName = "strName",
                        Value = strName
                    }, new SqlParameter
                    {
                        ParameterName = "value",
                        Value = value
                    }).ToList();
                }
            }
        }


    }
}
