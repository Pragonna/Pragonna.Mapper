using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Serialization;

namespace Pragonna.Mapper.Concurrency;

public class MapBuilder<TSource, TDestination>
{
    internal Dictionary<object, object> mappedObjects = [];
    internal Dictionary<object, (PropertyInfo sourceMember, PropertyInfo destMember)> mappedProperties = [];

    public MapBuilder<TSource, TDestination> ReverseMap()
    {
        mappedObjects.Add(typeof(TDestination), typeof(TSource));
        return this;
    }

    public MapBuilder<TSource, TDestination> ForMember(Expression<Func<TDestination, object>> destinationMember,
        Expression<Func<TSource, object>> sourceMember)
    {
        var destPropertyInfo = GetPropertyInfo(destinationMember);
        var sourcePropertyInfo = GetPropertyInfo(sourceMember);

        mappedProperties.Add(typeof(TSource), (sourcePropertyInfo, destPropertyInfo));

        return this;
    }
    public MapBuilder<TSource, TDestination> ForMember<TSourceType,TDestinationType>(Expression<Func<TDestinationType, object>> destinationMember,
        Expression<Func<TSourceType, object>> sourceMember)
    {
        var destPropertyInfo = GetPropertyInfo(destinationMember);
        var sourcePropertyInfo = GetPropertyInfo(sourceMember);

        mappedProperties.Add(typeof(TSource), (sourcePropertyInfo, destPropertyInfo));

        return this;
    }

    private PropertyInfo GetPropertyInfo<T>(Expression<Func<T, object>> expression)
    {
        if (expression.Body is UnaryExpression unaryExpr && unaryExpr.Operand is MemberExpression memberExpr)
            return memberExpr.Member as PropertyInfo;

        if (expression.Body is MemberExpression member)
            return member.Member as PropertyInfo;

        throw new InvalidOperationException("Expression does not refer to a property.");
    }
}