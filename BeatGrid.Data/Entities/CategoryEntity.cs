using Amazon.DynamoDBv2.DataModel;

namespace BeatGrid.Data.Entities
{
    [DynamoDBTable("BeatGrid.SoundCategory")]
    public class CategoryEntity
    {
        [DynamoDBHashKey]
        public string Name { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey]
        public string Parent { get; set; }
    }
}
