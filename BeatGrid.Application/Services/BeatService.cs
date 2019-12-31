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
        Task<string> CreateBeat(Beat beat);
        Task UpdateBeat(Beat beat);
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

        public async Task<string> CreateBeat(Beat beat)
        {
            var id = Guid.NewGuid().ToString();
            var entity = _mapper.Map<BeatEntity>(beat);
            entity.Id = id;

            await _repository.SaveBeat(entity);

            return id;
        }

        public async Task UpdateBeat(Beat beat)
        {
            var entity = _mapper.Map<BeatEntity>(beat);
            await _repository.SaveBeat(entity);
        }

        public async Task DeleteBeat(string id)
        {
            await _repository.DeleteBeat(id);
        }
    }
}
