using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KuberAPI.Dto;
using KuberAPI.Interfaces.Services;
using KuberAPI.Models;
using KuberAPI.Responses;
using KuberAPI;
using KuberAPI.Controllers;


namespace KuberAPI.Controllers
{
    [Authorize]
    //[ApiController]
    [Route("api/[controller]")]
    [EnableCors(Constants.KuberServerCorsPolicy)]
    public class RidesController : KuberBaseController
    {
        private IEntityService<User> UserService;
        private IEntityService<Ride> RideService;
        private IEntityService<Address> AddressService;

        public RidesController(IEntityService<User> userService, IEntityService<Ride> rideService, IEntityService<Address> addressService)
        {
            UserService = userService;
            RideService = rideService;
            AddressService = addressService;
        }

        [HttpPost]
        [Route("")]
        public ActionResult Create([FromBody] RideRequestDto request)
        {
            if (LoggedInUserId == null)
            {
                return BadRequest("Invalid user id");
            }

            // check usertype == 'passenger'
            if (LoggedInUserType != Constants.Passenger)
            {
                return BadRequest("Invalid user type");
            }

            // todo: put all the Context calls in a try/catch block like in UsersController/Post function

            // Business rule
            var anyActiveRides = RideService.Count(r =>
                r.PassengerId == LoggedInUserId &&
                (r.RideStatus == RideStatus.Requested || r.RideStatus == RideStatus.Accepted || r.RideStatus == RideStatus.PickedUp)) > 0;
            if (anyActiveRides)
            {
                return BadRequest("User already has an active ride. Can't request another ride.");
            }

            var fromAddress = new Address
            {
                FormattedAddress = request.PickupAddress.FormattedAddress,
                Latitude = request.PickupAddress.Latitude.Value,
                Longitude = request.PickupAddress.Longitude.Value
            };
            AddressService.Create(fromAddress);

            var toAddress = new Address
            {
                FormattedAddress = request.DropoffAddress.FormattedAddress,
                Latitude = request.DropoffAddress.Latitude.Value,
                Longitude = request.DropoffAddress.Longitude.Value
            };
            AddressService.Create(toAddress);

            var ride = new Ride
            {
                PassengerId = LoggedInUserId.Value,
                FromAddress = fromAddress,
                ToAddress = toAddress,
                Requested = DateTime.UtcNow,
                RideStatus = RideStatus.Requested
            };
            var rideId = RideService.Create(ride);

            return Ok(new CreateResponse { Id = rideId });
        }

        [HttpGet]
        public ActionResult GetAnyActive()
        {
            if (LoggedInUserId == null)
            {
                return BadRequest("Invalid user id");
            }

            var ride = RideService.Get(r =>
                    (r.PassengerId == LoggedInUserId || r.DriverId == LoggedInUserId) &&
                    (r.RideStatus == RideStatus.Requested || r.RideStatus == RideStatus.Accepted || r.RideStatus == RideStatus.PickedUp)
                );
            if (ride == null)
            {
                return NoContent();
            }

            var rideResponse = new RideResponse
            {
                Id = ride.RideId,
                PassengerId = ride.PassengerId,
                DriverId = ride.DriverId,
                RideStatus = ride.RideStatus,
                FromAddress = new AddressDto
                {
                    FormattedAddress = ride.FromAddress.FormattedAddress,
                    Latitude = ride.FromAddress.Latitude,
                    Longitude = ride.FromAddress.Longitude
                },
                ToAddress = new AddressDto
                {
                    FormattedAddress = ride.ToAddress.FormattedAddress,
                    Latitude = ride.ToAddress.Latitude,
                    Longitude = ride.ToAddress.Longitude
                },
                CurrentAddress = new AddressDto
                {
                    FormattedAddress = ride.CurrentAddress
                },
                Requested = ride.Requested,
                Cancelled = ride.Cancelled,
                Accepted = ride.Accepted,
                Rejected = ride.Rejected,
                PositionUpdated = ride.PositionUpdated,
                PickedUp = ride.PickedUp,
                DroppedOff = ride.DroppedOff
            };

            if (ride.CurrentLatitude != null)
            {
                if (ride.CurrentLatitude.HasValue)
                {
                    rideResponse.CurrentAddress.Latitude = ride.CurrentLatitude.Value;
                }
                if (ride.CurrentLongitude.HasValue)
                {
                    rideResponse.CurrentAddress.Longitude = ride.CurrentLongitude.Value;
                }
            }

            return Ok(rideResponse);
        }

