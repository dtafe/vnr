using HRM.Business.Category.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Data.Main.Data.Sql;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using VnResource.Helper.Data;
using VnResource.Helper.Linq;
using VnResource.Helper.Utility;

namespace HRM.Business.Category.Domain
{
    public class Cat_MasterDataGroupServices : BaseService
    {
        public List<Cat_SysTablesMultiEntity> GetChildItems(string objectName)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var enittyType = unitOfWork.GetEntityType(objectName);

                string nameField = enittyType.Name.Substring(4);
                nameField = nameField + DefaultConstants.Name;

                if (!enittyType.HasProperty(nameField))
                {
                    if (enittyType.HasProperty(DefaultConstants.Code))
                    {
                        nameField = DefaultConstants.Code;
                    }
                    else
                    {
                        nameField = string.Empty;
                    }
                }

                string[] listField = null;
                string idField = "ID AS ObjectID";

                if (!string.IsNullOrWhiteSpace(nameField))
                {
                    nameField = nameField + " AS Name";
                    listField = new string[] { idField, nameField };
                }
                else
                {
                    listField = new string[] { idField };
                }

                var lstObjectNameObj = unitOfWork.CreateQueryable(Guid.Empty, enittyType, 
                    string.Empty).SelectFields<Cat_SysTablesMultiEntity>(listField).ToList();
                return lstObjectNameObj;

            }

            return null;
        }

        public Cat_MasterDataGroupItemEntity AddMasterDataGroupItems(Cat_MasterDataGroupItemEntity model)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoMasterDataGroupItem = new Cat_MasterDataGroupItemRepository(unitOfWork);
                var lstCat_MasterDataGroupItem = new List<Cat_MasterDataGroupItem>();

                #region Lấy ds MasterDataGroupItem theo MasterDataGroupID
                var masterDataGroupItem_objectIdExists = unitOfWork.CreateQueryable<Cat_MasterDataGroupItem>(Guid.Empty, m => m.MasterDataGroupID == model.MasterDataGroupID).Select(p => p.ObjectID).ToList();
                #endregion

                if (model.ObjectIDs.Any())
                {
                    //không lưu những item co objectId đã tồn tại trong masterDataGroupItem theo masterDataGroupID
                    model.ObjectIDs = model.ObjectIDs.Except(masterDataGroupItem_objectIdExists).ToList();

                    foreach (var objectId in model.ObjectIDs)
                    {
                        var masterDataGroupItem= new  Cat_MasterDataGroupItem();
                        masterDataGroupItem.ID = Guid.NewGuid();
                        masterDataGroupItem.ObjectID = objectId;
                        masterDataGroupItem.ObjectName = model.ObjectName;
                        masterDataGroupItem.MasterDataGroupID = model.MasterDataGroupID;
                        lstCat_MasterDataGroupItem.Add(masterDataGroupItem);//add vao repo
                    }
                    if (lstCat_MasterDataGroupItem.Any())
                    {
                        repoMasterDataGroupItem.Add(lstCat_MasterDataGroupItem);
                        repoMasterDataGroupItem.SaveChanges();    
                    }                    
                }
            }
            return model;
        }
    
    }
}
