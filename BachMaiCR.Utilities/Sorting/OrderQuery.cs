using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BachMaiCR.Utilities
{
  public static class OrderQuery
  {
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
    {
      return OrderQuery.ApplyOrder<T>(source, property, "OrderBy");
    }

    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
    {
      return OrderQuery.ApplyOrder<T>(source, property, "OrderByDescending");
    }

    public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
    {
      return OrderQuery.ApplyOrder<T>((IQueryable<T>) source, property, "ThenBy");
    }

    public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
    {
      return OrderQuery.ApplyOrder<T>((IQueryable<T>) source, property, "ThenByDescending");
    }

    private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
    {
      string[] strArray = property.Split('.');
      Type type = typeof (T);
      ParameterExpression parameterExpression = Expression.Parameter(type, "x");
      Expression expression = (Expression) parameterExpression;
      foreach (string name in strArray)
      {
        PropertyInfo property1 = type.GetProperty(name);
        expression = (Expression) Expression.Property(expression, property1);
        type = property1.PropertyType;
      }
      LambdaExpression lambdaExpression = Expression.Lambda(typeof (Func<,>).MakeGenericType(typeof (T), type), expression, new ParameterExpression[1]
      {
        parameterExpression
      });
      return (IOrderedQueryable<T>) ((IEnumerable<MethodInfo>) typeof (Queryable).GetMethods()).Single<MethodInfo>((method => method.Name == methodName && method.IsGenericMethodDefinition && method.GetGenericArguments().Length == 2 && method.GetParameters().Length == 2)).MakeGenericMethod(typeof (T), type).Invoke(null, new object[2]
      {
        source,
        lambdaExpression
      });
    }
  }
}
