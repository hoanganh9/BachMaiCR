using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace BachMaiCR.DataAccess.Repository
{
    public class DoctorRepository : EFRepository<DOCTOR>, IDoctorRepository, IRepository<DOCTOR>
    {
        public DoctorRepository(DbContext dbContext)
          : base(dbContext)
        {
        }

        public PagedList<DOCTOR> GetAll(DoctorSearch entity, int page, int size)
        {
            try
            {
                IQueryable<DOCTOR> source = this.DbSet.AsNoTracking().Where((obj => (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && (string.IsNullOrEmpty(entity.SearchName) || obj.DOCTOR_NAME.Contains(entity.SearchName.Trim()) || obj.ABBREVIATION.Contains(entity.SearchName.Trim())) && (string.IsNullOrEmpty(entity.SearchPhone) || obj.PHONE.Contains(entity.SearchPhone.Trim())) && (string.IsNullOrEmpty(entity.SearchCode) || obj.CODE_STAFF.Contains(entity.SearchCode.Trim())) && (string.IsNullOrEmpty(entity.SearchIdentity) || obj.IDENTITY_CARD.Contains(entity.SearchIdentity.Trim())) && (string.IsNullOrEmpty(entity.SearchEmail) || obj.EMAIL.Contains(entity.SearchEmail.Trim())) && (string.IsNullOrEmpty(entity.SearchAddress) || obj.ADDRESS.Contains(entity.SearchAddress.Trim()))));
                if (entity.SearchDeprtId.HasValue && entity.SearchDeprtId.Value >0)
                {
                    IQueryable<LM_DEPARTMENT> searchKeys = this.DbContext.LM_DEPARTMENT.AsNoTracking().Where((t => t.DEPARTMENT_PATH.Contains("," + entity.SearchDeprtId.ToString() + ",")));
                    source = this.MultiValueContainsAny(source, searchKeys);
                }
                if (entity.SearchDoctorLevelId.HasValue && entity.SearchDoctorLevelId.Value > 0)
                    source = source.Where((t => t.DOCTOR_LEVEL_ID.Equals(entity.SearchDoctorLevelId.Value)));
                if (entity.SearchPositionId.HasValue && entity.SearchPositionId.Value > 0)
                    source = source.Where((t => t.POSITION_IDs.Contains("," + entity.SearchPositionId.Value.ToString() + ",")));
                if (entity.SearchDegreeId.HasValue && entity.SearchDegreeId.Value > 0)
                    source = source.Where((t => t.EDUCATION_IDs.Contains("," + entity.SearchDegreeId.Value.ToString() + ",")));
                return source.OrderBy(t => t.ABBREVIATION).Paginate(page, size, 0);
            }
            catch
            {
                return new PagedList<DOCTOR>();
            }
        }

        public List<DOCTOR> GetAllByDepartmentId(int deptId)
        {
            try
            {
                IQueryable<DOCTOR> source = this.DbSet.AsNoTracking().Where((obj => obj.ISDELETE.Value.Equals(false)));
                if (deptId > 0)
                {
                    IQueryable<LM_DEPARTMENT> searchKeys = this.DbContext.LM_DEPARTMENT.AsNoTracking().Where((t => t.DEPARTMENT_PATH.Contains("," + deptId.ToString() + ",")));
                    source = this.MultiValueContainsAny(source, searchKeys);
                }
                return source.OrderBy(t => t.ABBREVIATION).ToList();
            }
            catch
            {
                return (List<DOCTOR>)null;
            }
        }

        public List<DoctorList> GetAllByDepartmentUsername(string userName)
        {
            try
            {
                int? deptId = this.DbContext.ADMIN_USER.Where(user => user.USERNAME == userName).Select(user => new
                {
                    LM_DEPARTMENT_ID = user.LM_DEPARTMENT_ID
                }).ToList()[0].LM_DEPARTMENT_ID;
                IQueryable<DoctorList> source = this.DbSet.AsNoTracking().Where((obj => obj.ISDELETE.Value.Equals(false) && obj.ISACTIVED == true)).Select((obj => new DoctorList()
                {
                    DOCTORS_ID = obj.DOCTORS_ID,
                    DOCTOR_NAME = obj.DOCTOR_NAME,
                    ABBREVIATION = obj.ABBREVIATION,
                    LEVEL_NAME = obj.DOCTOR_LEVEL.LEVEL_NAME,
                    LM_DEPARTMENT_IDs = obj.LM_DEPARTMENT_IDs,
                    PHONE = obj.PHONE
                }));
                if (deptId.GetValueOrDefault()>0)
                {
                    IQueryable<LM_DEPARTMENT_LIST_ID> searchKeys = this.DbContext.LM_DEPARTMENT.Where((dept => dept.DEPARTMENT_PATH.Contains("," + deptId.ToString() + ","))).Select((dept => new LM_DEPARTMENT_LIST_ID()
                    {
                        LM_DEPARTMENT_ID = dept.LM_DEPARTMENT_ID
                    }));
                    source = this.MultiValueContainsAnyList(source, searchKeys);
                }
                return source.OrderBy(t => t.ABBREVIATION).OrderBy(t => t.DOCTOR_NAME).ToList();
            }
            catch
            {
                return (List<DoctorList>)null;
            }
        }

        public IQueryable<DoctorList> MultiValueContainsAnyList(IQueryable<DoctorList> source, IQueryable<LM_DEPARTMENT_LIST_ID> searchKeys)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (searchKeys == null || searchKeys.Count() == 0)
                return source;
            Expression<Func<DoctorList, bool>> expression = null;
            foreach (LM_DEPARTMENT_LIST_ID searchKey in searchKeys)
            {
                LM_DEPARTMENT_LIST_ID dept = searchKey;
                Expression<Func<DoctorList, bool>> second = (t => t.LM_DEPARTMENT_IDs.Contains("," + dept.LM_DEPARTMENT_ID.ToString() + ","));
                expression = expression != null ? expression.Or(second) : second;
            }
            return source.Where(expression);
        }

        public IQueryable<DOCTOR> MultiValueContainsAny(IQueryable<DOCTOR> source, IQueryable<LM_DEPARTMENT> searchKeys)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (searchKeys == null || searchKeys.Count() == 0)
                return source;
            Expression<Func<DOCTOR, bool>> expression = null;
            foreach (LM_DEPARTMENT searchKey in searchKeys)
            {
                LM_DEPARTMENT dept = searchKey;
                Expression<Func<DOCTOR, bool>> second = (t => t.LM_DEPARTMENT_IDs.Contains("," + dept.LM_DEPARTMENT_ID.ToString() + ","));
                expression = expression != null ? expression.Or(second) : second;
            }
            return source.Where(expression);
        }

        public List<DOCTOR> GetAll(DoctorSearch entity)
        {
            try
            {
                IQueryable<DOCTOR> source = this.DbSet.AsNoTracking().Where((obj => (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && (string.IsNullOrEmpty(entity.SearchName) || obj.DOCTOR_NAME.Contains(entity.SearchName.Trim()) || obj.ABBREVIATION.Contains(entity.SearchName.Trim())) && (string.IsNullOrEmpty(entity.SearchPhone) || obj.PHONE.Contains(entity.SearchPhone.Trim())) && (string.IsNullOrEmpty(entity.SearchCode) || obj.CODE_STAFF.Contains(entity.SearchCode.Trim())) && (string.IsNullOrEmpty(entity.SearchIdentity) || obj.IDENTITY_CARD.Contains(entity.SearchIdentity.Trim())) && (string.IsNullOrEmpty(entity.SearchEmail) || obj.EMAIL.Contains(entity.SearchEmail.Trim())) && (string.IsNullOrEmpty(entity.SearchAddress) || obj.ADDRESS.Contains(entity.SearchAddress.Trim()))));
                if (entity.SearchDeprtId.HasValue && entity.SearchDeprtId.Value>0)
                {
                    IQueryable<LM_DEPARTMENT> searchKeys = this.DbContext.LM_DEPARTMENT.AsNoTracking().Where((t => t.DEPARTMENT_PATH.Contains("," + entity.SearchDeprtId.ToString() + ",")));
                    source = this.MultiValueContainsAny(source, searchKeys);
                }
                if (entity.SearchDoctorLevelId.HasValue && entity.SearchDoctorLevelId.Value > 0)
                    source = source.Where((t => t.DOCTOR_LEVEL_ID.Equals(entity.SearchDoctorLevelId.Value)));
                if (entity.SearchPositionId.HasValue && entity.SearchPositionId.Value > 0)
                    source = source.Where((t => t.POSITION_IDs.Contains("," + entity.SearchPositionId.Value.ToString() + ",")));
                if (entity.SearchDegreeId.HasValue && entity.SearchDegreeId.Value > 0)
                    source = source.Where((t => t.EDUCATION_IDs.Contains("," + entity.SearchDegreeId.Value.ToString() + ",")));
                return source.OrderBy(t => t.ABBREVIATION).ToList();
            }
            catch
            {
                return (List<DOCTOR>)new PagedList<DOCTOR>();
            }
        }

        public List<DOCTOR> GetAll(int departmentId)
        {
            try
            {
                if (departmentId > 0)
                {
                    this.DbContext.LM_DEPARTMENT.Where(o => o.LM_DEPARTMENT_ID == departmentId).FirstOrDefault();
                    return this.DbSet.AsNoTracking().Where((obj => (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && obj.LM_DEPARTMENT_IDs.Contains("," + (object)departmentId + ","))).OrderBy(t => t.ABBREVIATION).ToList();
                }
                return this.DbSet.AsNoTracking().Where((obj => obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false))).OrderBy(t => t.ABBREVIATION).ToList();
            }
            catch
            {
                return new List<DOCTOR>();
            }
        }

        public List<DOCTOR> GetAll_List()
        {
            return this.DbSet.AsNoTracking().Where((obj => obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false))).OrderBy(t => t.DOCTOR_NAME).ToList();
        }

        public List<DOCTOR> GetAllByArrayDepartmentID(int[] ArrayIdDepartment)
        {
            List<DOCTOR> list = this.DbSet.AsNoTracking().Where(obj => obj.ISDELETE == false && obj.ISACTIVED == true).OrderBy(obj => obj.DOCTOR_ORDER).ToList();
            int length = ArrayIdDepartment.Length;
            if (length > 0)
            {
                for (int index = 0; index < length; ++index)
                {
                    string vR = ArrayIdDepartment[index].ToString();
                    list = list.Where((t => t.LM_DEPARTMENT_IDs.Contains("," + vR + ","))).ToList();
                }
            }
            return list.ToList();
        }

        public List<DoctorColumn> GetAllByTemplateColumn(int idColumn)
        {
            return this.DbContext.DoctorColumns.Where(obj => obj.TEMPLATE_COLUM_ID == idColumn).OrderBy(o => o.DOCTORS_ID).ToList();
        }

        public List<DoctorInCalendar> GetAllDoctor(DateTime? date, int templateId)
        {
            IQueryable<DoctorInCalendar> source = this.DbContext.CALENDAR_DUTY.Join(this.DbContext.CALENDAR_DATA, (calendar => calendar.CALENDAR_DUTY_ID), (data => data.CALENDAR_DUTY_ID), (calendar, data) => new
            {
                calendar = calendar,
                data = data
            }).Join(this.DbContext.CALENDAR_DOCTOR, data => data.data.CALENDAR_DATA_ID, (doc => doc.CALENDAR_DATA_ID), (data, doc) => new
            {
                data = data,
                doc = doc
            }).Where(data => data.data.calendar.ISDELETE == false && data.data.data.ISDELETE == false && SqlFunctions.DateDiff("dd", date, data.data.data.DATE_START) == 0 && (data.data.calendar.DUTY_TYPE == 1 || data.data.calendar.DUTY_TYPE == 3)).Select(data => new DoctorInCalendar()
            {
                DATE_START = data.data.data.DATE_START,
                DOCTORS_ID = data.doc.DOCTORS_ID,
                CALENDAR_DATA_ID = data.data.data.CALENDAR_DATA_ID,
                CALENDAR_STATUS = data.data.calendar.CALENDAR_STATUS,
                CALENDAR_NAME = data.data.calendar.CALENDAR_NAME,
                TEMPLATES_ID = data.data.calendar.TEMPLATES_ID,
                ISAPPROVED = data.data.calendar.ISAPPROVED,
                CALENDAR_DUTY_ID = data.data.calendar.CALENDAR_DUTY_ID
            });
            if (templateId > 0)
                source = source.Where((o => o.TEMPLATES_ID != templateId));
            return source.OrderBy(o => o.DOCTORS_ID).ToList();
        }

        public List<DoctorInCalendar> GetAllDoctorNotLeader(DateTime? date, int isLeader)
        {
            return this.DbContext.CALENDAR_DUTY.Join(this.DbContext.CALENDAR_DATA, (calendar => calendar.CALENDAR_DUTY_ID), (data => data.CALENDAR_DUTY_ID), (calendar, data) => new
            {
                calendar = calendar,
                data = data
            }).Join(this.DbContext.CALENDAR_DOCTOR, data => data.data.CALENDAR_DATA_ID, (doc => doc.CALENDAR_DATA_ID), (data, doc) => new
            {
                data = data,
                doc = doc
            }).Where(data => data.data.calendar.ISDELETE == false && data.data.data.ISDELETE == false && (isLeader == 1 ? (data.doc.DOCTOR.DOCTOR_GROUP_ID == 1 || data.doc.DOCTOR.DOCTOR_GROUP_ID == 2) && data.data.calendar.DUTY_TYPE == 3 : (data.doc.DOCTOR.DOCTOR_GROUP_ID == 1 || data.doc.DOCTOR.DOCTOR_GROUP_ID == 2 || data.doc.DOCTOR.DOCTOR_GROUP_ID == 3) && (data.data.calendar.DUTY_TYPE == 1 || data.data.calendar.DUTY_TYPE == 3) && data.data.calendar.TEMPLATES_ID != new int?()) && SqlFunctions.DateDiff("dd", date, data.data.data.DATE_START) == 0).Select(data => new DoctorInCalendar()
            {
                DATE_START = data.data.data.DATE_START,
                DOCTORS_ID = data.doc.DOCTORS_ID,
                CALENDAR_DATA_ID = data.data.data.CALENDAR_DATA_ID,
                CALENDAR_STATUS = data.data.calendar.CALENDAR_STATUS,
                CALENDAR_NAME = data.data.calendar.CALENDAR_NAME,
                TEMPLATES_ID = data.data.calendar.TEMPLATES_ID,
                ISAPPROVED = data.data.calendar.ISAPPROVED,
                CALENDAR_DUTY_ID = data.data.calendar.CALENDAR_DUTY_ID
            }).OrderBy(o => o.DOCTORS_ID).ToList();
        }

        public List<DocTorDate> GetAllDayByDoctor(int idDoctor, int idCalendarDuty)
        {
            return this.DbContext.DocTorDates.Where(obj => obj.DOCTORS_ID == idDoctor && obj.CALENDAR_DUTY_ID == idCalendarDuty).OrderBy(o => o.DATE_START).ToList();
        }

        public List<DateChangeList> GetAllDayByDoctorDefault(int idDoctor, int idCalendarDuty, DateTime date)
        {
            return this.DbContext.CALENDAR_DATA.Join(this.DbContext.CALENDAR_DOCTOR, (calendar => calendar.CALENDAR_DATA_ID), (doc => doc.CALENDAR_DATA_ID), (calendar, doc) => new
            {
                calendar = calendar,
                doc = doc
            }).Where(data => data.calendar.CALENDAR_DUTY_ID == idCalendarDuty && data.doc.DOCTORS_ID == idDoctor && SqlFunctions.DateDiff("dd", (DateTime?)date, data.calendar.DATE_START) >= (int?)0).Select(data => new DateChangeList()
            {
                DATE_START = data.calendar.DATE_START,
                CALENDAR_DUTY_ID = data.calendar.CALENDAR_DUTY_ID,
                DOCTORS_ID = data.doc.DOCTORS_ID,
                CALENDAR_DATA_ID = data.calendar.CALENDAR_DATA_ID
            }).OrderBy(o => o.DATE_START).ToList();
        }

        public List<SelectListItem> GetListItemBase()
        {
            return this.DbSet.AsNoTracking().Where((obj => obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false))).OrderBy(obj => obj.DOCTOR_NAME).Select((obj => new SelectListItem()
            {
                Value = obj.DOCTORS_ID.ToString(),
                Text = obj.DOCTOR_NAME
            })).ToList();
        }

        public string GetMutilNameDoctors(string lstId)
        {
            if (string.IsNullOrEmpty(lstId))
                return string.Empty;
            IQueryable<string> source = this.DbSet.AsNoTracking().Where((obj => lstId.Contains("," + obj.DOCTORS_ID.ToString() + ","))).Select(obj => obj.DOCTOR_NAME);
            if (lstId.Any())
                return string.Join(", ", source.ToArray()).Trim();
            return string.Empty;
        }

        public bool ExistReferenceDepartment(int deprtID)
        {
            return this.DbSet.AsNoTracking().Where(obj => obj.LM_DEPARTMENT_IDs.Contains("," + (object)deprtID + ",") && (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false))).Any();
        }

        public bool ExistIdentity(string identity)
        {
            if (string.IsNullOrEmpty(identity))
                return false;
            identity = identity.Trim().ToLower();
            return this.DbSet.AsNoTracking().Where(obj => obj.IDENTITY_CARD.Equals(identity) && (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false))).Any();
        }

        public List<DOCTOR> GetAllDoctorByGroup(int groupId)
        {
            return this.DbSet.AsNoTracking().Where(obj => obj.ISDELETE == false && obj.ISACTIVED == true && (groupId == 1 ? obj.DOCTOR_GROUP_ID == 1 : obj.DOCTOR_GROUP_ID == 1 || obj.DOCTOR_GROUP_ID == 2)).OrderBy(obj => obj.ABBREVIATION).ToList();
        }

        public bool ExistReferenceCategory(int usrID)
        {
            return this.DbSet.AsNoTracking().Where(obj => (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && (obj.EDUCATION_IDs.Contains("," + usrID.ToString() + ",") || obj.POSITION_IDs.Contains("," + usrID.ToString() + ","))).Any();
        }
    }
}