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
    public partial class CreateTeamForm : Form
    {
        ITeamRequestor callingFrom;

        List<ParticipantModel> availableParticipants = GlobalConfig.Connection.LoadParticipants();
        List<ParticipantModel> selectedParticipants = new List<ParticipantModel>();



        public CreateTeamForm(ITeamRequestor caller)
        {
            InitializeComponent();
            callingFrom = caller;
            WireUpLists();
        }

        public void WireUpLists()
        {
            selectTeamMemberComboBox.DataSource = null;
            selectTeamMemberComboBox.DataSource = availableParticipants;
            selectTeamMemberComboBox.DisplayMember = "FullName";

            teamMembersListBox.DataSource = null;
            teamMembersListBox.DataSource = selectedParticipants;
            teamMembersListBox.DisplayMember = "FullName";
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if(Validate_Participant())
            {
                ParticipantModel person = new ParticipantModel()
                {
                    FirstName = firstNameValue.Text,
                    LastName = lastNameValue.Text,
                    EmailAdress = emailValue.Text,
                    CellPhoneNumber = cellphoneValue.Text
                };

                GlobalConfig.Connection.CreateParticipant(person);
                selectedParticipants.Add(person);
                WireUpLists();

                firstNameValue.Text = "";
                lastNameValue.Text = "";
                emailValue.Text = "";
                cellphoneValue.Text = "";
            } 
            else
            {
                MessageBox.Show("Provided information are not sufficient to create TeamMember");
            }
        }

        private void addMemberButton_Click(object sender, EventArgs e)
        {
            if (selectTeamMemberComboBox.SelectedItem != null)
            {
                ParticipantModel p = selectTeamMemberComboBox.SelectedItem as ParticipantModel;
                availableParticipants.Remove(p);
                selectedParticipants.Add(p);
                WireUpLists();
            }
            else MessageBox.Show("selected member is invalid, try again");
        }

        private void removeSelectedMemberButton_Click(object sender, EventArgs e)
        {
            if (teamMembersListBox.SelectedItem != null)
            {
                ParticipantModel p = teamMembersListBox.SelectedItem as ParticipantModel;
                availableParticipants.Add(p);
                selectedParticipants.Remove(p);
                WireUpLists();
            }
            else MessageBox.Show("Selected member is invalid, try again");
        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            if (teamNameValue == null) return;
            if (teamMembersListBox == null) return;

            TeamModel team = new TeamModel() { 
                TeamName = teamNameValue.Text, 
                TeamMembers = teamMembersListBox.DataSource as List<ParticipantModel>
            };

            GlobalConfig.Connection.CreateTeam(team);
            callingFrom.TeamComplete(team);
            this.Close();
        }

        public void ClearForm()
        {
            teamNameValue.Text = "";
            teamMembersListBox.DataSource = null;
        }


        //form validation methods
        public bool Validate_Participant()
        {
            bool output = true;

            if (firstNameValue.Text.Length == 0) output = false;

            if (lastNameValue.Text.Length == 0) output = false;

            if (emailValue.Text.Length == 0) output = false;

            if (cellphoneValue.Text.Length == 0) output = false;

            return output;
        }
    }
}
