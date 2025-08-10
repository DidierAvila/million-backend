using AutoMapper;
using Million.Domain.DTOs;
using Million.Domain.Entities;

namespace Million.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Property mappings
            CreateMap<Property, PropertyDto>();
            CreateMap<CreatePropertyDto, Property>();
            CreateMap<UpdatePropertyDto, Property>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Owner mappings
            CreateMap<Owner, OwnerDto>();
            CreateMap<CreateOwnerDto, Owner>();
            CreateMap<UpdateOwnerDto, Owner>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // PropertyImage mappings
            CreateMap<PropertyImage, PropertyImageDto>();
            CreateMap<CreatePropertyImageDto, PropertyImage>();
            CreateMap<UpdatePropertyImageDto, PropertyImage>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // PropertyTrace mappings
            CreateMap<PropertyTrace, PropertyTraceDto>();
            CreateMap<CreatePropertyTraceDto, PropertyTrace>();
            CreateMap<UpdatePropertyTraceDto, PropertyTrace>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
