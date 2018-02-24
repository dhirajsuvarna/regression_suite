using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing_cost
{
    class ResponseData
    {
        public string PartNumber { get; set; }
        public double TotalCostPiece { get; set; }
        public double MaterialCostPiece { get; set; }
        public double SetupCostPiece { get; set; }
        public double RunTimePiece { get; set; }
    }
}
