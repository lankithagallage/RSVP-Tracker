namespace Rsvp.Application.Configurations.Mapper;

using AutoMapper;

using Rsvp.Application.Features.Events.Dtos;
using Rsvp.Domain.Contexts.Events;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    this.AllowNullCollections = true;

    this.CreateMap<Event, EventDto>()
      .ForMember(dest => dest.IsExpired, opt =>
        opt.MapFrom(src => src.EndTime < DateTime.UtcNow))
      .ForMember(dest => dest.OrganizerName,
        opt => opt.MapFrom(src => src.Organizer.FullName))
      .ReverseMap();
  }
}
