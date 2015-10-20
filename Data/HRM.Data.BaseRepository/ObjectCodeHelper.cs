using HRM.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnResource.Data.Entity;

namespace HRM.Data.BaseRepository
{
    public class ObjectCodeHelper : CodeGenerator
    {
        #region Properties

        public override Type TemplateItemType
        {
            get
            {
                base.TemplateItemType = typeof(Sys_CodeConfigItem);
                return base.TemplateItemType;
            }
            set
            {
                base.TemplateItemType = value;
            }
        }

        public override Type TemplateType
        {
            get
            {
                base.TemplateType = typeof(Sys_CodeConfig);
                return base.TemplateType;
            }
            set
            {
                base.TemplateType = value;
            }
        } 

        #endregion

        public ObjectCodeHelper(DbContext currentContext)
            : base(currentContext)
        {

        }
    }
}
