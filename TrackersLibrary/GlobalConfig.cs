using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackersLibrary;
using TrackersLibrary.DataAccess;
using System.Configuration;

namespace TrackersLibrary
{
    public static class GlobalConfig
    {

        public const string PrizesFile = "PrizesFile.csv";
        public const string ParticipantsFile = "ParticipantsFile.csv";
        public const string TeamsFile = "TeamsFile.csv";
        public const string TournamentsFile = "TournamentsFile.csv";
        public const string MatchupFile = "MatchupModels.csv";
        public const string MatchupEntryFile = "MatchupEntryModels.csv";


        public static IDataConnection Connection { get; private set; }

        public static void InitializeConnections(DatabaseType db)
        {
            if (db == DatabaseType.Sql)
            {
                SqlConnector sql = new SqlConnector();
                Connection = sql;
            }
            else if (db == DatabaseType.Textfile)
            {
                TextConnector text  = new TextConnector();
                Connection = text;
            }
        }

        //connection value string
        public static string CnnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public static string AppKeyLookup(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

    }
}
