using System.Windows.Forms;
using WorkSpaceSetUp.Scripts;
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
            FileGroupList.Click += FileGroupList_Click;
            FileGroupList.DoubleClick += FileGroupList_DoubleClick;
            AddFileGroup_Button.Click += AddFileGroup_Button_Click;
            RemoveFileGroup_Button.Click += RemoveFileGroup_Button_Click;
            FileGroupName_TextBox.TextChanged += FileGroupBane_TextBox_ValueChanged;
            FileGroupName_TextBox.LostFocus += FileGroupBane_TextBox_LostFocus;
            AddFolderPath_Button.Click += AddFolderPath_Button_Click;
            AddFilePath_Button.Click += AddFilePath_Button_Click;
            RemovePath_Button.Click += RemovePath_Button_Click;
            list_View.Click += FileGroupDataList_Click;

           
        }

        private async void LoadSavedData()
        {
            TResult<FileGroup[]?> tResult = await _interactionFormHandler.AsyncInitialize(FileGroupList.Items);
            _errorHandling.HandleError(tResult);
        }


        #endregion

        #region Events
        private void FileGroupList_Click(object? sender, System.EventArgs e)
        {
            TResult<int> tResult = _interactionFormHandler.Select_FileGroupInList(FileGroupList, list_View.Items, FileGroupName_TextBox);
            _errorHandling.HandleError(tResult);
        }
        private void FileGroupList_DoubleClick(object? sender, System.EventArgs e)
        {
            _interactionFormHandler.Open_FileGroupDataInList(FileGroupList);
        }
        private async void AddFileGroup_Button_Click(object? sender, EventArgs e)
        {
            TResult<FileGroup[]?> tResult = await _interactionFormHandler.CreateAndAddFileGroup(FileGroupList, list_View, FileGroupName_TextBox);
            _errorHandling.HandleError(tResult);
        }

        private async void RemoveFileGroup_Button_Click(object? sender, EventArgs e)
        {
            TResult<FileGroup[]?> tResult = await _interactionFormHandler.RemoveSelectedFileGroup(FileGroupList, list_View);
            _errorHandling.HandleError(tResult);
        }

        private void FileGroupBane_TextBox_ValueChanged(object? sender, EventArgs e)
        {
            _interactionFormHandler.UpdateActiveFileGroupName(FileGroupList, FileGroupName_TextBox);
        }

        private async void FileGroupBane_TextBox_LostFocus(object? sender, EventArgs e)
        {
            TResult<FileGroup[]?> tResult = await _interactionFormHandler.EndUpdateFileGroupName(FileGroupList, FileGroupName_TextBox);
            _errorHandling.HandleError(tResult);
        }

        private async void AddFolderPath_Button_Click(object? sender, EventArgs e)
        {
            TResult<FileGroup[]?> tResult = await _interactionFormHandler.AddFolderPathToFileGroup(FolderBrowserDialog1, FileGroupList, list_View);
            _errorHandling.HandleError(tResult);
        }
        private async void AddFilePath_Button_Click(object? sender, EventArgs e)
        {
            TResult<FileGroup[]?> tResult = await _interactionFormHandler.AddFilePathToFileGroup(OpenFileDialog1, FileGroupList, list_View);
            _errorHandling.HandleError(tResult);
        }

        private void FileGroupDataList_Click(object? sender, EventArgs e)
        {
            TResult<int> tResult  = _interactionFormHandler.Select_FileGroupPathInList(list_View);
            _errorHandling.HandleError(tResult);
        }

        private async void RemovePath_Button_Click(object? sender, EventArgs e)
        {
            TResult<FileGroup[]?> tResult = await _interactionFormHandler.RemoveSelectedPathOfFileGroup(list_View);
            _errorHandling.HandleError(tResult);
        }

        #endregion


    }
}
