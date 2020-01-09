using BeatGrid.Data.Entities;
using BeatGrid.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeatGrid.Application.Services
{
    public interface ISoundService
    {
        Task<IEnumerable<SoundEntity>> GetSounds();
        Task<string> CreateSound(SoundEntity sound);
        Task UpdateSound(SoundEntity sound);
        Task DeleteSound(string id);
    }

    public class SoundService : ISoundService
    {
        private readonly ISoundRepository _repository;

        public SoundService(ISoundRepository repository) => _repository = repository;

        public async Task<IEnumerable<SoundEntity>> GetSounds() => await _repository.GetAllSounds();

        public async Task<string> CreateSound(SoundEntity sound)
        {
            sound.Id = Guid.NewGuid().ToString();

            await _repository.SaveSound(sound);

            return sound.Id;
        }

        public async Task UpdateSound(SoundEntity sound) => await _repository.SaveSound(sound);

        public async Task DeleteSound(string id) => await _repository.DeleteSound(id);
    }
}
