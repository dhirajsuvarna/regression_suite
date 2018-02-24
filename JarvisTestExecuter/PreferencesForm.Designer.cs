namespace testing_cost
{
    partial class PreferencesForm
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
            this.user_PropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.save_Button = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // user_PropertyGrid
            // 
            this.user_PropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.user_PropertyGrid.LineColor = System.Drawing.SystemColors.ControlDark;
            this.user_PropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.user_PropertyGrid.Name = "user_PropertyGrid";
            this.user_PropertyGrid.Size = new System.Drawing.Size(746, 521);
            this.user_PropertyGrid.TabIndex = 0;
            // 
            // save_Button
            // 
            this.save_Button.Location = new System.Drawing.Point(295, 22);
            this.save_Button.Name = "save_Button";
            this.save_Button.Size = new System.Drawing.Size(87, 24);
            this.save_Button.TabIndex = 1;
            this.save_Button.Text = "Save";
            this.save_Button.UseVisualStyleBackColor = true;
            this.save_Button.Click += new System.EventHandler(this.save_Button_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.save_Button);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 521);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(746, 59);
            this.panel1.TabIndex = 2;
            // 
            // PreferencesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 580);
            this.Controls.Add(this.user_PropertyGrid);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PreferencesForm";
            this.Text = "Set User Preference";
            this.Load += new System.EventHandler(this.PreferencesForm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid user_PropertyGrid;
        private System.Windows.Forms.Button save_Button;
        private System.Windows.Forms.Panel panel1;
    }
}