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

        private const string PrizesFile = "PrizesFile.csv";
        private const string ParticipantsFile = "ParticipantsFile.csv";
        private const string TeamsFile = "TeamsFile.csv";
        private const string TournamentsFile = "TournamentsFile.csv";
        private const string MatchupFile = "MatchupModels.csv";
        private const string MatchupEntryFile = "MatchupEntryModels.csv";

        /// <summary>
        /// Method that stores prizes into text file=
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            //load the text file and convert the text to List<PrizeModel>
            List<PrizeModel> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

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
            prizes.SaveToPrizesFile(PrizesFile);

            return model;
        }


        public ParticipantModel CreateParticipant(ParticipantModel model)
        {
            //load text file with all person models
            List<ParticipantModel> persons = ParticipantsFile.FullFilePath().LoadFile().ConvertToParticipantModels();

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
            persons.SaveToParticipantsFile(ParticipantsFile);


            return model;
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            List<TeamModel> teams = TeamsFile.FullFilePath().LoadFile().ConvertToTeamModels(ParticipantsFile);

            //find last id
            int currentId = 1;
            if (teams.Count > 0)
            {
                currentId = teams.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            teams.Add(model);

            teams.SaveToTeamsFile(TeamsFile);

            return model;
        }


        public List<ParticipantModel> LoadParticipants()
        {
            return ParticipantsFile.FullFilePath().LoadFile().ConvertToParticipantModels();
        }

        public List<TeamModel> LoadTeams()
        {
            return TeamsFile.FullFilePath().LoadFile().ConvertToTeamModels(ParticipantsFile);
        }

        public List<PrizeModel> LoadPrizes()
        {
            return PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();
        }

        public TournamentModel CreateTournament(TournamentModel model)
        {
            List<TournamentModel> tournaments = TournamentsFile
                .FullFilePath()
                .LoadFile()
                .ConvertToTournamentModels(TeamsFile, ParticipantsFile, PrizesFile);

            int currentId = 1;

            if (tournaments.Count > 0)
            {
                currentId = tournaments.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;
            model.SaveRoundsToFile(MatchupFile, MatchupEntryFile);
            tournaments.Add(model);
            tournaments.SaveToTournamentsFile(TournamentsFile);

            return model;
        
        }

        public List<TournamentModel> LoadTournaments()
        {
            throw new NotImplementedException();
        }
    }
}
