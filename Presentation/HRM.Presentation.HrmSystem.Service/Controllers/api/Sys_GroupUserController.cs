using System.Web.Http;

namespace HRM.Presentation.HrmSystem.Service.Controllers
{
    //chưa sử dụng Sys_GroupEntityUser
    public class Sys_GroupEntityUserController : ApiController
    {
        //// GET api/<controller>
        ///// <summary>
        ///// Lấy tất cả dữ liệu
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Sys_GroupUserModel> Get()
        //{
        //    string status = string.Empty;
        //    var service = new Sys_GroupEntityUserServices();
        //    var list = service.GetData<Sys_GroupEntityUserEntity>(ConstantSql.hrm_sys_sp_get_GroupUser, ref status).AsQueryable<Sys_GroupEntityUserEntity>();
                
        //    return list.Select(item => new Sys_GroupUserModel
        //    {
        //        Id = item.Id,
        //        GroupUserName = item.GroupUserName,
        //        Code = item.Code,
        //        Note = item.Note,
        //        GroupID = item.GroupID,
        //        GroupName = item.GroupName,
        //        UserID = item.UserID,
        //        UserLogin = item.UserLogin,
        //    });


        //    #region code cũ
        //    //var service = new Sys_GroupEntityUserServices();
        //    //var list = service.GetGroupUsers();

        //    //return list.Select(item => new Sys_GroupUserModel
        //    //{
        //    //    Id = item.Id,
        //    //    GroupUserName = item.GroupUserName,
        //    //    Code = item.Code,
        //    //    Note = item.Note,
        //    //    GroupID = item.GroupID,
        //    //    GroupName = item.GroupName,
        //    //    UserID = item.UserID,
        //    //    UserLogin = item.UserLogin,
        //    //}); 
        //    #endregion
        //}

        //// GET api/<controller>/5
        //public Sys_GroupUserModel Getbyid(int id)
        //{
        //    string status = string.Empty;
        //    var service = new Sys_GroupEntityUserServices();
        //    var result = service.GetById<Sys_GroupEntityUser>(id, ref status);
        //    var sysgroupuser = new Sys_GroupUserModel
        //    {
        //        Id = result.Id,
        //        Code = result.Code,
        //        GroupUserName = result.GroupUserName,
        //        Note = result.Note
        //    };
        //    return sysgroupuser;

        //    #region code Cũ

        //    //var service = new Sys_GroupEntityUserServices();
        //    //var result = service.GetById(id);
        //    //var sysgroupuser = new Sys_GroupUserModel
        //    //{
        //    //    Id = result.Id,
        //    //    Code = result.Code,
        //    //    GroupUserName = result.GroupUserName,
        //    //    Note = result.Note
        //    //};
        //    //return sysgroupuser; 
        //    #endregion
        //}



        //public IEnumerable<Sys_GroupUserModel> Put([FromBody]int id)
        //{
        //    var service = new Sys_GroupEntityUserServices();
        //    var result = service.GetByUserId(id);
        //    return result.Select(item => new Sys_GroupUserModel
        //    {
        //        Id = item.Id,
        //        GroupUserName = item.GroupUserName,
        //        Code = item.Code,
        //        Note = item.Note,
        //        GroupID = item.GroupID,
        //        GroupName = item.GroupName,
        //        UserID = item.UserID,
        //        UserLogin = item.UserLogin,
        //        UserName = item.UserName,
        //        OrgStructuresId = item.OrgStructures.ToNumbers(),
        //        OrgStructuresName = item.OrgStructuresName
        //    });
        //}

        //[System.Web.Mvc.HttpPost]
        //[System.Web.Mvc.RouteAttribute("api/Sys_GroupEntityUser")]
        //public Sys_GroupUserModel Post([FromBody]Sys_GroupUserModel groupuser)
        //{
        //    var orgStructure = groupuser.OrgStructures.ToBinary();
        //    var model = new Sys_GroupEntityUser
        //    {
        //        Id = groupuser.Id,
        //        Code = groupuser.Code,
        //        GroupUserName = groupuser.GroupUserName,
        //        Note = groupuser.Note,
        //        GroupID = groupuser.GroupID,
        //        UserID = groupuser.UserID,
        //        OrgStructures = orgStructure
        //    };
        //    var service = new Sys_GroupEntityUserServices();
        //    if (groupuser.Id != 0)
        //    {
        //        groupuser.Id = groupuser.Id;
        //        service.Edit(model);
        //    }
        //    else
        //    {
        //        service.Add(model);
        //        groupuser.Id = model.Id;
        //    }

        //    return groupuser;
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //    var service = new Sys_GroupEntityUserServices();
        //    var result = service.Delete<Sys_GroupEntityUser>(id);
        //}
    }

}