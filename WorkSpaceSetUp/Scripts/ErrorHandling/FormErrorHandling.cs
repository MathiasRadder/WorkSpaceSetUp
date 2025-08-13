using System;

namespace WorkSpaceSetUp.Scripts.ErrorHandling
{
    internal class FormErrorHandling
    {
        #region Variables
        private const string _defaultErrorMessage = "Default Error message, if this is shown then something went wrong in error handling.";
        private const string _extraInfoOnIgnore = "On ignore, it will overwrite the save file";
        #endregion

        #region Initialize
        #endregion

        #region CallMethods
        public void HandleError<T>(TResult<T> result)
        {
            if (result.IsSuccess)
                return;

            string errorMessage = _defaultErrorMessage;
            string placeErrorMessage = _defaultErrorMessage;
            if (result.Errors != null)
                errorMessage = string.Join("\n ", result.Errors);
            if (!string.IsNullOrEmpty(result.ErrorOrigin))
                placeErrorMessage = result.ErrorOrigin;

            KeyValuePair<ErrorTypes, DialogResult> errorDialogResult = CreateMessageBox(errorMessage, placeErrorMessage, result.ErrorType);
            HandleDialogResult(errorDialogResult);
        }
        #endregion

        #region PrivateMethods
        private void HandleDialogResult(KeyValuePair<ErrorTypes, DialogResult> errorDialogResult)
        {
            switch (errorDialogResult.Key)
            {
          
                case ErrorTypes.Error:
                    if (errorDialogResult.Value == DialogResult.Abort)
                    {
                        Application.Exit();
                    }
                    else if(errorDialogResult.Value == DialogResult.Retry)
                    {
                        Application.Restart();
                        Environment.Exit(0);
                    }
                        break;
                case ErrorTypes.Fatal:
                    Application.Exit();
                    break;
                case ErrorTypes.None:
                case ErrorTypes.Warning:
                default:
                    break;
            }          
        }

        private KeyValuePair<ErrorTypes, DialogResult> CreateMessageBox(string errorMessage, string placeOfError, ErrorTypes errorType)
        {
            DialogResult dialogResult = new();
            switch (errorType)
            {
                case ErrorTypes.Warning:
                    dialogResult = MessageBox.Show(errorMessage, placeOfError, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case ErrorTypes.Error:
                    dialogResult = MessageBox.Show(errorMessage + " " + _extraInfoOnIgnore, placeOfError, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                    break;
                case ErrorTypes.Fatal:
                    dialogResult = MessageBox.Show(errorMessage, placeOfError, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    break;
                case ErrorTypes.None:
                default:
                    break;  
            }
            return new KeyValuePair<ErrorTypes, DialogResult>(errorType, dialogResult); ;
        }
        #endregion
    }
}
