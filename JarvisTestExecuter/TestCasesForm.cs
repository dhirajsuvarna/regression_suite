﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace testing_cost
{
    public partial class TestCasesForm : Form
    {
        protected static readonly log4net.ILog testLog = log4net.LogManager.GetLogger("TestCasesForm");

        private EventWaitHandle waitHandle = new ManualResetEvent(true);
        BackgroundWorker bwThread = null;
        Stopwatch mainStopWatch = new Stopwatch();
        IsStartEnabled startEnableDecider = new IsStartEnabled();

        public string TestCaseFile { get; set; }
        public string DFMProBinLocation { get; set; }

        private bool InvokedFromCmdLine { get; set; }
        public bool IsRegressionSystem { get; set; }

        public TestCasesForm(bool isInvokedFromCmdLine = false)
        {
            InvokedFromCmdLine = isInvokedFromCmdLine;

            InitializeComponent();
            InitializeControls();
        
            //INITIALIZE BACKGROUNDWORKER THREAD
            bwThread = new BackgroundWorker();
            bwThread.DoWork += bwThread_DoWork;
            bwThread.ProgressChanged += bwThread_ProgressChanged;
            bwThread.RunWorkerCompleted += bwThread_RunWorkerCompleted;
            bwThread.WorkerReportsProgress = true;

        }

        private void InitializeControls()
        {
            this.inputFile_TextBox.Validating += inputFile_TextBox_Validating;
            this.testapp_TextBox.Validating += testapp_TextBox_Validating;

            this.testCase_DataGridView.CellPainting += testCase_DataGridView_CellPainting;
            this.testCase_DataGridView.CellValueChanged += testCase_DataGridView_CellValueChanged;
            this.testCase_DataGridView.CurrentCellDirtyStateChanged += testCase_DataGridView_CurrentCellDirtyStateChanged;
            this.testCase_DataGridView.DataBindingComplete += testCase_DataGridView_DataBindingComplete;
        }

        #region DATA GRID VIEW POPULATING AND EVENT HANDLING
        private void PopulateDataGridView()
        {
            try
            {
                testCase_DataGridView.Columns.Clear();
                testCase_DataGridView.DataSource = null;
                //a pre-requisite of Microsoft.ACE.OLEDB.12.0 is required for this below thing to work.
                //this is not installed by defualt when installing Office Products
                OleDbConnection connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + inputFile_TextBox.Text + ";Extended Properties='Excel 12.0 Xml;HDR=Yes'");
                connection.Open();

                OleDbDataAdapter dataAdapter = new OleDbDataAdapter("select * from [sheet1$]", connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                dataTable.Columns.Add(Constants.DGVColumnName.IsRequestFileDiff, typeof(Boolean));
                dataTable.Columns.Add(Constants.DGVColumnName.IsResponseFileDiff, typeof(Boolean));

                DataColumn selectPartCol = new DataColumn(Constants.DGVColumnName.SelectParts, typeof(System.Boolean));
                selectPartCol.DefaultValue = true;
                dataTable.Columns.Add(selectPartCol);
                selectPartCol.SetOrdinal(Constants.DGVColumnIndex.SelectParts);

                testCase_DataGridView.DataSource = dataTable;

                DataColumn statusColumn = new DataColumn(Constants.DGVColumnName.Status, System.Type.GetType("System.String"));
                statusColumn.DefaultValue = eStatus.READY.ToString();
                dataTable.Columns.Add(statusColumn);

                DataGridViewLinkColumn requestFileColumn = new DataGridViewLinkColumn();
                requestFileColumn.HeaderText = "Cost Request File";
                requestFileColumn.Name = Constants.DGVColumnName.CostRequestFile;
                testCase_DataGridView.Columns.Add(requestFileColumn);

                DataGridViewButtonColumn compareRequestColumn = new DataGridViewButtonColumn();
                compareRequestColumn.HeaderText = "Compare Request File";
                compareRequestColumn.Name = Constants.DGVColumnName.CompareRequestFile;
                compareRequestColumn.Text = "Compare";
                compareRequestColumn.UseColumnTextForButtonValue = true;
                testCase_DataGridView.Columns.Add(compareRequestColumn);

                DataGridViewLinkColumn responseFileColumn = new DataGridViewLinkColumn();
                responseFileColumn.HeaderText = "Cost Response File";
                responseFileColumn.Name = Constants.DGVColumnName.CostResponseFile;
                testCase_DataGridView.Columns.Add(responseFileColumn);

                DataGridViewButtonColumn compareResponseColumn = new DataGridViewButtonColumn();
                compareResponseColumn.HeaderText = "Compare Response File";
                compareResponseColumn.Name = Constants.DGVColumnName.CompareResponseFile;
                compareResponseColumn.Text = "Compare";
                compareResponseColumn.UseColumnTextForButtonValue = true;
                testCase_DataGridView.Columns.Add(compareResponseColumn);
                

                DataGridViewLinkColumn dfmResultFileColumn = new DataGridViewLinkColumn();
                dfmResultFileColumn.HeaderText = "DFM Results File";
                dfmResultFileColumn.Name = Constants.DGVColumnName.DFMResultsFile;
                //testCase_DataGridView.Columns.Add(dfmResultFileColumn);


                //DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn();
                //checkboxColumn.HeaderText = "";
                //checkboxColumn.Name = Constants.DGVColumnName.SelectParts;
                //checkboxColumn.TrueValue = true;
                //checkboxColumn.FalseValue = false;
                AddHeaderCheckBox();
                //testCase_DataGridView.Columns.Insert (0, checkboxColumn);

                DataGridViewTextBoxColumn timeColumn = new DataGridViewTextBoxColumn();
                timeColumn.HeaderText = "Approx Run Time";
                timeColumn.Name = Constants.DGVColumnName.AppoxRunTime;
                testCase_DataGridView.Columns.Add(timeColumn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text);
            }
        }

        void testCase_DataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            testCase_DataGridView.Columns[Constants.DGVColumnName.IsRequestFileDiff].Visible = false;
            testCase_DataGridView.Columns[Constants.DGVColumnName.IsResponseFileDiff].Visible = false;

        }

        private void testCase_DataGridView_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            //Set all checkboxes in the rows to CHECKED STATE
            if (e.Column.Name == Constants.DGVColumnName.SelectParts)
            {
                foreach (DataGridViewRow thisRow in testCase_DataGridView.Rows)
                {
                    DataGridViewCheckBoxCell thisCell = thisRow.Cells[Constants.DGVColumnName.SelectParts] as DataGridViewCheckBoxCell;
                    thisCell.Value = true;
                }
            }

            //MAKE ALL COLUMNS EXCEPT THE CHECKBOX COLUMN AS READONLY
            if (e.Column.Name != Constants.DGVColumnName.SelectParts)
            {
                e.Column.ReadOnly = true;
            }

            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable; //make all columns not sortable, since the unbound columns loose their state.
        }

        private void testCase_DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) //check if header is not clicked
            {
                if (testCase_DataGridView.Columns[e.ColumnIndex].Name.Equals(Constants.DGVColumnName.CostRequestFile) || testCase_DataGridView.Columns[e.ColumnIndex].Name.Equals(Constants.DGVColumnName.CostResponseFile))
                {
                    string fullFilePath = testCase_DataGridView[e.ColumnIndex, e.RowIndex].Value.ToString();
                    if (fullFilePath != null)
                    {
                        if (File.Exists(fullFilePath))
                            Process.Start(fullFilePath); //OPEN XML FILES
                        else
                            MessageBox.Show("File Not Found: " + fullFilePath, this.Text);
                    }
                }

                if(testCase_DataGridView.Columns[e.ColumnIndex].Name.Equals(Constants.DGVColumnName.SelectParts))
                {
                    DataGridViewCheckBoxCell checkboxCell = testCase_DataGridView[e.ColumnIndex, e.RowIndex] as DataGridViewCheckBoxCell;
                    if(checkboxCell.Value == null || checkboxCell.Value == checkboxCell.FalseValue)
                    {
                        testCase_DataGridView[Constants.DGVColumnName.Status, e.RowIndex].Value = eStatus.SKIPPED.ToString();
                    }
                    else
                    {
                        testCase_DataGridView[Constants.DGVColumnName.Status, e.RowIndex].Value = eStatus.READY.ToString();
                    }
                }

                if (testCase_DataGridView.Columns[e.ColumnIndex].Name.Equals(Constants.DGVColumnName.CompareRequestFile))
                {
                    DataGridViewLinkCell linkCell = testCase_DataGridView[Constants.DGVColumnName.CostRequestFile, e.RowIndex] as DataGridViewLinkCell;
                    if (linkCell.Value != null && linkCell.Value.ToString().Length > 0)
                    {
                        //MessageBox.Show("do comparision");
                        if (IsComparisionToolSupported())
                        {
                            PerfomComparision(linkCell.Value.ToString());
                        }
                        else
                        {
                            MessageBox.Show(string.Format("Diff Tool {0} not supported!", Properties.Location.Default.DiffTool), this.Text);
                        }

                    }
                }

                if (testCase_DataGridView.Columns[e.ColumnIndex].Name.Equals(Constants.DGVColumnName.CompareResponseFile))
                {
                    DataGridViewLinkCell linkCell = testCase_DataGridView[Constants.DGVColumnName.CostResponseFile, e.RowIndex] as DataGridViewLinkCell;
                    if (linkCell.Value != null && linkCell.Value.ToString().Length > 0)
                    {
                        if (IsComparisionToolSupported())
                        {
                            PerfomComparision(linkCell.Value.ToString());
                        }
                        else
                        {
                            MessageBox.Show(string.Format("Diff Tool {0} not supported!", Properties.Location.Default.DiffTool), this.Text);
                        }
                    }
                }

            }
        }

        private void PerfomComparision(string iFileName)
        {
            if (Properties.Location.Default.DiffTool == Constants.DiffTools.Name.Araxis)
            {
                if (!File.Exists(Constants.DiffTools.Exe.Araxis))
                    MessageBox.Show(string.Format("Diff Tool not found at {0}", Constants.DiffTools.Exe.Araxis), this.Text);
                else
                {
                    string baselineFile = iFileName.Replace(Path.GetFullPath(Properties.Location.Default.PartsFolder), Path.GetFullPath(Properties.Location.Default.BaselineFolder));
                    string arguments = baselineFile + " " + iFileName;
                    Process.Start(Constants.DiffTools.Exe.Araxis, arguments);
                }

            }
            else if (Properties.Location.Default.DiffTool == Constants.DiffTools.Name.WinMerge)
            {
                if (!File.Exists(Constants.DiffTools.Exe.WinMerge))
                    MessageBox.Show(string.Format("Diff Tool not found at {0}", Constants.DiffTools.Exe.Araxis), this.Text);
                else
                {
                    string baselineFile = iFileName.Replace(Path.GetFullPath(Properties.Location.Default.PartsFolder), Path.GetFullPath(Properties.Location.Default.BaselineFolder));
                    string arguments = baselineFile + " " + iFileName;
                    Process.Start(Constants.DiffTools.Exe.WinMerge, arguments);
                }
            }
        }

        private bool IsComparisionToolSupported()
        {
            bool toolExists = true;
            List<string> DiffToolList = new List<string>();
            DiffToolList.Add(Constants.DiffTools.Name.Araxis);
            DiffToolList.Add(Constants.DiffTools.Name.WinMerge);

            string diffTool = DiffToolList.Find(s => (s == Properties.Location.Default.DiffTool));
            if (diffTool == null)
            {
                toolExists = false;
            }
            return toolExists;
        }

        private void testCase_DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (testCase_DataGridView.Columns[e.ColumnIndex].Name.Equals(Constants.DGVColumnName.CostRequestFile))
            {
                if (e.Value != null)
                {
                    string fullFilePath = e.Value.ToString();
                    e.Value = Path.GetFileName(fullFilePath);
                    DataGridViewLinkCell thisCell = testCase_DataGridView[e.ColumnIndex, e.RowIndex] as DataGridViewLinkCell;
                    thisCell.ToolTipText = fullFilePath;

                    DataGridViewCell requestCell = testCase_DataGridView[Constants.DGVColumnName.IsRequestFileDiff, e.RowIndex];
                    if (requestCell.Value != null && requestCell.Value is bool)
                    {
                        if ((bool)requestCell.Value)
                            thisCell.LinkColor = Color.Red;
                        else
                            thisCell.LinkColor = Color.ForestGreen;
                    }
                }
            }

            if(testCase_DataGridView.Columns[e.ColumnIndex].Name.Equals(Constants.DGVColumnName.CostResponseFile))
            {
                if (e.Value != null)
                {
                    string fullFilePath = e.Value.ToString();
                    e.Value = Path.GetFileName(fullFilePath);
                    DataGridViewLinkCell thisCell = testCase_DataGridView[e.ColumnIndex, e.RowIndex] as DataGridViewLinkCell;
                    thisCell.ToolTipText = fullFilePath;
                    
                    DataGridViewCell responseCell = testCase_DataGridView[Constants.DGVColumnName.IsResponseFileDiff, e.RowIndex];
                    if (responseCell.Value != null && responseCell.Value is bool)
                    {
                        if ((bool)responseCell.Value)
                            thisCell.LinkColor = Color.Red;
                        else
                            thisCell.LinkColor = Color.ForestGreen;
                    }
                }
            }

            if (testCase_DataGridView.Columns[e.ColumnIndex].Name.Equals(Constants.DGVColumnName.Status))
            {
                DataGridViewRow thisRow = testCase_DataGridView.Rows[e.RowIndex];
                DataGridViewCell thisCell = testCase_DataGridView[e.ColumnIndex, e.RowIndex];
                if (thisCell.Value.ToString() == eStatus.EXECUTING.ToString())
                {
                    thisCell.Style.ForeColor = Color.Orange;
                    thisRow.DefaultCellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }
                else if (thisCell.Value.ToString() == eStatus.COMPLETED.ToString())
                {
                    thisCell.Style.ForeColor = Color.ForestGreen;
                    thisRow.DefaultCellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Regular);
                }
                else if (thisCell.Value.ToString() == eStatus.PAUSED.ToString())
                {
                    thisCell.Style.ForeColor = Color.Red;
                    thisRow.DefaultCellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }
                else if (thisCell.Value.ToString() == eStatus.FAILED.ToString())
                {
                    thisCell.Style.ForeColor = Color.Red;
                    thisRow.DefaultCellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Regular);
                }
                else
                {
                    thisCell.Style.ForeColor = Color.Black;
                    thisRow.DefaultCellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Regular);
                }
            }
        }

        #endregion

        /////////////////////////////////////////////////////////

        #region CREATING CHECKBOX COLUMN WITH CHECKBOX HEADER

        CheckBox headerCheckBox = null;
        private void AddHeaderCheckBox()
        {
            headerCheckBox = new CheckBox();
            headerCheckBox.ThreeState = true;
            headerCheckBox.Size = new Size(15, 15);
            headerCheckBox.CheckState = CheckState.Checked;


            headerCheckBox.Click += HeaderCheckBox_Click;
            testCase_DataGridView.Controls.Add(headerCheckBox);
        }
        private void ResetHeaderCheckBoxLocation(int ColumnIndex, int RowIndex)
        {
            //Get the column header cell bounds
            Rectangle oRectangle = this.testCase_DataGridView.GetCellDisplayRectangle(ColumnIndex, RowIndex, true);

            Point oPoint = new Point();

            oPoint.X = oRectangle.Location.X + (oRectangle.Width - headerCheckBox.Width) / 2 + 1;
            oPoint.Y = oRectangle.Location.Y + (oRectangle.Height - headerCheckBox.Height) / 2 + 1;

            //Change the location of the CheckBox to make it stay on the header
            headerCheckBox.Location = oPoint;
        }

        private void testCase_DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (headerCheckBox != null & (e.ColumnIndex == 0 && e.RowIndex == -1))
                ResetHeaderCheckBoxLocation(e.ColumnIndex, e.RowIndex);
        }

        private void HeaderCheckBox_Click(object sender, EventArgs e)
        {
            if (headerCheckBox.CheckState == CheckState.Checked)
            {
                foreach (DataGridViewRow thisRow in testCase_DataGridView.Rows)
                {
                    thisRow.Cells[Constants.DGVColumnName.SelectParts].Value = true;
                }
            }
            else if (headerCheckBox.CheckState == CheckState.Unchecked)
            {
                foreach (DataGridViewRow thisRow in testCase_DataGridView.Rows)
                {
                    thisRow.Cells[Constants.DGVColumnName.SelectParts].Value = false;
                }
            }
            else if (headerCheckBox.CheckState == CheckState.Indeterminate)
            {
                headerCheckBox.CheckState = CheckState.Unchecked;
                foreach (DataGridViewRow thisRow in testCase_DataGridView.Rows)
                {
                    thisRow.Cells[Constants.DGVColumnName.SelectParts].Value = false;
                }
            }

            testCase_DataGridView.RefreshEdit();
        }
        private void testCase_DataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (testCase_DataGridView.CurrentCell is DataGridViewCheckBoxCell)
            {
                testCase_DataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }

        }

        private void testCase_DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (testCase_DataGridView.Columns[e.ColumnIndex].Name.Equals(Constants.DGVColumnName.SelectParts))
            {
                int numofCheckCount = (from row in testCase_DataGridView.Rows.Cast<DataGridViewRow>()
                                       where row.Cells[Constants.DGVColumnName.SelectParts].Value != null &&
                                       (bool)row.Cells[Constants.DGVColumnName.SelectParts].Value == true
                                       select row).Count();

                if (numofCheckCount == testCase_DataGridView.RowCount)
                {
                    headerCheckBox.CheckState = CheckState.Checked;
                }
                else if (numofCheckCount == 0)
                {
                    headerCheckBox.CheckState = CheckState.Unchecked;
                }
                else
                {
                    headerCheckBox.CheckState = CheckState.Indeterminate;
                }
            }
        }


        #endregion

        /////////////////////////////////////////////////////////

        #region HANDLING EVENTS OF CONTROLS ON FORM
        private void TestCasesForm_Load(object sender, EventArgs e)
        {
            if (Helper.AdminHelper.IsAdministrator() == false)
            {
                MessageBox.Show("Please run the application as Administrator.", this.Text);
                this.Close();
                return;
            }

            //DEFINING DATA BINDINGS
            inputFile_TextBox.DataBindings.Add("Text", Properties.Location.Default, "SelectedTestFile");
            testapp_TextBox.DataBindings.Add("Text", Properties.Location.Default, "SelectedAppFile");
            if(!IsRegressionSystem)
                startTest_Button.DataBindings.Add("Enabled", startEnableDecider, "YesNo");

            //make search invisible
            label3.Visible = false;
            searchPart_TextBox.Visible = false;

            if (InvokedFromCmdLine) //this will happen only in case the app is launched through command line.
            {
                inputFile_TextBox.Text = TestCaseFile;
                testapp_TextBox.Text = DFMProBinLocation;
            }

            if (IsRegressionSystem)
                startTest_Button.PerformClick();
        }

        void testapp_TextBox_Validating(object sender, CancelEventArgs e)
        {
            if (testapp_TextBox.Text == String.Empty)
            {
                startEnableDecider.IsTestAppTextBoxValid = false;            
            }
            else if (!Path.GetFileName(testapp_TextBox.Text).ToLower().Equals("dfmproetestapplication.exe"))
            {
                errorProvider1.SetError(testapp_TextBox, "Not a valid DFMPro Test Application");
                startEnableDecider.IsTestAppTextBoxValid = false;
            }
            else if (!File.Exists(testapp_TextBox.Text))
            {
                errorProvider1.SetError(testapp_TextBox, "File does not exists");
                startEnableDecider.IsTestAppTextBoxValid = false;
            }
            else
            {
                errorProvider1.SetError(testapp_TextBox, "");
                startEnableDecider.IsTestAppTextBoxValid = true;
            }
        }

        void inputFile_TextBox_Validating(object sender, CancelEventArgs e)
        {
            if (inputFile_TextBox.Text == String.Empty)
            {
                startEnableDecider.IsInputFileTextBoxValid = false;                
            }
            else if (!Path.GetExtension(inputFile_TextBox.Text).ToLower().Equals(".xlsx"))
            {
                // Set the ErrorProvider error with the text to display. 
                errorProvider1.SetError(inputFile_TextBox, "Not a valid excel file");
                startEnableDecider.IsInputFileTextBoxValid = false;
            }
            else if (!File.Exists(inputFile_TextBox.Text))
            {
                errorProvider1.SetError(inputFile_TextBox, "File does not exists");
                startEnableDecider.IsInputFileTextBoxValid = false;
            }
            else
            {
                errorProvider1.SetError(inputFile_TextBox, "");
                startEnableDecider.IsInputFileTextBoxValid = true;
            }
        }

        private void browseInputFile_Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "*.xls, *.xlsx (Excel Files)|*.xls; *.xlsx";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                inputFile_TextBox.Text = fileDialog.FileName;
                inputFile_TextBox.Focus();
            }
        }

        private void browseTestApp_Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "(*.exe) Executable Files |*.exe";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                testapp_TextBox.Text = fileDialog.FileName;
                testapp_TextBox.Focus();
            }

        }

        private void inputFile_TextBox_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(inputFile_TextBox.Text))
            {
               PopulateDataGridView();
            }
        }

        private void startTest_Button_Click(object sender, EventArgs e)
        {
            startTest_Button.Visible = false;
            pause_Button.Visible = true;
            searchPart_TextBox.Enabled = false;
            mainTimer.Start();
            mainStopWatch.Start();

            waitHandle.Set();

            if(!bwThread.IsBusy)
                bwThread.RunWorkerAsync();
        }
        
        private void searchPart_TextBox_TextChanged(object sender, EventArgs e)
        {
            DataView dataView = (testCase_DataGridView.DataSource as DataTable).DefaultView;
            string searchQuery = string.Format("[" + Constants.ExcelColumnNames.PartNumber + "] LIKE '%{0}%'" , searchPart_TextBox.Text);
            dataView.RowFilter = searchQuery;
        }

        private void settings_Button_Click(object sender, EventArgs e)
        {
            PreferencesForm userPrefForm = new PreferencesForm();
            userPrefForm.ShowDialog();
        }

        private void pause_Button_Click(object sender, EventArgs e)
        {
            pause_Button.Enabled = false;
            searchPart_TextBox.Enabled = true;
            toolStripStatusLabel1.Text = "Attempting to Pause the test";
            waitHandle.Reset();
        }

        private void reset_Button_Click(object sender, EventArgs e)
        {
            //ask for the user to take backup.
            DialogResult dialogResult =  MessageBox.Show("Do you want to save the results?", this.Text, MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
                BackupResults();
           
            DeleteResults();

            //reset datagridview
            foreach (DataGridViewRow thisRow in testCase_DataGridView.Rows)
            {
                thisRow.Cells[Constants.DGVColumnName.SelectParts].Value = true;
                thisRow.Cells[Constants.DGVColumnName.Status].Value = eStatus.READY.ToString();
                thisRow.Cells[Constants.DGVColumnName.CostRequestFile].Value = "";
                thisRow.Cells[Constants.DGVColumnName.CostResponseFile].Value = "";
            }
            testCase_DataGridView.RefreshEdit();

            //reset buttons
            reset_Button.Visible = false;
            startTest_Button.Visible = true;

            //reset timers
            mainStopWatch.Reset();
        }


        #endregion

        /////////////////////////////////////////////////////////

        #region BACKGROUND THREAD FUNCTIONS
        void bwThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            startTest_Button.Visible = false;
            pause_Button.Visible = false;
            reset_Button.Visible = true;
            searchPart_TextBox.Enabled = true;
            toolStripStatusLabel1.Text = "All Tests Completed";

            mainStopWatch.Stop();
            mainTimer.Stop();

            Properties.Location.Default.Save(); //this is not working check why.
            if (IsRegressionSystem)
            {
                this.Close();
            }
        }

        void bwThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        void bwThread_DoWork(object sender, DoWorkEventArgs e)
        {
            //copy the sc_vs_creocost report from template to working dir
            CopyComparisionTemplate();

            //register dlls in the local bin path - only for Local Regression
            if (!IsRegressionSystem)
            {
                RegisterBinaries();

                EditRegistry();
            }

            //for each of the test case execute DFMProTestApplication.
            foreach (DataGridViewRow thisRow in testCase_DataGridView.Rows)
            {
                try
                {
                    if ((bool)thisRow.Cells[Constants.DGVColumnName.SelectParts].Value == true)
                    {
                        PauseExecute(!waitHandle.WaitOne(0), thisRow);

                        this.Invoke((MethodInvoker)delegate
                        {
                            thisRow.Cells[Constants.DGVColumnName.Status].Value = eStatus.EXECUTING.ToString();
                        });

                        AutoScrollDataGridView(thisRow);

                        string partNumber = thisRow.Cells[Constants.ExcelColumnNames.PartNumber].Value.ToString(); //Using "Part No" can break the code.
                        string relativeLocation = thisRow.Cells[Constants.ExcelColumnNames.RelativeLocation].Value.ToString();

                        string partPath = Path.GetFullPath(Path.Combine(Properties.Location.Default.PartsFolder, relativeLocation, partNumber));
                        string dfmMaterial = thisRow.Cells[Constants.ExcelColumnNames.DFMMaterial].Value.ToString();
                        string processType = thisRow.Cells[Constants.ExcelColumnNames.Process].Value.ToString();

                        string timeCSVPath = null;
                        if (processType == Constants.ProcessType.Mill)
                            timeCSVPath = Path.GetFullPath(Path.Combine(Properties.Location.Default.PartsFolder, "mill.csv"));
                        else if (processType == Constants.ProcessType.SM)
                            timeCSVPath = Path.GetFullPath(Path.Combine(Properties.Location.Default.PartsFolder, "sm.csv"));
                        else
                            timeCSVPath = Path.GetFullPath(Path.Combine(Properties.Location.Default.PartsFolder, "im.csv"));

                        string ruleFilePath = Path.GetFullPath(Path.Combine(Properties.Location.Default.RuleFilePath, "AllRules.dfm"));
                        string processName = "\"" + testapp_TextBox.Text + "\"";
                        string arguments = ruleFilePath + " " + processType + " \"" + dfmMaterial + "\" " + partPath + " " + timeCSVPath;

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            toolStripStatusLabel1.Text = string.Format("Processing part: {0}", partNumber);
                        });

                        testLog.Info("Process: " + processName);
                        testLog.Info("Arguments: " + arguments);
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        Helper.ProcessHelper procHelper = new Helper.ProcessHelper();
                        procHelper.dataReceivedHandler = dataReceivedHandle;
                        procHelper.errorReceivedHandler = errorReceivedHandle;
                        procHelper.EnviornmentPath = Path.Combine(GetBinariesRootPath(), "costmodule"); //@"C:\Program Files\Geometric\DFMPro for Creo Parametric(x64)\costmodule";
                        procHelper.Execute(processName, arguments);
                        sw.Stop();

                        string approxRunTime = sw.Elapsed.ToString(@"hh\:mm\:ss");
                        testLog.Info("ApproxRunTime: " + approxRunTime);
                        this.Invoke((MethodInvoker)delegate
                        {
                            thisRow.Cells[Constants.DGVColumnName.AppoxRunTime].Value = approxRunTime;
                        });

                        string requestFileName = Path.Combine(Path.GetDirectoryName(partPath), Path.GetFileName(partPath) + "-Request.xml");
                        string responseFileName = Path.Combine(Path.GetDirectoryName(partPath), Path.GetFileName(partPath) + "-Response.xml");

                        //POST PROCESS RESPONSE XML TO GENERATE ACCURACY EXCEL
                        string isShouldCostPresent = thisRow.Cells[Constants.ExcelColumnNames.ShouldCostAvailable].Value.ToString();
                        if (isShouldCostPresent.ToLower() == "yes" && File.Exists(responseFileName))
                        {
                            //::refactor:: the below code is not extensible, should be closed for modifications
                            string shouldCostReport = Path.Combine(Properties.Location.Default.ReportPath, "SC_vs_CreoCost_Comparision_Report.xlsx");
                            if (processType != Constants.ProcessType.IM)
                            {
                                ResponseData responseData = new ResponseData();
                                ResponseXMLReader xmlReader = new ResponseXMLReader(responseData);
                                xmlReader.Read(responseFileName);

                                responseData.PartNumber = Path.GetFileName(partPath);  //this should not be done. this is done because, the part number in the response xml file has NT id appended.
                                ShouldCostReportPopulator reportPopulator = new ShouldCostReportPopulator(responseData);
                                reportPopulator.Populate(Path.GetFullPath(shouldCostReport));
                            }
                            else
                            {
                                IMResponseData responseData = new IMResponseData();
                                IMResponseReader xmlReader = new IMResponseReader(responseData);
                                xmlReader.Read(responseFileName);

                                responseData.PartNumber = Path.GetFileName(partPath);  //::refactor:: this should not be done. this is done because, the part number in the response xml file has NT id appended.
                                IMShouldCostReportPopulator reportPopulator = new IMShouldCostReportPopulator(responseData);
                                reportPopulator.Populate(Path.GetFullPath(shouldCostReport));
                            }
                        }

                        //CHECK IF THE REQUEST AND RESPONSE FILE IS CREATED.
                        if (File.Exists(requestFileName))
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                thisRow.Cells[Constants.DGVColumnName.CostRequestFile].Value = requestFileName;
                            });
                        }

                        if (File.Exists(responseFileName))
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                thisRow.Cells[Constants.DGVColumnName.CostResponseFile].Value = responseFileName;
                            });
                        }

                        //COMPARE WITH BASELINE AND SEE IF THERE ARE ANY DIFF
                        int isRequestFileDiff = CompareFiles(requestFileName);
                        int isResponseFileDiff = CompareFiles(responseFileName);
                        this.Invoke((MethodInvoker) delegate
                        {

                            thisRow.Cells[Constants.DGVColumnName.IsRequestFileDiff].Value = isRequestFileDiff;
                            thisRow.Cells[Constants.DGVColumnName.IsResponseFileDiff].Value = isResponseFileDiff;

                            thisRow.Cells[Constants.DGVColumnName.Status].Value = eStatus.COMPLETED.ToString();
                        });
                    }
                }
                catch (Exception ex)
                {
                    testLog.Error("Exception", ex);
                    thisRow.Cells[Constants.DGVColumnName.Status].Value = eStatus.FAILED.ToString();
                }
            }
        }

        private void EditRegistry()
        {
            string processName = "regedit.exe";
            string arguments = "/s " + Path.GetFullPath(@"..\..\..\..\reg\DFMProInterfaceOpenRegEntry.reg");

            testLog.Info(processName);
            testLog.Info(arguments);
            Helper.ProcessHelper procHelper = new Helper.ProcessHelper();
            int exitCode = procHelper.Execute(processName, arguments);
        }

        private void RegisterBinaries()
        {
            Helper.ProcessHelper procHelper = new Helper.ProcessHelper();
            procHelper.dataReceivedHandler = dataReceivedHandle;
            procHelper.errorReceivedHandler = errorReceivedHandle;

            //register costguiaddon
            string costguiaddonPath = Path.Combine(GetBinariesRootPath(), "costmodule", "costguiaddon.dll");
            string argument1 = "\"" + costguiaddonPath + "\" /codebase";
            testLog.Info("Register costguiaddon.dll");
            testLog.Info(costguiaddonPath + " " + argument1);
            procHelper.Execute(Constants.AppPath.RegAsm64, argument1);

            //register mti dlls
            string mtfeaturenetPath = Path.Combine(GetBinariesRootPath(), "costmodule", "MTFeatureNet.Interop.dll");
            string argumnet2 = "\"" + mtfeaturenetPath + "\" /codebase";
            testLog.Info("Register MTFeatureNet.Interop.dll");
            testLog.Info(mtfeaturenetPath + " " + argumnet2);
            procHelper.Execute(Constants.AppPath.RegAsm64, argumnet2);

            string mtspreadnetPath = Path.Combine(GetBinariesRootPath(), "costmodule", "MTSpreadNet.Interop.dll");
            string argument3 = "\"" + mtspreadnetPath + "\" /codebase";
            testLog.Info("Register MTSpreadNet.Interop.dll");
            testLog.Info(mtspreadnetPath + " " + argument3);
            procHelper.Execute(Constants.AppPath.RegAsm64, argument3);

            //register dfmproserver
            string dfmproserverPath = Path.Combine(GetBinariesRootPath(), "dfmpro", "DFMProServerProEu.exe");
            //string argument4 = "\"" + dfmproserverPath + "\" /RegServer";
            dfmproserverPath = "\"" + dfmproserverPath + "\"";
            string argument4 = "/RegServer";
            testLog.Info("Register DFMProServerProEu.exe");
            testLog.Info(dfmproserverPath + " " + argument4);
            procHelper.Execute(dfmproserverPath, argument4);
        }

        private void CopyComparisionTemplate()
        {
            string shouldCostTemplate = Path.Combine(Properties.Location.Default.TemplatePath, "SC_vs_CreoCost_Comparision_Report.xlsx");
            string shouldCostReport = Path.Combine(Properties.Location.Default.ReportPath, "SC_vs_CreoCost_Comparision_Report.xlsx");
            if (File.Exists(shouldCostTemplate))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(shouldCostReport));
                File.Copy(shouldCostTemplate, shouldCostReport, true);
                testLog.Info(string.Format("File Copied from {0} to {1}", shouldCostTemplate, shouldCostReport));
            }
            else
            {
                testLog.Info(shouldCostTemplate + "does not exists");
            }
        }

        private void AutoScrollDataGridView(DataGridViewRow iRow)
        {
            int firstDisplayed = testCase_DataGridView.FirstDisplayedScrollingRowIndex;
            int numDisplayed = testCase_DataGridView.DisplayedRowCount(false);
            int lastDispalyed = (firstDisplayed + numDisplayed) - 1;

            if (iRow.Index > lastDispalyed)
                testCase_DataGridView.FirstDisplayedScrollingRowIndex = firstDisplayed + 1;
        }

        private void PauseExecute(bool isPaused, DataGridViewRow iRow)
        {
            if (isPaused)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    iRow.Cells[Constants.DGVColumnName.Status].Value = eStatus.PAUSED.ToString();
                    toolStripStatusLabel1.Text = "Paused";
                    startTest_Button.Visible = true;
                    pause_Button.Visible = false;
                    pause_Button.Enabled = true;

                    mainTimer.Stop();
                    mainStopWatch.Stop();
                });
                
                waitHandle.WaitOne();
            }
        }

        //Returns Zero if the files are same 
        private int CompareFiles(string iRequestFileName)
        {
            string absolutePartsFolder = Path.GetFullPath(Properties.Location.Default.PartsFolder);
            string absoluteBaselineFolder = Path.GetFullPath(Properties.Location.Default.BaselineFolder);
            string baselineFile = iRequestFileName.Replace(absolutePartsFolder, absoluteBaselineFolder);

            //using fc.exe utility from command prompt so as to avoid any bugs while implementing our own utility
            //arguments /w => Commpress white space (this is, tabs and spaces) during the comparision.
            //https://ss64.com/nt/fc.html
            //
            string processName = "fc.exe";
            string arguments = "/w " + iRequestFileName + " " + baselineFile;
            testLog.Info("Process: " + processName);
            testLog.Info("Arguments: " + arguments);

            Helper.ProcessHelper procHelper = new Helper.ProcessHelper();
            procHelper.dataReceivedHandler = dataReceivedHandle;
            procHelper.errorReceivedHandler = errorReceivedHandle;
            int exitCode = procHelper.Execute(processName, arguments);

            if (!(exitCode == 0 || exitCode == 1))
            {
                testLog.Error(processName + " exited with error code: " + exitCode);
                throw new Exception();
            }
            return exitCode;
        }

        private void dataReceivedHandle(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                testLog.Info(e.Data);
            }
        }

        private void errorReceivedHandle(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                testLog.Error(e.Data);
            }
        }

        #endregion

        private void BackupResults()
        {
            string partsFolder = Properties.Location.Default.PartsFolder;
            string backupFolder = Path.Combine(Properties.Location.Default.BackupFolderPath, System.DateTime.Now.ToString("dd-MMM-yyyy_HH_mm"));
            var filesToBackup = Directory.EnumerateFiles(partsFolder, "*.*", SearchOption.AllDirectories).Where(file => file.ToLower().EndsWith(".xml") || file.ToLower().EndsWith(".dfmr") || file.ToLower().EndsWith(".dfmresult"));
            foreach (var thisFile in filesToBackup)
            {
                string destinationFile = thisFile.Replace(partsFolder, backupFolder);
                Directory.CreateDirectory(Path.GetDirectoryName(destinationFile));

                testLog.Info("Copy " + thisFile + "to " + destinationFile);
                File.Copy(thisFile, destinationFile, true);
            }

            infoStatusLabel.Text = "Test Results saved at " + backupFolder;
        }

        private string GetBinariesRootPath()
        {
            string rootPath = null;
            if (testapp_TextBox.Text != null)
            {
                rootPath = Path.GetDirectoryName(Path.GetDirectoryName(testapp_TextBox.Text));
            }
            return rootPath;
        }


        private void DeleteResults()
        {
            string partsFolder = Properties.Location.Default.PartsFolder;
            var filesToBeDeleted = Directory.EnumerateFiles(partsFolder, "*.*", SearchOption.AllDirectories).Where(file => file.ToLower().EndsWith(".xml") || file.ToLower().EndsWith(".dfmr") || file.ToLower().EndsWith(".dfmresult"));
            foreach (var file in filesToBeDeleted)
            {
                File.Delete(file);
            }
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            elapsedTimeLabel.Text = mainStopWatch.Elapsed.ToString(@"hh\:mm\:ss");
        }
    }

    class IsStartEnabled : INotifyPropertyChanged
    {
        public IsStartEnabled()
        { 
        
        }
        
        private bool isInputFileTextBoxValid;
        public bool IsInputFileTextBoxValid 
        {
            get
            {
                return isInputFileTextBoxValid; 
            }
            set
            {
                isInputFileTextBoxValid = value;
                YesNo = (isInputFileTextBoxValid && isTestAppTextBoxValid);
            }
        }

        private bool isTestAppTextBoxValid;
        public bool IsTestAppTextBoxValid
        {
            get
            {
                return isTestAppTextBoxValid;
            }
            set
            {
                isTestAppTextBoxValid = value;
                YesNo = (isInputFileTextBoxValid && isTestAppTextBoxValid);
            }
        }

        private bool isStartEnabled;
        public bool YesNo
        {
            get
            { 
                return isStartEnabled; 
            }
            set
            {
                isStartEnabled = value;
                NotifyPropertyChanged("YesNo");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
