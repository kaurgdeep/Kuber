using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberAPI
{
    public static class Constants
    {
        // Configuration keys
        public static string JWTSecurityKey = "SecurityKey";
        public static string DbConnection = "DbConnection";

        // Claims
        public static string EmailAddress = "EmailAddress";
        public static string UserId = "UserId";
        public static string UserType = "UserType";

        // UserTypes
        public static string Passenger = "Passenger";
        public static string Driver = "Driver";

    }
}
