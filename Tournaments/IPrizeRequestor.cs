using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackersLibrary.Models;

namespace Tournaments
{
    public interface IPrizeRequestor
    {
        void PrizeComplete(PrizeModel model);
    }
}
