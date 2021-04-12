using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqExpr {
	public static class WhereOverlapExtensions {
		public static IQueryable<TEnt> WhereOverlap<TEnt, TProp>(
			this IQueryable<TEnt> source,
			Expression<Func<TEnt, TProp?>> fromField,
			Expression<Func<TEnt, TProp?>> toField,
			TProp? from, TProp? to,
			bool excludeFrom = false,
			bool excludeTo = false)
			where TEnt : class
			where TProp : struct {

			if (!from.HasValue && !to.HasValue)
				return source;

			return source.Where(Predicate.WhereOverlap(
				fromField: fromField,
				toField: toField,
				from: from,
				to: to,
				excludeFrom: excludeFrom,
				excludeTo: excludeTo
			));
		}

		public static IQueryable<TEnt> WhereOverlap<TEnt, TProp>(
			this IQueryable<TEnt> source,
			Expression<Func<TEnt, TProp>> fromField,
			Expression<Func<TEnt, TProp>> toField,
			TProp? from, TProp? to,
			bool excludeFrom = false,
			bool excludeTo = false)
			where TEnt : class
			where TProp : struct {

			if (!from.HasValue && !to.HasValue)
				return source;

			return source.Where(Predicate.WhereOverlap(
				fromField: fromField,
				toField: toField,
				from: from,
				to: to,
				excludeFrom: excludeFrom,
				excludeTo: excludeTo
			));
		}

		public static IQueryable<TEnt> WhereOverlap<TEnt, TProp>(
			this IQueryable<TEnt> source,
			Expression<Func<TEnt, TProp>> fromField,
			Expression<Func<TEnt, TProp?>> toField,
			TProp? from, TProp? to,
			bool excludeFrom = false,
			bool excludeTo = false)
			where TEnt : class
			where TProp : struct {

			if (!from.HasValue && !to.HasValue)
				return source;

			return source.Where(Predicate.WhereOverlap(
				fromField: fromField,
				toField: toField,
				from: from,
				to: to,
				excludeFrom: excludeFrom,
				excludeTo: excludeTo
			));
		}

		public static IQueryable<TEnt> WhereOverlap<TEnt, TProp>(
			this IQueryable<TEnt> source,
			Expression<Func<TEnt, TProp?>> fromField,
			Expression<Func<TEnt, TProp>> toField,
			TProp? from, TProp? to,
			bool excludeFrom = false,
			bool excludeTo = false)
			where TEnt : class
			where TProp : struct {

			if (!from.HasValue && !to.HasValue)
				return source;

			return source.Where(Predicate.WhereOverlap(
				fromField: fromField,
				toField: toField,
				from: from,
				to: to,
				excludeFrom: excludeFrom,
				excludeTo: excludeTo
			));
		}
	}
}
