using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace regression_test_suite
{
    class InstallStep : Step
    {
        public InstallStep(string iMSIPath)
        {
            MSIPath = iMSIPath;
        }

        #region DEFINING DATA MEMEBERS AND PROPERTIES

        private string MSIPath { get; set; }
        public string InstallDir { get; set; }

        #endregion

        #region MAIN FUNCTION OF EXECUTE

        public override void Execute(object iArgument)
        {
            try
            {
                //SetFileAppenderPath("ThisStepFileAppender"); //:think:

                OnStepStarted(new StepEventArgs(iArgument));

                if (!File.Exists(MSIPath))
                {
                    log.Error("File Not Found: " + MSIPath); //:think:
                    throw new FileNotFoundException();
                }

                string processName = "msiexec.exe";
                //i = Installs or configures a product
                //qn = No UI
                //Lv = Verbose output
                //For detail options of msiexec.exe use following command on the command prompt "msiexec /?"
                MSIPath = Path.GetFullPath(MSIPath);
                string arguments;
                if (InstallDir != null)
                    arguments = "/i " + MSIPath + " INSTALLDIR=\"" + InstallDir + "\" /qn /Lv " + LogPath;
                else
                    arguments = "/i " + MSIPath + " /qn /Lv " + LogPath;

                Helper.ProcessHelper procHelper = new Helper.ProcessHelper();

                log.Info("Process: " + processName);
                log.Info("Arguments: " + arguments);
                int exitCode = procHelper.Execute(processName, arguments);
                if (exitCode != 0)
                {
                    string errorString = processName + " exited with error code: " + exitCode.ToString();
                    OnStepFailed(new StepFailedEventArgs(iArgument, errorString));
                }
                else
                {
                    OnStepCompleted(new StepEventArgs(iArgument));
                }

            }
            catch (FileNotFoundException ex)
            {
                OnStepFailed(new StepFailedEventArgs(iArgument, ex));
            }
            catch (Exception ex)
            {
                OnStepFailed(new StepFailedEventArgs(iArgument, ex));
            }
        }

        #endregion

    }
}
