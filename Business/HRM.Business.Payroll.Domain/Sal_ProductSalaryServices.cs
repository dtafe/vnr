using HRM.Business.Attendance.Models;
using HRM.Business.Category.Models;
using HRM.Business.Hr.Models;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Business.Payroll.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Threading;
using SystemThread = System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRM.Infrastructure.Utilities.Helper;
using HRM.Business.Insurance.Models;
using HRM.Business.Evaluation.Models;
using VnResource.Helper;
using VnResource.Helper.Data;
using HRM.Business.HrmSystem.Domain;
using System.Reflection;
using VnResource.Helper.Setting;
using VnResource.Helper.Linq;
using System.Threading.Tasks;
using VnResource.Helper.Ado;
using VnResource.AdoHelper;
using HRM.Business.Canteen.Models;
using HRM.Business.Hr.Domain;
namespace HRM.Business.Payroll.Domain
{
    public class Sal_ProductSalaryServices : BaseService
    {
        public void ComputeProductSalary(string OrgStructure, Guid? ProductID, Guid? ProductItemID, DateTime MonthStart, DateTime MonthEnd, string userLoginName)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoSal_ProductSalary = new CustomBaseRepository<Sal_ProductSalary>(unitOfWork);
                var repoSal_ProductSalary1 = new CustomBaseRepository<Hre_Profile>(unitOfWork);

                List<Cat_ProductEntity> ListProduct = new List<Cat_ProductEntity>();
                List<Cat_ProductItemEntity> ListProductItem = new List<Cat_ProductItemEntity>();
                List<Hre_ProfileEntity> ListProfile = new List<Hre_ProfileEntity>();
                List<Sal_ProductiveEntity> ListProductive = new List<Sal_ProductiveEntity>();
                List<Sal_ProductCapacityEntity> ListProductCapacity = new List<Sal_ProductCapacityEntity>();
                List<Sal_ProductSalaryEntity> ListProductSalary = new List<Sal_ProductSalaryEntity>();

                #region GetData
                string status = string.Empty;
                List<object> listModel = new List<object>();

                listModel = new List<object>();
                listModel.AddRange(new object[6]);
                listModel[4] = 1;
                listModel[5] = Int32.MaxValue - 1;
                ListProduct = GetData<Cat_ProductEntity>(listModel, ConstantSql.hrm_cat_sp_get_Product,userLoginName, ref status);

                listModel = new List<object>();
                listModel.AddRange(new object[3]);
                listModel[1] = 1;
                listModel[2] = Int32.MaxValue - 1;
                ListProductItem = GetData<Cat_ProductItemEntity>(listModel, ConstantSql.hrm_cat_sp_get_ProductItem,userLoginName, ref status);

                listModel = new List<object>();
                listModel.AddRange(new object[17]);
                listModel[2] = OrgStructure;
                listModel[15] = 1;
                listModel[16] = Int32.MaxValue - 1;
                ListProfile = GetData<Hre_ProfileEntity>(listModel, ConstantSql.hrm_hr_sp_get_ProfileAll,userLoginName, ref status);

                listModel = new List<object>();
                listModel.AddRange(new object[6]);
                listModel[2] = MonthStart;
                listModel[3] = MonthEnd;
                listModel[4] = 1;
                listModel[5] = Int32.MaxValue - 1;
                ListProductive = GetData<Sal_ProductiveEntity>(listModel, ConstantSql.hrm_sal_sp_get_Sal_Producttive, userLoginName,ref status);

                listModel = new List<object>();
                listModel.AddRange(new object[7]);
                listModel[3] = MonthStart;
                listModel[4] = MonthEnd;
                listModel[5] = 1;
                listModel[6] = Int32.MaxValue - 1;
                ListProductCapacity = GetData<Sal_ProductCapacityEntity>(listModel, ConstantSql.hrm_sal_sp_get_ProductCapacity,userLoginName, ref status);

