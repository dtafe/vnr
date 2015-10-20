using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Xml;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.UI.Controls.Menu
{
    public class XmlSiteMap
    {
        public XmlSiteMap()
        {
            RootNode = new SiteMapNode();
        }

        public SiteMapNode RootNode { get; set; }

        public virtual void LoadFrom(string physicalPath)
        {
            //var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            string filePath = HttpContext.Current.Server.MapPath(physicalPath);
            string content = File.ReadAllText(filePath);

            if (!string.IsNullOrEmpty(content))
            {
                using (var sr = new StringReader(content))
                {
                    using (var xr = XmlReader.Create(sr,
                            new XmlReaderSettings
                            {
                                CloseInput = true,
                                IgnoreWhitespace = true,
                                IgnoreComments = true,
                                IgnoreProcessingInstructions = true
                            }))
                    {
                        var doc = new XmlDocument();
                        doc.Load(xr);

                        if ((doc.DocumentElement != null) && doc.HasChildNodes)
                        {
                            XmlNode xmlRootNode = doc.DocumentElement.FirstChild;
                            Iterate(RootNode, xmlRootNode);
                        }
                    }
                }
            }
        }

        private static void Iterate(SiteMapNode siteMapNode, XmlNode xmlNode)
        {
            PopulateNode(siteMapNode, xmlNode);

            foreach (XmlNode xmlChildNode in xmlNode.ChildNodes)
            {
                if (xmlChildNode.LocalName.Equals("mvcSiteMapNode", StringComparison.InvariantCultureIgnoreCase))
                {
                    var siteMapChildNode = new SiteMapNode();
                    siteMapNode.ChildNodes.Add(siteMapChildNode);

                    Iterate(siteMapChildNode, xmlChildNode);
                }
            }
        }

        private static System.Collections.Hashtable _htMenuSites = new System.Collections.Hashtable();

        private static void PopulateNode(SiteMapNode siteMapNode, XmlNode xmlNode)
        {
            //title
            var nopResource = GetStringValueFromAttribute(xmlNode, "nopResource");
            siteMapNode.Parent = GetStringValueFromAttribute(xmlNode, "Parent");
            siteMapNode.NopResource = nopResource;

            if (!string.IsNullOrEmpty(nopResource))
            {
                //var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
                siteMapNode.Title = nopResource;
            }
            else
            {
                siteMapNode.Title = GetStringValueFromAttribute(xmlNode, "title");
            }


            //routes, url
            string moduleName = GetStringValueFromAttribute(xmlNode, "ModuleName");
            string controllerName = GetStringValueFromAttribute(xmlNode, "controller");
            string actionName = GetStringValueFromAttribute(xmlNode, "action");
            string url = GetStringValueFromAttribute(xmlNode, "url");
            if (!string.IsNullOrEmpty(moduleName))
            {
                siteMapNode.ModuleName = moduleName;
            }
            if (!string.IsNullOrEmpty(controllerName) && !string.IsNullOrEmpty(actionName))
            {
                siteMapNode.ControllerName = controllerName;
                siteMapNode.ActionName = actionName;

                siteMapNode.RouteValues = new RouteValueDictionary()
                                          {
                                              {"area", "Admin"}
                                          };
            }
            else if (!string.IsNullOrEmpty(url))
            {
                //siteMapNode.Url = url;
            }
            //set hrmwebsite
            siteMapNode.HrmWebSite = GetStringValueFromAttribute(xmlNode, "HrmWebSite");
            if (!string.IsNullOrEmpty(siteMapNode.HrmWebSite))
            {
                string site = (string)_htMenuSites[siteMapNode.HrmWebSite];
                if (string.IsNullOrEmpty(site))
                {
                    string siteDefault = (string)System.Configuration.ConfigurationManager.AppSettings[siteMapNode.HrmWebSite];
                    if (!string.IsNullOrEmpty(siteDefault))
                    {
                        _htMenuSites.Add(siteMapNode.HrmWebSite, siteDefault);
                        siteMapNode.Url = "#" + siteMapNode.HrmWebSite + "/" + siteMapNode.ControllerName + "/" + siteMapNode.ActionName; //siteDefault + "/#" + siteMapNode.ControllerName + "/" + siteMapNode.ActionName;
                        siteMapNode.Alias = siteDefault;
                    }
                }
                else
                {
                    siteMapNode.Alias = site;
                    siteMapNode.Url = "#" + siteMapNode.HrmWebSite + "/" + siteMapNode.ControllerName + "/" + siteMapNode.ActionName;
                }
            }

            //image URL
            siteMapNode.ImageUrl = GetStringValueFromAttribute(xmlNode, "ImageUrl");

            //permission name
            //var permissionNames = GetStringValueFromAttribute(xmlNode, "PermissionNames");
            //if (!string.IsNullOrEmpty(permissionNames))
            //{
            //    var permissionService = EngineContext.Current.Resolve<IPermissionService>();
            //    siteMapNode.Visible = permissionNames.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            //       .Any(permissionName => permissionService.Authorize(permissionName.Trim()));
            //}
            //else
            //{
            siteMapNode.Visible = true;
            //}
        }

        private static string GetStringValueFromAttribute(XmlNode node, string attributeName)
        {
            string value = null;

            if (node.Attributes.Count > 0)
            {
                XmlAttribute attribute = node.Attributes[attributeName];

                if (attribute != null)
                {
                    //value = attribute.Value.TranslateString();
                    value = attribute.Value;
                }
            }

            return value;
        }
    }
}
