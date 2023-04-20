using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackersLibrary.Models;

namespace TrackersLibrary.DataAccess.TextHelpers
{
    public static class TextConnectorProcessor
    {


        public static string FullFilePath(this string fileName)
        {
            return $"{ ConfigurationManager.AppSettings["filePath"] }\\{ fileName }";
        }

        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }
            return File.ReadAllLines(file).ToList();
        }





        //convert to models
        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();
            
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                PrizeModel p = new PrizeModel();
                p.Id = int.Parse(cols[0]);
                p.PlaceNumber = int.Parse(cols[1]);
                p.PlaceName = cols[2];
                p.PrizeAmount = decimal.Parse(cols[3]);
                p.PrizePercentage = double.Parse(cols[4]);
                output.Add(p);
            }
            return output;
        }
        public static List<ParticipantModel> ConvertToParticipantModels(this List<string> lines)
        {
            List<ParticipantModel> output = new List<ParticipantModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                ParticipantModel p = new ParticipantModel();
                p.Id = int.Parse(cols[0]);
                p.FirstName = cols[1];
                p.LastName = cols[2];
                p.EmailAdress = cols[3];
                p.CellPhoneNumber = cols[4];

                output.Add(p);
            }
            return output;
        }

        public static List<TeamModel> ConvertToTeamModels(this List<string> lines, string participantsFileName)
        {
            List<TeamModel> output = new List<TeamModel>();
            //load text file with all exisitng participants

            List<ParticipantModel> participants = participantsFileName.FullFilePath().LoadFile().ConvertToParticipantModels();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TeamModel t = new TeamModel();
                t.Id = int.Parse(cols[0]);
                t.TeamName = cols[1];

                string[] teamMembersIds = cols[2].Split('|');

                foreach (string id in teamMembersIds)
                {
                    t.TeamMembers.Add(participants.Where(x => x.Id == int.Parse(id)).First());
                }
                output.Add(t);
            }
            return output;
        }




        //save to files
        public static void SaveToPrizesFile(this List<PrizeModel> models, string fileName)
        { 
            
            List<string> lines = new List<string>();

            foreach (PrizeModel p in models)
            {
                lines.Add($"{ p.Id },{ p.PlaceNumber },{ p.PlaceName },{ p.PrizeAmount },{ p.PrizePercentage }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToParticipantsFile(this List<ParticipantModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach  (ParticipantModel p in models)
            {
                lines.Add($"{ p.Id },{ p.FirstName },{ p.LastName },{ p.EmailAdress },{ p.CellPhoneNumber }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToTeamsFile(this List<TeamModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (TeamModel team in models)
            {
                lines.Add($"{ team.Id },{ team.TeamName },{ ConvertParticipantsListToString(team.TeamMembers)}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToTournamentsFile(this List<TournamentModel> models, string fileName)
        {
            List<string>lines = new List<string>();

            foreach (TournamentModel tm in models)
            {
                lines.Add($@"{ tm.Id },
                             { tm.TournamentName },
                             { tm.EntryFee },
                             { ConvertTeamListToString(tm.EnteredTeams) },
                             { ConvertPrizesListToString(tm.Prizes) },
                             { ConvertRoundListToString(tm.Rounds) },");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);

        }


























        private static string ConvertRoundListToString(List<List<MatchupModel>> rounds)
        {
            // rounds - (id^id^id|id^id^id|id^id^id)

            string output = "";

            if (rounds.Count == 0)
            {
                return "";
            }

            foreach (List<MatchupModel> r in rounds)
            {
                output += $"{ ConvertMatchupListToString(r) }|";
            }

            return output.Substring(0, output.Length - 1);
        }

        private static string ConvertMatchupListToString(List<MatchupModel> matchups)
        {
            string output = "";

            if (matchups.Count == 0)
            {
                return "";
            }

            foreach (MatchupModel m in matchups)
            {
                output += $"{ m.Id }^";
            }

            return output.Substring(0, output.Length - 1);
        }






        private static string ConvertPrizesListToString(List<PrizeModel> prizes)
        {
            string output = "";

            if (prizes.Count == 0)
            {
                return "";
            }

            foreach (PrizeModel p in prizes)
            {
                output += $"{ p.Id }|";
            }

            return output.Substring(0, output.Length - 1);
        }

        private static string ConvertParticipantsListToString(List<ParticipantModel> participants)
        {
            string output = "";

            if (participants.Count == 0)
            {
                return "";
            }

            foreach (ParticipantModel p in participants)
            {
                output += $"{ p.Id }|";
            }

            return output.Substring(0, output.Length - 1);
        }

        public static string ConvertTeamListToString(List<TeamModel> teams)
        {
            string output = "";

            if (teams.Count == 0)
            {
                return "";
            }

            foreach (TeamModel t in teams)
            {
                output += $"{ t.Id }|";
            }

            return output.Substring(0, output.Length - 1);
        }


        public static List<TournamentModel> ConvertToTournamentModels(this List<string> lines, string teamsFileName, string participantsFileName, string prizesFileName)
        {
            // TODO convert ot tournaments model method
            //data structer in text file
            //id, TournamentName, EntryFee, (id|id|id - Entered Teams), (id|id|id - Prizes), (Rounds - id^id^id|id^id^id|id^id^id)

            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teams = teamsFileName.FullFilePath().LoadFile().ConvertToTeamModels(participantsFileName);
            List<PrizeModel> prizes = prizesFileName.FullFilePath().LoadFile().ConvertToPrizeModels();


            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TournamentModel tm = new TournamentModel();
                tm.Id = int.Parse(cols[0]);
                tm.TournamentName = cols[1];
                tm.EntryFee = decimal.Parse(cols[2]);

                string[] teamIds = cols[3].Split('|');

                foreach (string id in teamIds)
                {
                    tm.EnteredTeams.Add(teams.Where(x => x.Id == int.Parse(id)).First());
                }

                string[] prizeIds = cols[4].Split('|');

                foreach (string id in prizeIds)
                {
                    tm.Prizes.Add(prizes.Where(x => x.Id == int.Parse(id)).First());
                }

                // TODO capture Rounds information

            }

            return output;
        }




    }
}
