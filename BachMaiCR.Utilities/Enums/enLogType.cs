namespace BachMaiCR.Utilities.Enums
{
    public enum enLogType
    {
        [StringValue("Log thông thường")] NomalLog,
        [StringValue("Log theo người dùng")] UserLog,
        [StringValue("Log hệ thống")] SystemLog,
        [StringValue("Log tấn công")] AttackLog,
    }
}