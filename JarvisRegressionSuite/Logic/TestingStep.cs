using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace regression_test_suite
{
    class TestingStep : Step
    {
        public override void Execute(object iObject)
        {
            try
            {
                SetFileAppenderPath("ThisStepFileAppender");

                OnStepStarted(new StepEventArgs(iObject));

                string processName = "JarvisTestExecuter.exe";
                string dfmTestAppPath = Path.Combine(Properties.Regression.Default.DFMInstallLocation, @"dfmpro", Properties.Regression.Default.DFMTestAppName);
                string arguments = "\"" + Path.GetFullPath(Properties.Regression.Default.RegressionInputFile) + "\" \"" + dfmTestAppPath + "\"";

                log.Info("Process Name: " + processName);
                log.Info("Arguments: " + arguments);
                Helper.ProcessHelper procHelper = new Helper.ProcessHelper();
                int exitCode = procHelper.Execute(processName, arguments);
                if (exitCode == 0)
                {
                    OnStepCompleted(new StepEventArgs(iObject));
                }
                else
                {
                    string errorString = processName + " exited with error code : " + exitCode.ToString();
                    OnStepFailed(new StepFailedEventArgs(iObject, errorString));
                }

            }
            catch (Exception ex)
            {
                OnStepFailed(new StepFailedEventArgs(iObject, ex));
            }

        }
    }
}
