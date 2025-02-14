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

    this.CreateMap<Event, EventItemDto>()
      .IncludeBase<Event, EventDto>()
      .ForMember(dest => dest.Attendees, opt =>
        opt.MapFrom(src => src.Attendees.Select(a => new EventAttendeeDto
        {
          UserId = a.User.Id,
          AttendeeName = a.User.FullName,
          Status = a.Status,
          RsvpDate = a.CreatedAt,
        }).ToList()))
      .ForMember(dest => dest.Orgnizer, opt =>
        opt.MapFrom(src => new OrganizerDto
        {
          UserId = src.Organizer.Id,
          FullName = src.Organizer.FullName,
          Email = src.Organizer.Email,
        }));
  }
}
