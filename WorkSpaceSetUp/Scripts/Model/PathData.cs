using System;
using Newtonsoft.Json;


namespace WorkSpaceSetUp.Scripts.Model
{
    public class PathData
    {
        [JsonProperty(nameof(Name))]
        public string Name { get; set; }

        [JsonProperty(nameof(Path))]
        public string Path { get; set; }
        [JsonProperty(nameof(DataType))]
        public string DataType { get; set; }

        public PathData(string name, string path, string dataType)
        {
            Name = name;
            Path = path;
            DataType = dataType;
        }

        public PathData()
        {
            Name = string.Empty;
            Path = string.Empty;
            DataType = string.Empty;
        }

        public void Reset()
        {
            Name = string.Empty;
            Path = string.Empty;
            DataType = string.Empty;
        }

        public override string ToString()
        {
            return Name;
        }
    }

}
