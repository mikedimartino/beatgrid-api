using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace BeatGrid.Data.Entities
{

    [DynamoDBTable("Beat")]
    public class BeatEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Tempo { get; set; }
        public TimeSignature TimeSignature { get; set; }
        public int DivisionLevel { get; set; }
        public List<Row> Rows { get; set; }
        public List<Measure> Measures { get; set; }
    }

    public class TimeSignature
    {
        public int NotesPerMeasure { get; set; }
        public int NoteType { get; set; } // 2 = half, 4 = quarter, 16 = 16th, etc..
    }

    public class Row
    {
        public int Index { get; set; }
        public string SoundId { get; set; }
    }

    public class Measure
    {
        public List<Square> ActiveSquares { get; set; }
    }

    public class Square
    {
        public int Row { get; set; }
        public float Column { get; set; }
    }
}
