using KuberAPI.Models;
using KuberAPI.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberAPI.Responses
{
    public class MeResponse : SuccessResponse
    {
        public int? UserId { get; set; }
        public string EmailAddress { get; set; }
        public string UserType { get; set; }
    }
}
