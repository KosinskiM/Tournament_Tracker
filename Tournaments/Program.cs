using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tournaments;
using TrackersLibrary;

namespace TrackerUI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            // Initialize the database connection
            GlobalConfig.InitializeConnections(DatabaseType.Textfile);


            //Application.Run(new CreatePrizeForm());

            //Application.Run(new CreateTournamentForm());

            Application.Run(new TournamentDashboardForm());

            //main form for the future
            //Application.Run(new TournamentDashboardForm());
        }
    }
}
