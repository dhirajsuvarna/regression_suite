using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.IO;

namespace regression_test_suite
{
    class LicenseSetupStep : Step
    {
        public override void Execute(object iObject)
        {
            try
            {
                SetFileAppenderPath("ThisStepFileAppender");

                OnStepStarted(new StepEventArgs(iObject));

                if (DoesServiceExists(Properties.Regression.Default.LicenseServiceName) == false)
                {
                    string processName = Properties.Regression.Default.LicenseServerPath + @"\installs.exe";
                    if (!File.Exists(processName))
                    {
                        throw new FileNotFoundException();
                    }

                    string serverLogPath = Path.Combine(Properties.Regression.Default.LicenseServerPath, "LicenseServer.log");
                    string arguments = "-c \"" + Properties.Regression.Default.DFMProLicenseFile + "\" -e \"" + Properties.Regression.Default.LicenseServerPath + "\\lmgrd.exe\" -n \"" + Properties.Regression.Default.LicenseServiceName + "\" -l \"" + serverLogPath + "\"";
                    log.Info("Process: " + processName);
                    log.Info("Arguments: " + arguments);
                    Helper.ProcessHelper procHelper = new Helper.ProcessHelper();
                    procHelper.dataReceivedHandler += dataReceivedHandler;
                    procHelper.errorReceivedHandler += errorReceivedHandler;
                    int exitCode = procHelper.Execute(processName, arguments); //:think: check logging - its not proper.
                    if (exitCode != 0)
                    {
                        string errorString = processName + " exited with error code " + exitCode.ToString();
                        OnStepFailed(new StepFailedEventArgs(iObject, errorString));
                    }
                }

                ServiceController sc = new ServiceController(Properties.Regression.Default.LicenseServiceName);
                if (sc.Status != ServiceControllerStatus.Running)
                {
                    log.Info("Attempting to start the service");
                    sc.Start();
                }

                //:todo:
                //check if the service is startes
                sc.Refresh();
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    log.Info("Service Started");
                    Environment.SetEnvironmentVariable("DFMPRO_LICENSE_FILE", Properties.Regression.Default.LicenseServer, EnvironmentVariableTarget.Process);
                    OnStepCompleted(new StepEventArgs(iObject));
                }
                else
                {
                    string errorString = "Not able to start the service";
                    OnStepFailed(new StepFailedEventArgs(iObject, errorString));
                }

                //check if the license exists.

            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException)
                {
                    FileNotFoundException fex = ex as FileNotFoundException;
                    log.Error("File Not Found: " + fex.FileName);
                }

                OnStepFailed(new StepFailedEventArgs(iObject, ex));
            }
            
        }

        private void errorReceivedHandler(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                log.Error(e.Data);
            }
        }

        private void dataReceivedHandler(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                log.Info(e.Data);
            }
        }

        private bool DoesServiceExists(string iServiceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            var service = services.FirstOrDefault(s => s.ServiceName == iServiceName);
            return service != null;
        }
    }
}
