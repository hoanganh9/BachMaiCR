using System;

namespace BachMaiCR.DBMapping.ModelsExt
{
  public class SearchTemplate
  {
    public int LM_DEPARTMENT_ID { get; set; }

    public string TEMPLATE_NAME { get; set; }

    public string ABBREVIATION { get; set; }

    public DateTime? DATE_APPROVED { get; set; }

    public string DATE_APPROVEDS { get; set; }

    public string ADMIN_USER_CREATE { get; set; }

    public string ADMIN_USER_APPROVED { get; set; }

    public int STATUS { get; set; }

    public string DEPARTMENTS { get; set; }
  }
}
