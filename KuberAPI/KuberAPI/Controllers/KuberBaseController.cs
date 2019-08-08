using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberAPI.Controllers
{
    [ApiController]
    public abstract class KuberBaseController : ControllerBase
    {

        // backing variable.
        private int? userId;
        protected int? UserId
        {
            get
            {
                if (userId != null)
                {
                    return userId;
                }

                // userId == null
                var sid = GetClaim(Constants.UserId);
                if (sid == null)
                {
                    return null;
                }
                if (int.TryParse(sid, out int id))
                {
                    userId = id;
                }

                return userId;
            }
        }

        private string emailAddress;
        protected string EmailAddress
        {
            get
            {
                if (emailAddress != null)
                {
                    return emailAddress;
                }

                var emailAddressFromClaims = GetClaim(Constants.EmailAddress);
                if (emailAddressFromClaims == null)
                {
                    return null;
                }
                emailAddress = emailAddressFromClaims;

                return emailAddress;
            }
        }

        private string userType;
        protected string UserType
        {
            get
            {
                if (userType != null)
                {
                    return userType;
                }

                var userTypeFromClaims = GetClaim(Constants.UserType);
                if (userTypeFromClaims == null)
                {
                    return null;
                }
                userType = userTypeFromClaims;

                return userType;
            }
        }

        private string GetClaim(string claimType)
        {
            return User?.Claims?.Where(c => c.Type == claimType)?.FirstOrDefault()?.Value;
        }

    }
}

