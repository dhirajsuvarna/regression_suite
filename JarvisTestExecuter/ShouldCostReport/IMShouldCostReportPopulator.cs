using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;


namespace testing_cost
{
    class IMShouldCostReportPopulator
    {
        public IMResponseData Data { get; private set; }

        //should be at some common location ::refactor::
        public const int PART_NUMBER_INDEX = 3;
        public const int TOTAL_COST_INDEX = 9;
        public const int SETUP_COST_INDEX = 13;
        public const int MATIERAL_COST_INDEX = 19;
        public const int TOOLING_COST_INDEX = 27;
        public const int COOLING_TIME_INDEX = 30;

        public IMShouldCostReportPopulator(IMResponseData iData)
        {
            Data = iData;
        }

        public void Populate(string iExcelFile)
        {
            Excel.Application xlApp = null;
            Excel.Workbook xlWorkbook = null;
            Excel.Worksheet xlWorksheet = null;
            try
            {
                xlApp = new Excel.Application();
                xlWorkbook = xlApp.Workbooks.Open(iExcelFile);
                xlWorksheet = (Excel.Worksheet)xlWorkbook.Worksheets["MainSheet"];

                //iterate columns
                Excel.Range partNumberColumn = xlWorksheet.UsedRange.Columns[PART_NUMBER_INDEX];

                //the only constrain here is that the part name should be unique+
                Excel.Range partCell = partNumberColumn.Find(Data.PartNumber);
                if (partCell != null)
                {
                    Excel.Range thisRow = xlWorksheet.UsedRange.Rows[partCell.Row];

                    //::refactor:: is ToString() Needed?
                    thisRow.Cells[MATIERAL_COST_INDEX].Value = Data.MaterialCost.ToString();
                    thisRow.Cells[TOTAL_COST_INDEX].Value = Data.TotalCost.ToString();
                    thisRow.Cells[SETUP_COST_INDEX].Value = Data.SetupCost.ToString();
                    thisRow.Cells[TOOLING_COST_INDEX].Value = Data.ToolingCost.ToString();
                    thisRow.Cells[COOLING_TIME_INDEX].Value = Data.CoolingTime.ToString();

                    xlWorkbook.Save();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (xlWorkbook != null)
                    xlWorkbook.Close(false, Type.Missing, Type.Missing);

                if (xlApp != null)
                    xlApp.Quit();

                //Marshal.FinalReleaseComObject(range1);
                //Marshal.FinalReleaseComObject(range2);
                Marshal.FinalReleaseComObject(xlWorksheet);
                Marshal.FinalReleaseComObject(xlWorkbook);
                //Marshal.FinalReleaseComObject(xlWorkBooks);
                Marshal.FinalReleaseComObject(xlApp);
            }

        }
    }
}
