using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class UserToGetUserResponseProfile : Profile
{
    public UserToGetUserResponseProfile()
    {
        CreateMap<User, GetUserResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.Name.Firstname} {src.Name.Lastname}"))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
    }
}
