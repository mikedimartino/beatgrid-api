using AutoMapper;
using BeatGrid.Contracts.Common;
using BeatGrid.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeatGrid.Application.Services
{
    public interface ISoundService
    {
        Task<IEnumerable<Sound>> GetSounds(IEnumerable<string> ids);
    }

    public class SoundService : ISoundService
    {
        private readonly ISoundRepository _repository;
        private readonly IMapper _mapper;

        public SoundService(ISoundRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Sound>> GetSounds(IEnumerable<string> ids)
        {
            var entities = await _repository.GetSounds(ids);
            return _mapper.Map<IEnumerable<Sound>>(entities);
        }
    }
}
