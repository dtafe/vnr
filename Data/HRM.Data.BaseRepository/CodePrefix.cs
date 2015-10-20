using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Linq;
using VnResource.Helper.Data;

namespace HRM.Data.BaseRepository
{
    public class CodePrefix
    {
        #region CodePrefix

        private int startIndex = 0;
        private int characterNumbers = 0;
        private bool allowZero = true;
        private bool fromLast = false;
        private IUnitOfWork unitOfWork;
        private bool isResetByDate = false;
        private bool isResetByMonth = false;
        private bool isResetByYear = false;
        private bool isResetByUser = false;
        private bool isResetByObject = false;
        private string objectTabledName = string.Empty;
        private string objectFieldName = string.Empty;
        private string resetObjectName = string.Empty;
        private string resetFieldName = string.Empty;
        private string resetObjectName1 = string.Empty;
        private string resetFieldName1 = string.Empty;
        private Guid _userLoginID = Guid.Empty;
        private Guid _codeObjectID = Guid.Empty;
        private Sys_CodeObject _codeObject = null;

        private string strText = string.Empty;
        private FunctionTypes functionType = FunctionTypes.TEXT;

        public enum FunctionTypes
        {
            TEXT = 0, DATE, ORDINAL, OBJECT
        }

        internal CodePrefix(Guid userLoginID, Sys_CodeObject codeObject)
        {
            _userLoginID = userLoginID;
            _codeObjectID = codeObject.ID;
            _codeObject = codeObject;
        }
        public FunctionTypes FunctionType
        {
            get
            {
                return functionType;
            }
        }

        protected bool IsResetByTime
        {
            get
            {
                return isResetByDate || isResetByMonth || isResetByYear;
            }
        }

        protected string ResetFieldName1
        {
            get
            {
                if (resetFieldName1 == null || string.IsNullOrEmpty(resetFieldName1.Trim()))
                {
                    if (ListResetObjectNames1 != null && ListResetObjectNames1.Count() > 0)
                    {
                        resetFieldName1 = Constant.ID;
                    }
                }

                return resetFieldName1;
            }
        }

        protected string[] ListResetObjectNames1
        {
            get
            {
                if (!string.IsNullOrEmpty(resetObjectName1))
                    return resetObjectName1.Split('.');
                else return null;
            }
        }

        protected string ResetFieldName
        {
            get
            {
                if (resetFieldName == null || string.IsNullOrEmpty(resetFieldName.Trim()))
                {
                    if (ListResetObjectNames != null && ListResetObjectNames.Count() > 0)
                    {
                        resetFieldName = Constant.ID;
                    }
                }

                return resetFieldName;
            }
        }

        protected string[] ListResetObjectNames
        {
            get
            {
                if (!string.IsNullOrEmpty(resetObjectName))
                    return resetObjectName.Split('.');
                else return null;
            }
        }

        protected string[] ListObjectTableNames
        {
            get
            {
                if (!string.IsNullOrEmpty(objectTabledName))
                    return objectTabledName.Split('.');
                else return null;
            }
        }

        protected string TimeFormat
        {
            get
            {
                if (isResetByDate)
                    return "ddMMyyyy";
                if (isResetByMonth)
                    return "MMyyyy";
                if (isResetByYear)
                    return "yyyy";
                return "ddMMyyyy";
            }
        }

        protected bool isDebugMode
        {
            get
            {
#if(DEBUG)
                return true;
#else
                return false;
#endif
            }
        }

