using System;

namespace WorkSpaceSetUp.Scripts.Model
{
    public class FileGroupManager
    {
        #region Variables
        private const int _maxContainerSize = 64;
        private FileGroup[] _fileGroupArray;
        private int _nextAvailableIndex = 0;
        private int _selectedFileGroupIndex = -1;
        private int _fileGroupCreatedAmount = 0;

        private const string _defaultFileGroupName = "FileGroup";

        public string SelectedFileGroupName
        {
            get { return _fileGroupArray[_selectedFileGroupIndex].NameFileGroup; }
            set { _fileGroupArray[_selectedFileGroupIndex].NameFileGroup = value; }
        }

        public FileGroup[] FileGroupArray
        {
            get { return _fileGroupArray; }
            set { _fileGroupArray = value; }
        }
            


        #endregion

        #region Initialize
        public FileGroupManager() 
        {
            int arraySize = Math.Clamp(Properties.Settings.Default.FileGroupDataAmount, 1, _maxContainerSize);
            _fileGroupArray = new FileGroup[arraySize];
        }
        #endregion

        #region CallMethods
        public string CreateFileGroup()
        {
            SortFileGroupArray();
            if (_nextAvailableIndex >= _fileGroupArray.Length || _nextAvailableIndex == -1)
                return string.Empty;

            _fileGroupCreatedAmount++;
            int availableIndex = _nextAvailableIndex;
            if (_fileGroupArray[availableIndex] == null)
                _fileGroupArray[availableIndex] = new FileGroup(_defaultFileGroupName + " " + _fileGroupCreatedAmount, _fileGroupCreatedAmount);
            else
                _fileGroupArray[availableIndex].Fill(_defaultFileGroupName + " " + _fileGroupCreatedAmount, _fileGroupCreatedAmount);
            _nextAvailableIndex++;
            return _fileGroupArray[availableIndex].NameFileGroup;
        }

        public void RemoveSelectedFileGroup()
        {
            if (_selectedFileGroupIndex == -1)
                return;

            _fileGroupArray[_selectedFileGroupIndex].Reset();
            _selectedFileGroupIndex = -1;
        }

        public void RemoveSelectedFilePath()
        {
            if (_selectedFileGroupIndex == -1)
                return;

            _fileGroupArray[_selectedFileGroupIndex].RemoveSelectedDataPath();
        }


        public bool UpdateSelectedIndex(string? name)
        {
            _selectedFileGroupIndex = Array.FindIndex(_fileGroupArray, fileGroup => fileGroup?.NameFileGroup == name);
            return _selectedFileGroupIndex != -1;
        }

        public bool UpdateSelectedPathIndex(string? name)
        {
            if (_selectedFileGroupIndex == -1)
                return false;
            return _fileGroupArray[_selectedFileGroupIndex].UpdateSelectedIndex(name);
        }

        public void AddPathToSelectedFileGroup(string name, string path, string type)
        {
            _fileGroupArray[_selectedFileGroupIndex].AddDataPath(name, path, type);
        }

        public string ResetDefaultFileGroupName()
        {
            return _defaultFileGroupName + " " + _selectedFileGroupIndex.ToString();
        }

        public List<PathData> GetSelectedFileGroupData()
        {
            if (_selectedFileGroupIndex == -1)
                return [];
            
            return _fileGroupArray[_selectedFileGroupIndex].PathDataList;
        }

        public void InitializeArrayTracker(int amountEverCreated)
        {
            _fileGroupCreatedAmount = amountEverCreated;
            SortFileGroupArray(true);

        }
        #endregion

        #region PrivateMethods
        private void SortFileGroupArray(bool orderRegardless = false)
        {
            if (!orderRegardless && _nextAvailableIndex < _fileGroupArray.Length && _nextAvailableIndex != -1)
                return;

            Array.Sort(_fileGroupArray,
            new Comparison<FileGroup>(
            (i1, i2) =>
            {
                if (i1 == null || i1.IndexOrder == -1) return 0;
                return i1.CompareTo(i2);
            }));
           
            _nextAvailableIndex = Array.FindIndex(_fileGroupArray,
            fileGroup => fileGroup == null || fileGroup.IndexOrder == -1);
        }
        #endregion
    }
}

