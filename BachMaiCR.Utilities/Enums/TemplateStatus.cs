

namespace BachMaiCR.Utilities.Enums
{
  public enum TemplateStatus
  {
    [StringValue("Chờ ban hành")] Create = 1,
    [StringValue("Đã ban hành")] Aproved = 2,
    [StringValue("Hủy ban hành")] CancelAproved = 3,
  }
}
