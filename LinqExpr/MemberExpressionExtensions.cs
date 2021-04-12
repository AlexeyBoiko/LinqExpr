using System;
using System.Linq.Expressions;

namespace LinqExpr {
	public static class MemberExpressionExtensions {
        public static Expression<Func<TEnt, bool>> HasVal<TEnt, TProp>(this Expression<Func<TEnt, TProp?>> field) where TProp : struct
            => Expression.Lambda<Func<TEnt, bool>>(Expression.NotEqual(field.Body, Expression.Constant(null, typeof(TProp?))), field.Parameters);

        public static Expression<Func<TEnt, bool>> HasNoVal<TEnt, TProp>(this Expression<Func<TEnt, TProp?>> field) where TProp : struct
            => Expression.Lambda<Func<TEnt, bool>>(Expression.Equal(field.Body, Expression.Constant(null, typeof(TProp?))), field.Parameters);

        public static Expression<Func<TEnt, bool>> LessOrEqual<TEnt, TProp>(this Expression<Func<TEnt, TProp>> field, TProp val)
            => Expression.Lambda<Func<TEnt, bool>>(Expression.LessThanOrEqual(field.Body, Expression.Constant(val, typeof(TProp))), field.Parameters);

        public static Expression<Func<TEnt, bool>> Less<TEnt, TProp>(this Expression<Func<TEnt, TProp>> field, TProp val)
            => Expression.Lambda<Func<TEnt, bool>>(Expression.LessThan(field.Body, Expression.Constant(val, typeof(TProp))), field.Parameters);

        public static Expression<Func<TEnt, bool>> GreaterOrEqual<TEnt, TProp>(this Expression<Func<TEnt, TProp>> field, TProp val)
            => Expression.Lambda<Func<TEnt, bool>>(Expression.GreaterThanOrEqual(field.Body, Expression.Constant(val, typeof(TProp))), field.Parameters);

        public static Expression<Func<TEnt, bool>> Greater<TEnt, TProp>(this Expression<Func<TEnt, TProp>> field, TProp val)
            => Expression.Lambda<Func<TEnt, bool>>(Expression.GreaterThan(field.Body, Expression.Constant(val, typeof(TProp))), field.Parameters);
    }
}
