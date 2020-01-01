using Amazon.DynamoDBv2.DataModel;
using BeatGrid.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeatGrid.Data.Repositories
{
    public interface IBeatRepository
    {
        Task<IEnumerable<BeatEntity>> GetBeats();
        Task<BeatEntity> GetBeat(string id);
        Task SaveBeat(BeatEntity beat);
        Task DeleteBeat(string id);
    }

    public class BeatRepository : IBeatRepository
    {
        private readonly IDynamoDBContext _context;

        public BeatRepository(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BeatEntity>> GetBeats()
        {
            var conditions = new List<ScanCondition>();
            var beats = await _context.ScanAsync<BeatEntity>(conditions).GetRemainingAsync();

            return beats;
        }

        public async Task<BeatEntity> GetBeat(string id)
        {
            //var beat = await _context.LoadAsync<BeatEntity>(id);
            var beat = (await _context.QueryAsync<BeatEntity>(id).GetRemainingAsync())?.FirstOrDefault();

            return beat;
        }

        public async Task SaveBeat(BeatEntity beat)
        {
            await _context.SaveAsync(beat);
        }

        public async Task DeleteBeat(string id)
        {
            await _context.DeleteAsync(new BeatEntity { Id = id });
        }
    }
}
