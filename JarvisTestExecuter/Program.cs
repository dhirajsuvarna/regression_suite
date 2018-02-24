using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testing_cost
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] iArguments)
        {
            if (iArguments.Length == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new TestCasesForm());
            }
            else if (iArguments.Length == 2)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                TestCasesForm testCaseForm = new TestCasesForm(true);
                testCaseForm.TestCaseFile = iArguments[0];
                testCaseForm.DFMProBinLocation = iArguments[1];
                
                string isRegressionSystem = Environment.GetEnvironmentVariable("IsRegressionSystem", EnvironmentVariableTarget.Machine);
                if (isRegressionSystem != null && isRegressionSystem.ToLower().Equals("yes"))
                    testCaseForm.IsRegressionSystem = true;
                else
                    testCaseForm.IsRegressionSystem = false;
                
                Application.Run(testCaseForm);

            }
            else
            {
                Console.WriteLine("Usage: <program name> <input file name> <location of dfm binairies>");
                return;
            }
        }
    }
}
