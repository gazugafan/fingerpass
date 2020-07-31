namespace gazugafan.fingerpass
{
	partial class SetMasterPasswordForm
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
			this.saveButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.currentPasswordLabel = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.currentPasswordTextbox = new System.Windows.Forms.TextBox();
			this.newPasswordTextbox = new System.Windows.Forms.TextBox();
			this.confirmPasswordTextbox = new System.Windows.Forms.TextBox();
			this.instructionsLabel = new System.Windows.Forms.Label();
			this.deleteLink = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// saveButton
			// 
			this.saveButton.Location = new System.Drawing.Point(420, 259);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(139, 42);
			this.saveButton.TabIndex = 4;
			this.saveButton.Text = "Save Changes";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(310, 259);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(104, 42);
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// currentPasswordLabel
			// 
			this.currentPasswordLabel.AutoSize = true;
			this.currentPasswordLabel.Location = new System.Drawing.Point(41, 132);
			this.currentPasswordLabel.Name = "currentPasswordLabel";
			this.currentPasswordLabel.Size = new System.Drawing.Size(171, 17);
			this.currentPasswordLabel.TabIndex = 2;
			this.currentPasswordLabel.Text = "Current Master Password:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(61, 174);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(151, 17);
			this.label2.TabIndex = 3;
			this.label2.Text = "New Master Password:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 215);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(200, 17);
			this.label3.TabIndex = 4;
			this.label3.Text = "Retype New Master Password:";
			// 
			// currentPasswordTextbox
			// 
			this.currentPasswordTextbox.Location = new System.Drawing.Point(218, 129);
			this.currentPasswordTextbox.Name = "currentPasswordTextbox";
			this.currentPasswordTextbox.Size = new System.Drawing.Size(341, 22);
			this.currentPasswordTextbox.TabIndex = 0;
			this.currentPasswordTextbox.UseSystemPasswordChar = true;
			// 
			// newPasswordTextbox
			// 
			this.newPasswordTextbox.Location = new System.Drawing.Point(218, 171);
			this.newPasswordTextbox.Name = "newPasswordTextbox";
			this.newPasswordTextbox.Size = new System.Drawing.Size(341, 22);
			this.newPasswordTextbox.TabIndex = 1;
			this.newPasswordTextbox.UseSystemPasswordChar = true;
			// 
			// confirmPasswordTextbox
			// 
			this.confirmPasswordTextbox.Location = new System.Drawing.Point(218, 212);
			this.confirmPasswordTextbox.Name = "confirmPasswordTextbox";
			this.confirmPasswordTextbox.Size = new System.Drawing.Size(341, 22);
			this.confirmPasswordTextbox.TabIndex = 2;
			this.confirmPasswordTextbox.UseSystemPasswordChar = true;
			// 
			// instructionsLabel
			// 
			this.instructionsLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.instructionsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.instructionsLabel.Location = new System.Drawing.Point(15, 2);
			this.instructionsLabel.Name = "instructionsLabel";
			this.instructionsLabel.Size = new System.Drawing.Size(544, 124);
			this.instructionsLabel.TabIndex = 8;
			this.instructionsLabel.Text = "To change your master password, first enter your current master password. If you " +
    "don\'t know it, you\'ll need to delete your password database and start over.";
			this.instructionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// deleteLink
			// 
			this.deleteLink.AutoSize = true;
			this.deleteLink.LinkColor = System.Drawing.Color.Red;
			this.deleteLink.Location = new System.Drawing.Point(16, 276);
			this.deleteLink.Name = "deleteLink";
			this.deleteLink.Size = new System.Drawing.Size(114, 17);
			this.deleteLink.TabIndex = 5;
			this.deleteLink.TabStop = true;
			this.deleteLink.Text = "Delete Database";
			this.deleteLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.deleteLink_LinkClicked);
			// 
			// SetMasterPasswordForm
			// 
			this.AcceptButton = this.saveButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(571, 313);
			this.ControlBox = false;
			this.Controls.Add(this.deleteLink);
			this.Controls.Add(this.instructionsLabel);
			this.Controls.Add(this.confirmPasswordTextbox);
			this.Controls.Add(this.newPasswordTextbox);
			this.Controls.Add(this.currentPasswordTextbox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.currentPasswordLabel);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.saveButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SetMasterPasswordForm";
			this.ShowIcon = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FingerPass";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.SetMasterPasswordForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label currentPasswordLabel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox currentPasswordTextbox;
		private System.Windows.Forms.TextBox newPasswordTextbox;
		private System.Windows.Forms.TextBox confirmPasswordTextbox;
		private System.Windows.Forms.Label instructionsLabel;
		private System.Windows.Forms.LinkLabel deleteLink;
	}
}