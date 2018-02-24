using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace regression_test_suite
{
    class UnzipStep : Step
    {
        public UnzipStep(string iUnzipFrom, string iUnzipTo, string iFileToUnzip)
        {
            unzipFrom = iUnzipFrom;
            unzipTo = iUnzipTo;
            fileToUnzip = iFileToUnzip;
        }

        public UnzipStep(string iUnzipFrom, string iUnzipTo, List<string> iFilesToUnzip)
        {
            unzipFrom = iUnzipFrom;
            unzipTo = iUnzipTo;
            filesToUnzip = iFilesToUnzip;
        }


        #region DEFINING MEMBER VARIABLES AND PROPERTIES
        public string unzipFrom;
        public string unzipTo;
        List<string> filesToUnzip;
        public string fileToUnzip;

        #endregion 

        #region MAIN STEP TO EXECUTE

        public override void Execute(object iArgument)
        {
            try
            {
                SetFileAppenderPath("ThisStepFileAppender"); //todo: this should be changed to interface since this error prone to maintainance

                OnStepStarted(new StepEventArgs(iArgument));

                string argumentString = CreateArgumentString();
                string processName = @"C:\Program Files\7-Zip\7z.exe";

                Helper.ProcessHelper procHelper = new Helper.ProcessHelper();
                procHelper.dataReceivedHandler += proc_OutputDataReceived;
                procHelper.errorReceivedHandler += proc_ErrorDataReceived;
                procHelper.EnviornmentPath = @"C:\Program Files\7-Zip"; //change
                log.Info("Process Name: " + processName);
                log.Info("Arguments: " + argumentString);
                int exitCode = procHelper.Execute(processName, argumentString);
                if (exitCode > 0)
                {
                    string errorString = processName + " exited with error code: " + exitCode.ToString();
                    OnStepFailed(new StepFailedEventArgs(iArgument, errorString));
                }
                else
                {
                    OnStepCompleted(new StepEventArgs(iArgument));
                }
            }
            catch (Exception ex)
            {
                OnStepFailed(new StepFailedEventArgs(iArgument, ex));
            }
        }

        private void proc_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if(e.Data != null)
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

        private string CreateArgumentString()
        {
            string argumentString = null;
            if (filesToUnzip != null)
            {
            }
            else
            {
                //e = extract 
                //-o = Setouput Directory
                //-y = Assume Yes on all Queries
                //Reference link: https://sevenzip.osdn.jp/chm/cmdline/commands/extract.htm 

                argumentString = "e " + unzipFrom + " -o" + unzipTo + " " + fileToUnzip + " -y";
            }

            return argumentString;
        }
 
        



        #endregion
    }
}
