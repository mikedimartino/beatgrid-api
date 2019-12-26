using System.Collections.Generic;
using System.Linq;

namespace BeatGrid.Contracts.Common
{
    public class Beat
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Tempo { get; set; }
        public TimeSignature TimeSignature { get; set; }
        public int DivisionLevel { get; set; }
        public List<Row> Rows { get; set; }
        public List<Measure> Measures { get; set; }

        public IEnumerable<string> GetSoundIds() => Rows.Select(r => r.SoundId);
    }
}
