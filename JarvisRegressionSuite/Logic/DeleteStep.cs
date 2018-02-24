using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace regression_test_suite
{
    class DeleteStep : Step
    {
        public DeleteStep()
        {

        }

        #region DEFINING DATA MEMEBERS AND PROPERTIES

        #endregion

        #region MAIN FUNCTION OF EXECUTE

        public override void Execute(object iObject)
        {
            try
            {
                SetFileAppenderPath("ThisStepFileAppender");

                OnStepStarted(new StepEventArgs(iObject));

                //delete installer folder //:think: 
                //var installerDir = new DirectoryInfo(Properties.Regression.Default.InstallerCopyPath);
                //if (installerDir.Exists)
                //{
                //    log.Info("Deleting folder: " + installerDir.FullName);
                //    installerDir.Delete(true); //:think: need to forcefully delete the folder.
                //}

                //delete log folder  -- to be deleted or not to be, is the question.
                //var logDir = new DirectoryInfo(Properties.Regression.Default.LogFolderPath);
                //if (logDir.Exists)
                //{
                //    log.Info("Deleting folder: " + logDir.FullName);
                //    logDir.Delete(true);
                //    //ProcessHelper procHelper = new ProcessHelper();
                //    //procHelper.ExecuteProcess("rmdir", "/s /q " + logDir);
                //}

                //delete xml, dfmr and dfmresults from parts folder
                var files = Directory.EnumerateFiles(Properties.Regression.Default.PartsFolder, "*.*", SearchOption.AllDirectories)
                    .Where(file => file.ToLower().EndsWith(".xml") || file.ToLower().EndsWith(".dfmr") || file.ToLower().EndsWith(".dfmresult"));

                foreach (string thisFile in files)
                {
                    log.Info("Deleting file: " + thisFile);
                    var file = new FileInfo(thisFile);
                    file.Delete();
                    
                }
                
                //delete reports folder
                var reportsDir = new DirectoryInfo(Properties.Regression.Default.ReportPath);
                if(reportsDir.Exists)
                {
                    log.Info("Deleting folder " + Properties.Regression.Default.ReportPath);
                    reportsDir.Delete(true);
                }

                OnStepCompleted(new StepEventArgs(iObject));
            }
            catch (Exception ex)
            {
                OnStepFailed(new StepFailedEventArgs(iObject, ex));
            }
        }
        #endregion
    }
}
