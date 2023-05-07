using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackersLibrary.Models;

namespace TrackersLibrary.DataAccess
{
    public interface IDataConnection
    {
        void CreatePrize(PrizeModel model);
        void CreateParticipant(ParticipantModel model);
        void CreateTeam(TeamModel model);
        void CreateTournament(TournamentModel model);

        //reading from storage
        List<ParticipantModel> LoadParticipants();
        List<TeamModel> LoadTeams();
        List<PrizeModel> LoadPrizes();
        List<TournamentModel> LoadTournaments();


        void UpdateEntry(MatchupEntryModel model);
        void UpdateMatchup(MatchupModel model);

    }
}
