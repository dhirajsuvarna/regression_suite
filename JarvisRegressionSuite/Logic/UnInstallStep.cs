using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace regression_test_suite
{
    class UnInstallStep : Step
    {
        public UnInstallStep(string iMSIPath, string iProductName)
        {
            MSIPath = iMSIPath;
            ProductName = iProductName; //this name will be used to see if the product is installed or not.
        }

        #region DEFINING DATA MEMEBERS AND PROPERTIES

        private string MSIPath { get; set; }
        private string ProductName { get; set; }

        #endregion

        #region MAIN FUNCTION OF EXECUTE

        public override void Execute(object iArgument)
        {
            try
            {
                //SetFileAppenderPath("ThisStepFileAppender");

                OnStepStarted(new StepEventArgs(iArgument));

                //ADD A CHECK TO SEE IF THE PRODUCT IS INSTALLED. //:imp:
                if (IsProductInstalled(ProductName))
                {
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
                    string arguments = "/x " + MSIPath + " /qn /Lv " + LogPath;

                    log.Info("Process: " + processName);
                    log.Info("Arguments: " + arguments);
                    Helper.ProcessHelper procHelper = new Helper.ProcessHelper();
                    int exitCode = procHelper.Execute(processName, arguments);
                    if (exitCode != 0)
                    {
                        string errorString = processName + " exited with error code: " + exitCode.ToString();
                        log.Error(errorString);
                        OnStepFailed(new StepFailedEventArgs(iArgument, errorString));
                    }
                    else
                    {
                        OnStepCompleted(new StepEventArgs(iArgument));
                    }
                }
                else
                {
                    log.Info("Cannot Uninstall since " + ProductName + " not installed");
                    OnStepCompleted(new StepEventArgs(iArgument));
                }
            }
            catch (Exception ex)
            {
                OnStepFailed(new StepFailedEventArgs(iArgument, ex));
            }
        }

        private bool IsProductInstalled(string iProductName)
        {
            bool isProductInstalled = false;
            string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey);
            if (key != null)
            {
                foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
                {
                    string displayName = subkey.GetValue("DisplayName") as string;
                    if (displayName != null && displayName.Contains(iProductName))
                    {  
                        isProductInstalled = true;
                    }
                }
                key.Close();
            }
            return isProductInstalled;
        }

        #endregion
    }
}
