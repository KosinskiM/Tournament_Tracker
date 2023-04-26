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

        //creation of objects
        PrizeModel CreatePrize(PrizeModel model);
        ParticipantModel CreateParticipant(ParticipantModel model);

        TeamModel CreateTeam(TeamModel model);

        TournamentModel CreateTournament(TournamentModel model);

        //reading from storage
        List<ParticipantModel> LoadParticipants();

        List<TeamModel> LoadTeams();

        List<PrizeModel> LoadPrizes();

        List<TournamentModel> LoadTournaments();

    }
}
