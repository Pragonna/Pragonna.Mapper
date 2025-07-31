# Pragonna.Mapper

A lightweight, high-performance object-to-object mapping library for .NET 9.0 with support for custom mapping profiles and property mappings.

## Features

- **Simple API**: Easy-to-use fluent interface for defining mappings
- **Custom Property Mapping**: Map properties with different names between source and destination objects
- **Reverse Mapping**: Automatically create bidirectional mappings
- **Dependency Injection Support**: Built-in integration with Microsoft.Extensions.DependencyInjection
- **Profile-Based Configuration**: Organize your mappings using profiles
- **High Performance**: Optimized for speed with minimal overhead

## Installation

```bash
dotnet add package Pragonna.Mapper
```

## Quick Start

### 1. Define Your Models

```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string EmailAddress { get; set; }
}
```

### 2. Create a Mapping Profile

```csharp
using Pragonna.Mapper.Concurrency;

public class UserProfile : MapProfile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.EmailAddress, src => src.Email)
            .ReverseMap();
    }
}
```

### 3. Register the Mapper

```csharp
using Pragonna.Mapper;

var builder = WebApplication.CreateBuilder(args);

// Register the mapper with dependency injection
builder.Services.AddCustomMapper(typeof(UserProfile).Assembly);

var app = builder.Build();
```

### 4. Use the Mapper

```csharp
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;

    public UserController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public ActionResult<UserDto> GetUser(int id)
    {
        var user = GetUserFromDatabase(id);
        var userDto = _mapper.Map<UserDto>(user);
        return Ok(userDto);
    }
}
```

## Advanced Usage

### Custom Property Mappings

Map properties with different names or types:

```csharp
public class OrderProfile : MapProfile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.CustomerName, src => src.Customer.Name)
            .ForMember(dest => dest.TotalAmount, src => src.Total);
    }
}
```

### Reverse Mapping

Create bidirectional mappings easily:

```csharp
public class ProductProfile : MapProfile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>()
            .ReverseMap(); // This creates both Product -> ProductDto and ProductDto -> Product mappings
    }
}
```

### Multiple Profiles

Organize your mappings across multiple profiles:

```csharp
public class UserProfile : MapProfile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserSummaryDto>();
    }
}

public class OrderProfile : MapProfile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<OrderItem, OrderItemDto>();
    }
}
```

## API Reference

### MapProfile

Base class for creating mapping configurations.

```csharp
public class MapProfile
{
    public MapBuilder<TSource, TDestination> CreateMap<TSource, TDestination>()
}
```

### MapBuilder<TSource, TDestination>

Fluent interface for configuring mappings.

```csharp
public class MapBuilder<TSource, TDestination>
{
    // Create reverse mapping
    public MapBuilder<TSource, TDestination> ReverseMap()
    
    // Map specific properties
    public MapBuilder<TSource, TDestination> ForMember(
        Expression<Func<TDestination, object>> destinationMember,
        Expression<Func<TSource, object>> sourceMember)
}
```

### IMapper

Main interface for performing mappings.

```csharp
public interface IMapper
{
    TDestination Map<TDestination>(object source);
}
```

### ServiceExtensions

Extension methods for dependency injection.

```csharp
public static class ServiceExtensions
{
    public static IServiceCollection AddCustomMapper(
        this IServiceCollection services, 
        Assembly assembly)
}
```

## Requirements

- .NET 9.0 or higher
- Microsoft.Extensions.DependencyInjection.Abstractions 10.0.0+

## Performance

Pragonna.Mapper is designed for high performance with:
- Minimal reflection overhead
- Efficient property mapping
- Optimized object creation
- Thread-safe operations

## Contributing

Contributions are welcome! Please feel free to submit issues, feature requests, or pull requests.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Roadmap

- [ ] Support for collection mappings
- [ ] Conditional mapping support
- [ ] Custom type converters
- [ ] Async mapping support
- [ ] Performance benchmarks
- [ ] More comprehensive documentation

## Support

If you encounter any issues or have questions, please create an issue on GitHub.

---

**Note**: This library is currently in development. Please test thoroughly before using in production environments.