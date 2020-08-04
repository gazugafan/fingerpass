using gazugafan.fingerpass.Properties;
using gazugafan.fingerpass.tray;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gazugafan.fingerpass
{
	public partial class DatabaseForm : Form
	{
		private Timer _updateWindowTimer;
		private bool _loadingData = false;
		private bool _validationProblems = false;
		static string defaultPassword = "ChangeMePls";

		//to support drag and drop...
		private Rectangle dragBoxFromMouseDown;
		private int rowIndexFromMouseDown;
		private int rowIndexOfItemUnderMouseToDrop = -1;

		private string _currentProgramName = null;
		private string _currentWindowTitle = null;
		private int _currentMatchIndex = -2;
		private string _newProgramName = null;
		private string _newWindowTitle = null;

		public DatabaseForm(string newProgramName = null, string newWindowTitle = null)
		{
			_newProgramName = newProgramName;
			_newWindowTitle = newWindowTitle;
			InitializeComponent();
		}

		private void DatabaseForm_Load(object sender, EventArgs e)
		{
			this.FormBorderStyle = FormBorderStyle.FixedDialog; //some DPI madness requires this to be set to sizable to start

			int[] widths = new int[5];
			widths[0] = 25;
			widths[1] = 25;
			widths[2] = 25;
			widths[3] = 15;
			widths[4] = 10;

			passwordsDataGrid.Columns.Add("programNameColumn", "Program Name");
			passwordsDataGrid.Columns.Add("windowTitleColumn", "Window Title");
			passwordsDataGrid.Columns.Add("passwordColumn", "Password");
			passwordsDataGrid.Columns.Add(new DataGridViewCheckBoxColumn());
			passwordsDataGrid.Columns.Add(new DataGridViewLinkColumn());

			passwordsDataGrid.Columns[3].HeaderText = "Press Enter?";
			passwordsDataGrid.Columns[4].HeaderText = "Actions";

			//set column widths based on percents, and other options...
			passwordsDataGrid.RowHeadersWidth = 50;
			for (int i = 0; i < passwordsDataGrid.Columns.Count; i++)
			{
				float colPercentage = ((float)widths[i] / 100);
				passwordsDataGrid.Columns[i].Width = (int)(colPercentage * (passwordsDataGrid.ClientRectangle.Width - 50));
				passwordsDataGrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
			}

			UpdatePasswordsList();

			instructionsPictureBox.Image = SystemIcons.Question.ToBitmap();

			_updateWindowTimer = new Timer();
			_updateWindowTimer.Interval = 1000;
			_updateWindowTimer.Tick += UpdateWindow;
			_updateWindowTimer.Start();

			//if we wanted to start creating a new password immediately, begin filling one out with the supplied name and title...
			if (_newProgramName != null || _newWindowTitle != null)
			{
				DataGridViewRow row = passwordsDataGrid.Rows[passwordsDataGrid.NewRowIndex];

				row.Cells[0].Value = _newProgramName;
				row.Cells[1].Value = _newWindowTitle;
				row.Cells[2].Value = "";
				row.Cells[3].Value = true;

				row.Cells[2].Selected = true;
				passwordsDataGrid.BeginEdit(true);
			}
		}

		private void UpdateWindow(object sender, EventArgs e)
		{
			string programName = Windows.GetFocusedApplicationName();
			string windowTitle = Windows.GetFocusedWindowTitle();

			//only update if we're not focused on ourself or a toast...
			if (!programName.ToLower().Contains("fingerpass") && !programName.ToLower().Contains("windows shell experience host"))
			{
				_currentProgramName = programName;
				_currentWindowTitle = windowTitle;
				this.programLabel.Text = _currentProgramName;
				this.windowLabel.Text = _currentWindowTitle;
			}

			int matchingIndex = -1;
			if (_currentProgramName != null)
			{
				try
				{
					matchingIndex = Program.keyDatabase.FindMatchingPasswordIndex(_currentProgramName, _currentWindowTitle);
					Password password = Program.keyDatabase.database.Passwords[matchingIndex];
					this.currentMatchLabel.Text = "Entry #" + (matchingIndex + 1).ToString() + " (" + password.Program + " / " + password.Title + ")";
					this.currentMatchLabel.ForeColor = Color.Green;
				}
				catch(Exception ex)
				{
					this.currentMatchLabel.Text = "None of your passwords match this Program and Window Title";
					this.currentMatchLabel.ForeColor = SystemColors.ControlText;
				}

				//if the matching password has changed, update the highlight...
				if (matchingIndex != _currentMatchIndex)
				{
					if (_currentMatchIndex >= 0)
					{
						passwordsDataGrid.Rows[_currentMatchIndex].DefaultCellStyle.BackColor = default;
						passwordsDataGrid.Rows[_currentMatchIndex].DefaultCellStyle.ForeColor = default;
						passwordsDataGrid.Rows[_currentMatchIndex].DefaultCellStyle.SelectionBackColor = default;
						passwordsDataGrid.Rows[_currentMatchIndex].DefaultCellStyle.SelectionForeColor = default;
					}

					if (matchingIndex >= 0)
					{
						passwordsDataGrid.Rows[matchingIndex].DefaultCellStyle.BackColor = Color.Green;
						passwordsDataGrid.Rows[matchingIndex].DefaultCellStyle.ForeColor = Color.White;
						passwordsDataGrid.Rows[matchingIndex].DefaultCellStyle.SelectionBackColor = Color.LightGreen;
						passwordsDataGrid.Rows[matchingIndex].DefaultCellStyle.SelectionForeColor = Color.DarkGreen;
					}

					_currentMatchIndex = matchingIndex;
				}
			}
		}

		private void UpdatePasswordsList()
		{
			int selectedIndex = -1;
			if (passwordsDataGrid.CurrentRow != null)
				selectedIndex = passwordsDataGrid.CurrentRow.Index;

			_loadingData = true;
			_validationProblems = false;

			passwordsDataGrid.Rows.Clear();
			Program.keyDatabase.database.Passwords.ForEach((password) =>
			{
				DataGridViewRow row = new DataGridViewRow();

				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells.Add(new DataGridViewTextBoxCell());
				row.Cells.Add(new DataGridViewCheckBoxCell());
				row.Cells.Add(new DataGridViewLinkCell());
				updatePasswordCells(row, password);

				passwordsDataGrid.Rows.Add(row);
			});

			_loadingData = false;

			if (selectedIndex >= 0 && selectedIndex < passwordsDataGrid.Rows.Count - 1)
			{
				passwordsDataGrid.ClearSelection();
				passwordsDataGrid.Rows[selectedIndex].Selected = true;
				passwordsDataGrid.Rows[selectedIndex].Cells[0].Selected = true;
			}
		}

		private void updatePasswordCells(DataGridViewRow row, Password password)
		{
			row.Cells[0].Value = password.Program;
			row.Cells[1].Value = password.Title;
			row.Cells[2].Value = defaultPassword;
			row.Cells[3].Value = password.PressEnter;
			row.Cells[4].Value = "Delete";
			(row.Cells[4] as DataGridViewLinkCell).LinkBehavior = LinkBehavior.HoverUnderline;
			(row.Cells[4] as DataGridViewLinkCell).TrackVisitedState = false;
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void changeMasterPasswordButton_Click(object sender, EventArgs e)
		{
			new SetMasterPasswordForm().ShowDialog();
			UpdatePasswordsList();
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (passwordsDataGrid.SelectedRows.Count == 1)
			{
				DataGridViewRow row = passwordsDataGrid.SelectedRows[0];
				DeletedPassword(row.Index);
			}
		}

		private void DeletedPassword(int index)
		{
			if (index >= 0 && index < Program.keyDatabase.database.Passwords.Count)
			{
				Password password = Program.keyDatabase.database.Passwords[index];

				if (new FingerprintPromptForm("Are you sure you want to delete this " + password.Program + " password?").ShowDialog() == DialogResult.OK)
				{
					Program.keyDatabase.RemoveIndex(index);
					UpdatePasswordsList();
				}
			}
		}

		private void passwordsDataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex == 2 && e.Value != null)
			{
				e.Value = new String('\u25CF', e.Value.ToString().Length);
			}
			else if (e.ColumnIndex == 3)
			{
				passwordsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = "If checked, we'll also press ENTER after filling in your password.";
			}
		}

		private void passwordsDataGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			if (e.Control is TextBox)
			{
				TextBox textBox = e.Control as TextBox;
				if (textBox != null)
				{
					if (passwordsDataGrid.CurrentCell.ColumnIndex == 2)
						textBox.UseSystemPasswordChar = true;
					else
						textBox.UseSystemPasswordChar = false;
				}
			}
		}

		private void passwordsDataGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			if (e.ColumnIndex == 2)
			{
				passwordsDataGrid.Rows[e.RowIndex].Cells[2].Value = "";
			}
		}

		private void passwordsDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0)
			{
				DataGridViewCell cell = passwordsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
				if (cell.ColumnIndex == 4)
				{
					DeletedPassword(cell.RowIndex);
				}
			}
		}

		private void passwordsDataGrid_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
		{
			if (_loadingData)
				return;

			if (e.RowIndex >= 0)
			{
				DataGridViewRow row = passwordsDataGrid.Rows[e.RowIndex];
				bool isRowNew = (row.Cells[4].Value == null);

				if ((row.Cells[0].Value == null || row.Cells[0].Value.ToString().Trim() == "") && (row.Cells[1].Value == null || row.Cells[1].Value.ToString().Trim() == ""))
				{
					row.ErrorText = "Please enter a program name and/or window title that the password applies to.";
					row.Cells[0].ErrorText = row.ErrorText;
					row.Cells[1].ErrorText = row.ErrorText;
					e.Cancel = true;

					//if this is actually a totally blank new password, just stop...
					if (isRowNew && (row.Cells[2].Value == null || row.Cells[2].Value.ToString().Trim() == ""))
					{
						//just reload password database on next tick to cancel...
						BeginInvoke(new MethodInvoker(delegate
						{
							UpdatePasswordsList();
						}));
					}
				}

				if ((row.Cells[2].Value == null || row.Cells[2].Value.ToString().Trim() == ""))
				{
					row.ErrorText = "Please enter a password for this program name and/or window title.";
					row.Cells[2].ErrorText = row.ErrorText;
					e.Cancel = true;
				}

				if (!e.Cancel)
				{
					if (isRowNew)
					{
						int index = Program.keyDatabase.FindPasswordIndex((row.Cells[0].Value != null) ? row.Cells[0].Value.ToString() : "", (row.Cells[1].Value != null) ? row.Cells[1].Value.ToString() : "");
						if (index >= 0)
						{
							row.ErrorText = "You already have a password in the database with this program name and window title.";
							row.Cells[0].ErrorText = row.ErrorText;
							row.Cells[1].ErrorText = row.ErrorText;
							e.Cancel = true;
						}
					}
				}

				if (!e.Cancel)
				{
					_validationProblems = false;
					row.ErrorText = null;
					foreach (DataGridViewCell cell in row.Cells)
						cell.ErrorText = null;

					if (passwordsDataGrid.IsCurrentRowDirty)
					{
						try
						{
							Password updatedPassword = null;
							string program = (row.Cells[0].Value != null) ? row.Cells[0].Value.ToString():"";
							string window = (row.Cells[1].Value != null) ? row.Cells[1].Value.ToString():"";
							bool enter = (row.Cells[3].Value != null) ? (bool)row.Cells[3].Value : false;
							string password = row.Cells[2].Value.ToString();

							if (isRowNew)
							{
								updatedPassword = Program.keyDatabase.AddOrUpdate(program, window, enter, password);
							}
							else
							{
								//if password wasn't updated, no need to change it and ecrypt a new one. Otherwise, we should double-check that they want to overwrite an existing password...
								if (password == defaultPassword)
								{
									password = null;
								}
								else if (MessageBox.Show("You're about to overwrite your existing " + program + " password. If you continue, the old password will be lost forever. Are you sure?", "WARNING", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
								{
									e.Cancel = true;
									BeginInvoke(new MethodInvoker(delegate
									{
										UpdatePasswordsList();
									}));

									return;
								}

								updatedPassword = Program.keyDatabase.UpdateIndex(row.Index, program, window, enter, password);
							}

							updatePasswordCells(row, updatedPassword);
						}
						catch (Exception ex)
						{
							MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

							//something went wrong. just reload password database on next tick...
							BeginInvoke(new MethodInvoker(delegate
							{
								UpdatePasswordsList();
							}));
						}
					}
				}
				else
				{
					//after repeated failures, show the error in a messagebox and give the user an easy way to cancel...
					if (_validationProblems)
					{
						if (MessageBox.Show(row.ErrorText, "Problem", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
						{
							//just reload password database on next tick to cancel...
							BeginInvoke(new MethodInvoker(delegate
							{
								UpdatePasswordsList();
							}));
						}
					}

					_validationProblems = true;
				}
			}
		}

		private void passwordsDataGrid_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
		{
			if (e.Row.Index >= 0 && e.Row.Index < Program.keyDatabase.database.Passwords.Count)
			{
				e.Cancel = true; //we handle this ourselves

				//delete password on next tick...
				BeginInvoke(new MethodInvoker(delegate
				{
					DeletedPassword(e.Row.Index);
				}));
			}
		}

		private void passwordsDataGrid_MouseMove(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				// If the mouse moves outside the rectangle, start the drag.
				if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
				{
					// Proceed with the drag and drop, passing in the list item.                    
					DragDropEffects dropEffect = passwordsDataGrid.DoDragDrop(
					passwordsDataGrid.Rows[rowIndexFromMouseDown],
					DragDropEffects.Move);
				}
			}
		}

		private void passwordsDataGrid_MouseDown(object sender, MouseEventArgs e)
		{
			// Get the index of the item the mouse is below.
			rowIndexFromMouseDown = passwordsDataGrid.HitTest(e.X, e.Y).RowIndex;

			if (e.Button == MouseButtons.Right)
			{
				passwordsDataGrid.ClearSelection();
				passwordsDataGrid.Rows[rowIndexFromMouseDown].Selected = true;
			}
			else if (e.Button == MouseButtons.Left)
			{
				if (rowIndexFromMouseDown >= 0 && !passwordsDataGrid.Rows[rowIndexFromMouseDown].IsNewRow && !passwordsDataGrid.CurrentRow.IsNewRow)
				{
					// Remember the point where the mouse down occurred. 
					// The DragSize indicates the size that the mouse can move 
					// before a drag event should be started.                
					Size dragSize = SystemInformation.DragSize;

					// Create a rectangle using the DragSize, with the mouse position being
					// at the center of the rectangle.
					dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
																   e.Y - (dragSize.Height / 2)),
										dragSize);
				}
				else // Reset the rectangle if the mouse is not over an item in the ListBox.
					dragBoxFromMouseDown = Rectangle.Empty;
			}
		}

		private void passwordsDataGrid_DragOver(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Move;

			//if the drop index has changed, update it and force a repaint to draw the insertion point marker...
			Point clientPoint = passwordsDataGrid.PointToClient(new Point(e.X, e.Y));
			int index = passwordsDataGrid.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
			if (index != rowIndexOfItemUnderMouseToDrop)
			{
				rowIndexOfItemUnderMouseToDrop = index;
				passwordsDataGrid.Invalidate();
			}
		}

		private void passwordsDataGrid_DragDrop(object sender, DragEventArgs e)
		{
			// The mouse locations are relative to the screen, so they must be 
			// converted to client coordinates.
			Point clientPoint = passwordsDataGrid.PointToClient(new Point(e.X, e.Y));

			// Get the row index of the item the mouse is below. 
			rowIndexOfItemUnderMouseToDrop = passwordsDataGrid.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

			// If the drag operation was a move then remove and insert the row.
			if (e.Effect == DragDropEffects.Move)
			{
				if (rowIndexOfItemUnderMouseToDrop >= 0 && rowIndexOfItemUnderMouseToDrop < passwordsDataGrid.Rows.Count && !passwordsDataGrid.Rows[rowIndexOfItemUnderMouseToDrop].IsNewRow)
				{
					Program.keyDatabase.MovePassword(rowIndexFromMouseDown, rowIndexOfItemUnderMouseToDrop);
					UpdatePasswordsList();
				}
			}

			rowIndexOfItemUnderMouseToDrop = -1;
		}

		private void moveToTopToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (passwordsDataGrid.SelectedRows.Count == 1)
			{
				DataGridViewRow row = passwordsDataGrid.SelectedRows[0];
				if (!row.IsNewRow)
				{
					Program.keyDatabase.MovePassword(row.Index, 0);
					UpdatePasswordsList();
				}
			}
		}

		private void moveToBottomToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (passwordsDataGrid.SelectedRows.Count == 1)
			{
				DataGridViewRow row = passwordsDataGrid.SelectedRows[0];
				if (!row.IsNewRow)
				{
					Program.keyDatabase.MovePassword(row.Index, Program.keyDatabase.database.Passwords.Count - 1);
					UpdatePasswordsList();
				}
			}
		}

		/// <summary>
		/// Paints an insertion point marker when dragging
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void passwordsDataGrid_Paint(object sender, PaintEventArgs e)
		{
			if (rowIndexOfItemUnderMouseToDrop >= 0 && rowIndexOfItemUnderMouseToDrop < passwordsDataGrid.Rows.Count)
			{
				DataGridViewRow row = passwordsDataGrid.Rows[rowIndexOfItemUnderMouseToDrop];
				if (!row.IsNewRow)
				{
					int index = rowIndexOfItemUnderMouseToDrop;
					if (index > rowIndexFromMouseDown)
						index++;

					int dividerHeight = 4;
					int width = passwordsDataGrid.DisplayRectangle.Width - 2;
					int relativeY = (index > 0
										 ? passwordsDataGrid.GetRowDisplayRectangle(index - 1, false).Bottom
										 : passwordsDataGrid.Columns[0].HeaderCell.Size.Height);

					if (relativeY == 0) relativeY = passwordsDataGrid.GetRowDisplayRectangle(passwordsDataGrid.FirstDisplayedScrollingRowIndex, true).Top;
					int locationX = passwordsDataGrid.DisplayRectangle.Left + 2;
					int locationY = relativeY - (int)Math.Ceiling((double)dividerHeight / 2);
					Rectangle rectangle = new Rectangle(locationX, locationY, width, dividerHeight);

					SolidBrush divider = new SolidBrush(Color.DarkRed);
					e.Graphics.FillRectangle(divider, rectangle);
				}
			}
		}

		private void copyPasswordToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (passwordsDataGrid.SelectedRows.Count == 1)
			{
				DataGridViewRow row = passwordsDataGrid.SelectedRows[0];

				if (new FingerprintPromptForm().ShowDialog() == DialogResult.OK)
				{
					try
					{
						Program.keyDatabase.CopyPassword(row.Index);
						Program.trayIcon.SendToast("FingerPass", "Password copied to clipboard!");
					}
					catch(Exception ex)
					{
						MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		private void exportButton_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();

			saveFileDialog.Filter = "fingerpass export files (*.fingerpass)|*.fingerpass|All files (*.*)|*.*";
			saveFileDialog.RestoreDirectory = true;

			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					Program.keyDatabase.Export(Path.GetFullPath(saveFileDialog.FileName));
					MessageBox.Show("Database exported successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void importButton_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Importing will overwrite your existing database of passwords. They'll be lost forever. Are you sure you want to proceed?", "WARNING", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();

				openFileDialog.Filter = "fingerpass export files (*.fingerpass)|*.fingerpass|All files (*.*)|*.*";
				openFileDialog.RestoreDirectory = true;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					try
					{
						Program.keyDatabase.Import(Path.GetFullPath(openFileDialog.FileName));
						UpdatePasswordsList();
						MessageBox.Show("Database imported successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		private void DatabaseForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			_updateWindowTimer.Stop();
			_updateWindowTimer = null;
		}
	}
}
