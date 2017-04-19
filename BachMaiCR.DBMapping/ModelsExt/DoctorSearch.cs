

namespace BachMaiCR.DBMapping.ModelsExt
{
  public class DoctorSearch
  {
    public string SearchName { get; set; }

    public string SearchCode { get; set; }

    public string SearchIdentity { get; set; }

    public string SearchAddress { get; set; }

    public string SearchPhone { get; set; }

    public string SearchEmail { get; set; }

    public string PathDepat { get; set; }

    public string SearchFeastName { get; set; }

    public int? SearchDeprtId { get; set; }

    public int? SearchDoctorLevelId { get; set; }

    public int? SearchDegreeId { get; set; }

    public int? SearchPositionId { get; set; }

    public int? SearchFeastId { get; set; }

    public int? CalendarYear { get; set; }

    public DoctorSearch()
    {
      this.SearchName = "";
      this.SearchCode = "";
      this.SearchIdentity = "";
      this.SearchEmail = "";
      this.SearchAddress = "";
      this.SearchPhone = "";
      this.SearchFeastName = "";
    }
  }
}
