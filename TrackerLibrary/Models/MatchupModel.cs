using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class MatchupModel
    {
        public List<int> Entries { get; set; } = new List<int>();
        public TeamModel Winner { get; set; }
        public int MatchupRound { get; set; }

    }
}
