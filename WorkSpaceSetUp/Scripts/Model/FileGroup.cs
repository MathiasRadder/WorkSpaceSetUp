using System;
using Newtonsoft.Json;


namespace WorkSpaceSetUp.Scripts.Model
{
    public class FileGroup : IComparable<FileGroup>
    {
        #region Variables
        private string _name = string.Empty;
        private int _indexArray = -1;
        [JsonProperty(nameof(NameFileGroup))]
        public string NameFileGroup { get => _name; set => _name = value; }
        [JsonProperty(nameof(IndexOrder))]
        public int IndexOrder { get => _indexArray; set => _indexArray = value; }

        private readonly List<PathData> _pathDataList = [];
        private int _selectedPathDataIndex = -1;

        [JsonProperty(nameof(PathDataList))]
        public List<PathData> PathDataList { get { return _pathDataList; } }

       


        #endregion

        #region Initialize
        public FileGroup(string name, int indexArray = -1)
        {
            _name = name;
            _indexArray = indexArray;
        }
        public override string ToString() { return NameFileGroup; }
        #endregion

        #region CallMethods
        public void AddDataPath(string name, string path, string type)
        {
            _pathDataList.Add(new PathData(name, path, type));
        }

        public void AddDataPath(PathData pathData)
        {
            _pathDataList.Add(pathData);
        }

        public void RemoveSelectedDataPath()
        {
            if (_selectedPathDataIndex == -1)
                return;

            _pathDataList.RemoveAt(_selectedPathDataIndex);
            _selectedPathDataIndex = -1;
        }

        public bool UpdateSelectedIndex(string? name)
        {
            _selectedPathDataIndex = _pathDataList.FindIndex(fileGroup => fileGroup.Name == name);
            return _selectedPathDataIndex != -1;
        }

        public void Fill(string name, int indexArray)
        {
            _name = name;
            _indexArray = indexArray;
        }
        public void Reset()
        {
            NameFileGroup = string.Empty;
            _pathDataList.Clear();
            _selectedPathDataIndex = -1;
            _indexArray = -1;
        }
        #endregion

        #region PrivateMethods
        public int CompareTo(FileGroup? obj)
        {
            if (obj == null || obj.IndexOrder == -1)
                return -1;

            if (_indexArray == obj.IndexOrder)
                return 0;

            return _indexArray.CompareTo(obj.IndexOrder);
        }

        #endregion
    }
}
