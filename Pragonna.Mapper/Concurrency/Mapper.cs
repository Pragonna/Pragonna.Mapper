using System.Reflection;
using Pragonna.Mapper.Abstractions;

namespace Pragonna.Mapper.Concurrency;

public class Mapper : IMapper
{
    private readonly IEnumerable<MapProfile> _profiles;

    public Mapper(IEnumerable<MapProfile> profiles)
    {
        _profiles = profiles;
    }

    public TDestination Map<TDestination>(object source)
    {
        MapProfile _profile =
            _profiles.FirstOrDefault(x =>
                x.Builders.ContainsKey((source.GetType(), typeof(TDestination)))
                || x.Builders.ContainsKey((typeof(TDestination), source.GetType())));

        if (_profile is null)
            throw new ArgumentNullException("Source object can not be mapped");

        var builder = _profile.Builders;
        dynamic mapBuilder = builder.FirstOrDefault().Value;

        var destinationType = (Type)mapBuilder.mappedObjects[source.GetType()];
        var HasSrcDict = mapBuilder.mappedProperties.TryGetValue(source.GetType(),
            out (PropertyInfo dictionarySrcCfg, PropertyInfo dest) dictionarySrcCfg);
        var HasDestDict = mapBuilder.mappedProperties.TryGetValue(typeof(TDestination),
            out (PropertyInfo dictionarySrcCfg, PropertyInfo dest) dictionaryDestCfg);

        if (destinationType is null) throw new ArgumentNullException("Destination object can not be mapped");

        var sourceType = source.GetType();
        var destination = Activator.CreateInstance(destinationType);

        var sourceProperties = sourceType.GetProperties();
        foreach (var sourceProperty in sourceProperties)
        {
            if (HasDestDict)
            {
                if (EnsureAndSetMappedProperty<TDestination>(source, dictionaryDestCfg, sourceProperty, destinationType,
                        destination)) continue;
            }
            else if (HasSrcDict)
            {
                if (EnsureAndSetMappedProperty<TDestination>(source, dictionarySrcCfg, sourceProperty, destinationType,
                        destination)) continue;
            }

            var destinationProperty = destinationType.GetProperties().FirstOrDefault(p =>
                p.Name == sourceProperty.Name && p.PropertyType.Name == sourceProperty.PropertyType.Name);
            if (destinationProperty is null || !destinationProperty.CanWrite) continue;

            destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
        }

        return (TDestination)destination;
    }

    private static bool EnsureAndSetMappedProperty<TDestination>(object source,
        (PropertyInfo sourceMember, PropertyInfo destinationMember) result, PropertyInfo sourceProperty,
        Type destinationType, object? destination)
    {
        if (result.sourceMember.MemberType.Equals(result.destinationMember.MemberType)
            && result.sourceMember.PropertyType.Equals(sourceProperty.PropertyType))
        {
            var destProp = destinationType.GetProperty(result.destinationMember.Name);
            if (destProp is null) return false;
            destProp.SetValue(destination, sourceProperty.GetValue(source));
            return true;
        }

        return false;
    }
}