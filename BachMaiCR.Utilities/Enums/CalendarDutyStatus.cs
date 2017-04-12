namespace BachMaiCR.Utilities.Enums
{
  public enum CalendarDutyStatus
  {
    [StringValue("Tạo mới")] Created = 1,
    [StringValue("Gửi duyệt")] SendApproved = 2,
    [StringValue("Duyệt")] Approved = 3,
    [StringValue("Hủy duyệt")] CancelApproved = 4,
  }
}
