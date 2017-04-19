

namespace BachMaiCR.Utilities.Enums
{
  public enum CalendarChangeApproved
  {
    [StringValue("Thêm mới")] Created = 1,
    [StringValue("Gửi duyệt")] SendApproved = 2,
    [StringValue("Duyệt")] Approved = 3,
    [StringValue("Hủy Duyệt")] CancelApproved = 4,
  }
}
