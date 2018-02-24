using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing_cost
{
    class IMResponseData
    {
        public string PartNumber { get; set; }

        public double TotalCost { get; set; }

        public double SetupCost { get; set; }

        public double MaterialCost { get; set; }

        public double ToolingCost { get; set; }

        public double CoolingTime { get; set; }
    }
}
