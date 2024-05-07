using AutoMapper;
using RapidPayAPI.Repositories.Users;
using RapidPayAPI.Services.Users.Models;

namespace RapidPayAPI.Services.Users.Mappings
{
    public class UsersMappingProfile : Profile
    {
        public UsersMappingProfile()
        {
            CreateMap<User, UserResult>()
                .ForMember(userResult => userResult.UserRoleName, options =>
                    options.MapFrom(user => user.UserRole.Name));
        }
    }
}
