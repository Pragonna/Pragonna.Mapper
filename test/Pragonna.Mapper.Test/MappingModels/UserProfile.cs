

using Pragonna.Mapper.Concurrency;

public class UserProfile : MapProfile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.UserName, source => source.Name)
            .ReverseMap();
    }
}