        [HttpGet]
        [Route("near-by")]
        public ActionResult GetNearByRides([FromQuery] decimal latitude, [FromQuery] decimal longitude, [FromQuery] decimal radiusMeters)
        {
            if (LoggedInUserId == null)
            {
                return BadRequest("Invalid user id");
            }

            if (LoggedInUserType != Constants.Driver)
            {
                return BadRequest($"Invalid user type. Expected Driver, got {LoggedInUserType}");
            }

            //DbGeography searchLocation = DbGeography.FromText(String.Format("POINT({0} {1})", longitude, latitude));
            if (radiusMeters == 0)
            {
                radiusMeters = 804.672M; // 1/2 mile in 
            }

            var allRequestedRides = RideService.GetMany(r =>
                (r.RideStatus == RideStatus.Requested)) // and requested in the last 15 minutes (?) and near by
                .Take(25) // take only top 25 rides for now
                .ToList();

            var rides = allRequestedRides.Where(ride => DistanceBetweenPlaces((double)latitude, (double)longitude, (double)ride.FromAddress.Latitude, (double)ride.FromAddress.Longitude) <= (double)radiusMeters)
                .Select(ride => new RideResponse
                {
                    Id = ride.RideId,
                    PassengerId = ride.PassengerId,
                    DriverId = ride.DriverId,
                    RideStatus = ride.RideStatus,
                    FromAddress = new AddressDto
                    {
                        FormattedAddress = ride.FromAddress.FormattedAddress,
                        Latitude = ride.FromAddress.Latitude,
                        Longitude = ride.FromAddress.Longitude
                    },
                    ToAddress = new AddressDto
                    {
                        FormattedAddress = ride.ToAddress.FormattedAddress,
                        Latitude = ride.ToAddress.Latitude,
                        Longitude = ride.ToAddress.Longitude
                    },
                    CurrentAddress = null,
                    Requested = ride.Requested,
                    Cancelled = ride.Cancelled,
                    Accepted = ride.Accepted,
                    Rejected = ride.Rejected,
                    PositionUpdated = ride.PositionUpdated,
                    PickedUp = ride.PickedUp,
                    DroppedOff = ride.DroppedOff
                });

            if (rides == null || rides.Count() == 0)
            {
                return NoContent();
            }

            return Ok(rides);
        }

        // from: https://stackoverflow.com/questions/27928/calculate-distance-between-two-latitude-longitude-points-haversine-formula
        const double RADIUS = 6378.16;

        private static double Radians(double x)
        {
            return x * Math.PI / 180;
        }

        // in meters
        private static double DistanceBetweenPlaces(double lon1, double lat1, double lon2, double lat2)
        {
            double dlon = Radians(lon2 - lon1);
            double dlat = Radians(lat2 - lat1);

            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(lat1)) * Math.Cos(Radians(lat2)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return angle * RADIUS * 1000;
        }

