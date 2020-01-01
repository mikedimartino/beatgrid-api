using Amazon.DynamoDBv2.DataModel;
using BeatGrid.Contracts.Common;
using System;
using System.Collections.Generic;

namespace BeatGrid.Data.Entities
{

    [DynamoDBTable("BeatGrid.Beat")]
    public class BeatEntity
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        [DynamoDBRangeKey]
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string Name { get; set; }
        public int Tempo { get; set; }
        public TimeSignature TimeSignature { get; set; }
        public int DivisionLevel { get; set; }
        public List<Row> Rows { get; set; }
        public List<Measure> Measures { get; set; }
    }
}
