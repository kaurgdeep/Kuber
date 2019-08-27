using KuberAPI.Dto;
using KuberAPI.Models;
using KuberAPI.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberAPI.Responses
{
    public class RideResponse : SuccessResponse
    {
        public int Id { get; set; }
        public int PassengerId { get; set; }
        public int? DriverId { get; set; }
        public AddressDto FromAddress { get; set; }
        public AddressDto ToAddress { get; set; }
        public AddressDto CurrentAddress { get; set; }
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