        [HttpGet]
        [Route("{rideId}")]
        public ActionResult Get(int rideId)
        {
            if (LoggedInUserId == null)
            {
                return BadRequest("Invalid user id");
            }

            var ride = RideService.Get(r =>
                r.RideId == rideId &&
                (r.DriverId == LoggedInUserId || r.PassengerId == LoggedInUserId));
            if (ride == null)
            {
                // Log the error
                return BadRequest("There is no such ride for this user");
            }

            var rideResponse = new RideResponse
            {
                Id = rideId,
                PassengerId = ride.PassengerId,
                DriverId = ride.DriverId,
                RideStatus = ride.RideStatus,
                FromAddress = new AddressDto
                {
                    FormattedAddress = ride.FromAddress.FormattedAddress,
                    Latitude = ride.FromAddress.Latitude,
                    Longitude = ride.FromAddress.Longitude
                },
                ToAddress = new AddressDto
                {
                    FormattedAddress = ride.ToAddress.FormattedAddress,
                    Latitude = ride.ToAddress.Latitude,
                    Longitude = ride.ToAddress.Longitude
                },
                CurrentAddress = new AddressDto
                {
                    FormattedAddress = ride.CurrentAddress
                },
                Requested = ride.Requested,
                Cancelled = ride.Cancelled,
                Accepted = ride.Accepted,
                Rejected = ride.Rejected,
                PositionUpdated = ride.PositionUpdated,
                PickedUp = ride.PickedUp,
                DroppedOff = ride.DroppedOff
            };

            if (ride.CurrentLatitude != null)
            {
                if (ride.CurrentLatitude.HasValue)
                {
                    rideResponse.CurrentAddress.Latitude = ride.CurrentLatitude.Value;
                }
                if (ride.CurrentLongitude.HasValue)
                {
                    rideResponse.CurrentAddress.Longitude = ride.CurrentLongitude.Value;
                }
            }

            return Ok(rideResponse);
        }

        [HttpPost]
        [Route("{rideId}/update-location")]
        public ActionResult UpdateRide(int rideId, [FromBody] RideStatusDto rideStatus)
        {
            if (LoggedInUserId == null)
            {
                return BadRequest("Invalid user id");
            }
            if (LoggedInUserType != Constants.Driver)
            {
                return BadRequest($"Invalid user type. Expected Driver, got {LoggedInUserType}");
            }

            var ride = RideService.Get(r =>
                r.RideId == rideId &&
                r.DriverId == LoggedInUserId &&
                (r.RideStatus == RideStatus.Accepted || r.RideStatus == RideStatus.PickedUp));
            if (ride == null)
            {
                // Log the error
                return BadRequest("There is no such accepted or pickedup ride for this driver");
            }

            RideService.Update(() =>
            {
                ride.CurrentAddress = rideStatus?.CurrentAddress?.FormattedAddress;
                ride.CurrentLatitude = rideStatus?.CurrentAddress?.Latitude;
                ride.CurrentLongitude = rideStatus?.CurrentAddress?.Longitude;
                ride.PositionUpdated = DateTime.UtcNow;
            }, ride);

            return Ok(new RideResponse
            {
                Id = rideId,
                PassengerId = ride.PassengerId,
                DriverId = ride.DriverId,
                RideStatus = ride.RideStatus,
                FromAddress = new AddressDto
                {
                    FormattedAddress = ride.FromAddress.FormattedAddress,
                    Latitude = ride.FromAddress.Latitude,
                    Longitude = ride.FromAddress.Longitude
                },
                ToAddress = new AddressDto
                {
                    FormattedAddress = ride.ToAddress.FormattedAddress,
                    Latitude = ride.ToAddress.Latitude,
                    Longitude = ride.ToAddress.Longitude
                },
                CurrentAddress = new AddressDto
                {
                    FormattedAddress = ride.CurrentAddress,
                    Latitude = ride.CurrentLatitude.Value,
                    Longitude = ride.CurrentLongitude.Value
                },
                Requested = ride.Requested,
                Cancelled = ride.Cancelled,
                Accepted = ride.Accepted,
                Rejected = ride.Rejected,
                PositionUpdated = ride.PositionUpdated,
                PickedUp = ride.PickedUp,
                DroppedOff = ride.DroppedOff
            });
        }

        [HttpPost]
        [Route("{rideId}/cancel")]
        public ActionResult CancelRide(int rideId)
        {
            if (LoggedInUserId == null)
            {
                return BadRequest("Invalid user id");
            }
            // check usertype == 'passenger'
            if (LoggedInUserType != Constants.Passenger)
            {
                return BadRequest("Invalid user type");
            }

            var ride = RideService.Get(r =>
                r.RideId == rideId &&
                r.PassengerId == LoggedInUserId &&
                r.RideStatus == RideStatus.Requested);
            if (ride == null)
            {
                // Log the error
                return BadRequest("There is no such ride for this passenger");
            }


            RideService.Update(() =>
            {
                ride.RideStatus = RideStatus.Cancelled;
                ride.Cancelled = DateTime.UtcNow;
            }, ride);

            return Ok();
        }

