using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Documents
{
   public class LookUpCoordinatesResponse
    {
        public Coordinates coordinates { get; set; }
        public class Coordinates
        {
            public int pageNumber { get; set; }
            public double xCoordinate { get; set; }
            public double yCoordinate { get; set; }
        }
    }
}
