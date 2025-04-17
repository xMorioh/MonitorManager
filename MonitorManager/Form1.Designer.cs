namespace MonitorManager
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ApplicationName_textbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.MonitorAmount_comboBox = new System.Windows.Forms.ComboBox();
            this.Get_Monitor_VCP_Button = new System.Windows.Forms.Button();
            this.Custom_winddcutil_Param_textbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Launch_Custom_Param_Button = new System.Windows.Forms.Button();
            this.winddcutil_Link_Label = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 30000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ApplicationName_textbox
            // 
            this.ApplicationName_textbox.Location = new System.Drawing.Point(124, 6);
            this.ApplicationName_textbox.Name = "ApplicationName_textbox";
            this.ApplicationName_textbox.Size = new System.Drawing.Size(298, 20);
            this.ApplicationName_textbox.TabIndex = 0;
            this.ApplicationName_textbox.TextChanged += new System.EventHandler(this.ApplicationName_textbox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Application to watch:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "How many Monitors do you have?:";
            // 
            // MonitorAmount_comboBox
            // 
            this.MonitorAmount_comboBox.FormattingEnabled = true;
            this.MonitorAmount_comboBox.Location = new System.Drawing.Point(189, 41);
            this.MonitorAmount_comboBox.Name = "MonitorAmount_comboBox";
            this.MonitorAmount_comboBox.Size = new System.Drawing.Size(55, 21);
            this.MonitorAmount_comboBox.TabIndex = 4;
            this.MonitorAmount_comboBox.SelectedIndexChanged += new System.EventHandler(this.MonitorAmount_comboBox_SelectedIndexChanged);
            // 
            // Get_Monitor_VCP_Button
            // 
            this.Get_Monitor_VCP_Button.Location = new System.Drawing.Point(327, 41);
            this.Get_Monitor_VCP_Button.Name = "Get_Monitor_VCP_Button";
            this.Get_Monitor_VCP_Button.Size = new System.Drawing.Size(95, 21);
            this.Get_Monitor_VCP_Button.TabIndex = 5;
            this.Get_Monitor_VCP_Button.Text = "Get Monitor VCP";
            this.Get_Monitor_VCP_Button.UseVisualStyleBackColor = true;
            this.Get_Monitor_VCP_Button.Click += new System.EventHandler(this.Get_Monitor_VCP_Button_Click);
            // 
            // Custom_winddcutil_Param_textbox
            // 
            this.Custom_winddcutil_Param_textbox.Location = new System.Drawing.Point(150, 74);
            this.Custom_winddcutil_Param_textbox.Name = "Custom_winddcutil_Param_textbox";
            this.Custom_winddcutil_Param_textbox.Size = new System.Drawing.Size(170, 20);
            this.Custom_winddcutil_Param_textbox.TabIndex = 6;
            this.Custom_winddcutil_Param_textbox.TextChanged += new System.EventHandler(this.Custom_winddcutil_Param_textbox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Custom winddcutil Param";
            // 
            // Launch_Custom_Param_Button
            // 
            this.Launch_Custom_Param_Button.Location = new System.Drawing.Point(327, 74);
            this.Launch_Custom_Param_Button.Name = "Launch_Custom_Param_Button";
            this.Launch_Custom_Param_Button.Size = new System.Drawing.Size(95, 20);
            this.Launch_Custom_Param_Button.TabIndex = 8;
            this.Launch_Custom_Param_Button.Text = "Launch";
            this.Launch_Custom_Param_Button.UseVisualStyleBackColor = true;
            this.Launch_Custom_Param_Button.Click += new System.EventHandler(this.Launch_Custom_Param_Button_Click);
            // 
            // winddcutil_Link_Label
            // 
            this.winddcutil_Link_Label.AutoSize = true;
            this.winddcutil_Link_Label.Location = new System.Drawing.Point(12, 104);
            this.winddcutil_Link_Label.Name = "winddcutil_Link_Label";
            this.winddcutil_Link_Label.Size = new System.Drawing.Size(281, 13);
            this.winddcutil_Link_Label.TabIndex = 9;
            this.winddcutil_Link_Label.TabStop = true;
            this.winddcutil_Link_Label.Text = "winddcutil Github Page: github.com/scottaxcell/winddcutil";
            this.winddcutil_Link_Label.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.winddcutil_Link_Label_LinkClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 126);
            this.Controls.Add(this.winddcutil_Link_Label);
            this.Controls.Add(this.Launch_Custom_Param_Button);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Custom_winddcutil_Param_textbox);
            this.Controls.Add(this.Get_Monitor_VCP_Button);
            this.Controls.Add(this.MonitorAmount_comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ApplicationName_textbox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(450, 165);
            this.MinimumSize = new System.Drawing.Size(450, 165);
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.Text = "Monitor Manager";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox ApplicationName_textbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox MonitorAmount_comboBox;
        private System.Windows.Forms.Button Get_Monitor_VCP_Button;
        private System.Windows.Forms.TextBox Custom_winddcutil_Param_textbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Launch_Custom_Param_Button;
        private System.Windows.Forms.LinkLabel winddcutil_Link_Label;
    }
}

