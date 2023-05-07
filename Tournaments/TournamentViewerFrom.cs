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
        private TournamentModel tournament;
        List<int> rounds = new List<int>();
        private List<MatchupModel> selectedMatchups;

        public TournamentViewerFrom(TournamentModel tournamentModel)
        {
            InitializeComponent();
            tournament = tournamentModel;

            LoadFormData();
            LoadRounds();
            LoadMatchups();
            DisplayMatchupInfo();
        }
        private void LoadFormData()
        {
            tournamentValue.Text = tournament.TournamentName;
        }

        public void WireUpRounds()
        {
            roundComboBox.DataSource = null;
            roundComboBox.DataSource = rounds;
        }
        public void WireUpMatchups()
        {
            matchupListBox.DataSource = null;
            matchupListBox.DataSource = selectedMatchups;
            matchupListBox.DisplayMember = "DisplayName";
            DisplayMatchupInfo();
        }


        private void LoadRounds()
        {
            var roundCounter = 1;
            foreach (List<MatchupModel> round in tournament.Rounds)
            {
                rounds.Add(roundCounter);
                roundCounter++;
            }

            WireUpRounds();
        }

        private void LoadMatchups()
        {
            int round = (int)roundComboBox.SelectedItem;
            selectedMatchups = new List<MatchupModel>();
            if (unplayedOnlyCheckBox.Checked)
            {
                foreach (MatchupModel matchup in tournament.Rounds[round - 1])
                {
                    if (matchup.Winner == null)
                    {
                        selectedMatchups.Add(matchup);
                    }
                }
            }
            else
            {
                foreach (MatchupModel matchup in tournament.Rounds[round - 1])
                {
                    selectedMatchups.Add(matchup);
                }
            }
            WireUpMatchups();
        }


        private void LoadMatchup()
        {
            //BUG WITH ODD NUMBER OF ROUNDS
            MatchupModel matchup = tournament.Rounds[(int)roundComboBox.SelectedItem - 1][matchupListBox.SelectedIndex];
            if (matchup.Entries.Count > 1)
            {
                var entryCounter = 1;
                foreach (MatchupEntryModel entry in matchup.Entries)
                {
                    if (entry.TeamCompeting != null)
                    {
                        if (entryCounter == 1)
                        {
                            teamOneLabel.Text = entry.TeamCompeting.TeamName;
                            teamOneScoreValue.Text = entry.Score.ToString();
                            entryCounter++;
                        }
                        else
                        {
                            teamTwoLabel.Text = entry.TeamCompeting.TeamName;
                            teamTwoScoreValue.Text = entry.Score.ToString();
                            entryCounter = 1;
                        } 
                    }
                }
            }
            else
            {
                ClearMatchup();
            }
        }

        private void matchupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (matchupListBox.SelectedIndex > -1)
            {
                LoadMatchup();
                            DisplayMatchupInfo();
            }
            else
            {
                ClearMatchup();
            }
        }

        private void roundComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchups();
            DisplayMatchupInfo();
        }

        private void ClearMatchup()
        {
            teamOneLabel.Text = "<team one>";
            teamTwoLabel.Text = "<team two>";
            teamOneScoreValue.Text = "0";
            teamTwoScoreValue.Text = "0"; 
        }

        private void scoreButton_Click(object sender, EventArgs e)
        {
            string errorMessage = ValidateData();
            if (errorMessage.Length > 0)
            {
                MessageBox.Show($"Input Error: { errorMessage }");
                return;
            }

            MatchupModel matchup = (MatchupModel)matchupListBox.SelectedItem;
            List<MatchupEntryModel> entries = matchup.Entries;
            var teamCounter = 1;
            var entryCounter = 0;
            foreach (MatchupEntryModel entry in entries)
            {
                if (teamCounter == 1)
                {
                    entry.Score = double.Parse(teamOneScoreValue.Text);
                    GlobalConfig.Connection.UpdateEntry(entry);
                    matchup.Entries[entryCounter].Score = entry.Score;
                    teamCounter++;
                }
                else
                {
                    entry.Score = double.Parse(teamTwoScoreValue.Text);
                    GlobalConfig.Connection.UpdateEntry(entry);
                    matchup.Entries[entryCounter].Score = entry.Score;
                    teamCounter--;
                }
                TournamentLogic.UpdateWinnersInTournament(entryCounter, matchup);
                entryCounter++;
            }
            GlobalConfig.Connection.UpdateMatchup(matchup);
            TournamentLogic.AdvanceMatchupWinner(matchup, tournament);
            LoadMatchups();
        }

        private string ValidateData()
        {
            string errorMessage = "";

            double teamOneScore = 0;
            double teamTwoScore = 0;

            var t1valid = double.TryParse(teamOneScoreValue.Text, out teamOneScore);
            var t2valid = double.TryParse(teamTwoScoreValue.Text, out teamTwoScore);


            if (teamOneScore == teamTwoScore)
            {
                errorMessage = "Ties are not allowed";
            }
            else if(!t1valid)
            {
                errorMessage = "Team one score is invalid";
            }
            else if(!t2valid)
            {
                errorMessage = "Team two score is invalid";
            }
            else if(!t1valid && !t2valid)
            {
                errorMessage = "Team one and Team two scores are invalid";
            }

            return errorMessage;
        }









        private void unplayedOnlyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LoadMatchups();
        }

        private void DisplayMatchupInfo()
        {
            MatchupModel matchup = matchupListBox.SelectedItem as MatchupModel;

            if (matchup == null) return;

            bool isPlayed = matchup.Winner != null;
            bool isSingle = matchup.Entries.Count < 2;


            if (!isPlayed && !isSingle)
            {
                teamOneLabel.Visible = true;
                teamOneScoreLabel.Visible = true;
                teamOneScoreValue.Visible = true;
                teamTwoLabel.Visible = true;
                teamTwoScoreLabel.Visible = true;
                teamTwoScoreValue.Visible = true;
                versusLabel.Visible = true;
                scoreButton.Visible = true;
            }
            else if(isPlayed && isSingle)
            {
                teamOneLabel.Visible = true;
                teamOneScoreLabel.Visible = true;
                teamOneScoreValue.Visible = true;
                teamOneScoreValue.Text = "winner";
                teamTwoLabel.Visible = false;
                teamTwoScoreLabel.Visible = false;
                teamTwoScoreValue.Visible = false;
                versusLabel.Visible = false;
                scoreButton.Visible = false;
            }
            if (isPlayed && !isSingle)
            {
                teamOneLabel.Visible = true;
                teamOneScoreLabel.Visible = true;
                teamOneScoreValue.Visible = true;
                teamTwoLabel.Visible = true;
                teamTwoScoreLabel.Visible = true;
                teamTwoScoreValue.Visible = true;
                versusLabel.Visible = true;
                scoreButton.Visible = false;
            }

        }

    }
}
