using BachMaiCR.DBMapping.ModelsExt;
using System.Data.Entity;

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