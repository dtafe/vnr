using HRM.Presentation.HrmSystem.Service.Framework.MVC;

namespace HRM.Presentation.HrmSystem.Service.Framework
{
    public class HRMDisplayName : System.ComponentModel.DisplayNameAttribute, IModelAttribute
    {
         private string _resourceValue = string.Empty;
        //private bool _resourceValueRetrived;

         public HRMDisplayName(string resourceKey)
            : base(resourceKey)
        {
            ResourceKey = resourceKey;
        }

        public string ResourceKey { get; set; }

        public override string DisplayName
        {
            get
            {
                //do not cache resources because it causes issues when you have multiple languages
                //if (!_resourceValueRetrived)
                //{
                //var langId = 1;// EngineContext.Current.Resolve<IWorkContext>().WorkingLanguage.Id;
                //    _resourceValue = EngineContext.Current
                //        .Resolve<ILocalizationService>()
                //        .GetResource(ResourceKey, langId, true, ResourceKey);
                //    _resourceValueRetrived = true;
                //}
                //return _resourceValue;
                return "";
            }
        }

        public string Name
        {
            get { return "HRMDisplayName"; }
        }
    }
}