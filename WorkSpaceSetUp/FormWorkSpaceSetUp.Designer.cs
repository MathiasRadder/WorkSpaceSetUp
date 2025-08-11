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
            splitContainer1 = new SplitContainer();
            RemoveFileGroup_Button = new Button();
            AddFileGroup_Button = new Button();
            FileGroupListLabel = new Label();
            FileGroupList = new ListBox();
            list_View = new ListView();
            CollumHeaderName = new ColumnHeader();
            CollumHeaderType = new ColumnHeader();
            CollumHeaderPath = new ColumnHeader();
            AddFilePath_Button = new Button();
            FileGroupName_TextBox = new TextBox();
            RemovePath_Button = new Button();
            AddFolderPath_Button = new Button();
            OpenFileDialog1 = new OpenFileDialog();
            FolderBrowserDialog1 = new FolderBrowserDialog();
            error_Provider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)error_Provider).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(RemoveFileGroup_Button);
            splitContainer1.Panel1.Controls.Add(AddFileGroup_Button);
            splitContainer1.Panel1.Controls.Add(FileGroupListLabel);
            splitContainer1.Panel1.Controls.Add(FileGroupList);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(list_View);
            splitContainer1.Panel2.Controls.Add(AddFilePath_Button);
            splitContainer1.Panel2.Controls.Add(FileGroupName_TextBox);
            splitContainer1.Panel2.Controls.Add(RemovePath_Button);
            splitContainer1.Panel2.Controls.Add(AddFolderPath_Button);
            splitContainer1.Size = new Size(800, 450);
            splitContainer1.SplitterDistance = 375;
            splitContainer1.TabIndex = 0;
            // 
            // RemoveFileGroup_Button
            // 
            RemoveFileGroup_Button.Location = new Point(174, 336);
            RemoveFileGroup_Button.Name = "RemoveFileGroup_Button";
            RemoveFileGroup_Button.Size = new Size(94, 29);
            RemoveFileGroup_Button.TabIndex = 3;
            RemoveFileGroup_Button.Text = "Remove";
            RemoveFileGroup_Button.UseVisualStyleBackColor = true;
            // 
            // AddFileGroup_Button
            // 
            AddFileGroup_Button.Location = new Point(12, 336);
            AddFileGroup_Button.Name = "AddFileGroup_Button";
            AddFileGroup_Button.Size = new Size(94, 29);
            AddFileGroup_Button.TabIndex = 2;
            AddFileGroup_Button.Text = "Add";
            AddFileGroup_Button.UseVisualStyleBackColor = true;
            // 
            // FileGroupListLabel
            // 
            FileGroupListLabel.AutoSize = true;
            FileGroupListLabel.Location = new Point(12, 23);
            FileGroupListLabel.Name = "FileGroupListLabel";
            FileGroupListLabel.Size = new Size(103, 20);
            FileGroupListLabel.TabIndex = 1;
            FileGroupListLabel.Text = "File Group List";
            // 
            // FileGroupList
            // 
            FileGroupList.FormattingEnabled = true;
            FileGroupList.Location = new Point(12, 46);
            FileGroupList.Name = "FileGroupList";
            FileGroupList.Size = new Size(256, 284);
            FileGroupList.TabIndex = 0;
            // 
            // list_View
            // 
            list_View.Columns.AddRange(new ColumnHeader[] { CollumHeaderName, CollumHeaderType, CollumHeaderPath });
            list_View.FullRowSelect = true;
            list_View.Location = new Point(29, 46);
            list_View.MultiSelect = false;
            list_View.Name = "list_View";
            list_View.Size = new Size(264, 284);
            list_View.TabIndex = 8;
            list_View.UseCompatibleStateImageBehavior = false;
            list_View.View = View.Details;
            // 
            // CollumHeaderName
            // 
            CollumHeaderName.Text = "Name";
            // 
            // CollumHeaderType
            // 
            CollumHeaderType.Text = "Type";
            // 
            // CollumHeaderPath
            // 
            CollumHeaderPath.Text = "Path";
            CollumHeaderPath.Width = 180;
            // 
            // AddFilePath_Button
            // 
            AddFilePath_Button.Location = new Point(29, 385);
            AddFilePath_Button.Name = "AddFilePath_Button";
            AddFilePath_Button.Size = new Size(94, 29);
            AddFilePath_Button.TabIndex = 7;
            AddFilePath_Button.Text = "Add File";
            AddFilePath_Button.UseVisualStyleBackColor = true;
            // 
            // FileGroupName_TextBox
            // 
            FileGroupName_TextBox.Location = new Point(29, 16);
            FileGroupName_TextBox.Name = "FileGroupName_TextBox";
            FileGroupName_TextBox.Size = new Size(125, 27);
            FileGroupName_TextBox.TabIndex = 6;
            // 
            // RemovePath_Button
            // 
            RemovePath_Button.Location = new Point(191, 336);
            RemovePath_Button.Name = "RemovePath_Button";
            RemovePath_Button.Size = new Size(94, 29);
            RemovePath_Button.TabIndex = 5;
            RemovePath_Button.Text = "Remove";
            RemovePath_Button.UseVisualStyleBackColor = true;
            // 
            // AddFolderPath_Button
            // 
            AddFolderPath_Button.Location = new Point(29, 336);
            AddFolderPath_Button.Name = "AddFolderPath_Button";
            AddFolderPath_Button.Size = new Size(94, 29);
            AddFolderPath_Button.TabIndex = 4;
            AddFolderPath_Button.Text = "Add Folder";
            AddFolderPath_Button.UseVisualStyleBackColor = true;
            // 
            // OpenFileDialog1
            // 
            OpenFileDialog1.FileName = "OpenFileDialog1";
            // 
            // error_Provider
            // 
            error_Provider.ContainerControl = this;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(splitContainer1);
            Name = "Form1";
            Text = "WorkSpaceSetUp";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)error_Provider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Label FileGroupListLabel;
        private ListBox FileGroupList;
        private Button RemoveFileGroup_Button;
        private Button AddFileGroup_Button;
        private TextBox FileGroupName_TextBox;
        private Button RemovePath_Button;
        private Button AddFolderPath_Button;
        private OpenFileDialog OpenFileDialog1;
        private FolderBrowserDialog FolderBrowserDialog1;
        private Button AddFilePath_Button;
        private ErrorProvider error_Provider;
        private ListView list_View;
        private ColumnHeader CollumHeaderName;
        private ColumnHeader CollumHeaderType;
        private ColumnHeader CollumHeaderPath;
    }
}
