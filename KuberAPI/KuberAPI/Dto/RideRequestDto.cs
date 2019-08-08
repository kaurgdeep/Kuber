using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberAPI.Dto
{

    public class AddressDto
    {
        public string FormattedAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }

    public class RideRequestDto
    {
        public AddressDto PickupAddress { get; set; }
        public AddressDto DropoffAddress { get; set; }
    }
   
}
