namespace Ez.Application
{
	partial class MainForm
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
			this.refreshTimer = new System.Windows.Forms.Timer(this.components);
			this.rendererControl = new EZ.Renderer.RendererControl();
			this.SuspendLayout();
			// 
			// refreshTimer
			// 
			this.refreshTimer.Enabled = true;
			this.refreshTimer.Interval = 10;
			this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
			// 
			// rendererControl
			// 
			this.rendererControl.BackColor = System.Drawing.Color.LightBlue;
			this.rendererControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rendererControl.Location = new System.Drawing.Point(0, 0);
			this.rendererControl.Name = "rendererControl";
			this.rendererControl.Size = new System.Drawing.Size(784, 562);
			this.rendererControl.TabIndex = 0;
			this.rendererControl.VSync = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 562);
			this.Controls.Add(this.rendererControl);
			this.Name = "MainForm";
			this.Text = "Application";
			this.ResumeLayout(false);

		}

		#endregion

		private EZ.Renderer.RendererControl rendererControl;
		private System.Windows.Forms.Timer refreshTimer;
	}
}

