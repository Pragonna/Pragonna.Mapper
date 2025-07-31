using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Pragonna.Mapper.Abstractions;

namespace Pragonna.Mapper.Test;

[TestFixture]
public class PragonnaMapperTest
{
    private readonly User user;
    private readonly IServiceCollection services;

    public PragonnaMapperTest()
    {
        user = new User
        {
            Id = 1,
            Guid = Guid.NewGuid(),
            Name = "Pragonna",
        };

        services = new ServiceCollection();
    }

    [Test]
    public void Map_ShouldBeCorrectly()
    {
        services.AddCustomMapper(Assembly.GetExecutingAssembly());
        var provider = services.BuildServiceProvider();
        var mapper = provider.GetRequiredService<IMapper>();

        var userDto = mapper.Map<UserDto>(user);

        userDto.Should().NotBeNull();
        userDto.UserName.Should().Be(user.Name);
        userDto.Id.Should().Be(user.Id);
        userDto.Guid.Should().Be(user.Guid);
    }
}