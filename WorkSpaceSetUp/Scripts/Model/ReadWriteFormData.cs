using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using System.Collections.Generic;
using WorkSpaceSetUp.Scripts.ErrorHandling;


namespace WorkSpaceSetUp.Scripts.Model
{
    internal class ReadWriteFormData
    {
        #region Variables
        private const string _folderName = "SaveFolder";
        private const string _fileType = ".json";
        private string _filePath = string.Empty;
        private string _fileName = string.Empty; 
        private readonly JsonSerializerSettings _jsonSettings = new();

        private const string _errorMessageJson = "Could not read/load file. The File might be corrupted.";
        private const string _loadDataOrigin = "Load data error.";
        private const string _writeDataOrigin = "Save data error.";
        ErrorTypes _errorType = ErrorTypes.Error;
        public string FileName { get {return _fileName; } set { _fileName = value; } }
        public string FilePath { get { return _filePath; } set { _filePath = value; } }
        #endregion

        #region Initialize
        public ReadWriteFormData()
        {
            _jsonSettings = new JsonSerializerSettings
            {
                Error = (sender, args) =>
                {
                    args.ErrorContext.Handled = true;
                }
            };

            _fileName = Properties.Settings.Default.SaveFileName;
        
            string workingDirectory = Environment.CurrentDirectory;
            string? projectDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName;
            _filePath = Path.Combine(projectDirectory, _folderName);
        }
        #endregion

        #region CallMethods
        public async Task<TResult<FileGroup[]?>> LoadFormData()
        {
           
            FileGroup[]? fileGroup = null;
            StreamReader? sr = null;
            string errorOrigin = string.Empty;
            TResult<FileGroup[]?>? result = null;
            try
            {
                string pathWithName = Path.Combine(_filePath,
                 _fileName + _fileType);
                if (File.Exists(pathWithName))
                {
                    sr = new StreamReader(pathWithName);
                }
                else
                {
                    sr = new StreamReader(File.Create(pathWithName));
                    return TResult<FileGroup[]?>.Success(fileGroup);
                }

                var sb = new StringBuilder();
                string? line = sr.ReadLine();
                while (line != null)
                {
                    sb.AppendLine(line);
                    line = sr.ReadLine();
                }

                if (sb.Length > 0)
                    fileGroup = await Task.Run(() => JsonConvert.DeserializeObject<FileGroup[]>(sb.ToString(), _jsonSettings));

                ArgumentNullException.ThrowIfNull(fileGroup, _errorMessageJson);
                result = TResult<FileGroup[]?>.Success(fileGroup);
            }
            catch (Exception ex)
            {
                result = TResult<FileGroup[]?>.Failure(new string[] { ex.Message }, _loadDataOrigin, _errorType);     
            }
            finally
            {
                sr?.Dispose();
            }
            return result;
        }

       public async Task<TResult<FileGroup[]?>> WriteFormData(FileGroup[] fileGroupArray)
        {
            StreamWriter? streamWriter = null;
            TResult<FileGroup[]?>? result = null;
            try
            {           
                string pathWithName = Path.Combine(_filePath,
                    _fileName + _fileType);
                string jsonString = JsonConvert.SerializeObject(fileGroupArray, _jsonSettings);

                if (File.Exists(pathWithName))
                {
                   streamWriter = new(pathWithName);
                    await streamWriter.WriteAsync(jsonString);
                }
                else
                {
                    streamWriter = new(File.Create(pathWithName));
                    await streamWriter.WriteAsync(jsonString);
                   
                }

                result = TResult<FileGroup[]?>.Success(null);
            }
            catch (Exception ex)
            {
                result = TResult<FileGroup[]?>.Failure(new string[] { ex.Message }, _writeDataOrigin, _errorType);
            }
            finally
            {
                streamWriter?.Dispose();
            }
            return result;
        }
        #endregion
    }

}
