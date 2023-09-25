using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using backend.Models.DTOs;
using LearnOAuth.Models;
using LearnOAuth.Models.DTOs;
using Newtonsoft.Json.Linq;

namespace LearnOAuth
{
  public class AutoMapperProfile : Profile
  {
    public AutoMapperProfile()
    {
      CreateMap<HttpResponse, APIResponseDTO<object>>()
      .ForMember(dest => dest.ErrorCode, opt => opt.MapFrom(src => src.StatusCode));
      CreateMap<User, UserDTO>();
      CreateMap<UserProfile, User>()
        .ForMember(dest => dest.username, opts => opts.MapFrom(src => src.email))
        .ForMember(dest => dest.googleId, opts => opts.MapFrom(src => src.sub));
    }
  }
}