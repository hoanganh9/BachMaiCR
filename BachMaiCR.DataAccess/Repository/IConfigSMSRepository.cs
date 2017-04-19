using BachMaiCR.DBMapping.Models;
using BachMaiCR.DBMapping.ModelsExt;
using BachMaiCR.Utilities;

namespace BachMaiCR.DataAccess.Repository
{
  public interface IConfigSMSRepository : IRepository<CONFIG_SMS>
  {
    PagedList<CONFIG_SMS> GetAll(ConfigSMSSearch entity, int page, int size);
  }
}
