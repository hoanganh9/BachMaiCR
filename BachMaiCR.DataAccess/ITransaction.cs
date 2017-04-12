using System;

namespace BachMaiCR.DataAccess
{
  public interface ITransaction : IDisposable
  {
    void Commit();

    void Rollback();
  }
}
