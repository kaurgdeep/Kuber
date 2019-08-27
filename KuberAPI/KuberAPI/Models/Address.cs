using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KuberAPI.Models
{
    public class Address
    {
        public int AddressId { get; set; }

        public string FormattedAddress { get; set; }

        [Column(TypeName = "decimal(18,14)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(18,14)")]
        public decimal Longitude { get; set; }

        //public int AddressId { get; set; }
        //public string FormattedAddress { get; set; }
        //public int Lat { get; set; }
        //public int Lng  { get; set; }
        //public int ZipCode { get; set; }
        //public string City { get; set; }
        //public string State { get; set; }

    }
}
