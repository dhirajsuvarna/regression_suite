using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace regression_test_suite
{
    class BackupStep : Step
    {
        public override void Execute(object iObject)
        {
            try
            {
                SetFileAppenderPath("ThisStepFileAppender");
                
                OnStepStarted(new StepEventArgs(iObject));

                string regressionStartDate = System.DateTime.Now.ToString("dd-MMM-yyyy_HH_mm"); //:change: regression date should come from the calling class
                string backupFolderPath = Path.Combine(Properties.Regression.Default.TestResultsBackup, regressionStartDate);
                log.Info("Backup Folder: " + backupFolderPath);
                Properties.Internal.Default.BackupResultsPath = Path.GetFullPath(backupFolderPath);

                string resultSourcePath = Properties.Regression.Default.PartsFolder;
                string resultDsntPath = Path.Combine(backupFolderPath, "testresults");

                //COPY ALL XML, DFMR AND DFMRESULTS FILES
                log.Info("COPY RESULT FILES");
                var files = Directory.EnumerateFiles(resultSourcePath, "*.*", SearchOption.AllDirectories).Where(s => s.ToLower().EndsWith(".xml") || s.ToLower().EndsWith(".dfmr") || s.ToLower().EndsWith(".dfmresult"));
                foreach (string thisFile in files)
                {
                    string destinationFile = thisFile.Replace(resultSourcePath, resultDsntPath);
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationFile));
                    
                    log.Info("Copy " + thisFile + "to " + destinationFile);
                    File.Copy(thisFile, destinationFile, true);
                }

                //COPY LOG FOLDER
                string logSourcePath = Properties.Regression.Default.LogFolderPath;
                string logDstnPath = Path.Combine(backupFolderPath, "log");

                Helper.ProcessHelper procHelper = new Helper.ProcessHelper();
                procHelper.dataReceivedHandler += DataReceivedHandler;
                procHelper.errorReceivedHandler += ErrorReceivedHander;

                log.Info("COPY LOG FILES");
                procHelper.Execute("robocopy.exe", logSourcePath + " " + logDstnPath);

                //COPY REPORTS FOLDER
                string reportSourcePath = Properties.Regression.Default.ReportPath;
                string reportDstnPath = Path.Combine(backupFolderPath, "reports");

                log.Info("COPY REPORT FILES");
                procHelper.Execute("robocopy.exe", reportSourcePath + " " + reportDstnPath);

                OnStepCompleted(new StepEventArgs(iObject));

            }
            catch (Exception ex)
            {
                OnStepFailed(new StepFailedEventArgs(iObject, ex));
            }
        }

        private void ErrorReceivedHander(object sender, System.Diagnostics.DataReceivedEventArgs e)
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
    }
}
