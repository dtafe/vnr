using System;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Business.HrmSystem.Models;
using System.Collections.Generic;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using VnResource.Helper.Data;
using HRM.Infrastructure.Utilities;
using System.Linq;

namespace HRM.Business.HrmSystem.Domain
{
    public class Sys_AllSettingServices : BaseService
    {
          
        #region comment code do chưa co object Sys_AllSetting
        /// <summary>
        /// Lấy tất cả các bản ghi 
        /// Get by key
        /// </summary>
        /// <returns></returns>
        public List<Sys_AllSettingEntity> Get(string key)
        {
            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Sys_AllSettingRepository repo = new Sys_AllSettingRepository(unitOfWork);
                return repo.GetAllSettings(key);
            }
        }

        /// <summary>
        /// Lấy toàn bộ data theo store
        /// </summary>
        /// <returns></returns>
        public string GetSubmenu(string name, string key, Guid user)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Sys_AllSettingRepository(unitOfWork);
                Sys_AllSettingEntity entity = new Sys_AllSettingEntity();
                entity.Name = name;
                entity.Value1 = key;
                entity.UserID = user;
                var result = string.Empty;// repo.GetSubMenu(entity);
                return result;
            }
        }

        public void UpdateSettingByName(string strName, string value)
        {
            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Sys_AllSettingRepository repo = new Sys_AllSettingRepository(unitOfWork);
                repo.UpdateSettingByName(strName, value);
            }
        }
       
        /// <summary>
        /// Lấy bản ghi có id
        /// </summary>
        /// <returns></returns>
        public Sys_AllSetting GetById(Guid id)
        {
            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                Sys_AllSettingRepository repo = new Sys_AllSettingRepository(unitOfWork);
                Sys_AllSetting model = new Sys_AllSetting();
                model = repo.GetById(id);
                return model;
            }
        }


        /// <summary>
        /// Lấy tất cả các bản ghi 
        /// Get by key
        /// </summary>
        /// <returns></returns>
        public void SaveConfig(List<Dictionary<string,string>> lstModel)
        {
            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new CustomBaseRepository<Sys_AllSetting>(unitOfWork);


                string status = string.Empty;
                List<Dictionary<Guid, string>> lstUpdate = new List<Dictionary<Guid, string>>();
                var valueTemp = new Sys_AllSetting();
                var modelTemp = new Sys_AllSetting();
                List<Guid> lstConfigIds = new List<Guid>();
                List<Sys_AllSetting> lstAdd = new List<Sys_AllSetting>();
                List<Sys_AllSetting> lstEdit = new List<Sys_AllSetting>();
                List<Sys_AllSetting> lstDel = new List<Sys_AllSetting>();

                var getdata = repo.FindBy(s => s.IsDelete == null).ToList();

                if (lstModel[0].FirstOrDefault().Value.Length > 0)
                {
                    var lstStr = lstModel.FirstOrDefault()["lstConfigIds"].Split(',');
                    lstConfigIds = lstStr.Select(Guid.Parse).ToList();
                }

                foreach (var item in lstConfigIds)
                {
                    modelTemp = GetById<Sys_AllSetting>(item, ref status);
                    lstUpdate.Add(new Dictionary<Guid, string>() { { modelTemp.ID, modelTemp.Name } });
                }
                for (int i = 1; i < lstModel.Count; i++)
                {
                    var lstEntity = getdata.Where(s => s.Name == lstModel[i].FirstOrDefault().Key).ToList();

                    if (lstEntity.Count == 0)
                    {
                        valueTemp = new Sys_AllSetting();
                        valueTemp.ID = Guid.NewGuid();
                        valueTemp.Name = lstModel[i].FirstOrDefault().Key;
                        valueTemp.Value1 = lstModel[i].FirstOrDefault().Value;
                        lstAdd.Add(valueTemp);
                        continue;
                    }
                    if (lstEntity.Count > 0)
                    {
                        valueTemp = new Sys_AllSetting();
                        foreach (var item in lstEntity)
                        {
                            item.IsDelete = true;
                            lstDel.Add(item);
                            //repo.Edit(item);
                        }
                        var isUpdate = lstUpdate.Where(s => s.FirstOrDefault().Value == lstModel[i].FirstOrDefault().Key).FirstOrDefault();
                        if (isUpdate != null)
                        {
                            valueTemp.ID = lstEntity.FirstOrDefault().ID;
                            valueTemp.Name = lstModel[i].FirstOrDefault().Key;
                            valueTemp.Value1 = lstModel[i].FirstOrDefault().Value;
                            valueTemp.IsDelete = null;
                            lstEdit.Add(valueTemp);
                        }
                        else
                        {
                            valueTemp.ID = Guid.NewGuid();
                            valueTemp.Name = lstModel[i].FirstOrDefault().Key;
                            valueTemp.Value1 = lstModel[i].FirstOrDefault().Value;
                            lstAdd.Add(valueTemp);
                        }
                    }
                }
                repo.Edit(lstDel);
                repo.Add(lstAdd);
                repo.Edit(lstEdit);
                repo.SaveChanges();

                
            }
        }
        #endregion
     
    }
}
