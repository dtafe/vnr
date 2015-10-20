using Aspose.Words;
using Aspose.Words.Drawing;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Infrastructure.Utilities.Helper
{
    public static class ExportHelper
    {
        #region Private Members
        private static string _FileName = "";
        private static Aspose.Words.Document _oWord;

        #endregion

        #region Properties
        /// <summary>
        /// Readonly: Return the name of output file
        /// </summary>
        public static string FileName
        {
            get { return _FileName; }
        }
        #endregion

        #region Constructors

        //public static ExportHelper()
        //{
        //}
        #endregion

        #region Custom Methods
        #region Open and Save
        /// <summary>
        /// Open file and load to memory
        /// </summary>
        /// <param name="FileName">include folder path</param>
        /// <returns>
        /// 0: Open successful
        /// -1: Can't open file
        /// </returns>
        public static int Open(string FileName)
        {
            try
            {
                System.IO.Stream licStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VnResourceWeb.Utils.License.xml");
                //Open file
                _FileName = FileName;
                _oWord = new Aspose.Words.Document(_FileName);
                if (_oWord == null)
                {
                    return -1;
                }
               
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 0;
        }
        /// <summary>
        /// Save file in format Word 2003
        /// </summary>
        /// <returns>
        /// 0: Save successful
        /// -1: Can't create folder
        /// -2: Can't save file
        /// </returns>
        public static int Save()
        {
            int iResult = 0;

            //Save file
            try
            {
                _oWord.Save(_FileName, SaveFormat.Doc);
            }
            catch
            {
                iResult = -2;
            }
            return iResult;
        }
        /// <summary>
        /// Save as
        /// </summary>
        /// <param name="NewFile"></param>
        /// <returns></returns>
        public static int Save(string NewFile)
        {
            int iResult = 0;

            //Save file
            try
            {
                _oWord.Save(NewFile, SaveFormat.Doc);
            }
            catch
            {
                iResult = -2;
            }
            return iResult;
        }
        #endregion

        #region MailMerge Utils
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InputTable"></param>
        public static string ExportToWord(DataSet ds, string templatepath, string outputPath)
        {

            ExportHelper.Open(templatepath);
            ExportHelper.MergeField(ds.Tables[0]);
            ExportHelper.ExecuteWithRegions(ds.Tables[0]);
            ExportHelper.Save(outputPath);
            return outputPath;

        }
        public static void MergeFieldByReplace(DataTable InputTable)
        {
            try
            {
                string m_Colname = "";
                string m_FieldValue = "";
                foreach (DataRow row in InputTable.Rows)
                {
                    for (int i = 0; i <= InputTable.Columns.Count - 1; i++)
                    {
                        m_Colname = InputTable.Columns[i].ColumnName.ToUpper();
                        m_FieldValue = row[m_Colname].ToString();
                        m_FieldValue = m_FieldValue.Replace("\r\n", "XXXXXX");
                        m_FieldValue = m_FieldValue.Replace("\n\r", "XXXXXX");
                        m_FieldValue = m_FieldValue.Replace("\n", "XXXXXX");
                        m_FieldValue = m_FieldValue.Replace("\r", "XXXXXX");
                        _oWord.Range.Replace("<<" + m_Colname + ">>", m_FieldValue, false, false);
                    }
                }

                _oWord.Range.Replace("XXXXXX", Aspose.Words.ControlChar.LineBreakChar.ToString(), false, false);
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }
        /// <summary>
        /// Replace merge field of the specified name with image.
        /// </summary>
        /// <param name=”mergefieldName”>Name of the merge field, where the image should be inserted.
        /// The merge field itself is destroyed in the process.
        /// If the merge field name is prefixed (e.g. “Image:Image1″),
        /// then the name should be specified without prefix (e.g. “Image1″).</param>
        /// <param name=”imageFileName”>fully qualified name image file.
        /// <param name=”builder”>DocumentBuilder.</param>
        /// <returns>Returns true, if the merge field with the specified name was found in the document,
        /// false – if no such filed exists in the document.</returns>
        private static bool InsertImage(string mergeFieldName, string imageFileName)
        {
            DocumentBuilder builder = new DocumentBuilder(_oWord);
            // Move builder to merge field (merge field is automatically removed).
            if (builder.MoveToMergeField(mergeFieldName))
            {
                // No image resize by default.
                // (Setting size to negative values makes image to be inserted without resizing.) 
                double width = -1;
                double height = -1;
                // Check if the image is inserted into table cell.
                Cell cell = (Cell)builder.CurrentParagraph.GetAncestor(typeof(Aspose.Words.Cell));
                if (cell != null)
                {
                    // Set the cell properties so that the inserted image could occupy the cell space exactly.
                    cell.CellFormat.LeftPadding = 0;
                    cell.CellFormat.RightPadding = 0;
                    cell.CellFormat.TopPadding = 0;
                    cell.CellFormat.BottomPadding = 0;
                    cell.CellFormat.WrapText = false;
                    cell.CellFormat.FitText = true;
                    // Get cell dimensions.
                    width = cell.CellFormat.Width;
                    height = cell.ParentRow.RowFormat.Height;
                }
                // Check if the image is inserted into a textbox.                
                Shape shape = (Shape)builder.CurrentParagraph.GetAncestor(typeof(Aspose.Words.Drawing.Shape));
                if ((shape != null) && (shape.ShapeType == ShapeType.TextBox))
                {
                    // Set the textbox properties so that the inserted image could occupy the textbox space exactly.
                    shape.TextBox.InternalMarginTop = 0;
                    shape.TextBox.InternalMarginLeft = 0;
                    shape.TextBox.InternalMarginBottom = 0;
                    shape.TextBox.InternalMarginRight = 0;
                    // Get cell dimensions.
                    width = shape.Width;
                    height = shape.Height;
                }
                // Insert image with or without rescaling, based on the previously done analysis.
                builder.InsertImage(imageFileName, width, height);
                // Signal the caller that the image was succesfully inserted at merge field position.
                return true;
            }
            else
            {
                // Signal the caller that no merge field with the specified name could be found in the document.
                return false;
            }

        }

        public static void MergeField(DataTable InputTable)
        {
            try
            {
                string m_Colname = "";
                string m_FieldValue = "";
                foreach (DataRow row in InputTable.Rows)
                {
                    for (int i = 0; i <= InputTable.Columns.Count - 1; i++)
                    {
                        m_Colname = InputTable.Columns[i].ColumnName;
                        m_FieldValue = row[m_Colname].ToString();
                        m_FieldValue = m_FieldValue.Replace("\r\n", "XXXXXX");
                        m_FieldValue = m_FieldValue.Replace("\n\r", "XXXXXX");
                        m_FieldValue = m_FieldValue.Replace("\n", "XXXXXX");
                        m_FieldValue = m_FieldValue.Replace("\r", "XXXXXX");
                        row[m_Colname] = m_FieldValue;
                    }
                }
                _oWord.MailMerge.Execute(InputTable);
                _oWord.Range.Replace("XXXXXX", Aspose.Words.ControlChar.LineBreakChar.ToString(), false, false);
            }
            catch (Exception exc)
            {
                //throw exc;
            }
        }

        public static void MergeFieldWithImageRescaling(DataTable InputTable, string ImageFieldName)
        {
            try
            {
                string m_Colname = "";
                string m_FieldValue = "";
                List<string> LstImagePath = new List<string>();
                foreach (DataRow row in InputTable.Rows)
                {
                    for (int i = 0; i <= InputTable.Columns.Count - 1; i++)
                    {
                        m_Colname = InputTable.Columns[i].ColumnName;
                        m_FieldValue = row[m_Colname].ToString();
                        if (m_Colname != ImageFieldName)
                        {
                            m_FieldValue = m_FieldValue.Replace("\r\n", "XXXXXX");
                            m_FieldValue = m_FieldValue.Replace("\n\r", "XXXXXX");
                            m_FieldValue = m_FieldValue.Replace("\n", "XXXXXX");
                            m_FieldValue = m_FieldValue.Replace("\r", "XXXXXX");
                            row[m_Colname] = m_FieldValue;
                            _oWord.Range.Replace("[" + m_Colname + "]", m_FieldValue, false, false);
                        }
                        else
                        {
                            if (m_FieldValue[m_FieldValue.Length - 1].ToString() == "/")
                            {
                                DocumentBuilder builder = new DocumentBuilder(_oWord);
                                builder.MoveToMergeField(ImageFieldName);
                                builder.Write("Hình 3 x 4");
                            }
                            else
                            {
                                InsertImage(ImageFieldName, m_FieldValue);
                            }

                        }

                    }
                }
                _oWord.Range.Replace("XXXXXX", Aspose.Words.ControlChar.LineBreakChar.ToString(), false, false);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public static void MergeTableFieldByReplace(DataTable InputTable)
        {
            string m_Colname = "";
            string m_FieldValue = "";
            int i;

            try
            {

                // Get column name
                m_Colname = InputTable.Columns[0].ColumnName.ToUpper();

                // Get data
                for (i = 0; i <= InputTable.Rows.Count - 2; i++)
                {
                    m_FieldValue += InputTable.Rows[i][0].ToString().Replace("\r\n", "XXXXXX") + "@";
                }
                if (InputTable.Rows.Count > 0)
                {
                    m_FieldValue += InputTable.Rows[i][0].ToString().Replace("\r\n", "XXXXXX");
                }

                m_FieldValue = m_FieldValue.Replace("\r", "XXXXXX");

                // Replace


                _oWord.Range.Replace("<<" + m_Colname + ">>", m_FieldValue, false, false);

                _oWord.Range.Replace("@", Aspose.Words.ControlChar.LineBreak, false, false);

                _oWord.Range.Replace("XXXXXX", Aspose.Words.ControlChar.LineBreak, false, false);

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public static void MergeTableFieldParagraphFormat(DataTable InputTable, double intLenght)
        {
            try
            {
                DocumentBuilder _oBWord = new DocumentBuilder(_oWord);
                string m_Colname = "";
                string m_FieldValue = "";
                foreach (DataRow row in InputTable.Rows)
                {
                    for (int i = 0; i <= InputTable.Columns.Count - 1; i++)
                    {
                        m_Colname = InputTable.Columns[i].ColumnName.ToUpper();
                        m_FieldValue = row[m_Colname].ToString();
                        m_FieldValue = m_FieldValue.Replace("\r\n", "XXXXXX");
                        m_FieldValue = m_FieldValue.Replace("\n\r", "XXXXXX");
                        m_FieldValue = m_FieldValue.Replace("\n", "XXXXXX");
                        m_FieldValue = m_FieldValue.Replace("\r", "XXXXXX");
                        _oBWord.Document.Range.Replace("<<" + m_Colname + ">>", m_FieldValue, false, false);
                    }
                }
                _oBWord.ParagraphFormat.Style.ParagraphFormat.SpaceBefore = intLenght;
                _oBWord.Document.Range.Replace("XXXXXX", Aspose.Words.ControlChar.LineBreakChar.ToString(), false, false);

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="InputDataSet"></param>
        public static void ExecuteWithRegions(DataSet InputDataSet)
        {
            ExecuteWithRegions(InputDataSet.Tables[0]);
            return;
            string m_Colname = "";
            string m_FieldValue = "";
            foreach (DataTable table in InputDataSet.Tables)
            {
                if (table.TableName != null && table.TableName != "")
                {
                    foreach (DataRow row in table.Rows)
                    {
                        for (int i = 0; i <= table.Columns.Count - 1; i++)
                        {
                            m_Colname = table.Columns[i].ColumnName;
                            m_FieldValue = row[m_Colname].ToString();
                            m_FieldValue = m_FieldValue.Replace("\r\n", "XXXXXX");
                            m_FieldValue = m_FieldValue.Replace("\n\r", "XXXXXX");
                            m_FieldValue = m_FieldValue.Replace("\n", "XXXXXX");
                            m_FieldValue = m_FieldValue.Replace("\r", "XXXXXX");
                            row[m_Colname] = m_FieldValue;
                        }
                    }
                    _oWord.MailMerge.ExecuteWithRegions(table);
                }
            }
            _oWord.Range.Replace("XXXXXX", Aspose.Words.ControlChar.LineBreakChar.ToString(), false, false);

        }
        public static void ExecuteWithImageRegions(DataSet InputDataSet, string serverpath)
        {
            string m_Colname = "";
            string m_FieldValue = "";
            DataTable tbl = new DataTable();
            DocumentBuilder builder = new DocumentBuilder(_oWord);
            tbl.Columns.Add("Image1");
            tbl.Columns.Add("Image2");

            foreach (DataTable table in InputDataSet.Tables)
            {
                if (table.TableName != null && table.TableName != "")
                {
                    foreach (DataRow row in table.Rows)
                    {
                        DataRow dr = tbl.NewRow();
                        for (int i = 0; i <= table.Columns.Count - 1; i++)
                        {
                            m_Colname = table.Columns[i].ColumnName;
                            m_FieldValue = row[m_Colname].ToString();
                            if (m_Colname == "ImagePath1")
                            {
                                dr["Image1"] = m_FieldValue;
                            }
                            if (m_Colname == "ImagePath2")
                            {
                                dr["Image2"] = m_FieldValue;
                            }
                        }
                        tbl.Rows.Add(dr);
                    }
                    _oWord.MailMerge.ExecuteWithRegions(table);
                }
            }

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                if (tbl.Rows[i][0].ToString() != "" && File.Exists(serverpath + tbl.Rows[i][0].ToString()))
                {
                    InsertImage("Image1", serverpath + tbl.Rows[i][0].ToString());
                }
                else
                {
                    builder.MoveToMergeField("Image1");
                    builder.Write("Hình 3 x 4");
                }

                if (tbl.Rows[i][1].ToString() != "" && File.Exists(serverpath + tbl.Rows[i][1].ToString()))
                {
                    InsertImage("Image2", serverpath + tbl.Rows[i][1].ToString());
                }
                else
                {
                    builder.MoveToMergeField("Image2");
                    builder.Write("Hình 3 x 4");
                }

            }


        }

        public static void ExecuteWithOnceImageRegions(DataSet InputDataSet, string serverpath)
        {
            string m_Colname = "";
            string m_FieldValue = "";
            DataTable tbl = new DataTable();
            if (_oWord == null)
                return;
            DocumentBuilder builder = new DocumentBuilder(_oWord);
            tbl.Columns.Add("Image1");

            foreach (DataTable table in InputDataSet.Tables)
            {
                if (table.TableName != null && table.TableName != "")
                {
                    foreach (DataRow row in table.Rows)
                    {
                        DataRow dr = tbl.NewRow();
                        for (int i = 0; i <= table.Columns.Count - 1; i++)
                        {
                            m_Colname = table.Columns[i].ColumnName;
                            m_FieldValue = row[m_Colname].ToString();
                            if (m_Colname == "ImagePath")
                            {
                                dr["Image1"] = m_FieldValue;
                            }

                        }
                        tbl.Rows.Add(dr);
                    }
                    _oWord.MailMerge.ExecuteWithRegions(table);
                }
            }

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                if (tbl.Rows[i][0].ToString() != "" && File.Exists(serverpath + tbl.Rows[i][0].ToString()))
                {
                    InsertImage("Image1", serverpath + tbl.Rows[i][0].ToString());
                }
                else
                {
                    builder.MoveToMergeField("Image1");
                    // builder.Write("");
                    // builder.Write("Hình 3 x 4");
                }

            }


        }

        public static string Export(DataTable dt, string templatePath, ref string outputpath)
        {
            Open(templatePath);
            ExecuteWithRegions(dt);
            try
            {
                MergeFieldByReplace(dt);
            }
            catch
            {
            }
            Save(outputpath);
            return outputpath;
        }

     
        public static void ExecuteWithRegions(DataTable dataTable)
        {
            if (_oWord != null)
            {
                _oWord.MailMerge.ExecuteWithRegions(dataTable);

            }
        }
        #endregion

        #endregion
    }
}
