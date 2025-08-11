using System;
using System.Diagnostics;
using System.Windows.Forms;
using WorkSpaceSetUp.Scripts.Model;


namespace WorkSpaceSetUp.Scripts.ViewModel
{
    public class InteractionFormHandler
    {

        #region Variables
        private WorkSpaceModel _workSpaceModel = new(); 

        private const string _nameNotFound = " not found.";
        private const string _errorMessageMaxSize = "Maximum file group size reached";
        private const string _errorMessageOpenDialog = "Something went wrong, could not choose file/folder.";
        ErrorTypes _errorTypeInteract = ErrorTypes.Error;
        ErrorTypes _errorTypeOpenCloseFile = ErrorTypes.Warning;

        #endregion

        #region Initialize
        public InteractionFormHandler()
        {
        }

        public async Task<TResult<FileGroup[]?>> AsyncInitialize(ListBox.ObjectCollection? fileGroupListBoxItems)
        {
            ArgumentNullException.ThrowIfNull(fileGroupListBoxItems);
            TResult<FileGroup[]?> tResult = await _workSpaceModel.AsyncInitialize();
            if(tResult.IsFailure)
                return tResult;

            foreach (var item in _workSpaceModel.FileGroupMan.FileGroupArray)
            {
                if (item == null || string.IsNullOrEmpty(item.NameFileGroup))
                    continue;
                fileGroupListBoxItems?.Add(item);
            }
            return tResult;
        }

        [Conditional("DEBUG")]
        public async void InitialiseTestingData(FileGroup[] fileManagerFileGroupArray, FileGroup[] arrayToWriteToFile, string fileNameToTest, string folderPath, bool skipWriteData = false)
        {
            _workSpaceModel.FileGroupMan.FileGroupArray = fileManagerFileGroupArray;
            if (!string.IsNullOrEmpty(fileNameToTest) || !string.IsNullOrEmpty(folderPath))
            {
                _workSpaceModel.UpdateReadWriteData(fileNameToTest, folderPath);
            }

            if (!skipWriteData)
                 await _workSpaceModel.WriteLocalData();
        }

        [Conditional("DEBUG")]
        public void SetUpdateTestFileGroupAndPathData(FileGroup[] fileGroup, int selectedIndexFileGroup, string[] pathdata, int indexOfPathData)
        {
            _workSpaceModel.FileGroupMan.FileGroupArray = fileGroup;
            if (selectedIndexFileGroup != -1)
            {
                _workSpaceModel.FileGroupMan.UpdateSelectedIndex(fileGroup[selectedIndexFileGroup].NameFileGroup);
                foreach (var item in pathdata)
                {
                    _workSpaceModel.FileGroupMan.AddPathToSelectedFileGroup(item, item, _workSpaceModel.FolderTypeName);
                }
            }

            if (indexOfPathData != -1)
                _workSpaceModel.FileGroupMan.UpdateSelectedPathIndex(pathdata[indexOfPathData]);
        }

        #endregion

        #region CallMethods
        public TResult<int> Select_FileGroupInList(ListBox fileGroupListBox, ListView.ListViewItemCollection listviewFileDataItems, TextBox textBox)
        {
          
            Debug.Assert(fileGroupListBox != null);
            if (string.IsNullOrEmpty(fileGroupListBox?.SelectedItem?.ToString()))
                return TResult<int>.Success(0);

            string? name = fileGroupListBox.SelectedItem?.ToString();
            if (_workSpaceModel.FileGroupMan.UpdateSelectedIndex(name) == false)
            {
                fileGroupListBox.ClearSelected();
                return TResult<int>.Failure(new string[] { name?.ToString() + _nameNotFound }, System.Reflection.MethodBase.GetCurrentMethod()?.Name, _errorTypeInteract);
            }
            if (textBox != null)
                textBox.Text = _workSpaceModel.FileGroupMan.SelectedFileGroupName;

            listviewFileDataItems?.Clear();
            List<PathData> pathDataList = _workSpaceModel.FileGroupMan.GetSelectedFileGroupData();
          
            pathDataList.ForEach(item => {
                ListViewItem listItem = new(new string[] { item.Name, item.DataType, item.Path });//This is pretty bad, this should be not by hand
                listviewFileDataItems?.Add(listItem); 
            });
            return TResult<int>.Success(0);
        }

        public TResult<int?> Open_FileGroupDataInList(ListBox fileGrouplistBox)
        {
            Debug.Assert(fileGrouplistBox != null);
            if (fileGrouplistBox.SelectedIndex == -1)
                return TResult<int?>.Success(0);

            return _workSpaceModel.Open_SelectedFileGroupPaths();
        }

       

