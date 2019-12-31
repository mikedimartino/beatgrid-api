using AutoMapper;
using BeatGrid.Contracts.Common;
using BeatGrid.Data.Entities;

namespace BeatGrid.Application.Map
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BeatEntity, Beat>();
            CreateMap<SoundEntity, Sound>();
            CreateMap<Beat, BeatEntity>();
        }
    }
}
