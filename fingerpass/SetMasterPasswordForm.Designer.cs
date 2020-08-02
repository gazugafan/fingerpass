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
			this.saveButton.Location = new System.Drawing.Point(368, 227);
			this.saveButton.Margin = new System.Windows.Forms.Padding(2);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(104, 34);
			this.saveButton.TabIndex = 4;
			this.saveButton.Text = "Save Changes";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(286, 227);
			this.cancelButton.Margin = new System.Windows.Forms.Padding(2);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(78, 34);
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// currentPasswordLabel
			// 
			this.currentPasswordLabel.AutoSize = true;
			this.currentPasswordLabel.Location = new System.Drawing.Point(42, 120);
			this.currentPasswordLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.currentPasswordLabel.Name = "currentPasswordLabel";
			this.currentPasswordLabel.Size = new System.Drawing.Size(128, 13);
			this.currentPasswordLabel.TabIndex = 2;
			this.currentPasswordLabel.Text = "Current Master Password:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(54, 154);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(116, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "New Master Password:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(17, 188);
			this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(153, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Retype New Master Password:";
			// 
			// currentPasswordTextbox
			// 
			this.currentPasswordTextbox.Location = new System.Drawing.Point(172, 117);
			this.currentPasswordTextbox.Margin = new System.Windows.Forms.Padding(2);
			this.currentPasswordTextbox.Name = "currentPasswordTextbox";
			this.currentPasswordTextbox.Size = new System.Drawing.Size(300, 20);
			this.currentPasswordTextbox.TabIndex = 0;
			this.currentPasswordTextbox.UseSystemPasswordChar = true;
			// 
			// newPasswordTextbox
			// 
			this.newPasswordTextbox.Location = new System.Drawing.Point(172, 151);
			this.newPasswordTextbox.Margin = new System.Windows.Forms.Padding(2);
			this.newPasswordTextbox.Name = "newPasswordTextbox";
			this.newPasswordTextbox.Size = new System.Drawing.Size(300, 20);
			this.newPasswordTextbox.TabIndex = 1;
			this.newPasswordTextbox.UseSystemPasswordChar = true;
			// 
			// confirmPasswordTextbox
			// 
			this.confirmPasswordTextbox.Location = new System.Drawing.Point(172, 185);
			this.confirmPasswordTextbox.Margin = new System.Windows.Forms.Padding(2);
			this.confirmPasswordTextbox.Name = "confirmPasswordTextbox";
			this.confirmPasswordTextbox.Size = new System.Drawing.Size(300, 20);
			this.confirmPasswordTextbox.TabIndex = 2;
			this.confirmPasswordTextbox.UseSystemPasswordChar = true;
			// 
			// instructionsLabel
			// 
			this.instructionsLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.instructionsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.instructionsLabel.Location = new System.Drawing.Point(38, 3);
			this.instructionsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.instructionsLabel.Name = "instructionsLabel";
			this.instructionsLabel.Size = new System.Drawing.Size(408, 91);
			this.instructionsLabel.TabIndex = 8;
			this.instructionsLabel.Text = "To change your master password, first enter your current master password. If you " +
    "don\'t know it, you\'ll need to delete your password database and start over.";
			this.instructionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// deleteLink
			// 
			this.deleteLink.AutoSize = true;
			this.deleteLink.LinkColor = System.Drawing.Color.Red;
			this.deleteLink.Location = new System.Drawing.Point(12, 240);
			this.deleteLink.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.deleteLink.Name = "deleteLink";
			this.deleteLink.Size = new System.Drawing.Size(87, 13);
			this.deleteLink.TabIndex = 5;
			this.deleteLink.TabStop = true;
			this.deleteLink.Text = "Delete Database";
			this.deleteLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.deleteLink_LinkClicked);
			// 
			// SetMasterPasswordForm
			// 
			this.AcceptButton = this.saveButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(483, 272);
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
			this.Margin = new System.Windows.Forms.Padding(2);
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