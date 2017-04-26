namespace BachMaiCR.Utilities.Enums
{
    public enum Step
    {
        [StringValue("Đã có thể sử dụng")] Allow,
        [StringValue("Chờ active")] Active,
        [StringValue("Chờ thay đổi mật khâu")] ChangePass,
        [StringValue("Chờ đăng ký CA")] RegistCa,
    }
}