using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqExpr {
	public static class WhereInRangeExtensions {
		public static IQueryable<TEnt> WhereInRange<TEnt, TProp>(
			this IQueryable<TEnt> source,
			Expression<Func<TEnt, TProp>> field,
			TProp? from, TProp? to,
			bool excludeFrom = false,
			bool excludeTo = false)
			where TEnt : class
			where TProp : struct {

			if (!from.HasValue && !to.HasValue)
				return source;

			return source.Where(Predicate.WhereInRange(
				field: field,
				from: from,
				to: to,
				excludeFrom: excludeFrom,
				excludeTo: excludeTo
			));
		}
	}
}
