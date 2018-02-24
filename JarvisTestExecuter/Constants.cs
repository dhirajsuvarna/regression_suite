using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing_cost
{
    enum eStatus
    { 
        READY,
        SKIPPED,
        EXECUTING,
        PAUSED,
        COMPLETED,
        FAILED
    }

    public static class Constants
    {
        public static class ProcessType
        {
            public const string Mill = "Mill";
            public const string SM = "SheetMetal";
            public const string IM = "IM";
        }

        public static class ExcelColumnNames
        { 
            public const string SrNo = "Sr No";
            public const string RelativeLocation = "Relative Location";
            public const string PartNumber = "Part No";
            public const string Process = "Process";
            public const string DFMMaterial = "DFM Material";
            public const string CostMaterialGrade = "Cost Material Grade";
            public const string CostMaterialFamily = "Cost Material Family";
            public const string AnnualQuantiy = "Annual Qty";
            public const string ShouldCostAvailable = "Should Cost Available?";
        }
        
        public static class DGVColumnName
        { 
            public const string IsRequestFileDiff = "IsRequestFileDiff";
            public const string IsResponseFileDiff = "IsResponseFileDiff";
            public const string Status = "Status";
            public const string CostRequestFile = "CostRequestFile";
            public const string CostResponseFile = "CostResponseFile";
            public const string DFMResultsFile = "DFMResultsFile";
            public const string SelectParts = " ";
            public const string CompareRequestFile = "CompareRequestFile";
            public const string CompareResponseFile = "CompareResponseFile";
            public const string AppoxRunTime = "ApproxRunTime";
        }

        public static class DGVColumnIndex
        {
            public const int SelectParts = 0;
        }

        public static class DiffTools
        {
            public static class Name
            {
                public const string Araxis = "Araxis";
                public const string WinMerge = "WinMerge";
            }

            public static class Exe
            {
                public const string Araxis = @"C:\Program Files (x86)\Araxis\Araxis Merge\Merge.exe";
                public const string WinMerge = @"C:\Program Files (x86)\WinMerge\WinMergeU.exe";
            }
        }

        public static class AppPath
        {
            public const string RegAsm64 = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\regasm.exe";
        }
    }
}
