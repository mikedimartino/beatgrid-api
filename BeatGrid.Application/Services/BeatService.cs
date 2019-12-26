using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BeatGrid.Contracts.Common;
using BeatGrid.Data.Entities;
using BeatGrid.Data.Repositories;

namespace BeatGrid.Application.Services
{
    public interface IBeatService
    {
        Task<IEnumerable<Beat>> GetBeats();
        Task<Beat> GetBeat(string id);
        Task<string> CreateBeat(object beat);
        Task UpdateBeat(object beat);
        Task DeleteBeat(string id);
    }

    public class BeatService : IBeatService
    {
        private readonly IBeatRepository _repository;
        private readonly IMapper _mapper;

        public BeatService(IBeatRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Beat>> GetBeats()
        {
            var beatEntities = await _repository.GetBeats();
            return _mapper.Map<IEnumerable<Beat>>(beatEntities);
        }

        public async Task<Beat> GetBeat(string id)
        {
            var beatEntity = await _repository.GetBeat(id);
            return _mapper.Map<Beat>(beatEntity);
        }

        public async Task<string> CreateBeat(object beat)
        {
            var id = Guid.NewGuid().ToString();
            var entity = (BeatEntity)beat;
            entity.Id = id;

            await _repository.SaveBeat(entity);

            return id;
        }

        public async Task UpdateBeat(object beat)
        {
            var entity = (BeatEntity)beat;
            await _repository.SaveBeat(entity);
        }

        public async Task DeleteBeat(string id)
        {
            await _repository.DeleteBeat(id);
        }
    }
}
