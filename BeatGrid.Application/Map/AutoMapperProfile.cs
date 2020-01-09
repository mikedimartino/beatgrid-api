using BeatGrid.Contracts.Common;
using BeatGrid.Data.Entities;
using AutoMapper;

namespace BeatGrid.Application.Map
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BeatEntity, Beat>();
            CreateMap<Beat, BeatEntity>();
        }
    }
}
