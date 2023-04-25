using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackersLibrary.Models
{
    /// <summary>
    /// 
    /// </summary>

    public class MatchupEntryModel
    {
        public int Id { get; set; }
        public TeamModel TeamCompeting { get; set; } 
        public double Score { get; set; }
        public MatchupModel ParentMatchup { get; set; }

        /*

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialScore"></param>
        public MatchupEntryModel(double initialScore)
        {
            Score = initialScore;
        
        }
        */

    }
}
