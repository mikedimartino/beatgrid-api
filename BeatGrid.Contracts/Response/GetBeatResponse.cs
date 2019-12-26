using System.Collections.Generic;
using BeatGrid.Contracts.Common;

namespace BeatGrid.Contracts.Response
{
    public class GetBeatResponse
    {
        public Beat Beat { get; set; }
        public IEnumerable<Sound> Sounds { get; set; }
    }
}
