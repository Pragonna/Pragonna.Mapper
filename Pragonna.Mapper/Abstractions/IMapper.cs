namespace Pragonna.Mapper.Abstractions;

public interface IMapper
{
    TDestination Map<TDestination>(object source);
}

