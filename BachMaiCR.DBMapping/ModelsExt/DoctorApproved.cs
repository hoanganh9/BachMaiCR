

namespace BachMaiCR.DBMapping.ModelsExt
{
  public class DoctorApproved
  {
    public int idItem { get; set; }

    public int idDoctor { get; set; }

    public int idCalendarduty { get; set; }

    public int idColumn { get; set; }

    public string dateStart { get; set; }

    public int typeAction { get; set; }

    public int idDoctorChange { get; set; }
  }
}
