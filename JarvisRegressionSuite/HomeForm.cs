using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;

using RegressionSettings = regression_test_suite.Properties.Regression;
using System.Drawing.Imaging;
using System.Security.Principal;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace regression_test_suite
{
    public partial class HomeForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private BindingList<Step> stepList = new BindingList<Step>();
        private Stopwatch stopWatch = new Stopwatch();
        private Step failedStep = null;

        public HomeForm()
        {
            InitializeComponent();
            ModifyDataGridViewProperties();
            CreateSteps();
            GenerateDataGrid();
        }


        ///////////////////////////////////////////////////////////////////////////////

        #region DATA GRID MODIFYING, POPULATING AND EVENT HANDLING 
        private void ModifyDataGridViewProperties()
        {
            this.steps_DataGridView.AutoGenerateColumns = false;
            this.steps_DataGridView.CellFormatting += steps_DataGridView_CellFormatting;
            this.steps_DataGridView.CellContentClick += steps_DataGridView_CellContentClick;
        }

        void GenerateDataGrid()
        {
            steps_DataGridView.DataSource = stepList;

            DataGridViewTextBoxColumn stepNumberColumn = new DataGridViewTextBoxColumn();
            stepNumberColumn.HeaderText = "Step #";
            stepNumberColumn.Name = "stepNumber";
            //stepNumberColumn.DataPropertyName 

            steps_DataGridView.Columns.Add(stepNumberColumn);

            DataGridViewTextBoxColumn stepDescriptionColumn = new DataGridViewTextBoxColumn();
            stepDescriptionColumn.HeaderText = "Steps Description";
            stepDescriptionColumn.Name = "stepsDescription";
            stepDescriptionColumn.DataPropertyName = "Name";

            steps_DataGridView.Columns.Add(stepDescriptionColumn);

            DataGridViewTextBoxColumn statusColumn = new DataGridViewTextBoxColumn();
            statusColumn.HeaderText = "Status";
            statusColumn.Name = "status";
            statusColumn.DataPropertyName = "Status";

            steps_DataGridView.Columns.Add(statusColumn);

            DataGridViewLinkColumn logColumn = new DataGridViewLinkColumn();
            logColumn.HeaderText = "Log";
            logColumn.Name = "log";
            logColumn.DataPropertyName = "LogPath";

            steps_DataGridView.Columns.Add(logColumn);
        }

        void steps_DataGridView_CellFormatting(object sender, System.Windows.Forms.DataGridViewCellFormattingEventArgs e)
        {
            if (steps_DataGridView.Columns[e.ColumnIndex].Name.Equals("log"))
            {
                if (e.Value != null)
                {
                    Step thisStep = steps_DataGridView.Rows[e.RowIndex].DataBoundItem as Step;
                    if (thisStep.Status == eStatus.COMPLETED || thisStep.Status == eStatus.FAILED)
                    {
                        string fullLogPath = e.Value.ToString();
                        e.Value = Path.GetFileName(fullLogPath);
                        DataGridViewCell thisCell = steps_DataGridView[e.ColumnIndex, e.RowIndex];
                        thisCell.ToolTipText = fullLogPath;

                    }
                    else
                    {
                        e.Value = "";
                    }
                }
            }
            if (steps_DataGridView.Columns[e.ColumnIndex].Name.Equals("status"))
            {
                Step thisStep = steps_DataGridView.Rows[e.RowIndex].DataBoundItem as Step;
                DataGridViewCell thisCell = steps_DataGridView[e.ColumnIndex, e.RowIndex];
                if (thisStep.Status == eStatus.READY)
                {
                    e.Value = "";
                    thisCell.Style.ForeColor = Color.Black;
                }
                else if (thisStep.Status == eStatus.COMPLETED)
                {
                    e.Value = "Completed";
                    thisCell.Style.ForeColor = Color.ForestGreen;
                    DataGridViewRow thisRow = steps_DataGridView.Rows[e.RowIndex];
                    thisRow.DefaultCellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Regular);
                }
                else if (thisStep.Status == eStatus.FAILED)
                {
                    e.Value = "Failed";
                    thisCell.Style.ForeColor = Color.Red;

                    DataGridViewRow thisRow = steps_DataGridView.Rows[e.RowIndex];
                    thisRow.DefaultCellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }
                else if (thisStep.Status == eStatus.SKIPPED)
                {
                    e.Value = "Skipped";
                    thisCell.Style.ForeColor = Color.DarkGray;
                }
                else if (thisStep.Status == eStatus.STARTED)
                {
                    e.Value = "In Progress";
                    thisCell.Style.ForeColor = Color.Orange;
                    DataGridViewRow thisRow = steps_DataGridView.Rows[e.RowIndex];
                    thisRow.DefaultCellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }
            }
        }

        void steps_DataGridView_CellContentClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (steps_DataGridView.Columns[e.ColumnIndex].Name.Equals("log"))
            {
                string fullLogPath = steps_DataGridView[e.ColumnIndex, e.RowIndex].Value.ToString();
                if (fullLogPath != null)
                {
                    if (File.Exists(fullLogPath))
                        Process.Start(fullLogPath); //Open the log file with the default text editor
                    else
                        MessageBox.Show("File Not Found: " + fullLogPath, this.Text); 
                }
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////

        #region CREATING STEPS

        private void CreateSteps()
        {

            DeleteStep cleanupStep = new DeleteStep();
            cleanupStep.Name = "Delete Previous Results & Reports";
            cleanupStep.LogPath = RegressionSettings.Default.LogFolderPath + @"\cleanup.log";
            cleanupStep.Status = (Properties.Debug.Default.SkipDeleteStep) ? eStatus.SKIPPED : eStatus.READY;
            stepList.Add(cleanupStep);

            UnInstallStep uninstallCostAddon = new UnInstallStep(RegressionSettings.Default.InstallerCopyPath + "\\" + RegressionSettings.Default.CostAddonMSIPackageName, Properties.Regression.Default.CostProductName);
            uninstallCostAddon.Name = "Uninstall Cost Addon Product";
            uninstallCostAddon.LogPath = RegressionSettings.Default.LogFolderPath + @"\uninstall_cost.log";
            uninstallCostAddon.StepFailed += (stepFailedEventHandler);
            uninstallCostAddon.Status = (Properties.Debug.Default.SkipUninstallCost) ? eStatus.SKIPPED : eStatus.READY;
            stepList.Add(uninstallCostAddon);

            UnInstallStep uninstallDFMPro = new UnInstallStep(RegressionSettings.Default.InstallerCopyPath + "\\" + RegressionSettings.Default.DFMMSIPackageName, Properties.Regression.Default.DFMProductName);
            uninstallDFMPro.Name = "Uninstall DFMPro Product";
            uninstallDFMPro.LogPath = RegressionSettings.Default.LogFolderPath + @"\uninstall_dfm.log";
            uninstallDFMPro.StepFailed += (stepFailedEventHandler);
            uninstallDFMPro.Status = (Properties.Debug.Default.SkipUninstallDFM) ? eStatus.SKIPPED : eStatus.READY;
            stepList.Add(uninstallDFMPro);

            CopyStep copyDFMInstaller = new CopyStep(RegressionSettings.Default.DFMProInstallerPath, RegressionSettings.Default.InstallerCopyPath, RegressionSettings.Default.DFMInstallerName);
            copyDFMInstaller.Name = "Copy DFMPro Installer";
            copyDFMInstaller.LogPath = RegressionSettings.Default.LogFolderPath + @"\dfm_installer_copy.log";
            copyDFMInstaller.StepStarted += (stepStartedEventHandler);
            copyDFMInstaller.StepCompleted += (stepCompletedEventHandler);
            copyDFMInstaller.StepFailed += (stepFailedEventHandler);
            //copyStep.dataReceivedEventHandler += new DataReceivedEventHandler(stepProgressEventHandler); //msg: future use
            //copyStep.errorReceivedEventHandler += new DataReceivedEventHandler(stepProgressEventHandler); //msg: future use
            copyDFMInstaller.Status = (Properties.Debug.Default.SkipCopyDFMInstaller) ? eStatus.SKIPPED : eStatus.READY;
            stepList.Add(copyDFMInstaller);

            string dfmInstallerPath = RegressionSettings.Default.InstallerCopyPath + "\\" + RegressionSettings.Default.DFMInstallerName;
            UnzipStep unzipDFMInstaller = new UnzipStep(dfmInstallerPath, RegressionSettings.Default.InstallerCopyPath, RegressionSettings.Default.DFMMSIPackageName);
            unzipDFMInstaller.Name = "Extact DFMPro MSI Package from Installer";
            unzipDFMInstaller.LogPath = RegressionSettings.Default.LogFolderPath + @"\unzip_dfm_msi.log";
            unzipDFMInstaller.StepStarted += (stepStartedEventHandler);
            unzipDFMInstaller.StepCompleted += (stepCompletedEventHandler);
            unzipDFMInstaller.StepFailed += (stepFailedEventHandler);
            unzipDFMInstaller.Status = (Properties.Debug.Default.SkipUnzipDFM) ? eStatus.SKIPPED : eStatus.READY;
            stepList.Add(unzipDFMInstaller);

            string dfmMSIPath = RegressionSettings.Default.InstallerCopyPath + "\\" + RegressionSettings.Default.DFMMSIPackageName;
            InstallStep installDfm = new InstallStep(dfmMSIPath);
            installDfm.Name = "Install DFMPro Product";
            installDfm.LogPath = RegressionSettings.Default.LogFolderPath + @"\install_dfm.log";
            installDfm.StepStarted += (stepStartedEventHandler);
            installDfm.StepCompleted += (stepCompletedEventHandler);
            installDfm.StepFailed += (stepFailedEventHandler);
            installDfm.Status = (Properties.Debug.Default.SkipInstallDFM) ? eStatus.SKIPPED : eStatus.READY;
            stepList.Add(installDfm);

            CopyStep copyCostInstaller = new CopyStep(RegressionSettings.Default.CostAddonInstallerPath, RegressionSettings.Default.InstallerCopyPath, RegressionSettings.Default.CostAddonInstallerName);
            copyCostInstaller.Name = "Copy Cost Addon Installer";
            copyCostInstaller.LogPath = RegressionSettings.Default.LogFolderPath + @"\cost_installer_copy.log";
            copyCostInstaller.StepStarted += (stepStartedEventHandler);
            copyCostInstaller.StepCompleted += (stepCompletedEventHandler);
            copyCostInstaller.StepFailed += (stepFailedEventHandler);
            copyCostInstaller.Status = (Properties.Debug.Default.SkipCopyCostInstaller) ? eStatus.SKIPPED : eStatus.READY;
            stepList.Add(copyCostInstaller);

            string costInstallerPath = RegressionSettings.Default.InstallerCopyPath + "\\" + RegressionSettings.Default.CostAddonInstallerName;
            UnzipStep unzipCostInstaller = new UnzipStep(costInstallerPath, RegressionSettings.Default.InstallerCopyPath, RegressionSettings.Default.CostAddonMSIPackageName);
            unzipCostInstaller.Name = "Extract Cost Addon MSI Package from Installer";
            unzipCostInstaller.LogPath = RegressionSettings.Default.LogFolderPath + @"\unzip_cost_msi.log";
            unzipCostInstaller.StepStarted += (stepStartedEventHandler);
            unzipCostInstaller.StepCompleted += (stepCompletedEventHandler);
            unzipCostInstaller.StepFailed += (stepFailedEventHandler);
            unzipCostInstaller.Status = (Properties.Debug.Default.SkipUnzipCost) ? eStatus.SKIPPED : eStatus.READY;
            stepList.Add(unzipCostInstaller);

            string costMSIPath = RegressionSettings.Default.InstallerCopyPath + "\\" + RegressionSettings.Default.CostAddonMSIPackageName;
            InstallStep installCostAddon = new InstallStep(costMSIPath);
            installCostAddon.Name = "Install Cost Addon Product";
            installCostAddon.InstallDir = Properties.Regression.Default.DFMInstallLocation;
            installCostAddon.LogPath = RegressionSettings.Default.LogFolderPath + @"\install_dfm.log";
            installCostAddon.StepStarted += (stepStartedEventHandler);
            installCostAddon.StepCompleted += (stepCompletedEventHandler);
            installCostAddon.StepFailed += (stepFailedEventHandler);
            installCostAddon.Status = (Properties.Debug.Default.SkipInstallCost) ? eStatus.SKIPPED : eStatus.READY;
            stepList.Add(installCostAddon);

            SVNUpdateStep updateSVN = new SVNUpdateStep();
            updateSVN.Name = "SVN Update Baseline and other folders";
            updateSVN.LogPath = Properties.Regression.Default.LogFolderPath + @"\svnupdate.log";
            //updateSVN.StepFailed += (stepFailedEventHandler);
            updateSVN.Status = (Properties.Debug.Default.SkipSVNUpdate) ? eStatus.SKIPPED : eStatus.READY;
            stepList.Add(updateSVN);

            RegisterDFMProStep registerDFMPro = new RegisterDFMProStep();
            registerDFMPro.Name = "Register DFMPro";
            registerDFMPro.LogPath = RegressionSettings.Default.LogFolderPath + @"\register_dfm.log";
            registerDFMPro.StepStarted += (stepStartedEventHandler);
            registerDFMPro.StepCompleted += (stepCompletedEventHandler);
            registerDFMPro.StepFailed += (stepFailedEventHandler);
            registerDFMPro.Status = (Properties.Debug.Default.SkipRegDFM) ? eStatus.SKIPPED : eStatus.READY;
            stepList.Add(registerDFMPro);

            CopyStep copyTestApp = new CopyStep(RegressionSettings.Default.TestAppPath, RegressionSettings.Default.DFMInstallLocation + @"\dfmpro", RegressionSettings.Default.DFMTestAppName);
            copyTestApp.Name = "Copy DFMPro Test Application";
            copyTestApp.StepFailed += (stepFailedEventHandler);
            copyTestApp.Status = (Properties.Debug.Default.SkipCopyTestApp) ? eStatus.SKIPPED : eStatus.READY;
            stepList.Add(copyTestApp);

            LicenseSetupStep setupDFMLicense = new LicenseSetupStep();
            setupDFMLicense.Name = "Setup DFMPro License";
            setupDFMLicense.LogPath = RegressionSettings.Default.LogFolderPath + @"\setup_dfm_license.log";
            setupDFMLicense.StepStarted += (stepStartedEventHandler);
            setupDFMLicense.StepCompleted += (stepCompletedEventHandler);
            setupDFMLicense.StepFailed += (stepFailedEventHandler);
            setupDFMLicense.Status = (Properties.Debug.Default.SkipLiceseSetup) ? eStatus.SKIPPED : eStatus.READY;
            stepList.Add(setupDFMLicense);

            TestingStep testing = new TestingStep();
            testing.Name = "Run Tests";
            testing.LogPath = RegressionSettings.Default.LogFolderPath + @"\testing_step.log";
            testing.StepStarted += (stepStartedEventHandler);
            testing.StepCompleted += (stepCompletedEventHandler);
            testing.StepFailed += (stepFailedEventHandler);
            testing.Status = (Properties.Debug.Default.SkipTestingStep) ? eStatus.SKIPPED : eStatus.READY;
            stepList.Add(testing);

            CompareFolderStep compareFolders = new CompareFolderStep(RegressionSettings.Default.BaselineFolder, RegressionSettings.Default.PartsFolder);
            compareFolders.Name = "Comparing Results";
            compareFolders.LogPath = RegressionSettings.Default.LogFolderPath + @"\diff_report.log";
            compareFolders.StepStarted += (stepStartedEventHandler);
            compareFolders.StepCompleted += (stepCompletedEventHandler);
            compareFolders.StepFailed += (stepFailedEventHandler);
            compareFolders.Status = (Properties.Debug.Default.SkipCompare) ? eStatus.SKIPPED : eStatus.READY;
            stepList.Add(compareFolders);

            BackupStep takeBackup = new BackupStep();
            takeBackup.Name = "Backup Results";
            takeBackup.LogPath = RegressionSettings.Default.LogFolderPath + @"\backup.log";
            takeBackup.StepStarted += (stepStartedEventHandler);
            takeBackup.StepCompleted += (stepCompletedEventHandler);
            takeBackup.StepFailed += (stepFailedEventHandler);
            takeBackup.Status = (Properties.Debug.Default.SkipBackup) ? eStatus.SKIPPED : eStatus.READY;
            takeBackup.IsSkippable = false;
            stepList.Add(takeBackup);

            GenerateReportStep generateReport = new GenerateReportStep();
            generateReport.Name = "Generating Report";
            generateReport.StepStarted += onGenerateReport_StepHandler;
            generateReport.StepFailed += (stepFailedEventHandler);
            generateReport.Status = (Properties.Debug.Default.SkipGenerateReport) ? eStatus.SKIPPED : eStatus.READY;
            stepList.Add(generateReport);

            SendEmail sendMail = new SendEmail();
            sendMail.Name = "Sending Email";
            sendMail.LogPath = RegressionSettings.Default.LogFolderPath + @"\mail.log";
            sendMail.StepStarted += (sendMailStartedEventHandler);
            sendMail.StepCompleted += (stepCompletedEventHandler);
            sendMail.StepFailed += (stepFailedEventHandler);
            sendMail.Status = (Properties.Debug.Default.SkipSendEmail) ? eStatus.SKIPPED : eStatus.READY;
            sendMail.IsSkippable = false;
            stepList.Add(sendMail);
        }

        private void onGenerateReport_StepHandler(object o, StepEventArgs e)
        {
            Properties.Internal.Default.RegressionTimeTaken = elapsedTime_Label.Text;
        }


        #endregion

        ///////////////////////////////////////////////////////////////////////////////

        #region HANDLING EVENTS OF STEPS

        private void sendMailStartedEventHandler(object o, StepEventArgs e)
        {
            //var failedSteps = stepList.Where(thisStep => thisStep.Status == eStatus.FAILED);

            SendEmail sendMailStep = o as SendEmail;
            sendMailStep.FailedStep = this.failedStep;
            //sendMailStep.FailedSteps = failedSteps.ToList<Step>();
            //if (sendMailStep.FailedSteps.Count > 0)
            if(this.failedStep != null)
                TakeActiveWindowScreenShot();
        }

        private void stepCompletedEventHandler(object sender, StepEventArgs e)
        {
            //msg: can be implemented

        }

        private void stepFailedEventHandler(object sender, StepFailedEventArgs e)
        {
            //skip remaining steps. -- need to figure out how to do this
            DataGridViewRow row = e.Argument as DataGridViewRow;
            Step fStep = row.DataBoundItem as Step;
            foreach (Step thisStep in stepList)
            {
                if (thisStep != fStep && thisStep.Status == eStatus.READY && thisStep.IsSkippable == true)
                    thisStep.Status = eStatus.SKIPPED;
            }

            this.failedStep = fStep;
        }


        private void stepStartedEventHandler(object sender, StepEventArgs e)
        {
            //msg: can be implemented
        }

        #region Commented code of How to access Controls from Threads
        
        //private delegate void SetTextCallback(string iText);
        //public void WriteOutputTextBox(string iText)
        //{
        //    if (output_TextBox.InvokeRequired)
        //    {
        //        SetTextCallback callback = new SetTextCallback(WriteOutputTextBox);
        //        this.Invoke(callback, new object[] { iText });
        //    }
        //    {
        //        output_TextBox.Text = iText;
        //        output_TextBox.ScrollToCaret();
        //    }
        //}

        #endregion

        private void stepProgressEventHandler(object sender, DataReceivedEventArgs e)
        {
            //msg: can be implemented
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////

        #region HANDLING FORM CONTROL EVENTS


        private void startReg_Button_Click(object sender, EventArgs e)
        {
            DeleteLogFolder();

            BackgroundWorker bwThread = new BackgroundWorker();
            bwThread.WorkerReportsProgress = true;
            bwThread.WorkerSupportsCancellation = true;

            bwThread.DoWork += bwThread_DoWork;
            bwThread.ProgressChanged += bwThread_ProgressChanged;
            bwThread.RunWorkerCompleted += bwThread_RunWorkerCompleted;

            stopWatch.Start();
            regression_Timer.Start();

            Properties.Internal.Default.RegressionTriggerDateTime = System.DateTime.Now.ToString("dd MMM, yyyy hh:mm tt");
            bwThread.RunWorkerAsync(); //Start the Thread

            startReg_Button.Enabled = false;

            //todo: delete the below code after the work is done.
            //TestCasesForm testCaseForm = new TestCasesForm();
            //testCaseForm.ShowDialog();

        }

        private void DeleteLogFolder()
        {
            //delete log folder  -- to be deleted or not to be, is the question.
            var logDir = new DirectoryInfo(Properties.Regression.Default.LogFolderPath);
            if (logDir.Exists)
            {
                log.Info("Deleting folder: " + logDir.FullName);
                logDir.Delete(true);
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////

        #region TIMER FUNCTIONS
        private void regression_Timer_Tick(object sender, EventArgs e)
        {
            elapsedTime_Label.Text = stopWatch.Elapsed.ToString(@"hh\:mm\:ss");
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////

        #region BACKGROUND THREAD FUNCTIONS
        private void bwThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            stopWatch.Stop();
            regression_Timer.Stop();
            startReg_Button.Visible = false;
            viewLog_Button.Visible = true;

            string isRegressionSystem = Environment.GetEnvironmentVariable("IsRegressionSystem", EnvironmentVariableTarget.Machine);
            if (isRegressionSystem != null && isRegressionSystem.ToLower().Equals("yes") && Properties.Debug.Default.AutoStartRegression)
            {
                this.Close();
            }
        }

        private void bwThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void bwThread_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (DataGridViewRow row in steps_DataGridView.Rows)
            {
                Step thisStep = row.DataBoundItem as Step;
                if(thisStep.Status == eStatus.READY)
                    thisStep.Execute(row);
            }

        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////

        private void TakeActiveWindowScreenShot()
        {
            System.Threading.Thread.Sleep(500); //so that the screen is in proper state.
            Rectangle bounds = this.Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }
                bitmap.Save("regression_status.png", ImageFormat.Png);
            }
        }

        private void viewLog_Button_Click(object sender, EventArgs e)
        {
            string logPath = Path.Combine(Properties.Regression.Default.LogFolderPath, "JarvisRegressionSuite.log");
            if (File.Exists(logPath))
                Process.Start(logPath);
            else
                MessageBox.Show("File Not Found: " + logPath, this.Text);
        }

        private void settings_Button_Click(object sender, EventArgs e)
        {
            PreferenceForm prefForm = new PreferenceForm();
            prefForm.ShowDialog();
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            if (Helper.AdminHelper.IsAdministrator() == false)
            {
                MessageBox.Show("Please run the application as Administrator.", this.Text);
                this.Close();
            }

            string isRegressionSystem = Environment.GetEnvironmentVariable("IsRegressionSystem", EnvironmentVariableTarget.Machine);
            if (isRegressionSystem != null && isRegressionSystem.ToLower().Equals("yes") && Properties.Debug.Default.AutoStartRegression)
            {
                startReg_Button.PerformClick();
            }
        }
    }
}
