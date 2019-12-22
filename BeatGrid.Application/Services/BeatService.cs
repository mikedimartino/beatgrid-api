using System.Collections.Generic;
using System.Threading.Tasks;
using BeatGrid.Data.Repositories;

namespace BeatGrid.Application.Services
{
    public interface IBeatService
    {
        Task<IEnumerable<object>> GetBeats();
    }

    public class BeatService : IBeatService
    {
        private readonly IBeatRepository _repository;

        public BeatService(IBeatRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<object>> GetBeats()
        {
            return await _repository.GetBeats();
        }
    }
}
