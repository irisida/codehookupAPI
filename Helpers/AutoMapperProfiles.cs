using System.Linq;
using AutoMapper;
using hookup.API.DTOs;
using hookup.API.Models;

namespace hookup.API.Helpers
{
  /**
   * Automapper as name suggest maps between the entities
   * based on a configuration supplying the source entity
   * and the view that should be return in a data transfer
   * object. We also see the transations between entity
   * fields and attibutes to be returned in our case the
   * phot urls and the calculation of ages based on a
   * date of birth.
   *
   * standard direct usage is CreateMap(entity, dto)
   * mutations use the .ForMember(destination, sourceHandler)
   */

  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
      CreateMap<User, UserForListDTO>()
        .ForMember(dest => dest.PhotoUrl, opt =>
        {
          opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsAvatar).Url);
        })
        .ForMember(dest => dest.Age, opt =>
        {
          opt.MapFrom(d => d.DateOfBirth.CalculateAge());
        });
      CreateMap<User, UserForDetailedDTO>()
      .ForMember(dest => dest.PhotoUrl, opt =>
        {
          opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsAvatar).Url);
        })
      .ForMember(dest => dest.Age, opt =>
        {
          opt.MapFrom(d => d.DateOfBirth.CalculateAge());
        });
      CreateMap<Photo, PhotosForDetailedDTO>();
    }
  }
}