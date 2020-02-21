using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeatGrid.Data.Entities;
using Amazon.DynamoDBv2.DataModel;

namespace BeatGrid.Data.Repositories
{
    public interface IBeatRepository
    {
        Task<BeatEntity> GetBeat(string id);
        Task<IEnumerable<BeatEntity>> GetBeats();
        Task SaveBeat(BeatEntity beat);
        Task DeleteBeat(string id);
    }

    public class BeatRepository : IBeatRepository
    {
        private readonly IDynamoDBContext _context;

        public BeatRepository(IDynamoDBContext context) => _context = context;

        public async Task<BeatEntity> GetBeat(string id) =>
            (await _context.QueryAsync<BeatEntity>(id).GetRemainingAsync())?.FirstOrDefault();

        public async Task<IEnumerable<BeatEntity>> GetBeats()
        {
            var conditions = new List<ScanCondition>();
            return await _context.ScanAsync<BeatEntity>(conditions).GetRemainingAsync();
        }

        public async Task SaveBeat(BeatEntity beat) => await _context.SaveAsync(beat);

        public async Task DeleteBeat(string id) => await _context.DeleteAsync<BeatEntity>(id);
    }
}
