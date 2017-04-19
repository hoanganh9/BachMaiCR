using System;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;

namespace BachMaiCR.DataAccess
{
  public static class ContextExtensions
  {
    public static string GetTableName<T>(this DbContext context) where T : class
    {
      return ((IObjectContextAdapter) context).ObjectContext.GetTableName<T>();
    }

    public static string GetTableKeyName<T>(this DbContext context) where T : class
    {
      return ((IObjectContextAdapter) context).ObjectContext.GetTableKeyName<T>();
    }

    public static string GetTableKeyName<T>(this ObjectContext context) where T : class
    {
      return context.CreateObjectSet<T>().EntitySet.ElementType.KeyMembers.Select(k => k.Name).FirstOrDefault();
    }

    public static string GetTableName<T>(this ObjectContext context) where T : class
    {
      return new Regex("FROM (?<table>.*) AS").Match(context.CreateObjectSet<T>().ToTraceString()).Groups["table"].Value;
    }
  }
}
