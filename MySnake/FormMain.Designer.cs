namespace MySnake
{
    partial class FormMain
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
            this.btnStartSnake = new System.Windows.Forms.Button();
            this.MainTimer = new System.Windows.Forms.Timer(this.components);
            this.NumericSpeed = new System.Windows.Forms.NumericUpDown();
            this.TableBotStatus = new System.Windows.Forms.ListView();
            this.colNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colController = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRestart = new System.Windows.Forms.Button();
            this.GraphicControl = new OpenTK.GLControl();
            this.lblGameSpeed = new System.Windows.Forms.Label();
            this.lblBotCount = new System.Windows.Forms.Label();
            this.NumericBotCount = new System.Windows.Forms.NumericUpDown();
            this.CheckHasPlayer = new System.Windows.Forms.CheckBox();
            this.GroupDisplay = new System.Windows.Forms.GroupBox();
            this.CheckShowObstacleBorder = new System.Windows.Forms.CheckBox();
            this.CheckShowDeadSnake = new System.Windows.Forms.CheckBox();
            this.CheckStartPlayerSnake = new System.Windows.Forms.CheckBox();
            this.CheckStartBotSnake = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.NumericSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericBotCount)).BeginInit();
            this.GroupDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartSnake
            // 
            this.btnStartSnake.Location = new System.Drawing.Point(1040, 119);
            this.btnStartSnake.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartSnake.Name = "btnStartSnake";
            this.btnStartSnake.Size = new System.Drawing.Size(112, 34);
            this.btnStartSnake.TabIndex = 5;
            this.btnStartSnake.Text = "Start";
            this.btnStartSnake.UseVisualStyleBackColor = true;
            this.btnStartSnake.Click += new System.EventHandler(this.btnStartSnakeMove_Click);
            // 
            // MainTimer
            // 
            this.MainTimer.Enabled = true;
            this.MainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
            // 
            // NumericSpeed
            // 
            this.NumericSpeed.Location = new System.Drawing.Point(1040, 27);
            this.NumericSpeed.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NumericSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericSpeed.Name = "NumericSpeed";
            this.NumericSpeed.Size = new System.Drawing.Size(112, 27);
            this.NumericSpeed.TabIndex = 9;
            this.NumericSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericSpeed.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericSpeed.ValueChanged += new System.EventHandler(this.NumericSpeed_ValueChanged);
            // 
            // TableBotStatus
            // 
            this.TableBotStatus.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colNumber,
            this.colController,
            this.colLength,
            this.colStatus});
            this.TableBotStatus.FullRowSelect = true;
            this.TableBotStatus.GridLines = true;
            this.TableBotStatus.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.TableBotStatus.HideSelection = false;
            this.TableBotStatus.Location = new System.Drawing.Point(878, 160);
            this.TableBotStatus.Name = "TableBotStatus";
            this.TableBotStatus.Size = new System.Drawing.Size(400, 220);
            this.TableBotStatus.TabIndex = 11;
            this.TableBotStatus.UseCompatibleStateImageBehavior = false;
            this.TableBotStatus.View = System.Windows.Forms.View.Details;
            // 
            // colNumber
            // 
            this.colNumber.Text = "#";
            this.colNumber.Width = 30;
            // 
            // colController
            // 
            this.colController.Text = "Controller";
            this.colController.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colController.Width = 150;
            // 
            // colLength
            // 
            this.colLength.Text = "Length";
            this.colLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colLength.Width = 100;
            // 
            // colStatus
            // 
            this.colStatus.Text = "Status";
            this.colStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colStatus.Width = 100;
            // 
            // btnRestart
            // 
            this.btnRestart.Location = new System.Drawing.Point(1166, 119);
            this.btnRestart.Margin = new System.Windows.Forms.Padding(4);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(112, 34);
            this.btnRestart.TabIndex = 12;
            this.btnRestart.Text = "Reset";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // GraphicControl
            // 
            this.GraphicControl.BackColor = System.Drawing.Color.Black;
            this.GraphicControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.GraphicControl.Location = new System.Drawing.Point(13, 13);
            this.GraphicControl.Margin = new System.Windows.Forms.Padding(4);
            this.GraphicControl.Name = "GraphicControl";
            this.GraphicControl.Size = new System.Drawing.Size(845, 845);
            this.GraphicControl.TabIndex = 13;
            this.GraphicControl.VSync = false;
            // 
            // lblGameSpeed
            // 
            this.lblGameSpeed.Location = new System.Drawing.Point(877, 27);
            this.lblGameSpeed.Name = "lblGameSpeed";
            this.lblGameSpeed.Size = new System.Drawing.Size(157, 27);
            this.lblGameSpeed.TabIndex = 14;
            this.lblGameSpeed.Text = "Game speed:";
            this.lblGameSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBotCount
            // 
            this.lblBotCount.Location = new System.Drawing.Point(877, 60);
            this.lblBotCount.Name = "lblBotCount";
            this.lblBotCount.Size = new System.Drawing.Size(157, 27);
            this.lblBotCount.TabIndex = 15;
            this.lblBotCount.Text = "Bot count:";
            this.lblBotCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NumericBotCount
            // 
            this.NumericBotCount.Location = new System.Drawing.Point(1040, 60);
            this.NumericBotCount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NumericBotCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericBotCount.Name = "NumericBotCount";
            this.NumericBotCount.Size = new System.Drawing.Size(112, 27);
            this.NumericBotCount.TabIndex = 16;
            this.NumericBotCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericBotCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericBotCount.ValueChanged += new System.EventHandler(this.NumericBotCount_ValueChanged);
            // 
            // CheckHasPlayer
            // 
            this.CheckHasPlayer.AutoSize = true;
            this.CheckHasPlayer.Location = new System.Drawing.Point(1168, 63);
            this.CheckHasPlayer.Name = "CheckHasPlayer";
            this.CheckHasPlayer.Size = new System.Drawing.Size(110, 23);
            this.CheckHasPlayer.TabIndex = 17;
            this.CheckHasPlayer.Text = "Has player?";
            this.CheckHasPlayer.UseVisualStyleBackColor = true;
            this.CheckHasPlayer.CheckedChanged += new System.EventHandler(this.CheckHasPlayer_CheckedChanged);
            // 
            // GroupDisplay
            // 
            this.GroupDisplay.Controls.Add(this.CheckShowObstacleBorder);
            this.GroupDisplay.Controls.Add(this.CheckShowDeadSnake);
            this.GroupDisplay.Location = new System.Drawing.Point(878, 386);
            this.GroupDisplay.Name = "GroupDisplay";
            this.GroupDisplay.Size = new System.Drawing.Size(394, 92);
            this.GroupDisplay.TabIndex = 18;
            this.GroupDisplay.TabStop = false;
            this.GroupDisplay.Text = "Display";
            // 
            // CheckShowObstacleBorder
            // 
            this.CheckShowObstacleBorder.AutoSize = true;
            this.CheckShowObstacleBorder.Checked = true;
            this.CheckShowObstacleBorder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckShowObstacleBorder.Location = new System.Drawing.Point(6, 55);
            this.CheckShowObstacleBorder.Name = "CheckShowObstacleBorder";
            this.CheckShowObstacleBorder.Size = new System.Drawing.Size(189, 23);
            this.CheckShowObstacleBorder.TabIndex = 20;
            this.CheckShowObstacleBorder.Text = "Show obstacle border?";
            this.CheckShowObstacleBorder.UseVisualStyleBackColor = true;
            // 
            // CheckShowDeadSnake
            // 
            this.CheckShowDeadSnake.AutoSize = true;
            this.CheckShowDeadSnake.Location = new System.Drawing.Point(6, 26);
            this.CheckShowDeadSnake.Name = "CheckShowDeadSnake";
            this.CheckShowDeadSnake.Size = new System.Drawing.Size(159, 23);
            this.CheckShowDeadSnake.TabIndex = 19;
            this.CheckShowDeadSnake.Text = "Show dead snake?";
            this.CheckShowDeadSnake.UseVisualStyleBackColor = true;
            // 
            // CheckStartPlayerSnake
            // 
            this.CheckStartPlayerSnake.Checked = true;
            this.CheckStartPlayerSnake.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckStartPlayerSnake.Location = new System.Drawing.Point(881, 93);
            this.CheckStartPlayerSnake.Name = "CheckStartPlayerSnake";
            this.CheckStartPlayerSnake.Size = new System.Drawing.Size(165, 27);
            this.CheckStartPlayerSnake.TabIndex = 21;
            this.CheckStartPlayerSnake.Text = "Move player snake";
            this.CheckStartPlayerSnake.UseVisualStyleBackColor = true;
            this.CheckStartPlayerSnake.CheckedChanged += new System.EventHandler(this.CheckStartPlayerSnake_CheckedChanged);
            // 
            // CheckStartBotSnake
            // 
            this.CheckStartBotSnake.Checked = true;
            this.CheckStartBotSnake.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckStartBotSnake.Location = new System.Drawing.Point(881, 126);
            this.CheckStartBotSnake.Name = "CheckStartBotSnake";
            this.CheckStartBotSnake.Size = new System.Drawing.Size(153, 27);
            this.CheckStartBotSnake.TabIndex = 22;
            this.CheckStartBotSnake.Text = "Move bot snake";
            this.CheckStartBotSnake.UseVisualStyleBackColor = true;
            this.CheckStartBotSnake.CheckedChanged += new System.EventHandler(this.CheckStartBotSnake_CheckedChanged);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1290, 871);
            this.Controls.Add(this.CheckStartBotSnake);
            this.Controls.Add(this.CheckStartPlayerSnake);
            this.Controls.Add(this.GroupDisplay);
            this.Controls.Add(this.CheckHasPlayer);
            this.Controls.Add(this.NumericBotCount);
            this.Controls.Add(this.lblBotCount);
            this.Controls.Add(this.lblGameSpeed);
            this.Controls.Add(this.GraphicControl);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.TableBotStatus);
            this.Controls.Add(this.NumericSpeed);
            this.Controls.Add(this.btnStartSnake);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMain";
            this.Text = "My Snake";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.NumericSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericBotCount)).EndInit();
            this.GroupDisplay.ResumeLayout(false);
            this.GroupDisplay.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnStartSnake;
        private System.Windows.Forms.Timer MainTimer;
        private System.Windows.Forms.NumericUpDown NumericSpeed;
        private System.Windows.Forms.ListView TableBotStatus;
        private System.Windows.Forms.ColumnHeader colNumber;
        private System.Windows.Forms.ColumnHeader colLength;
        private System.Windows.Forms.Button btnRestart;
        private OpenTK.GLControl GraphicControl;
        private System.Windows.Forms.ColumnHeader colController;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.Label lblGameSpeed;
        private System.Windows.Forms.Label lblBotCount;
        private System.Windows.Forms.NumericUpDown NumericBotCount;
        private System.Windows.Forms.CheckBox CheckHasPlayer;
        private System.Windows.Forms.GroupBox GroupDisplay;
        private System.Windows.Forms.CheckBox CheckShowDeadSnake;
        private System.Windows.Forms.CheckBox CheckShowObstacleBorder;
        private System.Windows.Forms.CheckBox CheckStartPlayerSnake;
        private System.Windows.Forms.CheckBox CheckStartBotSnake;
    }
}

