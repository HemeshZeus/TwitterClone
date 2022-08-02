namespace com.tweetapp.webapi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Authentication endpoints
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    public class UtilityController : ControllerBase
    {
        /// <summary>
        /// Health Test for All users
        /// </summary>
        [HttpGet("/api/v{version:apiVersion}/tweets/health-test")]
        [MapToApiVersion("1.0")]
        public ActionResult HealthTestv1()
        {
            return Ok("Sucess - msg from version 1.0");
        }

        /// <summary>
        /// Health Test for authenticated users
        /// </summary>
        [Authorize]
        [HttpGet("/api/v{version:apiVersion}/tweets/health-test")]
        [MapToApiVersion("1.1")]
        public ActionResult HealthTestv1p1()
        {
            return Ok("Sucess - msg from version 1.1");
        }
    }
}
