using AutoMapper;
using HackerNews.Api.Models;
using HackerNews.Api.Models.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Item, ItemDTO>()
            .ForMember(dest => dest.By, opt => opt.MapFrom(src => src.ByUsername))
            .ForMember(dest => dest.Time, opt => opt.MapFrom(src => ((DateTimeOffset)src.CreatedAt).ToUnixTimeSeconds()))
            .ForMember(dest => dest.Descendants, opt => opt.MapFrom(src => src.Kids.Count));

        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.Submitted, opt => opt.MapFrom(src => src.Submissions))
            .ForMember(dest => dest.Created, opt => opt.MapFrom(src => ((DateTimeOffset)src.CreatedAt).ToUnixTimeSeconds()));
    }
}