namespace testing_cost
{
    partial class TestCasesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.startTest_Button = new System.Windows.Forms.Button();
            this.testCase_DataGridView = new System.Windows.Forms.DataGridView();
            this.pause_Button = new System.Windows.Forms.Button();
            this.reset_Button = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.infoStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.elapsedTimeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainTimer = new System.Windows.Forms.Timer(this.components);
            this.header_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.settings_Button = new System.Windows.Forms.Button();
            this.inputFile_TextBox = new System.Windows.Forms.TextBox();
            this.browseTestApp_Button = new System.Windows.Forms.Button();
            this.searchPart_TextBox = new System.Windows.Forms.TextBox();
            this.testapp_TextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.browseInputFile_Button = new System.Windows.Forms.Button();
            this.main_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.bottom_Panel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.testCase_DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.header_TableLayoutPanel.SuspendLayout();
            this.main_TableLayoutPanel.SuspendLayout();
            this.bottom_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // startTest_Button
            // 
            this.startTest_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.startTest_Button.Location = new System.Drawing.Point(671, 27);
            this.startTest_Button.Name = "startTest_Button";
            this.startTest_Button.Size = new System.Drawing.Size(110, 40);
            this.startTest_Button.TabIndex = 0;
            this.startTest_Button.Text = "Start";
            this.startTest_Button.UseVisualStyleBackColor = true;
            this.startTest_Button.Click += new System.EventHandler(this.startTest_Button_Click);
            // 
            // testCase_DataGridView
            // 
            this.testCase_DataGridView.AllowUserToAddRows = false;
            this.testCase_DataGridView.AllowUserToDeleteRows = false;
            this.testCase_DataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.testCase_DataGridView.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.testCase_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.testCase_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testCase_DataGridView.Location = new System.Drawing.Point(13, 117);
            this.testCase_DataGridView.Name = "testCase_DataGridView";
            this.testCase_DataGridView.Size = new System.Drawing.Size(1508, 489);
            this.testCase_DataGridView.TabIndex = 5;
            this.testCase_DataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.testCase_DataGridView_CellContentClick);
            this.testCase_DataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.testCase_DataGridView_CellFormatting);
            this.testCase_DataGridView.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.testCase_DataGridView_ColumnAdded);
            // 
            // pause_Button
            // 
            this.pause_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pause_Button.Location = new System.Drawing.Point(671, 27);
            this.pause_Button.Name = "pause_Button";
            this.pause_Button.Size = new System.Drawing.Size(110, 40);
            this.pause_Button.TabIndex = 12;
            this.pause_Button.Text = "Pause";
            this.pause_Button.UseVisualStyleBackColor = true;
            this.pause_Button.Visible = false;
            this.pause_Button.Click += new System.EventHandler(this.pause_Button_Click);
            // 
            // reset_Button
            // 
            this.reset_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.reset_Button.Location = new System.Drawing.Point(671, 27);
            this.reset_Button.Name = "reset_Button";
            this.reset_Button.Size = new System.Drawing.Size(110, 40);
            this.reset_Button.TabIndex = 6;
            this.reset_Button.Text = "Reset";
            this.reset_Button.UseVisualStyleBackColor = true;
            this.reset_Button.Visible = false;
            this.reset_Button.Click += new System.EventHandler(this.reset_Button_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoStatusLabel,
            this.toolStripStatusLabel3,
            this.elapsedTimeLabel,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 686);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1534, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // infoStatusLabel
            // 
            this.infoStatusLabel.Name = "infoStatusLabel";
            this.infoStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(0, 17);
            // 
            // elapsedTimeLabel
            // 
            this.elapsedTimeLabel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 5);
            this.elapsedTimeLabel.Name = "elapsedTimeLabel";
            this.elapsedTimeLabel.Size = new System.Drawing.Size(0, 14);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel1.Text = "Ready";
            // 
            // mainTimer
            // 
            this.mainTimer.Interval = 500;
            this.mainTimer.Tick += new System.EventHandler(this.mainTimer_Tick);
            // 
            // header_TableLayoutPanel
            // 
            this.header_TableLayoutPanel.ColumnCount = 4;
            this.header_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.header_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.43204F));
            this.header_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.16309F));
            this.header_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.header_TableLayoutPanel.Controls.Add(this.label2, 0, 1);
            this.header_TableLayoutPanel.Controls.Add(this.label3, 0, 2);
            this.header_TableLayoutPanel.Controls.Add(this.settings_Button, 3, 2);
            this.header_TableLayoutPanel.Controls.Add(this.inputFile_TextBox, 1, 0);
            this.header_TableLayoutPanel.Controls.Add(this.browseTestApp_Button, 2, 1);
            this.header_TableLayoutPanel.Controls.Add(this.searchPart_TextBox, 1, 2);
            this.header_TableLayoutPanel.Controls.Add(this.testapp_TextBox, 1, 1);
            this.header_TableLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.header_TableLayoutPanel.Controls.Add(this.browseInputFile_Button, 2, 0);
            this.header_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.header_TableLayoutPanel.Location = new System.Drawing.Point(13, 13);
            this.header_TableLayoutPanel.Name = "header_TableLayoutPanel";
            this.header_TableLayoutPanel.RowCount = 3;
            this.header_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.header_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.header_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.header_TableLayoutPanel.Size = new System.Drawing.Size(1508, 98);
            this.header_TableLayoutPanel.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "TestApp Location";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "Search Part";
            // 
            // settings_Button
            // 
            this.settings_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.settings_Button.Location = new System.Drawing.Point(1430, 72);
            this.settings_Button.Name = "settings_Button";
            this.settings_Button.Size = new System.Drawing.Size(75, 23);
            this.settings_Button.TabIndex = 11;
            this.settings_Button.Text = "Settings";
            this.settings_Button.UseVisualStyleBackColor = true;
            this.settings_Button.Click += new System.EventHandler(this.settings_Button_Click);
            // 
            // inputFile_TextBox
            // 
            this.inputFile_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.inputFile_TextBox.Location = new System.Drawing.Point(113, 5);
            this.inputFile_TextBox.Name = "inputFile_TextBox";
            this.inputFile_TextBox.Size = new System.Drawing.Size(727, 21);
            this.inputFile_TextBox.TabIndex = 0;
            this.inputFile_TextBox.TextChanged += new System.EventHandler(this.inputFile_TextBox_TextChanged);
            // 
            // browseTestApp_Button
            // 
            this.browseTestApp_Button.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.browseTestApp_Button.Location = new System.Drawing.Point(863, 37);
            this.browseTestApp_Button.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.browseTestApp_Button.Name = "browseTestApp_Button";
            this.browseTestApp_Button.Size = new System.Drawing.Size(34, 21);
            this.browseTestApp_Button.TabIndex = 3;
            this.browseTestApp_Button.Text = "...";
            this.browseTestApp_Button.UseVisualStyleBackColor = true;
            this.browseTestApp_Button.Click += new System.EventHandler(this.browseTestApp_Button_Click);
            // 
            // searchPart_TextBox
            // 
            this.searchPart_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.searchPart_TextBox.Location = new System.Drawing.Point(113, 70);
            this.searchPart_TextBox.Name = "searchPart_TextBox";
            this.searchPart_TextBox.Size = new System.Drawing.Size(727, 21);
            this.searchPart_TextBox.TabIndex = 4;
            this.searchPart_TextBox.TextChanged += new System.EventHandler(this.searchPart_TextBox_TextChanged);
            // 
            // testapp_TextBox
            // 
            this.testapp_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.testapp_TextBox.Location = new System.Drawing.Point(113, 37);
            this.testapp_TextBox.Name = "testapp_TextBox";
            this.testapp_TextBox.Size = new System.Drawing.Size(727, 21);
            this.testapp_TextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Input File";
            // 
            // browseInputFile_Button
            // 
            this.browseInputFile_Button.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.browseInputFile_Button.Location = new System.Drawing.Point(863, 5);
            this.browseInputFile_Button.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.browseInputFile_Button.Name = "browseInputFile_Button";
            this.browseInputFile_Button.Size = new System.Drawing.Size(34, 21);
            this.browseInputFile_Button.TabIndex = 1;
            this.browseInputFile_Button.Text = "...";
            this.browseInputFile_Button.UseVisualStyleBackColor = true;
            this.browseInputFile_Button.Click += new System.EventHandler(this.browseInputFile_Button_Click);
            // 
            // main_TableLayoutPanel
            // 
            this.main_TableLayoutPanel.ColumnCount = 1;
            this.main_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.main_TableLayoutPanel.Controls.Add(this.header_TableLayoutPanel, 0, 0);
            this.main_TableLayoutPanel.Controls.Add(this.testCase_DataGridView, 0, 1);
            this.main_TableLayoutPanel.Controls.Add(this.bottom_Panel, 0, 2);
            this.main_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.main_TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.main_TableLayoutPanel.Name = "main_TableLayoutPanel";
            this.main_TableLayoutPanel.Padding = new System.Windows.Forms.Padding(10);
            this.main_TableLayoutPanel.RowCount = 3;
            this.main_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 104F));
            this.main_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.main_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.main_TableLayoutPanel.Size = new System.Drawing.Size(1534, 708);
            this.main_TableLayoutPanel.TabIndex = 14;
            // 
            // bottom_Panel
            // 
            this.bottom_Panel.Controls.Add(this.reset_Button);
            this.bottom_Panel.Controls.Add(this.pause_Button);
            this.bottom_Panel.Controls.Add(this.startTest_Button);
            this.bottom_Panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottom_Panel.Location = new System.Drawing.Point(13, 612);
            this.bottom_Panel.Name = "bottom_Panel";
            this.bottom_Panel.Size = new System.Drawing.Size(1508, 83);
            this.bottom_Panel.TabIndex = 1;
            // 
            // TestCasesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1534, 708);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.main_TableLayoutPanel);
            this.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(1482, 710);
            this.Name = "TestCasesForm";
            this.Text = "Jarvis Test Executer";
            this.Load += new System.EventHandler(this.TestCasesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.testCase_DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.header_TableLayoutPanel.ResumeLayout(false);
            this.header_TableLayoutPanel.PerformLayout();
            this.main_TableLayoutPanel.ResumeLayout(false);
            this.bottom_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button startTest_Button;
        private System.Windows.Forms.DataGridView testCase_DataGridView;
        private System.Windows.Forms.CheckBox selectAll_CheckBox;
        private System.Windows.Forms.Button pause_Button;
        private System.Windows.Forms.Button reset_Button;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel infoStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel elapsedTimeLabel;
        private System.Windows.Forms.Timer mainTimer;
        private System.Windows.Forms.TableLayoutPanel main_TableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel header_TableLayoutPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button settings_Button;
        private System.Windows.Forms.TextBox inputFile_TextBox;
        private System.Windows.Forms.Button browseTestApp_Button;
        private System.Windows.Forms.TextBox searchPart_TextBox;
        private System.Windows.Forms.Button browseInputFile_Button;
        private System.Windows.Forms.TextBox testapp_TextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel bottom_Panel;
    }
}