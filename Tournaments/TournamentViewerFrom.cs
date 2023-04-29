using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackersLibrary;
using TrackersLibrary.Models;

namespace Tournaments
{
    public partial class TournamentViewerFrom : Form
    {
        private List<TeamModel> teams = GlobalConfig.Connection.LoadTeams();
        private TournamentModel tournament;
        private List<MatchupModel> matchups;

        public TournamentViewerFrom(TournamentModel tournamentModel)
        {
            InitializeComponent();
            tournament = tournamentModel;
            //rounds = tournamentModel.Rounds;
            LoadFormData();
            WireUpRoundList();
            WireUpMatchupList();
            matchupListBox.SelectedIndex = 0;
            WireUpScores();
        }

        private void LoadFormData()
        {
            tournamentValue.Text = tournament.TournamentName;
        }

        private void WireUpRoundList()
        {
            List<int> rounds = new List<int>();
            var roundCounter = 1;
            foreach (List<MatchupModel> round in tournament.Rounds)
            {
                rounds.Add(roundCounter);
                roundCounter++;

            }
            roundComboBox.DataSource = rounds;
        }

        private void WireUpMatchupList()
        {
            matchups = tournament.Rounds[(int)roundComboBox.SelectedItem - 1];
            List<string>matches = new List<string>();

            foreach (MatchupModel matchup in matchups)
            {

                StringBuilder matchupName = new StringBuilder();
                foreach(MatchupEntryModel entry in matchup.Entries)
                {
                    if (entry.TeamCompetingId != 0 && matchup.Entries.Count > 1)
                    {
                        int Id = entry.TeamCompetingId;
                        TeamModel team = teams.Where(x => x.Id == Id).First();
                        matchupName.Insert(matchupName.Length, team.TeamName + " VS ");
                    }
                }
                if(matchupName.Length > 0)
                {
                    matchupName.Remove(matchupName.Length - 4, 4);
                    matches.Add(matchupName.ToString());
                }
            }
            matchupListBox.DataSource = null;
            matchupListBox.DataSource = matches;
        }

        private void WireUpScores()
        {
            //BUG WITH ODD NUMBER OF ROUNDS
            MatchupModel matchup = tournament.Rounds[(int)roundComboBox.SelectedItem - 1][matchupListBox.SelectedIndex];
            if (matchup.Entries.Count > 1)
            {
                var counter = 1;
                foreach (MatchupEntryModel entry in matchup.Entries)
                {
                    int Id = entry.TeamCompetingId;
                    TeamModel team = teams.Where(x => x.Id == Id).First();

                    if (counter == 1)
                    {
                        teamOneLabel.Text = team.TeamName;
                        teamOneScoreValue.Text = entry.Score.ToString();
                        counter++;
                    }
                    else
                    {
                        teamTwoLabel.Text = team.TeamName;
                        teamTwoScoreValue.Text = entry.Score.ToString();
                    }
                }
            }
        }

        private void matchupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (matchupListBox.SelectedIndex > -1)
            {
                WireUpScores();
            }
            else
            {
                ClearScores();
            }
        }

        private void roundComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            WireUpMatchupList();
        }

        private void ClearScores()
        {
            teamOneLabel.Text = "<team one>";
            teamTwoLabel.Text = "<team two>";
            teamOneScoreValue.Text = "0";
            teamTwoScoreValue.Text = "0"; 
        }

        private void scoreButton_Click(object sender, EventArgs e)
        {
            //TODO update winners !!! 

            List<MatchupEntryModel> entries = tournament.Rounds[(int)roundComboBox.SelectedItem - 1][matchupListBox.SelectedIndex].Entries;

            var teamCounter = 1;
            var entryCounter = 0;
            foreach (MatchupEntryModel entry in entries)
            {
                if (teamCounter == 1)
                {
                    entry.Score = int.Parse(teamOneScoreValue.Text);
                    GlobalConfig.Connection.UpdateEntry(entry);
                    tournament.Rounds[(int)roundComboBox.SelectedItem - 1][matchupListBox.SelectedIndex].Entries[entryCounter].Score = entry.Score;
                    teamCounter++;
                }
                else
                {
                    entry.Score = int.Parse(teamTwoScoreValue.Text);
                    GlobalConfig.Connection.UpdateEntry(entry);
                    tournament.Rounds[(int)roundComboBox.SelectedItem - 1][matchupListBox.SelectedIndex].Entries[entryCounter].Score = entry.Score;
                    teamCounter--;
                }
                if (entryCounter % 2 == 1 && entryCounter > 0)
                {
                    if(entries[entryCounter].Score > entries[entryCounter - 1].Score)
                    {
                        tournament.Rounds[(int)roundComboBox.SelectedItem - 1][matchupListBox.SelectedIndex].WinnerId = entries[entryCounter].TeamCompetingId;
                        GlobalConfig.Connection.UpdateMatchup(tournament.Rounds[(int)roundComboBox.SelectedItem - 1][matchupListBox.SelectedIndex]);
                    }
                    else
                    {
                        tournament.Rounds[(int)roundComboBox.SelectedItem - 1][matchupListBox.SelectedIndex].WinnerId = entries[entryCounter - 1].TeamCompetingId;
                        GlobalConfig.Connection.UpdateMatchup(tournament.Rounds[(int)roundComboBox.SelectedItem - 1][matchupListBox.SelectedIndex]);
                    }
                }
                entryCounter++;
            }
        }
    }
}
