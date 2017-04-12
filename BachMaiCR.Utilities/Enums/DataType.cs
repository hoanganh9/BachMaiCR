namespace BachMaiCR.Utilities.Enums
{
  public enum DataType
  {
    [StringValue("Single line of text")] Type1 = 1,
    [StringValue("Multiple lines of text")] Type2 = 2,
    [StringValue("Number (1, 1.0, 100)")] Type3 = 3,
    [StringValue("Date and Time")] Type4 = 4,
    [StringValue("Lookup")] Type5 = 5,
    [StringValue("Yes/No (check box)")] Type6 = 6,
  }
}
