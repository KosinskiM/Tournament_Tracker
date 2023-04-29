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
    public partial class TournamentDashboardForm : Form, ITournamentRequestor
    {
        List<TournamentModel> tournaments = GlobalConfig.Connection.LoadTournaments();

        public TournamentDashboardForm()
        {
            InitializeComponent();
            WireUpLists();
            // load tournaments to dropbox ! 
        }


        private void WireUpLists()
        {
            loadExisitingTournamentComboBox.DataSource = null;
            loadExisitingTournamentComboBox.DataSource = tournaments;
            loadExisitingTournamentComboBox.DisplayMember = "TournamentName";
        }

        private void loadTournamentButton_Click(object sender, EventArgs e)
        {
            TournamentViewerFrom form = new TournamentViewerFrom((TournamentModel)loadExisitingTournamentComboBox.SelectedItem);
            form.Show();
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            CreateTournamentForm form = new CreateTournamentForm(this);
            form.Show();
        }

        public void TournamentComplete(TournamentModel model)
        {
            tournaments.Add(model);
            WireUpLists();
        }
    }
}
