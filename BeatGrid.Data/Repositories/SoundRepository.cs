using Amazon.DynamoDBv2.DataModel;
using BeatGrid.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeatGrid.Data.Repositories
{
    public interface ISoundRepository
    {
        public Task<List<SoundEntity>> GetSounds(IEnumerable<string> ids);
    }

    public class SoundRepository : ISoundRepository
    {
        private readonly IDynamoDBContext _context;

        public SoundRepository(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task<List<SoundEntity>> GetSounds(IEnumerable<string> ids)
        {
            var soundsBatch = _context.CreateBatchGet<SoundEntity>();
            foreach (var id in ids)
            {
                soundsBatch.AddKey(id);
            }

            await soundsBatch.ExecuteAsync();

            return soundsBatch.Results;
        }
    }
}
