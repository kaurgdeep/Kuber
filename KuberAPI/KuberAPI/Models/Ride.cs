using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


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

        public string CurrentAddress { get; set; }

        [Column(TypeName = "decimal(18,14)")]
        public decimal? CurrentLatitude { get; set; }

        [Column(TypeName = "decimal(18,14)")]
        public decimal? CurrentLongitude { get; set; }

        public RideStatus RideStatus { get; set; }

        public DateTime Requested { get; set; }
        public DateTime? Cancelled { get; set; }
        public DateTime? Accepted { get; set; }
        public DateTime? Rejected { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? DroppedOff { get; set; }
        public DateTime? PositionUpdated { get; set; }
    }
}
