namespace Pragonna.Mapper.Concurrency;

public class MapProfile
{
    private readonly Dictionary<(Type source, Type destination), object> _builders = [];
    internal IReadOnlyDictionary<(Type source, Type destination), object> Builders => _builders;
    public MapBuilder<TSource, TDestination> CreateMap<TSource, TDestination>()
    {
        var mapBuilder = new MapBuilder<TSource, TDestination>();
        mapBuilder.mappedObjects.Add(typeof(TSource), typeof(TDestination));
        _builders[(typeof(TSource), typeof(TDestination))] = mapBuilder;

        return mapBuilder;
    }
}