        [HttpPost]
        [Route("{rideId}/accept")]
        public ActionResult AcceptRide(int rideId)
        {
            if (LoggedInUserId == null)
            {
                return BadRequest("Invalid user id");
            }
            // check usertype == 'passenger'
            if (LoggedInUserType != Constants.Driver)
            {
                return BadRequest($"Invalid user type {LoggedInUserType}");
            }

            var driver = UserService.Get(u => u.UserId == LoggedInUserId);
            if (driver == null)
            {
                return BadRequest("No user found");
            }

            var ride = RideService.Get(r => r.RideId == rideId && r.RideStatus == RideStatus.Requested);
            if (ride == null)
            {
                // Log the error
                return BadRequest("There is no such requested ride");
            }

            var anyAcceptedRides = RideService.Count(r =>
                r.DriverId == LoggedInUserId && (r.RideStatus == RideStatus.Accepted || r.RideStatus == RideStatus.PickedUp)) > 0;
            if (anyAcceptedRides)
            {
                // Log the error
                return BadRequest("Driver can accept only one Ride");
            }

            RideService.Update(() =>
            {
                ride.Driver = driver;
                ride.RideStatus = RideStatus.Accepted;
                ride.Accepted = DateTime.UtcNow;
            }, ride);

            return Ok();
        }

        [HttpPost]
        [Route("{rideId}/reject")]
        public ActionResult RejectRide(int rideId)
        {
            if (LoggedInUserId == null)
            {
                return BadRequest("Invalid user id");
            }
            // check usertype == 'passenger'
            if (LoggedInUserType != Constants.Driver)
            {
                return BadRequest("Invalid user type");
            }

            var ride = RideService.Get(r => r.RideId == rideId && r.DriverId == LoggedInUserId && r.RideStatus == RideStatus.Accepted);
            if (ride == null)
            {
                // Log the error
                return BadRequest("There is no such ride accepted by the user");
            }

            RideService.Update(() =>
            {
                ride.RideStatus = RideStatus.Rejected;
                ride.Rejected = DateTime.UtcNow;
            }, ride);

            return Ok();
        }

        [HttpPost]
        [Route("{rideId}/pick-up")]
        public ActionResult PickupRide(int rideId)
        {
            if (LoggedInUserId == null)
            {
                return BadRequest("Invalid user id");
            }
            // check usertype == 'passenger'
            if (LoggedInUserType != Constants.Driver)
            {
                return BadRequest("Invalid user type");
            }

            var ride = RideService.Get(r => r.RideId == rideId && r.DriverId == LoggedInUserId && r.RideStatus == RideStatus.Accepted);
            if (ride == null)
            {
                // Log the error
                return BadRequest("There is no such ride accepted by the user");
            }

            RideService.Update(() =>
            {
                ride.RideStatus = RideStatus.PickedUp;
                ride.PickedUp = DateTime.UtcNow;
            }, ride);

            return Ok();
        }

        [HttpPost]
        [Route("{rideId}/drop-off")]
        public ActionResult DropOffRide(int rideId)
        {
            if (LoggedInUserId == null)
            {
                return BadRequest("Invalid user id");
            }
            // check usertype == 'passenger'
            if (LoggedInUserType != Constants.Driver)
            {
                return BadRequest("Invalid user type");
            }

            var ride = RideService.Get(r => r.RideId == rideId && r.DriverId == LoggedInUserId && r.RideStatus == RideStatus.PickedUp);
            if (ride == null)
            {
                // Log the error
                return BadRequest("There is no such ride accepted by the user");
            }

            RideService.Update(() =>
            {
                ride.RideStatus = RideStatus.DroppedOff;
                ride.DroppedOff = DateTime.UtcNow;
            }, ride);

            return Ok();
        }
    }
}