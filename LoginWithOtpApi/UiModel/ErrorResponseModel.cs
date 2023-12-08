namespace LoginWithOtpApi.UiModel
{
    public class ErrorResponseModel
    {
        public bool success { get; set; }
        public string ErrorMessage { get; set; }
        public bool isDevEnvironment { get; set; }
        public bool IncludeExcpetionAndStackTrace { get; set; }
        public string stackTrace { get; set; }
        public string ActualErrorMessage { get; set; }

        public ErrorResponseModel()
        {
            success = false;
            isDevEnvironment = false;
            IncludeExcpetionAndStackTrace = false;
        }
    }
}