        internal string GetString(object entityBase, Sys_CodeObject codeObject, DateTime date)
        {
            //var repoUser = new CustomBaseRepository<Sys_UserInfo>(unitOfWork);
            var repoObjDate = new CustomBaseRepository<Sys_CodeObjectByUserNDate>(unitOfWork);

            switch (functionType)
            {
                case FunctionTypes.DATE:
                    return date.ToString(strText);
                case FunctionTypes.OBJECT:
                    string resultObject = string.Empty;
                    if (entityBase != null)
                    {
                        object objectFieldValue = null;
                        object entityObject = null;

                        if (ListObjectTableNames != null && ListObjectTableNames.Count() > 0)
                        {
                            foreach (string resetName in ListObjectTableNames)
                            {
                                if (entityBase.HasProperty(resetName) && entityBase.HasProperty("ID"))
                                {
                                    unitOfWork.Context.Entry(entityBase).Reference(resetName).Load();
                                }

                                entityObject = entityBase.GetPropertyValue(resetName);
                                var entityID = entityObject.GetPropertyValue("ID");
                                if (entityObject != null && entityID.ToString() != Guid.Empty.ToString())
                                {
                                    objectFieldValue = entityObject.GetPropertyValue(objectFieldName);
                                }
                            }
                        }
                        else
                        {
                            objectFieldValue = entityBase.GetPropertyValue(objectFieldName);
                        }

                        if (objectFieldValue != null)
                        {
                            resultObject = GetNumberCharater(objectFieldValue.ToString());
                        }
                    }
                    return resultObject;
                case FunctionTypes.ORDINAL:
                    Sys_CodeObjectByUserNDate codeObjectByUserNDate = null;
                    if (isResetByUser || isResetByObject)
                    {
                        object objectFieldValue = null;
                        object objectFieldValue1 = null;

                        if (entityBase != null && isResetByObject)
                        {
                            if (ListResetObjectNames != null && ListResetObjectNames.Count() > 0)
                            {
                                object entityObject = null;

                                foreach (string resetName in ListResetObjectNames)
                                {
                                    if (entityBase.HasProperty(resetName) && entityBase.HasProperty("ID"))
                                    {
                                        unitOfWork.Context.Entry(entityBase).Reference(resetName).Load();
                                    }
                                    entityObject = entityBase.GetPropertyValue(resetName);
                                    var entityID = entityObject.GetPropertyValue("ID");

                                    if (entityObject != null && entityID.ToString() != Guid.Empty.ToString())
                                    {
                                        objectFieldValue = entityObject.GetPropertyValue(ResetFieldName);
                                    }
                                }
                            }
                            else if (!string.IsNullOrWhiteSpace(ResetFieldName))
                            {
                                objectFieldValue = entityBase.GetPropertyValue(ResetFieldName);
                            }

                            if (ListResetObjectNames1 != null && ListResetObjectNames1.Count() > 0)
                            {
                                object entityObject = null;

                                foreach (string resetName in ListResetObjectNames1)
                                {
                                    entityObject = entityBase.GetPropertyValue(resetName);
                                    var entityID = entityObject.GetPropertyValue("ID");
                                    if (entityObject != null && entityID.ToString() != Guid.Empty.ToString())
                                    {
                                        objectFieldValue1 = entityObject.GetPropertyValue(ResetFieldName1);
                                    }
                                }
                            }
                            else if (!string.IsNullOrWhiteSpace(ResetFieldName1))
                            {
                                objectFieldValue1 = entityBase.GetPropertyValue(ResetFieldName1);
                            }
                        }

                        //ID đối tượng reset theo người dùng hoặc theo đối tượng bất kì
                        objectFieldValue = objectFieldValue != null ? objectFieldValue : string.Empty;
                        objectFieldValue1 = objectFieldValue1 != null ? objectFieldValue1 : string.Empty;

                        string resetObjectID = objectFieldValue.ToString();
                        string resetObjectID1 = objectFieldValue1.ToString();

                        if (isResetByUser)
                        {
                            resetObjectID = _userLoginID.ToString();
                            resetObjectID1 = string.Empty;
                        }

                        if ((string.IsNullOrEmpty(resetObjectID.Trim()) || resetObjectID == Guid.Empty.ToString())
                            && (string.IsNullOrEmpty(resetObjectID1.Trim()) || resetObjectID1 == Guid.Empty.ToString()))
                        {
                            return string.Empty;
                        }

                        if (IsResetByTime)
                        {
                            codeObjectByUserNDate = repoObjDate.FindBy(cobund => cobund.CodeObjectID == _codeObjectID && cobund.ResetObjectID == resetObjectID &&
                                ((cobund.ResetObjectID1 == null && resetObjectID1 == "") || cobund.ResetObjectID1 == resetObjectID1)
                                    && cobund.Date != null).ToList().Where(cobund => cobund.Date.Value.ToString(TimeFormat) == date.ToString(TimeFormat)).FirstOrDefault();
                        }
                        else
                        {
                            codeObjectByUserNDate = repoObjDate.FindBy(cobund => cobund.CodeObjectID == _codeObjectID && cobund.ResetObjectID == resetObjectID
                               && ((cobund.ResetObjectID1 == null && resetObjectID1 == "") || cobund.ResetObjectID1 == resetObjectID1)).FirstOrDefault();
                        }

                        if (codeObjectByUserNDate == null)
                        {
                            codeObjectByUserNDate = new Sys_CodeObjectByUserNDate();
                            codeObjectByUserNDate.ID = Guid.NewGuid();
                            codeObjectByUserNDate.CodeObjectID = _codeObjectID;
                            codeObjectByUserNDate.ResetObjectID = resetObjectID;
                            codeObjectByUserNDate.ResetObjectID1 = resetObjectID1;
                            codeObjectByUserNDate.Ordinal = 1;

                            if (IsResetByTime)
                            {
                                codeObjectByUserNDate.Date = date;
                            }
                            repoObjDate.Add(codeObjectByUserNDate);
                        }
                    }
                    else if (IsResetByTime)
                    {
                        codeObjectByUserNDate = repoObjDate.FindBy(cobund => cobund.CodeObjectID == _codeObjectID && cobund.Date != null).ToList().Where(cobund =>
                            cobund.Date.Value.ToString(TimeFormat) == date.ToString(TimeFormat)).FirstOrDefault();

                        if (codeObjectByUserNDate == null)
                        {
                            codeObjectByUserNDate = new Sys_CodeObjectByUserNDate();
                            codeObjectByUserNDate.ID = Guid.NewGuid();
                            codeObjectByUserNDate.CodeObjectID = _codeObjectID;
                            codeObjectByUserNDate.Ordinal = 1;
                            codeObjectByUserNDate.Date = date;
                            repoObjDate.Add(codeObjectByUserNDate);
                        }
                    }

                    string result = string.Empty;
                    if (codeObjectByUserNDate != null)
                    {
                        int ordinal = codeObjectByUserNDate.Ordinal.GetInteger();
                        result = GetNumberCharater(ordinal.ToString());
                        codeObjectByUserNDate.Ordinal += 1;
                    }
                    else
                    {
                        int ordinal = _codeObject.Ordinal.GetInteger();
                        result = GetNumberCharater(ordinal.ToString());
                    }
                    return result;

                case FunctionTypes.TEXT:
                default:
                    return strText;
            }
        }

