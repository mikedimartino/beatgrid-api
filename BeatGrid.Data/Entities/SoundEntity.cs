using Amazon.DynamoDBv2.DataModel;

namespace BeatGrid.Data.Entities
{
    [DynamoDBTable("BeatGrid.Sound")]
    public class SoundEntity
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        [DynamoDBGlobalSecondaryIndexHashKey]
        public string Category { get; set; }

        public string Url { get; set; }
    }
}
