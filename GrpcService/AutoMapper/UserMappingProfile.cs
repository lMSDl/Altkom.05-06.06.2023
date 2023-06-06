using AutoMapper;

namespace GrpcService.AutoMapper
{
    internal class UserMappingProfile : Profile
    {
        public UserMappingProfile() {
            CreateMap<GrpcService.Protos.Users.User, Models.User>();
            CreateMap<Models.User, GrpcService.Protos.Users.User>();
        }
    }
}