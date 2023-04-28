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
        public int MatchupRound { get; set; }
        public List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();
        public int WinnerId { get; set; }    
        public TeamModel Winner { get; set; }
    }
}
