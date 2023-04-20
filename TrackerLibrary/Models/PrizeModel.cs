using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class PrizeModel
    {
        /// <summary>
        /// The unique identifier for the prize
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The numeric identifier for the place
        /// </summary>
        public int PlaceNumber { get; set; }

        /// <summary>
        /// The name for the place
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// the fixed amount this place earns or zero if not used
        /// </summary>
        public decimal PrizeAmount { get; set; }

        /// <summary>
        //the number that represents the percentage of the overall
        //take or 0 if not used. the percentage is a fraction of 1 (0.5 - 50%) 
        /// </summary>
        public double PrizePercentage { get; set; }

        public PrizeModel()
        {

        }

        public PrizeModel(string placeName, string placeNumber, string prizeAmount, string prizePercentage)
        {
            this.PlaceName = placeName;

            int placeNumberValue;
            int.TryParse(placeNumber, out placeNumberValue);
            this.PlaceNumber = placeNumberValue;

            decimal prizeAmountValue = 0;
            decimal.TryParse(prizeAmount, out prizeAmountValue);
            this.PrizeAmount = prizeAmountValue;    

            double prizePrecentageValue = 0;
            double.TryParse(prizePercentage, out prizePrecentageValue);
            this.PrizePercentage = prizePrecentageValue;
        }

    }
}
