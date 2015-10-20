using HRM.Business.Category.Models;
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
using VnResource.Helper;
using VnResource.Helper.Data;
using System.Reflection;
using VnResource.Helper.Setting;
using VnResource.Helper.Linq;
using System.Threading.Tasks;
using VnResource.Helper.Ado;

namespace HRM.Business.Category.Domain
{
    public class Cat_FormulaTemplateServices : BaseService
    {
        public void CreateFormulaTemplate(List<Cat_FormulaTemplateEntity> listFormulaTemplate,Guid GradeID)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoCat_FormulaTemplate = new CustomBaseRepository<Cat_FormulaTemplate>(unitOfWork);
                var repoCat_Element = new CustomBaseRepository<Cat_Element>(unitOfWork);

                #region Xử lý bảng template
                //lấy các phần tử template trong grade
                List<Cat_FormulaTemplate> listFormulaTemplateByGrade = repoCat_FormulaTemplate.FindBy(m => m.IsDelete != true && m.GradeID != null && m.GradeID == GradeID).ToList();
                //xóa các phần tử đó
                repoCat_FormulaTemplate.Delete(listFormulaTemplateByGrade);

                //insert lại các phần tử mới
                List<Cat_FormulaTemplate> listFormulaTemplateModel = listFormulaTemplate.Translate<Cat_FormulaTemplate>();
                listFormulaTemplateModel.ForEach(m => m.GradeID = GradeID);
                repoCat_FormulaTemplate.Add(listFormulaTemplateModel); 
                #endregion

                #region Xử lý bảng cat_element
                //lấy các phần tử có cùng mã code với template
                List<Cat_Element> listElement = repoCat_Element.FindBy(m => m.IsDelete != true && m.GradePayrollID == GradeID).ToList();
                listElement = listElement.Where(m => listFormulaTemplate.Any(t => t.ElementCode.ReplaceSpace() == m.ElementCode.ReplaceSpace())).ToList();
                //xóa các phần tử đó đi
                repoCat_Element.Delete(listElement);

                //tạo ra các element
                List<Cat_Element> listElementModel = new List<Cat_Element>();
                Cat_Element item = new Cat_Element();
                foreach (var template in listFormulaTemplate)
                {
                    item = new Cat_Element();
                    item.GradePayrollID = GradeID;
                    item.ElementCode = template.ElementCode.ReplaceSpace();
                    item.ElementName = template.ElementName;
                    item.Formula = template.Formula;
                    item.IsBold = template.IsBold;
                    item.Invisible = template.Invisible;
                    item.Description = template.Description;
                    item.TabType = CatElementType.Payroll.ToString();
                    item.MethodPayroll = MethodPayroll.E_NORMAL.ToString();
                    item.ElementType = CatElementType.Payroll.ToString();    
                    item.Type = EnumDropDown.ElementDataType.Double.ToString();
                    listElementModel.Add(item);
                }
                repoCat_Element.Add(listElementModel);

                unitOfWork.SaveChanges();
                #endregion

            }
        }
    }
}
