using Amazon.DynamoDBv2.DataModel;

namespace BeatGrid.Data.Entities
{
    [DynamoDBTable("BeatGrid.Kit")]
    public class KitEntity
    {
        [DynamoDBHashKey]
        public string Name { get; set; }

        public string Sound { get; set; }
    }
}
