using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;

namespace BachMaiCR.DataAccess.Repository
{
  public interface IDoctorRepository : IRepository<DOCTOR>
  {
    List<DocTorDate> GetAllDayByDoctor(int idDoctor, int idCalendarDuty);

    List<DateChangeList> GetAllDayByDoctorDefault(int idDoctor, int idCalendarDuty, DateTime date);

    List<DOCTOR> GetAllByArrayDepartmentID(int[] ArrayIdDepartment);

    List<DoctorColumn> GetAllByTemplateColumn(int idColumn);

    List<SelectListItem> GetListItemBase();

    List<DOCTOR> GetAll_List();

    string GetMutilNameDoctors(string lstId);

    List<DOCTOR> GetAll(int departmentId);

    PagedList<DOCTOR> GetAll(DoctorSearch entity, int page, int size);

    bool ExistReferenceDepartment(int deprtID);

    bool ExistIdentity(string identity);

    List<DOCTOR> GetAllDoctorByGroup(int groupId);

    bool ExistReferenceCategory(int usrID);

    List<DOCTOR> GetAll(DoctorSearch entity);

    List<DOCTOR> GetAllByDepartmentId(int deptId);

    List<DoctorList> GetAllByDepartmentUsername(string userName);

    List<DoctorInCalendar> GetAllDoctor(DateTime? date, int templateId);

    List<DoctorInCalendar> GetAllDoctorNotLeader(DateTime? date, int isLeader);
  }
}
