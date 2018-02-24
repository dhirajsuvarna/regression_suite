using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace regression_test_suite
{
    class GenerateReportStep : Step
    {
        public override void Execute(object iObject)
        {
            try
            {
                OnStepStarted(new StepEventArgs(iObject));

                string noDiffFile = Path.Combine(Properties.Regression.Default.ReportPath, "zerodiff");

                if (!File.Exists(noDiffFile))
                {
                    //check if sample.hmtl exists
                    string emailReport = Path.Combine(Properties.Regression.Default.ReportPath, "success_email_template_nonzerodiff.html");
                    if (File.Exists(emailReport))
                    {
                        string emailReportText = File.ReadAllText(emailReport);

                        WriteToHeader(ref emailReportText);


                        string millReportPath = Path.Combine(Properties.Regression.Default.SharedPath, "reports", "mill_diff_rpt.html");
                        string smReportPath = Path.Combine(Properties.Regression.Default.SharedPath, "reports", "sm_diff_rpt.html");
                        string imReportPath = Path.Combine(Properties.Regression.Default.SharedPath, "reports", "im_diff_rpt.html");

                        string millRequestReport = Path.Combine(Properties.Regression.Default.SharedPath, "reports", "mill_diff_request_rpt.html");
                        string millResponseReport = Path.Combine(Properties.Regression.Default.SharedPath, "reports", "mill_diff_response_rpt.html");
                        string millOnlyBaselineReport = Path.Combine(Properties.Regression.Default.SharedPath, "reports", "mill_diff_onlybaseline_rpt.html");
                        string millOnlyPartReport = Path.Combine(Properties.Regression.Default.SharedPath, "reports", "mill_diff_onlypart_rpt.html");

                        string smRequestReport = Path.Combine(Properties.Regression.Default.SharedPath, "reports", "sm_diff_request_rpt.html");
                        string smResponseReport = Path.Combine(Properties.Regression.Default.SharedPath, "reports", "sm_diff_response_rpt.html");
                        string smOnlyBaselineReport = Path.Combine(Properties.Regression.Default.SharedPath, "reports", "sm_diff_onlybaseline_rpt.html");
                        string smOnlyPartReport = Path.Combine(Properties.Regression.Default.SharedPath, "reports", "sm_diff_onlypart_rpt.html");

                        string imRequestReport = Path.Combine(Properties.Regression.Default.SharedPath, "reports", "im_diff_request_rpt.html");
                        string imResponseReport = Path.Combine(Properties.Regression.Default.SharedPath, "reports", "im_diff_response_rpt.html");
                        string imOnlyBaselineReport = Path.Combine(Properties.Regression.Default.SharedPath, "reports", "im_diff_onlybaseline_rpt.html");
                        string imOnlyPartReport = Path.Combine(Properties.Regression.Default.SharedPath, "reports", "im_diff_onlypart_rpt.html");

                        emailReportText = emailReportText.Replace("{milldiffRequest}", millRequestReport);
                        emailReportText = emailReportText.Replace("{milldiffResponse}", millResponseReport);
                        emailReportText = emailReportText.Replace("{millOnlyBaseline}", millOnlyBaselineReport);
                        emailReportText = emailReportText.Replace("{millOnlyPart}", millOnlyPartReport);
                        emailReportText = emailReportText.Replace("{milldiffreport}", millReportPath);

                        emailReportText = emailReportText.Replace("{smdiffRequest}", smRequestReport);
                        emailReportText = emailReportText.Replace("{smdiffResponse}", smResponseReport);
                        emailReportText = emailReportText.Replace("{smOnlyBaseline}", smOnlyBaselineReport);
                        emailReportText = emailReportText.Replace("{smOnlyPart}", smOnlyPartReport);
                        emailReportText = emailReportText.Replace("{smdiffreport}", smReportPath);

                        emailReportText = emailReportText.Replace("{imdiffRequest}", imRequestReport);
                        emailReportText = emailReportText.Replace("{imdiffResponse}", imResponseReport);
                        emailReportText = emailReportText.Replace("{imOnlyBaseline}", imOnlyBaselineReport);
                        emailReportText = emailReportText.Replace("{imOnlyPart}", imOnlyPartReport);
                        emailReportText = emailReportText.Replace("{imdiffreport}", imReportPath);

                        //----------------------------------------------------------------------------------

                        string baselineFullPath = Path.GetFullPath(Properties.Regression.Default.BaselineFolder);
                        string sharedBaseline = baselineFullPath.Replace(Properties.Regression.Default.LocalRegPath, Properties.Regression.Default.SharedPath);
                        string sharedResultPath = Properties.Internal.Default.BackupResultsPath.Replace(Properties.Regression.Default.LocalRegPath, Properties.Regression.Default.SharedPath);
                        string dailyResultFullPath = Path.GetFullPath(Properties.Regression.Default.PartsFolder);
                        string sharedDailyResultPath = dailyResultFullPath.Replace(Properties.Regression.Default.LocalRegPath, Properties.Regression.Default.SharedPath);

                        emailReportText = emailReportText.Replace("{baselineResults}", sharedBaseline);
                        emailReportText = emailReportText.Replace("{dailyTestResults}", sharedDailyResultPath);
                        emailReportText = emailReportText.Replace("{backupTestResults}", sharedResultPath);

                        File.WriteAllText(emailReport, emailReportText);
                    }
                }
                else
                {
                    string emailReport = Path.Combine(Properties.Regression.Default.TemplatePath, "success_email_template_zerodiff.html");
                    if(File.Exists(emailReport))
                    {
                        string emailReportText = File.ReadAllText(emailReport);

                        WriteToHeader(ref emailReportText);

                        string baselineFullPath = Path.GetFullPath(Properties.Regression.Default.BaselineFolder);
                        string sharedBaseline = baselineFullPath.Replace(Properties.Regression.Default.LocalRegPath, Properties.Regression.Default.SharedPath);
                        string sharedResultPath = Properties.Internal.Default.BackupResultsPath.Replace(Properties.Regression.Default.LocalRegPath, Properties.Regression.Default.SharedPath);
                        string dailyResultFullPath = Path.GetFullPath(Properties.Regression.Default.PartsFolder);
                        string sharedDailyResultPath = dailyResultFullPath.Replace(Properties.Regression.Default.LocalRegPath, Properties.Regression.Default.SharedPath);

                        emailReportText = emailReportText.Replace("{baselineResults}", sharedBaseline);
                        emailReportText = emailReportText.Replace("{dailyTestResults}", sharedDailyResultPath);
                        emailReportText = emailReportText.Replace("{backupTestResults}", sharedResultPath);

                        File.WriteAllText(emailReport, emailReportText);
                    }
                }
                OnStepCompleted(new StepEventArgs(iObject));
            }
            catch (Exception ex)
            {
                OnStepFailed(new StepFailedEventArgs(iObject, ex));
            }

        }

        private void WriteToHeader(ref string iEmailText)
        {
            iEmailText = iEmailText.Replace("{RegTriggerDateTime}", Properties.Internal.Default.RegressionTriggerDateTime);
            iEmailText = iEmailText.Replace("{RegTimeTaken}", Properties.Internal.Default.RegressionTimeTaken);

            iEmailText = iEmailText.Replace("{dfmProductVersion}", ProductVersion(Properties.Regression.Default.DFMProductName));
            iEmailText = iEmailText.Replace("{costProductVersion}", ProductVersion(Properties.Regression.Default.CostProductName));
            iEmailText = iEmailText.Replace("{costServerName}", CostServerName());
            iEmailText = iEmailText.Replace("{costDbName}", CostDatabaseName());

            string dfmsvnrevisionFilePath = Path.Combine(Properties.Regression.Default.DFMProInstallerPath, "build_svn_revision.txt");
            if (File.Exists(dfmsvnrevisionFilePath))
            {
                string dfmsvnrevisionFile = File.ReadAllText(dfmsvnrevisionFilePath);
                string dfmsvnrevisionStr = Regex.Match(dfmsvnrevisionFile, "[0-9]+").Value;
                int dfmsvnrevision;
                if (int.TryParse(dfmsvnrevisionStr, out dfmsvnrevision))
                {
                    iEmailText = iEmailText.Replace("{dfmSVNRevision}", dfmsvnrevisionStr);
                }
            }
               
        }

        private string ProductVersion(string iProductName)
        {
            string productVersion = null;
            string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey);
            if (key != null)
            {
                foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
                {
                    string displayName = subkey.GetValue("DisplayName") as string;
                    if (displayName != null && displayName.Contains(iProductName))
                    {
                        productVersion = subkey.GetValue("DisplayVersion") as string;
                    }
                }
                key.Close();
            }
            return productVersion;
        }

        private string CostDatabaseName()
        {
            string databaseName = null;
            RegistryKey key = Registry.CurrentUser.OpenSubKey(Properties.Regression.Default.MTIRegistryPath + "\\Database");
            if (key != null)
            {
                databaseName = key.GetValue("CurrentDatabaseName") as string;
                key.Close();
            }
            return databaseName;
        }

        private string CostServerName()
        {
            string serverName = null;
            RegistryKey key = Registry.CurrentUser.OpenSubKey(Properties.Regression.Default.MTIRegistryPath + "\\Database");
            if (key != null)
            {
                serverName = key.GetValue("LoginServer") as string;
                key.Close();
            }
            return serverName;
        }
    }
}
