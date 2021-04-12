using System;
using System.Linq.Expressions;

namespace LinqExpr {
	public partial class Predicate {
		public static Expression<Func<TEnt, bool>> WhereOverlap<TEnt, TProp>(
			Expression<Func<TEnt, TProp?>> fromField,
			Expression<Func<TEnt, TProp?>> toField,
			TProp? from, TProp? to,
			bool excludeFrom = false,
			bool excludeTo = false)
			where TEnt : class
			where TProp : struct {

			return ExprCreate(
				from: from,
				to: to,
				toExprFunc: () => ToExpr(fromField, to, excludeFrom),
				fromExprFunc: () => FromExpr(toField, from, excludeTo));
		}

		public static Expression<Func<TEnt, bool>> WhereOverlap<TEnt, TProp>(
			Expression<Func<TEnt, TProp>> fromField,
			Expression<Func<TEnt, TProp>> toField,
			TProp? from, TProp? to,
			bool excludeFrom = false,
			bool excludeTo = false)
			where TEnt : class
			where TProp : struct {

			return ExprCreate(
				from: from,
				to: to,
				toExprFunc: () => ToExpr(fromField, to, excludeFrom),
				fromExprFunc: () => FromExpr(toField, from, excludeTo));
		}

		public static Expression<Func<TEnt, bool>> WhereOverlap<TEnt, TProp>(
			Expression<Func<TEnt, TProp>> fromField,
			Expression<Func<TEnt, TProp?>> toField,
			TProp? from, TProp? to,
			bool excludeFrom = false,
			bool excludeTo = false)
			where TEnt : class
			where TProp : struct {

			return ExprCreate(
				from: from,
				to: to,
				toExprFunc: () => ToExpr(fromField, to, excludeFrom),
				fromExprFunc: () => FromExpr(toField, from, excludeTo));
		}

		public static Expression<Func<TEnt, bool>> WhereOverlap<TEnt, TProp>(
			Expression<Func<TEnt, TProp?>> fromField,
			Expression<Func<TEnt, TProp>> toField,
			TProp? from, TProp? to,
			bool excludeFrom = false,
			bool excludeTo = false)
			where TEnt : class
			where TProp : struct {

			return ExprCreate(
				from: from, 
				to: to, 
				toExprFunc: () => ToExpr(fromField, to, excludeFrom),
				fromExprFunc: () => FromExpr(toField, from, excludeTo));
		}

		static Expression<Func<TEnt, bool>> ExprCreate<TEnt, TProp>(
			TProp? from, TProp? to,
			Func<Expression<Func<TEnt, bool>>> toExprFunc, 
			Func<Expression<Func<TEnt, bool>>> fromExprFunc)
			where TEnt : class
			where TProp : struct {

			if (!to.HasValue && !from.HasValue)
				return PredicateBuilder.True<TEnt>();

			var toExpr = toExprFunc();
			var fromExpr = fromExprFunc();

			return (toExpr != null && fromExpr != null)
				? PredicateBuilder.And(toExpr, fromExpr)
				: toExpr ?? fromExpr;
		}

		static Expression<Func<TEnt, bool>> ToExpr<TEnt, TProp>(Expression<Func<TEnt, TProp?>> fromField, TProp? to, bool excludeFrom = false)
			where TEnt : class
			where TProp : struct {

			if (!to.HasValue)
				return null;

			return PredicateBuilder.Or(
				excludeFrom ? fromField.Less(to.Value) : fromField.LessOrEqual(to.Value),
				fromField.HasNoVal());
		}

		static Expression<Func<TEnt, bool>> ToExpr<TEnt, TProp>(Expression<Func<TEnt, TProp>> fromField, TProp? to, bool excludeFrom = false)
			where TEnt : class
			where TProp : struct {

			if (!to.HasValue)
				return null;

			return excludeFrom ? fromField.Less(to.Value) : fromField.LessOrEqual(to.Value);
		}

		static Expression<Func<TEnt, bool>> FromExpr<TEnt, TProp>(Expression<Func<TEnt, TProp>> toField, TProp? from, bool excludeTo = false)
			where TEnt : class
			where TProp : struct {

			if (!from.HasValue)
				return null;
			
			return excludeTo ? toField.Greater(from.Value) : toField.GreaterOrEqual(from.Value);
		}

		static Expression<Func<TEnt, bool>> FromExpr<TEnt, TProp>(Expression<Func<TEnt, TProp?>> toField, TProp? from, bool excludeTo = false)
			where TEnt : class
			where TProp : struct {

			if (!from.HasValue)
				return null;

			return PredicateBuilder.Or(
				excludeTo ? toField.Greater(from.Value) : toField.GreaterOrEqual(from.Value),
				toField.HasNoVal());
		}
	}
}
