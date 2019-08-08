using KuberAPI.Dto;
using KuberAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberAPI.Controllers
{

    [Authorize]
    //[ApiController]
    [Route("api/[controller]")]
    public class RidesController : KuberBaseController
    {
        private KuberContext Context;

        public RidesController(KuberContext context)
        {
            Context = context;
        }

        [HttpPost]
        public ActionResult Post([FromBody] RideRequestDto request)
        {
            if (base.UserId == null)
            {
                return BadRequest("Invalid user id");
            }

            // check usertype == 'passenger'
            if (base.UserType != Constants.Passenger)
            {
                return BadRequest("Invalid user type");
            }

            // Business rule
            // todo: put all the Context calls in a try/catch block like in UsersController/Post function
            var anyActiveRides = Context.Rides.Where(
                r => r.PassengerId == UserId && r.RideStatus == RideStatus.Requested || r.RideStatus == RideStatus.Accepted || r.RideStatus == RideStatus.PickedUp)
                .Count() > 0;
            if (anyActiveRides)
            {
                return BadRequest("User already has an active ride. Can't request another ride.");
            }

            var fromAddress = new Address
            {
                FormattedAddress = request.PickupAddress.FormattedAddress,
                Latitude = request.PickupAddress.Latitude,
                Longitude = request.PickupAddress.Longitude
            };
            Context.Addresses.Add(fromAddress);

            var toAddress = new Address
            {
                FormattedAddress = request.DropoffAddress.FormattedAddress,
                Latitude = request.DropoffAddress.Latitude,
                Longitude = request.DropoffAddress.Longitude
            };
            Context.Addresses.Add(toAddress);

            var ride = new Ride
            {
                PassengerId = UserId.Value,
                FromAddress = fromAddress,
                ToAddress = toAddress,
                Requested = DateTime.UtcNow,
                RideStatus = RideStatus.Requested
            };
            Context.Rides.Add(ride);
            Context.SaveChanges();

            return Ok(new
            {
                Data = new
                {
                    ride.RideId
                }
            });
        }

        [HttpPost]
        [Route("{rideId}/cancel")]
        public ActionResult CancelRide(int rideId)
        {
            if (base.UserId == null)
            {
                return BadRequest("Invalid user id");
            }
            // todo: check usertype == 'passenger'

            var ride = Context.Rides.Where(r => r.RideId == rideId && r.PassengerId == UserId && r.RideStatus == RideStatus.Requested).FirstOrDefault();
            if (ride == null)
            {
                // Log the error
                return BadRequest("There is no such ride for this passenger");
            }

            ride.RideStatus = RideStatus.Cancelled;
            ride.Cancelled = DateTime.UtcNow;

            Context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("{rideId}/accept")]
        public ActionResult AcceptRide(int rideId)
        {
            //if (base.UserId == null)
            //{
            //    return BadRequest("Invalid user id");
            //}

            //if USER is not driver fail the call
            if (base.UserType != Constants.Driver)
            {
                return BadRequest("Invalid user type");
            }

            var anyActiveRides = Context.Rides.Where(
                r => r.DriverId == UserId && (r.RideStatus == RideStatus.Accepted || r.RideStatus == RideStatus.PickedUp))
                .Count() > 0;
            if (anyActiveRides)
            {
                return BadRequest("Driver already has an active ride. Can't Accept another ride.");
            }

            var ride = Context.Rides.Where(r => r.RideId == rideId && r.RideStatus == RideStatus.Requested).FirstOrDefault();
            if (ride == null) // if ride doesn't exist
            {
                // Log the error
                return BadRequest("There is no such requested ride for Driver");
            }
            
            var driver = Context.Users.Where(u => u.UserId == UserId).FirstOrDefault();
            if(driver == null)
            {
                return BadRequest("Driver not found");
            }
            ride.Driver = driver;
            ride.RideStatus = RideStatus.Accepted;
            ride.Accepted = DateTime.UtcNow;


            Context.SaveChanges();

            return Ok(new
            {
                Data = new
                {
                    ride.RideId
                }
            });
        }


        [HttpPost]
        [Route("{rideId}/reject")]
        public ActionResult RejectRide(int rideId)
        {

            //if USER is not driver fail the call
            if (base.UserType != Constants.Driver)
            {
                return BadRequest("User type is not Driver");
            }

            var ride = Context.Rides.Where(r => r.RideId == rideId && r.DriverId == UserId && r.RideStatus == RideStatus.Accepted).FirstOrDefault();
            if (ride == null) // if ride doesn't exist
            {
                // Log the error
                return BadRequest("There is no such ride for this Driver");
            }
            
            ride.RideStatus = RideStatus.Rejected;
            ride.Rejected = DateTime.UtcNow;

            Context.SaveChanges();

            return Ok(new
            {
                Data = new
                {
                    ride.RideId
                }
            });
        }

        

        [HttpPost]
        [Route("{rideId}/pickup")]
        public ActionResult PickUp(int rideId)
        {

            //if USER is not driver fail the call
            if (base.UserType != Constants.Driver)
            {
                return BadRequest("User type is not Driver");
            }

            var ride = Context.Rides.Where(r => r.RideId == rideId && r.DriverId == UserId && r.RideStatus == RideStatus.Accepted).FirstOrDefault();
            if (ride == null) // if ride doesn't exist
            {
                // Log the error
                return BadRequest("There is no such ride for this Driver");
            }

            ride.RideStatus = RideStatus.PickedUp;
            ride.PickedUp = DateTime.UtcNow;

            Context.SaveChanges();

            return Ok(new
            {
                Data = new
                {
                    ride.RideId
                }
            });
        }

        [HttpPost]
        [Route("{rideId}/dropoff")]
        public ActionResult DropOff(int rideId)
        {

            //if USER is not driver fail the call
            if (base.UserType != Constants.Driver)
            {
                return BadRequest("User type is not Driver");
            }

            var ride = Context.Rides.Where(r => r.RideId == rideId && r.DriverId == UserId && r.RideStatus == RideStatus.PickedUp).FirstOrDefault();
            if (ride == null) // if ride doesn't exist
            {
                // Log the error
                return BadRequest("There is no such ride for this Driver");
            }

            ride.RideStatus = RideStatus.DroppedOff;
            ride.DroppedOff = DateTime.UtcNow;

            Context.SaveChanges();

            return Ok(new
            {
                Data = new
                {
                    ride.RideId
                }
            });
        }

    }
}
