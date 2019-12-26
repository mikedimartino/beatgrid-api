namespace BeatGrid.Contracts.Common
{
    public class TimeSignature
    {
        public int NotesPerMeasure { get; set; }
        public int NoteType { get; set; } // 2 = half, 4 = quarter, 16 = 16th, etc..
    }
}
