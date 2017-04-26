namespace BachMaiCR.Utilities.Enums
{
    public enum CalendarChangeType
    {
        [StringValue("Đổi lịch")] Change = 1,
        [StringValue("Xóa lịch")] Deleted = 2,
        [StringValue("Thêm mới")] Add = 3,
    }
}