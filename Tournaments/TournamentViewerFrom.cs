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
            //rounds = tournamentModel.Rounds;

            LoadFormData();
            LoadRounds();
            LoadMatchups();

            //matchupListBox.SelectedIndex = 0;
            //LoadScores();
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
            foreach (MatchupModel matchup in tournament.Rounds[round - 1])
            {
                if(matchup.Winner == null || !unplayedOnlyCheckBox.Checked)
                {
                    selectedMatchups.Add(matchup);
                }
                else
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
            }
            else
            {
                ClearMatchup();
            }
        }

        private void roundComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchups();
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

        private void unplayedOnlyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LoadMatchups();
        }
    }
}
