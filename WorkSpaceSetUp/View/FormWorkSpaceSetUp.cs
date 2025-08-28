using System.Windows.Forms;
using WorkSpaceSetUp.Scripts.ErrorHandling;
using WorkSpaceSetUp.Scripts.Model;
using WorkSpaceSetUp.Scripts.ViewModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WorkSpaceSetUp
{
    public partial class FormWorkSpaceSetUp : Form
    {
        #region Variables
        private readonly InteractionFormHandler _interactionFormHandler = new();
        private readonly FormErrorHandling _errorHandling = new();

        #endregion


        #region Initialize
        public FormWorkSpaceSetUp()
        {
            InitializeComponent();
            SetUpEventHandlers();
            LoadSavedData();
        }

      
        private void SetUpEventHandlers()
        {
            _fileGroupListBox.Click += FileGroupList_Click;
            _fileGroupListBox.DoubleClick += FileGroupList_DoubleClick;
            _addFileGroupButton.Click += AddFileGroup_Button_Click;
            _removeFileGroupButton.Click += RemoveFileGroup_Button_Click;
            _fileGroupNameTextBox.TextChanged += FileGroupBane_TextBox_ValueChanged;
            _fileGroupNameTextBox.LostFocus += FileGroupBane_TextBox_LostFocus;
            _addFolderPathButton.Click += AddFolderPath_Button_Click;
            _addFilePathButton.Click += AddFilePath_Button_Click;
            _removePathButton.Click += RemovePath_Button_Click;
            _pathDataListView.Click += FileGroupDataList_Click;

           
        }

        private async void LoadSavedData()
        {
            TResult<FileGroup[]?> tResult = await _interactionFormHandler.AsyncInitialize(_fileGroupListBox.Items);
            _errorHandling.HandleError(tResult);
        }


        #endregion

        #region Events
        private void FileGroupList_Click(object? sender, System.EventArgs e)
        {
            TResult<int> tResult = _interactionFormHandler.Select_FileGroupInList(_fileGroupListBox, _pathDataListView.Items, _fileGroupNameTextBox);
            _errorHandling.HandleError(tResult);
        }
        private void FileGroupList_DoubleClick(object? sender, System.EventArgs e)
        {
            _interactionFormHandler.Open_FileGroupDataInList(_fileGroupListBox);
        }
        private async void AddFileGroup_Button_Click(object? sender, EventArgs e)
        {
            TResult<FileGroup[]?> tResult = await _interactionFormHandler.CreateAndAddFileGroup(_fileGroupListBox, _pathDataListView, _fileGroupNameTextBox);
            _errorHandling.HandleError(tResult);
        }

        private async void RemoveFileGroup_Button_Click(object? sender, EventArgs e)
        {
            TResult<FileGroup[]?> tResult = await _interactionFormHandler.RemoveSelectedFileGroup(_fileGroupListBox, _pathDataListView);
            _errorHandling.HandleError(tResult);
        }

        private void FileGroupBane_TextBox_ValueChanged(object? sender, EventArgs e)
        {
            _interactionFormHandler.UpdateActiveFileGroupName(_fileGroupListBox, _fileGroupNameTextBox);
        }

        private async void FileGroupBane_TextBox_LostFocus(object? sender, EventArgs e)
        {
            TResult<FileGroup[]?> tResult = await _interactionFormHandler.EndUpdateFileGroupName(_fileGroupListBox, _fileGroupNameTextBox);
            _errorHandling.HandleError(tResult);
        }

        private async void AddFolderPath_Button_Click(object? sender, EventArgs e)
        {
            TResult<FileGroup[]?> tResult = await _interactionFormHandler.AddFolderPathToFileGroup(_folderBrowserDialog, _fileGroupListBox, _pathDataListView);
            _errorHandling.HandleError(tResult);
        }
        private async void AddFilePath_Button_Click(object? sender, EventArgs e)
        {
            TResult<FileGroup[]?> tResult = await _interactionFormHandler.AddFilePathToFileGroup(_openFileDialog, _fileGroupListBox, _pathDataListView);
            _errorHandling.HandleError(tResult);
        }

        private void FileGroupDataList_Click(object? sender, EventArgs e)
        {
            TResult<int> tResult  = _interactionFormHandler.Select_FileGroupPathInList(_pathDataListView);
            _errorHandling.HandleError(tResult);
        }

        private async void RemovePath_Button_Click(object? sender, EventArgs e)
        {
            TResult<FileGroup[]?> tResult = await _interactionFormHandler.RemoveSelectedPathOfFileGroup(_pathDataListView);
            _errorHandling.HandleError(tResult);
        }

        #endregion


    }
}
