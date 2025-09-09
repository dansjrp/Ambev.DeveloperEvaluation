using System;
using System.Linq;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.ORM.Extension;

public static class QueryableExtensions
{
    public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.PropertyOrField(parameter, propertyName);
        var lambda = Expression.Lambda(property, parameter);
        var methodName = "OrderBy";
        var result = Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(T), property.Type }, source.Expression, Expression.Quote(lambda));
        return source.Provider.CreateQuery<T>(result);
    }

    public static IQueryable<T> OrderByDynamicDescending<T>(this IQueryable<T> source, string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.PropertyOrField(parameter, propertyName);
        var lambda = Expression.Lambda(property, parameter);
        var methodName = "OrderByDescending";
        var result = Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(T), property.Type }, source.Expression, Expression.Quote(lambda));
        return source.Provider.CreateQuery<T>(result);
    }
}
