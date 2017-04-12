using System;
using System.Transactions;

namespace BachMaiCR.DataAccess
{
  public class Transaction : ITransaction, IDisposable
  {
    protected UnitOfWork uow { get; private set; }

    protected TransactionScope ts { get; private set; }

    public Transaction(UnitOfWork u)
    {
      this.uow = u;
      this.ts = new TransactionScope();
    }

    public void Commit()
    {
      this.uow.Save();
      this.ts.Complete();
    }

    public void Rollback()
    {
    }

    public void Dispose()
    {
      if (this.ts == null)
        return;
      this.ts.Dispose();
      this.ts = (TransactionScope) null;
      this.uow = (UnitOfWork) null;
    }
  }
}