                listModel = new List<object>();
                listModel.AddRange(new object[7]);
                listModel[0] = OrgStructure;
                listModel[3] = MonthStart;
                listModel[4] = MonthEnd;
                listModel[5] = 1;
                listModel[6] = Int32.MaxValue - 1;
                ListProductSalary = GetData<Sal_ProductSalaryEntity>(listModel, ConstantSql.hrm_sal_sp_get_Sal_ProductSalary,userLoginName, ref status);
                #endregion

                #region Delete dữ liệu củ
                //lọc ra theo nhân viên
                List<Sal_ProductSalary> listProductSalaryByProfile = ListProductSalary.Where(m => m.ProfileID != null && ListProfile.Any(t => t.ID == m.ProfileID)).ToList().Translate<Sal_ProductSalary>();
                //bật cờ isdelete
                repoSal_ProductSalary.Delete(listProductSalaryByProfile);

                #endregion

                #region Progress
                
                //lọc Productive && ProductCapacity theo product || productitem
                ListProductive = ListProductive.Where(m => (ProductID != null && m.ProductID == ProductID) || (ProductItemID != null && m.ProductItemID == ProductItemID)).ToList();
                ListProductCapacity = ListProductCapacity.Where(m => (ProductID != null && m.ProductID == ProductID) || (ProductItemID != null && m.ProductItemID == ProductItemID)).ToList();

