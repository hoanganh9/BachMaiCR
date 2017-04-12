using System.Data.Entity;
using BachMaiCR.DBMapping.ModelsExt;

namespace BachMaiCR.DBMapping
{
  public class IgnoreContext
  {
    public void Ignore(DbModelBuilder modelBuilder)
    {
      modelBuilder.Ignore<DoctorX>();
    }
  }
}
