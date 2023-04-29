using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackersLibrary.Models;

namespace Tournaments
{
    public interface ITournamentRequestor
    {
        void TournamentComplete(TournamentModel model);
    }
}
