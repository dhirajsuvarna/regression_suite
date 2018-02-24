using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;

namespace regression_test_suite
{
    class CopyStep : Step
    {
        public CopyStep(string iCopyFrom, string iCopyTo, string iFileToCopy)
        {
            copyFrom = iCopyFrom;
            copyTo = iCopyTo;
            fileToCopy = iFileToCopy;
        }

        ///////////////////////////////////////////////////////////////////////////////

        #region DEFINING DATA MEMBERS AND PROPERTIES
        
        private string copyFrom;
        private string copyTo;
        private string fileToCopy;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////

        #region DEFINING EVENTS AND ITS HANDLERS
        
        //public DataReceivedEventHandler errorReceivedEventHandler;    //msg: future use
        //public DataReceivedEventHandler dataReceivedEventHandler;     //msg: future use

        #endregion

        ///////////////////////////////////////////////////////////////////////////////

        #region MAIN FUNCTION OF EXECUTE 
        public override void Execute(object iArgument)
        {
            try
            {
                SetFileAppenderPath("ThisStepFileAppender"); //todo: this should be changed to interface since this error prone to maintainance

                OnStepStarted(new StepEventArgs(iArgument));

                //check if the source exits
                if (!File.Exists(copyFrom + "\\" + fileToCopy))
                {
                    log.Error("File Not Found: " + copyFrom + "\\" + fileToCopy); //should this logging moved into OnStepFailed? //:think:
                    throw new System.IO.FileNotFoundException();
                }

                //check if the log path exits
                if (!Directory.Exists(copyTo))
                {
                    Directory.CreateDirectory(copyTo);
                }


                string processName = "robocopy.exe";
                //no need to check if destination exits since robocopy creates destination if not present.
                string arguments = "\"" + copyFrom + "\" \"" + copyTo + "\" \"" + fileToCopy + "\"";

                Helper.ProcessHelper procHelper = new Helper.ProcessHelper();
                procHelper.dataReceivedHandler += proc_OutputDataReceived;
                procHelper.errorReceivedHandler += proc_ErrorDataReceived;
               
                int exitCode = procHelper.Execute(processName, arguments);

                if (exitCode < 8) //check this link for exit codes of robocopy https://ss64.com/nt/robocopy-exit.html
                {
                    OnStepCompleted(new StepEventArgs(iArgument));
                }
                else
                {
                    string errorString = processName + " exited with error code: " + exitCode.ToString();
                    OnStepFailed(new StepFailedEventArgs(iArgument, errorString));
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

        void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (/*errorReceivedEventHandler != null &&*/ !String.IsNullOrEmpty(e.Data))
            {
                log.Error(e.Data);
                //errorReceivedEventHandler(sender, e); //msg: future use
            }
        }

        void proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (/*dataReceivedEventHandler != null &&*/ !String.IsNullOrEmpty(e.Data))
            {
                log.Info(e.Data);
                //dataReceivedEventHandler(sender, e); //msg: future use
            }
        }

        #endregion

    }
}
