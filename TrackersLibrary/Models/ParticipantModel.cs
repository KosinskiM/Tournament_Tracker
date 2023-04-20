using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackersLibrary.Models
{
    public class ParticipantModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAdress { get; set; }
        public string CellPhoneNumber { get; set; }

        public string FullName 
        {
            get
            {
                return FirstName + " " + LastName; 
            }
        }
    }
}
