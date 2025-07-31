using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Pragonna.Mapper.Abstractions;
using Pragonna.Mapper.Concurrency;

namespace Pragonna.Mapper;

public static class ServiceExtensions
{
    public static IServiceCollection AddCustomMapper(this IServiceCollection services, Assembly assembly)
    {
        var profiles = assembly.GetTypes()
            .Where(x => typeof(MapProfile).IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface);

        List<MapProfile> _profiles = [];
        foreach (var profile in profiles)
        {
            _profiles.Add((MapProfile)Activator.CreateInstance(profile));
        }

        services.AddSingleton<IMapper>(provider => { return new Concurrency.Mapper(_profiles); });

        return services;
    }
}