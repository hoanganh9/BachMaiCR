namespace BachMaiCR.Utilities.Enums
{
  public enum enActionType
  {
    [StringValue("Xem thông tin")] View = 0,
    [StringValue("Cập nhật thông tin")] Update = 1,
    [StringValue("Xóa thông tin")] Delete = 2,
    [StringValue("Xuất dữ liệu ra file")] ExportExcel = 3,
    [StringValue("Nhập dữ liệu từ file")] ImporttExcel = 4,
    [StringValue("Thêm mới")] Insert = 5,
    [StringValue("Gửi duyệt")] SendApproved = 6,
    [StringValue("Phê duyệt")] Approve = 7,
    [StringValue("Hủy duyệt")] ApproveCancel = 8,
    [StringValue("Gửi SMS")] SendSMS = 9,
    [StringValue("Phân quyền")] Permission = 10,
    [StringValue("Hành động khác")] Other = 100,
  }
}
