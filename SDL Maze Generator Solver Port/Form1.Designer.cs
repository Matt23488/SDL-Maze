namespace SDL_Maze_Generator_Solver_Port
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
			this.widthTextBox = new System.Windows.Forms.TextBox();
			this.heightTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.demoCheckBox = new System.Windows.Forms.CheckBox();
			this.startButton = new System.Windows.Forms.Button();
			this.delayTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.controlsButton = new System.Windows.Forms.Button();
			this.fullScreenCheckBox = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// widthTextBox
			// 
			this.widthTextBox.Location = new System.Drawing.Point(95, 12);
			this.widthTextBox.Name = "widthTextBox";
			this.widthTextBox.Size = new System.Drawing.Size(127, 20);
			this.widthTextBox.TabIndex = 0;
			this.widthTextBox.Text = "65";
			// 
			// heightTextBox
			// 
			this.heightTextBox.Location = new System.Drawing.Point(95, 38);
			this.heightTextBox.Name = "heightTextBox";
			this.heightTextBox.Size = new System.Drawing.Size(127, 20);
			this.heightTextBox.TabIndex = 1;
			this.heightTextBox.Text = "49";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Width:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 41);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(41, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Height:";
			// 
			// demoCheckBox
			// 
			this.demoCheckBox.AutoSize = true;
			this.demoCheckBox.Location = new System.Drawing.Point(95, 114);
			this.demoCheckBox.Name = "demoCheckBox";
			this.demoCheckBox.Size = new System.Drawing.Size(84, 17);
			this.demoCheckBox.TabIndex = 4;
			this.demoCheckBox.Text = "Demo Mode";
			this.demoCheckBox.UseVisualStyleBackColor = true;
			// 
			// startButton
			// 
			this.startButton.Location = new System.Drawing.Point(12, 137);
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size(75, 23);
			this.startButton.TabIndex = 5;
			this.startButton.Text = "Start";
			this.startButton.UseVisualStyleBackColor = true;
			this.startButton.Click += new System.EventHandler(this.startButton_Click);
			// 
			// delayTextBox
			// 
			this.delayTextBox.Location = new System.Drawing.Point(95, 64);
			this.delayTextBox.Name = "delayTextBox";
			this.delayTextBox.Size = new System.Drawing.Size(127, 20);
			this.delayTextBox.TabIndex = 6;
			this.delayTextBox.Text = "25";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 67);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(59, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "Delay (ms):";
			// 
			// controlsButton
			// 
			this.controlsButton.Location = new System.Drawing.Point(93, 137);
			this.controlsButton.Name = "controlsButton";
			this.controlsButton.Size = new System.Drawing.Size(75, 23);
			this.controlsButton.TabIndex = 8;
			this.controlsButton.Text = "Controls";
			this.controlsButton.UseVisualStyleBackColor = true;
			this.controlsButton.Click += new System.EventHandler(this.controlsButton_Click);
			// 
			// fullScreenCheckBox
			// 
			this.fullScreenCheckBox.AutoSize = true;
			this.fullScreenCheckBox.Location = new System.Drawing.Point(95, 91);
			this.fullScreenCheckBox.Name = "fullScreenCheckBox";
			this.fullScreenCheckBox.Size = new System.Drawing.Size(79, 17);
			this.fullScreenCheckBox.TabIndex = 9;
			this.fullScreenCheckBox.Text = "Full Screen";
			this.fullScreenCheckBox.UseVisualStyleBackColor = true;
			this.fullScreenCheckBox.CheckedChanged += new System.EventHandler(this.fullScreenCheckBox_CheckedChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(234, 172);
			this.Controls.Add(this.fullScreenCheckBox);
			this.Controls.Add(this.controlsButton);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.delayTextBox);
			this.Controls.Add(this.startButton);
			this.Controls.Add(this.demoCheckBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.heightTextBox);
			this.Controls.Add(this.widthTextBox);
			this.Name = "Form1";
			this.Text = "Create Maze";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox widthTextBox;
		private System.Windows.Forms.TextBox heightTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox demoCheckBox;
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.TextBox delayTextBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button controlsButton;
		private System.Windows.Forms.CheckBox fullScreenCheckBox;

	}
}

