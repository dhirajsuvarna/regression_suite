using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace testing_cost
{
    class IMResponseReader
    {
        public IMResponseData Data { get; private set; }

        public IMResponseReader(IMResponseData data)
        {
            Data = data;
        }

        public void Read(string iResponseFile)
        {
            try
            {
                XElement hcl = XElement.Load(iResponseFile);

                var partNumberQuery = from parts in hcl.Descendants("Parts")
                                      select parts.Element("Part").Attribute("PartNumber").Value;

                foreach (var partNumber in partNumberQuery)
                    Data.PartNumber = partNumber;

                var totalCostQuery = from costDetail in hcl.Descendants("CostDetails")
                                     select (double)costDetail.Element("TotalCost");

                foreach (var totalCost in totalCostQuery)
                    Data.TotalCost = totalCost;

                var setupCostQuery = from costDetail in hcl.Descendants("CostDetails")
                                     select (double)costDetail.Element("SetupCost");

                foreach (var setupCost in setupCostQuery)
                    Data.SetupCost = setupCost;

                var materialCostQuery = from costDetail in hcl.Descendants("CostDetails")
                                        select (double)costDetail.Element("MaterialCost");

                foreach (var materialCost in materialCostQuery)
                    Data.MaterialCost = materialCost;

                var toolingCostQuery = from costDetail in hcl.Descendants("CostDetails")
                                       select (double)costDetail.Element("TooingCost");

                foreach (var toolingCost in toolingCostQuery)
                    Data.ToolingCost = toolingCost;

                var coolingTimeQuery = from processDetail in hcl.Descendants("ProcessDetails")
                                       select (double)processDetail.Element("CoolingTime");

                foreach (var coolingTime in coolingTimeQuery)
                    Data.CoolingTime = coolingTime;
            }
            catch (Exception ex)
            { 
            
            }
        
        }
    }
}
