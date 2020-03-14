using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeatGrid.Contracts.Common;
using BeatGrid.Data.Entities;
using BeatGrid.Data.Repositories;
using AutoMapper;
using FluentValidation;

namespace BeatGrid.Application.Services
{
    public interface IBeatService
    {
        Task<IEnumerable<Beat>> GetBeats(bool isAuthenticated);
        Task<string> CreateBeat(Beat beat, string userId, bool shouldSync = false);
        Task UpdateBeat(Beat beat, string userId, bool shouldSync = false);
        Task DeleteBeat(string id, bool shouldSync = false);
        Task Sync();
    }

    public class BeatService : IBeatService
    {
        private readonly IBeatRepository _repository;
        private readonly IPublicBeatRepository _publicRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<Beat> _validator;

        public BeatService(
            IBeatRepository repository,
            IPublicBeatRepository publicRepository,
            IMapper mapper,
            IValidator<Beat> validator)
        {
            _repository = repository;
            _publicRepository = publicRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IEnumerable<Beat>> GetBeats(bool isAuthenticated)
        {
            var beatEntities = await (isAuthenticated ? _repository.GetBeats() : _publicRepository.GetBeats());
            return _mapper.Map<IEnumerable<Beat>>(beatEntities);
        }

        public async Task<string> CreateBeat(Beat beat, string userId, bool shouldSync = false)
        {
            beat.Id = Guid.NewGuid().ToString();

            var validationResult = _validator.Validate(beat);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<BeatEntity>(beat);
            entity.CreatedBy = userId;
            entity.CreateDate = DateTime.UtcNow;
            entity.ModifyDate = DateTime.UtcNow;

            await _repository.SaveBeat(entity);
            if (shouldSync) await Sync();

            return beat.Id;
        }

        public async Task UpdateBeat(Beat beat, string userId, bool shouldSync = false)
        {
            var validationResult = _validator.Validate(beat);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var savedBeat = await _repository.GetBeat(beat.Id);
            if (savedBeat == null)
            {
                throw new KeyNotFoundException();
            }

            var entity = _mapper.Map<BeatEntity>(beat);
            entity.ModifyDate = DateTime.UtcNow;
            entity.CreatedBy = savedBeat.CreatedBy;
            entity.CreateDate = savedBeat.CreateDate;

            await _repository.SaveBeat(entity);
            if (shouldSync) await Sync();
        }

        public async Task DeleteBeat(string id, bool shouldSync = false)
        {
            await _repository.DeleteBeat(id);
            if (shouldSync) await Sync();
        }

        /// <summary>
        /// Sync public beats (static JSON on S3) with live beats (DynamoDB)
        /// </summary>
        /// <returns></returns>
        public async Task Sync()
        {
            var liveBeats = await _repository.GetBeats();
            _publicRepository.SaveBeatsAsync(liveBeats);
        }
    }
}
