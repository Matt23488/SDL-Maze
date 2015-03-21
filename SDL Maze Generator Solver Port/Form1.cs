using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SDL_Maze_Generator_Solver_Port
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void startButton_Click(object sender, EventArgs e)
		{
			bool primaryMonitor = Screen.FromControl(this).Primary;
			uint delay = Convert.ToUInt32(delayTextBox.Text);
			bool isDemo = demoCheckBox.Checked;
			MyMaze maze;

			this.Hide();

			if (fullScreenCheckBox.Checked)
			{
				maze = new MyMaze(primaryMonitor, delay, isDemo);
			}
			else
			{
				int width = Convert.ToInt32(widthTextBox.Text);
				int height = Convert.ToInt32(heightTextBox.Text);
				maze = new MyMaze(width, height, primaryMonitor, delay, isDemo);
			}

			maze.start();

			this.Show();
		}

		private void controlsButton_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Arrow Keys:\tMove through maze\n" +
				"Tab:\t\tAuto-solve the maze\n" +
				"Esc:\t\tQuit\n\n" +
				"Demo mode will show the maze being generated, and will auto-solve itself.",
				"Controls",
				MessageBoxButtons.OK,
				MessageBoxIcon.Question);
		}

		private void fullScreenCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (fullScreenCheckBox.Checked)
			{
				widthTextBox.Enabled = false;
				heightTextBox.Enabled = false;
			}
			else
			{
				widthTextBox.Enabled = true;
				heightTextBox.Enabled = true;
			}
		}
	}
}
