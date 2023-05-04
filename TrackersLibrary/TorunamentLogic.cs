using System;
using System.Collections.Generic;
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
                    currentMatchup.Entries.Add(new MatchupEntryModel { ParentMatchup = match , ParentMatchupId = match.Id});

                    if(currentMatchup.Entries.Count > 1)
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
                matchup.Entries.Add(new MatchupEntryModel { TeamCompeting = team , TeamCompetingId = team.Id});

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

        public static void AdvanceWinners(TournamentModel tournament)
        {
            for (int i = 0; i < tournament.Rounds.Count - 1; i++)
            {
                foreach (MatchupModel mRound1 in tournament.Rounds[i])
                {
                    if (mRound1.Winner != null)
                    {
                        foreach (MatchupModel mRound2 in tournament.Rounds[i+1])
                        {
                            foreach(MatchupEntryModel entry in mRound2.Entries)
                            {
                                if (entry.ParentMatchup.Id == mRound1.Id)
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


        public static void AdvanceMatchupWinner(MatchupModel matchup, TournamentModel tournament)
        {
            if (matchup.Winner != null)
            {
                if(matchup.MatchupRound + 1 < tournament.Rounds.Count)
                {
                    foreach (MatchupModel m2 in tournament.Rounds[matchup.MatchupRound + 1])
                    {
                        foreach (MatchupEntryModel entry in m2.Entries)
                        {
                            if (entry.ParentMatchup.Id == matchup.Id)
                            {
                                entry.TeamCompetingId = m2.WinnerId;
                                entry.TeamCompeting = m2.Winner;
                            }
                        }
                    }
                }
            }








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
                                if (entry.ParentMatchupId == mRound1.Id)
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




    }
}
