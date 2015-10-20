using HRM.Data.Attendance.Data;
using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
//using HRM.Data.Attendance.Model;
using System.Linq;
//using HRM.Data.Main.Sql;
using HRM.Business.Attendance.Models;
using System.Collections.Generic;
using System;
using HRM.Infrastructure.Utilities;
using HRM.Infrastructure.Utilities;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using System.Data;
using HRM.Business.Hr.Models;
using HRM.Business.Hr.Domain;
using System.Collections;
using System.Globalization;

namespace HRM.Business.Attendance.Domain
{
    public class Att_GradeServices : BaseService
    {
        public void AddDataForGrade(string ProfileIDs, Guid GradeAttendanceID, DateTime DateHire)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);
                List<Guid> lstProfileIDs = ProfileIDs.Split(',').Select(x => Guid.Parse(x)).ToList();
                List<Att_Grade> lstGrade = new List<Att_Grade>();
                foreach (var item in lstProfileIDs)
                {
                    Att_Grade Grade = new Att_Grade();
                    Grade.ProfileID = item;
                    Grade.GradeAttendanceID = GradeAttendanceID;
                    Grade.MonthStart = DateHire;
                    lstGrade.Add(Grade);
                }
                repoAtt_Grade.Add(lstGrade);
                repoAtt_Grade.SaveChanges();
            }
        }
        public static Att_Grade GetGrade(Hre_Profile profile, DateTime effectMonth)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);
                //NOTE: can modify lai cau truy van de gioi han data lay len
                List<Att_Grade> lst = repoAtt_Grade
                    .FindBy(alw => alw.IsDelete == null
                        && alw.ProfileID == profile.ID
                        && alw.MonthStart <= effectMonth)
                    .OrderByDescending(alw => alw.MonthStart).ToList();

                if (lst != null && lst.Count > 0)
                {
                    // Lấy grade có ngày đến hạn, nếu không có ngày đến hạn xem như Ok không hết hạn, nếu có ngày đến hạn
                    DateTime temp = new DateTime(effectMonth.Year, effectMonth.Month, 1);
                    if (!lst[0].MonthEnd.HasValue || (lst[0].MonthEnd.HasValue && lst[0].MonthEnd.Value >= temp))
                    {
                        return lst[0];
                    }
                }
                return null;
            }
        }

        public static List<Att_Grade> getAllGrade(List<Guid> listProfileId, DateTime monthYear)
        {
            return getAllGrade2(listProfileId, monthYear);
        }

        public static List<Att_Grade> getAllGrade2(List<Guid> listProfileId, DateTime monthYear)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);

                List<Att_Grade> lst = new List<Att_Grade>();
                string[] strInclude = new string[] { "Cat_GradeAttendance" };

                lst = repoAtt_Grade.FindBy(gd => listProfileId.Contains(gd.ProfileID.Value)
                    && gd.MonthStart <= monthYear).OrderByDescending(prop => prop.MonthStart).ToList();

                List<Att_Grade> lstGrade = new List<Att_Grade>();
                foreach (Att_Grade grade in lst)
                {
                    Att_Grade grade1 = lstGrade.FirstOrDefault(prop => prop.ProfileID == grade.ProfileID);
                    if (grade1 == null)
                    {
                        lstGrade.Add(grade);
                    }
                }
                return lstGrade;
            }
        }

    }
}
