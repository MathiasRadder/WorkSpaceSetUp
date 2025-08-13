using System;
using System.Collections.Generic;
using System.Diagnostics;
using WorkSpaceSetUp.Scripts.ErrorHandling;

namespace WorkSpaceSetUp.Scripts.Model
{
    public class WorkSpaceModel
    {
        #region Variables
        private readonly FileGroupManager _fileGroupManager = new();
        private readonly ReadWriteFormData _readWriteFormData = new();
        private const string _fileTypeName = "file";
        private const string _folderTypeName = "folder";
        private const char _lastIndexSlash = '\\';
        ErrorTypes _errorTypeOpenCloseFile = ErrorTypes.Warning;

        public FileGroupManager FileGroupMan { get { return _fileGroupManager; } }
        public string FileTypeName {  get { return _fileTypeName; } }
        public string FolderTypeName {  get { return _folderTypeName; } }
        #endregion


        #region Initialize
        public async Task<TResult<FileGroup[]?>> AsyncInitialize()
        {
            FileGroup[]? fileGroupArray = null;
            TResult<FileGroup[]?> resultFileGroup = await _readWriteFormData.LoadFormData();
            if (resultFileGroup.IsSuccess)
                fileGroupArray = resultFileGroup.Value;
            else
                return resultFileGroup;

            if (fileGroupArray != null)
                _fileGroupManager.FileGroupArray = fileGroupArray;

            int highestIndex = 0;
            foreach (var item in _fileGroupManager.FileGroupArray)
            {
                if (item == null || string.IsNullOrEmpty(item.NameFileGroup))
                    continue;
                if (highestIndex < item.IndexOrder)
                    highestIndex = item.IndexOrder;
            }

            _fileGroupManager.InitializeArrayTracker(highestIndex);
            return await _readWriteFormData.WriteFormData(_fileGroupManager.FileGroupArray);
        }
        #endregion


        #region CallMethods
        [Conditional("DEBUG")]
        public void UpdateReadWriteData(string fileNameToTest, string folderPath)
        {
            _readWriteFormData.FileName = fileNameToTest;
            _readWriteFormData.FilePath = folderPath;
        }

        public TResult<int?> Open_SelectedFileGroupPaths()
        {
            TResult<int?>? tResult = null;
            try
            {
                List<PathData> pathDataList = _fileGroupManager.GetSelectedFileGroupData();
                foreach (var item in pathDataList)
                {
                    if (item.DataType == _folderTypeName && !Directory.Exists(item.Path) ||
                        item.DataType == _fileTypeName && !File.Exists(item.Path))
                        continue;//this should give feedback and perhaps handle it

                    ProcessStartInfo psi = new();
                    psi.FileName = item.Path;
                    psi.UseShellExecute = true;
                    Process.Start(psi);
                }
                tResult = TResult<int?>.Success(0);
            }
            catch (Exception ex)
            {
                tResult = TResult<int?>.Failure(new string[1] { ex.Message }, null, _errorTypeOpenCloseFile);
            }
            return tResult;
        }

        public async Task<TResult<FileGroup[]?>> WriteLocalData()
        {
            return await _readWriteFormData.WriteFormData(_fileGroupManager.FileGroupArray);
        }

        public string GetNameFromPath(string path)
        {
            int lastIndexSlash = path.LastIndexOf(_lastIndexSlash) + 1;
            string folderName = string.Empty;
            if (lastIndexSlash < path.Length - 1)
                folderName = path.Substring(lastIndexSlash);
            else
                folderName = path.Substring(0, path.LastIndexOf(_lastIndexSlash));

            return folderName;
        }
        #endregion


        #region PrivateMethods

        #endregion
    }
}
