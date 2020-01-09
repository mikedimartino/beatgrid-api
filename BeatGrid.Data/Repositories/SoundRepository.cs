using System.Collections.Generic;
using System.Threading.Tasks;
using BeatGrid.Data.Entities;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace BeatGrid.Data.Repositories
{
    public interface ISoundRepository
    {
        Task<List<SoundEntity>> GetAllSounds();
        Task<List<SoundEntity>> GetSoundsByNames(IEnumerable<string> names);
        Task<List<SoundEntity>> GetSoundsByCategory(string category);
        Task<List<SoundEntity>> GetSoundsByCategories(IEnumerable<string> categories);
        Task SaveSound(SoundEntity sound);
        Task DeleteSound(string id);
    }

    public class SoundRepository : ISoundRepository
    {
        private readonly IDynamoDBContext _context;

        public SoundRepository(IAmazonDynamoDB client, IDynamoDBContext context) => _context = context;

        public async Task<List<SoundEntity>> GetAllSounds()
        {
            var conditions = new List<ScanCondition>();
            return await _context.ScanAsync<SoundEntity>(conditions).GetRemainingAsync();
        }

        public async Task<List<SoundEntity>> GetSoundsByNames(IEnumerable<string> names)
        {
            var soundsBatch = _context.CreateBatchGet<SoundEntity>();
            foreach (var name in names)
            {
                soundsBatch.AddKey(name);
            }

            await soundsBatch.ExecuteAsync();

            return soundsBatch.Results;
        }

        public async Task<List<SoundEntity>> GetSoundsByCategories(IEnumerable<string> categories)
        {
            var config = new DynamoDBOperationConfig { IndexName = "Category-index" };
            var soundsBatch = _context.CreateBatchGet<SoundEntity>(config);

            foreach (var category in categories)
            {
                soundsBatch.AddKey(category);
            }

            await soundsBatch.ExecuteAsync();

            return soundsBatch.Results;
        }

        public async Task<List<SoundEntity>> GetSoundsByCategory(string category)
        {
            var config = new DynamoDBOperationConfig { IndexName = "Category-index" };
            return await _context.QueryAsync<SoundEntity>(category, config).GetRemainingAsync();
        }

        public async Task SaveSound(SoundEntity sound) => await _context.SaveAsync(sound);

        public async Task DeleteSound(string id) => await _context.DeleteAsync<SoundEntity>(id);
    }
}
