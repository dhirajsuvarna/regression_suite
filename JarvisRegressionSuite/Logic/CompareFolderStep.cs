using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace regression_test_suite
{
    class CompareFolderStep : Step
    {
        public CompareFolderStep(string iLeftFolder, string iRightFolder)
        {
            leftFolder = iLeftFolder;
            rightFolder = iRightFolder;
        }

        #region DEFINE DATA MEMBERS AND PROPERTIES

        string leftFolder;
        string rightFolder;

        #endregion

        #region MAIN FUNCTION OF EXECUTE

        public override void Execute(object iArgument)
        {
            try
            {
                SetFileAppenderPath("ThisStepFileAppender");

                OnStepStarted(new StepEventArgs(iArgument));

                string processName = @"C:\cygwin64\bin\bash.exe";
                string diffScriptPath = Properties.Regression.Default.ScriptsFolder + "\\" + "generate_diff_report.sh";
                string arguments = diffScriptPath + " " + leftFolder + " " + rightFolder;

                Helper.ProcessHelper procHelper = new Helper.ProcessHelper();
                procHelper.EnviornmentPath = @"C:\cygwin64\bin";
                procHelper.dataReceivedHandler += DataReceivedHandler;
                procHelper.errorReceivedHandler += ErrorRecivedHandler;

                log.Info("Process: " + processName);
                log.Info("Arguments: " + arguments);
                int exitCode = procHelper.Execute(processName, arguments);
                if (exitCode == 0)
                {
                    OnStepCompleted(new StepEventArgs(iArgument));
                }
                else
                {
                    string errorString = processName + "failed with error code + " + exitCode.ToString();
                    OnStepFailed(new StepFailedEventArgs(iArgument, errorString));
                }
            }
            catch (Exception ex)
            {
                OnStepFailed(new StepFailedEventArgs(iArgument, ex));
            }
        }

        private void ErrorRecivedHandler(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                log.Error(e.Data);
            }
        }

        private void DataReceivedHandler(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                log.Info(e.Data);
            }
        }
        #endregion
    }
}
