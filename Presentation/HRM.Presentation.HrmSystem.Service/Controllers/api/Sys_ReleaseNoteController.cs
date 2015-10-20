using System.Web.Http;
//using HRM.Data.HrmSystem.Model;ti
namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class Sys_ReleaseNoteController : ApiController
    {
        #region Sys_ReleaseNote

        ///// <summary> Lấy tất cả dữ liệu </summary>
        ///// <returns></returns>
        //public IEnumerable<Sys_ReleaseNoteModel> Get()
        //{
        //    string status = string.Empty;
        //    var service = new Sys_ReleaseNoteServices();
        //    var hre = service.GetAllUseEntity<Sys_ReleaseNote>(ref status);

        //    return hre.Select(item => new Sys_ReleaseNoteModel()
        //    {
        //        Id = item.Id,
        //        Code = item.Code,
        //        DateRelease = item.DateRelease,
        //        ReleaseNoteName = item.ReleaseNoteName,
        //        KnownIssuesandProblems = item.KnownIssuesandProblems,
        //        Fixes = item.Fixes,
        //        FeaturesNew = item.FeaturesNew
        //    });


        //    #region code cũ
        //    //var service = new Sys_ReleaseNoteServices();
        //    //var hre = service.Get();

        //    //return hre.Select(item => new Sys_ReleaseNoteModel()
        //    //{
        //    //    Id = item.Id,
        //    //    Code = item.Code,
        //    //    DateRelease = item.DateRelease,
        //    //    ReleaseNoteName = item.ReleaseNoteName,
        //    //    KnownIssuesandProblems = item.KnownIssuesandProblems,
        //    //    Fixes = item.Fixes,
        //    //    FeaturesNew = item.FeaturesNew
        //    //}); 
        //    #endregion
        //}

        //public Sys_ReleaseNoteModel Get(Guid id)
        //{
        //    string status = string.Empty;
        //    var service = new Sys_ReleaseNoteServices();
        //    var listEntity = service.GetAllUseEntity<Sys_ReleaseNote>(ref status);
        //    var model = listEntity.Where(w => w.UserUpdate == "LoginTemp" && w.UserCreate == "LoginTemp").Select(s => new Sys_ReleaseNoteModel
        //    {
        //        Code = s.Code,
        //        ReleaseNoteName = s.ReleaseNoteName
        //    });

        //    return model.FirstOrDefault();
        //}

        ///// <summary> Lấy dữ liệu theo id </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public Sys_ReleaseNoteModel Get(int id)
        //{
        //    string status = string.Empty;
        //    var service = new Sys_ReleaseNoteServices();
        //    var releaseNote = service.GetById<Sys_ReleaseNote>(id, ref status);
        //    var result = new Sys_ReleaseNoteModel()
        //    {
        //        Id = releaseNote.Id,
        //        Code = releaseNote.Code,
        //        DateRelease = releaseNote.DateRelease,
        //        ReleaseNoteName = releaseNote.ReleaseNoteName,
        //        KnownIssuesandProblems = releaseNote.KnownIssuesandProblems,
        //        Fixes = releaseNote.Fixes,
        //        FeaturesNew = releaseNote.FeaturesNew
        //    };
        //    return result;


        //    #region code cũ
        //    //var service = new Sys_ReleaseNoteServices();
        //    //var releaseNote = service.GetById(id);
        //    //var result = new Sys_ReleaseNoteModel()
        //    //{
        //    //    Id = releaseNote.Id,
        //    //    Code = releaseNote.Code,
        //    //    DateRelease = releaseNote.DateRelease,
        //    //    ReleaseNoteName = releaseNote.ReleaseNoteName,
        //    //    KnownIssuesandProblems = releaseNote.KnownIssuesandProblems,
        //    //    Fixes = releaseNote.Fixes,
        //    //    FeaturesNew = releaseNote.FeaturesNew
        //    //};
        //    //return result; 
        //    #endregion
        //}

        ///// <summary> Xử lý edit hay add new tùy thuộc vào id đã có hay chưa </summary>
        ///// <param name="releaseNote"></param>
        ///// <returns></returns>
        //public Sys_ReleaseNoteModel Put(Sys_ReleaseNoteModel releaseNote)
        //{
        //    var model = new Sys_ReleaseNote
        //    {
        //        Id = releaseNote.Id,
        //        Code = releaseNote.Code,
        //        DateRelease = releaseNote.DateRelease,
        //        ReleaseNoteName = releaseNote.ReleaseNoteName,
        //        KnownIssuesandProblems = releaseNote.KnownIssuesandProblems,
        //        Fixes = releaseNote.Fixes,
        //        FeaturesNew = releaseNote.FeaturesNew
        //    };
        //    var service = new Sys_ReleaseNoteServices();
        //    if (releaseNote.Id != 0)
        //    {
        //        model.Id = releaseNote.Id;
        //        service.Edit<Sys_ReleaseNote>(model);
        //    }
        //    else
        //    {
        //        service.Add<Sys_ReleaseNote>(model);
        //        releaseNote.Id = model.Id;
        //    }
        //    return releaseNote;
        //}

        //[System.Web.Mvc.HttpPost]
        //[System.Web.Mvc.RouteAttribute("api/Sys_ReleaseNote")]
        //public Sys_ReleaseNoteModel Post([FromBody]Sys_ReleaseNoteModel releaseNote)
        //{
        //    var service = new Sys_ReleaseNoteServices();

        //    var model = new Sys_ReleaseNote
        //    {
        //        Id = releaseNote.ID,
        //        Code = releaseNote.Code,
        //        ReleaseNoteName = releaseNote.ReleaseNoteName,
        //        UserCreate = releaseNote.UserNameCreate,
        //        UserUpdate = releaseNote.UserNameUpdate,
        //    };
        //    string status = string.Empty;
        //    var listEntity = service.GetAllUseEntity<Sys_ReleaseNote>(ref status);
        //    if (listEntity != null && listEntity.Count > 0)
        //    {
        //        var modelCheckDelete = listEntity.Where(w => w.UserUpdate == "LoginTemp" && w.UserCreate == "LoginTemp").FirstOrDefault();
        //        if (modelCheckDelete != null)
        //        {
        //            service.Delete<Sys_ReleaseNote>(modelCheckDelete.Id);
        //        }
        //    }
        //    service.Add<Sys_ReleaseNote>(model);
        //    releaseNote.ID = model.Id;

        //    return releaseNote;
        //}


        ///// <summary> Xử lý Remove một record dựa vào Id (Remove # Delete: Remove chuyển IsDelete từ null thành true, Delete thì delete record khỏi database </summary>
        ///// <param name="id"></param>
        //public Sys_ReleaseNoteModel Remove(int id)
        //{
        //    var service = new Sys_ReleaseNoteServices();
        //    service.Remove<Sys_ReleaseNote>(id);
        //    var result = new Sys_ReleaseNoteModel();
        //    return result;
        //}

        //public Sys_ReleaseNoteModel Delete(int id)
        //{
        //    var service = new Sys_ReleaseNoteServices();
        //    service.Remove<Sys_ReleaseNote>(id);
        //    var result = new Sys_ReleaseNoteModel();
        //    return result;
        //}

        #endregion

    }
}