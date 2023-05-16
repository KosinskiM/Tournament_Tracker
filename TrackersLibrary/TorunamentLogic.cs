using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackersLibrary.Models;

namespace TrackersLibrary
{
    public static class TournamentLogic
    {


        public static void CreateRounds(TournamentModel model)
        {
            List<TeamModel> randomizedTeams = RandomizeTeamOrder(model.EnteredTeams);
            int rounds = FindNumberOfRounds(randomizedTeams.Count);
            int byes = NumberOfByes(rounds, randomizedTeams.Count);

            model.Rounds.Add(CreateFirstRound(byes, randomizedTeams));

            CreateOtherRounds(model, rounds);
            UpdateSingleTeamMatchups(model);
            AdvanceBuyWinner(model);
        }

        private static void CreateOtherRounds(TournamentModel model, int rounds)
        {
            int round = 2;
            List<MatchupModel> previousRound = model.Rounds[0];
            List<MatchupModel> currentRound = new List<MatchupModel>();
            MatchupModel currentMatchup = new MatchupModel();

            while (round <= rounds)
            {
                foreach (MatchupModel match in previousRound)
                {
                    currentMatchup.Entries.Add(new MatchupEntryModel { ParentMatchup = match, ParentMatchupId = match.Id });

                    if (currentMatchup.Entries.Count > 1)
                    {
                        currentMatchup.MatchupRound = round;
                        currentRound.Add(currentMatchup);
                        currentMatchup = new MatchupModel();
                    }
                }

                model.Rounds.Add(currentRound);
                previousRound = currentRound;

                currentRound = new List<MatchupModel>();
                round += 1;
            }

        }

        public static List<MatchupModel> CreateFirstRound(int byes, List<TeamModel> teams)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            MatchupModel matchup = new MatchupModel();

            foreach (TeamModel team in teams)
            {
                matchup.Entries.Add(new MatchupEntryModel { TeamCompeting = team, TeamCompetingId = team.Id });

                if (byes > 0 || matchup.Entries.Count > 1)
                {
                    matchup.MatchupRound = 1;
                    output.Add(matchup);
                    matchup = new MatchupModel();

                    if (byes > 0)
                    {
                        byes -= 1;
                    }
                }
            }
            return output;
        }

        private static int NumberOfByes(int rounds, int numberOfTeams)
        {
            int totalTeams = 1;

            for (int i = 1; i <= rounds; i++)
            {
                totalTeams *= 2;
            }

            return (totalTeams - numberOfTeams);
        }

        public static int FindNumberOfRounds(int teamCount)
        {
            int output = 1;
            int val = 2;

            while (val < teamCount)
            {
                output += 1;
                val *= 2;
            }

            return output;
        }

