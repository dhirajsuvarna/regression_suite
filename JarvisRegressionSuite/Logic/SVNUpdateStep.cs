using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace regression_test_suite
{
    class SVNUpdateStep : Step
    {
        public override void Execute(object iArgument)
        {
            try
            {
                SetFileAppenderPath("ThisStepFileAppender");

                OnStepStarted(new StepEventArgs(iArgument));

                string processName = @"C:\Program Files\SlikSvn\bin\svn";
                string foldersToUpdate = Path.GetFullPath(Properties.Regression.Default.BaselineFolder + "\\..\\..");

                string svnrevertArgument = "revert --recursive " + foldersToUpdate;

                log.Info("Process: " + processName);
                log.Info("Arguments: " + svnrevertArgument);
                Helper.ProcessHelper procHelper = new Helper.ProcessHelper();
                procHelper.dataReceivedHandler += proc_OutputDataReceived;
                procHelper.errorReceivedHandler += proc_ErrorDataReceived;

                int exitCode = procHelper.Execute(processName, svnrevertArgument);

                if (exitCode != 0)
                { 
                    string errorString = processName + " exited with error code: " + exitCode.ToString();
                    OnStepFailed(new StepFailedEventArgs(iArgument, errorString));
                }
                else
                {
                    string svnupdateArgument = "up " + foldersToUpdate;

                    log.Info("Process: " + processName);
                    log.Info("Arguments: " + svnupdateArgument);
                    int upExitCode = procHelper.Execute(processName, svnupdateArgument);
                    if (upExitCode != 0)
                    {
                        string errorString = processName + " exited with error code: " + upExitCode.ToString();
                        OnStepFailed(new StepFailedEventArgs(iArgument, errorString));
                    }
                    else
                    {
                        OnStepCompleted(new StepEventArgs(iArgument));
                    }
                }

            }
            catch (Exception ex)
            {
                OnStepFailed(new StepFailedEventArgs(iArgument, ex));
            }
        }

        private void proc_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                log.Error(e.Data);
            }
        }

        private void proc_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                log.Info(e.Data);
            }
        }
    }
}
