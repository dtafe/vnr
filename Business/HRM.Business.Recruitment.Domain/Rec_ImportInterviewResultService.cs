using HRM.Business.Main.Domain;
using HRM.Business.Recruitment.Models;
using HRM.Data.BaseRepository;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnResource.Helper.Ado;
using VnResource.Helper.Data;
using VnResource.Helper.Linq;
using VnResource.Helper.Utility;
using VnResource.Importer;

namespace HRM.Business.Recruitment.Domain
{
    public class Rec_ImportInterviewResultService : BaseService
    {
        public Guid UserID { get; set; }
        public string FileName { get; set; }

        public static Dictionary<Guid, DataTable> importObjects;
        public static Dictionary<Guid, DataTable> invalidObjects;
        private Dictionary<string, string> fieldMappings;
        private List<string> listInvisibleField;

        public List<string> ListInvisibleField
        {
            get
            {
                if (listInvisibleField == null)
                {
                    listInvisibleField = new List<string>();
                }

                return listInvisibleField;
            }
        }

        public static Dictionary<Guid, DataTable> ImportObjects
        {
            get
            {
                if (importObjects == null)
                {
                    importObjects = new Dictionary<Guid, DataTable>();
                }

                return importObjects;
            }
        }

        public static Dictionary<Guid, DataTable> InvalidObjects
        {
            get
            {
                if (invalidObjects == null)
                {
                    invalidObjects = new Dictionary<Guid, DataTable>();
                }

                return invalidObjects;
            }
        }

        public Dictionary<string, string> FieldMappings
        {
            get
            {
                if (fieldMappings == null)
                {
                    fieldMappings = new Dictionary<string, string>();

                    fieldMappings.Add("CandidateCode", "A");
                    fieldMappings.Add("CandidateName", "B");
                    fieldMappings.Add("JobVacancyCode", "C");
                    fieldMappings.Add("CandidateEmail", "D");
                    fieldMappings.Add("LanguageCode", "E");

                    fieldMappings.Add("Group1Score1", "F");
                    fieldMappings.Add("Group1Score2", "G");
                    fieldMappings.Add("Group1Score3", "H");
                    fieldMappings.Add("GroupCondition1", "I");
                    fieldMappings.Add("GroupResult1", "J");

                    fieldMappings.Add("Group2Score1", "K");
                    fieldMappings.Add("Group2Score2", "L");
                    fieldMappings.Add("Group2Score3", "M");
                    fieldMappings.Add("GroupCondition2", "N");
                    fieldMappings.Add("GroupResult2", "O");

                    fieldMappings.Add("Group3Score1", "P");
                    fieldMappings.Add("Group3Score2", "Q");
                    fieldMappings.Add("Group3Score3", "R");
                    fieldMappings.Add("GroupCondition3", "S");
                    fieldMappings.Add("GroupResult3", "T");

                    fieldMappings.Add("Group4Score1", "U");
                    fieldMappings.Add("Group4Score2", "V");
                    fieldMappings.Add("Group4Score3", "W");
                    fieldMappings.Add("GroupCondition4", "X");
                    fieldMappings.Add("GroupResult4", "Y");

                    fieldMappings.Add("Group5Score1", "Z");
                    fieldMappings.Add("Group5Score2", "AA");
                    fieldMappings.Add("Group5Score3", "AB");
                    fieldMappings.Add("GroupCondition5", "AC");
                    fieldMappings.Add("GroupResult5", "AD");

                    fieldMappings.Add("Group6Score1", "AE");
                    fieldMappings.Add("Group6Score2", "AF");
                    fieldMappings.Add("Group6Score3", "AG");
                    fieldMappings.Add("GroupCondition6", "AH");
                    fieldMappings.Add("GroupResult6", "AI");

                    fieldMappings.Add("Group7Score1", "AJ");
                    fieldMappings.Add("Group7Score2", "AK");
                    fieldMappings.Add("Group7Score3", "AL");
                    fieldMappings.Add("GroupCondition7", "AM");
                    fieldMappings.Add("GroupResult7", "AN");
                }

                return fieldMappings;
            }
        }

        public DataTable GetImportObject()
        {
            if (ImportObjects.ContainsKey(UserID))
            {
                return ImportObjects[UserID];
            }

            return new DataTable("");
        }

        public DataTable GetInvalidObject()
        {
            if (InvalidObjects.ContainsKey(UserID))
            {
                return InvalidObjects[UserID];
            }

            return new DataTable("");
        }

