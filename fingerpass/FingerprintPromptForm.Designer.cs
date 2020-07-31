namespace gazugafan.fingerpass
{
	partial class FingerprintPromptForm
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
			this.cancelButton = new System.Windows.Forms.Button();
			this.instructionsLabel = new System.Windows.Forms.Label();
			this.iconPictureBox = new System.Windows.Forms.PictureBox();
			this.passwordButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(455, 165);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(104, 42);
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// instructionsLabel
			// 
			this.instructionsLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.instructionsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.instructionsLabel.Location = new System.Drawing.Point(15, 9);
			this.instructionsLabel.Name = "instructionsLabel";
			this.instructionsLabel.Size = new System.Drawing.Size(544, 68);
			this.instructionsLabel.TabIndex = 8;
			this.instructionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// iconPictureBox
			// 
			this.iconPictureBox.Location = new System.Drawing.Point(249, 80);
			this.iconPictureBox.Name = "iconPictureBox";
			this.iconPictureBox.Size = new System.Drawing.Size(64, 64);
			this.iconPictureBox.TabIndex = 9;
			this.iconPictureBox.TabStop = false;
			// 
			// passwordButton
			// 
			this.passwordButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.passwordButton.Location = new System.Drawing.Point(12, 165);
			this.passwordButton.Name = "passwordButton";
			this.passwordButton.Size = new System.Drawing.Size(246, 42);
			this.passwordButton.TabIndex = 10;
			this.passwordButton.Text = "Enter Master Password Instead";
			this.passwordButton.UseVisualStyleBackColor = true;
			this.passwordButton.Click += new System.EventHandler(this.passwordButton_Click);
			// 
			// FingerprintPromptForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(571, 219);
			this.ControlBox = false;
			this.Controls.Add(this.passwordButton);
			this.Controls.Add(this.iconPictureBox);
			this.Controls.Add(this.instructionsLabel);
			this.Controls.Add(this.cancelButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FingerprintPromptForm";
			this.ShowIcon = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FingerPass";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FingerprintPromptForm_FormClosing);
			this.Load += new System.EventHandler(this.FingerprintPromptForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label instructionsLabel;
		private System.Windows.Forms.PictureBox iconPictureBox;
		private System.Windows.Forms.Button passwordButton;
	}
}