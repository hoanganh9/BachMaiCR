using System.Collections.Generic;
using System.Linq.Expressions;

namespace BachMaiCR.Utilities
{
  public class ParameterRebinder : ExpressionVisitor
  {
    private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

    public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
    {
      this._map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
    }

    public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
    {
      return new ParameterRebinder(map).Visit(exp);
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
      ParameterExpression parameterExpression;
      if (this._map.TryGetValue(node, out parameterExpression))
        node = parameterExpression;
      return base.VisitParameter(node);
    }
  }
}
