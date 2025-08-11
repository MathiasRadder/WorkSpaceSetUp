using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//I got this from https://stackoverflow.com/questions/77961808/how-should-i-return-the-result-from-a-service-use-case-class


namespace WorkSpaceSetUp.Scripts
{
    public enum ErrorTypes
    {
        None,
        Warning,
        Error,
        Fatal
    }
    public class TResult<T>
    {


        #region Variables
        public bool IsSuccess { get; private set; }
        public bool IsFailure { get; private set; }
        public T? Value { get; private set; }
        public string[]? Errors { get; private set; }
        public string? ErrorOrigin { get; private set; }

        public ErrorTypes ErrorType { get; private set; }

        #endregion

        #region Initialize
        private TResult() { }
        public static TResult<T> Success(T value) => new() { IsSuccess = true, IsFailure = false, Value = value };
        public static TResult<T> Failure(string[] errors, string? placeError, ErrorTypes errorType) => new() { IsSuccess = false, IsFailure = true, Errors = errors, ErrorOrigin = placeError, ErrorType = errorType };
        #endregion
    }

}
