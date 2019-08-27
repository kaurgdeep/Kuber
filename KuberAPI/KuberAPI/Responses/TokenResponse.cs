using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KuberAPI.Responses;

namespace KuberAPI.Responses
{
    public class TokenResponse : SuccessResponse
    {
        public string Token { get; set; }
    }
}
