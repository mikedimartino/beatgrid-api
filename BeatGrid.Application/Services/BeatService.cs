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
        Task<IEnumerable<Beat>> GetBeats();
        Task<string> CreateBeat(Beat beat, string userId);
        Task UpdateBeat(Beat beat, string userId);
        Task DeleteBeat(string id);
    }

    public class BeatService : IBeatService
    {
        private readonly IBeatRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<Beat> _validator;

        public BeatService(
            IBeatRepository repository,
            IMapper mapper,
            IValidator<Beat> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IEnumerable<Beat>> GetBeats()
        {
            var beatEntities = await _repository.GetBeats();
            return _mapper.Map<IEnumerable<Beat>>(beatEntities);
        }

        public async Task<string> CreateBeat(Beat beat, string userId)
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

            return beat.Id;
        }

        public async Task UpdateBeat(Beat beat, string userId)
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
        }

        public async Task DeleteBeat(string id) => await _repository.DeleteBeat(id);
    }
}
