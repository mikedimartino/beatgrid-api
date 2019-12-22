using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using BeatGrid.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeatGrid.Data.Repositories
{
    public interface IBeatRepository
    {
        public Task<IEnumerable<object>> GetBeats();
    }

    public class BeatRepository : IBeatRepository
    {
        private readonly DynamoDBContext _context;

        public BeatRepository(IAmazonDynamoDB dynamoDb)
        {
            _context = new DynamoDBContext(dynamoDb);
        }

        public async Task<IEnumerable<object>> GetBeats()
        {
            var conditions = new List<ScanCondition>();
            var beats = await _context.ScanAsync<BeatEntity>(conditions).GetRemainingAsync();

            return beats;
        }
    }
}
