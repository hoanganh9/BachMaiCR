using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BachMaiCR.Web.Common
{
    public class WebConst
    {
        #region ---------- Quản trị hệ thống --------------
        public const string CacheUserPrefix = @"LOGGED_IN_USER_";

        public const string CacheSessionToken = @"CACHE_SESSION_TOKEN";

        public const string DefaultAdminAccount = @"admin";

        public const string QTHT_GROUP_CODE = @"QTHT_GROUP_CODE";

        public const string QTHT_GROUP_NAME = @"Quản trị hệ thống";

        public const string USERS_GROUP_CODE = @"USERS_GROUP_CODE";

        public const string USERS_GROUP_NAME = @"Quản lý người dùng";

        public const string ROLES_GROUP_NAME = @"Quản lý nhóm quyền";

        public const string ROLES_GROUP_CODE = @"ROLES_GROUP_CODE";

        public const string ACTIONS_GROUP_NAME = @"Quản lý chức năng";

        public const string ACTIONS_GROUP_CODE = @"ACTIONS_GROUP_CODE";
        #endregion

        #region ----------- Quản lý nhóm quyền -----------
        public const string MENU_ROLE_NAME = "Xem danh sách nhóm quyền";
        public const string MENU_ROLE_CODE = "Admin_ManageRoles";

        public const string ACTION_ROLE_ADD_NAME = "Cập nhật nhóm quyền";
        public const string ACTION_ROLE_ADD_CODE = "WebpageRoles_AddRole";

        public const string ACTION_ROLE_ADDUSER_NAME = "Thêm người dùng";
        public const string ACTION_ROLE_ADDUSER_CODE = "WebpageRoles_AddUser";

        public const string ACTION_ROLE_DELETE_NAME = "Xóa nhóm quyền";
        public const string ACTION_ROLE_DELETE_CODE = "WebpageRoles_Delete";

        public const string ACTION_ROLE_ACTIVE_NAME = "active/deactive nhóm quyền";
        public const string ACTION_ROLE_ACTIVE_CODE = "WebpageRoles_UpdateActive";

        public const string ACTION_ROLE_UPDATEPER_NAME = "Cập nhật quyền cho nhóm quyền";
        public const string ACTION_ROLE_UPDATEPER_CODE = "WebpageRoles_UpdateRoleWithActions";

        public const string ACTION_ROLE_GETACTIONS_NAME = "Lấy danh sách chức năng theo nhóm quyền";
        public const string ACTION_ROLE_GETACTIONS_CODE = "WebpageRoles_GetActionIds";
        #endregion

        #region ----------- Quản lý Người dùng -----------
        public const string MENU_USER_NAME = "Xem danh sách người dùng";
        public const string MENU_USER_CODE = "Admin_ManageUsers";

        public const string ACTION_USER_ADD_NAME = "Cập nhật người dùng";
        public const string ACTION_USER_ADD_CODE = "Users_AddUser";

        public const string ACTION_USER_RESETPASS_NAME = "Reset mật khẩu người dùng";
        public const string ACTION_USER_RESETPASS_CODE = "Users_ResetPass";

        public const string ACTION_USER_DELETE_NAME = "Xóa người dùng";
        public const string ACTION_USER_DELETE_CODE = "Users_Delete";

        public const string ACTION_USER_ACTIVE_NAME = "Active/deactive người dùng";
        public const string ACTION_USER_ACTIVE_CODE = "Users_ActiveChage";

        public const string ACTION_USER_UPDATEPER_NAME = "Phân quyền truy cập cho người dùng";
        public const string ACTION_USER_UPDATEPER_CODE = "Users_SaveConfigUser";

        #endregion

        #region ----------- Quản lý chức năng -----------
        public const string MENU_ACTION_NAME = "Xem danh sách chức năng";
        public const string MENU_ACTION_CODE = "Admin_ManageActions";

        public const string ACTION_ACTION_UPDATE_NAME = "Cập nhật năng vào CSDL";
        public const string ACTION_ACTION_UPDATE_CODE = "Admin_GenerateWebPageActions";

        public const string ACTION_ACTION_ACTIVE_NAME = "Active/deactive chức năng";
        public const string ACTION_ACTION_ACTIVE_CODE = "WebpageActions_UpdateActive";

        public const string ACTION_ACTION_EDIT_NAME = "Sửa chức năng";
        public const string ACTION_ACTION_EDIT_CODE = "WebpageActions_AddAction";
        #endregion

        #region ----------- MENU DOCTOR -----------
        public const string MENU_DOCTOR_NAME = "Danh mục cán bộ";
        public const string MENU_DOCTOR_CODE = "DOCTOR";
        public const string ACTION_DOCTOR_SAVE_NAME = "Cập nhật thông tin";
        public const string ACTION_DOCTOR_SAVE_CODE = "DOCTOR_UPDATE";

        public const string ACTION_DOCTOR_EXPORT_NAME = "Xuất dữ liệu Excel";
        public const string ACTION_DOCTOR_EXPORT_CODE = "DOCTOR_EXPORT";

        public const string ACTION_DOCTOR_DELETE_NAME = "Xóa thông tin";
        public const string ACTION_DOCTOR_DELETE_CODE = "DOCTOR_DELETE";

        public const string ACTION_DOCTOR_IMPORT_NAME = "Nhập dữ liệu Excel";
        public const string ACTION_DOCTOR_IMPORT_CODE = "DOCTOR_IMPORT";

        public const string ACTION_DOCTOR_VIEW_NAME = "Xem hồ sơ cán bộ";
        public const string ACTION_DOCTOR_VIEW_CODE = "DOCTOR_VIEW";


        public const string ACTION_DOCTOR_ACTIVE_NAME = "Active/deactive cán bộ";
        public const string ACTION_DOCTOR_ACTIVE_CODE = "DOCTOR_ACTIVE";
        #endregion
        #region ----------- MENU CONFIG HOLIDAY -----------
        public const string MENU_CONFIG_HOLIDAY_NAME = "Danh sách cán bộ nghỉ";
        public const string MENU_CONFIG_HOLIDAY_CODE = "CONFIG_HOLIDAYS";
        public const string ACTION_CONFIG_HOLIDAY_SAVE_NAME = "Cập nhật thông tin cán bộ nghỉ";
        public const string ACTION_CONFIG_HOLIDAY_SAVE_CODE = "CONFIG_HOLIDAYS_UPDATE";
        public const string ACTION_CONFIG_HOLIDAY_DELETE_NAME = "Xóa thông tin cán bộ nghỉ";
        public const string ACTION_CONFIG_HOLIDAY_DELETE_CODE = "CONFIG_HOLIDAYS_DELETE";

        public const string ACTION_CONFIG_HOLIDAY_VIEW_NAME = "Xem danh sách cán bộ nghỉ";
        public const string ACTION_CONFIG_HOLIDAY_VIEW_CODE = "CONFIG_HOLIDAYS_VIEW";
        #endregion

        #region ----------- MENU CONFIG SMS -----------
        public const string MENU_CONFIG_SMS_NAME = "Cấu hình gửi SMS";
        public const string MENU_CONFIG_SMS_CODE = "CONFIG_SMS";
        public const string ACTION_CONFIG_SMS_SAVE_NAME = "Cập nhật thông tin";
        public const string ACTION_CONFIG_SMS_SAVE_CODE = "CONFIG_SMS_UPDATE";
        public const string ACTION_CONFIG_SMS_DELETE_NAME = "Xóa thông tin";
        public const string ACTION_CONFIG_SMS_DELETE_CODE = "CONFIG_SMS_DELETE";

        public const string ACTION_CONFIG_SMS_VIEW_NAME = "Xem danh sách";
        public const string ACTION_CONFIG_SMS_VIEW_CODE = "CONFIG_SMS_VIEW";
        public const string ACTION_CONFIG_SMS_ACTIVE_NAME = "Active/deactive cấu hình";
        public const string ACTION_CONFIG_SMS_ACTIVE_CODE = "CONFIG_SMS_ACTIVE";
        #endregion
        #region ----------- MENU CONFIG DIRECT -----------
        public const string MENU_CONFIG_DIRECT_NAME = "Danh sách cán bộ đi trực";
        public const string MENU_CONFIG_DIRECT_CODE = "CONFIG_DIRECT";
        public const string ACTION_CONFIG_DIRECT_SAVE_NAME = "Cập nhật thông tin cán bộ trực";
        public const string ACTION_CONFIG_DIRECT_SAVE_CODE = "CONFIG_DIRECT_UPDATE";
        public const string ACTION_CONFIG_DIRECT_DELETE_NAME = "Xóa thông tin cán bộ trực";
        public const string ACTION_CONFIG_DIRECT_DELETE_CODE = "CONFIG_DIRECT_DELETE";

        public const string ACTION_CONFIG_DIRECT_VIEW_NAME = "Xem danh sách cán bộ trực";
        public const string ACTION_CONFIG_DIRECT_VIEW_CODE = "CONFIG_DIRECT_VIEW";
        #endregion

        #region ----------- MENU CATEGORY -----------
        public const string MENU_CATEGORY_NAME = "Danh mục Chức danh/Học hàm - Học vị/Loại ngày nghỉ";
        public const string MENU_CATEGORY_CODE = "CATEGORY";
        public const string ACTION_CATEGORY_DELETE_NAME = "Xóa danh mục";
        public const string ACTION_CATEGORY_DELETE_CODE = "CATEGORY_DELETE";

        public const string ACTION_CATEGORY_SAVE_NAME = "Cập nhật danh mục";
        public const string ACTION_CATEGORY_SAVE_CODE = "CATEGORY_SAVE";

        public const string ACTION_CATEGORY_VIEW_NAME = "Xem thông tin danh mục";
        public const string ACTION_CATEGORY_VIEW_CODE = "CATEGORY_VIEW";
        #endregion
        #region ----------- MENU FEAST -----------
        public const string MENU_FEAST_NAME = "Danh mục ngày nghỉ lễ";
        public const string MENU_FEAST_CODE = "FEAST";
        public const string ACTION_FEAST_DELETE_NAME = "Xóa danh mục ngày nghỉ lễ";
        public const string ACTION_FEAST_DELETE_CODE = "FEAST_DELETE";

        public const string ACTION_FEAST_SAVE_NAME = "Cập nhật danh mục ngày nghỉ lễ";
        public const string ACTION_FEAST_SAVE_CODE = "FEAST_SAVE";

        public const string ACTION_FEAST_VIEW_NAME = "Xem thông tin danh mục ngày nghỉ lễ";
        public const string ACTION_FEAST_VIEW_CODE = "FEAST_VIEW";
        #endregion  

        #region ----------- MENU DEPARTMENT -----------
        public const string MENU_DEPARTMENT_NAME = "Quản lý phòng ban";
        public const string MENU_DEPARTMENT_CODE = "DEPARTMENT";
        public const string ACTION_DEPARTMENT_DELETE_NAME = "Xóa phòng ban";
        public const string ACTION_DEPARTMENT_DELETE_CODE = "DEPARTMENT_DELETE";

        public const string ACTION_DEPARTMENT_SAVE_NAME = "Cập nhật thông tin";
        public const string ACTION_DEPARTMENT_SAVE_CODE = "DEPARTMENT_SAVE";

        public const string ACTION_DEPARTMENT_VIEW_NAME = "Xem phòng ban";
        public const string ACTION_DEPARTMENT_VIEW_CODE = "DEPARTMENT_VIEW";
        #endregion

        #region ----------- MENU REPORTOFDAY -----------
        public const string MENU_REPORTOFDAY_NAME = "Báo cáo hàng ngày";
        public const string MENU_REPORTOFDAY_CODE = "REPORTOFDAY";
        public const string ACTION_REPORTOFDAY_DELETE_NAME = "Xóa báo cáo hàng ngày";
        public const string ACTION_REPORTOFDAY_DELETE_CODE = "REPORTOFDAY_DELETE";
        public const string ACTION_REPORTOFDAY_SAVE_NAME = "Cập nhật thông tin";
        public const string ACTION_REPORTOFDAY_SAVE_CODE = "REPORTOFDAY_SAVE";
        public const string ACTION_REPORTOFDAY_VIEW_NAME = "Xem báo cáo hàng ngày";
        public const string ACTION_REPORTOFDAY_VIEW_CODE = "REPORTOFDAY_VIEW";

        #endregion

        #region ----------- MENU REPORTOFHOSPITAL -----------
        public const string MENU_REPORTOFHOSPITAL_NAME = "Thống kê số ca trực toàn viện";
        public const string MENU_REPORTOFHOSPITAL_CODE = "REPORTOFHOSPITAL";

        public const string ACTION_REPORTOFHOSPITAL_EXPORT_NAME = "Xuất Excel";
        public const string ACTION_REPORTOFHOSPITAL_EXPORT_CODE = "REPORTOFHOSPITAL_EXPORT";

        #endregion

        #region ----------- MENU REPORTOFHOLIDAY -----------
        public const string MENU_REPORTOFHOLIDAY_NAME = "Thống kê cán bộ trực theo ngày nghỉ lễ";
        public const string MENU_REPORTOFHOLIDAY_CODE = "REPORTOFHOLIDAY";

        public const string ACTION_REPORTOFHOLIDAY_EXPORT_NAME = "Xuất Excel";
        public const string ACTION_REPORTOFHOLIDAY_EXPORT_CODE = "REPORTOFHOLIDAY_EXPORT";

        #endregion


        #region ----------- MENU DOCTORLEVEL -----------
        public const string MENU_DOCTORLEVEL_NAME = "Danh mục vị trí cán bộ";
        public const string MENU_DOCTORLEVEL_CODE = "DOCTORLEVEL";

        public const string ACTION_DOCTORLEVEL_SAVE_NAME = "Cập nhật thông tin";
        public const string ACTION_DOCTORLEVEL_SAVE_CODE = "DOCTORLEVEL_SAVE";

        public const string ACTION_DOCTORLEVEL_DELETE_NAME = "Xóa thông tin vị trí";
        public const string ACTION_DOCTORLEVEL_DELETE_CODE = "DOCTORLEVEL_DELETE";

        public const string ACTION_DOCTORLEVEL_VIEW_NAME = "Xem thông tin vị trí";
        public const string ACTION_DOCTORLEVEL_VIEW_CODE = "DOCTORLEVEL_VIEW";
        #endregion

        #region ----------- MENU ADMIN LOG -----------
        public const string MENU_ADMINLOG_NAME = "Nhật ký người dùng";
        public const string MENU_ADMINLOG_CODE = "ADMINLOG";      

        public const string ACTION_ADMINLOG_INSERT_NAME = "Thêm nhật ký người dùng";
        public const string ACTION_ADMINLOG_INSERT_CODE = "ADMINLOG_INSERT";

        public const string ACTION_ADMINLOG_EDIT_NAME = "Sửa nhật ký người dùng";
        public const string ACTION_ADMINLOG_EDIT_CODE = "ADMINLOG_EDIT";

        public const string ACTION_ADMINLOG_DELETE_NAME = "Xóa nhật ký người dùng";
        public const string ACTION_ADMINLOG_DELETE_CODE = "ADMINLOG_DELETE";

        public const string ACTION_ADMINLOG_SAVE_NAME = "Cập nhật thông tin";
        public const string ACTION_ADMINLOG_SAVE_CODE = "ADMINLOG_SAVE";

        public const string ACTION_ADMINLOG_VIEW_NAME = "Xem nhật ký người dùng";
        public const string ACTION_ADMINLOG_VIEW_CODE = "ADMINLOG_VIEW";
        #endregion

        public const string ACTION_INSERT_NAME = "Thêm thông tin";
        public const string ACTION_EDIT_NAME = "Sửa thông tin";
        public const string ACTION_DELETE_NAME = "Xóa thông tin";
        public const string ACTION_SAVE_NAME = "Cập nhật thông tin";
        public const string ACTION_VIEW_NAME = "Xem thông tin";
        public const string ACTION_INDEX_NAME = "Truy cập chức năng";

        public const string ACTION_INSERT_CODE = "Insert";
        public const string ACTION_EDIT_CODE = "Edit";
        public const string ACTION_DELETE_CODE = "Delete";
        public const string ACTION_SAVE_CODE = "Save";
        public const string ACTION_VIEW_CODE = "View";
        public const string ACTION_INDEX_CODE = "Index";

        //Chuc nang tao lap template
        #region ---------- Cấu hình biểu mẫu --------------
        public const string MENU_TEMPLATE_NAME = "Cấu hình biểu mẫu";
        public const string MENU_TEMPLATE_CODE = "Template";

        public const string ACTION_VIEW_TEMPLATE_NAME = "Xem biểu mẫu";
        public const string ACTION_VIEW_TEMPLATE_CODE = "Template_View";

        public const string ACTION_INSERT_TEMPLATE_NAME = "Lập biểu mẫu";
        public const string ACTION_INSERT_TEMPLATE_CODE = "Template_Insert";

        public const string ACTION_EDIT_TEMPLATE_NAME = "Sửa biểu mẫu";
        public const string ACTION_EDIT_TEMPLATE_CODE = "Template_Edit";

        public const string ACTION_DELETE_TEMPLATE_NAME = "Xóa biểu mẫu";
        public const string ACTION_DELETE_TEMPLATE_CODE = "Template_Deleted";

        public const string ACTION_APPROVED_TEMPLATE_NAME = "Phê duyệt biểu mẫu";
        public const string ACTION_APPROVED_TEMPLATE_CODE = "Template_Approved";
        #endregion
        #region ---------- Tạo lập lịch lãnh đạo --------------

        public const string MENU_CALENDAR_LEADER_NAME = "Lịch lãnh đạo";
        public const string MENU_CALENDAR_LEADER_CODE = "CalendarDutyLeader";

        public const string ACTION_VIEW_CALENDAR_LEADER_NAME = "Xem lịch lãnh đạo";
        public const string ACTION_VIEW_CALENDAR_LEADER_CODE = "CalendarDutyLeader_View";

        public const string ACTION_INSERT_CALENDAR_LEADER_NAME = "Lập lịch lãnh đạo";
        public const string ACTION_INSERT_CALENDAR_LEADER_CODE = "CalendarDutyLeader_Insert";

        public const string ACTION_EDIT_CALENDAR_LEADER_NAME = "Sửa lịch lãnh đạo";
        public const string ACTION_EDIT_CALENDAR_LEADER_CODE = "CalendarDutyLeader_Edit";

        public const string ACTION_DELETE_CALENDAR_LEADER_NAME = "Xóa lịch lãnh đạo";
        public const string ACTION_DELETE_CALENDAR_LEADER_CODE = "CalendarDutyLeader_Deleted";

        public const string ACTION_APPROVED_CALENDAR_LEADER_NAME = "Phê duyệt lịch lãnh đạo";
        public const string ACTION_APPROVED_CALENDAR_LEADER_CODE = "CalendarDutyLeader_Approved";


        public const string ACTION_SEND_CALENDAR_LEADER_NAME = "Gửi duyệt lịch lãnh đạo";
        public const string ACTION_SEND_CALENDAR_LEADER_CODE = "CalendarDutyLeader_Send";

        public const string ACTION_CANCEL_APPROVED_CALENDAR_LEADER_NAME = "Hủy duyệt lịch lãnh đạo";
        public const string ACTION_CANCEL_APPROVED_CALENDAR_LEADER_CODE = "CalendarDutyLeader_CancelApproved";

        public const string ACTION_EXPORT_CALENDAR_LEADER_NAME = "Xuất dữ liệu Excel";
        public const string ACTION_EXPORT_CALENDAR_LEADER_CODE = "CalendarDutyLeader_Export";
        #endregion
        #region ---------- LỊCH TOÀN BỆNH VIỆN --------------

        public const string MENU_CALENDAR_HOSPITAL_NAME = "Lịch trực toàn Bệnh viện";
        public const string MENU_CALENDAR_HOSPITAL_CODE = "CalendarDutyHospital";

        public const string ACTION_VIEW_CALENDAR_HOSPITAL_NAME = "Xem lịch trực";
        public const string ACTION_VIEW_CALENDAR_HOSPITAL_CODE = "CalendarDutyHospital_List";

        public const string ACTION_SYNTHESIS_CALENDAR_HOSPITAL_NAME = "Tổng hợp lịch trực";
        public const string ACTION_SYNTHESIS_CALENDAR_HOSPITAL_CODE = "CalendarDutyHospital_View";

    
     

 
        #endregion
        #region ---------- Lịch thường trú ban giám đốc --------------

        public const string MENU_CALENDAR_DIRECTORS_NAME = "Lịch thường trú Ban giám đốc";
        public const string MENU_CALENDAR_DIRECTORS_CODE = "CalendarDutyDirectors";

        public const string ACTION_VIEW_CALENDAR_DIRECTORS_NAME = "Xem bảng thường trú Ban giám đốc";
        public const string ACTION_VIEW_CALENDAR_DIRECTORS_CODE = "CalendarDutyDirectors_View";

        public const string ACTION_INSERT_CALENDAR_DIRECTORS_NAME = "Lập bảng thường trú Ban giám đốc";
        public const string ACTION_INSERT_CALENDAR_DIRECTORS_CODE = "CalendarDutyDirectors_Insert";

        public const string ACTION_EDIT_CALENDAR_DIRECTORS_NAME = "Sửa bảng thường trú Ban giám đốc";
        public const string ACTION_EDIT_CALENDAR_DIRECTORS_CODE = "CalendarDutyDirectors_Edit";

        public const string ACTION_DELETE_CALENDAR_DIRECTORS_NAME = "Xóa bảng thường trú Ban giám đốc";
        public const string ACTION_DELETE_CALENDAR_DIRECTORS_CODE = "CalendarDutyDirectors_Deleted";

        public const string ACTION_APPROVED_CALENDAR_DIRECTORS_NAME = "Phê duyệt bảng thường trú Ban giám đốc";
        public const string ACTION_APPROVED_CALENDAR_DIRECTORS_CODE = "CalendarDutyDirectors_Approved";

        public const string ACTION_SEND_CALENDAR_DIRECTORS_NAME = "Gửi duyệt bảng thường trú Ban giám đốc";
        public const string ACTION_SEND_CALENDAR_DIRECTORS_CODE = "CalendarDutyDirectors_Send";

        public const string ACTION_CANCEL_APPROVED_CALENDAR_DIRECTORS_NAME = "Hủy duyệt bảng thường trú Ban giám đốc";
        public const string ACTION_CANCEL_APPROVED_CALENDAR_DIRECTORS_CODE = "CalendarDutyDirectors_CancelApproved";

        public const string ACTION_EXPORT_CALENDAR_DIRECTORS_NAME = "Xuất dữ liệu Excel";
        public const string ACTION_EXPORT_CALENDAR_DIRECTORS_CODE = "CalendarDutyDirectors_Export";


        public const string ACTION_IMPORT_CALENDAR_DIRECTORS_NAME = "Nhập dữ liệu Excel";
        public const string ACTION_IMPORT_CALENDAR_DIRECTORS_CODE = "CalendarDutyDirectors_Import";
        #endregion
        #region ---------- Chức năng quản lý lịch trực đơn vị --------------
        public const string CALENDAR_DUTY_GROUP_NAME = @"Lịch trực Khoa/Viện/TT";
        public const string CALENDAR_DUTY_GROUP_CODE = @"CalendarDuty";

        public const string ACTION_INSERT_CALENDAR_DUTY_NAME = "Lập lịch trực";
        public const string ACTION_EDIT_CALENDAR_DUTY_NAME = "Sửa thông tin lịch trực";
        public const string ACTION_DELETE_CALENDAR_DUTY_NAME = "Xóa thông tin lịch trực";
        public const string ACTION_VIEW_CALENDAR_DUTY_NAME = "Xem thông tin lịch trực";
        public const string ACTION_INDEX_CALENDAR_DUTY_NAME = "Danh sách lịch trực";
        public const string ACTION_SEND_CALENDAR_DUTY_NAME = "Gửi duyệt lịch trực";
        public const string ACTION_CANCEL_CALENDAR_DUTY_NAME = "Hủy duyệt lịch trực";
        public const string ACTION_APPROVED_CALENDAR_DUTY_NAME = "Duyệt lịch trực";
        public const string ACTION_SEARCH_CALENDAR_DUTY_NAME = "Tìm kiếm lịch trực";

        public const string ACTION_EXPORT_CALENDAR_DUTY_NAME = "Xuất Excel lịch trực";
        public const string ACTION_IMPORT_EXCEL_CALENDAR_DUTY_NAME ="Nhập lịch trực từ Excel";
        public const string ACTION_HISTORY_CALENDAR_DUTY_NAME = "Lịch sử đổi lịch trực";

        
        public const string ACTION_INSERT_CALENDAR_DUTY_CODE = "CalendarDuty_Insert";
        public const string ACTION_EDIT_CALENDAR_DUTY_CODE = "CalendarDuty_Edit";
        public const string ACTION_DELETE_CALENDAR_DUTY_CODE = "CalendarDuty_Delete";
        public const string ACTION_VIEW_CALENDAR_DUTY_CODE = "CalendarDuty_View";
        public const string ACTION_INDEX_CALENDAR_DUTY_CODE = "CalendarDuty_Index";
        public const string ACTION_SEND_CALENDAR_DUTY_CODE = "CalendarDuty_Send";
        public const string ACTION_CANCEL_CALENDAR_DUTY_CODE = "CalendarDuty_Cancel";
        public const string ACTION_APPROVED_CALENDAR_DUTY_CODE = "CalendarDuty_Approved";
        public const string ACTION_SEARCH_CALENDAR_DUTY_CODE = "CalendarDuty_Search";


        public const string ACTION_EXPORT_EXCEL_CALENDAR_DUTY_CODE = "CalendarDuty_Export";
        public const string ACTION_IMPORT_EXCEL_CALENDAR_DUTY_CODE = "CalendarDuty_Import";
        public const string ACTION_HISTORY_EXCEL_CALENDAR_DUTY_CODE = "CalendarDuty_History";
      
        #endregion
        #region "--------Lịch cá nhân"
        public const string MENU_PERSONAL_CALENDAR_NAME = "Lịch cá nhân";
        public const string MENU_PERSONAL_CALENDAR_CODE = "PersonalCalendar";

        public const string ACTION_VIEW_PERSONAL_CALENDAR_NAME = "Xem lịch cá nhân";
        public const string ACTION_VIEW_PERSONAL_CALENDAR_CODE = "PersonalCalendar_View";
        #endregion
    }
}