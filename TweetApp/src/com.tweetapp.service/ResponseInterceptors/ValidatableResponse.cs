namespace com.tweetapp.service
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    /// <summary>
    /// For returning the response when there are any failures.
    /// </summary>
    /// <typeparam name="TModel">generic model.</typeparam>
    public class ValidatableResponse<TModel> : ControllerBase
        where TModel : class
    {
        

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatableResponse{TModel}"/> class.
        /// ValidatableResponse for there any failures.
        /// </summary>
        /// <param name="message">message.</param>
        /// <param name="errorMessages">errorMessages.</param>
        /// <param name="data">generic data.</param>
        public ValidatableResponse(string message, IList<string> errorMessages = null, TModel data = null)
        {
            IList<string> _errorMessages;

            _errorMessages = errorMessages ?? new List<string>();

            if (_errorMessages.Count > 0)
            {
                this.ResponseBody = new ResponseBody<TModel>(message, errorMessages);
                this.StatusCode = 400;
                this.Message = "Please correct the following errors:  " + string.Join(",", _errorMessages);
            }
            else
            {
                this.ResponseBody = new ResponseBody<TModel>(message, data);
            }
        }

        public ValidatableResponse(string message, int errorCode)
        {         
                this.ErrorResponseBody = new ErrorResponseBody(message, errorCode);          
        }

        public ActionResult HttpResponseQueryResult
        {
            get
            {
                switch (this.StatusCode)
                {
                    case 201:
                        return this.Created("",new { message = this.ResponseBody.Message });
                    case 200:
                        return this.Ok(this.ResponseBody.Data);
                    case 400:
                        return this.BadRequest(this.ErrorResponseBody);
                    case 404:
                        return this.NotFound(this.ErrorResponseBody);
                    case 500:
                        var errorObjectResult = this.BadRequest(this.ErrorResponseBody);
                        errorObjectResult.StatusCode = 500;
                        return errorObjectResult;
                    default:
                        return this.BadRequest(this.ErrorResponseBody); 
                }
            }
        }

        /// <summary>
        /// HttpResponseCommandResult.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult HttpResponseCommandResult
        {
            get
            {
                switch (this.StatusCode)
                {
                    case 200: return this.Ok(new { message = this.ResponseBody.Message });
                    case 400: return this.BadRequest(this.ErrorResponseBody);
                    case 404: return this.NotFound(this.ErrorResponseBody);
                    case 500:
                        var errorObjectResult = this.BadRequest(this.ErrorResponseBody);
                        errorObjectResult.StatusCode = 500;
                        return errorObjectResult;
                    default: return this.BadRequest(this.ErrorResponseBody);
                }
            }
        }

        public new int StatusCode { get; set; }

        public string Message { get; set; }

        public ResponseBody<TModel> ResponseBody { get; }
        public ErrorResponseBody ErrorResponseBody { get; }
    }
}
