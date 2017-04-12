using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Web.Common.Attributes;

namespace BachMaiCR.Web.Models
{
    public class TemplateModel
    {
        public int TEMPLATES_ID { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string TEMPLATE_NAME { get; set; }
        public string ABBREVIATION { get; set; }
        public Nullable<bool> ISDELETE { get; set; }
        public string USER_CREATE { get; set; }
        public Nullable<System.DateTime> DATE_CREATE { get; set; }     
        public string USER_APPROVED { get; set; }
        public Nullable<System.DateTime> DATE_APPROVED { get; set; }
        public Nullable<int> STATUS { get; set; }
        public string DESCRIPTION { get; set; }
        public virtual ICollection<TEMPLATE_COLUM> TEMPLATE_COLUM { get; set; }
        public string USER_CREATE_NAME { get; set; }
        public Nullable<System.DateTime> DATE_START { get; set; }
        public Nullable<System.DateTime> DATE_END { get; set; }
        /// <summary>
        /// hàm tạo đối tượng
        /// </summary>
        /// <param name="entity"></param>
        public TemplateModel(TEMPLATE entity)
        {
            this.TEMPLATES_ID = entity.TEMPLATES_ID;
            this.DEPARTMENT_NAME = entity.LM_DEPARTMENT_ID == null ? "N/A" : entity.LM_DEPARTMENT.DEPARTMENT_NAME;           
            this.TEMPLATE_NAME = entity.TEMPLATE_NAME;
            this.ABBREVIATION = entity.ABBREVIATION;
            this.USER_CREATE = entity.USER_CREATE;
            this.USER_CREATE_NAME = entity.ADMIN_USER.FULLNAME;
            this.DATE_CREATE = entity.DATE_CREATE;
            this.USER_APPROVED = entity.USER_APPROVED;
            this.DATE_APPROVED = entity.DATE_APPROVED;
            this.STATUS = entity.STATUS;
            this.DESCRIPTION = entity.DESCRIPTION;
            this.TEMPLATE_COLUM = entity.TEMPLATE_COLUM;
            this.ISDELETE = entity.ISDELETE;
            this.DATE_START = entity.DATE_START;
            this.DATE_END = entity.DATE_END;   
        }
        public TEMPLATE GetTemplateModel()
        {
            var entity = new TEMPLATE();
            entity.TEMPLATES_ID = this.TEMPLATES_ID;
            entity.STATUS = this.STATUS;
            entity.ISDELETE = this.ISDELETE;          
            return entity;
        }
        /// <summary>
        /// Lấy trạng thái template
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetStatus(object obj)
        {
            string status = Convert.ToString(obj);
            string vR="";
           
            if (status == "1")
            {
                vR = Resources.Localization.StatusCreate;
            }
            else if (status == "2")
            {
                vR = Resources.Localization.StatusAproved;
            }
            else if (status == "3")
            {
                vR = Resources.Localization.StatusCancelAproved;
            }
            return vR;
        }
     
    }
    public class ITEM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public partial class TEMPLATES
    {
        public int TEMPLATES_ID { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string TEMPLATE_NAME { get; set; }
        public string ABBREVIATION { get; set; }
        public string DOCTOR_NAME_CREATE { get; set; }
        public string USER_CREATE { get; set; }
        public string DATE_CREATE { get; set; }
        public string DOCTOR_NAME_APPROVED { get; set; }
        public string USER_APPROVED { get; set; }
        public string DATE_APPROVED { get; set; }
        public string STATUS { get; set; }       
    }
    /// <summary>
    /// Lop doi tuong dinh nghia trong muc lich ca nhan
    /// </summary>
    public class CalendarDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public long start { get; set; }
        public long end { get; set; }
        public string url { get; set; }
        public bool allDay { get; set; }
    }
    public class CALENDAR
    {
        public DateTime DATE_START { get; set; }
        public DateTime? DATE_END { get; set; }

        public int DOCTORS_ID { get; set; }
        public int DUTY_TYPE { get; set; }
        public string CONTENT { get; set; }
        public int LM_DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public int CALENDAR_DUTY_ID { get; set; }
       
    }
    public class DOCTOR_NAME_LIST
    {
        public string DOCTOR_NAME { get; set; }
    }
    public partial class TEMPLATE_MAP
    {
       

        public int TEMPLATES_ID { get; set; }
        public string TEMPLATE_NAME { get; set; }
        public string ABBREVIATION { get; set; }
        public Nullable<System.DateTime> DATE_START { get; set; }
        public Nullable<System.DateTime> DATE_END { get; set; }
        public Nullable<int> STATUS { get; set; }
        public Nullable<System.DateTime> DATE_CREATE { get; set; }
        public Nullable<int> USER_CREATE_ID { get; set; }
        public string USER_CREATE { get; set; }
        public Nullable<System.DateTime> DATE_APPROVED { get; set; }
        public Nullable<int> USER_APPROVED_ID { get; set; }
        public string USER_APPROVED { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<bool> ISDELETE { get; set; }
        public Nullable<int> LM_DEPARTMENT_ID { get; set; }
        public virtual ADMIN_USER ADMIN_USER { get; set; }
        public virtual ADMIN_USER ADMIN_USER1 { get; set; }
        public virtual LM_DEPARTMENT LM_DEPARTMENT { get; set; }
        public virtual ICollection<TEMPLATE_COLUM> TEMPLATE_COLUM { get; set; }
    }
    /// <summary>
    /// Lưu trữ lịch động
    /// </summary>
    public class AUTO_CALENDAR_DOCTOR
    {
      
        public string COLUM_NAME { get; set; }
        public string LEVEL_NAME { get; set; }

        public string DOCTOR_NAME { get; set; }
        public string TEMPLATE_NAME { get; set; }
        public int DOCTORS_ID { get; set; }
        public int ROW_COUNTER { get; set; }
        public int TEMPLATE_COLUM_ID { get; set; }
        public int COLUM_ORDER { get; set; }
        public int TEMPLATES_ID { get; set; }
        public int TIME_DISTANCE { get; set; }

        public int NORMS_DIRECT { get; set; }
        public int COUNT_DIRECT { get; set; }
        public int COUNT_HOLIDAY { get; set; }
        public int COUNT_NUMBER_DIRECT { get; set; }
        public DateTime? DATE_START { get; set; }
        public DateTime? DATE_CONTINUE { get; set; }
      

    }
    public class AUTO_CALENDAR_LEADER
    {

     
        public int DOCTORS_ID { get; set; }
        public string DOCTOR_NAME { get; set; }
        public string LM_DEPARTMENT_IDs { get; set; }
        public int ROW_COUNTER { get; set; }
      
        public int TIME_DISTANCE { get; set; }

        public int NORMS_DIRECT { get; set; }
        public int COUNT_DIRECT { get; set; }
        public int COUNT_HOLIDAY { get; set; }
        public int COUNT_NUMBER_DIRECT { get; set; }
        public DateTime? DATE_START { get; set; }
        public DateTime? DATE_CONTINUE { get; set; }
        public int AUTO_CALENDAR { get; set; }

    }
}
