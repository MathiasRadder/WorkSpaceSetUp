using System;
using System.Diagnostics;
using System.Windows.Forms;
using WorkSpaceSetUp.Scripts.ErrorHandling;
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
        /// <summary>
        /// Initializes asynchronous the model <see cref="WorkSpaceModel.AsyncInitialize"/> and 
        /// gets the filegroup data from the model to fill in the control collection <see cref="ListBox.ObjectCollection"/>
        /// </summary>
        /// <param name="fileGroupListBoxItems"> Obligated control collection <see cref="ListBox.ObjectCollection"/> to update the UI</param>
        /// <returns> Returns a Returns a <see cref="TResult{FileGroup[]?}"/></returns>
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
        /// <summary>
        /// It checks if the selected control <see cref="ListBox"/> <paramref name="fileGroupListBox"/> is correct with the model data. <see cref="FileGroupManager.UpdateSelectedIndex(string?)"/>
        /// If so, it updates the UI controls <see cref=" ListView.ListViewItemCollection"/>, <see cref="TextBox"/> to show the selected data.
        /// </summary>
        /// <param name="fileGroupListBox">Obligated control <see cref="ListBox"/> to check if its selected data is correct with the model data.</param>
        /// <param name="listviewFileDataItems">Optional collection in control <see cref=" ListView.ListViewItemCollection"/> to update the selected filegroup data </param>
        /// <param name="textBox">Optional control <see cref="TextBox"/> to update the selected filegroup name </param>
        /// <returns> Returns a <see cref="TResult{int}"/> </returns>
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


        /// <summary>
        /// Opens the files and folders in the selected filegroup.
        /// </summary>
        /// <param name="fileGrouplistBox"> Obligated control <see cref="ListBox"> that contains the selected name of the filegroup</param>
        /// <returns> Returns a <see cref="TResult{int}"/></returns>
        public TResult<int?> Open_FileGroupDataInList(ListBox fileGrouplistBox)
        {
            Debug.Assert(fileGrouplistBox != null);
            if (fileGrouplistBox.SelectedIndex == -1)
                return TResult<int?>.Success(0);

            return _workSpaceModel.Open_SelectedFileGroupPaths();
        }


        /// <summary>
        /// Creates a new file group from the model <see cref="FileGroupManager.CreateFileGroup"/> and adds the filegroup name to <see cref="Listbox"/>.
        ///Then selects it and updates the UI accordingly. Then writes the new updated data to the chosen file, <see cref="ReadWriteFormData.WriteFormData(FileGroup[])"/>.
        /// </summary>
        /// <param name="fileGroupListBox"> Obligated control <see cref="ListBox"/> to add and select the created filegroup name</param>
        /// <param name="listView">Optional control <see cref="ListView"/> to update UI and show list of paths from selected filegroup <see cref="Select_FileGroupInList"/> </param>
        /// <param name="textBox">Optional control <see cref="TextBox"/> to update UI and name of selected filegroup <see cref="Select_FileGroupInList"/></param>
        /// <returns> Representing the async operation which contains a return of <see cref="TResult{FileGroup[]?}"/> </returns>
        public async Task<TResult<FileGroup[]?>> CreateAndAddFileGroup(ListBox fileGroupListBox, ListView listView, System.Windows.Forms.TextBox textBox)
        {
            Debug.Assert(fileGroupListBox != null);
            string? fileGroupName = _workSpaceModel.FileGroupMan.CreateFileGroup();
            if (string.IsNullOrEmpty(fileGroupName))
                return TResult<FileGroup[]?>.Failure(new string[] { _errorMessageMaxSize }, System.Reflection.MethodBase.GetCurrentMethod()?.ToString(), _errorTypeOpenCloseFile);

            fileGroupListBox.Items.Add(fileGroupName);
            fileGroupListBox.SelectedIndex = fileGroupListBox.Items.Count - 1;
            Select_FileGroupInList(fileGroupListBox, listView.Items, textBox);

            return await _workSpaceModel.WriteLocalData();
        }

        /// <summary>
        /// Removes the selected item in the control <see cref="ListBox"/> <param name="fileGroupListBox"/>, 
        /// clears the collection in control <see cref="ListView"> and updates the UI accordingly. 
        /// It also removes the selected local data from the model <see cref="FileGroupManager.RemoveSelectedFileGroup"/> and
        /// then writes the new updated data to the chosen file, <see cref="ReadWriteFormData.WriteFormData(FileGroup[])"/>.
        /// </summary>
        /// <param name="fileGroupListBox"> Obligated control <see cref="ListBox"/> to remove the selected item. </param>
        /// <param name="listView">Optional control <see cref="ListView"/> to clear its collection.</param>
        /// <returns> Representing the async operation which contains a return of <see cref="TResult{FileGroup[]?}"/><</returns>
        public async Task<TResult<FileGroup[]?>> RemoveSelectedFileGroup(ListBox fileGroupListBox, ListView listView)
        {
            Debug.Assert(fileGroupListBox != null);
            if (fileGroupListBox.SelectedIndex == -1)
                return TResult<FileGroup[]?>.Success(null);

            fileGroupListBox.Items.RemoveAt(fileGroupListBox.SelectedIndex);
            fileGroupListBox.SelectedIndex = -1;
            listView?.Items.Clear();
            _workSpaceModel.FileGroupMan.RemoveSelectedFileGroup();
            return await _workSpaceModel.WriteLocalData();
        }

        /// <summary>
        /// Updates the name of the selected control <see cref="ListBox"/> <param name="fileGroupListBox"/> via the control <see cref="TextBox"/> <param name="textBox"/>
        /// newly updated text and updates the UI accordingly.
        /// </summary>
        /// <param name="fileGroupListBox">Obligated control <see cref="ListBox"/> to update the selected item name.</param>
        /// <param name="textBox">Obligated control <see cref="TextBox"/> to retrieve the text that has been put in.</param>
        public void UpdateActiveFileGroupName(ListBox fileGroupListBox, TextBox textBox)
        {
            Debug.Assert(fileGroupListBox != null && textBox != null);
            if (fileGroupListBox.SelectedIndex == -1)
                return;

            fileGroupListBox.Items[fileGroupListBox.SelectedIndex] = textBox.Text;
            _workSpaceModel.FileGroupMan.SelectedFileGroupName = textBox.Text;
        }
        /// <summary>
        /// Checks and updates the name of the selected control <see cref="ListBox"/> <param name="fileGroupListBox"/> via the control <see cref="TextBox"/> <param name="textBox"/>
        /// newly updated text and updates the UI accordingly. Resets the name if is incorrect <see cref="string.IsNullOrEmpty"/> to a default name <see cref="FileGroupManager.ResetDefaultFileGroupName"/>
        /// Then writes the new updated data to the chosen file, <see cref="ReadWriteFormData.WriteFormData(FileGroup[])"/>.
        /// </summary>
        /// <param name="fileGroupListBox">Obligated control <see cref="ListBox"/> to update the selected item name.</param>
        /// <param name="textBox">Obligated control <see cref="TextBox"/> to retrieve the text that has been put in.</param>
        /// <returns>Representing the async operation which contains a return of <see cref="TResult{FileGroup[]?}"/></returns>
        public async Task<TResult<FileGroup[]?>> EndUpdateFileGroupName(ListBox fileGroupListBox, TextBox textBox)
        {
            Debug.Assert(fileGroupListBox != null && textBox != null);
            if (fileGroupListBox.SelectedIndex == -1)
                return TResult<FileGroup[]?>.Success(null);

            string name = textBox.Text;
            if (string.IsNullOrEmpty(name))
                name = _workSpaceModel.FileGroupMan.ResetDefaultFileGroupName();

            fileGroupListBox.Items[fileGroupListBox.SelectedIndex] = name;
            _workSpaceModel.FileGroupMan.SelectedFileGroupName = name;

            return await _workSpaceModel.WriteLocalData();
        }

        /// <summary>
        /// Via the CommonDialog <see cref="FolderBrowserDialog"\> <param name="folderBrowserDialog"\>, opens the folder browser dialog <see cref="FolderBrowserDialog.ShowDialog"\> and adds the folder-path, type and name 
        /// to selected filegroup data in the model <see cref="AddNewPath"\>and updates the control <see cref="ListView"\> <param name="listView"\> with the newly given data and updates the UI accordingly.
        ///  Then writes the new updated data to the chosen file, <see cref="ReadWriteFormData.WriteFormData(FileGroup[])"/>.
        /// </summary>
        /// <param name="folderBrowserDialog">Obligated CommonDialog <see cref="FolderBrowserDialog"/> opens a folder dialog to catch the data from it.</param>
        /// <param name="fileGroupListBox">Obligated control <see cref="ListBox"/> to check the selected item.</param>
        /// <param name="listView">Optional  control <see cref="ListView"/> to update the UI by adding the new path. <see cref="AddNewPath"></param>
        /// <returns>Representing the async operation which contains a return of <see cref="TResult{FileGroup[]?}"/></returns>
        public async Task<TResult<FileGroup[]?>> AddFolderPathToFileGroup(FolderBrowserDialog folderBrowserDialog, ListBox fileGroupListBox, ListView listView)
        {
            Debug.Assert(fileGroupListBox != null && folderBrowserDialog != null);
            if (fileGroupListBox.SelectedIndex == -1)
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


        /// <summary>
        /// Via the FileDialog  <see cref="OpenFileDialog"\> <param name="openFileDialog"\>, opens the file browser dialog <see cref="OpenFileDialog.ShowDialog"\> and adds the file-path, type and name 
        /// to selected filegroup data in the model <see cref="AddNewPath"\>and updates the control <see cref="ListView"\> <param name="listView"\> with the newly given data and updates the UI accordingly.
        /// Then writes the new updated data to the chosen file, <see cref="ReadWriteFormData.WriteFormData(FileGroup[])"/>.
        /// </summary>
        /// <param name="openFileDialog">Obligated FileDialog <see cref="OpenFileDialog"/> opens a file dialog to catch the data from it.</param>
        /// <param name="fileGroupListBox">Obligated control <see cref="ListBox"/> to check the selected item.</param>
        /// <param name="listView">Optional  control <see cref="ListView"/> to update the UI by adding the new path. <see cref="AddNewPath"></param>
        /// <returns>Representing the async operation which contains a return of <see cref="TResult{FileGroup[]?}"/></returns>
        public async Task<TResult<FileGroup[]?>> AddFilePathToFileGroup(OpenFileDialog openFileDialog, ListBox fileGroupListBox, ListView listView)
        {
            Debug.Assert(fileGroupListBox != null && openFileDialog != null);
            if (fileGroupListBox.SelectedIndex == -1)
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

        /// <summary>
        /// Checks and removes the singular selected path from the control <see cref="ListView"\> <param name="listView"\> and updates the UI accordingly.
        /// After that it also removes the selected data in the model, <see cref="FileGroupManager.RemoveSelectedFilePath"\>.
        /// Then writes the new updated data to the chosen file, <see cref="ReadWriteFormData.WriteFormData(FileGroup[])"/>.
        /// </summary>
        /// <param name="listView">Obligated control <see cref="ListView"\> where we retrieve the singular selected data to remove.</param>
        /// <returns>Representing the async operation which contains a return of <see cref="TResult{FileGroup[]?}"/></returns>
        public async Task<TResult<FileGroup[]?>> RemoveSelectedPathOfFileGroup(ListView listView)
        {
            Debug.Assert(listView != null);
            if (listView.SelectedIndices.Count == 0 || listView.SelectedIndices[0] == -1)
                return TResult<FileGroup[]?>.Success(null);

            listView.Items.RemoveAt(listView.SelectedIndices[0]);
            listView.SelectedIndices.Clear();
            listView.SelectedItems.Clear();
            _workSpaceModel.FileGroupMan.RemoveSelectedFilePath();
            return await _workSpaceModel.WriteLocalData();
        }

        /// <summary>
        /// It checks if the selected control <see cref="ListView"/> <param name="listView"/> is correct with the selected model data.
        /// If so then it will update the data in the model as well  <see cref="FileGroupManager.UpdateSelectedPathIndex(string?)"/>
        /// </summary>
        /// <param name="listView">Obligated control <see cref="ListView"/> to retrieve the selected data.</param>
        /// <returns>It returns a <see cref="TResult{int}"/></returns>
        public TResult<int> Select_FileGroupPathInList(ListView listView)
        {
            Debug.Assert(listView != null);
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
