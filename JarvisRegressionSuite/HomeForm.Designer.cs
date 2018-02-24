namespace regression_test_suite
{
    partial class HomeForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.startReg_Button = new System.Windows.Forms.Button();
            this.steps_DataGridView = new System.Windows.Forms.DataGridView();
            this.output_TextBox = new System.Windows.Forms.TextBox();
            this.regression_Timer = new System.Windows.Forms.Timer(this.components);
            this.viewLog_Button = new System.Windows.Forms.Button();
            this.settings_Button = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.elapsedTime_Label = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.steps_DataGridView)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(403, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Regression Steps:";
            // 
            // startReg_Button
            // 
            this.startReg_Button.Location = new System.Drawing.Point(455, 525);
            this.startReg_Button.Margin = new System.Windows.Forms.Padding(4);
            this.startReg_Button.Name = "startReg_Button";
            this.startReg_Button.Size = new System.Drawing.Size(150, 40);
            this.startReg_Button.TabIndex = 3;
            this.startReg_Button.Text = "Start Regression";
            this.startReg_Button.UseVisualStyleBackColor = true;
            this.startReg_Button.Click += new System.EventHandler(this.startReg_Button_Click);
            // 
            // steps_DataGridView
            // 
            this.steps_DataGridView.AllowUserToAddRows = false;
            this.steps_DataGridView.AllowUserToDeleteRows = false;
            this.steps_DataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.steps_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.steps_DataGridView.Location = new System.Drawing.Point(27, 79);
            this.steps_DataGridView.Margin = new System.Windows.Forms.Padding(4);
            this.steps_DataGridView.Name = "steps_DataGridView";
            this.steps_DataGridView.ReadOnly = true;
            this.steps_DataGridView.Size = new System.Drawing.Size(1011, 421);
            this.steps_DataGridView.TabIndex = 4;
            // 
            // output_TextBox
            // 
            this.output_TextBox.Location = new System.Drawing.Point(27, 606);
            this.output_TextBox.Multiline = true;
            this.output_TextBox.Name = "output_TextBox";
            this.output_TextBox.ReadOnly = true;
            this.output_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.output_TextBox.Size = new System.Drawing.Size(1011, 159);
            this.output_TextBox.TabIndex = 5;
            // 
            // regression_Timer
            // 
            this.regression_Timer.Interval = 500;
            this.regression_Timer.Tick += new System.EventHandler(this.regression_Timer_Tick);
            // 
            // viewLog_Button
            // 
            this.viewLog_Button.Location = new System.Drawing.Point(455, 525);
            this.viewLog_Button.Margin = new System.Windows.Forms.Padding(4);
            this.viewLog_Button.Name = "viewLog_Button";
            this.viewLog_Button.Size = new System.Drawing.Size(150, 40);
            this.viewLog_Button.TabIndex = 11;
            this.viewLog_Button.Text = "View Log";
            this.viewLog_Button.UseVisualStyleBackColor = true;
            this.viewLog_Button.Visible = false;
            this.viewLog_Button.Click += new System.EventHandler(this.viewLog_Button_Click);
            // 
            // settings_Button
            // 
            this.settings_Button.Location = new System.Drawing.Point(963, 33);
            this.settings_Button.Name = "settings_Button";
            this.settings_Button.Size = new System.Drawing.Size(75, 23);
            this.settings_Button.TabIndex = 12;
            this.settings_Button.Text = "Settings";
            this.settings_Button.UseVisualStyleBackColor = true;
            this.settings_Button.Click += new System.EventHandler(this.settings_Button_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.elapsedTime_Label});
            this.statusStrip1.Location = new System.Drawing.Point(0, 807);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1069, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(88, 17);
            this.toolStripStatusLabel1.Text = "Elapsed Time:";
            // 
            // elapsedTime_Label
            // 
            this.elapsedTime_Label.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.elapsedTime_Label.Name = "elapsedTime_Label";
            this.elapsedTime_Label.Size = new System.Drawing.Size(59, 17);
            this.elapsedTime_Label.Text = "00:00:00";
            // 
            // HomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 829);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.settings_Button);
            this.Controls.Add(this.viewLog_Button);
            this.Controls.Add(this.output_TextBox);
            this.Controls.Add(this.steps_DataGridView);
            this.Controls.Add(this.startReg_Button);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "HomeForm";
            this.Text = "Jarvis - The Regression Test Suite";
            this.Load += new System.EventHandler(this.HomeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.steps_DataGridView)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button startReg_Button;
        private System.Windows.Forms.DataGridView steps_DataGridView;
        private System.Windows.Forms.TextBox output_TextBox;
        private System.Windows.Forms.Timer regression_Timer;
        private System.Windows.Forms.Button viewLog_Button;
        private System.Windows.Forms.Button settings_Button;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel elapsedTime_Label;
    }
}

