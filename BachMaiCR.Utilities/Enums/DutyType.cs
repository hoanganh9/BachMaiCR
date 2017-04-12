namespace BachMaiCR.Utilities.Enums
{
  public enum DutyType
  {
    [StringValue("Lịch lãnh đạo")] Leader = 1,
    [StringValue("Thường trú Ban giám đốc")] Directors = 2,
    [StringValue("Lịch Phòng ban/Khoa/Viện/Trung tâm")] Deparment = 3,
    [StringValue("Lịch toàn bệnh viên")] Hospital = 4,
  }
}
