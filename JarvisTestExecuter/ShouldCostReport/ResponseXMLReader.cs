using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace testing_cost
{
    class ResponseXMLReader
    {
        public ResponseData Data { get; private set; }
 
        public ResponseXMLReader(ResponseData data)
        {
            Data = data;
        }

        public void Read(string iResponseFile)
        {
            try
            {
                XElement costimator = XElement.Load(iResponseFile);

                //We need to change this in case the response xml file has multiple parts.
                var partNumberQuery = from parts in costimator.Descendants("Parts")
                                      select parts.Element("Part").Attribute("PartNumber").Value;

                foreach (var partNumber in partNumberQuery)
                    Data.PartNumber = partNumber;

                var totalcostpieceQuery = from part in costimator.Descendants("Part")
                                          select (double)part.Element("TotalCostPiece");

                foreach (var totalCostPiece in totalcostpieceQuery)
                    Data.TotalCostPiece = totalCostPiece;

                var materiacostpieceQuery = from part in costimator.Descendants("Part")
                                            select (double)part.Element("MaterialCostPiece");

                foreach (var materialCostPiece in materiacostpieceQuery)
                    Data.MaterialCostPiece = materialCostPiece;

                var setupcostpieceQuery = from part in costimator.Descendants("Part")
                                          select (double)part.Element("SetupCostPiece");

                foreach (var setupCostPiece in setupcostpieceQuery)
                    Data.SetupCostPiece = setupCostPiece;

                var mfgTimequery = from operation in costimator.Descendants("Operation")
                            select operation.Element("ManufacturingTimePiece");

                double totalMfgTimePiece = 0.0;
                foreach (var x in mfgTimequery)
                {
                    if (x != null)
                        totalMfgTimePiece += Double.Parse(x.Value);

                }

                var stdTimequery = from operation in costimator.Descendants("Operation")
                                   select operation.Element("StandardTimePiece");

                double totalStdTimePiece = 0.0;
                foreach (var x in stdTimequery)
                {
                    if (x != null)
                        totalStdTimePiece += Double.Parse(x.Value);

                }
                var allowTimequery = from operation in costimator.Descendants("Operation")
                                   select operation.Element("AllowanceTimePiece");

                double totalAllowanceTimePiece = 0.0;
                foreach (var x in allowTimequery)
                {
                    if (x != null)
                        totalAllowanceTimePiece += Double.Parse(x.Value);

                }
                var toolChangeTimequery = from operation in costimator.Descendants("Operation")
                                   select operation.Element("ToolChangeTimePiece");

                double totalToolChangeTimePiece = 0.0;
                foreach (var x in toolChangeTimequery)
                {
                    if (x != null)
                        totalToolChangeTimePiece += Double.Parse(x.Value);

                }

                Data.RunTimePiece = (totalMfgTimePiece + totalStdTimePiece + totalAllowanceTimePiece + totalToolChangeTimePiece) / 60.0;
            }
            catch (Exception ex)
            { 
                //loggins is pending.
            }
        
        }
    }
}
