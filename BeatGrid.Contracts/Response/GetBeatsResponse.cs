using BeatGrid.Contracts.Common;
using System.Collections.Generic;

namespace BeatGrid.Contracts.Response
{
    public class GetBeatsResponse
    {
        public IEnumerable<Beat> Beats { get; set; }
        public IEnumerable<Sound> Sounds { get; set; }
    }
}
