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
            this.btnStartSnakeMoving = new System.Windows.Forms.Button();
            this.lblPlayer_Score = new System.Windows.Forms.Label();
            this.MainTimer = new System.Windows.Forms.Timer(this.components);
            this.btnFindPath = new System.Windows.Forms.Button();
            this.NumericSpeed = new System.Windows.Forms.NumericUpDown();
            this.TableBotStatus = new System.Windows.Forms.ListView();
            this.clName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clPoint = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRestart = new System.Windows.Forms.Button();
            this.GraphicControl = new OpenTK.GLControl();
            ((System.ComponentModel.ISupportInitialize)(this.NumericSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStartSnakeMoving
            // 
            this.btnStartSnakeMoving.Location = new System.Drawing.Point(881, 139);
            this.btnStartSnakeMoving.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartSnakeMoving.Name = "btnStartSnakeMoving";
            this.btnStartSnakeMoving.Size = new System.Drawing.Size(112, 34);
            this.btnStartSnakeMoving.TabIndex = 5;
            this.btnStartSnakeMoving.Text = "Start move";
            this.btnStartSnakeMoving.UseVisualStyleBackColor = true;
            this.btnStartSnakeMoving.Click += new System.EventHandler(this.btnStartSnakeMove_Click);
            // 
            // lblPlayer_Score
            // 
            this.lblPlayer_Score.AutoSize = true;
            this.lblPlayer_Score.Location = new System.Drawing.Point(877, 43);
            this.lblPlayer_Score.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPlayer_Score.Name = "lblPlayer_Score";
            this.lblPlayer_Score.Size = new System.Drawing.Size(166, 19);
            this.lblPlayer_Score.TabIndex = 6;
            this.lblPlayer_Score.Text = "Điểm của người chơi: ";
            // 
            // MainTimer
            // 
            this.MainTimer.Enabled = true;
            this.MainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
            // 
            // btnFindPath
            // 
            this.btnFindPath.Location = new System.Drawing.Point(881, 181);
            this.btnFindPath.Margin = new System.Windows.Forms.Padding(4);
            this.btnFindPath.Name = "btnFindPath";
            this.btnFindPath.Size = new System.Drawing.Size(112, 34);
            this.btnFindPath.TabIndex = 8;
            this.btnFindPath.Text = "Find Path";
            this.btnFindPath.UseVisualStyleBackColor = true;
            this.btnFindPath.Click += new System.EventHandler(this.btnFindPath_Click);
            // 
            // NumericSpeed
            // 
            this.NumericSpeed.Location = new System.Drawing.Point(881, 105);
            this.NumericSpeed.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NumericSpeed.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumericSpeed.Name = "NumericSpeed";
            this.NumericSpeed.Size = new System.Drawing.Size(112, 27);
            this.NumericSpeed.TabIndex = 9;
            this.NumericSpeed.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.NumericSpeed.ValueChanged += new System.EventHandler(this.NumericSpeed_ValueChanged);
            // 
            // TableBotStatus
            // 
            this.TableBotStatus.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clName,
            this.clPoint});
            this.TableBotStatus.FullRowSelect = true;
            this.TableBotStatus.GridLines = true;
            this.TableBotStatus.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.TableBotStatus.HideSelection = false;
            this.TableBotStatus.Location = new System.Drawing.Point(881, 222);
            this.TableBotStatus.Name = "TableBotStatus";
            this.TableBotStatus.Size = new System.Drawing.Size(268, 220);
            this.TableBotStatus.TabIndex = 11;
            this.TableBotStatus.UseCompatibleStateImageBehavior = false;
            this.TableBotStatus.View = System.Windows.Forms.View.Details;
            // 
            // clName
            // 
            this.clName.Text = "";
            this.clName.Width = 200;
            // 
            // btnRestart
            // 
            this.btnRestart.Location = new System.Drawing.Point(1001, 139);
            this.btnRestart.Margin = new System.Windows.Forms.Padding(4);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(112, 34);
            this.btnRestart.TabIndex = 12;
            this.btnRestart.Text = "Restart";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // GraphicControl
            // 
            this.GraphicControl.BackColor = System.Drawing.Color.Black;
            this.GraphicControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.GraphicControl.Location = new System.Drawing.Point(13, 13);
            this.GraphicControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GraphicControl.Name = "GraphicControl";
            this.GraphicControl.Size = new System.Drawing.Size(845, 845);
            this.GraphicControl.TabIndex = 13;
            this.GraphicControl.VSync = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1308, 871);
            this.Controls.Add(this.GraphicControl);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.TableBotStatus);
            this.Controls.Add(this.NumericSpeed);
            this.Controls.Add(this.btnFindPath);
            this.Controls.Add(this.lblPlayer_Score);
            this.Controls.Add(this.btnStartSnakeMoving);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMain";
            this.Text = "My Snake";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.NumericSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnStartSnakeMoving;
        private System.Windows.Forms.Label lblPlayer_Score;
        private System.Windows.Forms.Timer MainTimer;
        private System.Windows.Forms.Button btnFindPath;
        private System.Windows.Forms.NumericUpDown NumericSpeed;
        private System.Windows.Forms.ListView TableBotStatus;
        private System.Windows.Forms.ColumnHeader clName;
        private System.Windows.Forms.ColumnHeader clPoint;
        private System.Windows.Forms.Button btnRestart;
        private OpenTK.GLControl GraphicControl;
    }
}

