namespace Rsvp.Application.Configurations.Mapper;

using AutoMapper;

using Rsvp.Application.Features.Events.Dtos;
using Rsvp.Domain.Contexts.Events;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    this.AllowNullCollections = true;

    this.CreateMap<Event, EventDto>().ReverseMap();
  }
}
