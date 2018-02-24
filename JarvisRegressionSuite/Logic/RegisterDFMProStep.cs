using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace regression_test_suite
{
    class RegisterDFMProStep : Step
    {
        #region MAIN FUNCTION OF EXECUTE

        public override void Execute(object iArgument)
        {
            try
            {
                SetFileAppenderPath("ThisStepFileAppender");

                OnStepStarted(new StepEventArgs(iArgument));

                string processName = "regedit.exe";
                string arguments = "/s " + Properties.Regression.Default.RegFilePath + @"\DFMProInterfaceOpenRegEntry.reg";

                //RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Geometric\DPT_PRODUCT\DFMPro\ProE\Add-in");
                //key.SetValue("bIsDFMProInterfaceOpen", "1");
                Helper.ProcessHelper procHelper = new Helper.ProcessHelper();
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
            catch (Exception ex)
            {
                OnStepFailed(new StepFailedEventArgs(iArgument, ex));
            }

        }

        #endregion
    }
}
