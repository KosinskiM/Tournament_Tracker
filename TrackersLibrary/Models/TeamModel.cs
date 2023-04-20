using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackersLibrary.Models
{
    public class TeamModel
    {
        public int Id { get; set; }
        
        /// <summary>
        /// The name of the team.
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// The list of team members.
        /// </summary>
        public List<ParticipantModel> TeamMembers { get; set; } = new List<ParticipantModel>();
    }



}