                //duyệt qua profile
                foreach (var profile in ListProductive)
                {
                    #region các biến lưu dữ liệu tính đc
                    //tính sản phẩm thừa của nhân viên trong tháng trước
                    double TotalQuantityProductPrevious = 0;
                    //tổng sản lượng nhân viên làm đc, chưa cộng sản phẩm thừa vào
                    double TotalQuantityProduct = 0;
                    //Tổng sản lượng dùng để tính lương
                    double TotalQuantitySalary = 0;
                    //sản phẩm thừa trong tháng tính lương
                    double TotalQuantitySalaryPrevious = 0;
                    //tổng sản phẩm của tất cả nhân viên làm trong tháng
                    double TotalQuantityTotalProfile = 0;
                    //tổng tiền của sản phẩm
                    double TotalAmountByProduct = 0;
                    //lưu lại ID của loại tiền tệ
                    Guid? CurrencyID = null; 
                    #endregion

                    //các sản phẩm mà nhân viên làm đc trong tháng
                    List<Sal_ProductiveEntity> ListProductiveByProfile = new List<Sal_ProductiveEntity>();
                    if (profile.ProductItemID != null)
                    {
                        ListProductiveByProfile = ListProductive.Where(m => m.ProfileID != null && m.ProfileID == profile.ID && m.ProductItemID != null && m.ProductItemID == profile.ProductItemID).ToList();
                        TotalQuantityTotalProfile = ListProductive.Where(m => m.ProductItemID != null && m.ProductItemID == profile.ProductItemID).Sum(m => m.Quantity != null ? (double)m.Quantity : 0);
                    }
                    else if (profile.ProductID != null)
                    {
                        ListProductiveByProfile = ListProductive.Where(m => m.ProfileID != null && m.ProfileID == profile.ID && m.ProductID != null && m.ProductID == profile.ProductID).ToList();
                        TotalQuantityTotalProfile = ListProductive.Where(m => m.ProductID != null && m.ProductID == profile.ProductID).Sum(m => m.Quantity != null ? (double)m.Quantity : 0);
                    }

                    //tổng sản lượng nhân viên làm đc, chưa cộng sản phẩm thừa vào
                    TotalQuantityProduct = ListProductiveByProfile.Sum(m => m.Quantity != null ? (double)m.Quantity : 0);

                    //tính sản phẩm thừa của nhân viên trong tháng trước
                    TotalQuantityProductPrevious = 0;
                    DateTime dateStart = new DateTime(MonthStart.Year, MonthStart.Month, MonthStart.Day);
                    //lấy ra lương sản phẩm của tháng trước để lấy sản phẩm thừa
                    var ListProductSalaryPrevious = ListProductSalary.Where(m => m.ProfileID != null && m.ProfileID == profile.ID && m.MonthYear != null && m.MonthYear < dateStart).OrderByDescending(m => m.MonthYear).ToList();
                    if (profile.ProductItemID != null)
                    {
                        ListProductSalaryPrevious = ListProductSalaryPrevious.Where(m => m.ProductID == profile.ProductID).ToList();
                    }
                    else if (profile.ProductID != null)
                    {
                        ListProductSalaryPrevious = ListProductSalaryPrevious.Where(m => m.ProductItemID == profile.ProductItemID).ToList();
                    }
                    TotalQuantityProductPrevious = ListProductSalaryPrevious.Sum(m => m.QtyNext != null ? (double)m.QtyNext : 0);
                    //sum tổng sản phẩm và sản phẩm thừa tháng trước lại
                    TotalQuantityProduct += TotalQuantityProductPrevious;

                    //lấy định mức sản phẩm
                    var ProductCapacity = ListProductCapacity.Where(m => m.DepartmentID == profile.OrgStructureID).FirstOrDefault();

                    //kiểm tra định mức phải khác null
                    if (ProductCapacity!=null && ProductCapacity.MaxCapacity != null)
                    {
                        //tổng sản lượng nhỏ hơn định mức
                        if (ProductCapacity.MaxCapacity >= TotalQuantityProduct)
                        {
                            TotalQuantitySalary = TotalQuantityProduct;
                            TotalQuantitySalaryPrevious = 0;
                        }
                        else// tổng sản lượng lớn hơn định mức
                        {
                            TotalQuantitySalary = TotalQuantityProduct / TotalQuantityTotalProfile * (double)ProductCapacity.MaxCapacity;
                            TotalQuantitySalaryPrevious = TotalQuantityProduct - TotalQuantitySalary;
                        }
                    }

                    //lấy giá tiền của sản phẩm
                    if (profile.ProductItemID != null)
                    {
                        var ProductByID=ListProductItem.Where(m => m.ID == (Guid)profile.ProductItemID).FirstOrDefault();
                        //cập nhật tiền tệ
                        CurrencyID = ProductByID != null ? ProductByID.CurrencyID : null;
                        //cập nhật tổng tiền
                        TotalAmountByProduct = TotalQuantitySalary * (ProductByID != null && ProductByID.UnitPrice != null ? (double)ProductByID.UnitPrice : 0);
                    }
                    else if (profile.ProductID != null)
                    {
                        var ProductByID = ListProduct.Where(m => m.ID == (Guid)profile.ProductID).FirstOrDefault();
                        //cập nhật tiền tệ
                        CurrencyID = ProductByID != null ? ProductByID.CurrencyID : null;
                        //câp nhật tổng tiền
                        TotalAmountByProduct = TotalQuantitySalary * (ProductByID != null && ProductByID.BonusPerUnit != null ? (double)ProductByID.BonusPerUnit : 0);
                    }
          
                    //lưu dữ liệu vào bảng sal_productsalary
                    Sal_ProductSalary ProductSalary = new Sal_ProductSalary();
                    ProductSalary.ID = Guid.NewGuid();
                    ProductSalary.ProfileID = profile.ProfileID;
                    ProductSalary.ProductID = profile.ProductID;
                    ProductSalary.ProductItemID = profile.ProductItemID;
                    ProductSalary.MonthYear = new DateTime(MonthStart.Year, MonthStart.Month, MonthStart.Day);
                    ProductSalary.QtyActual = TotalQuantityProduct;
                    ProductSalary.QtyNext = TotalQuantitySalaryPrevious;
                    ProductSalary.QtyPrevious = TotalQuantityProductPrevious;
                    ProductSalary.QtySalary = TotalQuantitySalary;
                    ProductSalary.Amount = TotalAmountByProduct;
                    ProductSalary.CurrencyID = CurrencyID;
                    repoSal_ProductSalary.Add(ProductSalary);
                }

                unitOfWork.SaveChanges();
                #endregion
            }
        }
    }
}
