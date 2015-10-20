namespace HRM.Presentation.UI.Controls.Kendo.ControlBase
{
    public class VnrPropertiesBase
    {
        #region Properties

        private string name;
        private int width;
        private int height;
        private object value;


        private bool enable = true;

        /// <summary>
        /// Tên Control được tạo ra
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Chiều rộng control vừa đươc tạo
        /// </summary>
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// Chiều dài control vừa được tạo
        /// </summary>
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// Giá trị của control vừa tạo ra
        /// </summary>
        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Quy định thuộc tính có cho phép chỉnh sửa control hay không
        /// </summary>
        public bool Enable
        {
            get { return enable; }
            set { enable = value; }
        }

        #endregion
    }
}
