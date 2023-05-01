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

        public string DisplayName 
        {
            get {
                StringBuilder matchupName = new StringBuilder();

                var entryCounter = 1;


                if (Entries.Count > 1)
                {
                    foreach (MatchupEntryModel me in Entries)
                    {
                        if (me.TeamCompeting != null)
                        {
                            if (entryCounter == 1)
                            {
                                matchupName.Insert(matchupName.Length, Entries[0].TeamCompeting.TeamName);
                                entryCounter++;
                            }
                            else
                            {
                                matchupName.Insert(matchupName.Length, " VS " + Entries[1].TeamCompeting.TeamName);
                            }
                        }
                        else
                        {
                            if (entryCounter == 1)
                            {
                                matchupName.Insert(matchupName.Length, "team unknown");
                                entryCounter++;
                            }
                            else
                            {
                                matchupName.Insert(matchupName.Length, " VS " + "team unknown");
                            }
                        }
                    }
                }
                else
                {
                    if (Entries[0].TeamCompeting != null)
                    {
                        matchupName.Insert(0, Entries[0].TeamCompeting.TeamName);
                    }
                    else
                    {
                        matchupName.Insert(matchupName.Length, "No teams qualified yet");
                    }
                }

                return matchupName.ToString();            
            }
        }

    }
}
