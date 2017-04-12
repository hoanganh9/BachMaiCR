using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BachMaiCR.Utilities
{
  public static class ExpressionHelper
  {
    public static Expression<T> Compose<T>(this LambdaExpression first, Expression<T> second, Func<Expression, Expression, Expression> merge)
    {
      Expression expression = ParameterRebinder.ReplaceParameters(first.Parameters.Select((f, i) =>
      {
        var data = new{ f = f, s = second.Parameters[i] };
        return data;
      }).ToDictionary(p => p.s, p => p.f), second.Body);
      return Expression.Lambda<T>(merge(first.Body, expression), (IEnumerable<ParameterExpression>) first.Parameters);
    }

    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
      return first.Compose<Func<T, bool>>(second, new Func<Expression, Expression, Expression>(Expression.And));
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
      return first.Compose<Func<T, bool>>(second, new Func<Expression, Expression, Expression>(Expression.Or));
    }

    public static bool Match<T>(this Expression<Func<T, bool>> spec, T target) where T : class
    {
      if (spec == null)
        throw new ArgumentNullException("spec");
      if ((object) target == null)
        throw new ArgumentNullException("target");
      return spec.Compile()(target);
    }
  }
}
