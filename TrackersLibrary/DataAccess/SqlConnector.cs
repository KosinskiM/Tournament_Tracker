using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackersLibrary.Models;

namespace TrackersLibrary.DataAccess
{
    public class SqlConnector : IDataConnection
    {
        private const string db = "Tournaments";



        //Create/Save methods

        /// <summary>
        /// Saves a new price to the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void CreatePrize(PrizeModel model)
        {
            using (IDbConnection connection = new MySqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("place_number", model.PlaceNumber);
                p.Add("place_name", model.PlaceName);
                p.Add("prize_amount", model.PrizeAmount);
                p.Add("prize_percentage", model.PrizePercentage);
                p.Add("prize_id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("insert_prize", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("prize_id");
            }
        }


        /// <summary>
        /// save new participant to database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void CreateParticipant(ParticipantModel model)
        {
            using (IDbConnection connection = new MySqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("first_name", model.FirstName);
                p.Add("last_name", model.LastName);
                p.Add("email", model.EmailAdress);
                p.Add("phone", model.CellPhoneNumber);
                p.Add("participant_id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("insert_participant", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("participant_id");
            }
        }

        /// <summary>
        /// save new team to database
        /// </summary>
        /// <returns></returns>
        public void CreateTeam(TeamModel model)
        {
            //adding new team to table[teams]
            using (IDbConnection connection = new MySqlConnection(GlobalConfig.CnnString(db)))
            {
                var t = new DynamicParameters();
                t.Add("team_id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                t.Add("team_name", model.TeamName);

                connection.Execute("insert_team", t, commandType: CommandType.StoredProcedure);

                var Id = t.Get<int>("team_id");
                model.Id = Id;

                //updating team_members[table]
                foreach (ParticipantModel tm in model.TeamMembers)
                {
                    var m = new DynamicParameters();
                    m.Add("team_id", model.Id);
                    m.Add("participant_id", tm.Id);
                    connection.Execute("insert_team_member", m, commandType: CommandType.StoredProcedure);
                }
            }
        }





        // load methods
        public List<ParticipantModel> LoadParticipants()
        {
            List<ParticipantModel> output;

            //count all participants
            using (IDbConnection connection = new MySqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<ParticipantModel>("get_participants_all",commandType: CommandType.StoredProcedure).OrderBy(x => x.Id).ToList();
            }
            return output;
        }
        public List<TeamModel> LoadTeams()
        {
            List<TeamModel> output = new List<TeamModel>();

            using (IDbConnection connection = new MySqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<TeamModel>("get_teams_all", commandType: CommandType.StoredProcedure).OrderBy(x => x.Id).ToList();

                foreach (TeamModel team in output)
                {
                    DynamicParameters p = new DynamicParameters();
                    p.Add("team_id", team.Id);
                    team.TeamMembers = connection.Query<ParticipantModel>("get_team_participants",p, commandType: CommandType.StoredProcedure).ToList();
                }

            }
            return output;
        }

        public List<PrizeModel> LoadPrizes()
        {
            List<PrizeModel> output = new List<PrizeModel>();

            using (IDbConnection connection = new MySqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<PrizeModel>("get_prizes_all", commandType: CommandType.StoredProcedure).OrderBy(x => x.Id).ToList();
            }
            return output;
        }














        public void CreateTournament(TournamentModel model)
        {
            using (IDbConnection connection = new MySqlConnection(GlobalConfig.CnnString(db)))
            {
                SaveTournament(model,connection);
                SaveTournamentPrizes(model, connection);
                SaveTournamentEntries(model, connection);
                SaveTournamentRounds(model, connection);
            }
        }

        private void SaveTournamentRounds(TournamentModel model, IDbConnection connection)
        {
            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel matchup in round)
                {
                    var p = new DynamicParameters();
                    p.Add("tournament_id", model.Id);
                    p.Add("matchup_round", matchup.MatchupRound);
                    if (matchup.WinnerId != 0)
                    {
                        p.Add("winner_team_id", matchup.WinnerId);
                    }
                    else
                    {
                        p.Add("winner_team_id", null);
                    }

                    p.Add("matchup_id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("insert_matchup", p, commandType: CommandType.StoredProcedure);

                    matchup.Id = p.Get<int>("matchup_id");

                    // TODO delete if wrong TournamentLogic.AdvanceMatchupWinner(matchup,model);

                    foreach (MatchupEntryModel entry in matchup.Entries)
                    {
                        p = new DynamicParameters();

                        p.Add("matchup_id", matchup.Id);

                        if (entry.ParentMatchup == null)
                        {
                            p.Add("parent_matchup_id", null);
                        }
                        else
                        {
                            p.Add("parent_matchup_id", entry.ParentMatchup.Id);
                        }

                        if (entry.TeamCompeting == null)
                        {
                            p.Add("team_competing_id", null);
                        }
                        else
                        {
                            p.Add("team_competing_id", entry.TeamCompeting.Id);
                        }

                        p.Add("matchup_entry_id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                        connection.Execute("insert_matchup_entry", p, commandType: CommandType.StoredProcedure);

                        entry.Id = p.Get<int>("matchup_entry_id");
                    }
                }
            }
        
        }

        private void SaveTournamentEntries(TournamentModel model, IDbConnection connection)
        {
            foreach (TeamModel team in model.EnteredTeams)
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("tournament_entry_id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("tournament_id", model.Id);
                p.Add("team_id", team.Id);

                connection.Execute("insert_tournament_entry", p, commandType: CommandType.StoredProcedure);
            }

        }

        private void SaveTournamentPrizes(TournamentModel model, IDbConnection connection)
        {

            foreach (PrizeModel prize in model.Prizes)
            {
                ////blad !!!
                //DynamicParameters p = new DynamicParameters();
                //p.Add("prize_id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                //p.Add("place_number", prize.PlaceNumber);
                //p.Add("place_name", prize.PlaceName);
                //p.Add("prize_amount", prize.PrizeAmount);
                //p.Add("prize_percentage", prize.PrizePercentage);

                //connection.Execute("insert_prize", p, commandType: CommandType.StoredProcedure);

                //prize.Id = p.Get<int>("prize_id");
                ////blad !!
                
                DynamicParameters p = new DynamicParameters();
                p = new DynamicParameters();
                p.Add("prize_id", prize.Id);
                p.Add("tournament_id", model.Id);

                connection.Execute("insert_tournament_prize", p, commandType: CommandType.StoredProcedure);

            }

        }

        private void SaveTournament(TournamentModel model, IDbConnection connection)
        {
            //TODO zmienic sql tak zeby odczytywal id a nie liczyl rekordy

            DynamicParameters p = new DynamicParameters();
            p.Add("tournament_name", model.TournamentName);
            p.Add("entry_fee", model.EntryFee);
            p.Add("tournament_id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            connection.Execute("insert_tournament", p, commandType: CommandType.StoredProcedure);

            model.Id = p.Get<int>("tournament_id");

        }

        public List<TournamentModel> LoadTournaments()
        {
            // TODO finish load tournament method in sql 

            List<TournamentModel> tournaments;
            List<List<MatchupModel>> rounds = new List<List<MatchupModel>>();
            List<MatchupModel> matchups;

            //count all participants
            using (IDbConnection connection = new MySqlConnection(GlobalConfig.CnnString(db)))
            {
                tournaments = connection.Query<TournamentModel>("get_tournaments_all", commandType: CommandType.StoredProcedure).OrderBy(x => x.Id).ToList();

                //populate teams
                foreach (TournamentModel t in tournaments)
                {
                    rounds.Clear();
                    DynamicParameters p = new DynamicParameters();
                    p.Add("tournament_id",t.Id);
                    t.EnteredTeams = connection.Query<TeamModel>("get_teamsByTournamentId", p, commandType: CommandType.StoredProcedure).OrderBy(x => x.Id).ToList();
                    t.Prizes = connection.Query<PrizeModel>("get_prizesByTournamentId", p, commandType: CommandType.StoredProcedure).OrderBy(x => x.Id).ToList();

                    foreach (TeamModel tm in t.EnteredTeams)
                    {
                        p = new DynamicParameters();
                        p.Add("team_id", tm.Id);
                        tm.TeamMembers = connection.Query<ParticipantModel>("get_participantsByTeamId", p, commandType: CommandType.StoredProcedure).OrderBy(x => x.Id).ToList(); 
                    }

                    p = new DynamicParameters();
                    p.Add("tournament_id", t.Id);


                    //populate rounds
                    List<MatchupModel> round = new List<MatchupModel>();
                    matchups = (connection.Query<MatchupModel>("get_matchupsByTournamentId", p, commandType: CommandType.StoredProcedure).OrderBy(x => x.Id).ToList());

                    int lastRound = 1;
                    int matchupCounter = 1;
                    foreach (MatchupModel matchup in matchups)
                    {
                        //populate the winner
                        if (matchup.WinnerId > 0)
                        {
                            //p = new DynamicParameters();
                            //p.Add("winner_id", matchup.WinnerId);
                            //p.Add("TeamName", 0, dbType: DbType.String, ParameterDirection.Output);
                            //connection.Execute("get_teamsByWinnerId", p, commandType: CommandType.StoredProcedure);
                            //matchup.Winner.TeamName = p.Get<string>("TeamName");
                            matchup.Winner = t.EnteredTeams.Where(x => x.Id == matchup.WinnerId).First();

                            p = new DynamicParameters();
                            p.Add("team_id", matchup.WinnerId);
                            matchup.Winner.TeamMembers = connection.Query<ParticipantModel>("get_participantsByTeamId", p, commandType: CommandType.StoredProcedure).OrderBy(x => x.Id).ToList();
                        }

                        //populate entries
                        p = new DynamicParameters();
                        p.Add("matchup_id", matchup.Id);
                        matchup.Entries = connection.Query<MatchupEntryModel>("get_matchupEntriesByMatchupId", p, commandType: CommandType.StoredProcedure).OrderBy(x => x.Id).ToList();
                        
                        //populate entries teams
                        foreach (MatchupEntryModel me in matchup.Entries)
                        {
                            if (me.TeamCompetingId != 0)
                            {
                                me.TeamCompeting = t.EnteredTeams.Where(x => x.Id == me.TeamCompetingId).First();
                            }
                        }


                        if(matchup.MatchupRound == lastRound)
                        {
                            round.Add(matchup);
                        }
                        else
                        {
                            rounds.Add(round);
                            lastRound++;
                            round = new List<MatchupModel>{matchup};
                        }

                        if (matchups.Count == matchupCounter)
                        {
                            rounds.Add(round);
                        }

                        matchupCounter++;
                    }
                    t.Rounds = rounds;
                }
            }

            //TODO ROUNDS ERROR !!!! check it

            return tournaments;
        }



        
        public void UpdateEntry(MatchupEntryModel entry)
        {
            using (IDbConnection connection = new MySqlConnection(GlobalConfig.CnnString(db)))
            {
                DynamicParameters p = new DynamicParameters();

                p.Add("entry_id", entry.Id);
                p.Add("score", entry.Score);

                connection.Execute("update_entryById",p,commandType: CommandType.StoredProcedure);
            }
        }

        public void UpdateMatchup(MatchupModel matchup)
        {
            using (IDbConnection connection = new MySqlConnection(GlobalConfig.CnnString(db)))
            {
                DynamicParameters p = new DynamicParameters();

                p.Add("matchup_id", matchup.Id);
                p.Add("winner_id", matchup.WinnerId);
                connection.Execute("update_matchupById", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void CompleteTournament(TournamentModel model)
        {
            using (IDbConnection connection = new MySqlConnection(GlobalConfig.CnnString(db)))
            {
                DynamicParameters p = new DynamicParameters();

                p.Add("tournament_id", model.Id);
                connection.Execute("update_tournamentStatus", p, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