        public static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            return teams.OrderBy(x => Guid.NewGuid()).ToList();
        }

        public static TournamentModel UpdateSingleTeamMatchups(TournamentModel tournament)
        {

            foreach (List<MatchupModel> round in tournament.Rounds)
            {
                foreach (MatchupModel matchup in round)
                {
                    if (matchup.Entries.Count == 1)
                    {
                        matchup.WinnerId = matchup.Entries[0].TeamCompetingId;
                        matchup.Winner = matchup.Entries[0].TeamCompeting;
                    }
                }
            }
            return tournament;
        }

        public static void AdvanceBuyWinner(TournamentModel tournament)
        {
            //zle bo matchup zaczyna id od 0
            for (int i = 0; i < tournament.Rounds.Count - 1; i++)
            {
                foreach (MatchupModel mRound1 in tournament.Rounds[i])
                {
                    if (mRound1.Winner != null)
                    {
                        foreach (MatchupModel mRound2 in tournament.Rounds[i + 1])
                        {
                            foreach (MatchupEntryModel entry in mRound2.Entries)
                            {
                                if (entry.ParentMatchup == mRound1)
                                {
                                    entry.TeamCompetingId = mRound1.WinnerId;
                                    entry.TeamCompeting = mRound1.Winner;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void UpdateWinnersInTournament(int entryCounter, MatchupModel matchup)
        {
            string moreOrLessWins = ConfigurationManager.AppSettings["moreOrLessWins"];

            if (entryCounter % 2 == 1 && entryCounter > 0)
            {
                if (moreOrLessWins == "1")
                {
                    if (matchup.Entries[entryCounter].Score > matchup.Entries[entryCounter - 1].Score)
                    {
                        matchup.WinnerId = matchup.Entries[entryCounter].TeamCompeting.Id;
                        matchup.Winner = matchup.Entries[entryCounter].TeamCompeting;
                    }
                    else if (matchup.Entries[entryCounter].Score < matchup.Entries[entryCounter - 1].Score)
                    {
                        matchup.WinnerId = matchup.Entries[entryCounter - 1].TeamCompeting.Id;
                        matchup.Winner = matchup.Entries[entryCounter - 1].TeamCompeting;
                    }
                }
                else
                {
                    if (matchup.Entries[entryCounter].Score < matchup.Entries[entryCounter - 1].Score)
                    {
                        matchup.WinnerId = matchup.Entries[entryCounter].TeamCompeting.Id;
                        matchup.Winner = matchup.Entries[entryCounter].TeamCompeting;
                    }
                    else if (matchup.Entries[entryCounter].Score > matchup.Entries[entryCounter - 1].Score)
                    {
                        matchup.WinnerId = matchup.Entries[entryCounter - 1].TeamCompeting.Id;
                        matchup.Winner = matchup.Entries[entryCounter - 1].TeamCompeting;
                    }
                }
            }
            entryCounter++;
        }

        public static void AdvanceMatchupWinner(MatchupModel matchup, TournamentModel tournament)
        {
            if (matchup.Winner != null)
            {
                if (matchup.MatchupRound < tournament.Rounds.Count)
                {
                    foreach (MatchupModel m2 in tournament.Rounds[matchup.MatchupRound])
                    {
                        foreach (MatchupEntryModel entry in m2.Entries)
                        {
                            if (entry.ParentMatchup == matchup || entry.ParentMatchupId == matchup.Id)
                            {
                                entry.TeamCompetingId = matchup.WinnerId;
                                entry.TeamCompeting = matchup.Winner;
                            }
                        }
                    }
                }
            }
            AlertUsersToNewRound(tournament);
        }

        private static void AlertUsersToNewRound(this TournamentModel model)
        { 
            int currentRoundNumber = model.CheckCurrentRound();
            if (currentRoundNumber > 1)
            {
                List<MatchupModel> currentRound = model.Rounds.Where(x => x.First().MatchupRound == currentRoundNumber).First();

                foreach (MatchupModel matchup in currentRound)
                {
                    foreach (MatchupEntryModel entry in matchup.Entries)
                    {
                        foreach (ParticipantModel p in entry.TeamCompeting.TeamMembers)
                        {
                            AlertParticipantToNewRound(p, entry.TeamCompeting.TeamName,matchup.Entries.Where(x => x.TeamCompeting != entry.TeamCompeting).FirstOrDefault());
                        }
                    }
                }
            }
        }

        private static void AlertParticipantToNewRound(ParticipantModel p, string teamName, MatchupEntryModel competitor)
        {
            if (p.EmailAdress.Length == 0 )
            {
                return;
            }
            string toAddress = "";
            string subject = "";
            StringBuilder body = new StringBuilder();

            if (competitor != null)
            {
                subject = $"You have a new matchup with {competitor.TeamCompeting.TeamName}";
                body.AppendLine("<h1> You have a new matchup </h1>");
                body.AppendLine("<p><strong>Competitor:</strong>");
                body.Append(competitor.TeamCompeting.TeamName);
                body.AppendLine("\n");
                body.AppendLine("\nHave a great time!");
                body.AppendLine("\n~Tournament Tracker </p>");
            }
            else
            {
                subject = "You have a bye week this round";
                body.AppendLine("Have a great time on your round off!");
                body.AppendLine("~Tournament Tracker </p>");
            }

            toAddress = p.EmailAdress;
            EmailLogic.SendEmail(toAddress, subject, body.ToString());
        }

        private static int CheckCurrentRound(this TournamentModel model)
        {
            int output = 1;

            foreach (List<MatchupModel> round in model.Rounds)
            {
                if (round.All(x => x.Winner != null))
                {
                    output += 1;
                }
                else return output;
            }

            CompleteTournament(model);
            return output - 1;
        }

        private static void CompleteTournament(TournamentModel model)
        {
            GlobalConfig.Connection.CompleteTournament(model);
            TeamModel winners = model.Rounds.Last().First().Winner;
            TeamModel runnerUp = model.Rounds.Last().First().Entries.Where(x => x.TeamCompeting != winners).First().TeamCompeting;

            decimal winnerPrize = 0;
            decimal runnerUpPrize = 0;


            if (model.Prizes.Count > 0)
            {
                decimal totalIncome = model.EnteredTeams.Count * model.EntryFee;

                PrizeModel firstPlacePrize = model.Prizes.Where(x => x.PlaceNumber == 1).FirstOrDefault();
                PrizeModel secondPlacePrize = model.Prizes.Where(x => x.PlaceNumber == 2).FirstOrDefault();

                if (firstPlacePrize != null)
                {
                    winnerPrize = firstPlacePrize.CalculatePrizePayout(totalIncome);
                }

                if (secondPlacePrize != null)
                {
                    runnerUpPrize = secondPlacePrize.CalculatePrizePayout(totalIncome);
                }
            }

            string subject = "";
            StringBuilder body = new StringBuilder();

            subject = $"In { model.TournamentName }, { winners.TeamName }, has won!";

            body.AppendLine("<h1> We have a WINNER ! </h1>");
            body.AppendLine("<p><strong>Congratulations to our winner on a great tournament</strong></p>");
            body.AppendLine("<br />");

            if (winnerPrize > 0)
            {
                body.AppendLine("<br />");
                body.AppendLine($"<p>{ winners.TeamName } will recive { winnerPrize } </p>");
            }

            if (runnerUpPrize > 0)
            {
                body.AppendLine("<br />");
                body.AppendLine($"<p>{ winners.TeamName } will recive { runnerUpPrize } </p>");
            }

            body.AppendLine("<br />");
            body.AppendLine("<p>Thanks for competing !</p>");
            body.AppendLine("<p>~Tournament Tracker </p>");

            //email adresses of every person on every team
            List<string> bcc = new List<string>();
            foreach  (TeamModel team in model.EnteredTeams)
            {
                foreach (ParticipantModel participant in team.TeamMembers)
                {
                    if (participant.EmailAdress.Length > 0)
                    {
                        bcc.Add(participant.EmailAdress);
                    }
                }
            }

            EmailLogic.SendEmail(new List<string>(),bcc, subject, body.ToString());

            model.CompleteTournament();

        }

        private static decimal CalculatePrizePayout(this PrizeModel prize, decimal totalIncome)
        {
            decimal output = 0;

            if (prize.PrizeAmount > 0)
            {
                output = prize.PrizeAmount;
            }
            else
            {
                output = Decimal.Multiply(totalIncome,Convert.ToDecimal(prize.PrizePercentage / 100));
            }
            return output;
        }
    }
}
