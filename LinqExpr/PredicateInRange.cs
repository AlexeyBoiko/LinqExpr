using System;
using System.Linq.Expressions;

namespace LinqExpr {
	public partial class Predicate {
		public static Expression<Func<TEnt, bool>> WhereInRange<TEnt, TProp>(
			Expression<Func<TEnt, TProp>> field,
			TProp? from, TProp? to,
			bool excludeFrom = false,
			bool excludeTo = false)
			where TEnt : class
			where TProp : struct {

			return ExprCreate(
				from: from,
				to: to,
				toExprFunc: () => ToExpr(field, to, excludeFrom),
				fromExprFunc: () => FromExpr(field, from, excludeTo));
		}

	}
}
