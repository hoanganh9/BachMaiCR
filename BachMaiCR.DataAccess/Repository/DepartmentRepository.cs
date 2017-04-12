





using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;

namespace BachMaiCR.DataAccess.Repository
{
  public class DepartmentRepository : EFRepository<LM_DEPARTMENT>, IDepartmentRepository, IRepository<LM_DEPARTMENT>
  {
    public DepartmentRepository(DbContext dbContext)
      : base(dbContext)
    {
    }

    public bool ExistCode(LM_DEPARTMENT entity)
    {
      if (entity == null)
        return false;
      IQueryable<LM_DEPARTMENT> source = this.DbSet.AsNoTracking().Where((obj => obj.DEPARTMENT_CODE.Equals(entity.DEPARTMENT_CODE.Trim()) && (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false))));
      if (entity.LM_DEPARTMENT_ID <= 0)
        return source.Count<LM_DEPARTMENT>() > 0;
      return source.Where((obj => obj.LM_DEPARTMENT_ID != entity.LM_DEPARTMENT_ID)).Count<LM_DEPARTMENT>() > 0;
    }

    public bool ExistChild(int id)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.DEPARTMENT_PARENT_ID.Value.Equals(id) && (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)))).Count<LM_DEPARTMENT>() > 0;
    }

    public PagedList<LM_DEPARTMENT> GetAll(int type, string name, int page, int size, string sort, string sortDir)
    {
      IQueryable<LM_DEPARTMENT> source = this.DbSet.AsNoTracking().Where((obj => (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && (obj.DEPARTMENT_NAME.Contains(name) || obj.DEPARTMENT_CODE.Contains(name))));
      if (sortDir.ToLower() == "asc")
        return source.OrderBy<LM_DEPARTMENT>(sort).ThenBy<LM_DEPARTMENT>("DEPARTMENT_NAME").Paginate<LM_DEPARTMENT>(page, size, 0);
      return source.OrderByDescending<LM_DEPARTMENT>(sort).ThenByDescending<LM_DEPARTMENT>("DEPARTMENT_NAME").Paginate<LM_DEPARTMENT>(page, size, 0);
    }

    public List<LM_DEPARTMENT> GetAll_List()
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false))).OrderBy((t => t.DEPARTMENT_NAME)).ToList<LM_DEPARTMENT>();
    }

    public List<LM_DEPARTMENT> GetChildDepartment(int parrentId)
    {
      if (parrentId == 0)
        return this.DbSet.Where((o => o.DEPARTMENT_PARENT_ID == (int?) parrentId && o.ISDELETE == false)).OrderBy<LM_DEPARTMENT>("DEPARTMENT_NAME").ToList<LM_DEPARTMENT>();
      return this.DbSet.Where((o => o.DEPARTMENT_PARENT_ID == (int?) parrentId && o.ISDELETE == false)).OrderBy<LM_DEPARTMENT>("DEPARTMENT_NAME").ToList<LM_DEPARTMENT>();
    }

    public List<LM_DEPARTMENT> GetChildDepartment(string partparrentId)
    {
      if (string.IsNullOrEmpty(partparrentId))
        return (List<LM_DEPARTMENT>) null;
      return this.DbSet.Where((o => o.DEPARTMENT_PATH.StartsWith(partparrentId))).OrderBy<LM_DEPARTMENT>("DEPARTMENT_PATH").ThenBy<LM_DEPARTMENT>("DEPARTMENT_NAME").ToList<LM_DEPARTMENT>();
    }

    public List<SelectListItem> GetListItemBase()
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false))).OrderBy((obj => obj.DEPARTMENT_PATH)).Select((obj => new SelectListItem()
      {
        Value = obj.LM_DEPARTMENT_ID.ToString(),
        Text = obj.DEPARTMENT_NAME
      })).ToList<SelectListItem>();
    }

    public List<ItemBase> GetListItemBase(List<int> ids)
    {
      return this.DbSet.AsNoTracking().Where((obj => (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && ids.Contains(obj.LM_DEPARTMENT_ID))).OrderBy((obj => obj.DEPARTMENT_PATH)).Select((obj => new ItemBase()
      {
        Id = obj.LM_DEPARTMENT_ID,
        Name = obj.DEPARTMENT_NAME,
        Code = obj.DEPARTMENT_CODE
      })).ToList<ItemBase>();
    }

    private string getSpaceLevel(int? level)
    {
      if (level.HasValue.Equals(false))
        return "";
      string str = "";
      for (int index = 0; index < level.Value; ++index)
        str += "     ";
      return str;
    }

    public List<int> GetAllByDepartmentID(string departmentPart)
    {
      return this.DbSet.AsNoTracking().Where((obj => (obj.ISDELETE.HasValue.Equals(false) || obj.ISDELETE.Value.Equals(false)) && obj.DEPARTMENT_PATH.Contains(departmentPart))).Select((obj => obj.LM_DEPARTMENT_ID)).ToList<int>();
    }

    public List<DEPARTMENTLIST> GetAllDepartmentByParent(string departmentPart)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.ISDELETE == false && obj.DEPARTMENT_PATH.IndexOf(departmentPart) > 0)).OrderBy((obj => obj.DEPARTMENT_NAME)).Select((obj => new DEPARTMENTLIST()
      {
        LM_DEPARTMENT_ID = obj.LM_DEPARTMENT_ID,
        DEPARTMENT_NAME = obj.DEPARTMENT_NAME
      })).ToList<DEPARTMENTLIST>();
    }

    public List<DEPARTMENTLIST> GetAllDepartmentByLevel(int Level)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.ISDELETE == false && obj.LEVELS == (int?) Level)).OrderBy((obj => obj.DEPARTMENT_NAME)).Select((obj => new DEPARTMENTLIST()
      {
        LM_DEPARTMENT_ID = obj.LM_DEPARTMENT_ID,
        DEPARTMENT_NAME = obj.DEPARTMENT_NAME
      })).ToList<DEPARTMENTLIST>();
    }

    public List<DEPARTMENTLIST> GetAllDepartmentByLevelVT(int Level)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.ISDELETE == false && obj.LEVELS == (int?) Level)).OrderBy((obj => obj.DEPARTMENT_NAME)).Select((obj => new DEPARTMENTLIST()
      {
        LM_DEPARTMENT_ID = obj.LM_DEPARTMENT_ID,
        DEPARTMENT_NAME = obj.DEPARTMENT_CODE
      })).ToList<DEPARTMENTLIST>();
    }

    public List<DEPARTMENTLIST> GetAllDepartmentById(int? DeparmentId)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.ISDELETE == false && (int?) obj.LM_DEPARTMENT_ID == DeparmentId)).OrderBy((obj => obj.DEPARTMENT_NAME)).Select((obj => new DEPARTMENTLIST()
      {
        LM_DEPARTMENT_ID = obj.LM_DEPARTMENT_ID,
        DEPARTMENT_NAME = obj.DEPARTMENT_NAME
      })).ToList<DEPARTMENTLIST>();
    }

    public bool ExistDeparmentLevel0(int? DeparmentId)
    {
      return this.DbSet.AsNoTracking().Where((obj => obj.ISDELETE == false && obj.LEVELS == 0 && obj.LM_DEPARTMENT_ID == DeparmentId)).Count<LM_DEPARTMENT>() > 0;
    }

    public string DeparmentNameById(int? DeparmentId)
    {
      var source = this.DbSet.AsNoTracking().Where((obj => obj.ISDELETE == false && obj.LM_DEPARTMENT_ID == DeparmentId)).OrderBy((obj => obj.DEPARTMENT_NAME)).Select(obj => new
      {
        DEPARTMENT_NAME = obj.DEPARTMENT_NAME
      });
      string str = "";
      var list = source.ToList();
      if (list.Count > 0)
        str = Convert.ToString(list[0].DEPARTMENT_NAME);
      return str;
    }

    public string GetListDepartment(string Ids)
    {
      if (string.IsNullOrEmpty(Ids))
        return string.Empty;
      string[] strArray = Ids.Split(',');
      List<string> stringList = new List<string>();
      if (strArray != null && strArray.Length > 0)
      {
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (!string.IsNullOrEmpty(strArray[index]))
          {
            int DepID = 0;
            if (int.TryParse(strArray[index], out DepID))
            {
              string departmentName = this.DbSet.AsNoTracking().FirstOrDefault<LM_DEPARTMENT>((obj => obj.LM_DEPARTMENT_ID == DepID)).DEPARTMENT_NAME;
              stringList.Add(departmentName);
            }
          }
        }
      }
      if (stringList != null && stringList.Count > 0)
        return string.Join(";", stringList.ToArray());
      return string.Empty;
    }

    public string GetListDepartmentCode(string Ids)
    {
      if (string.IsNullOrEmpty(Ids))
        return string.Empty;
      string[] strArray = Ids.Split(',');
      List<string> stringList = new List<string>();
      if (strArray != null && strArray.Length > 0)
      {
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (!string.IsNullOrEmpty(strArray[index]))
          {
            int DepID = 0;
            if (int.TryParse(strArray[index], out DepID))
            {
              string departmentCode = this.DbSet.AsNoTracking().FirstOrDefault<LM_DEPARTMENT>((obj => obj.LM_DEPARTMENT_ID == DepID)).DEPARTMENT_CODE;
              stringList.Add(departmentCode);
            }
          }
        }
      }
      if (stringList != null && stringList.Count > 0)
        return string.Join(";", stringList.ToArray());
      return string.Empty;
    }
  }
}
