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
    public partial class CreateTournamentForm : Form, IPrizeRequestor , ITeamRequestor
    {

        List<TeamModel> StoredTeams = GlobalConfig.Connection.LoadTeams();
        List<TeamModel> SelectedTeams = new List<TeamModel>();
        List<PrizeModel> SelectedPrizes = new List<PrizeModel>();

        ITournamentRequestor callingform;

        public CreateTournamentForm(ITournamentRequestor caller)
        {
            InitializeComponent();
            WireUpLists();

            callingform = caller;
        }

        private void WireUpLists()
        {
            selectTeamComboBox.DataSource = null;
            selectTeamComboBox.DataSource = StoredTeams;
            selectTeamComboBox.DisplayMember = "TeamName";

            tournamentTeamsListBox.DataSource = null;
            tournamentTeamsListBox.DataSource = SelectedTeams;
            tournamentTeamsListBox.DisplayMember = "TeamName";

            prizesListBox.DataSource = null;
            prizesListBox.DataSource = SelectedPrizes;
            prizesListBox.DisplayMember = "PlaceName";
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel t = selectTeamComboBox.SelectedItem as TeamModel;
            if (t != null)
            {
                SelectedTeams.Add(t);
                StoredTeams.Remove(t);

                WireUpLists();
            }
        }

        private void deleteSelectedTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel t = tournamentTeamsListBox.SelectedItem as TeamModel;
            if (t != null)
            {
                SelectedTeams.Remove(t);
                StoredTeams.Add(t);

                WireUpLists();
            }
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            // wire up prize form
            CreatePrizeForm frm = new CreatePrizeForm(this);
            frm.Show();
        }

        public void PrizeComplete(PrizeModel model)
        {
            SelectedPrizes.Add(model);
            WireUpLists();
        }

        private void deleteSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            PrizeModel prize = prizesListBox.SelectedItem as PrizeModel;
            SelectedPrizes.Remove(prize);
            WireUpLists();

        }



        private void createNewTeamLabel_Click(object sender, EventArgs e)
        {
            CreateTeamForm frm = new CreateTeamForm(this);
            frm.Show();
        }

        public void TeamComplete(TeamModel model)
        {
            SelectedTeams.Add(model);
            WireUpLists();
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            decimal fee = 0;

            bool feeAcceptable = decimal.TryParse(entryFeeValue.Text,out fee);

            if(!feeAcceptable)
            {
                MessageBox.Show("you need to enter valid Entry fee",
                    "Invalid Fee",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            TournamentModel to = new TournamentModel();
            to.TournamentName = tournamentNameValue.Text;
            to.EntryFee = fee;
            to.EnteredTeams = SelectedTeams;
            to.Prizes = SelectedPrizes;

            // order our list randomly
            // check if it is big enough - if not add automatic wins
            // 2^n power
            //create our 

            TournamentLogic.CreateRounds(to);

            //create tournament entry
            //crate all of the prizes entries
            //create all of team entries



            GlobalConfig.Connection.CreateTournament(to);


            callingform.TournamentComplete(to);

            this.Close();

        }
    }
}
