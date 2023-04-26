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
    public partial class TournamentDashboardForm : Form
    {
        List<TournamentModel> tournaments = GlobalConfig.Connection.LoadTournaments(); 

        public TournamentDashboardForm()
        {
            InitializeComponent();
        }


    }
}
