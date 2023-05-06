using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackersLibrary.Models;
using TrackersLibrary.DataAccess.TextHelpers;

namespace TrackersLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {

        public void CreatePrize(PrizeModel model)
        {
            //load the text file and convert the text to List<PrizeModel>
            List<PrizeModel> prizes = GlobalConfig.PrizesFile.FullFilePath().LoadFile().ConvertStringToPrizeModels();

            //Find the Id
            int currentId = 1;

            if(prizes.Count >0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }
            
            model.Id = currentId;

            //Add new record with new id
            prizes.Add(model);


            //save the list<PrizeModel> to the text file
            prizes.SaveToPrizesFile();

        }
        public void CreateParticipant(ParticipantModel model)
        {
            //load text file with all person models
            List<ParticipantModel> persons = GlobalConfig.ParticipantsFile.FullFilePath().LoadFile().ConvertStringToParticipantModels();

            //find last id
            int currentId = 1;

            if (persons.Count > 0)
            {
                currentId = persons.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            //adding new record to file
            persons.Add(model);

            //save the list<PersonModel> to the text file
            persons.SaveToParticipantsFile();
        }
        public void CreateTeam(TeamModel model)
        {
            List<TeamModel> teams = GlobalConfig.TeamsFile.FullFilePath().LoadFile().ConvertStringToTeamModels();

            //find last id
            int currentId = 1;
            if (teams.Count > 0)
            {
                currentId = teams.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            teams.Add(model);

            teams.SaveToTeamsFile();
        }
        public void CreateTournament(TournamentModel model)
        {
            List<TournamentModel> tournaments = GlobalConfig.TournamentsFile
                .FullFilePath()
                .LoadFile()
                .ConvertStringToTournamentModels();

            int currentId = 1;

            if (tournaments.Count > 0)
            {
                currentId = tournaments.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;
            model.SaveRoundsToFile();
            tournaments.Add(model);
            tournaments.SaveToTournamentsFile();
        }


        public List<ParticipantModel> LoadParticipants()
        {
            return GlobalConfig.ParticipantsFile.FullFilePath().LoadFile().ConvertStringToParticipantModels();
        }

        public List<TeamModel> LoadTeams()
        {
            return GlobalConfig.TeamsFile.FullFilePath().LoadFile().ConvertStringToTeamModels();
        }

        public List<PrizeModel> LoadPrizes()
        {
            return GlobalConfig.PrizesFile.FullFilePath().LoadFile().ConvertStringToPrizeModels();
        }

        public List<TournamentModel> LoadTournaments()
        {
            List<TournamentModel> tournaments = GlobalConfig.TournamentsFile
                .FullFilePath()
                .LoadFile()
                .ConvertStringToTournamentModels();
            return tournaments;
        }


        public void UpdateEntry(MatchupEntryModel entry)
        {
            entry.UpdateEntryToFile();
        }

        public void UpdateMatchup(MatchupModel model)
        {
            model.UpdateMatchupToFile();
        }
    }
}
