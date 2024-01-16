using AngularAuthenApi.Models;
using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            CreateMap<UserDto, User>();
            CreateMap<ComplaintDto, Complaint>();
            CreateMap<DemandDto, Demand>();
            CreateMap<AttachmentDto, Attachment>();
            CreateMap<loginDto, UserAuthentication>();
            CreateMap<RegisterDto, UserAuthentication>();


        }
    }
}
