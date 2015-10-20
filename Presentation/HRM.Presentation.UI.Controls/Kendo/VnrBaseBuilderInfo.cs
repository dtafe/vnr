using System;

namespace HRM.Presentation.UI.Controls.Kendo
{
    public class VnrBaseBuilderInfo
    {
        #region Properties

        private string name = string.Empty;

        /// <summary>
        /// Tên control được tạo ra.
        /// </summary>
        public virtual string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = GetType().Name;//Tên theo builderInfo
                    name += DateTime.Now.ToOADate().ToString();
                }

                return name;
            }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Tên controller dữ liệu.
        /// </summary>
        public virtual string Controller { get; set; }

        #endregion
    }
}
