using Amazon.DynamoDBv2.DataModel;

namespace BeatGrid.Data.Entities
{
    [DynamoDBTable("Beats")]
    public class BeatEntity
    {
        [DynamoDBHashKey(AttributeName = "id")]
        public string Id { get; set; }

        [DynamoDBProperty(AttributeName = "json")]
        public string Json { get; set; }

        [DynamoDBProperty(AttributeName = "name")]
        public string Name { get; set; }
    }
}