        public void ImportInterviewResult()
        {
            if (!string.IsNullOrWhiteSpace(FileName))
            {
                using (var context = new VnrHrmDataContext())
                {
                    IUnitOfWork unitOfWork = new UnitOfWork(context);
                    ExcelImporter importer = new ExcelImporter(context);
                    importer.FileName = FileName;

                    var objectType = typeof(Rec_ImportInterviewResultEntity);
                    List<ImportItemInfo> importTemplateItems = new List<ImportItemInfo>();

                    importTemplateItems.Add(CreateImportItemInfo(0, false, true, 1, "CandidateCode", string.Empty, FieldMappings["CandidateCode"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(1, true, true, 1, "CandidateName", string.Empty, FieldMappings["CandidateName"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(2, false, true, 1, "JobVacancyCode", string.Empty, FieldMappings["JobVacancyCode"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(3, true, true, 1, "CandidateEmail", string.Empty, FieldMappings["CandidateEmail"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(4, true, true, 1, "LanguageCode", string.Empty, FieldMappings["LanguageCode"], string.Empty));

                    importTemplateItems.Add(CreateImportItemInfo(5, true, true, 1, "Group1Score1", string.Empty, FieldMappings["Group1Score1"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(6, true, true, 1, "Group1Score2", string.Empty, FieldMappings["Group1Score2"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(7, true, true, 1, "Group1Score3", string.Empty, FieldMappings["Group1Score3"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(8, false, true, 1, "GroupCondition1", string.Empty, FieldMappings["GroupCondition1"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(9, false, true, 1, "GroupResult1", string.Empty, FieldMappings["GroupResult1"], string.Empty));

                    importTemplateItems.Add(CreateImportItemInfo(10, true, true, 1, "Group2Score1", string.Empty, FieldMappings["Group2Score1"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(11, true, true, 1, "Group2Score2", string.Empty, FieldMappings["Group2Score2"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(12, true, true, 1, "Group2Score3", string.Empty, FieldMappings["Group2Score3"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(13, false, true, 1, "GroupCondition2", string.Empty, FieldMappings["GroupCondition2"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(14, false, true, 1, "GroupResult2", string.Empty, FieldMappings["GroupResult2"], string.Empty));

                    importTemplateItems.Add(CreateImportItemInfo(15, true, true, 1, "Group3Score1", string.Empty, FieldMappings["Group3Score1"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(16, true, true, 1, "Group3Score2", string.Empty, FieldMappings["Group3Score2"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(17, true, true, 1, "Group3Score3", string.Empty, FieldMappings["Group3Score3"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(18, false, true, 1, "GroupCondition3", string.Empty, FieldMappings["GroupCondition3"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(19, false, true, 1, "GroupResult3", string.Empty, FieldMappings["GroupResult3"], string.Empty));

                    importTemplateItems.Add(CreateImportItemInfo(20, true, true, 1, "Group4Score1", string.Empty, FieldMappings["Group4Score1"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(21, true, true, 1, "Group4Score2", string.Empty, FieldMappings["Group4Score2"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(22, true, true, 1, "Group4Score3", string.Empty, FieldMappings["Group4Score3"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(23, false, true, 1, "GroupCondition4", string.Empty, FieldMappings["GroupCondition4"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(24, false, true, 1, "GroupResult4", string.Empty, FieldMappings["GroupResult4"], string.Empty));

                    importTemplateItems.Add(CreateImportItemInfo(25, false, true, 1, "GroupCondition5", string.Empty, FieldMappings["GroupCondition5"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(26, true, true, 1, "Group5Score1", string.Empty, FieldMappings["Group5Score1"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(27, true, true, 1, "Group5Score2", string.Empty, FieldMappings["Group5Score2"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(28, true, true, 1, "Group5Score3", string.Empty, FieldMappings["Group5Score3"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(29, false, true, 1, "GroupResult5", string.Empty, FieldMappings["GroupResult5"], string.Empty));

                    importTemplateItems.Add(CreateImportItemInfo(30, true, true, 1, "Group6Score1", string.Empty, FieldMappings["Group6Score1"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(31, true, true, 1, "Group6Score2", string.Empty, FieldMappings["Group6Score2"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(32, true, true, 1, "Group6Score3", string.Empty, FieldMappings["Group6Score3"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(33, false, true, 1, "GroupCondition6", string.Empty, FieldMappings["GroupCondition6"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(34, false, true, 1, "GroupResult6", string.Empty, FieldMappings["GroupResult6"], string.Empty));

                    importTemplateItems.Add(CreateImportItemInfo(35, true, true, 1, "Group7Score1", string.Empty, FieldMappings["Group7Score1"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(36, true, true, 1, "Group7Score2", string.Empty, FieldMappings["Group7Score2"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(37, true, true, 1, "Group7Score3", string.Empty, FieldMappings["Group7Score3"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(38, false, true, 1, "GroupCondition7", string.Empty, FieldMappings["GroupCondition7"], string.Empty));
                    importTemplateItems.Add(CreateImportItemInfo(39, false, true, 1, "GroupResult7", string.Empty, FieldMappings["GroupResult7"], string.Empty));

                    var dtImportObject = importer.ReadExcelData(objectType, 0, 4, 0, importTemplateItems);
                    listInvisibleField = importTemplateItems.Where(d => d.IsOutOfRange).Select(d => d.ExcelField1.TrimAll()).ToList();

                    var listJobVacancyCode = dtImportObject.Rows.OfType<DataRow>().Where(d =>
                        !d.IsNull(FieldMappings["JobVacancyCode"])).Select(d => d[FieldMappings["JobVacancyCode"]].GetString()).Distinct().ToList();

                    var listLanguageCode = dtImportObject.Rows.OfType<DataRow>().Where(d =>
                        !d.IsNull(FieldMappings["LanguageCode"])).Select(d => d[FieldMappings["LanguageCode"]].GetString()).Distinct().ToList();

                    var listCandidateCode = dtImportObject.Rows.OfType<DataRow>().Where(d =>
                        !d.IsNull(FieldMappings["CandidateCode"])).Select(d => d[FieldMappings["CandidateCode"]].GetString()).Distinct().ToList();

                    var listGroupConditionCode = dtImportObject.Rows.OfType<DataRow>().Where(d =>
                        !d.IsNull(FieldMappings["GroupCondition1"])).Select(d => d[FieldMappings["GroupCondition1"]].GetString()).Distinct().ToList();

                    listGroupConditionCode.AddRange(dtImportObject.Rows.OfType<DataRow>().Where(d =>
                        !d.IsNull(FieldMappings["GroupCondition2"])).Select(d => d[FieldMappings["GroupCondition2"]].GetString()).Distinct().ToList());

                    listGroupConditionCode.AddRange(dtImportObject.Rows.OfType<DataRow>().Where(d =>
                        !d.IsNull(FieldMappings["GroupCondition3"])).Select(d => d[FieldMappings["GroupCondition3"]].GetString()).Distinct().ToList());

                    listGroupConditionCode.AddRange(dtImportObject.Rows.OfType<DataRow>().Where(d =>
                        !d.IsNull(FieldMappings["GroupCondition4"])).Select(d => d[FieldMappings["GroupCondition4"]].GetString()).Distinct().ToList());

                    listGroupConditionCode.AddRange(dtImportObject.Rows.OfType<DataRow>().Where(d =>
                        !d.IsNull(FieldMappings["GroupCondition5"])).Select(d => d[FieldMappings["GroupCondition5"]].GetString()).Distinct().ToList());

                    listGroupConditionCode.AddRange(dtImportObject.Rows.OfType<DataRow>().Where(d =>
                        !d.IsNull(FieldMappings["GroupCondition6"])).Select(d => d[FieldMappings["GroupCondition6"]].GetString()).Distinct().ToList());

                    listGroupConditionCode.AddRange(dtImportObject.Rows.OfType<DataRow>().Where(d =>
                        !d.IsNull(FieldMappings["GroupCondition7"])).Select(d => d[FieldMappings["GroupCondition7"]].GetString()).Distinct().ToList());

                    var listJobVacancy = new List<Rec_JobVacancy>().Select(d => new
                    {
                        d.ID,
                        d.Code
                    }).ToList();

                    var listLanguage = new List<Cat_NameEntity>().Select(d => new
                    {
                        d.ID,
                        d.Code
                    }).ToList();

                    var listCandidate = new List<Rec_Candidate>().Select(d => new
                    {
                        d.ID,
                        d.CodeCandidate,
                        d.CandidateName
                    }).ToList();

                    var listGroupCondition = new List<Rec_GroupCondition>().Select(d => new
                    {
                        d.ID,
                        d.Code,
                        d.GroupName
                    }).ToList();

                    var listRecruitmentHistory = new List<Rec_RecruitmentHistory>().Select(d => new
                    {
                        d.ID,
                        d.CandidateID,
                        d.DateApply
                    }).ToList();

                    var listInterview = new List<Rec_Interview>().Select(d => new
                    {
                        d.ID,
                        d.CandidateID,
                        d.GroupConditionID,
                        d.RecruitmentHistoryID
                    }).ToList();

                    foreach (var item in listLanguageCode.Chunk(1000))
                    {
                        listLanguage.AddRange(unitOfWork.CreateQueryable<Cat_NameEntity>(d =>
                            item.Contains(d.Code)).Select(d => new
                            {
                                d.ID,
                                d.Code
                            }).ToList());
                    }

                    foreach (var item in listJobVacancyCode.Chunk(1000))
                    {
                        listJobVacancy.AddRange(unitOfWork.CreateQueryable<Rec_JobVacancy>(d =>
                            item.Contains(d.Code)).Select(d => new
                            {
                                d.ID,
                                d.Code
                            }).ToList());
                    }

                    foreach (var item in listCandidateCode.Chunk(1000))
                    {
                        listCandidate.AddRange(unitOfWork.CreateQueryable<Rec_Candidate>(d =>
                            item.Contains(d.CodeCandidate)).Select(d => new
                            {
                                d.ID,
                                d.CodeCandidate,
                                d.CandidateName
                            }).ToList());
                    }

                    foreach (var item in listGroupConditionCode.Chunk(1000))
                    {
                        listGroupCondition.AddRange(unitOfWork.CreateQueryable<Rec_GroupCondition>(d =>
                            item.Contains(d.Code)).Select(d => new
                            {
                                d.ID,
                                d.Code,
                                d.GroupName
                            }).ToList());
                    }

                    var listCandidateID = listCandidate.Select(d => d.ID).Distinct().ToList();

                    foreach (var item in listCandidateID.Chunk(1000))
                    {
                        listRecruitmentHistory.AddRange(unitOfWork.CreateQueryable<Rec_RecruitmentHistory>(d =>
                            item.Contains(d.CandidateID)).Select(d => new
                            {
                                d.ID,
                                d.CandidateID,
                                d.DateApply
                            }).ToList());
                    }

                    foreach (var item in listCandidateID.Chunk(1000))
                    {
                        listInterview.AddRange(unitOfWork.CreateQueryable<Rec_Interview>(d =>
                            d.CandidateID.HasValue && item.Contains(d.CandidateID.Value)).Select(d => new
                            {
                                d.ID,
                                d.CandidateID,
                                d.GroupConditionID,
                                d.RecruitmentHistoryID
                            }).ToList());
                    }

                    dtImportObject.Columns.Add("CandidateID", typeof(Guid));
                    dtImportObject.Columns.Add("JobVacancyID", typeof(Guid));
                    dtImportObject.Columns.Add("LanguageID", typeof(Guid));
                    dtImportObject.Columns.Add("InterviewID", typeof(Guid));
                    dtImportObject.Columns.Add("RecruitmentHistoryID", typeof(Guid));
                    dtImportObject.Columns.Add("InterviewID1", typeof(Guid));
                    dtImportObject.Columns.Add("InterviewID2", typeof(Guid));
                    dtImportObject.Columns.Add("InterviewID3", typeof(Guid));
                    dtImportObject.Columns.Add("InterviewID4", typeof(Guid));
                    dtImportObject.Columns.Add("InterviewID5", typeof(Guid));
                    dtImportObject.Columns.Add("InterviewID6", typeof(Guid));
                    dtImportObject.Columns.Add("InterviewID7", typeof(Guid));
                    dtImportObject.Columns.Add("GroupConditionID1", typeof(Guid));
                    dtImportObject.Columns.Add("GroupConditionID2", typeof(Guid));
                    dtImportObject.Columns.Add("GroupConditionID3", typeof(Guid));
                    dtImportObject.Columns.Add("GroupConditionID4", typeof(Guid));
                    dtImportObject.Columns.Add("GroupConditionID5", typeof(Guid));
                    dtImportObject.Columns.Add("GroupConditionID6", typeof(Guid));
                    dtImportObject.Columns.Add("GroupConditionID7", typeof(Guid));

                    var dtInvalidObject = new DataTable("InvalidObject");
                    dtInvalidObject.Columns.Add("DataField", typeof(string));
                    dtInvalidObject.Columns.Add("InvalidValue", typeof(string));
                    dtInvalidObject.Columns.Add("ExcelField", typeof(string));
                    dtInvalidObject.Columns.Add("ExcelValue", typeof(object));
                    dtInvalidObject.Columns.Add("ValueType", typeof(string));
                    dtInvalidObject.Columns.Add("Desciption", typeof(string));
                    List<DataRow> listInvalidRow = new List<DataRow>();

                    foreach (DataRow excelRow in dtImportObject.Rows)
                    {
                        var excelRowIndex = excelRow.Field<int>(DefaultConstants.ExcelRowIndex);
                        var nullData = HRM.Business.Main.Domain.InvalidDataType.NullData.ToString().TranslateString();
                        var duplicateInDb = HRM.Business.Main.Domain.InvalidDataType.DuplicateInDb.ToString().TranslateString();
                        var duplicateInFile = HRM.Business.Main.Domain.InvalidDataType.DuplicateInFile.ToString().TranslateString();
                        var referenceNotFound = HRM.Business.Main.Domain.InvalidDataType.ReferenceNotFound.ToString().TranslateString();

                        var listGroupConditionID = new List<Guid>();
                        Guid recruitmentHistoryID = Guid.Empty;
                        Guid candidateID = Guid.Empty;
                        bool isInvalid = false;

                        foreach (var templateItem in importTemplateItems)
                        {
                            string fieldName = templateItem.ChildFieldLevel1.TrimAll();
                            var excelField = templateItem.ExcelField1.TrimAll();
                            string excelAddress = excelField + (excelRowIndex + 1);

                            if (excelRow.IsNull(excelField))
                            {
                                if (!templateItem.AllowNull.GetBoolean())
                                {
                                    bool isNotNullGroup = false;

                                    if (fieldName == "GroupCondition1" || fieldName == "GroupResult1")
                                    {
                                        if (!excelRow.IsNull(FieldMappings["GroupCondition1"])
                                            || !excelRow.IsNull(FieldMappings["Group1Score1"])
                                            || !excelRow.IsNull(FieldMappings["Group1Score2"])
                                            || !excelRow.IsNull(FieldMappings["Group1Score3"])
                                            || !excelRow.IsNull(FieldMappings["GroupResult1"]))
                                        {
                                            isNotNullGroup = true;
                                        }
                                    }
                                    else if (fieldName == "GroupCondition2" || fieldName == "GroupResult2")
                                    {
                                        if (!excelRow.IsNull(FieldMappings["GroupCondition2"])
                                            || !excelRow.IsNull(FieldMappings["Group2Score1"])
                                            || !excelRow.IsNull(FieldMappings["Group2Score2"])
                                            || !excelRow.IsNull(FieldMappings["Group2Score3"])
                                            || !excelRow.IsNull(FieldMappings["GroupResult2"]))
                                        {
                                            isNotNullGroup = true;
                                        }
                                    }
                                    else if (fieldName == "GroupCondition3" || fieldName == "GroupResult3")
                                    {
                                        if (!excelRow.IsNull(FieldMappings["GroupCondition3"])
                                            || !excelRow.IsNull(FieldMappings["Group3Score1"])
                                            || !excelRow.IsNull(FieldMappings["Group3Score2"])
                                            || !excelRow.IsNull(FieldMappings["Group3Score3"])
                                            || !excelRow.IsNull(FieldMappings["GroupResult3"]))
                                        {
                                            isNotNullGroup = true;
                                        }
                                    }
                                    else if (fieldName == "GroupCondition4" || fieldName == "GroupResult4")
                                    {
                                        if (!excelRow.IsNull(FieldMappings["GroupCondition4"])
                                            || !excelRow.IsNull(FieldMappings["Group4Score1"])
                                            || !excelRow.IsNull(FieldMappings["Group4Score2"])
                                            || !excelRow.IsNull(FieldMappings["Group4Score3"])
                                            || !excelRow.IsNull(FieldMappings["GroupResult4"]))
                                        {
                                            isNotNullGroup = true;
                                        }
                                    }
                                    else if (fieldName == "GroupCondition5" || fieldName == "GroupResult5")
                                    {
                                        if (!excelRow.IsNull(FieldMappings["GroupCondition5"])
                                            || !excelRow.IsNull(FieldMappings["Group5Score1"])
                                            || !excelRow.IsNull(FieldMappings["Group5Score2"])
                                            || !excelRow.IsNull(FieldMappings["Group5Score3"])
                                            || !excelRow.IsNull(FieldMappings["GroupResult5"]))
                                        {
                                            isNotNullGroup = true;
                                        }
                                    }
                                    else if (fieldName == "GroupCondition6" || fieldName == "GroupResult6")
                                    {
                                        if (!excelRow.IsNull(FieldMappings["GroupCondition6"])
                                            || !excelRow.IsNull(FieldMappings["Group6Score1"])
                                            || !excelRow.IsNull(FieldMappings["Group6Score2"])
                                            || !excelRow.IsNull(FieldMappings["Group6Score3"])
                                            || !excelRow.IsNull(FieldMappings["GroupResult6"]))
                                        {
                                            isNotNullGroup = true;
                                        }
                                    }
                                    else if (fieldName == "GroupCondition7" || fieldName == "GroupResult7")
                                    {
                                        if (!excelRow.IsNull(FieldMappings["GroupCondition7"])
                                            || !excelRow.IsNull(FieldMappings["Group7Score1"])
                                            || !excelRow.IsNull(FieldMappings["Group7Score2"])
                                            || !excelRow.IsNull(FieldMappings["Group7Score3"])
                                            || !excelRow.IsNull(FieldMappings["GroupResult7"]))
                                        {
                                            isNotNullGroup = true;
                                        }
                                    }
                                    else
                                    {
                                        isNotNullGroup = true;
                                    }

                                    if (isNotNullGroup)
                                    {
                                        var invalidRow = dtInvalidObject.NewRow();
                                        dtInvalidObject.Rows.Add(invalidRow);
                                        invalidRow.SetField("DataField", fieldName);
                                        invalidRow.SetField("InvalidValue", string.Empty);
                                        invalidRow.SetField("ExcelField", excelAddress);
                                        invalidRow.SetField("ExcelValue", string.Empty);
                                        invalidRow.SetField("Desciption", nullData);
                                        isInvalid = true;
                                    }
                                }
                            }
                            else
                            {
                                var excelValue = excelRow[excelField];

                                if (fieldName == "JobVacancyCode")
                                {
                                    var jobVacancy = listJobVacancy.Where(d => d.Code == excelValue.GetString()).FirstOrDefault();

                                    if (jobVacancy == null)
                                    {
                                        var invalidRow = dtInvalidObject.NewRow();
                                        dtInvalidObject.Rows.Add(invalidRow);
                                        invalidRow.SetField("DataField", fieldName);
                                        invalidRow.SetField("InvalidValue", excelValue.GetString());
                                        invalidRow.SetField("ExcelField", excelAddress);
                                        invalidRow.SetField("ExcelValue", excelValue);
                                        invalidRow.SetField("Desciption", referenceNotFound);
                                        isInvalid = true;
                                    }
                                    else
                                    {
                                        excelRow.SetField("JobVacancyID", jobVacancy.ID);
                                    }
                                }
                                else if (fieldName == "LanguageCode")
                                {
                                    var language = listLanguage.Where(d => d.Code == excelValue.GetString()).FirstOrDefault();

                                    if (language == null)
                                    {
                                        if (!string.IsNullOrWhiteSpace(excelValue.GetString()))
                                        {
                                            var invalidRow = dtInvalidObject.NewRow();
                                            dtInvalidObject.Rows.Add(invalidRow);
                                            invalidRow.SetField("DataField", fieldName);
                                            invalidRow.SetField("InvalidValue", excelValue.GetString());
                                            invalidRow.SetField("ExcelField", excelAddress);
                                            invalidRow.SetField("ExcelValue", excelValue);
                                            invalidRow.SetField("Desciption", referenceNotFound);
                                            isInvalid = true;
                                        }
                                    }
                                    else
                                    {
                                        excelRow.SetField("LanguageID", language.ID);
                                    }
                                }
                                else if (fieldName == "CandidateCode")
                                {
                                    candidateID = listCandidate.Where(d => d.CodeCandidate
                                        == excelValue.GetString()).Select(d => d.ID).FirstOrDefault();

                                    if (candidateID == Guid.Empty)
                                    {
                                        var invalidRow = dtInvalidObject.NewRow();
                                        dtInvalidObject.Rows.Add(invalidRow);
                                        invalidRow.SetField("DataField", fieldName);
                                        invalidRow.SetField("InvalidValue", excelValue.GetString());
                                        invalidRow.SetField("ExcelField", excelAddress);
                                        invalidRow.SetField("ExcelValue", excelValue);
                                        invalidRow.SetField("Desciption", referenceNotFound);
                                        isInvalid = true;
                                    }
                                    else
                                    {
                                        excelRow.SetField("CandidateID", candidateID);

                                        recruitmentHistoryID = listRecruitmentHistory.Where(d => d.CandidateID == candidateID)
                                            .OrderByDescending(d => d.DateApply).Select(d => d.ID).FirstOrDefault();

                                        if (recruitmentHistoryID != Guid.Empty)
                                        {
                                            excelRow.SetField("RecruitmentHistoryID", recruitmentHistoryID);
                                        }
                                    }
                                }
                                else if (fieldName == "GroupCondition1" || fieldName == "GroupCondition2"
                                    || fieldName == "GroupCondition3" || fieldName == "GroupCondition4"
                                    || fieldName == "GroupCondition5" || fieldName == "GroupCondition6"
                                    || fieldName == "GroupCondition7")
                                {
                                    var groupConditionID = listGroupCondition.Where(d => d.Code
                                          == excelValue.GetString()).Select(d => d.ID).FirstOrDefault();

                                    if (groupConditionID == Guid.Empty)
                                    {
                                        var invalidRow = dtInvalidObject.NewRow();
                                        dtInvalidObject.Rows.Add(invalidRow);
                                        invalidRow.SetField("DataField", fieldName);
                                        invalidRow.SetField("InvalidValue", excelValue.GetString());
                                        invalidRow.SetField("ExcelField", excelAddress);
                                        invalidRow.SetField("ExcelValue", excelValue);
                                        invalidRow.SetField("Desciption", referenceNotFound);
                                        isInvalid = true;
                                    }
                                    else
                                    {
                                        string groupIDField = fieldName.Substring(14);
                                        groupIDField = "GroupConditionID" + groupIDField;
                                        excelRow.SetField(groupIDField, groupConditionID);

                                        if (listGroupConditionID.Contains(groupConditionID))
                                        {
                                            var invalidRow = dtInvalidObject.NewRow();
                                            dtInvalidObject.Rows.Add(invalidRow);
                                            invalidRow.SetField("DataField", fieldName);
                                            invalidRow.SetField("InvalidValue", excelValue.GetString());
                                            invalidRow.SetField("ExcelField", excelAddress);
                                            invalidRow.SetField("ExcelValue", excelValue);
                                            invalidRow.SetField("Desciption", duplicateInFile);
                                            isInvalid = true;
                                        }
                                        else
                                        {
                                            listGroupConditionID.Add(groupConditionID);

                                            var isDupplicateOnTable = dtImportObject.Rows.OfType<DataRow>().Any(d => d != excelRow && d["CandidateID"].GetString() == candidateID.ToString()
                                                && (d["GroupConditionID1"].GetString() == groupConditionID.ToString() || d["GroupConditionID2"].GetString() == groupConditionID.ToString()
                                                || d["GroupConditionID3"].GetString() == groupConditionID.ToString() || d["GroupConditionID4"].GetString() == groupConditionID.ToString()
                                                || d["GroupConditionID5"].GetString() == groupConditionID.ToString() || d["GroupConditionID6"].GetString() == groupConditionID.ToString()
                                                || d["GroupConditionID7"].GetString() == groupConditionID.ToString()));

                                            if (isDupplicateOnTable)
                                            {
                                                var invalidRow = dtInvalidObject.NewRow();
                                                dtInvalidObject.Rows.Add(invalidRow);
                                                invalidRow.SetField("DataField", fieldName);
                                                invalidRow.SetField("InvalidValue", excelValue.GetString());
                                                invalidRow.SetField("ExcelField", excelAddress);
                                                invalidRow.SetField("ExcelValue", excelValue);
                                                invalidRow.SetField("Desciption", duplicateInFile);
                                                isInvalid = true;
                                            }
                                            else
                                            {
                                                var listInterviewByCandidate = listInterview.Where(d => d.CandidateID == candidateID && d.RecruitmentHistoryID == recruitmentHistoryID).ToList();
                                                var interViewDupplicate = listInterviewByCandidate.Where(d => d.GroupConditionID == groupConditionID).FirstOrDefault();

                                                if (interViewDupplicate != null)
                                                {
                                                    string interviewIDField = fieldName.Substring(14);
                                                    interviewIDField = "InterviewID" + interviewIDField;
                                                    excelRow.SetField(interviewIDField, interViewDupplicate.ID);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (isInvalid)
                        {
                            listInvalidRow.Add(excelRow);
                        }
                    }

                    if (listInvalidRow.Count() > 0)
                    {
                        foreach (DataRow item in listInvalidRow)
                        {
                            dtImportObject.Rows.Remove(item);
                        }
                    }

                    if (ImportObjects.ContainsKey(UserID))
                    {
                        ImportObjects[UserID] = dtImportObject;
                    }
                    else
                    {
                        ImportObjects.Add(UserID, dtImportObject);
                    }

                    if (InvalidObjects.ContainsKey(UserID))
                    {
                        InvalidObjects[UserID] = dtInvalidObject;
                    }
                    else
                    {
                        InvalidObjects.Add(UserID, dtInvalidObject);
                    }
                }
            }
        }

        public bool SaveInterviewResult()
        {
            bool result = true;

            if (ImportObjects.ContainsKey(UserID))
            {
                using (var context = new VnrHrmDataContext())
                {
                    IUnitOfWork unitOfWork = new UnitOfWork(context);
                    var dtImportObject = ImportObjects[UserID];
                    var dtInterview = new DataTable("Rec_Interview");
                    dtInterview.Columns.Add("ID", typeof(Guid));
                    dtInterview.Columns.Add("CandidateID", typeof(Guid));
                    dtInterview.Columns.Add("JobVacancyID", typeof(Guid));
                    dtInterview.Columns.Add("LanguageID", typeof(Guid));
                    dtInterview.Columns.Add("GroupConditionID", typeof(Guid));
                    dtInterview.Columns.Add("RecruitmentHistoryID", typeof(Guid));
                    dtInterview.Columns.Add("Score", typeof(double));
                    dtInterview.Columns.Add("Score1", typeof(double));
                    dtInterview.Columns.Add("Score2", typeof(double));
                    dtInterview.Columns.Add("ResultInterview", typeof(string));
                    dtInterview.Columns.Add("LevelInterview", typeof(int));

                    var dtCandidate = new DataTable("Rec_Candidate");
                    dtCandidate.Columns.Add("ID", typeof(Guid));
                    dtCandidate.Columns.Add("Status", typeof(string));

                    var dtHisstorry = new DataTable("Rec_RecruitmentHistory");
                    dtHisstorry.Columns.Add("ID", typeof(Guid));
                    dtHisstorry.Columns.Add("Status", typeof(string));

                    foreach (DataRow importRow in dtImportObject.Rows)
                    {
                        var candidateStatus = EnumDropDown.CandidateStatus.E_PASS;
                        var candidateID = importRow["CandidateID"];
                        object groupResult = null;

                        if (!importRow.IsNull(FieldMappings["GroupResult1"]))
                        {
                            groupResult = importRow[FieldMappings["GroupResult1"]];
                            DataRow interviewRow1 = dtInterview.NewRow();
                            dtInterview.Rows.Add(interviewRow1);

                            interviewRow1.SetField("ID", Guid.NewGuid());
                            var interviewID = importRow["InterviewID1"];

                            if (interviewID != null && interviewID.IsTypeOf(typeof(Guid)))
                            {
                                if (interviewID.GetString() != Guid.Empty.ToString())
                                {
                                    interviewRow1.SetField("ID", interviewID);
                                    interviewRow1.AcceptChanges();//update mode
                                }
                            }

                            interviewRow1.SetField("CandidateID", candidateID);
                            interviewRow1.SetField("GroupConditionID", importRow["GroupConditionID1"]);
                            interviewRow1.SetField("RecruitmentHistoryID", importRow["RecruitmentHistoryID"]);
                            interviewRow1.SetField("Score", importRow[FieldMappings["Group1Score1"]]);
                            interviewRow1.SetField("Score1", importRow[FieldMappings["Group1Score2"]]);
                            interviewRow1.SetField("Score2", importRow[FieldMappings["Group1Score3"]]);
                            interviewRow1.SetField("ResultInterview", groupResult);
                            interviewRow1.SetField("LevelInterview", 1);

                            if (!importRow.IsNull("JobVacancyID"))
                            {
                                interviewRow1.SetField("JobVacancyID", importRow["JobVacancyID"]);
                            }

                            if (!importRow.IsNull("LanguageID"))
                            {
                                interviewRow1.SetField("LanguageID", importRow["LanguageID"]);
                            }

                            if (groupResult.GetString() == EnumDropDown.CandidateStatus.E_FAIL.ToString())
                            {
                                candidateStatus = EnumDropDown.CandidateStatus.E_FAIL;
                            }
                        }

                        if (!importRow.IsNull(FieldMappings["GroupResult2"]))
                        {
                            groupResult = importRow[FieldMappings["GroupResult2"]];
                            DataRow interviewRow2 = dtInterview.NewRow();
                            dtInterview.Rows.Add(interviewRow2);

                            interviewRow2.SetField("ID", Guid.NewGuid());
                            var interviewID = importRow["InterviewID2"];

                            if (interviewID != null && interviewID.IsTypeOf(typeof(Guid)))
                            {
                                if (interviewID.GetString() != Guid.Empty.ToString())
                                {
                                    interviewRow2.SetField("ID", interviewID);
                                    interviewRow2.AcceptChanges();//update mode
                                }
                            }

                            interviewRow2.SetField("CandidateID", candidateID);
                            interviewRow2.SetField("GroupConditionID", importRow["GroupConditionID2"]);
                            interviewRow2.SetField("RecruitmentHistoryID", importRow["RecruitmentHistoryID"]);
                            interviewRow2.SetField("Score", importRow[FieldMappings["Group2Score1"]]);
                            interviewRow2.SetField("Score1", importRow[FieldMappings["Group2Score2"]]);
                            interviewRow2.SetField("Score2", importRow[FieldMappings["Group2Score3"]]);
                            interviewRow2.SetField("ResultInterview", groupResult);
                            interviewRow2.SetField("LevelInterview", 2);

                            if (!importRow.IsNull("JobVacancyID"))
                            {
                                interviewRow2.SetField("JobVacancyID", importRow["JobVacancyID"]);
                            }

                            if (!importRow.IsNull("LanguageID"))
                            {
                                interviewRow2.SetField("LanguageID", importRow["LanguageID"]);
                            }

                            if (groupResult.GetString() == EnumDropDown.CandidateStatus.E_FAIL.ToString())
                            {
                                candidateStatus = EnumDropDown.CandidateStatus.E_FAIL;
                            }
                        }

                        if (!importRow.IsNull(FieldMappings["GroupResult3"]))
                        {
                            groupResult = importRow[FieldMappings["GroupResult3"]];
                            DataRow interviewRow3 = dtInterview.NewRow();
                            dtInterview.Rows.Add(interviewRow3);

                            interviewRow3.SetField("ID", Guid.NewGuid());
                            var interviewID = importRow["InterviewID3"];

                            if (interviewID != null && interviewID.IsTypeOf(typeof(Guid)))
                            {
                                if (interviewID.GetString() != Guid.Empty.ToString())
                                {
                                    interviewRow3.SetField("ID", interviewID);
                                    interviewRow3.AcceptChanges();//update mode
                                }
                            }

                            interviewRow3.SetField("CandidateID", candidateID);
                            interviewRow3.SetField("GroupConditionID", importRow["GroupConditionID3"]);
                            interviewRow3.SetField("RecruitmentHistoryID", importRow["RecruitmentHistoryID"]);
                            interviewRow3.SetField("Score", importRow[FieldMappings["Group3Score1"]]);
                            interviewRow3.SetField("Score1", importRow[FieldMappings["Group3Score2"]]);
                            interviewRow3.SetField("Score2", importRow[FieldMappings["Group3Score3"]]);
                            interviewRow3.SetField("ResultInterview", groupResult);
                            interviewRow3.SetField("LevelInterview", 3);

                            if (!importRow.IsNull("JobVacancyID"))
                            {
                                interviewRow3.SetField("JobVacancyID", importRow["JobVacancyID"]);
                            }

                            if (!importRow.IsNull("LanguageID"))
                            {
                                interviewRow3.SetField("LanguageID", importRow["LanguageID"]);
                            }

                            if (groupResult.GetString() == EnumDropDown.CandidateStatus.E_FAIL.ToString())
                            {
                                candidateStatus = EnumDropDown.CandidateStatus.E_FAIL;
                            }
                        }

                        if (!importRow.IsNull(FieldMappings["GroupResult4"]))
                        {
                            groupResult = importRow[FieldMappings["GroupResult4"]];
                            DataRow interviewRow4 = dtInterview.NewRow();
                            dtInterview.Rows.Add(interviewRow4);

                            interviewRow4.SetField("ID", Guid.NewGuid());
                            var interviewID = importRow["InterviewID4"];

                            if (interviewID != null && interviewID.IsTypeOf(typeof(Guid)))
                            {
                                if (interviewID.GetString() != Guid.Empty.ToString())
                                {
                                    interviewRow4.SetField("ID", interviewID);
                                    interviewRow4.AcceptChanges();//update mode
                                }
                            }

                            interviewRow4.SetField("CandidateID", candidateID);
                            interviewRow4.SetField("GroupConditionID", importRow["GroupConditionID4"]);
                            interviewRow4.SetField("RecruitmentHistoryID", importRow["RecruitmentHistoryID"]);
                            interviewRow4.SetField("Score", importRow[FieldMappings["Group4Score1"]]);
                            interviewRow4.SetField("Score1", importRow[FieldMappings["Group4Score2"]]);
                            interviewRow4.SetField("Score2", importRow[FieldMappings["Group4Score3"]]);
                            interviewRow4.SetField("ResultInterview", groupResult);
                            interviewRow4.SetField("LevelInterview", 4);

                            if (!importRow.IsNull("JobVacancyID"))
                            {
                                interviewRow4.SetField("JobVacancyID", importRow["JobVacancyID"]);
                            }

                            if (!importRow.IsNull("LanguageID"))
                            {
                                interviewRow4.SetField("LanguageID", importRow["LanguageID"]);
                            }

                            if (groupResult.GetString() == EnumDropDown.CandidateStatus.E_FAIL.ToString())
                            {
                                candidateStatus = EnumDropDown.CandidateStatus.E_FAIL;
                            }
                        }

                        if (!importRow.IsNull(FieldMappings["GroupResult5"]))
                        {
                            groupResult = importRow[FieldMappings["GroupResult5"]];
                            DataRow interviewRow5 = dtInterview.NewRow();
                            dtInterview.Rows.Add(interviewRow5);

                            interviewRow5.SetField("ID", Guid.NewGuid());
                            var interviewID = importRow["InterviewID5"];

                            if (interviewID != null && interviewID.IsTypeOf(typeof(Guid)))
                            {
                                if (interviewID.GetString() != Guid.Empty.ToString())
                                {
                                    interviewRow5.SetField("ID", interviewID);
                                    interviewRow5.AcceptChanges();//update mode
                                }
                            }

                            interviewRow5.SetField("CandidateID", candidateID);
                            interviewRow5.SetField("GroupConditionID", importRow["GroupConditionID5"]);
                            interviewRow5.SetField("RecruitmentHistoryID", importRow["RecruitmentHistoryID"]);
                            interviewRow5.SetField("Score", importRow[FieldMappings["Group5Score1"]]);
                            interviewRow5.SetField("Score1", importRow[FieldMappings["Group5Score2"]]);
                            interviewRow5.SetField("Score2", importRow[FieldMappings["Group5Score3"]]);
                            interviewRow5.SetField("ResultInterview", groupResult);
                            interviewRow5.SetField("LevelInterview", 5);

                            if (!importRow.IsNull("JobVacancyID"))
                            {
                                interviewRow5.SetField("JobVacancyID", importRow["JobVacancyID"]);
                            }

                            if (!importRow.IsNull("LanguageID"))
                            {
                                interviewRow5.SetField("LanguageID", importRow["LanguageID"]);
                            }

                            if (groupResult.GetString() == EnumDropDown.CandidateStatus.E_FAIL.ToString())
                            {
                                candidateStatus = EnumDropDown.CandidateStatus.E_FAIL;
                            }
                        }

                        if (!importRow.IsNull(FieldMappings["GroupResult6"]))
                        {
                            groupResult = importRow[FieldMappings["GroupResult6"]];
                            DataRow interviewRow6 = dtInterview.NewRow();
                            dtInterview.Rows.Add(interviewRow6);

                            interviewRow6.SetField("ID", Guid.NewGuid());
                            var interviewID = importRow["InterviewID6"];

                            if (interviewID != null && interviewID.IsTypeOf(typeof(Guid)))
                            {
                                if (interviewID.GetString() != Guid.Empty.ToString())
                                {
                                    interviewRow6.SetField("ID", interviewID);
                                    interviewRow6.AcceptChanges();//update mode
                                }
                            }

                            interviewRow6.SetField("CandidateID", candidateID);
                            interviewRow6.SetField("GroupConditionID", importRow["GroupConditionID6"]);
                            interviewRow6.SetField("RecruitmentHistoryID", importRow["RecruitmentHistoryID"]);
                            interviewRow6.SetField("Score", importRow[FieldMappings["Group6Score1"]]);
                            interviewRow6.SetField("Score1", importRow[FieldMappings["Group6Score2"]]);
                            interviewRow6.SetField("Score2", importRow[FieldMappings["Group6Score3"]]);
                            interviewRow6.SetField("ResultInterview", groupResult);
                            interviewRow6.SetField("LevelInterview", 6);

                            if (!importRow.IsNull("JobVacancyID"))
                            {
                                interviewRow6.SetField("JobVacancyID", importRow["JobVacancyID"]);
                            }

                            if (!importRow.IsNull("LanguageID"))
                            {
                                interviewRow6.SetField("LanguageID", importRow["LanguageID"]);
                            }

                            if (groupResult.GetString() == EnumDropDown.CandidateStatus.E_FAIL.ToString())
                            {
                                candidateStatus = EnumDropDown.CandidateStatus.E_FAIL;
                            }
                        }

                        if (!importRow.IsNull(FieldMappings["GroupResult7"]))
                        {
                            groupResult = importRow[FieldMappings["GroupResult7"]];
                            DataRow interviewRow7 = dtInterview.NewRow();
                            dtInterview.Rows.Add(interviewRow7);

                            interviewRow7.SetField("ID", Guid.NewGuid());
                            var interviewID = importRow["InterviewID7"];

                            if (interviewID != null && interviewID.IsTypeOf(typeof(Guid)))
                            {
                                if (interviewID.GetString() != Guid.Empty.ToString())
                                {
                                    interviewRow7.SetField("ID", interviewID);
                                    interviewRow7.AcceptChanges();//update mode
                                }
                            }

                            interviewRow7.SetField("CandidateID", candidateID);
                            interviewRow7.SetField("GroupConditionID", importRow["GroupConditionID7"]);
                            interviewRow7.SetField("RecruitmentHistoryID", importRow["RecruitmentHistoryID"]);
                            interviewRow7.SetField("Score", importRow[FieldMappings["Group7Score1"]]);
                            interviewRow7.SetField("Score1", importRow[FieldMappings["Group7Score2"]]);
                            interviewRow7.SetField("Score2", importRow[FieldMappings["Group7Score3"]]);
                            interviewRow7.SetField("ResultInterview", groupResult);
                            interviewRow7.SetField("LevelInterview", 7);

                            if (!importRow.IsNull("JobVacancyID"))
                            {
                                interviewRow7.SetField("JobVacancyID", importRow["JobVacancyID"]);
                            }

                            if (!importRow.IsNull("LanguageID"))
                            {
                                interviewRow7.SetField("LanguageID", importRow["LanguageID"]);
                            }

                            if (groupResult.GetString() == EnumDropDown.CandidateStatus.E_FAIL.ToString())
                            {
                                candidateStatus = EnumDropDown.CandidateStatus.E_FAIL;
                            }
                        }

                        if (groupResult != null)
                        {
                            DataRow candidateRow = dtCandidate.NewRow();
                            dtCandidate.Rows.Add(candidateRow);
                            candidateRow.AcceptChanges();//Chuyển qua update
                            candidateRow.SetField("ID", candidateID);
                            candidateRow.SetField("Status", candidateStatus.ToString());
                            var historyID = importRow["RecruitmentHistoryID"];

                            if (historyID != null && historyID.IsTypeOf(typeof(Guid)))
                            {
                                if (historyID.GetString() != Guid.Empty.ToString())
                                {
                                    DataRow hisstorryRow = dtHisstorry.NewRow();
                                    dtHisstorry.Rows.Add(hisstorryRow);
                                    hisstorryRow.AcceptChanges();//Chuyển qua update
                                    hisstorryRow.SetField("ID", historyID);
                                    hisstorryRow.SetField("Status", candidateStatus.ToString());
                                }
                            }
                        }
                    }

                    using (DbCommander commander = new DbCommander(context.Database.Connection))
                    {
                        var transaction = commander.BeginTransaction();

                        try
                        {
                            var resultCount = commander.ExecuteBatch(transaction, dtInterview);
                            resultCount = commander.UpdateTable(transaction, dtCandidate);
                            resultCount = commander.UpdateTable(transaction, dtHisstorry);
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            result = false;
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }

            if (result)
            {
                if (ImportObjects.ContainsKey(UserID))
                {
                    ImportObjects[UserID] = null;
                    ImportObjects.Remove(UserID);
                }

                if (InvalidObjects.ContainsKey(UserID))
                {
                    InvalidObjects[UserID] = null;
                    InvalidObjects.Remove(UserID);
                }
            }

            return result;
        }

        private ImportItemInfo CreateImportItemInfo(int ordinal, bool allowNull,
            bool allowDuplicate, int duplicateGroup, string childFieldLevel1,
            string childFieldLevel2, string excelField1, string excelField2)
        {
            ImportItemInfo result = new ImportItemInfo
            {
                Ordinal = ordinal,
                AllowNull = allowNull,
                AllowDuplicate = allowDuplicate,
                DuplicateGroup = duplicateGroup,
                ChildFieldLevel1 = childFieldLevel1,
                ChildFieldLevel2 = childFieldLevel2,
                ExcelField1 = excelField1,
                ExcelField2 = excelField2
            };

            return result;
        }
    }
}