        private string GetNumberCharater(string p)
        {
            string result = string.Empty;

            if (characterNumbers < 0)
                return p;

            var index = startIndex;

            if (fromLast)
            {
                index = p.Length - startIndex - characterNumbers;
                index = index < 0 ? 0 : index;
            }

            if (index < 0 || index > p.Length)
            {
                p = string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(p))
            {
                var subString = p.Substring(index);

                if (characterNumbers >= subString.Length)
                {
                    result = subString;
                }
                else if (characterNumbers == 0)
                {
                    result = p.Substring(index, p.Length - index);
                }
                else
                {
                    result = p.Substring(index, characterNumbers);
                }
            }

            if (allowZero)
            {
                result = result.PadLeft(characterNumbers, '0');
            }

            return result;
        }

        static internal CodePrefix FromExpression(string expression, IUnitOfWork unitOfWork, Guid userLoginID, Sys_CodeObject codeObject)
        {
            expression = expression.Trim();
            try
            {
                CodePrefix result = new CodePrefix(userLoginID, codeObject);
                result.unitOfWork = unitOfWork;
                result.isResetByDate = codeObject.IsResetByDate != null ? codeObject.IsResetByDate.Value : false;
                result.isResetByMonth = codeObject.IsResetByMonth != null ? codeObject.IsResetByMonth.Value : false;
                result.isResetByYear = codeObject.IsResetByYear != null ? codeObject.IsResetByYear.Value : false;
                result.isResetByUser = codeObject.IsResetByUser != null ? codeObject.IsResetByUser.Value : false;
                result.isResetByObject = codeObject.IsResetByObject != null ? codeObject.IsResetByObject.Value : false;
                result.resetObjectName = result.isResetByObject ? codeObject.ResetObjectName : string.Empty;
                result.resetFieldName = result.isResetByObject ? codeObject.ResetFieldName : string.Empty;
                result.resetObjectName1 = result.isResetByObject ? codeObject.ResetObjectName1 : string.Empty;
                result.resetFieldName1 = result.isResetByObject ? codeObject.ResetFieldName1 : string.Empty;

                if (result.resetFieldName == null || string.IsNullOrEmpty(result.resetFieldName.Trim()))
                {
                    if (!string.IsNullOrWhiteSpace(codeObject.ResetObjectName))
                    {
                        result.resetFieldName = Constant.ID;
                    }
                }

                if (result.resetFieldName1 == null || string.IsNullOrEmpty(result.resetFieldName1.Trim()))
                {
                    if (!string.IsNullOrWhiteSpace(codeObject.ResetObjectName1))
                    {
                        result.resetFieldName1 = Constant.ID;
                    }
                }

                string function = GetFunction(expression);

                try
                {
                    result.functionType = Common.GetEnumValue<FunctionTypes>(function.Trim().ToUpper());
                }
                catch
                {
                }

                switch (result.functionType)
                {
                    case FunctionTypes.DATE:
                        result.GetDateFormat(expression);
                        break;
                    case FunctionTypes.ORDINAL:
                    case FunctionTypes.OBJECT:
                        result.GetNumber(expression);
                        break;
                    case FunctionTypes.TEXT:
                    default:
                        result.strText = function;
                        break;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new VNRException(ExceptionType.FRAMEWORK, "Wrong expression syntax!", e);
            }
        }

        private void GetNumber(string expression)
        {
            expression = expression.Trim();
            if (expression == functionType.ToString())
            {
                characterNumbers = -1;
                return;
            }
            if (!expression.ToUpper().StartsWith(functionType.ToString() + "("))
                throw new VNRException(ExceptionType.FRAMEWORK, "Can't get number!");
            int index = expression.IndexOf(')');
            if (index == -1)
                throw new VNRException(ExceptionType.FRAMEWORK, "Can't get number!");
            string result = expression.Substring(functionType.ToString().Length + 1, index - functionType.ToString().Length - 1);
            try
            {
                string[] splits = result.Split(',');

                if (functionType == FunctionTypes.OBJECT)
                {
                    if (splits.Count() >= 6)
                    {
                        //Đủ 6 tham số (table, field, startIndex, length, allowZero, fromlast)
                        objectTabledName = splits[0].Trim();
                        objectFieldName = splits[1].Trim();
                        startIndex = int.Parse(splits[2].Trim());
                        characterNumbers = int.Parse(splits[3].Trim());
                        allowZero = int.Parse(splits[4].Trim()) == 1;
                        fromLast = int.Parse(splits[5].Trim()) == 1;
                    }
                    else if (splits.Count() >= 5)
                    {
                        objectTabledName = splits[0].Trim();
                        objectFieldName = splits[1].Trim();
                        startIndex = int.Parse(splits[2].Trim());
                        characterNumbers = int.Parse(splits[3].Trim());
                        allowZero = int.Parse(splits[4].Trim()) == 1;
                    }
                    else if (splits.Count() >= 4)
                    {
                        objectTabledName = splits[0].Trim();
                        objectFieldName = splits[1].Trim();
                        startIndex = int.Parse(splits[2].Trim());
                        characterNumbers = int.Parse(splits[3].Trim());
                    }
                }
                else
                {
                    if (splits.Count() > 0)
                        characterNumbers = int.Parse(splits[0].Trim());
                    if (splits.Count() > 1)
                        allowZero = int.Parse(splits[1].Trim()) == 1;
                    if (splits.Count() > 2)
                        fromLast = int.Parse(splits[2].Trim()) == 1;
                }
            }
            catch
            {
                throw new VNRException(ExceptionType.FRAMEWORK, "Can't get number!");
            }
        }

        private void GetDateFormat(string expression)
        {
            expression = expression.Trim();
            if (expression.ToUpper() == "DATE")
            {
                strText = "yyyyMMdd";
                return;
            }
            if (!expression.ToUpper().StartsWith("DATE("))
                throw new VNRException(ExceptionType.FRAMEWORK, "Can't get datetime format!");
            int index = expression.IndexOf(')');
            if (index == -1)
                throw new VNRException(ExceptionType.FRAMEWORK, "Can't get datetime format!");
            strText = expression.Substring(5, index - 5);
            try
            {
                DateTime.Now.ToString(strText);
            }
            catch
            {
                throw new VNRException(ExceptionType.FRAMEWORK, "Can't get datetime format!");
            }
        }

        private static string GetFunction(string expression)
        {
            int index = expression.IndexOf('(');
            if (index == -1)
                return expression;
            return expression.Substring(0, index);
        }
        #endregion
    }
}
