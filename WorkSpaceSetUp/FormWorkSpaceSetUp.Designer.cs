namespace WorkSpaceSetUp
{
    partial class FormWorkSpaceSetUp
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            _splitContainer = new SplitContainer();
            _removeFileGroupButton = new Button();
            _addFileGroupButton = new Button();
            _fileGroupListLabel = new Label();
            _fileGroupListBox = new ListBox();
            _pathDataListView = new ListView();
            _columnHeaderName = new ColumnHeader();
            _columnHeaderType = new ColumnHeader();
            _columnHeaderPath = new ColumnHeader();
            _addFilePathButton = new Button();
            _fileGroupNameTextBox = new TextBox();
            _removePathButton = new Button();
            _addFolderPathButton = new Button();
            _openFileDialog = new OpenFileDialog();
            _folderBrowserDialog = new FolderBrowserDialog();
            _errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)_splitContainer).BeginInit();
            _splitContainer.Panel1.SuspendLayout();
            _splitContainer.Panel2.SuspendLayout();
            _splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_errorProvider).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            _splitContainer.Dock = DockStyle.Fill;
            _splitContainer.Location = new Point(0, 0);
            _splitContainer.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            _splitContainer.Panel1.Controls.Add(_removeFileGroupButton);
            _splitContainer.Panel1.Controls.Add(_addFileGroupButton);
            _splitContainer.Panel1.Controls.Add(_fileGroupListLabel);
            _splitContainer.Panel1.Controls.Add(_fileGroupListBox);
            // 
            // splitContainer1.Panel2
            // 
            _splitContainer.Panel2.Controls.Add(_pathDataListView);
            _splitContainer.Panel2.Controls.Add(_addFilePathButton);
            _splitContainer.Panel2.Controls.Add(_fileGroupNameTextBox);
            _splitContainer.Panel2.Controls.Add(_removePathButton);
            _splitContainer.Panel2.Controls.Add(_addFolderPathButton);
            _splitContainer.Size = new Size(800, 450);
            _splitContainer.SplitterDistance = 375;
            _splitContainer.TabIndex = 0;
            // 
            // _removeFileGroupButton
            // 
            _removeFileGroupButton.Location = new Point(174, 336);
            _removeFileGroupButton.Name = "_removeFileGroupButton";
            _removeFileGroupButton.Size = new Size(94, 29);
            _removeFileGroupButton.TabIndex = 3;
            _removeFileGroupButton.Text = "Remove";
            _removeFileGroupButton.UseVisualStyleBackColor = true;
            // 
            // _addFileGroupButton
            // 
            _addFileGroupButton.Location = new Point(12, 336);
            _addFileGroupButton.Name = "_addFileGroupButton";
            _addFileGroupButton.Size = new Size(94, 29);
            _addFileGroupButton.TabIndex = 2;
            _addFileGroupButton.Text = "Add";
            _addFileGroupButton.UseVisualStyleBackColor = true;
            // 
            // _fileGroupListLabel
            // 
            _fileGroupListLabel.AutoSize = true;
            _fileGroupListLabel.Location = new Point(12, 23);
            _fileGroupListLabel.Name = "_fileGroupListLabel";
            _fileGroupListLabel.Size = new Size(103, 20);
            _fileGroupListLabel.TabIndex = 1;
            _fileGroupListLabel.Text = "File Group List";
            // 
            // _fileGroupListBox
            // 
            _fileGroupListBox.FormattingEnabled = true;
            _fileGroupListBox.Location = new Point(12, 46);
            _fileGroupListBox.Name = "_fileGroupListBox";
            _fileGroupListBox.Size = new Size(256, 284);
            _fileGroupListBox.TabIndex = 0;
            // 
            // _pathDataListView
            // 
            _pathDataListView.Columns.AddRange(new ColumnHeader[] { _columnHeaderName, _columnHeaderType, _columnHeaderPath });
            _pathDataListView.FullRowSelect = true;
            _pathDataListView.Location = new Point(29, 46);
            _pathDataListView.MultiSelect = false;
            _pathDataListView.Name = "_pathDataListView";
            _pathDataListView.Size = new Size(264, 284);
            _pathDataListView.TabIndex = 8;
            _pathDataListView.UseCompatibleStateImageBehavior = false;
            _pathDataListView.View = View.Details;
            // 
            // _columnHeaderName
            // 
            _columnHeaderName.Text = "Name";
            // 
            // _columnHeaderType
            // 
            _columnHeaderType.Text = "Type";
            // 
            // _columnHeaderPath
            // 
            _columnHeaderPath.Text = "Path";
            _columnHeaderPath.Width = 180;
            // 
            // _addFilePathButton
            // 
            _addFilePathButton.Location = new Point(29, 385);
            _addFilePathButton.Name = "_addFilePathButton";
            _addFilePathButton.Size = new Size(94, 29);
            _addFilePathButton.TabIndex = 7;
            _addFilePathButton.Text = "Add File";
            _addFilePathButton.UseVisualStyleBackColor = true;
            // 
            // _fileGroupNameTextBox
            // 
            _fileGroupNameTextBox.Location = new Point(29, 16);
            _fileGroupNameTextBox.Name = "_fileGroupNameTextBox";
            _fileGroupNameTextBox.Size = new Size(125, 27);
            _fileGroupNameTextBox.TabIndex = 6;
            // 
            // _removePathButton
            // 
            _removePathButton.Location = new Point(191, 336);
            _removePathButton.Name = "_removePathButton";
            _removePathButton.Size = new Size(94, 29);
            _removePathButton.TabIndex = 5;
            _removePathButton.Text = "Remove";
            _removePathButton.UseVisualStyleBackColor = true;
            // 
            // _addFolderPathButton
            // 
            _addFolderPathButton.Location = new Point(29, 336);
            _addFolderPathButton.Name = "_addFolderPathButton";
            _addFolderPathButton.Size = new Size(94, 29);
            _addFolderPathButton.TabIndex = 4;
            _addFolderPathButton.Text = "Add Folder";
            _addFolderPathButton.UseVisualStyleBackColor = true;
            // 
            // _openFileDialog
            // 
            _openFileDialog.FileName = "OpenFileDialog";
            // 
            // _errorProvider
            // 
            _errorProvider.ContainerControl = this;
            // 
            // FormWorkSpaceSetUp
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(_splitContainer);
            Name = "FormWorkSpaceSetUp";
            Text = "WorkSpaceSetUp";
            _splitContainer.Panel1.ResumeLayout(false);
            _splitContainer.Panel1.PerformLayout();
            _splitContainer.Panel2.ResumeLayout(false);
            _splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_splitContainer).EndInit();
            _splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_errorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer _splitContainer;
        private Label _fileGroupListLabel;
        private ListBox _fileGroupListBox;
        private Button _removeFileGroupButton;
        private Button _addFileGroupButton;
        private TextBox _fileGroupNameTextBox;
        private Button _removePathButton;
        private Button _addFolderPathButton;
        private OpenFileDialog _openFileDialog;
        private FolderBrowserDialog _folderBrowserDialog;
        private Button _addFilePathButton;
        private ErrorProvider _errorProvider;
        private ListView _pathDataListView;
        private ColumnHeader _columnHeaderName;
        private ColumnHeader _columnHeaderType;
        private ColumnHeader _columnHeaderPath;
    }
}
