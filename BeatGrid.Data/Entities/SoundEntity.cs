using Amazon.DynamoDBv2.DataModel;

namespace BeatGrid.Data.Entities
{
    [DynamoDBTable("Sound")]
    public class SoundEntity
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
    }
}
