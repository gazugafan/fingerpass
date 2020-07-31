namespace gazugafan.fingerpass
{
	partial class DatabaseForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseForm));
			this.closeButton = new System.Windows.Forms.Button();
			this.passwordMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.copyPasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.moveToTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.moveToBottomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.changeMasterPasswordButton = new System.Windows.Forms.Button();
			this.exportButton = new System.Windows.Forms.Button();
			this.importButton = new System.Windows.Forms.Button();
			this.passwordsDataGrid = new System.Windows.Forms.DataGridView();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.programLabel = new System.Windows.Forms.Label();
			this.windowLabel = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.currentMatchLabel = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.instructionsPictureBox = new System.Windows.Forms.PictureBox();
			this.passwordMenuStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.passwordsDataGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.instructionsPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// closeButton
			// 
			this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.closeButton.Location = new System.Drawing.Point(789, 644);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(107, 42);
			this.closeButton.TabIndex = 1;
			this.closeButton.Text = "Done";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// passwordMenuStrip
			// 
			this.passwordMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.passwordMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyPasswordToolStripMenuItem,
            this.moveToTopToolStripMenuItem,
            this.moveToBottomToolStripMenuItem,
            this.toolStripMenuItem2,
            this.deleteToolStripMenuItem});
			this.passwordMenuStrip.Name = "contextMenuStrip1";
			this.passwordMenuStrip.Size = new System.Drawing.Size(188, 106);
			// 
			// copyPasswordToolStripMenuItem
			// 
			this.copyPasswordToolStripMenuItem.Name = "copyPasswordToolStripMenuItem";
			this.copyPasswordToolStripMenuItem.Size = new System.Drawing.Size(187, 24);
			this.copyPasswordToolStripMenuItem.Text = "Copy Password";
			this.copyPasswordToolStripMenuItem.Click += new System.EventHandler(this.copyPasswordToolStripMenuItem_Click);
			// 
			// moveToTopToolStripMenuItem
			// 
			this.moveToTopToolStripMenuItem.Name = "moveToTopToolStripMenuItem";
			this.moveToTopToolStripMenuItem.Size = new System.Drawing.Size(187, 24);
			this.moveToTopToolStripMenuItem.Text = "Move to Top";
			this.moveToTopToolStripMenuItem.Click += new System.EventHandler(this.moveToTopToolStripMenuItem_Click);
			// 
			// moveToBottomToolStripMenuItem
			// 
			this.moveToBottomToolStripMenuItem.Name = "moveToBottomToolStripMenuItem";
			this.moveToBottomToolStripMenuItem.Size = new System.Drawing.Size(187, 24);
			this.moveToBottomToolStripMenuItem.Text = "Move to Bottom";
			this.moveToBottomToolStripMenuItem.Click += new System.EventHandler(this.moveToBottomToolStripMenuItem_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(184, 6);
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(187, 24);
			this.deleteToolStripMenuItem.Text = "Delete";
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
			// 
			// changeMasterPasswordButton
			// 
			this.changeMasterPasswordButton.Location = new System.Drawing.Point(12, 644);
			this.changeMasterPasswordButton.Name = "changeMasterPasswordButton";
			this.changeMasterPasswordButton.Size = new System.Drawing.Size(196, 42);
			this.changeMasterPasswordButton.TabIndex = 29;
			this.changeMasterPasswordButton.Text = "Change Master Password";
			this.changeMasterPasswordButton.UseVisualStyleBackColor = true;
			this.changeMasterPasswordButton.Click += new System.EventHandler(this.changeMasterPasswordButton_Click);
			// 
			// exportButton
			// 
			this.exportButton.Location = new System.Drawing.Point(214, 644);
			this.exportButton.Name = "exportButton";
			this.exportButton.Size = new System.Drawing.Size(141, 42);
			this.exportButton.TabIndex = 30;
			this.exportButton.Text = "Export Database";
			this.exportButton.UseVisualStyleBackColor = true;
			this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
			// 
			// importButton
			// 
			this.importButton.Location = new System.Drawing.Point(361, 644);
			this.importButton.Name = "importButton";
			this.importButton.Size = new System.Drawing.Size(141, 42);
			this.importButton.TabIndex = 31;
			this.importButton.Text = "Import Database";
			this.importButton.UseVisualStyleBackColor = true;
			this.importButton.Click += new System.EventHandler(this.importButton_Click);
			// 
			// passwordsDataGrid
			// 
			this.passwordsDataGrid.AllowDrop = true;
			this.passwordsDataGrid.AllowUserToResizeRows = false;
			this.passwordsDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.passwordsDataGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
			this.passwordsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.passwordsDataGrid.ContextMenuStrip = this.passwordMenuStrip;
			this.passwordsDataGrid.Location = new System.Drawing.Point(12, 120);
			this.passwordsDataGrid.MultiSelect = false;
			this.passwordsDataGrid.Name = "passwordsDataGrid";
			this.passwordsDataGrid.RowHeadersWidth = 51;
			this.passwordsDataGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.passwordsDataGrid.RowTemplate.Height = 24;
			this.passwordsDataGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.passwordsDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.passwordsDataGrid.Size = new System.Drawing.Size(884, 433);
			this.passwordsDataGrid.TabIndex = 32;
			this.passwordsDataGrid.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.passwordsDataGrid_CellBeginEdit);
			this.passwordsDataGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.passwordsDataGrid_CellContentClick);
			this.passwordsDataGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.passwordsDataGrid_CellFormatting);
			this.passwordsDataGrid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.passwordsDataGrid_EditingControlShowing);
			this.passwordsDataGrid.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.passwordsDataGrid_RowValidating);
			this.passwordsDataGrid.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.passwordsDataGrid_UserDeletingRow);
			this.passwordsDataGrid.DragDrop += new System.Windows.Forms.DragEventHandler(this.passwordsDataGrid_DragDrop);
			this.passwordsDataGrid.DragOver += new System.Windows.Forms.DragEventHandler(this.passwordsDataGrid_DragOver);
			this.passwordsDataGrid.Paint += new System.Windows.Forms.PaintEventHandler(this.passwordsDataGrid_Paint);
			this.passwordsDataGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.passwordsDataGrid_MouseDown);
			this.passwordsDataGrid.MouseMove += new System.Windows.Forms.MouseEventHandler(this.passwordsDataGrid_MouseMove);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(231, 24);
			this.label1.TabIndex = 33;
			this.label1.Text = "Current Program Name:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.label2.Location = new System.Drawing.Point(31, 38);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(212, 24);
			this.label2.TabIndex = 34;
			this.label2.Text = "Current Window Title:";
			// 
			// programLabel
			// 
			this.programLabel.AutoEllipsis = true;
			this.programLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.programLabel.Location = new System.Drawing.Point(242, 9);
			this.programLabel.Name = "programLabel";
			this.programLabel.Size = new System.Drawing.Size(654, 24);
			this.programLabel.TabIndex = 35;
			this.programLabel.Text = "           This window will stay on top. Click on another window to see";
			// 
			// windowLabel
			// 
			this.windowLabel.AutoEllipsis = true;
			this.windowLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.windowLabel.Location = new System.Drawing.Point(242, 38);
			this.windowLabel.Name = "windowLabel";
			this.windowLabel.Size = new System.Drawing.Size(654, 24);
			this.windowLabel.TabIndex = 36;
			this.windowLabel.Text = "           how FingerPass reads its Program Name and Window Title,";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(9, 100);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(124, 17);
			this.label3.TabIndex = 37;
			this.label3.Text = "Saved Passwords:";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.SystemColors.InfoText;
			this.label4.Location = new System.Drawing.Point(61, 556);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(835, 77);
			this.label4.TabIndex = 38;
			this.label4.Text = resources.GetString("label4.Text");
			// 
			// currentMatchLabel
			// 
			this.currentMatchLabel.AutoEllipsis = true;
			this.currentMatchLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.currentMatchLabel.Location = new System.Drawing.Point(242, 67);
			this.currentMatchLabel.Name = "currentMatchLabel";
			this.currentMatchLabel.Size = new System.Drawing.Size(654, 24);
			this.currentMatchLabel.TabIndex = 40;
			this.currentMatchLabel.Text = "           then use that to create your password entries below...";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.label6.Location = new System.Drawing.Point(13, 67);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(230, 24);
			this.label6.TabIndex = 39;
			this.label6.Text = "Current Matching Entry:";
			// 
			// instructionsPictureBox
			// 
			this.instructionsPictureBox.Location = new System.Drawing.Point(12, 559);
			this.instructionsPictureBox.Name = "instructionsPictureBox";
			this.instructionsPictureBox.Size = new System.Drawing.Size(43, 41);
			this.instructionsPictureBox.TabIndex = 41;
			this.instructionsPictureBox.TabStop = false;
			// 
			// DatabaseForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.closeButton;
			this.ClientSize = new System.Drawing.Size(908, 698);
			this.Controls.Add(this.instructionsPictureBox);
			this.Controls.Add(this.currentMatchLabel);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.windowLabel);
			this.Controls.Add(this.programLabel);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.passwordsDataGrid);
			this.Controls.Add(this.importButton);
			this.Controls.Add(this.exportButton);
			this.Controls.Add(this.changeMasterPasswordButton);
			this.Controls.Add(this.closeButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DatabaseForm";
			this.ShowIcon = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FingerPass Database";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.DatabaseForm_Load);
			this.passwordMenuStrip.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.passwordsDataGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.instructionsPictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.Button changeMasterPasswordButton;
		private System.Windows.Forms.ContextMenuStrip passwordMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyPasswordToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem moveToTopToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem moveToBottomToolStripMenuItem;
		private System.Windows.Forms.Button exportButton;
		private System.Windows.Forms.Button importButton;
		private System.Windows.Forms.DataGridView passwordsDataGrid;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label programLabel;
		private System.Windows.Forms.Label windowLabel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label currentMatchLabel;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.PictureBox instructionsPictureBox;
	}
}