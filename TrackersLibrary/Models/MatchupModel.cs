using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackersLibrary.Models
{
    public class MatchupModel
    {
        public int Id { get; set; }
        public List<int> Entries { get; set; } = new List<int>();
        public TeamModel Winner { get; set; }
        public int MatchupRound { get; set; }

    }
}
