using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace KuberAPI.Controllers
{
    [ApiController]
    public abstract class KuberBaseController : ControllerBase
    {
        // backing variable.
        private int? userId;
        protected int? LoggedInUserId
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
        protected string LoggedInEmailAddress
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
        protected string LoggedInUserType
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