        public async Task<TResult<FileGroup[]?>> CreateAndAddFileGroup(ListBox listBox, ListView listView, TextBox textBox)
        {
            string? fileGroupName = _workSpaceModel.FileGroupMan.CreateFileGroup();
            if (string.IsNullOrEmpty(fileGroupName))
                return TResult<FileGroup[]?>.Failure(new string[] { _errorMessageMaxSize }, System.Reflection.MethodBase.GetCurrentMethod()?.ToString(), _errorTypeOpenCloseFile);

            listBox.Items.Add(fileGroupName);
            listBox.SelectedIndex = listBox.Items.Count - 1;
            Select_FileGroupInList(listBox, listView.Items, textBox);

            return await _workSpaceModel.WriteLocalData();
        }

     
        public async Task<TResult<FileGroup[]?>> RemoveSelectedFileGroup(ListBox listBox, ListView listView)
        {
            if (listBox.SelectedIndex == -1)
                return TResult<FileGroup[]?>.Success(null);

            listBox.Items.RemoveAt(listBox.SelectedIndex);
            listBox.SelectedIndex = -1;
            listView.Items.Clear();
            _workSpaceModel.FileGroupMan.RemoveSelectedFileGroup();
            return await _workSpaceModel.WriteLocalData();
        }

        public void UpdateActiveFileGroupName(ListBox listBox, TextBox textBox)
        {
            if (listBox.SelectedIndex == -1)
                return;

            listBox.Items[listBox.SelectedIndex] = textBox.Text;
            _workSpaceModel.FileGroupMan.SelectedFileGroupName = textBox.Text;
        }

        public async Task<TResult<FileGroup[]?>> EndUpdateFileGroupName(ListBox listBox, TextBox textBox)
        {
            if (listBox.SelectedIndex == -1)
                return TResult<FileGroup[]?>.Success(null);

            string name = textBox.Text;
            if (string.IsNullOrEmpty(name))
                name = _workSpaceModel.FileGroupMan.ResetDefaultFileGroupName();

            listBox.Items[listBox.SelectedIndex] = name;
            _workSpaceModel.FileGroupMan.SelectedFileGroupName = name;

            return await _workSpaceModel.WriteLocalData();
        }

      
        public async Task<TResult<FileGroup[]?>> AddFolderPathToFileGroup(FolderBrowserDialog folderBrowserDialog, ListBox listBoxFileGroups, ListView listView)
        {
            if (listBoxFileGroups.SelectedIndex == -1)
                return TResult<FileGroup[]?>.Success(null);

            string type = _workSpaceModel.FolderTypeName;
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.Cancel)
                return TResult<FileGroup[]?>.Success(null);
            else if (result != DialogResult.OK)
                return TResult<FileGroup[]?>.Failure
                    (new string[] { _errorMessageOpenDialog }, System.Reflection.MethodBase.GetCurrentMethod()?.Name, _errorTypeOpenCloseFile);

            string pathName = _workSpaceModel.GetNameFromPath(folderBrowserDialog.SelectedPath);
            AddNewPath(pathName, folderBrowserDialog.SelectedPath, type, listView);
            return await _workSpaceModel.WriteLocalData();
        }

        public async Task<TResult<FileGroup[]?>> AddFilePathToFileGroup(OpenFileDialog openFileDialog, ListBox listBoxFileGroups, ListView listView)
        {
            if (listBoxFileGroups.SelectedIndex == -1)
                return TResult<FileGroup[]?>.Success(null);
            string type = _workSpaceModel.FileTypeName;
            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.Cancel)
                return TResult<FileGroup[]?>.Success(null);
            else if (result != DialogResult.OK)
                return TResult<FileGroup[]?>.Failure
                    (new string[] { _errorMessageOpenDialog }, System.Reflection.MethodBase.GetCurrentMethod()?.Name, _errorTypeOpenCloseFile);

            string pathName = _workSpaceModel.GetNameFromPath(openFileDialog.FileName);
            AddNewPath(pathName, openFileDialog.FileName, type, listView);
            return await _workSpaceModel.WriteLocalData();
        }     

        public async Task<TResult<FileGroup[]?>> RemoveSelectedPathOfFileGroup(ListView listView)
        {
            if (listView.SelectedIndices.Count == 0 || listView.SelectedIndices[0] == -1)
                return TResult<FileGroup[]?>.Success(null);

            listView.Items.RemoveAt(listView.SelectedIndices[0]);
            listView.SelectedIndices.Clear();
            listView.SelectedItems.Clear();
            _workSpaceModel.FileGroupMan.RemoveSelectedFilePath();
            return await _workSpaceModel.WriteLocalData();
        }

        public TResult<int> Select_FileGroupPathInList(ListView listView)
        {
            if (string.IsNullOrEmpty(listView?.SelectedItems[0]?.ToString()))
                return TResult<int>.Success(0);

            string? name = listView.SelectedItems[0].Text;
            if (_workSpaceModel.FileGroupMan.UpdateSelectedPathIndex(name) == false)
            {
                listView.SelectedItems.Clear();
                return TResult<int>.Failure(new string[] { name?.ToString() + _nameNotFound }, System.Reflection.MethodBase.GetCurrentMethod()?.Name, _errorTypeInteract);
            }
            return TResult<int>.Success(0);
        }
        #endregion


        #region PrivateMethods     
        private void AddNewPath(string folderName,string path, string type, ListView listView)
        {
            _workSpaceModel.FileGroupMan.AddPathToSelectedFileGroup(folderName, path, type);
            ListViewItem listItem = new(new string[] { folderName, type, path });
            listView.Items.Add(listItem);
        }
        #endregion
    }
}
