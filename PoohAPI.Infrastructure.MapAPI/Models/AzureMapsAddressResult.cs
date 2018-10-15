using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Infrastructure.MapAPI.Models
{
    public class AzureMapsAddressResult
    {
        public List<AddressResult> results { get; set; }
    }

    public class AddressResult
    {
        public Position position { get; set; }
    }

    public class Position
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }
}
