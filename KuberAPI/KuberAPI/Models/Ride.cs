using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KuberAPI.Models
{
    public enum RideStatus
    {
        Requested,
        Cancelled,

        Accepted,
        Rejected,
        PickedUp,
        DroppedOff
    }

    public class Ride
    {
        public int RideId { get; set; }

        [Required]
        public int PassengerId { get; set; }
        public User Passenger { get; set; }

        public int? DriverId { get; set; }
        public User Driver { get; set; }

        [Required]
        public int FromAddressId { get; set; }
        public Address FromAddress { get; set; }

        [Required]
        public int ToAddressId { get; set; }
        public Address ToAddress { get; set; }

        public RideStatus RideStatus { get; set; }

        public DateTime Requested { get; set; }
        public DateTime? Cancelled { get; set; }
        public DateTime? Accepted { get; set; }
        public DateTime? Rejected { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? DroppedOff { get; set; }
    }
}
