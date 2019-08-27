﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberAPI.Dto
{

    public class RideRequestDto
    {
        public AddressDto PickupAddress { get; set; }
        public AddressDto DropoffAddress { get; set; }
        public AddressDto CurrentAddress { get; set; }
    }
   
}
