namespace com.tweetapp.service
{
    using System.Collections.Generic;

    public class ResponseBody<TData>
        where TData : class
    {
        public ResponseBody(string message, IList<string> errorMessages = null)
        {
            this.Message = message;
            this.Errors = errorMessages;
        }

        public ResponseBody(string message, TData data = null)
        {
            this.Message = message;
            this.Data = data;
        }

        public string Message { get; }

        public TData Data { get; }

        public IList<string> Errors { get; }
    }

    
    public class ErrorResponse
    {
        public string description { get; set; }
        public int responseCode { get; set; }
        public string responseName { get; set; }

    }
    
    public class ErrorResponseBody
    {
        public ErrorResponseBody(string message, int errorCode)
        {
            error = new ErrorResponse();
            switch (errorCode)
            {
                case 400:
                    error.responseName = "Bad Request";
                    break;
                case 404:
                     error.responseName = "Not Found";
                    break;
                case 409:
                    error.responseName = "Conflict";
                    break;
                case 500:
                    error.responseName = "Internal Server Error";
                    break;
            }     
            error.responseCode = errorCode;
            error.description = message;                
        }

        public ErrorResponse error { get; set; }
    }
}
